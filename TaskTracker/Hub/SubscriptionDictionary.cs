using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Concurrent;

namespace TaskTracker
{
    public class SubscriptionDictionary : ISubscriptionDictionary
    {
        private readonly ConcurrentDictionary<Guid, SubscriptionEntry> dictionary
            = new ConcurrentDictionary<Guid, SubscriptionEntry>();
        
        public event EventHandler SubscriptionAdded;
        public event EventHandler SubscriptionUpdated;
        
        public void AddOrUpdate(Guid id, SubscriptionEntry subscription)
        {
            var connectionSubscriptions =
                dictionary.AddOrUpdate(id, 
                connection =>
                {
                    var handler = SubscriptionAdded;
                    if (handler != null)
                        handler.Invoke(subscription, EventArgs.Empty);

                    return subscription;
                },
                (connection, oldSubscription) =>
                {
                    oldSubscription.Subscription = subscription.Subscription;
                    oldSubscription.PendingUpdate = subscription.PendingUpdate;

                    var handler = SubscriptionUpdated;
                    if (handler != null)
                        handler.Invoke(oldSubscription, EventArgs.Empty);

                    return oldSubscription;
                });
        }

        public void Remove(Guid connection)
        {
            SubscriptionEntry subscription;
            dictionary.TryRemove(connection, out subscription);
        }

        public IEnumerable<SubscriptionEntry> GetAll()
        {
            return dictionary.Values.ToArray();
        }
    }

}
