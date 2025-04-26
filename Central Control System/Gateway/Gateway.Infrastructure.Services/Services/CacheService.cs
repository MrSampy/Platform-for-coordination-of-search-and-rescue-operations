using Gateway.Domain.Services.Interfaces;
using System.Collections.Concurrent;

namespace Gateway.Infrastructure.Services.Services
{
    public sealed class CacheService<T> : ICacheService<T> where T : class
    {
        private readonly ConcurrentDictionary<string, List<T>> _cachedEntities = new();

        public List<T>? Get(string key) =>
            _cachedEntities.TryGetValue(key, out var result) ? result : null;

        public void Set(string key, List<T> entities) =>
            _cachedEntities[key] = entities;

        public void Remove(string key) =>
            _cachedEntities.TryRemove(key, out _);

        public void Reset() => _cachedEntities.Clear();
    }
}
