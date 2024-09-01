using System;
using System.IO;

namespace MessageFilesClassLibrary
{
    public class Message
    {
        public string Subject { get; set; }
        public string MessageText { get; set; }
        public string MessageFilePath { get; set; }

        public Message(string subject, string messageText)
        {
            Subject = subject;
            MessageText = messageText;
        }
        public void RenameSourceFileToBackup()
        {
            string filenameWithoutExtension = Path.GetFileNameWithoutExtension(MessageFilePath);
            string newFileNameWithBackupExtension = filenameWithoutExtension + ".backup";
            string directory = Path.GetDirectoryName(MessageFilePath);
            string destinationBackupFilePath = Path.Combine(directory, newFileNameWithBackupExtension);
            try
            {
                File.Move(MessageFilePath, destinationBackupFilePath);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error renaming '{MessageFilePath}' to '{destinationBackupFilePath}'. Error was: '{ex.Message}'");
            }

        }

        public static Message MessageFromTextFile(string pathToMessageFile)
        {
            try
            {
                string filename = Path.GetFileNameWithoutExtension(pathToMessageFile);
                string fileContents = File.ReadAllText(pathToMessageFile);
                return new Message(filename, fileContents) { MessageFilePath = pathToMessageFile};
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating Message object from reading contents of '{pathToMessageFile}'. Error was: '{ex.Message}'");
            }
        }

        public override string ToString()
        {
            return Subject.ToUpper() + Environment.NewLine + MessageText;
        }
    }
}