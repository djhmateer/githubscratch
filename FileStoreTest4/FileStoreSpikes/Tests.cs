using System;
using System.IO;
using Xunit;

namespace FileStoreSpikes
{
    public class Tests
    {
        [Fact]
        public void Thing()
        {
            Assert.Equal(1, 1);
        }

        [Fact]
        public void DaveFileWriter_Save_GivenID2AndAMessage_ShouldSaveToA2TxtFile_InCurrentDiretory()
        {
            var daveFileWriter = new DaveFileWriter();
            daveFileWriter.Save(2, "hello2");
        }

        [Fact]
        public void DaveFileWriter_Save_ShouldLogSavingThenSavedToCurrentDirectory()
        {
            var daveFileWriter = new DaveFileWriter();
            daveFileWriter.Save(2, "hello2");
        }
    }

    public class DaveFileWriter
    {
        public void Save(int id, string message){
            var path = Environment.CurrentDirectory + @"\" + id + ".txt";
            File.WriteAllText(path, message);
        }
    }
}
