using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    class User
    {
        public string username;
        public List<File> fileList = new List<File>();
        
        
        public void updateListView(ListView theList)
        {
            theList.Items.Clear();
            foreach (File aFile in fileList)
            {
                theList.Items.Add(new ListViewItem(new string[] { aFile.fileName, aFile.fileSize.ToString(), aFile.uploadTime.ToShortDateString() + " " + aFile.uploadTime.ToShortTimeString() }));
            }
        }
    }
}
