using System;
using System.IO;

namespace Mateer.Samples.Encapsulation.CodeExamples
{
    public interface IStore
    {
        void Save(int id, string message);
        Maybe<string> ReadAllText(int id);
    }

    public class FileStore : IStore, IStoreWriter
    {
        private DirectoryInfo workingDirectory;

        public FileStore(DirectoryInfo workingDirectory)
        {
            if (workingDirectory == null)
                throw new ArgumentNullException("workingDirectory");
            if (!workingDirectory.Exists)
                throw new ArgumentException("Boo", "workingDirectory");

            this.workingDirectory = workingDirectory;
        }

        public virtual void Save(int id, string message)
        {
            var path = this.GetFileInfo(id).FullName;
            File.WriteAllText(path, message);
        }


        public virtual Maybe<string> ReadAllText(int id)
        {
            var file = this.GetFileInfo(id);
            if (!file.Exists)
                return new Maybe<string>();
            var path = file.FullName;
            return new Maybe<string>(File.ReadAllText(path));
        }

        public virtual FileInfo GetFileInfo(int id)
        {
            return new FileInfo(
                Path.Combine(this.workingDirectory.FullName, id + ".txt"));
        }
    }

    // SqlStore derives from FileStore.. strange 
}
