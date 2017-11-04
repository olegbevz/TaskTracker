using System;

namespace TaskTracker
{
    public class SubscriptionEntry
    {
        public SubscriptionEntry(Subscription subscription, IClientProxy clientProxy)
        {
            Subscription = subscription;
            ClientProxy = clientProxy;
        }
        
        public Subscription Subscription { get; set; }
        public IClientProxy ClientProxy { get; set; }
        public DateTime? LastUpdateTime { get; set; }
        public bool PendingUpdate { get; set; }
    }

}
