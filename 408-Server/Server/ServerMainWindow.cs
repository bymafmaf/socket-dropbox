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
using Newtonsoft.Json;

namespace Server
{
    
    public partial class ServerMainWindow : Form
    {
        static bool terminating = false;
        static bool accept = true;
        static Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        public static BindingList<User> connectedUsers= new BindingList<User>();
        public static List<Thread> activeThreads = new List<Thread>();
        static public string folderPath = "";
        


        public ServerMainWindow()
        {
            InitializeComponent();
            myIPTextBox.Text = GetMyIP();
            portNumberInt.Value = 1;
            pathTextBox.Text = "C:\\Users\\monus\\Desktop";
            folderPath = pathTextBox.Text;
        }

        
        private string GetMyIP()
        {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    return ip.ToString();

            return "127.0.0.1";
        }

        public ListBox getLogListBox()
        {
            return connectedClientsListBox;
        }
        private void choosePath_Click(object sender, EventArgs e)
        {
            DialogResult result = choosePathDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                pathTextBox.Text = choosePathDialog.SelectedPath;
                folderPath = pathTextBox.Text;
            }
        }



        private void startListening_Click(object sender, EventArgs e)
        {
            try
            {
                if (folderPath != "")
                {
                    
                    Thread thrAccept;
                    server.Bind(new IPEndPoint(IPAddress.Any, (int)portNumberInt.Value));
                    Console.WriteLine("Started listening for incoming connections.");

                    server.Listen(3); //the parameter here is maximum length of the pending connections queue
                    thrAccept = new Thread(Accept);
                    thrAccept.IsBackground = true;
                    thrAccept.Start(this);
                } else
                {
                    MessageBox.Show("You have to select a path so that coming files can get recorded.", "Problem", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                }

            }
            catch
            {
                Console.WriteLine("Cannot create a server with the specified port number\n Check the port number and try again.");
                Console.Write("terminating...");
            }

        }
        static private void Accept(object main)
        {
            ServerMainWindow mainWindow = (ServerMainWindow)main;
            while (accept)
            {
                try
                {
                    User newUser = new User(mainWindow);
                    newUser.connection(server.Accept(), folderPath);
                    
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

        
    }
}
