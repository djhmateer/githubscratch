using Ploeh.AutoFixture.Xunit;
using System;
using System.IO;
using System.Linq;
using Xunit;
//using Xunit.Extensions;
//using Ploeh.AutoFixture.Xunit;

namespace Ploeh.Samples.Encapsulation.CodeExamples
{
    public class FileStoreUnitTests
    {
        //[Theory, AutoData]
        [Theory]
        [InlineData("test message")]
        public void ReadReturnsMessage(string expected)
        {
            var fileStore = new FileStore(Environment.CurrentDirectory);
            fileStore.Save(44, expected);

            var actual = fileStore.Read(44);

            Assert.Equal(expected, actual.Single());
        }

        //[Theory, AutoData]
        [Theory]
        [InlineData(44)]
        public void GetFileNameReturnsCorrectResult(int id)
        {
            var fileStore = new FileStore(Environment.CurrentDirectory);

            string actual = fileStore.GetFileName(id);

            var expected = Path.Combine(fileStore.WorkingDirectory, id + ".txt");
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ConstructWithNullDirectoryThrows()
        {
            Assert.Throws<ArgumentNullException>(() => new FileStore(null));
        }

        //[Theory, AutoData]
        //public void ConstructWithInvalidDirectoryThrows(string invalidDirectory)
        //{
        //    Assert.False(Directory.Exists(invalidDirectory));
        //    Assert.Throws<ArgumentException>(
        //        () => new FileStore(invalidDirectory));
        //}

        //[Theory, AutoData]
        //public void ReadUsageExample(string expected)
        //{
        //    var fileStore = new FileStore(Environment.CurrentDirectory);
        //    fileStore.Save(49, expected);

        //    var message = fileStore.Read(49).DefaultIfEmpty("").Single();

        //    Assert.Equal(expected, message);
        //}

        //[Theory, AutoData]
        //public void ReadExistingFileReturnsTrue(string expected)
        //{
        //    var fileStore = new FileStore(Environment.CurrentDirectory);
        //    fileStore.Save(50, expected);

        //    var actual = fileStore.Read(50);

        //    Assert.True(actual.Any());
        //    Assert.Equal(expected, actual.Single());
        //}

        //[Theory, AutoData]
        //public void ReadNonExistingFileReturnsFalse(string expected)
        //{
        //    var fileStore = new FileStore(Environment.CurrentDirectory);

        //    var actual = fileStore.Read(51);

        //    Assert.False(actual.Any());
        //}
    }
}
