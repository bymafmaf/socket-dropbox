using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    class File
    {
        public string fileName;
        public long fileSize;
        public DateTime uploadTime;

        [JsonIgnore]
        public const int fileBlockSize = 32768;

        public static void downloadTheFile(Stream toWrite, File fileInfo, Socket n, ClientMainWindow caller)
        {
            //same approach with uploading a file but now the client is the recipient.
            Dictionary<string, string> request = new Dictionary<string, string>();
            request.Add("fileProcess", "DOWNLOAD");
            request.Add("fileName", fileInfo.fileName);
            SendMessage(n, JsonConvert.SerializeObject(request));
            Byte[] buffer = new byte[32768];
            int rec = n.Receive(buffer); //thread waits here!

            if (rec <= 0)
            {
                caller.disconnected();
                return;
            }

            string newmessage = Encoding.Default.GetString(buffer);

            newmessage = newmessage.Substring(0, newmessage.IndexOf("\0"));
            File comingFile;
            if (newmessage.Contains("\"fileSize\":"))
            {
                comingFile = JsonConvert.DeserializeObject<File>(newmessage);
                Console.Write("started to get file:\r\n");
                Byte[] fileBuffer = new byte[comingFile.fileSize];
                SendMessage(n, "$SENDFILE#");
                int readBytes = 0;
                int minBlockSize = fileBlockSize < comingFile.fileSize ? fileBlockSize : (int)comingFile.fileSize;
                readBytes += n.Receive(fileBuffer, 0, minBlockSize, SocketFlags.None);
                while (readBytes != comingFile.fileSize)
                {
                    if ((comingFile.fileSize - readBytes) < minBlockSize)
                    {
                        int diff = (int)comingFile.fileSize - readBytes;
                        readBytes += n.Receive(fileBuffer, readBytes, diff, SocketFlags.None);
                    }
                    else
                    {
                        readBytes += n.Receive(fileBuffer, readBytes, minBlockSize, SocketFlags.None);
                    }

                }
                toWrite.Write(fileBuffer, 0, (int)comingFile.fileSize);
                toWrite.Close();
                caller.Invoke((MethodInvoker)(() => caller.writeLogEvent(comingFile.fileName + " is downloaded!")));
                MessageBox.Show("Download is completed!", "Download Progress", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }


        }
        static void SendMessage(Socket client, string message)
        {
            byte[] buffer = Encoding.Default.GetBytes(message);

            //we can send a byte[] 
            client.Send(buffer);
            Console.Write("Your message: " + message + " has been sent.\n");
        }
    }


}
