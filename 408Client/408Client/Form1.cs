using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _408Client
{
    public partial class Form1 : Form
    {
        static bool terminating = false;
        static Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        Stream currentFileStream;
        public Form1()
        {
            InitializeComponent();
            serverIPAdressText.Text = "127.0.0.1";
            portNumberInteger.Value = 1;
            usernameText.Text = "muvaffak";
        }

        private void connectButton_Click(object sender, EventArgs e)
        {

            string serverIP;
            int serverPort;

            try
            {
                serverIP = serverIPAdressText.Text;
                //note: if you are testing on the same PC, you may try 127.0.0.1 as server's IP. It should work.

                serverPort = (int)portNumberInteger.Value;

                client.Connect(serverIP, serverPort);

                SendMessage("$username:" + usernameText.Text + "#");
            }
            catch
            {
                Console.Write("Cannot connected to the specified server\n");
                Console.Write("terminating...");
            }
        }

        static void SendMessage(string message)
        {
            byte[] buffer = Encoding.Default.GetBytes(message);

            //we can send a byte[] 
            client.Send(buffer);
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

                        long fileSize = new FileInfo(openFileDialog1.FileName).Length;
                        SendMessage("$filename:" + openFileDialog1.SafeFileName);
                        SendMessage("$sizeOfFile:" + fileSize + "#");
                        Thread fileThread = new Thread(sendFile);
                        fileThread.Start(openFileDialog1.FileName);

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void sendFile(object fileNameObj)
        {
            bool received = false;
            while (!received)
            {
                Byte[] buffer = new byte[64];
                int rec = client.Receive(buffer);



                string serverResponse = Encoding.Default.GetString(buffer);
                if (serverResponse.Contains("$SENDFILE#"))
                    break;

            }

            string fileName = (string)fileNameObj;
            client.SendFile(fileName);

        }
    }
}
