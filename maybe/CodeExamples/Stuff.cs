using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Ploeh.Samples.Encapsulation.CodeExamples
{
    class Stuff
    {
        public class FileStore
        {
            public void Tester()
            {
                // this method may, or may not return a string
                Maybe<string> thing = Read(49);
                var message = thing.DefaultIfEmpty("").Single();
            }

            public FileStore(string workingDirectory)
            {
                // Guard clause as string is a ref type, so could be set to null outside
                if (workingDirectory == null)
                    throw new ArgumentNullException("workingDirectory");
                // Exception messages are like documentation
                if (!Directory.Exists(workingDirectory))
                    throw new ArgumentException("You tried to supply a working directory string which doesn't exist." +
                                                "  Please supply a valid path to an existing directory", "workingDirectory");

                this.WorkingDirectory = workingDirectory;
            }

            // Private set
            public string WorkingDirectory { get; private set; }

            // 1.Tester Doer
            //public bool Exists(int id)
            //{
            //    var path = GetFileName(id);
            //    return File.Exists(path);
            //}

            //// Could return a null/empty string if 1) no message  2) exception is thrown
            //public string Read(int id)
            //{
            //    var path = this.GetFileName(id);
            //    if (!File.Exists(path))
            //        throw new ArgumentException("bad path", "read");
            //    var message = File.ReadAllText(path);
            //    return message;
            //}

            // 2.TryRead (TryParse)
            //public bool TryRead(int id, out string message)
            //{
            //    message = null;
            //    var path = this.GetFileName(id);
            //    if (!File.Exists(path))
            //        return false;
            //    message = File.ReadAllText(path);
            //    return true;
            //}

            // Return value may be there, and may not be
            // and don't want to return null as it is tainted
            // want a collection of 0 or 1 strings
            // IEnumerable<string> is too wide as it could have 10 elements
            // Maybe<T> can return 0 of 1 <T>
            public Maybe<string> Read(int id)
            {
                var path = this.GetFileName(id);
                if (!File.Exists(path))
                    return new Maybe<string>();
                var message = File.ReadAllText(path);
                return new Maybe<string>(message);
            }

            // Will never return a null string
            public string GetFileName(int id)
            {
                // This can never be null as int is a value type, and workingDirectory is a pre condition
                return Path.Combine(this.WorkingDirectory, id + ".txt");
            }
        }
    }
}
