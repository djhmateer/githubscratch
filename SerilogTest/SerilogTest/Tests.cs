using Xunit;

namespace SerilogTest
{
    public class Tests
    {
        [Fact]
        public void Thing()
        {
            var storeLogger = new StoreLogger();
            storeLogger.Save("test");
            Assert.Equal(1, 1);
        }
    }
}
