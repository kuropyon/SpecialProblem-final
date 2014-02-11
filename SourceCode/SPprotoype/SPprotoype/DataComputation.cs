using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsInput;
using System.Windows;

namespace SPprotoype
{
    class DataComputation
    {
        Data leftButton;
        Data rightButton;
        Data open;
        Data close;
        ParallelCurve curve;
        Form3 keyboard;
        Graph graph = new Graph();
        
        public DataComputation()
        {
            initBasedCoords();
        }

        void initBasedCoords()
        {
            leftButton = new Data("presentationRight.txt");
            rightButton = new Data("presentationLeft.txt");
            open = new Data("openFile.txt");
            close = new Data("closeFile.txt");
        }
        public double analyzeData(int gestureNumber,Data nDataL, Data nDataR) // this will analyze and compare the given data/path to the based path.
        {//this will determine if the gesture is the same (parallel) to the defined gesture
            double nearness = 0;
           // if (graph.Visible)
             //   graph.Close();
            switch (gestureNumber)
            {
                case 1: 
                    if(!nDataL.Exception())
                        curve = new ParallelCurve(leftButton.getCoords(), nDataL.getCoords(), leftButton.bEquation, leftButton.eqList, nDataL.getCoords2());
                    if ((nearness = curve.getNearness()) < 0.15)
                    {
                        InputSimulator.SimulateKeyPress(VirtualKeyCode.RIGHT);
                        graph.setDataPoint(leftButton.getCoords(),curve.getExpectedValues(),nDataL.getCoords());
                        graph.Location = new System.Drawing.Point(0, graph.Height);
                        graph.Show();
                    }
                    break;
                case 2:
                    if (!nDataR.Exception())
                        curve = new ParallelCurve(rightButton.getCoords(), nDataR.getCoords(), rightButton.bEquation, rightButton.eqList, nDataR.getCoords2());
                    if ((nearness = curve.getNearness()) < 0.15)
                    {
                        InputSimulator.SimulateKeyPress(VirtualKeyCode.LEFT);
                        graph.setDataPoint(rightButton.getCoords(), curve.getExpectedValues(), nDataR.getCoords());
                        graph.Location = new System.Drawing.Point(0, graph.Height);
                        graph.Show();
                    }
                    break;
                case 3:
                    if (!nDataL.Exception())
                        curve = new ParallelCurve(open.getCoords(),nDataL.getCoords(),open.bEquation,open.eqList,nDataL.getCoords2());
                    if ((nearness = curve.getNearness()) < 0.15)
                    {
                        InputSimulator.SimulateKeyPress(VirtualKeyCode.RETURN);
                        graph.setDataPoint(open.getCoords(), curve.getExpectedValues(), nDataL.getCoords());
                        graph.Location = new System.Drawing.Point(0, graph.Height);
                        graph.Show();
                    }
                    break;
                case 4:
                    if (!nDataR.Exception())
                        curve = new ParallelCurve(close.getCoords(),nDataR.getCoords(),close.bEquation,close.eqList,nDataR.getCoords2());
                    if ((nearness = curve.getNearness()) < 0.15)
                    {
                        InputSimulator.SimulateModifiedKeyStroke(VirtualKeyCode.MENU, VirtualKeyCode.F4);
                        graph.setDataPoint(close.getCoords(), curve.getExpectedValues(), nDataR.getCoords());
                        graph.Location = new System.Drawing.Point(0, graph.Height);
                        graph.Show();
                    }
                    break;
                case 7:
                    InputSimulator.SimulateKeyPress(VirtualKeyCode.TAB);
                    break;
                case 8:
                    InputSimulator.SimulateModifiedKeyStroke(VirtualKeyCode.SHIFT, VirtualKeyCode.TAB);
                    break;
            }
            return nearness;
        }
    }
}
