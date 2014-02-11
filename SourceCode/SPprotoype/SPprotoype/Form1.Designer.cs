namespace SPprotoype
{
    partial class Form1
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
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(449, 327);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(449, 327);
            this.Controls.Add(this.flowLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form1";
            this.Text = "Applications";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.ResumeLayout(false);

        }

        #endregion
        #region Variables
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel[] flowLayoutPanel2;
        private System.Windows.Forms.PictureBox[] pictureBox2;
        private System.Windows.Forms.FlowDirection flow = new System.Windows.Forms.FlowDirection();
        private System.Drawing.Bitmap bitmap;
        private System.Drawing.Size size = new System.Drawing.Size(100, 100);
        private System.Drawing.Image img;
        private string[] files;
        private string[] fileNames;
        private string path;
        System.Windows.Forms.Label[] label;
        #endregion Variables

        public int addItem(int appNumber)
        {
            getFiles(appNumber);
            getIcon(appNumber);
            pictureBox2 = new System.Windows.Forms.PictureBox[files.Length];
            flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel[files.Length];
            label = new System.Windows.Forms.Label[fileNames.Length];
            flow = System.Windows.Forms.FlowDirection.TopDown;
            img = (System.Drawing.Image)bitmap;

            for (int i = 0; i < files.Length; i++)
            {
                flowLayoutPanel2[i] = new System.Windows.Forms.FlowLayoutPanel();
                flowLayoutPanel2[i].AutoSize = false;
                flowLayoutPanel2[i].FlowDirection = flow;
                pictureBox2[i] = new System.Windows.Forms.PictureBox();
                pictureBox2[i].Image = img;
                pictureBox2[i].Size = size;
                pictureBox2[i].Parent = this.flowLayoutPanel2[i];
                this.flowLayoutPanel2[i].Size = size;
                label[i] = new System.Windows.Forms.Label();
                label[i].ForeColor = System.Drawing.Color.White;
                label[i].Text = fileNames[i];
                label[i].AutoSize = true;
                label[i].Parent = this.flowLayoutPanel2[i];
                this.flowLayoutPanel2[i].Parent = this.flowLayoutPanel1;
            }
            return files.Length;
        }//end of addItem
        
        private void getFiles(int app)
        {

            switch (app)
            {
                    //pdf
                case 1: files = System.IO.Directory.GetFiles(@"D:\acads\CMSC190-2\SP\Files\PDF", "*.pdf");
                    fileNames = new string[files.Length];
                    for (int i = 0; i < files.Length; i++)
                    {
                        //path = files[i];
                        fileNames[i] = System.IO.Path.GetFileName(files[i]);
                    }
                        break;
                    //powerpoint (pptx)
                case 2: files = System.IO.Directory.GetFiles(@"D:\acads\CMSC190-2\SP\Files\Powerpoint", "*.ppt");
                        fileNames = new string[files.Length];
                        for (int i = 0; i < files.Length; i++)
                        {
                            //path = files[i];
                            fileNames[i] = System.IO.Path.GetFileName(files[i]);
                        }
                        break;
                    //prezi (exe)
                case 3:
                        files = System.IO.Directory.GetFiles(@"D:\acads\CMSC190-2\SP\Files\Prezi", "*.exe", System.IO.SearchOption.AllDirectories);
                        fileNames = new string[files.Length];
                        for (int i = 0; i < files.Length; i++)
                        {
                            //path = files[i];
                            fileNames[i] = System.IO.Path.GetFileName(files[i]);
                        }
                    break;
                default: break;
            }
        }//end of getFiles
        public string openFile(int fileNumber)
        {
            return files[fileNumber].ToString();
        }
        private void getIcon(int app)
        {
            switch (app)
            {
                case 1: bitmap = (System.Drawing.Bitmap)System.Drawing.Bitmap.FromFile(@"D:\acads\CMSC190-2\SP\images\pdfIconhover.png");
                    break;
                case 2: bitmap = (System.Drawing.Bitmap)System.Drawing.Bitmap.FromFile(@"D:\acads\CMSC190-2\SP\images\pptIconhover.png");
                    break;
                case 3: bitmap = (System.Drawing.Bitmap)System.Drawing.Bitmap.FromFile(@"D:\acads\CMSC190-2\SP\images\preziIconhover.png");
                    break;
                default: break;
            }
        }//end of getIcon
        private void getHoverIcon(int app)
        {
            switch (app)
            {
                case 1: bitmap = (System.Drawing.Bitmap)System.Drawing.Bitmap.FromFile(@"D:\acads\CMSC190-2\SP\images\pdfIcon.png");
                    break;
                case 2: bitmap = (System.Drawing.Bitmap)System.Drawing.Bitmap.FromFile(@"D:\acads\CMSC190-2\SP\images\pptIcon.png");
                    break;
                case 3: bitmap = (System.Drawing.Bitmap)System.Drawing.Bitmap.FromFile(@"D:\acads\CMSC190-2\SP\images\preziIcon.png");
                    break;
                default: break;
            }
        }//end of getHoverIcon

        public void hoverIcon(int fileNumber,int app)
        {
            getHoverIcon(app);
            pictureBox2[fileNumber].Image = (System.Drawing.Image)bitmap;
        }
        public void standbyIcon(int fileNumber, int app)
        {
            getIcon(app);
            if(fileNumber >= 0)
                pictureBox2[fileNumber].Image = (System.Drawing.Image)bitmap;
        }
    }
}