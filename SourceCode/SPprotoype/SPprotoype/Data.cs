using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.IO;

namespace SPprotoype
{
    class Data
    {
        private double[][] matrix;
        private double[][] nMatrix;
        private double[][] oMatrix;
        private string[] lines;
        private Point[] coords;
        private Point[] coords2;
        public double[] bEquation;
        private bool tryAgain = false;
        public EquationList[] eqList;
        int counter = 0;

        private struct finalEquation
        {
            double minDomain;
            double maxDomain;
            double[] equation;
        }
        private struct element
        {
            public double value;
            public int column;
        }


        public Data(string fileName) // if type = 1  -> whole folder, type = 2 -> specific file 
        {
            //counter = 0;
            string[] lines = System.IO.File.ReadAllLines(@"C:\SourceCode\" + fileName);
            coords = convertToPointArray(lines);
            if (coords.Length > 10)
            {
                coords2 = getPoints(coords);
                this.matrix = quadSpline(coords2);
                this.oMatrix = this.matrix;
                gaussian(this.matrix);
                bEquation = getEqValues(this.matrix);
                eqList = finalValues(this.matrix);
                tryAgain = false;
            }
            else
            {
                tryAgain = true;
            }
            
        }//end of contructor Data
        public bool Exception()
        {
            return tryAgain;
        }
        public double[][] getMatrix()
        {
            return this.matrix;
        }
        public Point[] getCoords()
        {
            return this.coords;
        }
        public Point[] getCoords2()
        {
            return this.coords2;
        }
        public double[] getEquation()
        {
            return this.bEquation;
        }

        private Point[] convertToPointArray(string[] file) //converts the array of strings to an array of points
        {
            string[] coordPerLine = new string[2];
            Point[] coords = new Point[file.Length];
            for (int x = 0; x < file.Length; x++)
            {
                coordPerLine = file[x].Split(' ');
                coords[x].X = Convert.ToDouble(coordPerLine[0]);
                coords[x].Y = Convert.ToDouble(coordPerLine[1]);
            }

            return coords;

        }//end of method convertToPointArray

        private Point[] getPoints(Point[] coords) //gets the point of interval
        {
            int jump = coords.Length;
            Point[] points;
            if (jump % 4 != 0)
            {
                points = new Point[((jump - (jump % 4)) / 4) + 1];
                points[(jump / 4) - 1] = coords[jump - 1];
            }
            else
                points = new Point[((jump - (jump % 4)) / 4)];

            for (int i = 0,j=0; i < jump; i += 4,j++)
            {
                points[j] = coords[i];
            }
            return points;
        }
        private double[][] quadSpline(Point[] coords) // quadratic spline interpolation
        {
            int polyType = 2;
            int n = coords.Length - 1; //number of intervals (n+1 points, n intervals)
            double[][] matrix = new double[(3 * n) - 1][]; //minus 1 since a1 = 0
            double[][] finalMatrix = new double[(3 * n) - 1][];
            //so we need to generate 3n equations
            int pow;
            element[][] dummy = new element[(3 * n) - 1][];
            for (int x = 0; x < (3 * n - 1); x++)
            {
                matrix[x] = new double[(3 * n) + 1];
                finalMatrix[x] = new double[(3 * n)];
                dummy[x] = new element[polyType + 2];
            }

            for (int i = 0, counter = 0, index = 2; index < coords.Length; i += 2, index++, counter += 3)
            {
                for (int j = 0; j < polyType + 2; j++)
                {
                    pow = polyType - j;
                    if (pow < 0)
                        pow = 0;
                    if (j >= polyType + 1)
                    {
                        dummy[i][j].value = coords[index - 1].Y;
                        dummy[i][j].column = 3 * n;

                        dummy[i + 1][j].value = coords[index - 1].Y;
                        dummy[i + 1][j].column = 3 * n;
                    }
                    else
                    {
                        dummy[i][j].value = Math.Pow(coords[index - 1].X, pow);
                        dummy[i][j].column = j + counter;

                        dummy[i + 1][j].value = Math.Pow(coords[index - 1].X, pow);
                        dummy[i + 1][j].column = j + counter + 3;
                    }

                }
               
            }


            for (int j = 0; j < polyType + 2; j++)
            {
                pow = polyType - j;
                if (j >= polyType + 1)
                {
                    dummy[2 * n - 2][j].value = coords[0].Y;
                    dummy[2 * n - 2][j].column = 3 * n;

                    dummy[2 * n - 1][j].value = coords[n].Y;
                    dummy[2 * n - 1][j].column = 3 * n;
                }
                else
                {
                    dummy[2 * n - 2][j].value = Math.Pow(coords[0].X, pow);
                    dummy[2 * n - 2][j].column = j;

                    dummy[2 * n - 1][j].value = Math.Pow(coords[n].X, pow);
                    dummy[2 * n - 1][j].column = (3 * n) - n + j;
                }
            }

            for (int i = 2*n, counter = 0, index = 2; i < (3 * n - 1); i++, counter += 3, index++)
            {
                if (index == coords.Length)
                {
                    index = 2;
                    counter = 0;
                }
               
                dummy[i][0].value = 2 * coords[index - 1].X;
                dummy[i][0].column = 0 + counter;

                dummy[i][1].value = 1;
                dummy[i][1].column = 1 + counter;

                dummy[i][2].value = 2 * -coords[index - 1].X;
                dummy[i][2].column = 0 + counter + 3;

                dummy[i][3].value = -1;
                dummy[i][3].column = 1 + counter + 3;
            }

            for (int i = 0; i < 3 * n - 1; i++)
            {
                for (int j = 0; j < polyType + 2; j++)
                {
                    matrix[i][dummy[i][j].column] = dummy[i][j].value;
                }
            }

            for (int i = 0; i < 3 * n - 1; i++)
            {
                for (int j = 0; j < 3 * n; j++)
                {
                    finalMatrix[i][j] = matrix[i][j + 1];
                }
            }
            return finalMatrix;
        }//end of method quadSpline


        private void swap(double[] x, double[] y)
        {
            double[] dummy = new double[x.Length];
            x.CopyTo(dummy, 0);
            y.CopyTo(x, 0);
            dummy.CopyTo(y, 0);

        }//end of method swap

        private void pivot(double[][] matrix, int column)
        {
            int indexX;
            double largest;
            largest = matrix[column][column];
            indexX = column;
            for (int i = column; i < matrix.Length - 1; i++)
            {
                if (Math.Abs(largest) < Math.Abs(matrix[i + 1][column]))
                {
                    largest = matrix[i + 1][column];
                    indexX = i + 1;
                }
            }
            swap(matrix[column], matrix[indexX]);
        }//end of method pivot

        private void copyMatrix(double[][] matrix, double[][] copy)
        {
            for (int y = 0; y < matrix.Length; y++)
            {
                copy[y] = new double[matrix[y].Length];
                matrix[y].CopyTo(copy[y], 0);
            }
        }//end of method copyMatrix

        public void gaussian(double[][] matrix) // this will solve the matrix using gaussian elimination
        {
            double[][] dummy = new double[matrix.Length][];
            //copyMatrix(matrix, dummy);
            double x = 0;
            //pivot(dummy, 0);
            for (int k = 0; k < matrix.Length; k++)
            {
                pivot(matrix, k);
                copyMatrix(matrix, dummy);
                for (int i = k; i < matrix.Length; i++)
                {
                    for (int j = k; j < matrix[i].Length; j++)
                    {
                        if (j < matrix[i].Length && i < matrix.Length - 1)
                        {
                            x = dummy[i + 1][j] - ((dummy[i + 1][k] / dummy[k][k]) * dummy[k][j]);
                            /*
                            textBox3.AppendText(dummy[i + 1][j].ToString() + " " + dummy[i + 1][k].ToString()
                                + " " + dummy[k][k].ToString() + " " + dummy[k][j].ToString() + "\n");
                              */
                            matrix[i + 1][j] = x;
                        }
                    }
                    //if (i == k + 2) break;
                }
                //if (k == 1) break;
            }
        }//end of method gaussian
        public EquationList[] finalValues(double[][] matrix) // this will get the final values
        {
            double[] values = getEqValues(matrix);
            this.eqList = new EquationList[values.Length / 3];
            for (int i = 0, j = 0; i < coords2.Length - 1; i++,j+=3)
            {
                this.eqList[j/3] = new EquationList(3, coords2[i].X, coords2[i + 1].X);
            }
            for (int i = 0,counter = 0; i < eqList.Length; i ++)
            {
                for (int j = 0; j < 3; j++)
                {
                    this.eqList[i].equation[j] = values[counter++];
                }
            }
                return this.eqList;
        }//end of method finalValues
        
        public double[] getEqValues(double[][] matrix)// includes a1 = 0
        {
            double[] values = getValues(matrix);
            double[] finalValues = new double[values.Length + 1];
            finalValues[0] = 0;
            for (int i = 1; i < values.Length + 1; i++)
            {
                finalValues[i] = values[i - 1];
            }
                return finalValues;
        }//end of method getEqValues
      
        private double[] getValues(double[][] matrix) // raw values from quadspline 
        {
            double x = 0, y = 0, f = 0;
            double[] results = new double[matrix.Length];
            for (int i = matrix.Length - 1; i >= 0; i--)
            {
                for (int j = matrix[i].Length - 1; j >= 0; j--)
                {
                    if (j == i)
                        x = matrix[i][j];
                    else if (j == matrix[i].Length - 1)
                    {
                        f = matrix[i][j];
                        // textBox1.AppendText(f.ToString()+ ",");
                    }
                    else
                    {
                        y += matrix[i][j] * results[j];
                        // textBox1.AppendText(y.ToString() + ",");
                    }
                }
                results[i] = (f - y) / x;
                f = y = x = 0;
                //textBox1.AppendText("\n");
            }
            return results;
        }//end of getValues

        public void saveIt() // save some data into textfiles for debugging
        {
            String[] file1 = new string[matrix.Length];
            String[] file2 = new string[oMatrix.Length];
            String[] file3 = new string[bEquation.Length];
            String[] file4 = new string[coords2.Length];
            
            
            for (int i = 0; i < matrix.Length; i++)
            {
                for (int j = 0; j < matrix[i].Length; j++)
                {
                    file1[i] = file1[i] + matrix[i][j].ToString()+" ";
                }
            }
            System.IO.File.WriteAllLines(@"C:\SourceCode\matrix.txt", file1);

            for (int i = 0; i < oMatrix.Length; i++)
            {
                for (int j = 0; j < oMatrix[i].Length; j++)
                {
                    file2[i] = file2[i] + oMatrix[i][j].ToString()+ " ";
                }
            }
            System.IO.File.WriteAllLines(@"C:\SourceCode\oMatrix.txt", file2);
            
            for (int i = 0; i < bEquation.Length; i++)
            {
                file3[i] = bEquation[i].ToString();
            }
            System.IO.File.WriteAllLines(@"C:\SourceCode\bEquation.txt", file3);


            for (int i = 0; i < coords2.Length; i++)
            {
                file4[i] = coords2[i].X.ToString() + " " + coords2[i].Y.ToString();
            }
            System.IO.File.WriteAllLines(@"C:\SourceCode\nCoords.txt", file4);
        }//end of method saveIt
       
    }//end of class Data

}//end of name space SPprototype
