﻿namespace _10.Files_and_Streams
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.IO;                //Задължително, когато работим с файлове.
    using System.Net.Sockets;
    using System.Net;

    public class FilesAndStreams
    {
        public static void Main(string[] args)
        {
            //**********************************************************************************************************************
            //Streams:
            //**********************************************************************************************************************



            //Methods of System.IO.Stream Class:

            //int Read(byte[] buffer, int offset, int count)
            //Read as many as count bytes from input stream, starting from the given offset position.
            //Returns the number of read bytes or 0 if end of stream is reached.
            //Can freeze for undefined time while reads at least 1 byte.
            //Can read less than the claimed number of bytes.

            //int Write(byte[] buffer, int offset, int count)
            //Writes to output stream sequence of count bytes, starting from the given offset position.
            //Can freeze for undefined time, until send all bytes to their destination.

            //Flush()
            //Sends the internal buffers data to its destination (data storage, I/O device, etc.)

            //Close() 
            //Calls Flush()
            //Closes the connection to the device(mechanism).
            //Releases the used resources.

            //Seek(offset, SeekOrigin)
            //Moves the position (if supported) with given offset towards the beginning, the end or the current position.



            //The FileStream Class:
            //FileStream fs = new FileStream(string fileName, FileMode [, FileAccess[, FileShare]]);

            //FileMode – opening file mode
            //Open, Append, Create, CreateNew, OpenOrCreate, Truncate

            //FileAccess – operations mode for the file
            //Read, Write, ReadWrite

            //FileShare – access rules for other users while file is opened
            //None, Read, Write, ReadWrite

            //**********************************************************************************************************************
            //Writing Text to File – Example:
            string text = "Кирилица";
            FileStream fileStream = new FileStream("../../log.txt", FileMode.Create);

            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(text);
                fileStream.Write(bytes, 0, bytes.Length);
            }
            finally
            {
                fileStream.Close();
            }

            //**********************************************************************************************************************
            //Copying File – Example:
            using (FileStream source = new FileStream("image.bmp", FileMode.Open))
            {
                using (FileStream destination = new FileStream("newmage.bmp", FileMode.Create))
                {
                    byte[] buffer = new byte[5];

                    while (true)
                    {
                        int readBytes = source.Read(buffer, 0, buffer.Length);
                        if (readBytes == 0)
                            break;
                        destination.Write(buffer, 0, readBytes);
                    }
                }
            }

            //**********************************************************************************************************************
            //Memory stream. Reading In-Memory String – Example:
            string text2 = "In-memory text.";
            byte[] bytes2 = Encoding.UTF8.GetBytes(text);

            using (MemoryStream memoryStream = new MemoryStream(bytes2))
            {
                while (true)
                {
                    int readByte = memoryStream.ReadByte();
                    if (readByte == -1)
                        break;
                    Console.WriteLine((char)readByte);
                }
            }

            //**********************************************************************************************************************
            //Network stream. Simple Web Server – Example:
            TcpListener tcpListener = new TcpListener(IPAddress.Any, 80);
            tcpListener.Start();
            Console.WriteLine("Listening on port {0}...", 80);

            while (true)
            {
                using (NetworkStream stream = tcpListener.AcceptTcpClient().GetStream())
                {
                    byte[] request = new byte[4096];
                    stream.Read(request, 0, 4096);
                    Console.WriteLine(Encoding.UTF8.GetString(request));

                    string html = string.Format("{0}{1}{2}{3} - {4}{2}{1}{0}",
                        "<html>", "<body>", "<h1>", "Welcome to my awesome site!", DateTime.Now);
                    byte[] htmlBytes = Encoding.UTF8.GetBytes(html);
                    stream.Write(htmlBytes, 0, htmlBytes.Length);
                }
            }

            //**********************************************************************************************************************
            //Files:
            //**********************************************************************************************************************



            //Read File:

            using (StreamReader reader = new StreamReader("somefile.txt"))
            {
                int lineNumber = 0;
                string line = reader.ReadLine();

                while (line != null)
                {
                    lineNumber++;
                    Console.WriteLine("Line {0}: {1}", lineNumber, line);
                    line = reader.ReadLine();
                }
            }

            //**********************************************************************************************************************
            //Write to File:

            //Writing Reversed Text to File – Example:
            using (StreamReader reader2 = new StreamReader("somefile.txt"))
            {
                using (StreamWriter writer = new StreamWriter("reversed.txt"))
                {
                    string line = reader2.ReadLine();

                    while (line != null)
                    {
                        for (int i = line.Length - 1; i >= 0; i--)
                        {
                            writer.Write(line[i]);
                        }

                        writer.WriteLine();

                        line = reader2.ReadLine();
                    }
                }
            }

            //**********************************************************************************************************************

            if (!File.Exists("myfile.txt"))     //Проверка дали файла съществува.
            {
                File.Create("myfile.txt");      //Създаване на файл.
            }

            string file = File.ReadAllText("myfile.txt");               //reads everything at once and returns a string
            string[] file1 = File.ReadAllLines("myfile.txt");           //reads line by line and returns a collection. Прочита целият файл на куп.
            IEnumerable<string> file3 = File.ReadLines("myfile.txt");   //Чете линия по линия, а не целият файл на куп.

            Console.WriteLine(file);

            foreach (var line in file1)
            {
                Console.WriteLine(line);
            }

            //**********************************************************************************************************************

            //WriteAllText() – takes a string and writes it to a file - пише върху старото съдържание.
            File.WriteAllText("myfile.txt", "Files are fun");

            //WriteAllLines() – takes a collection and writes every element on a new line - пише върху старото съдържание.
            string[] names = { "pesho", "ivan", "stamat", "mariika" };
            File.WriteAllLines("myfile.txt", names);

            Console.WriteLine();

            //There are also AppendAllText() and AppendAllLines() methods, that just add additional text to a file.
            //При Append не се трие старото съдържание, а се добавя новото отдолу. 

            //FileInfo
            var file2 = "myfile.txt";

            var fileinfo = new FileInfo(file2);

            Console.WriteLine(fileinfo.FullName);       //C:\Users\ilian\OneDrive\Projects\01.Learning_C_Sharp\Работа_с_файлове\bin\Debug\myfile.txt
            Console.WriteLine(fileinfo.Name);           //myfile.txt
            Console.WriteLine(fileinfo.Length);         //30        - Ако го разделим на 1024.0 ще го получим в Mb.
            Console.WriteLine(fileinfo.Attributes);     //Archive
            Console.WriteLine(fileinfo.Directory);      //C:\Users\ilian\OneDrive\Projects\01.Learning_C_Sharp\Работа_с_файлове\bin\Debug
            Console.WriteLine(fileinfo.DirectoryName);  //C:\Users\ilian\OneDrive\Projects\01.Learning_C_Sharp\Работа_с_файлове\bin\Debug - това обаче е стринг!
            Console.WriteLine(fileinfo.Exists);         //True
            Console.WriteLine(fileinfo.Extension);      //.txt

            //Местене на файл.
            File.Move(file2, $"../{file2}");             //Премести файла едно ниво нагоре във файловата система и го кръсти отново file2

            //Преименуване на файл.
            File.Move(file2, "anothername.txt");

            //Изстриване на файл.
            File.Delete("text.txt");

            //**********************************************************************************************************************
            //Директории:
            //CreateDirectory() – creates the directory and all subdirectories at the specified path, unless they already exist
            Directory.CreateDirectory("Test");

            //Delete() – deletes an empty directory
            Directory.Delete("Test");

            //Delete() – deletes a non empty directory
            Directory.Delete("Test", true);

            //Move() – moves a file or directory to a new location 
            Directory.Move("Test", "New Folder");

            //GetFiles() – returns the names of files (including their paths) in the specified directory 
            string[] filesInDir = Directory.GetFiles("TestFolder");

            //GetDirectories() – returns the names of subdirectories (including their paths) in the specified directory
            string[] subDirs = Directory.GetDirectories("TestFolder");

            //Начин да обиколим всички под-директории и файлове в един рут.
            string currentDir = Directory.GetCurrentDirectory();    //Връща пътя на директорията в която се изпълнява програмата.

            DirectoryInfo dirInfo = new DirectoryInfo(currentDir);

            DirectoryInfo rootDir = dirInfo.Parent.Parent;          //Качвам се две стъпки нагоре в директориите.

            TravereseDir(rootDir);                                  //Метод
        }

        public static void TravereseDir(DirectoryInfo currentDir, string prefix = "")
        {
            foreach (var dir in currentDir.GetDirectories())        //Обикалям всички директории в рута. 
            {
                Console.WriteLine(prefix + dir.Name);
                TravereseDir(dir, prefix + "--");
            }

            foreach (var file in currentDir.GetFiles())             //Обикалям всички файлове в съответната директория.
            {
                Console.WriteLine(prefix + "--" + file.Name);
            }
        }
    }
}
