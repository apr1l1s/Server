using Azure;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Model.Base;
using Server.Model.QA;
using Server.Server.Hashing;

namespace Server.Model
{
    internal class Database
    {
        public static LoginRequestQuestion Authentication(string login, string pass)
        {
            try
            {
                Console.WriteLine("Подключение к бд...");
                using (var bd = new workload_baseEntities() )
                {
                    Console.WriteLine("►Успешно");
                    Console.WriteLine("Выполнение запроса...");
                    //Поступающий пароль незахеширован
                    //В базе хранится хешированный и посоленый пароль
                    //Получить по логину соль
                    var salt = bd.users.Where(u => u.login == login).Select(u =>u.pass_salt).FirstOrDefault();
                    if (salt != null)
                    {
                        var unhashedSaltedPass = pass + salt;
                        var hashedSaltedPass = HashHelper.HashString(unhashedSaltedPass);
                        var user = bd.users.Where(u => u.login == login && u.pass == hashedSaltedPass).FirstOrDefault();
                        Console.WriteLine("►Успешно");
                        if (user != null)
                        {
                            Console.WriteLine("Пользователь выполнил вход!");
                            var answ = new LoginRequestQuestion()
                            {
                                teacher_id = user.user_id,
                                prof_id = user.prof_id
                            };
                            return answ;
                        }
                        else
                        {
                            Console.WriteLine("►Ошибка авторизации");
                            Console.WriteLine("Пароль или логин неверны");
                            var answ = new LoginRequestQuestion()
                            {
                                teacher_id = 0,
                                prof_id = 0
                            };
                            return answ;
                        }
                    }
                    else
                    {
                        Console.WriteLine("►Ошибка авторизации");
                        Console.WriteLine("Пароль или логин неверны");
                        var answ = new LoginRequestQuestion()
                        {
                            teacher_id = 0,
                            prof_id = 0
                        };
                        return answ;
                    }
                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("►Ошибка авторизации");
                Console.WriteLine(ex.Message);
                var answ = new LoginRequestQuestion()
                {
                    teacher_id = 0,
                    prof_id = 0
                };
                return answ;
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
