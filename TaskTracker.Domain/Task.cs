using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.ComponentModel.DataAnnotations;

namespace TaskTracker.Domain
{
    public class Task
    {
        public static Task Create(string name, string description, int priority, TimeSpan timeToComplete, DateTime added)
        {
            return new Task
            {
                Name = name,
                Description = description,
                Priority = priority,
                Added = added,
                Edited = added,
                Duration = timeToComplete,
                Status = TaskStatus.Active
            };
        }

        public Task()
        {
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
        
        public int Priority { get; set; }

        [JsonConverter(typeof(DateFormatConverter), "yyyy-MM-dd hh:mm:ss")]
        public DateTime Added { get; set; }

        [ConcurrencyCheck]
        [JsonConverter(typeof(DateFormatConverter), "yyyy-MM-dd hh:mm:ss")]
        public DateTime Edited { get; set; }
        
        public TimeSpan Duration { get; set; }

        [JsonConverter(typeof(StringEnumConverter), true)]
        public TaskStatus Status { get; set; }
    }
}
