using System;
using System.IO;
using System.IO.Compression;

namespace Homework6
{
    class Program
    {
        static int N;
        static bool isReadN = false;
        static string path = Environment.CurrentDirectory;

        /// <summary>
        /// Проверка на степень 2
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        static bool IsPowerOfTwo(int number)
        {
            return number > 0 && (number & -number) == number;
        }
        /// <summary>
        /// Чтение из файла количества чисел
        /// </summary>
        /// <returns></returns>
        static bool ReadN()
        {
            string[] fileText = File.ReadAllLines(Path.Combine(path,"test.txt"));

            if (fileText.Length != 0 && File.Exists(Path.Combine(path,"test.txt")))
            {
                isReadN = true;
                N = int.Parse(fileText[0]);
                return isReadN;
            }
            else return isReadN;
        }
        /// <summary>
        /// Запись в файл
        /// </summary>
        static void SolveGroupInFile()
        {
            int count = 1;
            StreamWriter result = new StreamWriter(Path.Combine(path, "result.txt"));
            for (int i = 1; i < N; i++)
            {
                if (IsPowerOfTwo(i))
                {
                    result.WriteLine(i + " " + "---Группа №"+ count);
                    count++;
                }
                else if (i == N - 1)
                {
                    result.WriteLine(i + " " + "---Группа №" + count);
                }
                else
                {
                    result.Write(i + " ");
                    continue;
                }
            }
            result.Dispose();
            result.Close();

            Compress(Path.Combine(path, "result.txt"), Path.Combine(path, "result.zip"));
        }
        /// <summary>
        /// Сжатие файла
        /// </summary>
        /// <param name="sourceFile"></param>
        /// <param name="compressedFile"></param>
        public static void Compress(string sourceFile, string compressedFile)
        {
            var startTime = System.Diagnostics.Stopwatch.StartNew();
            string elapsedTime;
            // поток для чтения исходного файла
            using (FileStream sourceStream = new FileStream(sourceFile, FileMode.OpenOrCreate))
            {
                // поток для записи сжатого файла
                using (FileStream targetStream = File.Create(compressedFile))
                {
                    // поток архивации
                    using (GZipStream compressionStream = new GZipStream(targetStream, CompressionMode.Compress))
                    {
                        sourceStream.CopyTo(compressionStream); // копируем байты из одного потока в другой
                        Console.WriteLine();
                        Console.WriteLine("Сжатие файла {0} завершено. Исходный размер: {1}  сжатый размер: {2} байт.",
                            sourceFile, sourceStream.Length.ToString(), targetStream.Length.ToString());
                    }
                }
            }
            startTime.Stop();
            var resultTime = startTime.Elapsed;

            // elapsedTime - строка, которая будет содержать значение затраченного времени
            elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:000}",
                resultTime.Hours,
                resultTime.Minutes,
                resultTime.Seconds,
                resultTime.Milliseconds);
            Console.WriteLine("Время операции сжатия: " + elapsedTime);
        }

        static int SolveGroup(int N)
        {
            int countGroup = 0;
            for (int i = 1; i < N; i++)
            {
                if (IsPowerOfTwo(i))
                {
                    countGroup++;
                }
                else if (i == N - 1)
                {
                    countGroup++;
                }
                else continue;
            }
            return countGroup;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            if(ReadN()) Console.WriteLine("Чтение прошло успешно");
            
            Console.WriteLine("1) Рассчитать все группы и записать в файл result.txt");
            Console.WriteLine("2) Кол-во групп для заданного N");
            Console.WriteLine("Выберите 1 или 2");
            if(Console.ReadKey().KeyChar=='1')
            {
                SolveGroupInFile();
            }
            else if(Console.ReadKey().KeyChar == '2')
            {
                Console.WriteLine("Кол-во групп: {0}", SolveGroup(N));
            }
            Console.ReadKey();
        }
    }
}
