using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
namespace SPprotoype
{
    class Posture
    {
        /*
         * Posture number
         * 1 -  NUI Mode
         * 2 - Standard Mode
         * 3 - Presentation
         * 4 - Keyboard
         */
        private Skeleton skelly;
        public int pNumber;
        public int mode;
        

        public Posture()
        {
        }
        public void setSkeleton(Skeleton skeleton)
        {
            this.skelly = skeleton;
        }
        //getInitPosture(Skeleton skeleton)
        //getTermPosture(Skeleton skeleton)
        public int getInitPosture(Skeleton skeleton) // determines if the posture of the user is a init posture
        {
            if(skeleton.Joints[JointType.HandLeft].Position.Y > skeleton.Joints[JointType.Head].Position.Y &&
                skeleton.Joints[JointType.HandLeft].Position.X > skeleton.Joints[JointType.Head].Position.X &&
                skeleton.Joints[JointType.HandLeft].Position.Z < skeleton.Joints[JointType.Head].Position.Z
                )//right arrow button init posture
            {// left hand upper right side
                return 1;
            }
            else if (skeleton.Joints[JointType.HandRight].Position.Y > skeleton.Joints[JointType.Head].Position.Y &&
                    skeleton.Joints[JointType.HandRight].Position.X < skeleton.Joints[JointType.Head].Position.X &&
                    skeleton.Joints[JointType.HandRight].Position.Z < skeleton.Joints[JointType.Head].Position.Z
                    )//left arrow button init posture
            {//right hand upper left side
                return 2;
            }
            else if (skeleton.Joints[JointType.HandLeft].Position.Y > skeleton.Joints[JointType.ShoulderCenter].Position.Y &&
                skeleton.Joints[JointType.HandLeft].Position.Y < skeleton.Joints[JointType.Head].Position.Y &&
                skeleton.Joints[JointType.HandLeft].Position.X < skeleton.Joints[JointType.Head].Position.X &&
                skeleton.Joints[JointType.HandLeft].Position.Z < skeleton.Joints[JointType.Head].Position.Z &&
                skeleton.Joints[JointType.HandRight].Position.Y < skeleton.Joints[JointType.HipCenter].Position.Y &&
                skeleton.Joints[JointType.HandRight].Position.X > skeleton.Joints[JointType.Head].Position.X
            )//open file init posture
            {//
                return 3;
            }
            else if (skeleton.Joints[JointType.HandRight].Position.Y > skeleton.Joints[JointType.ShoulderCenter].Position.Y &&
                skeleton.Joints[JointType.HandRight].Position.Y < skeleton.Joints[JointType.Head].Position.Y &&
                skeleton.Joints[JointType.HandRight].Position.X > skeleton.Joints[JointType.Head].Position.X &&
                skeleton.Joints[JointType.HandRight].Position.Z < skeleton.Joints[JointType.Head].Position.Z &&
                skeleton.Joints[JointType.HandLeft].Position.Y < skeleton.Joints[JointType.HipCenter].Position.Y &&
                skeleton.Joints[JointType.HandLeft].Position.X < skeleton.Joints[JointType.Head].Position.X
                )//close program init posture
            {//right hand upper right side
                return 4;
            }
            
            else if (skeleton.Joints[JointType.HandLeft].Position.X > skeleton.Joints[JointType.HandRight].Position.X &&
                skeleton.Joints[JointType.HandLeft].Position.Y > skeleton.Joints[JointType.HandRight].Position.Y &&
                skeleton.Joints[JointType.HandLeft].Position.Y > skeleton.Joints[JointType.Spine].Position.Y &&
                skeleton.Joints[JointType.HandRight].Position.Y > skeleton.Joints[JointType.Spine].Position.Y
                )//keyboard init posture
            {//left hand  over right hand at chest level
                return 5;
            }
            
        else if (skeleton.Joints[JointType.HandRight].Position.Y < skeleton.Joints[JointType.Spine].Position.Y &&
            skeleton.Joints[JointType.HandRight].Position.Y > skeleton.Joints[JointType.HipCenter].Position.Y &&
              skeleton.Joints[JointType.HandRight].Position.Y < skeleton.Joints[JointType.Head].Position.Y &&
            skeleton.Joints[JointType.HandRight].Position.X > skeleton.Joints[JointType.ShoulderRight].Position.X &&
             skeleton.Joints[JointType.HandRight].Position.X > skeleton.Joints[JointType.ElbowRight].Position.X
            )//mouse init posture
        {//right hand at stomach level
            return 6;
        }
            else if (skeleton.Joints[JointType.HandRight].Position.Y < skeleton.Joints[JointType.HipCenter].Position.Y &&
             skeleton.Joints[JointType.HandRight].Position.Y < skeleton.Joints[JointType.HipCenter].Position.Y &&
             skeleton.Joints[JointType.HandRight].Position.X < skeleton.Joints[JointType.Spine].Position.X
             )//alt + tab
            {
                return 7;
            }

            else return 0;
        }//end of method getInitPosture

