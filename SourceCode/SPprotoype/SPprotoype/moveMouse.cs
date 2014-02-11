using System;
using Microsoft.Kinect;
using Microsoft.Office.Core;
using System.Runtime.InteropServices;
using System.Windows;
using System.Threading;

/// <summary>
/// class for mouse events
/// </summary>
public class moveMouse
{
    #region properties
    private struct coorPT
    {
        public double oldX;
        public double oldY;
        public double oldZ;
        public double curX;
        public double curY;
        public double curZ;
    }
    private struct POINT
    {
        public int X;
        public int Y;

        public POINT(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }
    coorPT left;
    coorPT right;
    int diffX;
    int olddiffX;
    int diffY;
    int olddiffY;
    POINT p;
    bool leftMButtonDown = false;
    bool rightMButtonDown = false;
    public double angle = 0;
    
    #endregion properties

    #region constructor
    public moveMouse()
    {
        left.curX = 0;
        left.curY = 0;
        left.curZ = 0;
        left.oldX = 0;
        left.oldY = 0;
        left.oldZ = 0;
        right.curX = 0;
        right.curY = 0;
        right.curZ = 0;
        right.oldX = 0;
        right.oldY = 0;
        right.oldZ = 0;
    }
    #endregion constructor

    #region DLL imports
    [DllImport("User32.dll")] //import the library of user32.dll
    private static extern bool SetCursorPos(int x, int y);
    [DllImport("User32.dll")]
    static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);


    [Flags]
    public enum MouseEventFlags : uint
    {
        LEFTDOWN = 0x00000002,
        LEFTUP = 0x00000004,
        MIDDLEDOWN = 0x00000020,
        MIDDLEUP = 0x00000040,
        MOVE = 0x00000001,
        ABSOLUTE = 0x00008000,
        WHEEL = 0x00000800,
        RIGHTDOWN = 0x00000008,
        RIGHTUP = 0x00000010
    }

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool GetCursorPos(out POINT lpPoint);
    #endregion DLL imports


