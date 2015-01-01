namespace SocketsExchangeService
{
    partial class frmServiceLog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmServiceLog));
            this.txtLog = new System.Windows.Forms.TextBox();
            this.trayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.cmsTrayStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmClose = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmHide = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmShow = new System.Windows.Forms.ToolStripMenuItem();
            this.txtTraffic = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblRPIEndPoint = new System.Windows.Forms.Label();
            this.lblRPILastCom = new System.Windows.Forms.Label();
            this.pcbCarState = new System.Windows.Forms.PictureBox();
            this.label7 = new System.Windows.Forms.Label();
            this.lblRPIStatus = new System.Windows.Forms.Label();
            this.lsvRemConClients = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cmsTrayStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pcbCarState)).BeginInit();
            this.SuspendLayout();
            // 
            // txtLog
            // 
            this.txtLog.BackColor = System.Drawing.Color.White;
            this.txtLog.Location = new System.Drawing.Point(12, 29);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtLog.Size = new System.Drawing.Size(581, 188);
            this.txtLog.TabIndex = 0;
            // 
            // trayIcon
            // 
            this.trayIcon.ContextMenuStrip = this.cmsTrayStrip;
            this.trayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("trayIcon.Icon")));
            this.trayIcon.Text = "SocketServiceLog";
            this.trayIcon.Visible = true;
            this.trayIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.trayIcon_MouseDoubleClick);
            // 
            // cmsTrayStrip
            // 
            this.cmsTrayStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmClose,
            this.toolStripSeparator1,
            this.tsmHide,
            this.tsmShow});
            this.cmsTrayStrip.Name = "cmsTrayStrip";
            this.cmsTrayStrip.Size = new System.Drawing.Size(179, 76);
            this.cmsTrayStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.cmsTrayStrip_ItemClicked);
            // 
            // tsmClose
            // 
            this.tsmClose.Name = "tsmClose";
            this.tsmClose.Size = new System.Drawing.Size(178, 22);
            this.tsmClose.Text = "Close SocketService";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(175, 6);
            // 
            // tsmHide
            // 
            this.tsmHide.Name = "tsmHide";
            this.tsmHide.Size = new System.Drawing.Size(178, 22);
            this.tsmHide.Text = "Hide ServiceLog";
            // 
            // tsmShow
            // 
            this.tsmShow.Name = "tsmShow";
            this.tsmShow.Size = new System.Drawing.Size(178, 22);
            this.tsmShow.Text = "Show ServiceLog";
            // 
            // txtTraffic
            // 
            this.txtTraffic.BackColor = System.Drawing.Color.White;
            this.txtTraffic.Location = new System.Drawing.Point(12, 252);
            this.txtTraffic.Multiline = true;
            this.txtTraffic.Name = "txtTraffic";
            this.txtTraffic.ReadOnly = true;
            this.txtTraffic.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtTraffic.Size = new System.Drawing.Size(581, 188);
            this.txtTraffic.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 18);
            this.label1.TabIndex = 4;
            this.label1.Text = "Server Log:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(9, 227);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(263, 18);
            this.label2.TabIndex = 5;
            this.label2.Text = "Message Traffic (Only Asynch Events):";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(608, 227);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(110, 18);
            this.label3.TabIndex = 7;
            this.label3.Text = "Control Clients:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(608, 37);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(36, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "EndP:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(608, 63);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Last com:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(608, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 18);
            this.label6.TabIndex = 10;
            this.label6.Text = "RPI Client:";
            // 
            // lblRPIEndPoint
            // 
            this.lblRPIEndPoint.AutoSize = true;
            this.lblRPIEndPoint.Location = new System.Drawing.Point(667, 37);
            this.lblRPIEndPoint.Name = "lblRPIEndPoint";
            this.lblRPIEndPoint.Size = new System.Drawing.Size(27, 13);
            this.lblRPIEndPoint.TabIndex = 11;
            this.lblRPIEndPoint.Text = "N/A";
            // 
            // lblRPILastCom
            // 
            this.lblRPILastCom.AutoSize = true;
            this.lblRPILastCom.Location = new System.Drawing.Point(667, 63);
            this.lblRPILastCom.Name = "lblRPILastCom";
            this.lblRPILastCom.Size = new System.Drawing.Size(27, 13);
            this.lblRPILastCom.TabIndex = 12;
            this.lblRPILastCom.Text = "N/A";
            // 
            // pcbCarState
            // 
            this.pcbCarState.Image = global::SocketsExchangeService.Properties.Resources.Car_Off;
            this.pcbCarState.InitialImage = global::SocketsExchangeService.Properties.Resources.Car_Off;
            this.pcbCarState.Location = new System.Drawing.Point(611, 94);
            this.pcbCarState.Name = "pcbCarState";
            this.pcbCarState.Size = new System.Drawing.Size(167, 123);
            this.pcbCarState.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pcbCarState.TabIndex = 13;
            this.pcbCarState.TabStop = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(609, 89);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(138, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "Latest Reported Car Status:";
            // 
            // lblRPIStatus
            // 
            this.lblRPIStatus.BackColor = System.Drawing.Color.Red;
            this.lblRPIStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRPIStatus.Location = new System.Drawing.Point(691, 11);
            this.lblRPIStatus.Name = "lblRPIStatus";
            this.lblRPIStatus.Size = new System.Drawing.Size(87, 18);
            this.lblRPIStatus.TabIndex = 15;
            this.lblRPIStatus.Text = "DOWN";
            this.lblRPIStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lsvRemConClients
            // 
            this.lsvRemConClients.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.lsvRemConClients.FullRowSelect = true;
            this.lsvRemConClients.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lsvRemConClients.Location = new System.Drawing.Point(612, 248);
            this.lsvRemConClients.Name = "lsvRemConClients";
            this.lsvRemConClients.Size = new System.Drawing.Size(166, 192);
            this.lsvRemConClients.TabIndex = 16;
            this.lsvRemConClients.UseCompatibleStateImageBehavior = false;
            this.lsvRemConClients.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Width = 166;
            // 
            // frmServiceLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(789, 455);
            this.ControlBox = false;
            this.Controls.Add(this.lsvRemConClients);
            this.Controls.Add(this.lblRPIStatus);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.pcbCarState);
            this.Controls.Add(this.lblRPILastCom);
            this.Controls.Add(this.lblRPIEndPoint);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtTraffic);
            this.Controls.Add(this.txtLog);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "frmServiceLog";
            this.ShowInTaskbar = false;
            this.Text = "Socket Xchange Service";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmServiceLog_FormClosing);
            this.Load += new System.EventHandler(this.frmServiceLog_Load);
            this.Resize += new System.EventHandler(this.frmServiceLog_Resize);
            this.cmsTrayStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pcbCarState)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.NotifyIcon trayIcon;
        private System.Windows.Forms.ContextMenuStrip cmsTrayStrip;
        private System.Windows.Forms.ToolStripMenuItem tsmClose;
        private System.Windows.Forms.ToolStripMenuItem tsmHide;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem tsmShow;
        public System.Windows.Forms.TextBox txtTraffic;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        public System.Windows.Forms.Label lblRPIEndPoint;
        public System.Windows.Forms.Label lblRPILastCom;
        public System.Windows.Forms.PictureBox pcbCarState;
        private System.Windows.Forms.Label label7;
        public System.Windows.Forms.Label lblRPIStatus;
        public System.Windows.Forms.ListView lsvRemConClients;
        private System.Windows.Forms.ColumnHeader columnHeader1;
    }
}

