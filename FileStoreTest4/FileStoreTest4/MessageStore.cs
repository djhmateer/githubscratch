using System;
using System.IO;
using Serilog;
using Serilog.Events;

namespace Mateer.Samples.Encapsulation.CodeExamples
{
    public class MessageStore
    {
        public DirectoryInfo WorkingDirectory { get; private set; }
        private StoreLogger log;
        private StoreCache cache;
        private FileStore fileStore;

        // Strong indication that MessageStore cannot work without a workingDirectory
        // this is a pre-condition of the class
        public MessageStore(DirectoryInfo workingDirectory)
        {
            // Fail fast.. so can't have an invalid state of no working directory
            if (workingDirectory == null)
                throw new ArgumentNullException("There must be a working directory passed to save to");
            if (!Directory.Exists(workingDirectory.FullName))
                throw new ArgumentException("The workingDirectory must exist", "workingDirectory");

            this.WorkingDirectory = workingDirectory;
            this.log = new StoreLogger();
            this.cache = new StoreCache();
            this.fileStore = new FileStore();
        }

        // A Command (returns void)
        public void Save(int id, string message)
        {
            this.Log.Saving(id);
            var file = this.GetFileInfo(id);
            this.Store.WriteAllText(file.FullName, message);
            this.Cache.AddOrUpdate(id, message);
            this.Log.Saved(id);
        }

        // A Query.. Never return null.  Agree with team that null is not a valid value to return
        // Maybe<T> is good for dealing with a value that may not present
        public Maybe<string> Read(int id)
        {
            this.Log.Reading(id);
            var file = this.GetFileInfo(id);
            if (!file.Exists)
                return new Maybe<string>();

            // Gets message from the cache, or if not there, gets then adds it
            var message = this.Cache.GetOrAdd(
                id, arg => this.Store.ReadAllText(file.FullName));


            // Never want message to be Null - that is what the previous step is for
            this.Log.Returning(id);
            return new Maybe<string>(message);
        }

        // A Query (so this shouldn't have any side effects)
        public FileInfo GetFileInfo(int id)
        {
            // This can never be null as int is a value type, and workingDirectory is a pre-condition
            // talking to the virtual Property now (which can be overridden)
            return this.Store.GetFileInfo(
                id, this.WorkingDirectory.FullName);
        }

        // Factory readable properties (compiles to a method)
        // virtual means can be overridden
        protected virtual FileStore Store
        {
            get { return this.fileStore; }
        }

        protected virtual StoreCache Cache
        {
            get { return this.cache; }
        }

        protected virtual StoreLogger Log
        {
            get { return this.log; }
        }
    }
}

