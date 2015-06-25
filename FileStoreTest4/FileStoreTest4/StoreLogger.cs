using Serilog;

namespace Mateer.Samples.Encapsulation.CodeExamples
{
    public class StoreLogger{
        private ILogger log;

        public StoreLogger(){
            Log.Logger = new LoggerConfiguration()
                .WriteTo
                .RollingFile(@"c:\temp\log.txt")
                // When I run the app multiple times (as in tests) it overwrites the file..
                //.File(@"C:\Temp\Log.txt")
                .CreateLogger();

            //log = new LoggerConfiguration()
            //   .WriteTo
            //   .File(@"C:\Temp\Log.txt")
            //   //.RollingFile(@"C:\Temp\Log-{Date}.txt")
            //   .CreateLogger();
        }
        public virtual void Saving(int id)
        {
            Log.Information("Saving message {id}.", id);
        }

        public virtual void Saved(int id)
        {
            Log.Information("Saved message {id}.", id);
        }

        public virtual void Reading(int id)
        {
            Log.Information("Reading message {id}.", id);
        }

        public virtual void DidNotFind(int id)
        {
            Log.Information("No message {id} found.", id);
        }

        public virtual void Returning(int id)
        {
            Log.Information("Returning message {id}.", id);
        }
    }
}
