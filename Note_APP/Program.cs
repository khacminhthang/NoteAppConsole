using System;
// không gian tên collections
using System.Collections.Generic;
using System.IO;
// không gian tên hỗ trợ truy vấn
using System.Linq;
// chứa các lớp đại diện cho mã hóa ký tự ASCII và Unicode
using System.Text;
// lớp tác vụ đại diện cho một hoạt động không đồng bộ
using System.Threading.Tasks;
// hỗ trợ xử lý XML
using System.Xml;
using System.Xml.Linq;
// cho phếp bắt đầu hoặc dừng các chương trình hệ thống
using System.Diagnostics;

namespace Note_APP
{
    class Program
    {
        static void Main(string[] args)
        {
            ReadCommand();
            Console.ReadLine();
         
        }
        // thư mục lưu notes lấy đường dẫn
        private static string NoteDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Notes\";

        private static void ReadCommand()
        {
            //doc input cua nguoi dung
            // in ra o dia hien tai
            Console.Write(Directory.GetDirectoryRoot(NoteDirectory));
            string Command = Console.ReadLine();

            switch (Command.ToLower())
            {
                case "new":
                    {
                        NewNote();
                        Main(null);
                        break;
                    }
                case "edit":
                    {
                        EditNote();
                        Main(null);
                        break;
                    }
                case "read":
                    {
                        ReadNote();
                        Main(null);
                        break;
                    }
                case "delete":
                    {
                        DeleteNote();
                        Main(null);
                        break;
                    }
                case "shownotes":
                    {
                        ShowNotes();
                        Main(null);
                        break;
                    }
                case "exit":
                    {
                        Exit();
                        Main(null);
                        break;
                    }
                default:
                    CommandsAvalable();
                    Main(null);
                    break;
            }
        }

        private static void Exit()
        {
            Environment.Exit(0);
        }

        private static void CommandsAvalable()
        {
            Console.WriteLine(" New - Create a new note\n Edit - Edit a note\n Read -  Read a note\n ShowNotes - List all notes\n Exit - Exit the application\n Dir - Opens note directory\n Help - Shows this help message\n");
        }

        private static void ShowNotes()
        {
            string NoteLocation = NoteDirectory;
            DirectoryInfo dir = new DirectoryInfo(NoteLocation);
            if (Directory.Exists(NoteLocation))
            {
                FileInfo[] noteFiles = dir.GetFiles("*.xml");
                if(noteFiles.Count() != 0)
                {
                    // dat vi tri con tro
                    Console.SetCursorPosition(Console.CursorLeft + 1, Console.CursorTop + 2);
                    Console.WriteLine("+------------------+");
                    foreach(var item in noteFiles)
                    {
                        Console.WriteLine("   " + item.Name);
                    }
                    // lay chuoi dong moi
                    Console.WriteLine(Environment.NewLine);
                }
                else
                {
                    Console.WriteLine("No notes found.\n");
                }
            }
            else
            {
                Console.WriteLine("Directory does not exist");
            }
        }

        private static void DeleteNote()
        {
            Console.WriteLine("Please enter file name\n");
            string FileName = Console.ReadLine();

            if (File.Exists(NoteDirectory + FileName))
            {
                Console.WriteLine(Environment.NewLine + "Are you sure you wish to delete this file? Y/N\n");
                string Confirmation = Console.ReadLine().ToLower();

                if (Confirmation == "y")
                {
                    try
                    {
                        File.Delete(NoteDirectory + FileName);
                        Console.WriteLine("File has been deleted\n");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("File not deleted following error occured: " + ex.Message);
                    }
                }
                else if (Confirmation == "n")
                {
                    Main(null);
                }
                else
                {
                    Console.WriteLine("Invalid command\n");
                    DeleteNote();
                }
            }
            else
            {
                Console.WriteLine("File does not exist\n");
                DeleteNote();
            }
        }

        private static void ReadNote()
        {
            Console.WriteLine("Please enter file name");
            string fileName = Console.ReadLine();

            if(File.Exists(NoteDirectory + fileName))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(NoteDirectory + fileName);
                Console.WriteLine(doc.SelectSingleNode("//body").InnerText);

            }
            else
            {
                Console.WriteLine("file not found");
            }
        }

        private static void EditNote()
        {
            Console.WriteLine("Please enter file name \n");
            string fileName = Console.ReadLine();

            if(File.Exists(NoteDirectory + fileName))
            {
                XmlDocument doc = new XmlDocument();

                // load the document
                try
                {
                    doc.Load(NoteDirectory + fileName);
                    // lay noi dung note
                    Console.Write(doc.SelectSingleNode("//body").InnerText);
                    string ReadInput = Console.ReadLine();
                    if(ReadInput.ToLower() == "cancel")
                    {
                        Main(null);
                    }
                    else
                    {
                        string newText = doc.SelectSingleNode("//body").InnerText = ReadInput;
                        doc.Save(NoteDirectory + fileName);
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine("can not edit note");
                }
            }
            else
            {
                Console.WriteLine("File not found");
            }
        }

        private static void NewNote()
        {
            Console.WriteLine("Please enter notes: \n");
            string input = Console.ReadLine();
            // them xml setting ho tro viet xml
            XmlWriterSettings NoteSettings = new XmlWriterSettings();
            NoteSettings.CheckCharacters = false;
            NoteSettings.ConformanceLevel = ConformanceLevel.Auto;
            NoteSettings.Indent = true;

            string FileName = DateTime.Now.ToString("dd/MM/yyyyhhmmss") + ".xml";

            // viet file
            using (XmlWriter NewNote = XmlWriter.Create(NoteDirectory + FileName, NoteSettings))
            {
                NewNote.WriteStartDocument();
                NewNote.WriteStartElement("Note");
                NewNote.WriteElementString("body", input);
                NewNote.WriteEndElement();

                NewNote.Flush();
                NewNote.Close();
            }

        }

    }
}
