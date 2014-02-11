using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace SPprotoype
{
    class ParallelCurve // this class calculate the given data and tells if the given data is parallel to the base curve
    {
        private Point[] baseCoords;
        private Point[] newCoords;
        private Point[] expectedValues;
        private double[] equation;
        private double k;
        private double nearness;
        private double[] prime;
        private EquationList[] equationList;
        private Point[] pointInterval;

        public ParallelCurve(Point[] bCoords, Point[] nCoords,double[] bEquation,EquationList[] eqList,Point[] interval)
        {
            baseCoords = bCoords;
            newCoords = nCoords;
            equation = bEquation;
            equationList = eqList;
            pointInterval = interval;
            main();
        }
        public void main()
        {
            this.k = computeDistance();
            expectedValues = computeExValues(this.baseCoords, this.k);
            nearness = findK(expectedValues, newCoords);
            saveValues();
        }
        public double getNearness()
        {
            return this.nearness;
        }
        public Point[] getExpectedValues()
        {
            return this.expectedValues;
        }
        public double computeDistance()
        {
            return findK(this.baseCoords, this.newCoords);
        }
        private int findLength(int l1, int l2)
        {
            if (l1 <= l2)
                return l1;
            else
                return l2;
        }
        /*
        private double findError(Point[] bCoords, Point[] nCoords)
        {
            double[] k = new double[bCoords.Length];
            double _k = 0;
            int n = findLength(bCoords.Length,nCoords.Length);
            for (int i = 0; i <; i++)
            {
                k[i] 
            }
        }*/
        private double findK(Point[] bCoords, Point[] nCoords) //this function will find k(distance of the base path to the new path)
        {//assume that bMatrix.length == nMatrix.length
            double[] k = new double[bCoords.Length];
            double _k = 0;
            int n = findLength(bCoords.Length,nCoords.Length);
            for (int i = 0; i < n; i++)
            {
                //distance formula 
                k[i] = Math.Sqrt(Math.Pow((bCoords[i].X - nCoords[i].X), 2) + Math.Pow((bCoords[i].Y - nCoords[i].Y), 2));
            }
            _k = findMean(k);
            return _k;
        }

        private double findMean(double[] array) //finding the average of the distances
        {
            double mean = 0;
            for (int i = 0; i < array.Length; i++)
            {
                mean += array[i];
            }
            mean = mean / array.Length;
            return mean;
        }//end of findMean

        private double getPX(double K,double[] yPrime,Point bCoords) // get the new set of X's
        {//assumption that yPrime is linear. all equations are quadratic in nature
            double X = 0;
            double dY = 0;
            for (int i = 0; i < yPrime.Length; i++)
            {
                if (i == 0)
                    dY += yPrime[i] * bCoords.X;
                else
                    dY += yPrime[i];
            }
            X = bCoords.X - ((K*dY)/(Math.Sqrt(1+Math.Pow(dY,2))));
                return X;
        }//end of getPX

        private double getPY(double K, double[] yPrime, Point bCoords) // get the new set of Y's
        {
            double Y = 0;
            double dY = 0;
            for (int i = 0; i < yPrime.Length; i++)
            {
                if (i == 0)
                    dY += yPrime[i] * bCoords.X;
                else
                    dY += yPrime[i];
            }
            Y = bCoords.Y + (K / (Math.Sqrt(1 + Math.Pow(dY, 2))));
            return Y;
        }//end of getPy

        /*
        private double[] getDerivative(double[] nEquation)
        {
            double[] yPrime = new double[nEquation.Length - 1];
            for (int i = 0; i < yPrime.Length; i++)
            {
                yPrime[i] = Math.Pow(nEquation[i], (yPrime.Length - 1) - i);
            }
            this.prime = yPrime;
            return yPrime;
        }* */
        private EquationList[] getDerivative(EquationList[] nEquation) // this function will get the derivative
        {
            for(int i = 0 ; i < nEquation.Length ; i++)
            {
                nEquation[i].derivative = new double[nEquation[i].equation.Length-1];
                for (int j = 0; j < nEquation[i].derivative.Length; j++)
                {
                    nEquation[i].derivative[j] = Math.Pow(nEquation[i].equation[j], (nEquation[i].derivative.Length-1) - j);
                }
            }
            return nEquation;
        }

        private Point[] computeExValues(Point[] bCoords,double k) // this will compute for the expected values
        {
            Point[] expectedValues = new Point[bCoords.Length];
            EquationList[] equationPrime = getDerivative(this.equationList);
            for (int i = 0; i < bCoords.Length; i++)
            {
                for (int j = 1; j < pointInterval.Length; j++)
                {

                    if (bCoords[i].X <= pointInterval[j].X || bCoords[i].X >= pointInterval[j].X)
                    {
                        expectedValues[i].X = getPX(k, equationPrime[j-1].derivative, bCoords[i]);
                        expectedValues[i].Y = getPY(k, equationPrime[j-1].derivative, bCoords[i]);
                        break;
                    }
                }
            }
            return expectedValues;
        }

        private void saveValues() // save the data into text files
        {
            String[] files = new String[baseCoords.Length];
            String[] files2 = new String[newCoords.Length];
            String[] files3 = new String[expectedValues.Length];
            String[] files4 = new String[equation.Length];
            for (int i = 0; i < baseCoords.Length; i++)
            {
                files[i] = baseCoords[i].X.ToString() + " " + baseCoords[i].Y.ToString();
            }
            System.IO.File.WriteAllLines(@"C:\SourceCode\baseCoord.txt", files);

            for (int i = 0; i < newCoords.Length; i++)
            {
                files2[i] = newCoords[i].X.ToString() + " " + newCoords[i].Y.ToString();
            }
            System.IO.File.WriteAllLines(@"C:\SourceCode\newCoords.txt", files2);

            for (int i = 0; i < expectedValues.Length; i++)
            {
                files3[i] = expectedValues[i].X.ToString() + " " + expectedValues[i].Y.ToString();
            }
            System.IO.File.WriteAllLines(@"C:\SourceCode\expectedValues.txt", files3);

            for (int i = 0; i < equation.Length; i++)
            {
                files4[i] = equation[i].ToString();
            }
            System.IO.File.WriteAllLines(@"C:\SourceCode\equation.txt", files4);

        }
    }
}
