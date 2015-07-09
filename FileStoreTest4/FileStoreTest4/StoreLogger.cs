using System.Linq;
using Serilog;

namespace Mateer.Samples.Encapsulation.CodeExamples
{
    public class StoreLogger : IStoreWriter, IStoreReader
    {
        private readonly ILogger log;
        private readonly IStoreWriter writer;
        private readonly IStoreReader reader;

        public StoreLogger(ILogger log, IStoreWriter writer, IStoreReader reader){
            this.log = log;
            this.writer = writer;
            this.reader = reader;
        }

        public void Save(int id, string message){
            Log.Information("Saving message {id}.", id);
            this.writer.Save(id, message);
            Log.Information("Saved message {id}.", id);
        }

        public Maybe<string> Read(int id){
            this.log.Debug("Reading message {id}.", id);
            var retVal = this.reader.Read(id);
            if (retVal.Any())
                this.log.Debug("Returning message {id}.", id);
            else
                this.log.Debug("No message {id} found.", id);
            return retVal;
        }
    }
}
