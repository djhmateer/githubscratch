using System;
using System.IO;
using System.Linq;
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
            this.store = new FileStore(workingDirectory);
        }

        // A Command (returns void)
        public void Save(int id, string message)
        {
            this.log.Saving(id);
            this.store.WriteAllText(id, message);
            this.cache.AddOrUpdate(id, message);
            this.log.Saved(id);
        }

        public Maybe<string> Read(int id)
        {
            this.log.Reading(id);
            // this.store.ReadAllText is the delegate
            Maybe<string> message = this.cache.GetOrAdd(
                id, arg => this.store.ReadAllText(id));
            if (message.Any())
                this.log.Returning(id);
            else
                this.log.DidNotFind(id);
            return message;
        }

        public FileInfo GetFileInfo(int id)
        {
            return this.store.GetFileInfo(id);
        }
    }
}