    #region methods 
    public double animateMouse(Skeleton skelly)
    {// the Z coordinate of the hand will be the Y coordinate of the mouse
        double diffX = 0,diffY = 0, percentX = 0, percentY = 0;
        double positionX, positionY;
        double oldPosX = 0,oldPosY = 0;
        double dist = 0;
        int x = 0,y = 0,click = 0;
        this.right.curX = skelly.Joints[JointType.WristRight].Position.X;
        this.right.curZ = skelly.Joints[JointType.WristRight].Position.Z;
        this.right.curY = skelly.Joints[JointType.WristRight].Position.Y;
        /*
        positionX = skelly.Joints[JointType.Spine].Position.X - this.right.curX;
        positionZ = (skelly.Joints[JointType.Spine].Position.Z- 0.5) - this.right.curZ;

        oldPosX = skelly.Joints[JointType.Spine].Position.X - this.right.oldX;
        oldPosZ = (skelly.Joints[JointType.Spine].Position.Z- 0.5) - this.right.oldZ;
        */


        if (this.right.oldX != 0 && this.right.oldY != 0)
        {
            /*
            diffX = (positionX - oldPosX) * -1;
            diffZ = (positionZ - oldPosZ) * -1;
             * */
            diffX = this.right.curX - this.right.oldX;
            diffY = this.right.curY - this.right.oldY;
            dist = Math.Sqrt(Math.Pow(diffX, 2) + Math.Pow(diffY, 2));

            GetCursorPos(out p);
            percentX = (diffX * SystemParameters.VirtualScreenWidth)*2;
            percentY = (diffY * SystemParameters.VirtualScreenHeight)*2;

            if (percentX > 0)
                x = (int)(SystemParameters.VirtualScreenWidth * 0.02);
            else if (percentX < 0)
                x = (int)(SystemParameters.VirtualScreenWidth * 0.02)*-1;
            else
                x = 0;

            if(percentY > 0)
                y = (int)(SystemParameters.VirtualScreenHeight * 0.03) * -1;
            else if (percentY < 0)
                y = (int)(SystemParameters.VirtualScreenHeight * 0.03);
            else
                y = 0;
        }

        if (Math.Abs(diffY) > 0.01 && Math.Abs(diffX) > 0.01)
        {
            p.X = p.X + x;
            p.Y = p.Y + y;
            SetCursorPos(p.X, p.Y);
        }
        else if (Math.Abs(diffY) > 0.01 && Math.Abs(diffX) < 0.01)
        {
            p.Y = p.Y + y;
            SetCursorPos(p.X, p.Y);
        }
        else if (Math.Abs(diffY) < 0.01 && Math.Abs(diffX) > 0.01)
        {
            p.X = p.X + x;
            SetCursorPos(p.X, p.Y);
        }
        else
        {
            SetCursorPos(p.X, p.Y);
        }

        this.right.oldX = this.right.curX;
        this.right.oldZ = this.right.curZ;
        this.right.oldY = this.right.curY;
        
        if (clickOrNot(skelly) > 170)
        {
            leftClick(p.X, p.Y);
            Thread.Sleep(200);
            
        }
        else if(clickOrNot(skelly) < 30)
        {
            rightClick(p.X, p.Y);
            Thread.Sleep(200);
        }

        return clickOrNot(skelly);
    }
    private double clickOrNot(Skeleton skelly)
    {
        Point wrist = new Point(), elbow = new Point(), shoulder = new Point();
        double a, b, c;

        elbow.X = skelly.Joints[JointType.ElbowLeft].Position.X;
        elbow.Y = skelly.Joints[JointType.ElbowLeft].Position.Y;
        shoulder.X = skelly.Joints[JointType.ShoulderLeft].Position.X;
        shoulder.Y = skelly.Joints[JointType.ShoulderLeft].Position.Y;
        wrist.X = skelly.Joints[JointType.WristLeft].Position.X;
        wrist.Y = skelly.Joints[JointType.WristLeft].Position.Y;

        a = getDistance(shoulder,elbow);
        b = getDistance(elbow, wrist);
        c = getDistance(wrist, shoulder);
        return cosRule2(c, b, a);



    }
    private int ifClick(Skeleton skeleton)
    {
        double angle = 0;
        double diffZ = 0, diffY = 0;
        diffY = (skeleton.Joints[JointType.HandRight].Position.Y - skeleton.Joints[JointType.ElbowRight].Position.Y);
        diffZ = (skeleton.Joints[JointType.HandRight].Position.Z - skeleton.Joints[JointType.ElbowRight].Position.Z);
            angle = Math.Atan(diffZ/diffX) * (180 / Math.PI);

        this.angle = angle;
        if (angle >= 90 && angle <= 135)
            return 1;
        else return 0;
    }
    public static void leftClick(int x, int y)
    {
        mouse_event((int)(MouseEventFlags.LEFTDOWN), x, y, 0, 0);
        mouse_event((int)(MouseEventFlags.LEFTUP), x, y, 0, 0);
    }//left click event of the mouse
    public static void leftMButtonHold(int x, int y)
    {
        mouse_event((int)(MouseEventFlags.LEFTDOWN), x, y, 0, 0);
    }
    public static void doubleLeftClick(int x, int y)
    {
        mouse_event((int)(MouseEventFlags.LEFTDOWN), x, y, 0, 0);
        mouse_event((int)(MouseEventFlags.LEFTUP), x, y, 0, 0);
        mouse_event((int)(MouseEventFlags.LEFTDOWN), x, y, 0, 0);
        mouse_event((int)(MouseEventFlags.LEFTUP), x, y, 0, 0);
    }//left click event of the mouse
    public static void rightClick(int x, int y)
    {
        mouse_event((int)(MouseEventFlags.RIGHTDOWN), x, y, 0, 0);
        mouse_event((int)(MouseEventFlags.RIGHTUP), x, y, 0, 0);
    }
    public static void doubleRightClick(int x, int y)
    {
        mouse_event((int)(MouseEventFlags.RIGHTDOWN), x, y, 0, 0);
        mouse_event((int)(MouseEventFlags.RIGHTUP), x, y, 0, 0);
        mouse_event((int)(MouseEventFlags.RIGHTDOWN), x, y, 0, 0);
        mouse_event((int)(MouseEventFlags.RIGHTUP), x, y, 0, 0);
    }
    public static void rightMButtonhold(int x, int y)
    {
        mouse_event((int)(MouseEventFlags.RIGHTUP), x, y, 0, 0);
    }
    
