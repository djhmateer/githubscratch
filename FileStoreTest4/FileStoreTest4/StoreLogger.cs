using System.Linq;
using Serilog;

namespace Mateer.Samples.Encapsulation.CodeExamples
{
    public interface IStoreLogger
    {
        void Saving(int id, string message);
        void Saved(int id, string message);
        void Reading(int id);
        void DidNotFind(int id);
        void Returning(int id);
    }

    public class StoreLogger : IStoreLogger, IStoreWriter, IStoreReader
    {
        private readonly IStoreWriter writer;
        private readonly IStoreReader reader;

        public StoreLogger(IStoreWriter writer, IStoreReader reader){
            this.writer = writer;
            this.reader = reader;
        }

        public Maybe<string> Read(int id){
            Log.Information("Reading message {id}.", id);
            var retVal = this.reader.Read(id);
            if (retVal.Any())
                Log.Information("Returning message {id}.", id);
            else
                Log.Information("No message {id} found.", id);
            return retVal;
        }

        public void Save(int id, string message){
            Log.Information("Saving message {id}.", id);
            this.writer.Save(id, message);
            Log.Information("Saved message {id}.", id);
        }

        public virtual void Saving(int id, string message)
        {
          
        }

        public virtual void Saved(int id, string message)
        {
            
        }

        public virtual void Reading(int id)
        {
            
        }

        public virtual void DidNotFind(int id)
        {
            
        }

        public virtual void Returning(int id)
        {
            
        }
    }
}
