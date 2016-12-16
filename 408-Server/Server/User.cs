using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server
{
    public class User
    {
        public string username { get; set; }
        public List<File> fileList = new List<File>();

        [JsonIgnore]
        public Socket connectionSocket;
        [JsonIgnore]
        public string directoryToSave;
        [JsonIgnore]
        static public int fileBlockSize = 32768;
        [JsonIgnore]
        public ServerMainWindow mainWindow;
        public User(ServerMainWindow caller)
        {
            mainWindow = caller;
        }
        public void connection(Socket n, string folderPath)
        {
            connectionSocket = n;
            directoryToSave = folderPath;
            Thread thrReceive;
            thrReceive = new Thread(Receive);
            thrReceive.IsBackground = true;
            thrReceive.Start(this);
        }

        public void writeLogEvent(string message)
        {
            mainWindow.Invoke((MethodInvoker)(() => mainWindow.getLogListBox().Items.Add(message)));
            //mainWindow.getLogListBox().Items.Add(message);
        }

        public void scanUserFolder()
        {
            string[] filePaths = Directory.GetFiles(directoryToSave);
            fileList.Clear();
            foreach (string filePath in filePaths)
            {
                
                File aFile = new File(filePath);
                fileList.Add(aFile);
                
            }
        }
        public void setUsername (string name)
        {
            username = name;
            directoryToSave += "\\" + name;
            Directory.CreateDirectory(directoryToSave);
            ServerMainWindow.connectedUsers.Add(this);
        }

        public bool doesUserExist (User theUser)
        {
            foreach(User aUser in ServerMainWindow.connectedUsers)
            {
                if (aUser.username == theUser.username)
                    return true;
            }
            return false;
        }

        static private void Receive(object user)
        {
            User theUser = (User)user;
            bool connected = true;
            //acceptTemp = null;
            while (connected)
            {
                try
                {

                    Byte[] buffer = new byte[32768];
                    int rec = theUser.connectionSocket.Receive(buffer); //thread waits here!

                    if (rec <= 0)
                    {
                        throw new SocketException();
                        
                    }

                    string newmessage = Encoding.Default.GetString(buffer);
                   
                    newmessage = newmessage.Substring(0, newmessage.IndexOf("\0"));

                    //login is requested
                    if (newmessage.Contains("{\"username\""))
                    {
                        User newUser = JsonConvert.DeserializeObject<User>(newmessage);
                        Dictionary<String, String> status = new Dictionary<string, string>();
                        if (theUser.doesUserExist(newUser))
                        {
                            status.Add("ConnectionStatus", "ERROR");
                        } else
                        {
                            status.Add("ConnectionStatus", "OK");
                            theUser.writeLogEvent(newUser.username + " is connected.");
                            theUser.setUsername(newUser.username);
                        }
                        SendMessage(theUser.connectionSocket, JsonConvert.SerializeObject(status));
                        

                    }
                    //update about user info is requested
                    else if (newmessage.Contains("{\"userInfo\":\"GET\""))
                    {
                        theUser.scanUserFolder();
                        string userinfo = JsonConvert.SerializeObject(theUser);
                        SendMessage(theUser.connectionSocket, userinfo);
                    }
                    //a process is requested for a file that already exists
                    else if (newmessage.Contains("{\"fileProcess\""))
                    {
                        if (newmessage.Contains("RENAME"))
                        {
                            Dictionary<string, string> dic = JsonConvert.DeserializeObject<Dictionary<string, string>>(newmessage);
                            File theFile = new File(theUser, dic["oldFileName"]);

                            if (theFile.rename(dic["newFileName"]))
                            {
                                theUser.writeLogEvent(dic["oldFileName"] + " is renamed to " + dic["newFileName"]);
                                SendMessage(theUser.connectionSocket, "SUCCESS");
                            } else
                            {
                                SendMessage(theUser.connectionSocket, "ERROR");
                            }
                            
                            
                        }
                        else if (newmessage.Contains("DELETE"))
                        {
                            Dictionary<string, string> dic = JsonConvert.DeserializeObject<Dictionary<string, string>>(newmessage);
                            File theFile = File.getFromList(theUser.fileList, dic["fileName"]);
                            theFile.delete();
                            theUser.fileList.Remove(theFile);
                            theUser.writeLogEvent(dic["fileName"] + " is deleted");
                        } else if (newmessage.Contains("DOWNLOAD"))
                        {
                            Dictionary<string, string> dic = JsonConvert.DeserializeObject<Dictionary<string, string>>(newmessage);
                            File theFile = File.getFromList(theUser.fileList, dic["fileName"]);

                            string info = JsonConvert.SerializeObject(theFile);
                            SendMessage(theUser.connectionSocket, info);

                            Byte[] messageBuffer = new byte[32768];
                            int count = theUser.connectionSocket.Receive(messageBuffer); //thread waits here!

                            if (count <= 0)
                            {
                                throw new SocketException();
                            }

                            string confirmation = Encoding.Default.GetString(messageBuffer);
                            confirmation = confirmation.Substring(0, confirmation.IndexOf("\0"));

                            if (confirmation.Contains("$SENDFILE#"))
                            {
                                theUser.connectionSocket.SendFile(theFile.filePath);
                            }
                            theUser.writeLogEvent(dic["fileName"] + " has been downloaded");

                        }
                    }
                    //we will get a file
                    else if (newmessage.Contains("{\"fileName\""))
                    {
                        File comingFile = JsonConvert.DeserializeObject<File>(newmessage);


                        Console.Write("started to get file:\r\n");
                        Byte[] fileBuffer = new byte[comingFile.fileSize];
                        theUser.writeLogEvent(comingFile.fileName + " started to be uploaded with size " + comingFile.fileSize.ToString() + " byte.");
                        SendMessage(theUser.connectionSocket, "$SENDFILE#");
                        int readBytes = 0;
                        int minBlockSize = fileBlockSize < comingFile.fileSize ? fileBlockSize : (int)comingFile.fileSize;
                        readBytes += theUser.connectionSocket.Receive(fileBuffer, 0, minBlockSize, SocketFlags.None);
                        while (readBytes != comingFile.fileSize)
                        {
                            if ((comingFile.fileSize - readBytes) < minBlockSize)
                            {
                                int diff = (int)comingFile.fileSize - readBytes;
                                readBytes += theUser.connectionSocket.Receive(fileBuffer, readBytes, diff, SocketFlags.None);
                            }
                            else
                            {
                                readBytes += theUser.connectionSocket.Receive(fileBuffer, readBytes, minBlockSize, SocketFlags.None);
                            }

                        }



                        System.IO.File.WriteAllBytes(theUser.directoryToSave + "\\" + comingFile.fileName, fileBuffer);
                        theUser.writeLogEvent(comingFile.fileName + " completed uploading on " + DateTime.Now.ToLongTimeString());
                        theUser.fileList.Add(comingFile);
                        Dictionary<String, String> status = new Dictionary<string, string>();
                        status.Add("UploadStatus", "OK");
                        SendMessage(theUser.connectionSocket, JsonConvert.SerializeObject(status));

                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine("exception: " + e.ToString());
                    theUser.writeLogEvent(theUser.username + " has disconnected");
                    theUser.disconnection();

                    connected = false;
                }


            }
        }

        public void disconnection()
        {
            ServerMainWindow.connectedUsers.Remove(this);
            connectionSocket.Close();
        }

        
        static void SendMessage(Socket n, string message)
        {
            byte[] buffer = Encoding.Default.GetBytes(message);

            //we can send a byte[] 
            n.Send(buffer);
            Console.Write("Your message: " + message + " has been sent.\n");
        }
    }
}
