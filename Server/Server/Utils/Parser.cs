using Microsoft.Office.Interop.Excel;
using Server.Model;
using Server.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace Server.Server.Utils
{
    enum File_Type { 
        Workload_List = 1,
        Curriculum = 2
    }

    static class Parser
    {
        static object rOnly = true;
        static object SaveChanges = false;
        static object MissingObj = System.Reflection.Missing.Value;
        public static void Parse(string filename)
        {
            //Узнать какой тип файла
            File_Type file_type = CheckFileType(filename);
            Console.WriteLine(file_type.ToString());
            switch (file_type) {
                case File_Type.Curriculum:
                    //Парсить как учебный план
                    
                    break;
                case File_Type.Workload_List:
                    //Парсить как нагрузочный лист
                    Parse_Workload(filename);
                    break;
                default: break;
            }
        }
        static void Parse_Curriculum(string filename)
        {

        }
        static void Parse_Workload(string filename)
        {
            //Создание приложения
            Excel.Application app = new Excel.Application();
            Excel.Workbooks workbooks = null;
            Excel.Workbook workbook = null;
            Excel.Sheets sheets = null;
            try
            {
                var workload = new teachers_workload();
                var workload2 = new teachers_workload();
                using (var bd = new workload_baseEntities())
                {
                    //Открытие приложения в скрытом режиме
                    workbooks = app.Workbooks;
                    workbook = workbooks.Open(
                        filename, MissingObj, rOnly,
                        MissingObj, MissingObj, MissingObj,
                        MissingObj, MissingObj, MissingObj,
                        MissingObj, MissingObj, MissingObj,
                        MissingObj, MissingObj, MissingObj);

                    // Получение всех страниц докуента
                    sheets = workbook.Sheets;
                    var teacher_list = new List<string>();
                    var disciplinesList = new List<string>();
                    for (int j = 1; j < sheets.Count; j++)
                    {
                        //Вывод имени преподавателя
                        //Console.WriteLine(worksheet.Name);
                        var worksheet = sheets[j];

                        // Получаем диапазон используемых на странице ячеек
                        Excel.Range UsedRange = worksheet.UsedRange;
                        // Получаем строки в используемом диапазоне
                        Excel.Range urRows = UsedRange.Rows;
                        // Получаем столбцы в используемом диапазоне
                        Excel.Range urColums = UsedRange.Columns;
                        //Строка и столбец
                        // Количество строк и столбцов
                        int RowsCount = urRows.Count;
                        int ColumnsCount = urColums.Count;

                        //Имя преподавателя
                        Excel.Range CellRange = UsedRange.Cells[2, 1];
                        string teacherFullName = (CellRange == null || CellRange.Value2 == null) ? null :
                                            (CellRange as Excel.Range).Value2.ToString();

                        //Разделение фио
                        var user = new user();
                        var tl = teacherFullName.Split(' ');

                        string surname = null;
                        string name = null;
                        string middlename = null;

                        //Получение пользователя
                        if (tl.Count() < 4)
                        {
                            teacher_list.Add(tl[1] + ' ' + tl[2]);
                            surname = tl[1];
                            name = tl[2];
                        }
                        else
                        {
                            teacher_list.Add(tl[1] + ' ' + tl[2] + ' ' + tl[3]);
                            surname = tl[1];
                            name = tl[2];
                            middlename = tl[3];
                        }
                        user = bd.users.Where(u => u.surname == surname && u.name == name && u.middlename == middlename).FirstOrDefault();
                        if (user == null)
                        {
                            //Если преподаватель не найден, то сохранять ошибку
                            //throw new Exception("Пользователь отсутсвует в базе");
                        }
                        else
                        {
                            workload.user = user;
                            Console.WriteLine(workload.user.surname);
                        }

                        //Год нагрузочного листа
                        Excel.Range YearCell = UsedRange.Cells[1, 1];
                        string YearCellText = (YearCell == null || YearCell.Value2 == null) ? null :
                                            (YearCell as Excel.Range).Value2.ToString();
                        var yearList = YearCellText.Split(' ');
                        Console.WriteLine(yearList[3]);
                        workload.year = yearList[3];

                        //int disciplineStartCol = 1;
                        //bool end = false;
                        bool end = false;
                        var i = 7;
                        while (!end)
                        {
                            Excel.Range endCell = UsedRange.Cells[i, 1];
                            var endCellValue = (endCell == null || endCell.Value2 == null) ? null :
                                            (endCell as Excel.Range).Value2;
                            if (endCellValue == null || endCellValue.ToString() == "Итого") end = true;
                            else i++;
                        }
                        int elemEndRow = i;

                        int elemEndCol = 23;
                        int buff = 0;
                        for (int elemStartRow = 7; elemStartRow <= elemEndRow; elemStartRow++)
                        {
                            var group = new group();
                            var group2 = new group();
                            for (int elemStartCol = 1; elemStartCol <=elemEndCol; elemStartCol++)
                            {
                                Excel.Range cell = UsedRange.Cells[elemStartRow, elemStartCol];
                                var cellValue = (cell == null || cell.Value2 == null) ? null :
                                                (cell as Excel.Range).Value2;
                                string cell_buf = (cellValue == null) ? "0" : cellValue.ToString();

                                switch (elemStartCol)
                                {
                                    case 1:
                                        string title = cell_buf;

                                        workload.discipline = bd.disciplines.Where(d => d.title == title).FirstOrDefault();
                                        break;
                                    case 2:
                                        group.standart_title = cell_buf;
                                        break;
                                    case 3:
                                        var l = cell_buf.Split(' ');
                                        group.full_title = l[0];
                                        if (l.Length > 1)
                                        {
                                            group2.full_title = l[1];
                                        }
                                        break;
                                    case 4:
                                        group.finance_from = cell_buf == "дог" ? 2 : 1;
                                        break;
                                    case 5:
                                        break;
                                    case 6:
                                        workload.term_num = 1;
                                        buff = int.Parse(cell_buf);
                                        workload.sum = buff;
                                        break;
                                    case 7:
                                        if (cell_buf.Length > 5)
                                        cell_buf = cell_buf.Substring(0, 5);
                                        var buff_r = double.Parse(cell_buf);
                                        workload.in_week = buff_r;
                                        break;
                                    case 8:
                                        buff = int.Parse(cell_buf);
                                        workload.theory = buff;
                                        break;
                                    case 9:
                                        buff = int.Parse(cell_buf);
                                        workload.practice = buff;
                                        break;
                                    case 10:
                                        buff = int.Parse(cell_buf);
                                        workload.course = buff;
                                        break;
                                    case 11:
                                        buff = int.Parse(cell_buf);
                                        workload.consultation = buff;
                                        break;
                                    case 12:
                                        buff = int.Parse(cell_buf);
                                        workload.exam = buff;
                                        break;
                                    case 13:
                                        buff = int.Parse(cell_buf);
                                        workload2 = workload;
                                        workload2.term_num = 2;
                                        workload2.sum = buff;
                                        break;
                                    case 14:
                                        if (cell_buf.Length > 5)
                                        cell_buf = cell_buf.Substring(0, 5);
                                        buff_r = double.Parse(cell_buf);
                                        workload2.in_week = buff_r;
                                        break;
                                    case 15:
                                        buff = int.Parse(cell_buf);
                                        workload2.theory = buff;
                                        break;
                                    case 16:
                                        buff = int.Parse(cell_buf);
                                        workload2.practice = buff;
                                        break;
                                    case 17:
                                        buff = int.Parse(cell_buf);
                                        workload2.course = buff;
                                        break;
                                    case 18:
                                        buff = int.Parse(cell_buf);
                                        workload2.consultation = buff;
                                        break;
                                    case 19:
                                        buff = int.Parse(cell_buf);
                                        workload2.exam = buff;
                                        break;
                                    case 20:
                                        break;
                                    case 21:
                                        break;
                                    case 22:
                                        break;
                                    case 23:
                                        break;
                                }
                                

                            }
                            if (workload.discipline != null)
                            {
                                string code = group.full_title.Substring(0, 6);
                                group.specialization_code = code;
                                group2.standart_title = group.standart_title;
                                group2.finance_from = group.finance_from;
                                group2.specialization_code = group.specialization_code;
                                bd.groups.Add(group);
                                bd.groups.Add(group2);
                                bd.SaveChanges();

                                workload.group = bd.groups.Where(g => g.standart_title == group.standart_title).FirstOrDefault();
                                workload2.group = bd.groups.Where(g => g.standart_title == group.standart_title).FirstOrDefault();
                                bd.teachers_workload.Add(workload);
                                bd.teachers_workload.Add(workload2);
                                bd.SaveChanges();
                                Console.WriteLine("!");
                            }
                        }


                        // Очистка неуправляемых ресурсов на каждой итерации
                        if (urRows != null) Marshal.ReleaseComObject(urRows);
                        if (urColums != null) Marshal.ReleaseComObject(urColums);
                        if (UsedRange != null) Marshal.ReleaseComObject(UsedRange);
                        if (worksheet != null) Marshal.ReleaseComObject(worksheet);
                    }
                }
                //Console.WriteLine(teacher_list.Count.ToString());
                //var t = teacher_list.Distinct().ToList();

                //for(int h=0; h<t.Count; h++)
                //{
                //    var fullnamelist = t[h].Split(' ');
                //    if (fullnamelist.Count() < 3)
                //        Database.AddTeacher("user", "user", 2, fullnamelist[0], fullnamelist[1], "");
                //    else Database.AddTeacher("user", "user", 2, fullnamelist[0], fullnamelist[1], fullnamelist[2]);
                //}

                //for (int i = 0; i < t.Count; i++)
                //{
                //    Console.WriteLine(t[i]);
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                /* Очистка оставшихся неуправляемых ресурсов */
                if (sheets != null) Marshal.ReleaseComObject(sheets);
                if (workbook != null)
                {
                    workbook.Close(SaveChanges);
                    Marshal.ReleaseComObject(workbook);
                    workbook = null;
                }

                if (workbooks != null)
                {
                    workbooks.Close();
                    Marshal.ReleaseComObject(workbooks);
                    workbooks = null;
                }
                if (app != null)
                {
                    app.Quit();
                    Marshal.ReleaseComObject(app);
                    app = null;
                }
            }
        }
        public static File_Type CheckFileType(string FileName)
        {
            Excel.Application app = new Excel.Application();
            Excel.Workbooks workbooks = null;
            Excel.Workbook workbook = null;
            Excel.Sheets sheets = null;
            try
            {
                workbooks = app.Workbooks;
                workbook = workbooks.Open(FileName, MissingObj, rOnly, MissingObj, MissingObj,
                                            MissingObj, MissingObj, MissingObj, MissingObj, MissingObj,
                                            MissingObj, MissingObj, MissingObj, MissingObj, MissingObj);

                // Получение всех страниц докуента
                sheets = workbook.Sheets;
                Excel.Worksheet sheet = sheets[1];
                Console.WriteLine(sheet.Name);
                return sheet.Name == "Титул" ? File_Type.Curriculum : File_Type.Workload_List;
            }
            catch (Exception ex)
            {
                /* Обработка исключений */
                Console.WriteLine(ex.Message);
            }
            finally
            {
                /* Очистка оставшихся неуправляемых ресурсов */
                if (sheets != null) Marshal.ReleaseComObject(sheets);
                if (workbook != null)
                {
                    workbook.Close(SaveChanges);
                    Marshal.ReleaseComObject(workbook);
                    workbook = null;
                }

                if (workbooks != null)
                {
                    workbooks.Close();
                    Marshal.ReleaseComObject(workbooks);
                    workbooks = null;
                }
                if (app != null)
                {
                    app.Quit();
                    Marshal.ReleaseComObject(app);
                    app = null;
                }
            }
            return File_Type.Workload_List;
        }
    }
}
