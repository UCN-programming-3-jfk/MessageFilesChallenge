using MessageFilesClassLibrary;
using System;
using System.Collections.Generic;
using System.IO;

namespace MessageFilesConsoleApp
{
    class Program
    {
        public enum Operation
        {
            PrintMessages, Cleanup, Sort
        }

        static void Main(string[] args)
        {
            InspectArgumentsAndPerformActions(args);
        }

        private static void InspectArgumentsAndPerformActions(string[] args)
        {
            switch (args.Length)
            {
                case 0:
                    WriteInstructions();
                    break;
                case 2:
                    if (CheckArguments(args))
                    {
                        PerformActions(args[0], (Operation)Enum.Parse(typeof(Operation), args[1], true));
                    }
                    break;
                default:
                    Console.WriteLine("Only two arguments are supported. If you have spaces in your message folder path, surround the path argument with quotes.");
                    WriteInstructions();
                    break;
            }
        }

        private static void WriteInstructions()
        {
            Console.WriteLine("COMMAND LINE MESSAGEFILE APPLICATION");
            Console.WriteLine("\tUSAGE: MessageHelper.exe [message folder path] [operation]");
            Console.WriteLine($"\tEXAMPLE: MessageHelper.exe C:\\temp\\messages cleanup");
            Console.WriteLine($"Operations: ");
            Console.WriteLine($"\tprintmessages  - prints all messages in the message folder and renames them from *.txt to *.backup ");
            Console.WriteLine($"\tbackup         - moves all *.backup files to a subfolder '_backup'");
            Console.WriteLine($"\tsort           - sorts all *.txt messages into folders by their starting letter.");
        }

        private static bool CheckArguments(string[] args)
        {
            string folderPath = args[0];
            bool isValidFolderPath = Directory.Exists(folderPath);
            if (!isValidFolderPath)
            {
                Console.WriteLine($"The path '{folderPath}' was not found. Terminating.");
                return false;
            }
            string operationString = args[1].ToLower();
            bool isValidCommand = Enum.TryParse(typeof(Operation), operationString, true, out object operation);

            if (!isValidCommand)
            {
                Console.WriteLine($"The operation '{operationString}' was not recognized. Terminating.");
                return false;
            }

            return true;
        }

        private static void PerformActions(string messagefolderPath, Operation operationToPerform)
        {
            MessageBox messagebox = new MessageBox(messagefolderPath);
            switch (operationToPerform)
            {
                case Operation.PrintMessages:
                    Console.WriteLine($"Printing messages...");
                    PrintMessages(messagebox);
                    break;

                case Operation.Cleanup:
                    Console.WriteLine($"Cleaning up...");
                    messagebox.Cleanup();
                    break;

                case Operation.Sort:
                    Console.WriteLine($"Sorting messages...");
                    messagebox.SortMessages();
                    break;

                default:
                    Console.WriteLine($"Unknown operation: '{operationToPerform}'");
                    break;
            }
        }

        private static void PrintMessages(MessageBox messagebox)
        {
            List<Message> messages = messagebox.GetMessages();
            if (messages.Count == 0) { Console.WriteLine($"No messages"); }
            else 
            {
                foreach (var message in messages)
                {
                    Console.WriteLine(message.ToString());
                    message.RenameSourceFileToBackup();
                    Console.WriteLine("     ===============     ");
                }
            }
        }
    }
}