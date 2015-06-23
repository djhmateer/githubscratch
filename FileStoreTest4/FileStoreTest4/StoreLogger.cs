using Serilog;

namespace Mateer.Samples.Encapsulation.CodeExamples
{
    public class StoreLogger{
        private ILogger log;

        public StoreLogger(){
            log = new LoggerConfiguration()
               .WriteTo
               .File(@"C:\Temp\Log.txt")
               //.RollingFile(@"C:\Temp\Log-{Date}.txt")
               .CreateLogger();
        }
        public void Saving(int id)
        {
            log.Information("Saving message {id}.", id);
        }

        public void Saved(int id)
        {
            log.Information("Saved message {id}.", id);
        }

        public void Reading(int id)
        {
            log.Debug("Reading message {id}.", id);
        }

        public void DidNotFind(int id)
        {
            log.Debug("No message {id} found.", id);
        }

        public void Returning(int id)
        {
            log.Debug("Returning message {id}.", id);
        }
    }
}