    private double findAngle(Skeleton skeleton)
    {
        double dist1 = 0,dist2 = 0, hyp = 0,x,y;        
        x = Math.Pow(skeleton.Joints[JointType.WristRight].Position.X - skeleton.Joints[JointType.ElbowRight].Position.X,2);
        y = Math.Pow(skeleton.Joints[JointType.WristRight].Position.Y - skeleton.Joints[JointType.ElbowRight].Position.Y, 2);
        dist1 = Math.Sqrt(x+y);

        x = Math.Pow(skeleton.Joints[JointType.WristRight].Position.X - skeleton.Joints[JointType.HandRight].Position.X,2);
        y = Math.Pow(skeleton.Joints[JointType.WristRight].Position.Y - skeleton.Joints[JointType.HandRight].Position.Y,2);
        dist2 = Math.Sqrt(x+y);

        this.angle = Math.Atan(dist2 / dist1) * (180 / Math.PI);
        return this.angle;
    }
    private double findDistance(Skeleton skelly)
    {
        double distance = 0;

        distance = Math.Abs(skelly.Joints[JointType.ShoulderRight].Position.Z - skelly.Joints[JointType.HandRight].Position.Z);
        this.angle = distance;
        return distance;
    }

    private double getAngle(Skeleton skelly)
    {
        double a, b, c;
        Point elbow = new Point(), shoulder = new Point(), hand = new Point(), wrist = new Point();
        elbow.X = skelly.Joints[JointType.ElbowRight].Position.X;
        elbow.Y = skelly.Joints[JointType.ElbowRight].Position.Y;
        shoulder.X = skelly.Joints[JointType.ShoulderRight].Position.X;
        shoulder.Y = skelly.Joints[JointType.ShoulderRight].Position.Y;
        hand.X = skelly.Joints[JointType.HandRight].Position.X;
        hand.Y = skelly.Joints[JointType.HandRight].Position.Y;
        wrist.X = skelly.Joints[JointType.WristRight].Position.X;
        wrist.Y = skelly.Joints[JointType.WristRight].Position.Y;
        /*
        a = getDistance(shoulder, elbow);
        b = getDistance(elbow, hand);
        c = getDistance(hand, shoulder);
        */

        a = getDistance(elbow, wrist);
        b = getDistance(wrist,hand);
        c = getDistance(hand,elbow);
        return cosRule2(a, b, c);


    }
    private double getDistance(Point point1, Point point2)
    {
        return Math.Sqrt(Math.Pow(point1.X - point2.X, 2) + Math.Pow(point1.Y - point2.Y, 2));
    }
    private double cosRule1(double b, double c, double angle)
    {
        return Math.Sqrt(Math.Pow(b,2) + Math.Pow(c,2) - (2*b*c*Math.Cos(angle)));
    }
    private double cosRule2(double a, double b,double c)
    {
        double radian = Math.Acos((Math.Pow(b, 2) + Math.Pow(c, 2) - Math.Pow(a, 2)) / (2 * b * c));
        return radian * (180 / Math.PI);
        
    }
    #endregion methods

}
