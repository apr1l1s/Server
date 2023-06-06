

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
using Server.Model.Base;
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
            
            S.Server server = new S.Server("26.45.191.207");

            //var buf = JsonSerializer.Deserialize<LoginRequestResult>("{\"login\":\"user\",\"pass\":\"user\"}");
            //if (buf != null) Console.WriteLine($"{buf.login} {buf.pass}");

            //Parser.Parse(@"C:\Users\apr1l1s\Desktop\Нагрузочный лист.xlsx");

            //Console.WriteLine(file_name);

            



            Console.ReadKey();
        }
    }
}
