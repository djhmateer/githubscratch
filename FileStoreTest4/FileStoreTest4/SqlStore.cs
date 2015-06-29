using System.IO;

namespace Mateer.Samples.Encapsulation.CodeExamples{
    public class SqlStore : IStore
    {

        public void Save(int id, string message){
            // Write to db
        }

        public Maybe<string> ReadAllText(int id){
            // read
            return new Maybe<string>();
        }

        //public FileInfo GetFileInfo(int id)
        //{
        //    throw new System.NotSupportedException();
        //}
    }
}