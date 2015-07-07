using System;
using System.Collections.Concurrent;
using System.Linq;

namespace Mateer.Samples.Encapsulation.CodeExamples
{
    public interface IStoreCache{
        void Save(int id, string message);
        Maybe<string> GetOrAdd(int id, Func<int, Maybe<string>> messageFactory);
    }

    public class StoreCache : IStoreCache, IStoreWriter{
        private ConcurrentDictionary<int, Maybe<string>> cache;

        public StoreCache()
        {
            this.cache = new ConcurrentDictionary<int, Maybe<string>>();
        }

        public void Save(int id, string message)
        {
            var m = new Maybe<string>(message);
            // add key, value or update with int, string message
            this.cache.AddOrUpdate(id, m, (i, s) => m);
        }

        // A Function that takes an int and returns a string
        // the signature of the ConcurrentDictioary GetOrAdd method
        public Maybe<string> GetOrAdd(int id, Func<int, Maybe<string>> messageFactory)
        {
            return this.cache.GetOrAdd(id,messageFactory);
        }
    }
}
