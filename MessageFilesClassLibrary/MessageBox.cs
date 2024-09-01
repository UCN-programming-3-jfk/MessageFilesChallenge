using System;
using System.Collections.Generic;
using System.IO;

namespace MessageFilesClassLibrary
{
    public class MessageBox
    {
        public string MessageDirectoryPath { get; set; }

        public MessageBox(string messageFolder)
        {
            if (!Directory.Exists(messageFolder))
            {
                throw new ArgumentException($"Unable to initialize MessageFiles Folder '{messageFolder}' does not exist. ");
            }
            MessageDirectoryPath = messageFolder;
        }

        public List<Message> GetMessages()
        {
            List<Message> messages = new List<Message>();
            foreach (var filePath in Directory.GetFiles(MessageDirectoryPath, "*.txt"))
            {
                messages.Add(Message.MessageFromTextFile(filePath));
            }
            return messages;
        }

        public void Cleanup()
        {
            string backupFolder = Path.Combine(MessageDirectoryPath, "_backup");
            if (!Directory.Exists(backupFolder)) { Directory.CreateDirectory(backupFolder); }
            string destinationFilePath = null;
            foreach (var pathToFileToBackup in Directory.GetFiles(MessageDirectoryPath, "*.backup"))
            {
                try
                {
                    string fileNameWithoutFolder = Path.GetFileName(pathToFileToBackup);
                    destinationFilePath = Path.Combine(backupFolder, fileNameWithoutFolder);
                    File.Move(pathToFileToBackup, destinationFilePath);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error moving file '{pathToFileToBackup}' to '{destinationFilePath}'. Error was '{ex.Message}'");
                }
            }
        }

        public void SortMessages()
        {
            foreach (var message in GetMessages())
            {
                string sortFolderForMessage = CreateFolderForMessage(message);
                MoveMessage(message, sortFolderForMessage);
            }
        }

        private string CreateFolderForMessage(Message message)
        {
            string destinationDirectory = Path.Combine(MessageDirectoryPath, message.Subject.Substring(0, 1));

            try
            {
                Directory.CreateDirectory(destinationDirectory);
                return destinationDirectory;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating sorting directory '{destinationDirectory}' for file '{message.MessageFilePath}'. Error was '{ex.Message}'");
            }
            
        }
        private void MoveMessage(Message message, string sortFolderForMessage)
        {
            try
            {
                string newFilePath = Path.Combine(sortFolderForMessage, Path.GetFileName(message.MessageFilePath));
                File.Move(message.MessageFilePath, newFilePath);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error moving file '{message.MessageFilePath}' to sortingdirectory '{sortFolderForMessage}'. Error was '{ex.Message}'");
            }
        }
    }
}