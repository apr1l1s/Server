using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.Identity.Client;
using Server.Model;
using Server.Model.QA;
using System.IO;
using Server.Server.Utils;

namespace Server.Server
{
    internal class Server
    {
        IPAddress ip = IPAddress.Parse("26.45.191.207");
        int port = 10001;
        public Server(string ip)
        {
            this.ip = IPAddress.Parse(ip);
            var tcpListener = new TcpListener(this.ip, port);
            try
            {
                tcpListener.Start();    // запускаем сервер
                Console.WriteLine("Сервер запущен. Ожидание подключений... ");
                //Получаем входящее подключение
                while (true)
                {
                    var tcpClient = tcpListener.AcceptSocket();
                    var task = new Task(() =>
                    {
                        Console.WriteLine($"Task{Task.CurrentId} Starts");
                        //Получаем информацию из сообщения
                        byte[] data = new byte[1000000];
                        tcpClient.Receive(data);
                        //Энкодинг запроса
                        var request_string = Encoding.UTF8.GetString(data);
                        var request = new Request(request_string);
                        //Обработка запроса
                        Reply reply = ParseRequest(request);
                        //Отправка ответа
                        data = Encoding.UTF8.GetBytes(reply.ToString());
                        tcpClient.Send(data);
                        Console.WriteLine($"Клиенту {ip} отправлены данные\n");
                    });
                    task.Start();
                }  
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                //Останавливаем сервер
                
                tcpListener.Stop();
            }
        }
        public Reply ParseRequest(Request request)
        {
            bool have_body = request.content != "";
            switch (request.method)
            {
                case "GET":
                    GET(); 
                    break;
                case "POST":
                    if (have_body)
                    {
                        return POST(request);
                    }
                    break;
                case "PUT":
                    if (have_body)
                    {
                        PUT();
                    }
                    break;
                default:break;
            }
            var msg = "<h1>Sorry!<h1><p>Server is not online<p>";
            return new Reply(ErrorHandler.StatusType.internal_server_error, msg);
        }
        public void GET(Request request)
        {
            string answer = "";
            if (request.path == "/workload")
            {
                var request_string = JsonSerializer.Deserialize<GetTeacherWorkloadQuestion>(question);
                Request request = new Request("GET", "/workload", request_string);
            }
        }
        public Reply POST(Request request)
        {
            string answer = "";
            if (request.path == "/login")
            {
                LoginRequestResult result = new LoginRequestResult() { login = "1", pass = "1" };
                Console.WriteLine($"{request.method} {request.path} {request.content}");
                try
                {
                    Console.WriteLine(request.content);
                    result = JsonSerializer.Deserialize<LoginRequestResult>(request.content);
                    if (result != null)
                    {
                        var user = Database.Authentication(result.login, result.pass);
                        if (user.prof_id == 0)
                        {
                            return new Reply(ErrorHandler.StatusType.unauthorized, "");
                        }
                        else
                        {
                            answer = JsonSerializer.Serialize(new LoginRequestQuestion() { 
                                teacher_id = user.teacher_id,
                                prof_id = user.prof_id 
                            });
                            Console.WriteLine(answer);
                            return new Reply(ErrorHandler.StatusType.ok, answer);
                        }
                    }
                    else
                    {
                        return new Reply(ErrorHandler.StatusType.bad_request, "");
                    }
                }
                catch (JsonException ex)
                {
                    if (result.pass == "1")
                    {
                        Console.WriteLine("Ошибка парсинга");
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            if (request.path == "/file")
            {
                UploadFileRequestQuestion result = new UploadFileRequestQuestion() { filename = "", file = ""};
                Console.WriteLine($"{request.method}\n {request.path}\n {request.content}\n");
                try
                {
                    //Console.WriteLine(request.content);
                    result = JsonSerializer.Deserialize<UploadFileRequestQuestion>(request.content);
                    if (result != null)
                    {
                        //Загрузить файл в папку
                        var base64EncodedBytes = System.Convert.FromBase64String(result.file);
                        var file_path= result.filename.Split('\\');

                        var file_name = Environment.CurrentDirectory + "\\Files\\";
                        file_name += file_path[file_path.Length - 1];
                        Console.WriteLine(file_name);
                        var file = File.Create(file_name);
                        file.Write(base64EncodedBytes, 0, base64EncodedBytes.Length);
                        file.Close();
                        Console.WriteLine("►Файл успешно загружен на сервер!");
                        //Парсинг сайта
                        //Task.Run(() =>
                        //{
                        //    Parser.Parse(file.Name);
                        //});
                        //Отправка успешного выполнения
                        if (result.filename == "")
                        {
                            answer = JsonSerializer.Serialize(new StatusRequestAnswer() { status = 1 });
                            Console.WriteLine(answer);
                            return new Reply(ErrorHandler.StatusType.bad_request, "");
                        }
                        else
                        {
                            answer = JsonSerializer.Serialize(new StatusRequestAnswer() { status = 0 });
                            Console.WriteLine(answer);
                            return new Reply(ErrorHandler.StatusType.ok, answer);
                        }
                    }
                    else
                    {
                        answer = JsonSerializer.Serialize(new StatusRequestAnswer() { status = -1 });
                        Console.WriteLine(answer);
                        return new Reply(ErrorHandler.StatusType.bad_request, "");
                    }
                }
                catch(JsonException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            answer = JsonSerializer.Serialize(new StatusRequestAnswer() { status = -1 });
            Console.WriteLine(answer);
            return new Reply(ErrorHandler.StatusType.bad_request, "");
        }
        public void PUT()
        {

        }

        
    }
}
