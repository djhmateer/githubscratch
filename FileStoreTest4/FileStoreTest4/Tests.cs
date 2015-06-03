using System;
using System.Linq;
using Ploeh.AutoFixture.Xunit2;
using Serilog;
using Xunit;

namespace FileStoreTest4
{
    public class Tests
    {
        [Fact]
        public void Thing()
        {
            var log = new LoggerConfiguration()
                .WriteTo.RollingFile(@"C:\Temp\Log-{Date}.txt")
                .CreateLogger();

            log.Information("test");

            Assert.Equal(2, 2);
        }

        [Theory, AutoData]
        public void ReadReturnsMessage(string message)
        {
            var fileStore = new FileStore(Environment.CurrentDirectory);
            fileStore.Save(44, message);

            // An IEnumerable.. actually an array with 0 or 1 elements
            // so it may, or may not, return a string
            // The guarantee is that it will return a Maybe<string>
            // will never be null, so can chain like, and easier to read
            Maybe<string> actual = fileStore.Read(44);

            Assert.Equal(message, actual.Single());
        }
    }
}
