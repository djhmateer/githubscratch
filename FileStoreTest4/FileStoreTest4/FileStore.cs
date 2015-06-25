using System.IO;

namespace Mateer.Samples.Encapsulation.CodeExamples
{
    public class FileStore
    {
        public virtual void WriteAllText(string path, string message){
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
}
