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
        public static GetTeacherWorkloadResult GetTeacherWorkloads(int teacher_id)
        {
            var list = new List<GetTeacherWorkloadElem>();
            try
            {
                Console.WriteLine("Подключение к бд...");
                using(var bd = new workload_baseEntities())
                {
                    Console.WriteLine("►Успешно");
                    Console.WriteLine("Выполнение запроса...");
                    var tws = bd.teachers_workload.Where(w => w.teacher_id == teacher_id).ToList();
                    if (tws.Count > 0)
                    {
                        Console.WriteLine(tws.Count);
                        foreach (var tw in tws)
                        {
                            var elem = new GetTeacherWorkloadElem()
                            {
                                year = tw.year,
                                term_num = tw.term_num,
                                group_standard_name = tw.group.standart_title,
                                group_fullname = tw.group.full_title,
                                discipline = tw.discipline.title,
                                finance_form = tw.group.finance_form_types.type,
                                sum = tw.sum,
                                in_week = tw.in_week,
                                theory = tw.theory,
                                practice = tw.practice,
                                consultation = tw.consultation,
                                course = tw.course,
                                exam = tw.exam
                            };
                            list.Add(elem);
                        }
                    }
                }
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new GetTeacherWorkloadResult() { list = list };
        }
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
