using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace Client
{
    public partial class ClientMainWindow : Form
    {
        static Socket client;
        static User currentUser = new User();
        static File currentFile = new File();
        Stream currentFileStream;
        public ClientMainWindow()
        {
            InitializeComponent();
            uploadedFilesList.View = View.Details;
            uploadedFilesList.Columns.Add("Name");
            uploadedFilesList.Columns.Add("Size");
            uploadedFilesList.Columns.Add("Upload Time");
            uploadedFilesList.GridLines = true;

            serverIPAdressText.Text = "127.0.0.1";
            portNumberInteger.Value = 1;
            usernameText.Text = "muvaffak";
            currentUser.username = usernameText.Text;

            uploadButton.Enabled = false;
            disconnectButton.Enabled = false;
            listFilesButton.Enabled = false;
        }

        private void connectButton_Click(object sender, EventArgs e)
        {

            string serverIP;
            int serverPort;

            try
            {
                serverIP = serverIPAdressText.Text;

                serverPort = (int)portNumberInteger.Value;
                client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                client.Connect(serverIP, serverPort);

                string userInfoMessage = JsonConvert.SerializeObject(currentUser);
                SendMessage(userInfoMessage);

                //wait for the confirmation of login
                Byte[] buffer = new byte[64];
                int rec;
                try
                {
                    rec = client.Receive(buffer); //thread waits here!
                }
                catch
                {
                    disconnected();
                    return;
                }
                if (rec <= 0)
                {
                    disconnected();
                    return;
                }
                string serverResponse = Encoding.Default.GetString(buffer);
                Dictionary<string, string> responseStatus = JsonConvert.DeserializeObject<Dictionary<string, string>>(serverResponse);
                if (responseStatus["ConnectionStatus"] != "OK")
                {
                    MessageBox.Show("A user with the same name is already connected.", "Connection Problem", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    disconnected();
                } else
                {
                    connected();
                }
            }
            catch
            {
                MessageBox.Show("Couldn't connect to the server.", "Connection Problem", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            }
        }

        //necessary UI changes.
        public void connected()
        {
            uploadButton.Enabled = true;
            disconnectButton.Enabled = true;
            connectButton.Enabled = false;
            renameButton.Enabled = true;
            deleteButton.Enabled = true;
            downloadButton.Enabled = true;
            listFilesButton.Enabled = true;
            writeLogEvent("Connected!");
        }
        public void disconnected()
        {
            uploadButton.Enabled = false;
            disconnectButton.Enabled = false;
            connectButton.Enabled = true;
            renameButton.Enabled = false;
            deleteButton.Enabled = false;
            downloadButton.Enabled = false;
            listFilesButton.Enabled = false;
            writeLogEvent("Disconnected!");
        }

        //common function to send a message to the server
        public void SendMessage(string message)
        {
            byte[] buffer = Encoding.Default.GetBytes(message);

            //we can send a byte[] 
            try
            {
                client.Send(buffer);
            }
            catch 
            {
                MessageBox.Show("Server out of reach!", "Server Closed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                disconnected();
            }

            Console.Write("Your message: " + message + " has been sent.\n");
        }

        private void uploadButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((currentFileStream = openFileDialog1.OpenFile()) != null)
                    {
                        currentFile.fileName = openFileDialog1.SafeFileName;
                        currentFile.fileSize = new FileInfo(openFileDialog1.FileName).Length;

                        //send file info so that server allocates necessary memory
                        SendMessage( JsonConvert.SerializeObject(currentFile) );

                        Dictionary<string, object> info = new Dictionary<string, object>();
                        info.Add("fileName", openFileDialog1.FileName);
                        info.Add("caller", this);
                        Thread fileThread = new Thread(sendFile);
                        fileThread.Start(info);

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        static private void sendFile(object info)
        {
            Dictionary<string, object> callerInfo = (Dictionary<string, object>) info;
            //wait for server to acknowledge file with its properties
            ClientMainWindow caller = (ClientMainWindow)callerInfo["caller"];
            Byte[] buffer = new byte[64];
            int rec;
            try
            {
                rec = client.Receive(buffer); //thread waits here!
            }
            catch
            {
                caller.disconnected();
                return;
            }
            if (rec <= 0)
            {
                caller.disconnected();
                return;
            }
            string serverResponse = Encoding.Default.GetString(buffer);
            
            if (serverResponse.Contains("$SENDFILE#"))
            {
                string fileName = (string)callerInfo["fileName"];
                //start to send the actual file
                client.SendFile(fileName);
                waitUploading(callerInfo);
            }
        }

        static private void waitUploading(Dictionary<string, object> callerInfo)
        {
            ClientMainWindow caller = (ClientMainWindow)callerInfo["caller"];
            string path = (string)callerInfo["fileName"];
            FileInfo info = new FileInfo(path);
            string filename = info.Name;
            //wait for confirmation of upload.
            Byte[] buffer = new byte[64];
            int rec;
            try
            {
                rec = client.Receive(buffer); //thread waits here!
            }
            catch
            {
                caller.disconnected();
                return;
            }

            if (rec <= 0)
            {
                caller.disconnected();
                return;
            }
            string serverResponse = Encoding.Default.GetString(buffer);
            Dictionary<string, string> responseStatus = JsonConvert.DeserializeObject<Dictionary<string, string>>(serverResponse);
            if (responseStatus["UploadStatus"] == "OK")
            {
                caller.Invoke((MethodInvoker)(() => caller.writeLogEvent(filename + " is uploaded!")));
                MessageBox.Show("Upload completed!", "Upload Progress", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void usernameText_TextChanged(object sender, EventArgs e)
        {
            currentUser.username = usernameText.Text;
        }

        private void disconnectButton_Click(object sender, EventArgs e)
        {
            client.Disconnect(false);
            disconnected();
        }

        private void listFilesButton_Click(object sender, EventArgs e)
        {
            
            fileListUpdate();
        }

        private void fileListUpdate()
        {
            Dictionary<String, String> status = new Dictionary<string, string>();
            status.Add("userInfo", "GET");
            SendMessage(JsonConvert.SerializeObject(status));
            Byte[] buffer = new byte[32768];
            int rec;
            try
            {
                rec = client.Receive(buffer); //thread waits here!
            }
            catch
            {
                disconnected();
                return;
            }
            
            if (rec <= 0)
            {
                disconnected();
                return;
            }
            string serverResponse = Encoding.Default.GetString(buffer);
            User myInfo = JsonConvert.DeserializeObject<User>(serverResponse);
            setCurrentUser(myInfo);
            writeLogEvent("file list is updated.");
        }
        private void setCurrentUser (User newInfo)
        {
            //when user updated we have to update the UI
            currentUser.username = newInfo.username;
            currentUser.fileList = newInfo.fileList;
            currentUser.updateListView(uploadedFilesList);
        }

        private void downloadButton_Click(object sender, EventArgs e)
        {
            Stream myStream;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                    File fileInfo = new File();
                    fileInfo.fileName = uploadedFilesList.SelectedItems[0].Text;
                    File.downloadTheFile(myStream, fileInfo, client, this);
                }
            }
        }

        private void renameButton_Click(object sender, EventArgs e)
        {
            string filename = uploadedFilesList.SelectedItems[0].Text;
            RenameWindow renameWindow = new RenameWindow(client, filename, this);
            renameWindow.Show();
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            Dictionary<String, String> fileInfo = new Dictionary<string, string>();

            fileInfo.Add("fileProcess", "DELETE");
            string filename = uploadedFilesList.SelectedItems[0].Text;
            fileInfo.Add("fileName", filename);

            SendMessage(JsonConvert.SerializeObject(fileInfo));
            fileListUpdate();
            writeLogEvent(filename + " is deleted.");
        }

        public void writeLogEvent(string info)
        {
            logListBox.Items.Add(info);
        }
        
    }
}
