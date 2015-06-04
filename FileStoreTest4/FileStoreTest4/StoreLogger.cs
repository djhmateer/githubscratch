using Serilog;

namespace Mateer.Samples.Encapsulation.CodeExamples
{
    public class StoreLogger
    {
        public void Saving(int id)
        {
            Log.Information("Saving message {id}.", id);
        }

        public void Saved(int id)
        {
            Log.Information("Saved message {id}.", id);
        }

        public void Reading(int id)
        {
            Log.Debug("Reading message {id}.", id);
        }

        public void DidNotFind(int id)
        {
            Log.Debug("No message {id} found.", id);
        }

        public void Returning(int id)
        {
            Log.Debug("Returning message {id}.", id);
        }
    }
}
