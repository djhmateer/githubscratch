using Serilog;

namespace SerilogTest
{
    public class StoreLogger
    {
        public void Save(string message)
        {
            var log = new LoggerConfiguration()
                .WriteTo.RollingFile(@"C:\Temp\Log-{Date}.txt")
                .CreateLogger();
            log.Information("Saving");
        }
    }
}