        //get terminating posture
        public int getTermPosture(Skeleton skeleton, int initPosture) // determines if the posture of the user is a terminating posture
        {
            if(skeleton.Joints[JointType.HandLeft].Position.Y > skeleton.Joints[JointType.Head].Position.Y &&
                skeleton.Joints[JointType.HandLeft].Position.X < skeleton.Joints[JointType.Head].Position.X &&
                skeleton.Joints[JointType.HandLeft].Position.Z < skeleton.Joints[JointType.Head].Position.Z &&
                initPosture == 1
            )//left arrow button terminating posture
            {//left hand upper left side
                return 1;
            }
            else if (skeleton.Joints[JointType.HandRight].Position.Y > skeleton.Joints[JointType.Head].Position.Y &&
                 skeleton.Joints[JointType.HandRight].Position.X > skeleton.Joints[JointType.Head].Position.X &&
                 skeleton.Joints[JointType.HandRight].Position.Z < skeleton.Joints[JointType.Head].Position.Z &&
                initPosture == 2
                 )//right arrow term posture
            {//right hand upper right side
                return 2;
            }
            else if (
                skeleton.Joints[JointType.HandLeft].Position.Y > skeleton.Joints[JointType.HipCenter].Position.Y &&
                skeleton.Joints[JointType.HandLeft].Position.Y < skeleton.Joints[JointType.ShoulderCenter].Position.Y &&
                skeleton.Joints[JointType.HandLeft].Position.X > skeleton.Joints[JointType.Spine].Position.X &&
                initPosture == 3
                )//open file term posture
            {//elbow at the back but at chest level
                return 3;
            }
            else if (skeleton.Joints[JointType.HandRight].Position.Y > skeleton.Joints[JointType.HipCenter].Position.Y &&
                skeleton.Joints[JointType.HandRight].Position.Y < skeleton.Joints[JointType.ShoulderCenter].Position.Y &&
                skeleton.Joints[JointType.HandRight].Position.X < skeleton.Joints[JointType.Spine].Position.X &&
                initPosture == 4
                )//close app term posture
            {// right hand at stomach level 
                return 4;
            }
            else if (skeleton.Joints[JointType.HandLeft].Position.X > skeleton.Joints[JointType.HandRight].Position.X &&
                skeleton.Joints[JointType.HandLeft].Position.Y < skeleton.Joints[JointType.HandRight].Position.Y &&
                skeleton.Joints[JointType.HandLeft].Position.Y > skeleton.Joints[JointType.Spine].Position.Y &&
                skeleton.Joints[JointType.HandRight].Position.Y > skeleton.Joints[JointType.Spine].Position.Y &&
                initPosture == 5
                )//keyboard off 
            {
                return 5;
            }
            else if (skeleton.Joints[JointType.HandLeft].Position.Y > skeleton.Joints[JointType.ShoulderCenter].Position.Y &&
                        initPosture == 6
                    )//mouse term posture
            {//right hand at stomach level
                return 6;
            }
            else if(skeleton.Joints[JointType.HandLeft].Position.X > skeleton.Joints[JointType.HandRight].Position.X &&
                skeleton.Joints[JointType.HandLeft].Position.X > skeleton.Joints[JointType.HipLeft].Position.X &&
                skeleton.Joints[JointType.HandLeft].Position.Y > skeleton.Joints[JointType.HandRight].Position.Y &&
                skeleton.Joints[JointType.HandLeft].Position.Y > skeleton.Joints[JointType.HipCenter].Position.Y &&
                skeleton.Joints[JointType.HandLeft].Position.Y < skeleton.Joints[JointType.ShoulderCenter].Position.Y &&
                skeleton.Joints[JointType.HandLeft].Position.Y > skeleton.Joints[JointType.HipLeft].Position.Y &&
                initPosture == 7
                )
            {
                return 7;
            }
            else if (skeleton.Joints[JointType.HandLeft].Position.X < skeleton.Joints[JointType.HandRight].Position.X &&
                skeleton.Joints[JointType.HandLeft].Position.X > skeleton.Joints[JointType.HipLeft].Position.X &&
                skeleton.Joints[JointType.HandLeft].Position.Y > skeleton.Joints[JointType.HandRight].Position.Y &&
                skeleton.Joints[JointType.HandLeft].Position.Y > skeleton.Joints[JointType.HipCenter].Position.Y &&
                skeleton.Joints[JointType.HandLeft].Position.Y < skeleton.Joints[JointType.ShoulderCenter].Position.Y &&
                skeleton.Joints[JointType.HandLeft].Position.Y > skeleton.Joints[JointType.HipLeft].Position.Y &&
                initPosture == 7
            )
            {
                return 8;
            }
            else if (skeleton.Joints[JointType.HandLeft].Position.Y < skeleton.Joints[JointType.HipLeft].Position.Y &&
                skeleton.Joints[JointType.HandLeft].Position.X < skeleton.Joints[JointType.HipLeft].Position.X &&
                skeleton.Joints[JointType.HandRight].Position.Y < skeleton.Joints[JointType.HipRight].Position.Y &&
                skeleton.Joints[JointType.HandRight].Position.X > skeleton.Joints[JointType.HipRight].Position.X
                )//cancel the recording
            {//standby mode
                return -1;
            }
            else return 0;
        }
        
      
        public int ifKeyboard(Skeleton skeleton)
        {
            if (skeleton.Joints[JointType.HandLeft].Position.X >= skeleton.Joints[JointType.HandRight].Position.X &&
                skeleton.Joints[JointType.HandLeft].Position.Y > skeleton.Joints[JointType.HandRight].Position.Y) // commence keyboard mode
                return 1;
            else if (skeleton.Joints[JointType.HandLeft].Position.X >= skeleton.Joints[JointType.HandRight].Position.X &&
                skeleton.Joints[JointType.HandLeft].Position.Y < skeleton.Joints[JointType.HandRight].Position.Y) // close keyboard mode
                return 2;
            else
                return 3;
        }//end of method ifKeyboard

        private double getDiff(double min, double sub)
        {
            return (min - sub);
        }
    }//end class
}//end namespace
