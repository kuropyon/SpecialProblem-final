using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsInput;
using Microsoft.Kinect;
using System.Windows;
using System.Threading;

namespace SPprotoype
{
    class Keyboard
    {
          //InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_C);
        bool keyboardOn = false;
        public int lColumn = 0, lRow = 0, rColumn = 0, rRow = 0;
        public int rCellNumber=0, lCellNumber=0, leftKeyLayer = 0, rightKeyLayer = 0;
        //string[] keyPressed = new string[4];
        double[] keysPressedL = new double[4];
        double[] keysPressedR = new double[4];
        public bool lKeypressed = false;
        public bool rKeypressed = false;
        int keyCounter = 0;
        int counter = 0;

        public Keyboard()
        {}
        public void keyPressed(Skeleton skelly)
        {
            double ldistX = 0, ldistZ = 0; ; //lefthand coordinate
            double rdistX = 0, rdistZ = 0; ; //righthand coordinate

            ldistX = Math.Abs(skelly.Joints[JointType.HipCenter].Position.X - skelly.Joints[JointType.WristLeft].Position.X);
            ldistZ = Math.Abs(skelly.Joints[JointType.HipCenter].Position.Y - skelly.Joints[JointType.WristLeft].Position.Y);

            rdistX = Math.Abs(skelly.Joints[JointType.HipCenter].Position.X - skelly.Joints[JointType.WristRight].Position.X);
            rdistZ = Math.Abs(skelly.Joints[JointType.HipCenter].Position.Y - skelly.Joints[JointType.WristRight].Position.Y);

            //lefthand X coordinate
            if(ldistX >= 0 && ldistX < 0.15)
                this.lColumn = 3;
            else if(ldistX >= 0.15 && ldistX < 0.3)
                this.lColumn = 2;
            else if (ldistX >= 0.3 && ldistX < 0.45)
                this.lColumn = 1;
            else
                this.lColumn = 0;

            //lefthand Z coordinate
            if (ldistZ >= 0 && ldistZ < 0.15)
                this.lRow = 3;
            else if (ldistZ >= 0.15 && ldistZ < 0.3)
                this.lRow = 2;
            else if (ldistZ >= 0.3 && ldistZ < 0.45)
                this.lRow = 1;
            else
                this.lRow = 0;

            //righthand X coordinate
            if (rdistX >= 0 && rdistX < 0.15)
                this.rColumn = 1;
            else if (rdistX >= 0.15 && rdistX < 0.3)
                this.rColumn = 2;
            else if (rdistX >= 0.3 && rdistX < 0.45)
                this.rColumn = 3;
            else
                this.rColumn = 0;

            //righthand Z coordinate
            if (rdistZ >= 0 && rdistZ < 0.15)
                this.rRow = 3;
            else if (rdistZ >= 0.15 && rdistZ < 0.3)
                this.rRow = 2;
            else if (rdistZ >= 0.3 && rdistZ < 0.45)
                this.rRow = 1;
            else
                this.rRow = 0;

            whatCell();
            setKeyLayer(skelly, rCellNumber, lCellNumber);
            isPressed(skelly);
        }//end of keyPressed

        private void whatCell()
        {
            /*
             * L-hand 1-9
             * R-hand 10-18
             */
            this.lCellNumber = (((this.lRow-1) * 3) + this.lColumn) - 1;
            this.rCellNumber = ((((this.rRow - 1) * 3) + this.rColumn))-1;
            if (this.lCellNumber < 0) this.lCellNumber = 0;
            if (this.rCellNumber < 0) this.rCellNumber = 0;
        }
        private void isPressed(Skeleton skelly)
        {
            if (Math.Abs(skelly.Joints[JointType.Spine].Position.Z - skelly.Joints[JointType.HandLeft].Position.Z) >= 0.3)
            {
                lKeypressed = true;
            }
            else if (Math.Abs(skelly.Joints[JointType.Spine].Position.Z - skelly.Joints[JointType.HandLeft].Position.Z) <= 0.3)
            {
                lKeypressed = false;
            }
            if (Math.Abs(skelly.Joints[JointType.Spine].Position.Z - skelly.Joints[JointType.HandRight].Position.Z) >= 0.3)
            {
                rKeypressed = true;
            }
            else if (Math.Abs(skelly.Joints[JointType.Spine].Position.Z - skelly.Joints[JointType.HandRight].Position.Z) <= 0.3)
            {
                rKeypressed = false;
            }
        }
        private void setKeyLayer(Skeleton skelly, int right,int left)
        {
            if (left == 4)
            {
                if (Math.Abs(skelly.Joints[JointType.Spine].Position.Z - skelly.Joints[JointType.HandLeft].Position.Z) >= 0.4 && keyCounter == 0)
                {
                    this.leftKeyLayer = 2;
                    keyCounter++;
                    Thread.Sleep(300);
                }
                else if (Math.Abs(skelly.Joints[JointType.Spine].Position.Z - skelly.Joints[JointType.HandLeft].Position.Z) >= 0.4 && keyCounter == 1)
                {
                    this.leftKeyLayer = 1;
                    keyCounter--;
                    Thread.Sleep(300);
                }
            }
           if (right == 4)
           {
               if (Math.Abs(skelly.Joints[JointType.Spine].Position.Z - skelly.Joints[JointType.HandRight].Position.Z) >= 0.4 && keyCounter == 0)
               {
                   this.rightKeyLayer = 2;
                   keyCounter++;
                   Thread.Sleep(300);
               }
               else if (Math.Abs(skelly.Joints[JointType.Spine].Position.Z - skelly.Joints[JointType.HandRight].Position.Z) >= 0.4 && keyCounter == 1)
               {
                   this.rightKeyLayer = 1;
                   keyCounter--;
                   Thread.Sleep(300);
               }
            }
        }

       
        public int zoomIn(string pName)
        {
            if (pName.Equals("Explorer.EXE")) { return 1; }
            else if (pName.Equals("chrome.exe")) { return 1; }
            else if (pName.Equals("prezi.exe")) { return 2; }
            else if (pName.Equals("AcroRd32.exe")) { return 3; }
            else if (pName.Equals("PicasaPhotoViewer.exe")) { return 4; }
            else if (pName.Equals("iTunes.exe")) { return 5; }
            else if (pName.Equals("SPprotoype.vshost.exe")) { return 6; }
            else return 0;
        }

    }
}
