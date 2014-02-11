using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Kinect;
using PowerPoint = Microsoft.Office.Interop.PowerPoint;
using Microsoft.Office.Core;
using System.Collections.Generic;
using System.Timers;
using System.Runtime.InteropServices;
using System.Diagnostics;
using WindowsInput;
using System.Threading;
using System.IO;

namespace SPprotoype
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Member Variables
        private KinectSensor _Kinect;
        private readonly Brush[] _SkeletonBrushes;
        private Skeleton[] _FrameSkeletons;
        string pptname = "D:\\acads\\1stsemSY12-13\\CMSC190-1\\SP\\prototype\\SPprotoype\\sppresentation.pptx";
        moveMouse mouse = new moveMouse();
        PowerPoint.Application ppt;
        PowerPoint.Presentations spPt;
        PowerPoint.Presentation openPt;
        bool opened = false;
        static int crt = 1;
        static Process myProcess = null;
        //Thread keyboardThread = new Thread(virtualKeyboard);
        bool boardOn = false;
        float loriginx, loriginy, loriginz; float roriginx, roriginy, roriginz;
        static System.Timers.Timer _timer = new System.Timers.Timer(5000);
        static System.Timers.Timer _timer2 = new System.Timers.Timer(5000);
        float range = .25F;
        DateTime dateOnly = DateTime.Today;
        bool Record = false;
        private Data data;
        private Data data2;
        private Data bRightData;
        private Data bLeftData;
        private Data closeR;
        private Data closeL;
        private double[] closeREquation;
        private double[] closeLEquation;
        private double[] bRightEquation;
        private double[] bLeftEquation;
        private ParallelCurve coords;
        private ParallelCurve coords2;
        int counter = 0;
        bool flag = true;
        int appli;
        Form1 form = new Form1();
        Form3 form3 = new Form3();
        bool drag = false;
        bool readyDrag = false;
        bool keyboardOn = false;
        bool keyPressed = false;
        bool altTab = false;
        Posture posture = new Posture();
        int mode = 1;
        int pNumber = 0;
        bool mouseMode = false;
        Keyboard keyboard = new Keyboard();
        ProcessName processName = new ProcessName();
        Gesture gesture;
        DataComputation dataComputation;
        #endregion Member Variables

        #region Constructor
        public MainWindow()
        {
            InitializeComponent();
            this._SkeletonBrushes = new[] { Brushes.White, Brushes.Crimson, Brushes.Indigo, Brushes.DodgerBlue, Brushes.Purple, Brushes.Pink };
            DiscoverKinectSensor();
            this.Loaded += (s, e) => { DiscoverKinectSensor(); };
            this.Unloaded += (s, e) => { this.Kinect = null; };
            textBlock1.Text = "Record off";


            dataComputation = new DataComputation();
        
             
           appli = 0;
           mouse = new moveMouse();
          form3.initButtons(); 
            /*
            String date = dateOnly.ToString("d") + "\n";
            System.IO.File.WriteAllText(@"D:\acads\CMSC190-2\SP\textfiles\Coord.txt", date);
             */
            
            /*
            bEquation = bData.getEquation();
            data = new Data("Coords3.txt");
            coords = new ParallelCurve(bData.getCoords(), data.getCoords(), bEquation);
            textBox2.Text = coords.getNearness().ToString();
            bData.saveIt();
             */
        }

        #endregion Constructor

        #region Methods
        private void DiscoverKinectSensor()
        {
          
            KinectSensor.KinectSensors.StatusChanged += KinectSensors_StatusChanged;
            this.Kinect = KinectSensor.KinectSensors.FirstOrDefault(x => x.Status == KinectStatus.Connected);
        }
        private void KinectSensors_StatusChanged(object sender, StatusChangedEventArgs e)
        {
            switch (e.Status)
            {
                case KinectStatus.Connected:
                    if (this.Kinect == null)
                    {
                        this.Kinect = e.Sensor;
                       // kinectDetection.Text = "Kinect Detected";
                    }
                    break;
                case KinectStatus.Disconnected:
                    if (this.Kinect == e.Sensor)
                    {
                        this.Kinect = null;
                        this.Kinect = KinectSensor.KinectSensors.FirstOrDefault(x => x.Status == KinectStatus.Connected);
                        if (this.Kinect == null)
                        {
                            //kinectDetection.Text = "Kinect Disconnected";
                        }
                    }
                    break;
            }
        }
        #endregion Methods

        #region Properties
        public KinectSensor Kinect
        {
            get { return this._Kinect; }

            set
            {
                if (this._Kinect != value)
                {
                    //uninitialize
                    if (this._Kinect != null)
                    {
                        this._Kinect.Stop();
                        this._Kinect.SkeletonFrameReady -= KinectDevice_SkeletonFrameReady;
                        this._Kinect.SkeletonStream.Disable();
                        this._FrameSkeletons = null;
                        UninitializeKinectSensor(this._Kinect);
                    }
                    this._Kinect = value;
                    //initialize
                    if (value != null && value.Status == KinectStatus.Connected)
                    {
                        this._Kinect.SkeletonStream.Enable();
                        this._FrameSkeletons = new Skeleton[this._Kinect.SkeletonStream.FrameSkeletonArrayLength];
                        this._Kinect.SkeletonFrameReady += KinectDevice_SkeletonFrameReady;
                        InitializeKinectSensor(this._Kinect);
                    }
                }
            }
        }

        private void InitializeKinectSensor(KinectSensor sensor)
        {
            if (sensor != null)
            {
               // sensor.ColorStream.Enable();
               // sensor.ColorFrameReady += Kinect_ColorFrameReady;
                sensor.Start();
            }
        }
        private void UninitializeKinectSensor(KinectSensor sensor)
        {
            if (sensor != null)
            {
                sensor.Stop();
                //sensor.ColorFrameReady -= Kinect_ColorFrameReady;
            }
        }
        
        #endregion Properties

        private void KinectDevice_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            using(SkeletonFrame frame = e.OpenSkeletonFrame())
            {
                if (frame != null)
                {
                    Skeleton skeleton;
                    JointType[] joints;
                    double diff;
                    double diff2;
                    int flagCanvas = 0;
                    int oldPnumber = 0;


                    frame.CopySkeletonDataTo(this._FrameSkeletons);

                    for (int i = 0; i < this._FrameSkeletons.Length; i++)
                   {
                        skeleton = this._FrameSkeletons[i];
                        if (skeleton.TrackingState == SkeletonTrackingState.Tracked)
                        {
                            textBlock5.Text = skeleton.Joints[JointType.HandRight].Position.X.ToString();

                            diff2 = skeleton.Joints[JointType.ShoulderLeft].Position.Z - skeleton.Joints[JointType.WristLeft].Position.Z;
                            diff = skeleton.Joints[JointType.HandLeft].Position.X - skeleton.Joints[JointType.Spine].Position.X;
                            posture.setSkeleton(skeleton);
                            //pNumber = posture.watPost(skeleton, mode);
                            //textBox2.Text = posture.pNumber.ToString() + "|" + mode.ToString();
                            //textBox5.Text = diff2.ToString();                           
                            string pathName = processName.GetActiveProcessFilePath();
                            string pName = processName.getProcessName(pathName);
                            //textBox4.Text = pName;
                            pNumber = posture.getInitPosture(skeleton);
                            mode = processName.getMode(pName);
                            
                            if (Record) // init posture is met it will start recording the coordinates of the joints
                            {
                                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\SourceCode\newLeft.txt", true))
                                {
                                    file.WriteLine(skeleton.Joints[JointType.HandLeft].Position.X.ToString() + " " + skeleton.Joints[JointType.HandLeft].Position.Y.ToString());
                                }
                                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\SourceCode\newRight.txt", true))
                                {
                                    file.WriteLine(skeleton.Joints[JointType.HandRight].Position.X.ToString() + " " + skeleton.Joints[JointType.HandRight].Position.Y.ToString());
                                }
                          
                                textBlock2.Text = gesture.initPosture.ToString();    
                                if (mouseMode) //mouse pointer will move depending on the position of the right hand of the user
                                {
                                    textBlock6.Text = mouse.animateMouse(skeleton).ToString();
                                    if (gesture.getTermPosture(skeleton, gesture.initPosture) == 6)
                                    {
                                        mouseMode = false;
                                        Record = false;
                                        File.Delete(@"C:\SourceCode\newLeft.txt");
                                        File.Delete(@"C:\SourceCode\newRight.txt");
                                    }
                                }
                                else if(keyboardOn) // virtual keyboard is on
                                {
                                    keyboard.keyPressed(skeleton);
                                    form3.changeText(keyboard.leftKeyLayer, keyboard.rightKeyLayer);
                                    form3.activateButton(keyboard.lCellNumber, keyboard.rCellNumber,keyboard.lKeypressed,keyboard.rKeypressed);
                                    if (gesture.getTermPosture(skeleton, gesture.initPosture) == 5)
                                    {
                                        keyboardOn = false;
                                        Record = false;
                                        if (form3.Visible)
                                            form3.Close();
                                        File.Delete(@"C:\SourceCode\newLeft.txt");
                                        File.Delete(@"C:\SourceCode\newRight.txt");
                                    }
                                }
                                else if (altTab) // alt + tab
                                {
                                    if (gesture.getTermPosture(skeleton, gesture.initPosture) == 7)
                                    {
                                        InputSimulator.SimulateKeyPress(VirtualKeyCode.TAB);
                                        Thread.Sleep(1000);
                                    }
                                    else if (gesture.getTermPosture(skeleton, gesture.initPosture) == 8)
                                    {
                                        InputSimulator.SimulateModifiedKeyStroke(VirtualKeyCode.SHIFT, VirtualKeyCode.TAB);
                                        Thread.Sleep(1000);
                                    }
                                    else if (gesture.getTermPosture(skeleton, gesture.initPosture) == -1)
                                    {
                                        altTab = false;
                                        Record = false;
                                        InputSimulator.SimulateKeyUp(VirtualKeyCode.MENU);
                                    }
                                    File.Delete(@"C:\SourceCode\newLeft.txt");
                                    File.Delete(@"C:\SourceCode\newRight.txt");
                                }
                                else
                                {
                                    if (gesture.getTermPosture(skeleton, gesture.initPosture) != 0) // recording of coordinates will stop
                                    {// it means that the terminating posture was met
                                        Record = false;
                                        textBlock3.Text = gesture.termPosture.ToString();
                                        textBlock1.Text = "Record False";
                                        data = new Data("newLeft.txt");
                                        data2 = new Data("newRight.txt");
                                        textBlock4.Text = dataComputation.analyzeData(gesture.termPosture, data, data2).ToString();
                                        File.Delete(@"C:\SourceCode\newLeft.txt");
                                        File.Delete(@"C:\SourceCode\newRight.txt");
                                    }
                                    else
                                    {
                                        InputSimulator.SimulateKeyUp(VirtualKeyCode.MENU);
                                    }
                                }
                            }
                            else
                            {
                                //if (File.Exists(@"D:\acads\CMSC190-2\SP\textfiles\SPbasecoords\openFile.txt"))
                                  //  File.Delete(@"D:\acads\CMSC190-2\SP\textfiles\SPbasecoords\openFile.txt");
                                gesture = new Gesture();
                                if (gesture.getInitPosture(skeleton) != 0)
                                {
                                    if (gesture.getInitPosture(skeleton) == 6)
                                        mouseMode = true;
                                    else if (gesture.getInitPosture(skeleton) == 5)
                                    {
                                        form3.Location = new System.Drawing.Point((int)((SystemParameters.VirtualScreenWidth / 2) - (form3.Width / 2)), 
																							(int)(SystemParameters.VirtualScreenHeight - form3.Height)-42);
                                        if(!form3.Visible)
										    form3.ShowDialog();
                                        keyboardOn = true;
                                    }
                                    else if (gesture.getInitPosture(skeleton) == 7)
                                    {
                                        InputSimulator.SimulateKeyDown(VirtualKeyCode.MENU);
                                        InputSimulator.SimulateKeyPress(VirtualKeyCode.TAB);
                                        altTab = true;
                                    }
                                    Record = true;
                                    textBlock1.Text = "Record True";
                                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\SourceCode\newLeft.txt", true))
                                    {
                                        file.WriteLine(skeleton.Joints[JointType.HandLeft].Position.X.ToString() + " " + skeleton.Joints[JointType.HandLeft].Position.Y.ToString());
                                    }
                                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\SourceCode\newRight.txt", true))
                                    {
                                        file.WriteLine(skeleton.Joints[JointType.HandRight].Position.X.ToString() + " " + skeleton.Joints[JointType.HandRight].Position.Y.ToString());
                                    }
                                }
                            }

                           
                        }
                   }//end for loop
                }
            }
        } // endof KinectDevice_SkeletonFrameready

     



        

        #region Commands
        double findDiff(double min,double sub)
        {
            
            return (min - sub);
        }
        int hoverFile(Skeleton skelly, double diff,int totalFile)
        {
            double diff1 = 0;
            int row = 0, column = 0,coords = 0;
            diff1 = skelly.Joints[JointType.HandLeft].Position.Y - skelly.Joints[JointType.HipCenter].Position.Y;
            if (diff > -0.3 && diff <= -0.2)//1st column
            {
                column = 0;
            }
            else if (diff > -0.2 && diff <= -0.1)//2nd column
            {
                column = 1;
            }
            else if (diff > -0.1 && diff <= 0)//3rd column
            {
                column = 2;
            }
            else if (diff > 0 && diff <= 0.1)//4th column
            {
                column = 3;
            }
            else if (diff > 0.1 && diff <= 0.2)//5th column
            {
                column = 4;
            }
            else if (diff > 0.2 && diff <= 0.3)//6th column
            {
                column = 5;
            }
            else
            {
                column = -1;
            }

            //row
            if (diff1 > 0 && diff1 <= 0.1)//1st row
            {
                row = 6;
            }
            else if (diff1 > 0.1 && diff1 <= 0.2)//2nd row
            {
                row = 5;
            }
            else if (diff1 > 0.2 && diff1 <= 0.3)//3rd row
            {
                row = 4;
            }
            else if (diff1 > 0.3 && diff1 <= 0.4)//4th row
            {
                row = 3;
            }
            else if (diff1 > 0.4 && diff1 <= 0.5)//5th row
            {
                row = 2;
            }
            else if (diff1 > 0.5 && diff1 <= 0.6)//6th row
            {
                row = 1;
            }
            else if (diff1 > 0.6 && diff1 <= 0.7)//7th row
            {
                row = 0;
            }
            else
            {
                row = -1;
            }

            coords = (row * 6) + column;
            return coords;

        }//end of hoverfile

        


        static void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            crt = 0;
        }
        #endregion Commands


    }
}
