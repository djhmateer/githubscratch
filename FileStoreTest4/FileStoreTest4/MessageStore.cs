using System;
using System.IO;
using System.Linq;
using Serilog;
using Serilog.Events;

namespace Mateer.Samples.Encapsulation.CodeExamples
{
    // A role interface
    public interface IStoreWriter
    {
        void Save(int id, string message);
    }

    // 2 intermediary steps below
    public class LogSavingStoreWriter : IStoreWriter
    {
        public void Save(int id, string message)
        {
            Log.Information("Saving message {id}.", id);
        }
    }

    public class LogSavedStoreWriter : IStoreWriter
    {
        public void Save(int id, string message)
        {
            Log.Information("Saved message {id}.", id);
        }
    }

    public class MessageStore
    {
        public DirectoryInfo WorkingDirectory { get; private set; }
        private StoreLogger log;
        private StoreCache cache;
        private IStore store;
        private IFileLocator fileLocator;
        private IStoreWriter writer;

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
            var c = new StoreCache();
            this.cache = c;
            var fileStore = new FileStore(workingDirectory);
            this.store = fileStore;
            this.fileLocator = new FileLocator();
            // Deterministic order
            this.writer = new CompositeStoreWriter(
                new LogSavingStoreWriter(),
                fileStore,
                c,
                new LogSavedStoreWriter());
        }

        public class CompositeStoreWriter : IStoreWriter
        {
            private IStoreWriter[] writers;

            public CompositeStoreWriter(params IStoreWriter[] writers)
            {
                this.writers = writers;
            }

            public void Save(int id, string message)
            {
                foreach (var w in this.writers)
                    w.Save(id, message);
            }
        }

        // A Command (returns void)
        public void Save(int id, string message)
        {
            // 4 Commands that take id as an argument
            //this.log.Saving(id, message);
            //this.store.Save(id, message);
            //this.cache.Save(id, message);
            //this.log.Saved(id, message);

            // A factory property that returns an instance of IStoreWriter
            // Calls the compositeWriters save
            this.writer.Save(id, message);
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
            return this.fileLocator.GetFileInfo(id);
        }
    }

    public class FileLocator : IFileLocator
    {
        public FileInfo GetFileInfo(int id)
        {
            throw new NotImplementedException();
        }
    }

    public interface IFileLocator
    {
        FileInfo GetFileInfo(int id);
    }
}

