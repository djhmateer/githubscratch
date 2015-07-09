using System;
using System.IO;

namespace Mateer.Samples.Encapsulation.CodeExamples
{
    //public interface IStore
    //{
    //    void Save(int id, string message);
    //    Maybe<string> Read(int id);
    //}

    public class FileStore : IFileLocator, IStoreWriter, IStoreReader
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

        public void Save(int id, string message)
        {
            var path = this.GetFileInfo(id).FullName;
            File.WriteAllText(path, message);
        }


        public Maybe<string> Read(int id)
        {
            var file = this.GetFileInfo(id);
            if (!file.Exists)
                return new Maybe<string>();
            var path = file.FullName;
            return new Maybe<string>(File.ReadAllText(path));
        }

        public FileInfo GetFileInfo(int id)
        {
            return new FileInfo(
                Path.Combine(this.workingDirectory.FullName, id + ".txt"));
        }
    }
}
