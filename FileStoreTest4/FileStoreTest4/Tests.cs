using Ploeh.AutoFixture.Xunit2;
using Serilog;
using System;
using System.IO;
using System.Linq;
using Xunit;

namespace Mateer.Samples.Encapsulation.CodeExamples
{
    public class Tests
    {
        [Fact]
        public void LogTest()
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
            var fileStore = new FileStore(new DirectoryInfo(Environment.CurrentDirectory));

            fileStore.Save(44, message);

            // An IEnumerable.. actually an array with 0 or 1 elements
            // so it may, or may not, return a string
            // The guarantee is that it will return a Maybe<string>
            // will never be null, so can chain like, and easier to read
            Maybe<string> actual = fileStore.Read(44);

            Assert.Equal(message, actual.Single());
        }

        [Theory, AutoData]
        public void GetFileNameReturnsCorrectResult(int id)
        {
            var fileStore = new FileStore(new DirectoryInfo(Environment.CurrentDirectory));

            FileInfo fileInfo = fileStore.GetFileInfo(id);
            var expected = new FileInfo(Path.Combine(fileStore.WorkingDirectory.FullName, id + ".txt"));
            Assert.Equal(expected.FullName, fileInfo.FullName);
        }

        [Fact]
        public void ConstructWithNullDirectoryThrows()
        {
            Assert.Throws<ArgumentNullException>(() => new FileStore(null));
        }

        [Theory, AutoData]
        public void ConstructWithInvalidDirectoryThrows(string invalidDirectory)
        {
            Assert.False(Directory.Exists(invalidDirectory));
            Assert.Throws<ArgumentException>(
                () => new FileStore(new DirectoryInfo(invalidDirectory)));
        }

        [Theory, AutoData]
        public void ReadUsageExample(string expected)
        {
            var fileStore = new FileStore(new DirectoryInfo(Environment.CurrentDirectory));

            fileStore.Save(49, expected);

            // Maybe<string> would be there if not DefaultIfEmpty and Single
            // If file wasn't there, we'd simply get a "" message
            string message = fileStore.Read(49).DefaultIfEmpty("").Single();

            Assert.Equal(expected, message);
        }

        [Theory, AutoData]
        public void ReadExistingFileReturnsTrue(string expected)
        {
            var fileStore = new FileStore(new DirectoryInfo(Environment.CurrentDirectory));

            fileStore.Save(50, expected);

            Maybe<string> actual = fileStore.Read(50);

            // As there are 0 to 1 elements returned in the string array, so testing for 1
            Assert.True(actual.Any());
            Assert.True(actual.Count() == 1);
            Assert.Equal(expected, actual.Single());
        }

        [Theory, AutoData]
        public void ReadNonExistingFileReturnsFalse(string expected)
        {
            var fileStore = new FileStore(new DirectoryInfo(Environment.CurrentDirectory));

            Maybe<string> actual = fileStore.Read(51);

            // This is where we handle not being able to read the file
            Assert.False(actual.Any());
            Assert.True(actual.Count() == 0);

            // So the message is "" when it fails to read the file
            string message = actual.DefaultIfEmpty("").Single();
            Assert.Equal("", message);
        }
    }
}
