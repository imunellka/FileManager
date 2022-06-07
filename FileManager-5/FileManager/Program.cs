using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Read.me
/// Немного о коде: в программе реализованы все пункты основного функционала.
/// А также 4 из 5 дополнительных пункта ( все кроме 5)
/// Механика проста все операции доступны с файлами из текущей директории.
/// Выбираете директорию/диск - переходите туда.
/// Для перехода в различные директории есть 2 варианта:
///      1.указать полный путь к нужной директории
///      2.спускаться вниз по иерархии или выбрать диск.
/// Изначально для системы вы находитесь в папке, откуда запускали данные проект.
/// Я пыталась сделать кроссплатформенную программу, однако, если что-то не будет работать на linux или Windows сильно не ругайтесь.
/// Гарантирую работоспособность на Mac OS.
/// Приятного просмотра, проверяющий :)
/// </summary>
///

namespace FileManager
{
    class Program
    {
        // Общий счетчик , отвечающий за очищение консоли.
        static int cleaner = 0;


        /// <summary>
        /// Метод возвращает корректное число, вводимое пользователем.
        /// </summary>
        /// <param name="right">правая граница</param>
        /// <returns>число в интерале от 1 до right</returns>
        static int CheckLimit(int right=2)
        {
            int.TryParse(Console.ReadLine(), out int option);
            while (option < 1 || option > right)
            {
                Console.WriteLine($"Пожалуйста, введите положительное целое число до {right}");
                int.TryParse(Console.ReadLine(), out option);
            }
            return option;
        }


        /// <summary>
        /// Метод выбора папки и перехода в нее.
        /// </summary>
        static void SetDirectory()
        {
            Console.WriteLine($"Выберите опцию:\n1. выбор директории по заданному пути\n2. выбор директории из доступных на данном уровне");
            int option = CheckLimit(2);
            if (option == 2)
            {
                string dir = Directory.GetCurrentDirectory();
                List<string> direct = new List<string>();
                int j = 1;
                if (Directory.EnumerateDirectories(dir).Count() == 0)
                {
                    Console.WriteLine("Увы, здесь нет доступных папок");
                    return;
                }
                Console.WriteLine("Перед вами все папки доступные в месте, где вы находитесь:");
                foreach (string file in Directory.EnumerateDirectories(dir))
                {
                    Console.WriteLine($"{j++}. {file.Split(new char[] { '\\' })[file.Split(new char[] { '\\' }).Length - 1]}");
                    direct.Add(file.Split(new char[] { '\\' })[file.Split(new char[] { '\\' }).Length - 1]);
                }
                Console.WriteLine($"Выберите директорию: целое число до {direct.Count}");
                int length = CheckLimit(direct.Count);
                GoToDirectory(direct, length);
            }
            else
            {
                Console.WriteLine("Введите путь");
                string path = Console.ReadLine();
                try
                {
                    Directory.SetCurrentDirectory(path);
                    Console.WriteLine("Вы перешли в нужную папку.");
                }
                catch
                {
                    Console.WriteLine("Путь к желаемой директории задан некорректно.");
                }
            }
        }



        /// <summary>
        /// Вспомогательный метод, который обрабатывает переход в другую директорию.
        /// </summary>
        /// <param name="direct">список файлов</param>
        /// <param name="length">индекс нужного файла</param>
        private static void GoToDirectory(List<string> direct, int length)
        {
            try
            {
                Directory.SetCurrentDirectory(direct[length - 1]);
                Console.WriteLine("Вы перешли в нужную папку.");
            }
            catch
            {
                Console.WriteLine("Простите, данная директория не может быть выбрана");
            }
        }


