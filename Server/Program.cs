

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Server.Utils;
using Server.Model.QA;
using Server.Model;
using S = Server.Server;
using System.Text.Json;

namespace Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Database database = new Database();
            //if (database.connect()) Console.WriteLine("Success!"); else Console.WriteLine("FAULT");
            //Console.WriteLine("Введите айпи сервера");
            //var answ = Console.ReadLine();
            //S.Server server = new S.Server("26.45.191.207");

            //var buf = JsonSerializer.Deserialize<LoginRequestResult>("{\"login\":\"user\",\"pass\":\"user\"}");
            //if (buf != null) Console.WriteLine($"{buf.login} {buf.pass}");

            //Parser.CheckFileType(@"C:\Users\apr1l1s\Desktop\1.xlsx");
            Parser.Parse(@"C:\Users\apr1l1s\Desktop\Нагрузочный лист.xlsx");
            //string full_title = "1,739130435";
            //full_title.Trim();
            //full_title = full_title.Substring(0, 5);
            //var buff_r = double.Parse(full_title);
            //Console.WriteLine(full_title);
            //string code = full_title.Substring(0, 6);
            //Console.WriteLine(code);
            Console.ReadKey();
        }
    }
}
