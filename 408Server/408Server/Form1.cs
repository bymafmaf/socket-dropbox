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

namespace _408Server
{
    public partial class Form1 : Form
    {
        static bool terminating = false;
        static bool accept = true;
        static Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        static List<Socket> socketList = new List<Socket>();
        static List<string> connectedUsers = new List<string>();


        public Form1()
        {
            InitializeComponent();
            myIPTextBox.Text = GetMyIP();
            portNumberInt.Value = 1;
        }
        private string GetMyIP()
        {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    return ip.ToString();

            return "127.0.0.1";
        }

        private void choosePath_Click(object sender, EventArgs e)
        {
            DialogResult result = choosePathDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                //
                // The user selected a folder and pressed the OK button.
                // We print the number of files found.
                //
                pathTextBox.Text = choosePathDialog.SelectedPath;
            }
        }



        private void startListening_Click(object sender, EventArgs e)
        {
            try
            {
                Thread thrAccept;
                server.Bind(new IPEndPoint(IPAddress.Any, (int)portNumberInt.Value));
                Console.WriteLine("Started listening for incoming connections.");

                server.Listen(3); //the parameter here is maximum length of the pending connections queue
                thrAccept = new Thread(new ThreadStart(Accept));
                thrAccept.Start();
            }
            catch
            {
                Console.WriteLine("Cannot create a server with the specified port number\n Check the port number and try again.");
                Console.Write("terminating...");
            }

        }

        static private void Accept()
        {
            while (accept)
            {
                try
                {
                    socketList.Add(server.Accept());
                    Console.Write("New client connected...\n");
                    Thread thrReceive;
                    thrReceive = new Thread(new ThreadStart(Receive));
                    thrReceive.Start();
                }
                catch
                {
                    if (terminating)
                        accept = false;
                    else
                        Console.Write("Listening socket has stopped working...\n");
                }
            }
        }
        static private void Receive()
        {
            bool connected = true;
            Socket n = socketList[socketList.Count - 1];
            string username;
            string fileName;
            int sizeOfFile = 0;
            while (connected)
            {
                try
                {

                    Byte[] buffer = new byte[64];
                    int rec = n.Receive(buffer);


                    if (rec <= 0)
                    {
                        throw new SocketException();
                    }

                    string newmessage = Encoding.Default.GetString(buffer);
                    newmessage = newmessage.Substring(0, newmessage.IndexOf("\0"));

                    if (newmessage.Contains("$username:") && newmessage.Contains("#"))
                    {
                        username = newmessage.Substring(10, newmessage.IndexOf('#') - 10);
                        connectedUsers.Add(username);
                        Console.Write("Came: " + username + "\r\n");
                    }
                    else if (newmessage.Contains("$filename:"))
                    {
                        fileName = newmessage.Substring(10, newmessage.Length - 10);
                        Console.Write("will receive: " + fileName + "\r\n");
                    }
                    else if (newmessage.Contains("$sizeOfFile:"))
                    {

                        sizeOfFile = Convert.ToInt32(newmessage.Substring(11, newmessage.Length - 11));
                        Console.Write("size: " + sizeOfFile + "\r\n");
                        Thread fileReceiveThread = new Thread(ReceiveFile);
                        fileReceiveThread.Start(sizeOfFile);
                    }

                }
                catch
                {
                    if (!terminating)
                        Console.Write("Client has disconnected...\n");
                    n.Close();
                    socketList.Remove(n);
                    connected = false;
                }


            }
        }

        static private void ReceiveFile(object fileSizeObj)
        {
            byte[] buffer = Encoding.Default.GetBytes("$SENDFILE#");

            //we can send a byte[] 
            Socket n = socketList[socketList.Count - 1];
            n.Send(buffer);


            Int32 fileSize = (Int32)fileSizeObj;

            Console.Write("started to get file:\r\n");
            Byte[] fileBuffer = new byte[fileSize];
            n.Receive(fileBuffer);


            File.WriteAllBytes("output.txt", fileBuffer);
            Console.Write("file is written:\r\n");

        }

    }
}
