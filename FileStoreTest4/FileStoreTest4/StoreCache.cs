using System;
using System.Collections.Concurrent;

namespace Mateer.Samples.Encapsulation.CodeExamples
{
    public class StoreCache
    {
        private ConcurrentDictionary<int, string> cache;

        public StoreCache()
        {
            this.cache = new ConcurrentDictionary<int, string>();
        }

        public void AddOrUpdate(int id, string message)
        {
            // add key, value or update with int, string message
            this.cache.AddOrUpdate(id, message, (i, s) => message);
        }

        // A Function that takes an int and returns a string
        // the signature of the ConcurrentDictioary GetOrAdd method
        public string GetOrAdd(int id, Func<int, string> messageFactory)
        {
            return this.cache.GetOrAdd(id, messageFactory);
        }
    }
}
