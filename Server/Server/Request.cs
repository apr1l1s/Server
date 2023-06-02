using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Server
{
    internal class Request
    {
        //GET / HTTP/1.1
        //Client: apr1l1s1
        //Host: localhost:10001
        //Content-Type: application/json
        //Content-Length: 10
        //content

        public string method = "";
        public string path ="";
        public string content = "";
        
        public Request(string request_string) {
            var request_list = request_string.Split('\n');
            //обработка строки запроса
            var method_string = request_list[0];
            method = method_string.Split(' ')[0];
            path = method_string.Split(' ')[1];
            //обработка тела
            int i = 0;
            var have_body = false;
            for (i = 0; i < request_list.Length; i++)
            {
                //Есть тело
                if (request_list[i].StartsWith("Content-Length"))
                {
                    have_body = true;
                    break;
                }
            }
            if (have_body)
            {
                for (int j = i + 1; j < request_list.Length; j++)
                {
                    content += request_list[j];
                }
                content = content.Replace("\r", "");
                content = content.Replace("\0", "");
            }
        }
    }
}
