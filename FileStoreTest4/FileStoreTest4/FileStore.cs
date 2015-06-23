using System.IO;

namespace Mateer.Samples.Encapsulation.CodeExamples
{
    //public class FileStore : IStoreWriter, IStoreReader, IFileLocator
    public class FileStore
    {
        public void WriteAllText(string path, string message){
            File.WriteAllText(path, message);
        }

        public string ReadAllText(string path){
            return File.ReadAllText(path);
        }

        public FileInfo GetFileInfo(int id, string workingDirectory){
            return new FileInfo(
                Path.Combine(workingDirectory, id + ".txt"));
        }
    }
}
