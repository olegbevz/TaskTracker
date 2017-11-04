using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskTracker.DataAccess.Interfaces;
using System.Threading;
using Microsoft.Extensions.Logging;
using DomainTask = TaskTracker.Domain.Task;
using Microsoft.Extensions.DependencyInjection;

namespace TaskTracker
{
    public class SubscriptionScheduler
    {
        private readonly ISubscriptionDictionary subscriptionDictionary;
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly ILogger logger;

        private bool isRunning;
        private readonly Thread backgroundThread;
        private readonly AutoResetEvent stoppedEvent = new AutoResetEvent(false);
        private readonly AutoResetEvent subscribeEvent = new AutoResetEvent(false);
        private readonly TimeSpan sleepInterval = TimeSpan.FromMinutes(15);
        private readonly TimeSpan loopInterval = TimeSpan.FromMilliseconds(100);
        private readonly TimeSpan updateInterval = TimeSpan.FromSeconds(30);

        public SubscriptionScheduler(
            ISubscriptionDictionary subscriptionEntryPool,
            ILogger<SubscriptionScheduler> logger,
            IServiceScopeFactory serviceScopeFactory)
        {
            this.subscriptionDictionary = subscriptionEntryPool;
            this.subscriptionDictionary.SubscriptionAdded += OnSubscriptionAdded;
            this.subscriptionDictionary.SubscriptionUpdated += OnSubscriptionUpdated;
            this.logger = logger;
            this.serviceScopeFactory = serviceScopeFactory;

            this.backgroundThread = new Thread(BackgroundThread) { IsBackground = true };
        }

        ~SubscriptionScheduler()
        {
            if (this.backgroundThread != null)
                backgroundThread.Abort();
        }       

        public void Start()
        {
            isRunning = true;
            backgroundThread.Start();
        }

        public void Stop()
        {
            stoppedEvent.Reset();
            isRunning = false;
            stoppedEvent.WaitOne();
        }

        protected void RaiseStoppedEvent()
        {
            stoppedEvent.Set();
        }

        public void Dispose()
        {
            Stop();
            GC.SuppressFinalize(this);
        }

        protected void BackgroundThread()
        {
            try
            {
                while (isRunning)
                {
                    var subscriptions = subscriptionDictionary.GetAll();
                    if (!subscriptions.Any())
                        WaitForSubscriptions(sleepInterval);

                    foreach (var subscription in subscriptions)
                    {
                        var currentTime = DateTime.UtcNow;

                        if (UpdateSubscription(subscription, currentTime))
                        {
                            subscription.PendingUpdate = false;

                            var task = ExecuteSubscription(subscription.Subscription);

                            task.ContinueWith(
                                x => OnTaskCompleted(subscription.ClientProxy, x.Result),
                                TaskContinuationOptions.OnlyOnRanToCompletion);

                            task.ContinueWith(
                                x => OnTaskFaulted(x.Exception),
                                TaskContinuationOptions.OnlyOnFaulted);

                            subscription.LastUpdateTime = currentTime;
                        }
                    }

                    WaitForSubscriptions(loopInterval);
                }

                RaiseStoppedEvent();
            }
            catch (Exception ex)
            {
                isRunning = false;
                logger.LogError(ex, ex.Message);
            }
        }

        private bool UpdateSubscription(SubscriptionEntry subscription, DateTime currentTime)
        {
            if (subscription.PendingUpdate)
                return true;

            if (subscription.LastUpdateTime == null)
                return true;

            if (currentTime - subscription.LastUpdateTime > updateInterval)
                return true;

            return false;
        }

        private void OnTaskFaulted(Exception ex)
        {
            logger.LogError(ex, ex.Message);
        }

        private void OnTaskCompleted(IClientProxy clientProxy, IEnumerable<DomainTask> tasks)
        {
            clientProxy.Push(tasks);
        }

        private void WaitForSubscriptions(TimeSpan timeout)
        {
            subscribeEvent.Reset();
            subscribeEvent.WaitOne(timeout);
        }

        private async Task<IEnumerable<DomainTask>> ExecuteSubscription(Subscription subscription)
        {
            using (var scope = this.serviceScopeFactory.CreateScope())
            {
                var taskRepository = scope.ServiceProvider.GetService<ITaskRepository>();

                return await taskRepository.GetTasks(
                    subscription.TaskStatus,
                    subscription.SortOrder,
                    subscription.Skip,
                    subscription.Take);
            }
        }

        private void OnSubscriptionAdded(object sender, EventArgs e)
        {
            subscribeEvent.Set();
        }

        private void OnSubscriptionUpdated(object sender, EventArgs e)
        {
            subscribeEvent.Set();
        }
    }
}
