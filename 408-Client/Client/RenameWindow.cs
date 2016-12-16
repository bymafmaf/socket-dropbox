using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class RenameWindow : Form
    {
        static public Socket client;
        public string oldFileName;
        public ClientMainWindow caller;
        public RenameWindow(Socket connectedSocket, string fileName, ClientMainWindow main)
        {
            InitializeComponent();
            client = connectedSocket;
            oldFileName = fileName;
            newNameTextField.Text = fileName;
            caller = main;
        }
        static void SendMessage(string message)
        {
            byte[] buffer = Encoding.Default.GetBytes(message);

            //we can send a byte[] 
            client.Send(buffer);
            Console.Write("Your message: " + message + " has been sent.\n");
        }


        private void sendNewNameButton_Click(object sender, EventArgs e)
        {
            Dictionary<String, String> fileInfo = new Dictionary<string, string>();

            fileInfo.Add("fileProcess", "RENAME");
            fileInfo.Add("newFileName", newNameTextField.Text);
            fileInfo.Add("oldFileName", oldFileName);

            SendMessage(JsonConvert.SerializeObject(fileInfo));

            Byte[] buffer = new byte[32768];
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

            if (serverResponse.Contains("ERROR"))
            {
                caller.writeLogEvent("A file with the name " + newNameTextField.Text + " already exist. Cannot rename.");
                MessageBox.Show("A file with the name " + newNameTextField.Text + " already exist. Cannot rename.", "Rename Problem", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            }
            else if (serverResponse.Contains("SUCCESS"))
            {
                caller.writeLogEvent(oldFileName + " renamed to " + newNameTextField.Text);
                this.Close();
            }
            
            
        }
    }
}
