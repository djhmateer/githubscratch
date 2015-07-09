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
        // role interfaces, and class fields
        private IFileLocator fileLocator;
        private IStoreWriter writer;
        private IStoreReader reader;

        public MessageStore(IStoreWriter writer, IStoreReader reader, IFileLocator fileLocator)
        {
            // protecting invariance with guard clauses
            if (writer == null)
                throw new ArgumentNullException("fileLocator");
            if (reader == null)
                throw new ArgumentNullException("fileLocator");
            if (fileLocator == null)
                throw new ArgumentNullException("fileLocator");

            this.fileLocator = fileLocator;
            this.writer = writer;
            this.reader = reader;
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

