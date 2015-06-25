﻿using System;
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
        private IStore store;

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
            this.store = new FileStore();
        }

        // A Command (returns void)
        public void Save(int id, string message)
        {
            this.log.Saving(id);
            var file = this.GetFileInfo(id);
            this.store.WriteAllText(file.FullName, message);
            this.cache.AddOrUpdate(id, message);
            this.log.Saved(id);
        }

        // A Query.. Never return null.  Agree with team that null is not a valid value to return
        // Maybe<T> is good for dealing with a value that may not present
        public Maybe<string> Read(int id)
        {
            this.log.Reading(id);
            var file = this.GetFileInfo(id);
            if (!file.Exists)
                return new Maybe<string>();

            // Gets message from the cache, or if not there, gets then adds it
            var message = this.cache.GetOrAdd(
                id, arg => this.store.ReadAllText(file.FullName));


            // Never want message to be Null - that is what the previous step is for
            this.log.Returning(id);
            return new Maybe<string>(message);
        }

        // A Query (so this shouldn't have any side effects)
        public FileInfo GetFileInfo(int id)
        {
            // This can never be null as int is a value type, and workingDirectory is a pre-condition
            // talking to the virtual Property now (which can be overridden)
            return this.store.GetFileInfo(
                id, this.WorkingDirectory.FullName);
        }

    }
}
