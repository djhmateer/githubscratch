using System.IO;

namespace Mateer.Samples.Encapsulation.CodeExamples{
    public class SqlStore 
    {

        public void Save(int id, string message){
            // Write to db
        }

        public Maybe<string> Read(int id){
            // read
            return new Maybe<string>();
        }

        //public FileInfo GetFileInfo(int id)
        //{
        //    throw new System.NotSupportedException();
        //}
    }
}