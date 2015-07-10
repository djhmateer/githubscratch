using System;
using System.IO;
using Serilog;
using Xunit;

namespace FileStoreSpikes
{
    public class ConsoleStart
    {
        public static void Main()
        {
            Console.WriteLine("Hello world");
            var cr = new CompositionRoot();
            cr.Run();
            Console.ReadLine();
        }
    }

    public class Tests
    {
        //[Fact]
        //public void DaveFileWriter_Save_GivenID2AndAMessage_ShouldSaveToA2TxtFile_InCurrentDiretory()
        //{
        //    var daveFileWriter = new DaveMessageStore();
        //    daveFileWriter.Save(2, "hello2");
        //}

        //[Fact]
        //public void DaveFileWriter_Save_ShouldLogSavingThenSavedToCurrentDirectory()
        //{
        //    var daveFileWriter = new DaveMessageStore();
        //    daveFileWriter.Save(2, "hello2");
        //}
    }

    public class CompositionRoot
    {
        DaveStore daveStore;

        public CompositionRoot()
        {
            var daveWriter = new DaveWriter();
            var daveLogger = new DaveLogger(daveWriter);
            daveStore = new DaveStore(daveLogger);
        }

        public void Run()
        {
            // When save is called, want to call daveLogger.Save first
            daveStore.Save(5, "Console Test");
        }
    }

    public class DaveWriter : IDaveWriter
    {
        public void Save(int id, string message)
        {
            var path = Environment.CurrentDirectory + @"\" + id + ".txt";
            File.WriteAllText(path, message);
        }
    }

    public class DaveLogger : IDaveWriter
    {
        private readonly IDaveWriter writer;
        ILogger log;

        public DaveLogger(IDaveWriter writer)
        {
            this.writer = writer;
            log = new LoggerConfiguration()
                .WriteTo.RollingFile(@"C:\Temp\Log-{Date}.txt")
                .CreateLogger();
        }

        public void Save(int id, string message)
        {
            log.Information("Saving ", id);
            this.writer.Save(id, message);
            log.Information("Saved ", id);
        }
    }

    public class DaveStore : IDaveWriter
    {
        private readonly IDaveWriter writer;

        public DaveStore(IDaveWriter writer)
        {
            this.writer = writer;
        }

        public void Save(int id, string message)
        {
            this.writer.Save(id, message);
        }
    }

    public interface IDaveWriter
    {
        void Save(int id, string message);
    }
}
