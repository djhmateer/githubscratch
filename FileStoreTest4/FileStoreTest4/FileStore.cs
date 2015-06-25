using System.IO;

namespace Mateer.Samples.Encapsulation.CodeExamples
{
    public interface IStore{
        void WriteAllText(string path, string message);
        string ReadAllText(string path);
        FileInfo GetFileInfo(int id, string workingDirectory);
    }

    public class FileStore : IStore{
        public virtual void WriteAllText(string path, string message)
        {
            File.WriteAllText(path, message);
        }

        public virtual string ReadAllText(string path)
        {
            return File.ReadAllText(path);
        }

        public virtual FileInfo GetFileInfo(int id, string workingDirectory)
        {
            return new FileInfo(
                Path.Combine(workingDirectory, id + ".txt"));
        }
    }

    // SqlStore derives from FileStore.. strange 
    public class SqlStore : IStore
    {
        public void WriteAllText(string path, string message){
            // Write to db
        }

        public string ReadAllText(string path){
            // read
            return null;
        }

        public FileInfo GetFileInfo(int id, string workingDirectory){
            throw new System.NotSupportedException();
        }
    }
}
