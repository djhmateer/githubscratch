using System;
using System.IO;
using System.Linq;

namespace Mateer.Samples.Encapsulation.CodeExamples
{
    // A role interface
    public interface IStoreWriter
    {
        void Save(int id, string message);
    }

    public class MessageStore
    {
        public DirectoryInfo WorkingDirectory { get; private set; }
        private StoreLogger log;
        private StoreCache cache;
        private IStore store;
        private IFileLocator fileLocator;
        private IStoreWriter writer;
        private IStoreReader reader;

        public MessageStore(DirectoryInfo workingDirectory)
        {
            if (workingDirectory == null)
                throw new ArgumentNullException("There must be a working directory passed to save to");
            if (!Directory.Exists(workingDirectory.FullName))
                throw new ArgumentException("The workingDirectory must exist", "workingDirectory");

            this.WorkingDirectory = workingDirectory;
            var fileStore = new FileStore(workingDirectory);
            // Applied decorator pattern to the StoreCache
            // so when Save is called on the StoreCache
            // it actually calls Save on the fileStore.. the 'base'
            // before, as they both implement IStoreWriter

            // same instance plays the role of writer and reader
            var c = new StoreCache(fileStore, fileStore);
            this.cache = c;
            var l = new StoreLogger(c, c);
            this.log = l;

            this.store = fileStore;
            this.fileLocator = fileStore;
            this.writer = l;
            this.reader = l;
        }

        // A Command (returns void)
        public void Save(int id, string message)
        {
            // Calls the 'Russian doll' stack of Saves depending on what has been composed above
            // log first, then cache, then FileStore
            this.writer.Save(id, message);
        }

        public Maybe<string> Read(int id)
        {
            return this.reader.Read(id);
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

