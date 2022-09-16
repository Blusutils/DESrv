//using System.Windows.Forms;
namespace des_config {
    //class GuiForm : Form { }
    internal class Program {
        static void Main(string[] args) {
            /*if (args.Contains("-gui")) {
                new GuiForm()
            }*/
            Console.WriteLine("Select language, what we'll use when configuring DESrv. Type 'en' for English.");
            Console.WriteLine("Выберите язык, который мы будем использовать при настройке DESrv. Напишите 'ru' для русского языка");
            string lng;
            while (true) {
                lng = Console.ReadLine();
                switch (lng) {
                    case "en":
                        Console.WriteLine("Ok, done");
                        break;
                    case "ru":
                        Console.WriteLine("Хорошо, готово");
                        break;
                    default:
                        Console.WriteLine("Unknown language, try again");
                        continue;
                }
                break;
            };
        }
    }
}