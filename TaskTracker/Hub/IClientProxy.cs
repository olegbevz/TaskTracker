using System.Collections.Generic;
using TaskTracker.Domain;

namespace TaskTracker
{
    public interface IClientProxy
    {
        void Push(IEnumerable<Task> tasks);
    }

}
