using WindowsInput;
using System.Windows;
using System.Threading;
namespace SPprotoype
{
    partial class Form3
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form3));
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.Lime;
            this.flowLayoutPanel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("flowLayoutPanel1.BackgroundImage")));
            this.flowLayoutPanel1.ForeColor = System.Drawing.Color.White;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(12, 33);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(270, 270);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.BackColor = System.Drawing.Color.Lime;
            this.flowLayoutPanel2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("flowLayoutPanel2.BackgroundImage")));
            this.flowLayoutPanel2.ForeColor = System.Drawing.Color.White;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(317, 33);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(270, 270);
            this.flowLayoutPanel2.TabIndex = 1;
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Lime;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(599, 339);
            this.Controls.Add(this.flowLayoutPanel2);
            this.Controls.Add(this.flowLayoutPanel1);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.Color.Transparent;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form3";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Form3";
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.Color.Lime;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Button[] leftButtons = new System.Windows.Forms.Button[9];
        private System.Windows.Forms.Button[] rightButtons = new System.Windows.Forms.Button[9];
        private System.Drawing.Size size = new System.Drawing.Size(80, 80);
        private System.Drawing.Font font= new System.Drawing.Font("Arial", 16);        
        private System.Drawing.Bitmap background = (System.Drawing.Bitmap)System.Drawing.Bitmap.FromFile(@"D:\acads\CMSC190-2\SP\images\form3Background.png");

        public void initButtons()
        {
            for (int i = 0; i < 9; i++)
            {
                leftButtons[i] = new System.Windows.Forms.Button();
                leftButtons[i].Name = "button" + (i + 1).ToString();
                leftButtons[i].Size = size;
                leftButtons[i].Enabled = false;
                leftButtons[i].Font = font;
                leftButtons[i].ForeColor = System.Drawing.Color.Red;
                leftButtons[i].Parent = flowLayoutPanel1;
            }

            for (int j = 0; j < 9; j++)
            {
                rightButtons[j] = new System.Windows.Forms.Button();
                rightButtons[j].Name = "button" + (j+10).ToString();
                rightButtons[j].Size = size;
                rightButtons[j].Enabled = false;
                rightButtons[j].Font = font;
                leftButtons[j].ForeColor = System.Drawing.Color.Red;
                rightButtons[j].Parent = flowLayoutPanel2;
            }
            flowLayoutPanel1.BackgroundImage = background;
            flowLayoutPanel2.BackgroundImage = background;
        }

        public void changeText(int left, int right)
        {
            decimal x;
            char c;
            switch (left)
            {
                case 1: for (int i = 0,j=0; i < 9; i++,j++)
                    {
                        if (i == 4)
                        {
                            j--;
                            leftButtons[i].Text = " ";
                            continue;
                        }
                        else
                        {
                            c = (char)(65 + j);
                            leftButtons[i].Text = c.ToString();
                        }
                    }
                        break;
                case 2: for (int i = 0, j = 16; i < 9; i++, j++)
                        {
                            if (i == 4)
                            {
                                j--;
                                leftButtons[i].Text = " ";
                                continue;
                            }
                            else if (i == 6)
                            {
                                leftButtons[i].Text = "BACKSPACE";
                            }
                            else if (i == 7)
                            {
                                leftButtons[i].Text = "ENTER";
                            }
                            else if (i >= 8)
                            {
                                leftButtons[i].Text = " ";
                            }
                            else
                            {
                                c = (char)(65 + j);
                                leftButtons[i].Text = c.ToString();
                            }
                        }
                    break;
            }//end of switch(left)

            switch (right)
            {
                case 1: for (int i = 0, j = 8; i < 9; i++, j++)
                    {
                        if (i == 4)
                        {
                            j--;
                            rightButtons[i].Text = " ";
                            continue;
                        }
                        else
                        {
                            c = (char)(65 + j);
                            rightButtons[i].Text = c.ToString();
                        }
                    }
                    break;
                case 2: for (int i = 0, j = 21; i < 9; i++, j++)
                    {
                        if (i == 4)
                        {
                            j--;
                            rightButtons[i].Text = " ";
                        }
                        else if (i == 6)
                        {
                            rightButtons[i].Text = "SPACE";
                        }
                        else if (i >= 7)
                        {
                            rightButtons[i].Text = " ";
                        }
                        else
                        {
                            c = (char)(65 + j);
                            rightButtons[i].Text = c.ToString();
                        }
                    }
                    break;
                  
            }
        }//end of method changeText

        public Point activateButton(int leftPanel,int rightPanel,bool left,bool right)
        {
            Point keyPressed = new Point();

            for (int i = 0; i < rightButtons.Length; i++)
            {
                if (i != rightPanel)
                {
                    rightButtons[i].Enabled = false;
                    rightButtons[i].BackColor = System.Drawing.Color.White;
                }
                else
                {
                    rightButtons[i].Enabled = true;
                    rightButtons[i].BackColor = System.Drawing.Color.Firebrick;
                    if(right)
                        pressKey(rightButtons[i].Text);
                }
            }
            for (int j = 0; j < leftButtons.Length; j++)
            {
                if (j != leftPanel)
                {
                    leftButtons[j].Enabled = false;
                    leftButtons[j].BackColor = System.Drawing.Color.White;   
                }
                else
                {
                    leftButtons[j].Enabled = true;
                    leftButtons[j].BackColor = System.Drawing.Color.Firebrick;
                    if(left)
                        pressKey(leftButtons[j].Text);
                }
            }
            return keyPressed;
        }//end of method activate button

        public void pressKey(string key)
        {
            switch (key)
            {
                case "A": InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_A); Thread.Sleep(400); break;
                case "B": InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_B); Thread.Sleep(400); break;
                case "C": InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_C); Thread.Sleep(400); break;
                case "D": InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_D); Thread.Sleep(400); break;
                case "E": InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_E); Thread.Sleep(400); break;
                case "F": InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_F); Thread.Sleep(400); break;
                case "G": InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_G); Thread.Sleep(400); break;
                case "H": InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_H); Thread.Sleep(400); break;
                case "I": InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_I); Thread.Sleep(400); break;
                case "J": InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_J); Thread.Sleep(400); break;
                case "K": InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_K); Thread.Sleep(400); break;
                case "L": InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_L); Thread.Sleep(400); break;
                case "M": InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_M); Thread.Sleep(400); break;
                case "N": InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_N); Thread.Sleep(400); break;
                case "O": InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_O); Thread.Sleep(400); break;
                case "P": InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_P); Thread.Sleep(400); break;
                case "Q": InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_Q); Thread.Sleep(400); break;
                case "R": InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_R); Thread.Sleep(400); break;
                case "S": InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_S); Thread.Sleep(400); break;
                case "T": InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_T); Thread.Sleep(400); break;
                case "U": InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_U); Thread.Sleep(400); break;
                case "V": InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_V); Thread.Sleep(400); break;
                case "W": InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_W); Thread.Sleep(400); break;
                case "X": InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_X); Thread.Sleep(400); break;
                case "Y": InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_Y); Thread.Sleep(400); break;
                case "Z": InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_Z); Thread.Sleep(400); break;
                case "BACKSPACE": InputSimulator.SimulateKeyPress(VirtualKeyCode.BACK); Thread.Sleep(400); break;
                case "SPACE": InputSimulator.SimulateKeyPress(VirtualKeyCode.SPACE); Thread.Sleep(400); break;
                case "ENTER": InputSimulator.SimulateKeyPress(VirtualKeyCode.RETURN); Thread.Sleep(400); break;
            }
        }
    }//end of class Form3
}//end of namespace SPPrototype