using System;
using System.IO;

namespace zClassLibrary2
{
    // An object is data with behaviour
    public class FileStore : IMessageQuery
    {
        // The data of the object
        private DirectoryInfo workingDirectory;

        public FileStore(DirectoryInfo workingDirectory)
        {
            this.workingDirectory = workingDirectory;
        }

        // The behaviour of the object
        public string Read(int id)
        {
            var path = Path.Combine(
                this.workingDirectory.FullName,
                id + ".txt");
            return File.ReadAllText(path);
        }

        // Functions are pure behaviour... no data captured
        // same thing as the object does
        // takes workingDirectory and id (DirectoryInfo and int) as input
        // returns string as output
        Func<DirectoryInfo, int, string> read = (workingDirectory, id) =>
        {
            var path = Path.Combine(
                workingDirectory.FullName,
                id + ".txt");
            return File.ReadAllText(path);
        };



    }

    public class Thing
    {
        public void Something()
        {
// An outer variable
var workingDirectory = new DirectoryInfo(Environment.CurrentDirectory);

// This function 'closes over' or captures the workingDirectory
Func<int, string> read = id =>
{
    var path = Path.Combine(
        workingDirectory.FullName,
        id + ".txt");
    return File.ReadAllText(path);
};
        }
    }

    public interface IMessageQuery
    {
        string Read(int id);
    }
}
