using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SocketsExchangeService
{

    public partial class frmServiceLog : Form
    {

        
        //private SockMsg MsgFromClient;
        //private LogMsg TakeThisLogMsg;

        XChngServer xchg;
        //public LogMsg TakeThisLogMsg;
        //public delegate void ChangedEventHandler(string LogMsg);
        //public event ChangedEventHandler UpdateLog;

        public frmServiceLog()
        {
            InitializeComponent();
            //TakeThisLogMsg += new LogMsg(AddLogEntry)
            //UpdateLog += new ChangedEventHandler(AddLogEntry);
        }
        public void AddLogEntry(string LogMsg)
        {
            if (InvokeRequired)
            {
                //txtLog.Text = txtLog.Text + Environment.NewLine + LogMsg;
            }

        }

        private void frmServiceLog_Resize(object sender, EventArgs e)
        {

            if (FormWindowState.Minimized == this.WindowState)
            {
               trayIcon.Visible = true;
               trayIcon.ShowBalloonTip(500);
               this.Hide();
            }
        }

        private void frmServiceLog_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
        }

        private void trayIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
        }

        private void cmsTrayStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if(e.ClickedItem == tsmClose)
            {
                this.Close();
                //???
            }
            else if (e.ClickedItem == tsmHide)
            {
                this.Hide();
            }
            else if (e.ClickedItem == tsmShow)
            {
                this.Show();
                this.Visible = true;
                this.BringToFront();
            }
            
        }

        private void frmServiceLog_Load(object sender, EventArgs e)
        {
            xchg = new XChngServer(this);
            GlobSyn.gSync = (ISynchronizeInvoke)this;

            lsvRemConClients.Columns[0].Width = this.lsvRemConClients.Width - 20;
            
        }

        private void tsmShow_Click(object sender, EventArgs e)
        {
            this.Show();
            this.Visible = true;
            this.BringToFront();
        }
    }
}
