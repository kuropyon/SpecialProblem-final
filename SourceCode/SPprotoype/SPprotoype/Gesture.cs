using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;

namespace SPprotoype
{
    class Gesture
    {
        public int initPosture;
        public int termPosture;
        private Data path;
        private Posture posture;
        private ProcessName processName;
        private int gestureNumber;

        public Gesture()
        {
            this.posture = new Posture();
            this.processName = new ProcessName();
            this.initPosture = 0;
            this.termPosture = 0;
        }
        public void setPath(String pathName)
        {
            this.path = new Data(pathName);
        }
        public int getInitPosture(Skeleton skelly)
        {
            this.initPosture = posture.getInitPosture(skelly);
            return initPosture;
        }
        public int getTermPosture(Skeleton skelly, int initPosture)
        {
            this.termPosture = posture.getTermPosture(skelly, initPosture);
            return this.termPosture;
        }

        private int getMode()
        {
            string pathName;
            string pName;
            pathName = processName.GetActiveProcessFilePath();
            pName = processName.getProcessName(pathName);
            return processName.getMode(pName);
        }
    }
}
