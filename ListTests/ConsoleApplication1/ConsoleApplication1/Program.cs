using System;
using System.Collections.Generic;

namespace ConsoleApplication1 {
    class Program {
        static void Main(){
            // 75,000 ThingAs
            var listA = new List<ThingA>();
            listA.Add(new ThingA {Name = "dave", Imported = new DateTime(2015,01,01,10,00,00)});

            // 4,500 ThingBs
            var listB = new List<ThingB>();
            listB.Add(new ThingB {Name = "bob", Imported = new DateTime(2015,01,01,10,00,00)});
            listB.Add(new ThingB {Name = "dave", Imported = new DateTime(2015,01,01,9,00,00), 
                ImportedLowerBound = new DateTime(2015,01,01,8,58,00), 
                ImportedHigherBound = new DateTime(2015,01,01,9,02,00) });
            listB.Add(new ThingB {Name = "dave", Imported = new DateTime(2015,01,01,10,01,00), 
                ImportedLowerBound = new DateTime(2015,01,01,9,59,00), 
                ImportedHigherBound = new DateTime(2015,01,01,10,03,00) });

            // Try1. This takes 17secs
            foreach (var thingA in listA){
                foreach (var thingB in listB){
                    if (thingA.Name != thingB.Name){
                        continue;
                    }

                    // 4 minute range (pre computed bounds in ThingB)
                    if (thingA.Imported >= thingB.ImportedLowerBound && thingA.Imported <= thingB.ImportedHigherBound){
                        Console.WriteLine("Matched");
                    }
                }
            }

            // Try2.  Use Dictionary (only have .NET2 available).  2secs
            var dictOfThingB = new Dictionary<ThingB, string>();
            foreach (var thingA in listA){
                // compare to the dictOfThingB
            }
        }
    }

    class ThingA{
        public string Name { get; set; }
        public DateTime Imported { get; set; }
    }

    class ThingB{
        public string Name { get; set; }
        public DateTime Imported { get; set; }
        public DateTime ImportedLowerBound { get; set; }
        public DateTime ImportedHigherBound { get; set; }
    }
}
