using System;
using System.Collections.Concurrent;
using System.Linq;

namespace Mateer.Samples.Encapsulation.CodeExamples
{
    public interface IStoreCache
    {
        void Save(int id, string message);
        Maybe<string> Read(int id);
    }

    public interface IStoreReader
    {
        Maybe<string> Read(int id);
    }

    // IStore Writer is the role interface that defines the Save method
    public class StoreCache : IStoreCache, IStoreWriter, IStoreReader
    {
        private ConcurrentDictionary<int, Maybe<string>> cache;
        private readonly IStoreWriter writer;
        private readonly IStoreReader reader;

        public StoreCache(IStoreWriter writer, IStoreReader reader)
        {
            this.cache = new ConcurrentDictionary<int, Maybe<string>>();
            this.writer = writer;
            this.reader = reader;
        }

        public void Save(int id, string message)
        {
            // If you squint it could be base.Save(id, message)
            // favouring composition over inheritance
            this.writer.Save(id, message);
            var m = new Maybe<string>(message);
            // add key, value or update with int, string message
            this.cache.AddOrUpdate(id, m, (i, s) => m);
        }

        public Maybe<string> Read(int id){
            // If we find the value in the cache, return the value
            Maybe<string> retVal;
            if (this.cache.TryGetValue(id, out retVal))
                return retVal;

            // If not, look at underlying reader
            // a read through cache pattern
            retVal = this.reader.Read(id);
            if (retVal.Any())
                this.cache.AddOrUpdate(id, retVal, (i, s) => retVal);
            return retVal;
        }
    }
}
