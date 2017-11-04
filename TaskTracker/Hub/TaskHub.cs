using System;
using Microsoft.AspNetCore.SignalR;

namespace TaskTracker
{
    public class TaskHub : Hub<IClientProxy>
    {
        private readonly ISubscriptionDictionary subscriptions;

        public TaskHub(ISubscriptionDictionary subscriptions)
        {
            this.subscriptions = subscriptions;
        }

        public void Subscribe(Subscription subscription)
        {
            var connection = Context.ConnectionId;
            var subscriptionEntry = new SubscriptionEntry(subscription, Clients.Client(connection));
            subscriptionEntry.PendingUpdate = true;
            subscriptions.AddOrUpdate(Guid.Parse(connection), subscriptionEntry);
        }

        public void Unsubscribe()
        {
            subscriptions.Remove(Guid.Parse(Context.ConnectionId));
        }
    }
}
