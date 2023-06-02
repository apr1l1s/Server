using Azure;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Model.Base;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Server.Model
{
    internal class Database
    {
        public static int Authentication(string login, string pass)
        {
            try
            {
                Console.WriteLine("Подключение к бд...");
                using (var bd = new workload_baseEntities() )
                {
                    Console.WriteLine("►Успешно");
                    Console.WriteLine("Выполнение запроса...");
                    Console.WriteLine($"SELECT * FROM users where [login] = \'{login}\' and pass = \'{login}\'");
                    var user = bd.users.Where(u => u.login == login && u.pass == pass).FirstOrDefault();

                    Console.WriteLine("►Успешно");
                    if (user != null)
                    {
                        Console.WriteLine("Пользователь выполнил вход!");
                        return user.prof_id;
                    }
                    else
                    {
                        Console.WriteLine("►Ошибка авторизации");
                        Console.WriteLine("Пароль или логин неверны");
                        return 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("►Ошибка авторизации");
                Console.WriteLine(ex.Message);
                return 0;
            }
        }
        public static int FillWorkload()
        {
            return 0;
        }
        public static int AddTeacher(user user)
        {
            try
            {
                Console.WriteLine("Подключение к бд...");
                using (var bd = new workload_baseEntities())
                {
                    Console.WriteLine("►Успешно");
                    Console.WriteLine("Выполнение запроса...");
                    bd.users.Add(user);
                    bd.SaveChanges();
                    Console.WriteLine("►Успешно");
                    return 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("►Ошибка транзакции");
                Console.WriteLine(ex.Message);
                return 1;
            }
        }
        public static int AddDiscipline(discipline discipline)
        {
            try
            {
                Console.WriteLine("Подключение к бд...");
                using (var bd = new workload_baseEntities())
                {
                    
                    Console.WriteLine("►Успешно");
                    Console.WriteLine("Выполнение запроса...");
                    bd.disciplines.Add(discipline);
                    Console.WriteLine("►Успешно");
                    return 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("►Ошибка транзакции");
                Console.WriteLine(ex.Message);
                return 1;
            }
        }
    }
}
