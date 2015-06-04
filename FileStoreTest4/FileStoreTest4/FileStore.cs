using System;
using System.IO;

namespace FileStoreTest4
{
    public class FileStore
    {
        // Strong indication that FileStore cannot work without a workingDirectory
        // this is a pre-condition of the class
        public FileStore(string workingDirectory)
        {
            // Fail fast.. so can't have an invalid state of no working directory
            if (workingDirectory == null)
                throw new ArgumentNullException("There must be a working directory passed to save to");
            if (!Directory.Exists(workingDirectory))
                throw new ArgumentException("The workingDirectory must exist", "workingDirectory");

            this.WorkingDirectory = workingDirectory;
        }

        public string WorkingDirectory { get; private set; }

        // A Command (returns void)
        public void Save(int id, string message)
        {
            var path = this.GetFileName(id);
            File.WriteAllText(path, message);
        }

        // A Query.. Never return null.  Agree with team that null is not a valid value to return
        // Maybe<T> is good for dealing with a value that may not present
        public Maybe<string> Read(int id)
        {
            var path = this.GetFileName(id);
            if (!File.Exists(path))
                return new Maybe<string>();

            var message = File.ReadAllText(path);
            // Never want message to be Null - that is what the previous step is for
            return new Maybe<string>(message);
        }

        // A Query (so this shouldn't have any side effects)
        public string GetFileName(int id)
        {
            // This can never be null as int is a value type, and workingDirectory is a pre-condition
            return Path.Combine(this.WorkingDirectory, id + ".txt");
        }
    }
}
