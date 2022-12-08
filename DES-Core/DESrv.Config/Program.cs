//using System.Windows.Forms;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DESrv.Config {
    //class GuiForm : Form { }
    public class Program {
        static T Input<T>(string message, string errorMessage) {
            Console.WriteLine(message);
            var inp = Console.ReadLine();
            try {
                return (T)Convert.ChangeType(inp, typeof(T))!;
            } catch (InvalidCastException) {
                Console.WriteLine($"{errorMessage}. Try again.");
                return Input<T>(message, errorMessage);
            }
            
        }
        public static void Main(string[] args) {
            Console.WriteLine("Welcome to DESrv configuration setup.");
            Console.WriteLine("For now multilanguage is not supported");
            var configPath = System.Reflection.Assembly.GetExecutingAssembly().Location.Replace(Path.GetFileName(System.Reflection.Assembly.GetExecutingAssembly().Location), "config.json");
            /*if (args.Contains("-gui")) {
                new GuiForm()
            }*/
            //Console.WriteLine("Select language, what we'll use when configuring DESrv. Type 'en' for English.");
            //Console.WriteLine("Выберите язык, который мы будем использовать при настройке DESrv. Напишите 'ru' для русского языка");
            //string lng;
            //while (true) {
            //    lng = Console.ReadLine();
            //    switch (lng) {
            //        case "en":
            //            Console.WriteLine("Ok, done");
            //            break;
            //        case "ru":
            //            Console.WriteLine("Хорошо, готово");
            //            break;
            //        default:
            //            Console.WriteLine("Unknown language, try again");
            //            continue;
            //    }
            //    break;
            //};
            string rndm() {
                var i = Input<string>("Enter default random provider: ", "Invalid");
                if (i is not null or "") return "dotnet";
                else return i;
            }
            var cfg = new OurConfig {
                ipAdress = Input<string>("Enter default IP address: ", "Invalid"),
                logLevel = Input<int>("Enter default logging level: ", "Invalid"),
                extensionsToLoad = Input<string>("Enter default extensions IDs separated by semicolon: ", "Invalid").Split(';'),
                randomMode = rndm(),
                mainExtension = Input<string>("Endet main extension ID: ", "Invalid")
            };
            var f = File.OpenWrite(configPath);
            f.Write(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(cfg)));
            f.Close();
            Thread.Sleep(1000);
        }
    }
}