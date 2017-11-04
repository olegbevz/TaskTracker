using System;
using System.Collections.Generic;

namespace TaskTracker
{
    public interface ISubscriptionDictionary
    {
        void AddOrUpdate(Guid connection, SubscriptionEntry subscription);
        void Remove(Guid connection);
        IEnumerable<SubscriptionEntry> GetAll();

        event EventHandler SubscriptionAdded;
        event EventHandler SubscriptionUpdated;
    }
}