        /// <summary>
        /// Метод , отвечающий за выбор файла.
        /// </summary>
        /// <returns>Путь к выбранному файлу</returns>
        static string ChooseFile()
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                string dir = Directory.GetCurrentDirectory();
                List<string> files = new List<string>();
                int j = 1;
                if (Directory.EnumerateFiles(dir).Count() == 0)
                {
                    Console.WriteLine("Увы, здесь нет доступных файлов");
                    Console.ResetColor();
                    return "!!!";
                }
                Console.WriteLine("Перед вами все файлы папки, в которой вы находитесь:");
                foreach (string file in Directory.EnumerateFiles(dir))
                {
                    Console.WriteLine($"{j++}. {file.Split(new char[] { '\\' })[file.Split(new char[] { '\\' }).Length - 1]}");
                    files.Add(file.Split(new char[] { '\\' })[file.Split(new char[] { '\\' }).Length - 1]);
                }
                Console.WriteLine($"Выберите файл: целое число до {files.Count}");
                int path = CheckLimit(files.Count);
                Console.WriteLine("Файл выбран");
                Console.ResetColor();
                return files[path - 1];
            }
            catch
            {
                Console.WriteLine("Доступ закрыт.");
                return "!!!";
            }
        }

        /// <summary>
        /// Метод выводит на экран все файлы данной папки.
        /// </summary>
        static void AllFiles()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            try
            {
                string dir = Directory.GetCurrentDirectory();
                int j = 1;
                if (Directory.EnumerateFiles(dir).Count() == 0)
                {
                    Console.WriteLine("Увы, здесь нет доступных файлов");
                    return;
                }
                Console.WriteLine("Перед вами все файлы папки, в которой вы находитесь:");
                foreach (string file in Directory.EnumerateFiles(dir))
                    Console.WriteLine($"{j++}. {file.Split(new char[] { '\\' })[file.Split(new char[] { '\\' }).Length - 1]}");
            }
            catch
            {
                Console.WriteLine("Не удалось вывести файлы, доступ закрыт");
            }
            Console.ResetColor();
        }


        /// <summary>
        /// Метод , создающий копию файла.
        /// </summary>
        static void CopyFile()
        {
            string path = ChooseFile();
            if (path == "!!!")
            {
                Console.WriteLine("Создайте новый файл в этой директории или перейдите к выбору диска");
                return;
            }
            var s= path.Split("/");
            s[^1] = "Copy" + s[^1];
            string path2 = String.Join("/",s);
            try
            {
                File.Copy(path, path2);
                Console.WriteLine();
                Console.WriteLine($"Создана копия вашего файла в той же папке с название: {s[^1]}");
                Console.WriteLine(path2);
            }
            catch
            {
                Console.WriteLine("Некорректные данные, не удалось создать копию.");
                Console.WriteLine("Проверьте формат входных данных, возможна программа пока не научилась с такими работать.");
            }
        }

        /// <summary>
        /// Метод, удаляющий файл.
        /// </summary>
        static void DeleteFile()
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            string path = ChooseFile();
            if (path == "!!!")
            {
                Console.WriteLine("Создайте новый файл в этой директории или перейдите к выбору диска");
                return;
            }
            try
            {
                File.Delete(path);
                Console.WriteLine("Файл удален");
            }
            catch
            {
                Console.WriteLine("Простите, программа не может удалить этот файл");
            }
            Console.ResetColor();
        }

        /// <summary>
        /// Метод, создающий файл.
        /// </summary>
        static void CreateFile()
        {
            try
            {
                string fileName = Path.GetRandomFileName().Split(".")[0] + ".txt";
                string pathString = Directory.GetCurrentDirectory();
                Console.WriteLine("Выберите кодировку:");
                Console.WriteLine("1. UTF-8");
                Console.WriteLine("2. UNICODE");
                Console.WriteLine("3. ASCII");
                int code = CheckLimit(3);
                pathString = Path.Combine(pathString, fileName);
                using (FileStream fs = File.Create(pathString))
                {
                    StreamWriter output = new StreamWriter(fs);
                    Console.WriteLine("Введите ваши данные - окончание ввода пустая строка.");
                    string k = "";
                    k = Console.ReadLine().Trim();
                    while (k.Trim() != "")
                    {
                        switch (code)
                        {
                            case <= 1:
                                output.Write(k, Encoding.UTF8);
                                break;
                            case <= 2:
                                output.Write(k, Encoding.Unicode);
                                break;
                            case <= 3:
                                output.Write(k, Encoding.ASCII);
                                break;
                        }
                        output.Write("\n");
                        k = Console.ReadLine().Trim();
                    }
                    output.Close();
                }
                Console.WriteLine("Файл успешно создан");
                Console.WriteLine($"Путь к файлу: {pathString}");
            }
            catch
            {
                Console.WriteLine("Простите, программа не может создать этот файл");
            }
        }


        /// <summary>
        /// Метод , отвечающий за поиск по маске.
        /// </summary>
        static void Maska(bool tub= false)
        {
            string pathString = Directory.GetCurrentDirectory();
            Console.Write("Введите маску: ");
            string maska = Console.ReadLine().Trim();
            try
            {
                string[] files=ExtraMaska(tub, pathString, maska);
                if (files.Length == 0)
                {
                    Console.WriteLine("По заданной маске ничего не найдено");
                    return;
                }
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("Найденные файлы:");
                foreach (string file in files)
                {
                   Console.WriteLine(file);
                }
                Console.ResetColor();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.WriteLine("Введенная маска некорректна!");
            }
        }


        /// <summary>
        /// Вспомогательный метод, для поиска по маске.
        /// </summary>
        /// <param name="tub">флаг, определяющий , где осуществлять поиск</param>
        /// <param name="pathString">путь к текущей директории</param>
        /// <param name="maska">маска</param>
        /// <returns>список найденных файлов</returns>
        private static string[] ExtraMaska(bool tub, string pathString, string maska)
        {
            try
            {
                if (tub == true)
                {
                    bool completed = TimeLimit(TimeSpan.FromMilliseconds(1000), () =>
                    {
                        try
                        {
                            string[] dir = Directory.GetFiles(pathString, maska, SearchOption.AllDirectories);
                        }
                        catch { completed = false; }
                    });
                    if (completed == true)
                    {
                        string[] dir = Directory.GetFiles(pathString, maska, SearchOption.AllDirectories);
                        return dir;
                    }
                    else
                    {
                        Console.WriteLine("Превышен лимит времени по поиску из-за большого количества данных");
                        Console.WriteLine("Перейдите в более локальные директории для поиска");
                        string[] dir3 = new string[1] { "1" };
                        return dir3;
                    }
                }
                else
                {
                    string[] dir = Directory.GetFiles(pathString, maska);
                    return dir;
                }
            }
            catch
            {
                Console.WriteLine("Доступ закрыт ");
                string[] dir2 = new string[0];
                return dir2;
            }
        }


        /// <summary>
        /// Метод, ограничивающий функцию по времени.
        /// </summary>
        /// <param name="timeSpan">ограничение по времени</param>
        /// <param name="codeBlock">сама функция</param>
        /// <returns></returns>
        public static bool TimeLimit(TimeSpan timeSpan, Action codeBlock)
        {
            try
            {
                Task task = Task.Factory.StartNew(() => codeBlock());
                task.Wait(timeSpan);
                return task.IsCompleted;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Метод, осуществляющий перенос файлов по заданной маске.
        /// </summary>
        static void CopyMaska()
        {
            try
            {
                string pathString = Directory.GetCurrentDirectory();
                Console.Write("Введите маску: ");
                string maska = Console.ReadLine().Trim();
                string[] files = ExtraMaska(true, pathString, maska);
                if (files[0] == "1")
                    return;
                foreach (var fil in files)
                    Console.WriteLine(fil);
                Console.WriteLine("Введите полный путь до желаемой директории");
                string path = Console.ReadLine();
                Console.WriteLine("Если в процессе копирования совпадет имя файла: (выберите опцию)");
                Console.WriteLine($"1.заменить файл на новый\n2.оставить старый файл");
                int tub = CheckLimit(2);

                Directory.CreateDirectory(path);
                Directory.SetCurrentDirectory(path);
                if (files.Length != 0)
                {
                    foreach (var file in files)
                    {
                        string newPath = Path.Combine(path, file.Split("/")[^1]);
                        if (!File.Exists(newPath) || tub == 1)
                        {
                            if (File.Exists(newPath))
                            {
                                Directory.Delete(newPath);
                            }
                            File.Copy(file, newPath);
                        }
                    }
                    Console.WriteLine("Все файлы по заданной маске успешно перенесены в выбранную директорию");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine($"Программа не смогла осуществить перенос\nВероятная ошибка-некорректный путь до желаемой директории");
            }
        } 


        /// <summary>
        /// Метод объединяет файлы.
        /// </summary>
        static void ConcatenateFile()
        {
            string path = ChooseFile();
            if (path == "!!!")
            {
                Console.WriteLine("Создайте новый файл в этой директории или перейдите к выбору диска");
                return;
            }
            Console.WriteLine("Введите количество файлов, которые хотите соединить с данным (целое число до 10)");
            int size = CheckLimit(10);
            string[] fileNames = new string[size];
            Console.WriteLine("Введите полные пути к файлам.");
            for (int i = 0; i < size; i++)
                fileNames[i] = Console.ReadLine().Trim();
            try
            {
                using (StreamWriter sw = new StreamWriter(path,true,Encoding.Default))
                {
                    foreach (string fileName in fileNames)
                    {
                        sw.WriteLine(File.ReadAllText(fileName));
                    }
                }
                Console.WriteLine($"Файлы успешно объединены, путь : {path}");
                var text = File.ReadAllLines(path, Encoding.UTF8);
                Print(text);
            }
            catch
            {
                Console.WriteLine("Данные файлы нельзя обьединить в один.");
                Console.WriteLine("Проверьте расширение файлов.");
            }
        }


        /// <summary>
        /// Метод, перемещающий файл.
        /// </summary>
        static void MoveFile()
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            string path = ChooseFile();
            if (path == "!!!")
            {
                Console.WriteLine("Создайте новый файл в этой директории или перейдите к выбору диска");
                return;
            }
            Console.WriteLine("Укажите полный путь к директории , где должен лежать файл");
            string path11 = Console.ReadLine().Trim();
            try
            {
                string path2 = Path.Combine(path11, path.Split("/")[^1]);
                File.Move(path, path2);
                Console.WriteLine("Ваш файл перемещен");
            }
            catch
            {
                Console.WriteLine("Пользователь, новый путь некорректен или данный файл нельзя перенести");
            }
            Console.ResetColor();
        }


        /// <summary>
        /// Метод, выводящий файл на экран в выбранной кодировке.
        /// </summary>
        static void PrintFile()
        {
            string path = ChooseFile();
            if (path == "!!!")
            {
                Console.WriteLine("Создайте новый файл в этой директории или перейдите к выбору диска");
                return;
            }
            Console.WriteLine("Выберите кодировку:");
            Console.WriteLine("1. UTF-8");
            Console.WriteLine("2. UNICODE");
            Console.WriteLine("3. ASCII");
            int code = CheckLimit(3);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Предупреждение от системы!");
            Console.WriteLine("Файлы с расширением, отличным от '.txt' могут криво дешифроваться!");
            Console.ResetColor();
            try
            {
                switch (code)
                {
                    case <= 1:
                        var text = File.ReadAllLines(path, Encoding.UTF8);
                        Print(text);
                        break;
                    case <= 2:
                        text = File.ReadAllLines(path, Encoding.Unicode);
                        Print(text);
                        break;
                    case <= 3:
                        text = File.ReadAllLines(path, Encoding.ASCII);
                        Print(text);
                        break;
                }
            }
            catch
            {
                Console.WriteLine("Простите, файл не удалось раскодировать.");
            } 
        }


        /// <summary>
        /// Метод выводит на экран список строк.
        /// </summary>
        /// <param name="list">Список строк </param>
        static void Print(string[] list)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            foreach (var i in list)
               Console.WriteLine(i);
            Console.ResetColor();
        }


        /// <summary>
        /// Метод , выводящий информацию о доступных директориях.
        /// </summary>
        static void Information()
        {
            try
            {
                DriveInfo[] allDrives = DriveInfo.GetDrives();
                if (allDrives.Count() == 0)
                {
                    Console.WriteLine("Увы, здесь нет доступных дисков.");
                    Console.WriteLine("Можем предложить вам:");
                    Console.WriteLine("Создать новый файл или перейти к выбору директории");
                    return;
                }
                List<string> drivers = new List<string>();
                foreach (var driver in allDrives)
                    if (driver.IsReady)
                        drivers.Add(driver.Name);
                Console.WriteLine($"Выберите диск: целое число до {drivers.Count}");
                for (int i = 0; i < drivers.Count; i++)
                    Console.WriteLine($"{i + 1}. {drivers[i]}");
                int length = CheckLimit(drivers.Count);
                Console.WriteLine("Вы перешли в нужный диск.");

                Directory.SetCurrentDirectory(drivers[length - 1]);
            }
            catch
            {
                Console.WriteLine("Вы не можете перейти в этот диск.");
            }
        }


        /// <summary>
        /// Метод, находящий отличия в файлах.
        /// </summary>
        static void Differents()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Введите 2 полных пути к файлам , которые вы хотите сравнить");
            string file1 = Console.ReadLine().Trim();
            string file2 = Console.ReadLine().Trim();
            try
            {
                FileStream fs1 = new FileStream(file1, FileMode.Open);
                FileStream fs2 = new FileStream(file2, FileMode.Open);
                int lenght = (int)Math.Min(fs1.Length, fs2.Length);
                while (--lenght >= 0)
                {
                    var file1byte = fs1.ReadByte();
                    var file2byte = fs2.ReadByte();
                    if (file1byte != file2byte)
                    {
                        Console.WriteLine($"diff {(char)file2byte} -u original {(char)file1byte}");
                    }
                }
                
                if (fs1.Length > fs2.Length)
                {
                    Console.WriteLine($" В конце первого файла еще {fs1.Length - fs2.Length}, которые отсутствуют во втором");
                }
                else
                {
                    Console.WriteLine($" В конце второго файла еще {fs2.Length - fs1.Length}, которые отсутствуют в первом");
                }
                Console.ResetColor();
                fs1.Close();
                fs2.Close();
            }
            catch
            {
                Console.WriteLine("Программа не смогла сравнить файлы.\nВероятная ошибка - неподдерживаемое расширение файла или некорректный путь");
                Console.ResetColor();
            }
        }


        /// <summary>
        /// Основной метод, приводящий все в действие.
        /// </summary>
        static void Main()
        {
            ConsoleKeyInfo KeyToExit;
            do
            {
                int option = 0;
                if (cleaner==0)
                    TalkWithUser();
                while (option < 1 || option > 13)
                {
                    Console.WriteLine("Введите цифру от 1 до 13");
                    int.TryParse(Console.ReadLine(), out option);
                }
                switch (option)
                {
                    case <= 1:
                        Information();
                        break;
                    case <= 2:
                        SetDirectory();
                        break;
                    case <= 3:
                        AllFiles();
                        break;
                    case <= 4:
                        PrintFile();
                        break;
                    case <= 5:
                        CopyFile();
                        break;
                    case <= 6:
                        MoveFile();
                        break;
                    case <= 7:
                        DeleteFile();
                        break;
                    case <= 8:
                        CreateFile();
                        break;
                    case <= 9:
                        ConcatenateFile();
                        break;
                    case <= 10:
                        Maska();
                        break;
                    case <= 11:
                        Maska(true);
                        break;
                    case <= 12:
                        CopyMaska();
                        break;
                    case <= 13:
                        Differents();
                        break;
                }
                Console.WriteLine("Чтобы продолжить нажми любой символ, для выхода нажми escape.");
                KeyToExit = Console.ReadKey(true);
                cleaner++;
                if (cleaner >= 2)
                {
                    Console.Clear(); cleaner = 0;
                }
            } while (KeyToExit.Key != ConsoleKey.Escape);
            Console.WriteLine("До свидания!");
            Console.ReadLine();
        }


        /// <summary>
        /// Декомпозиция для общения с пользователем.
        /// </summary>
        private static void TalkWithUser()
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("Добро пожаловать в файловый менеджер");
            Console.WriteLine("Выберите нужную операцию:");
            Console.WriteLine("1.просмотр списка дисков компьютера и выбор диска;");
            Console.WriteLine("2.переход в другую директорию(выбор папки);");
            Console.WriteLine("3.просмотр списка файлов в директории;");
            Console.WriteLine("4.вывод содержимого текстового файла в консоль в выбранной кодировке (в том числе UTF-8);");
            Console.WriteLine("5.копирование файла;");
            Console.WriteLine("6.перемещение файла в другую директорию;");
            Console.WriteLine("7.удаление файла;");
            Console.WriteLine("8.создание простого текстового файла в выбранной кодировке (в том числе UTF-8);");
            Console.WriteLine("9.конкатенация содержимого двух или более текстовых файлов и вывод результата в консоль в кодировке UTF - 8");
            Console.WriteLine("10.поиск файлов по маске в текущей директории");
            Console.WriteLine("11.поиск файлов по маске в текущей директории и всех её поддиректориях");
            Console.WriteLine("12.копирование всех файлов из директории и всех её поддиректорий по маске в выбранную директорию");
            Console.WriteLine("13.вывод различий между двумя файлами");
            Console.ResetColor();
        }
    }
}