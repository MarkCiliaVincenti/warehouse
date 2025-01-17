﻿namespace Vayosoft.Caching.Redis
{
    public class RedisCachingMessage
    {
        public RedisCachingMessage()
        {
            Id = Guid.NewGuid().ToString("N");
            CreationDate = DateTime.UtcNow;
        }

        public DateTime? CreationDate { get; set; }

        public string InstanceId { get; set; }
        public string Id { get; set; }

        public object[] CacheKeys { get; set; }
        public bool IsPrefix { get; set; }
        public bool IsToken { get; set; }

        public override string ToString()
        {
            return $"{InstanceId}:{(IsToken ? "token" : "key")}:{string.Join(", ", CacheKeys?.Select(x => x) ?? new object[] { })}";
        }
    }
}
