using System;
using System.Threading.Tasks;

namespace AsyncConsole {
    class Program {
        static void Main() {
            // To show things hapenning in parallel
            MainA().Wait();
        }

        static async Task MainA() {
            Console.WriteLine("starting");
            var time = DateTime.Now;
            var t1 = DoSomething("t1", 4000);
            var t2 = DoSomething("t2", 3000);
            var t3 = DoSomething("t3", 2000);

            // All running in parallel now
            await t1;
            Console.WriteLine("after t1 - {0}", Actual(time));
            await t2;
            Console.WriteLine("after t2 - {0}", Actual(time));
            await t3;
            Console.WriteLine("after t3 - {0}", Actual(time));

            Console.WriteLine("Done: {0}", String.Join(", ", t1.Result, t2.Result, t3.Result));
            Console.WriteLine("Total: " + Actual(time));
        }

        static async Task<string> DoSomething(string name, int timeout) {
            var time = DateTime.Now;
            await Task.Delay(timeout);
            var message = "Exit " + name + " in: " + Actual(time);
            Console.WriteLine(message);
            return message;
        }

        static string Actual(DateTime time) {
            return (DateTime.Now - time).TotalMilliseconds.ToString();
        }
    }
}
