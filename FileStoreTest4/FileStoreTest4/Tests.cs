//using Ploeh.AutoFixture.Xunit2;
//using Serilog;
//using System;
//using System.Collections.Concurrent;
//using System.IO;
//using System.Linq;
//using Xunit;

//namespace Mateer.Samples.Encapsulation.CodeExamples
//{
//    public class Tests
//    {
//        [Theory, AutoData]
//        public void ReadReturnsMessage(string message)
//        {
//            var messageStore = new MessageStore(new DirectoryInfo(Environment.CurrentDirectory));

//            messageStore.Save(44, message);

//            // An IEnumerable.. actually an array with 0 or 1 elements
//            // so it may, or may not, return a string
//            // The guarantee is that it will return a Maybe<string>
//            // will never be null, so can chain and easier to read
//            Maybe<string> actual = messageStore.Read(44);

//            Assert.Equal(message, actual.Single());
//        }

//        //[Theory, AutoData]
//        //public void GetFileNameReturnsCorrectResult(int id)
//        //{
//        //    var fileStore = new MessageStore(new DirectoryInfo(Environment.CurrentDirectory));

//        //    FileInfo fileInfo = fileStore.GetFileInfo(id);
//        //    var expected = new FileInfo(Path.Combine(fileStore.WorkingDirectory.FullName, id + ".txt"));
//        //    Assert.Equal(expected.FullName, fileInfo.FullName);
//        //}

//        [Fact]
//        public void ConstructWithNullDirectoryThrows()
//        {
//            Assert.Throws<ArgumentNullException>(() => new MessageStore(null));
//        }

//        [Theory, AutoData]
//        public void ConstructWithInvalidDirectoryThrows(string invalidDirectory)
//        {
//            Assert.False(Directory.Exists(invalidDirectory));
//            Assert.Throws<ArgumentException>(
//                () => new MessageStore(new DirectoryInfo(invalidDirectory)));
//        }

//        [Theory, AutoData]
//        public void ReadUsageExample(string expected)
//        {
//            var fileStore = new MessageStore(new DirectoryInfo(Environment.CurrentDirectory));

//            fileStore.Save(49, expected);

//            // Maybe<string> would be there if not DefaultIfEmpty and Single
//            // If file wasn't there, we'd simply get a "" message
//            string message = fileStore.Read(49).DefaultIfEmpty("").Single();

//            Assert.Equal(expected, message);
//        }

//        [Theory, AutoData]
//        public void ReadExistingFileReturnsTrue(string expected)
//        {
//            var fileStore = new MessageStore(new DirectoryInfo(Environment.CurrentDirectory));

//            fileStore.Save(50, expected);

//            Maybe<string> actual = fileStore.Read(50);

//            // As there are 0 to 1 elements returned in the string array, so testing for 1
//            Assert.True(actual.Any());
//            Assert.True(actual.Count() == 1);
//            Assert.Equal(expected, actual.Single());
//        }

//        [Theory, AutoData]
//        public void ReadNonExistingFileReturnsFalse(string expected)
//        {
//            var fileStore = new MessageStore(new DirectoryInfo(Environment.CurrentDirectory));

//            Maybe<string> actual = fileStore.Read(51);

//            // This is where we handle not being able to read the file
//            Assert.False(actual.Any());
//            Assert.True(actual.Count() == 0);

//            // So the message is "" when it fails to read the file
//            string message = actual.DefaultIfEmpty("").Single();
//            Assert.Equal("", message);
//        }

//        //[Fact]
//        //public void LogTest()
//        //{
//        //    var log = new LoggerConfiguration()
//        //        .WriteTo.RollingFile(@"C:\Temp\Log-{Date}.txt")
//        //        .CreateLogger();

//        //    log.Information("test");

//        //    Assert.Equal(2, 2);
//        //}

//        [Fact]
//        public void ConcurrentDictTest()
//        {
//            var dict = new ConcurrentDictionary<int, string>();
//            // add key 1 value thing1 to the dictionary, or update key1 to thing1
//            dict.AddOrUpdate(1, "thing1", (i, s) => "thing1");
//            dict.AddOrUpdate(2, "thing2", (i, s) => "thing2");
//            dict.AddOrUpdate(1, "thing1", (i, s) => "thingUpdated");

//            // get a value from the dict, and if not there will insert
//            // useful for caching!
//            // 1 is there, so doesn't need to get its value
//            // call the delegate to get the value

//            // lambda expression
//            var result = dict.GetOrAdd(1, _ => Something(1));
//            // lambda expression
//            var result8 = dict.GetOrAdd(8, i => "hello" + i);

//            // an anonymous method
//            var result3 = dict.GetOrAdd(3, delegate { return Something(3); });

//            var result4 = dict.GetOrAdd(4, arg => Something(3));
//            // It recognises the signature, so don't need to pass the 5
//            var result5 = dict.GetOrAdd(5, ValueFactory);
//            // could pass the value anyway
//            var result6 = dict.GetOrAdd(6, ValueFactory(6));
//            var result7 = dict.GetOrAdd(7, Something);
//        }

//        private string ValueFactory(int i)
//        {
//            return "hello again " + i;
//        }

//        private string Something(int i)
//        {
//            return "hello";
//        }

//    }
//}
