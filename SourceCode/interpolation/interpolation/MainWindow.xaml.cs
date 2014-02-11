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

namespace interpolation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string[] lines = System.IO.File.ReadAllLines(@"D:\acads\CMSC190-2\SP\textfiles\nCoords.txt");
        //string[] lines = System.IO.File.ReadAllLines(@"D:\acads\CMSC190-2\SP\textfiles\testCoords2.txt");
        public MainWindow()
        {
            InitializeComponent();
            
        }
        struct element
        {
            public double value;
            public int column;
        }
        

        public Point[] convertToFloat(string[] file) //converts the array of coordinates to point array
        {
            string[] coordPerLine = new string[2];
            Point[] coords = new Point[lines.Length];
            for (int x = 0; x < file.Length; x++)
            {
                 coordPerLine = file[x].Split(' ');
                 coords[x].X = Convert.ToDouble(coordPerLine[0]);
                 coords[x].Y = Convert.ToDouble(coordPerLine[1]);   
            }

            return coords;
        
        }
        public void writeTolineMatrix(double[][] matrix)
        {
            string z;
            for(int i = 0; i < matrix.Length ; i++ )
            {
                for (int j = 0; j < matrix[i].Length; j++)
                {
                    z = Math.Round(matrix[i][j], 3).ToString();
                    textBox2.AppendText(z + "          ");
                }
                textBox2.AppendText("\n");
            }
        }
        public void writeResults(double[] result)
        {
            string z;
            for (int i = 0; i < result.Length; i++)
            {
                z = "x" + (i+1).ToString() + " = "+ result[i].ToString();
                    textBox3.AppendText(z + "\n");
            }
        }
        public double[][] convertToMatrix(Point[] coord,int polyType) //converting the coordinates into a system of equations
        { //poly determines the type of polynomial (quadratic,cubic)
            //
            double[][] matrix = new double[coord.Length][];
            double x = 0;
            for(int i = 0 ; i < coord.Length ; i++)
            {
                matrix[i] = new double[polyType + 2];
                for (int j = 0; j < polyType + 2; j++)
                {
                    if (j >= polyType + 1)
                        matrix[i][j] = coord[i].Y;
                    else
                    {
                        x = polyType - j;
                        if (x < 0)
                            x = 0;
                        matrix[i][j] = Math.Pow(coord[i].X,x);
                    }
                }
            }
            return matrix;
        }

        public double[][] quadSpline(Point[] coords) // quadratic spline interpolation
        {
            int polyType = 2;
            int n = coords.Length-1; //number of intervals (n+1 points, n intervals)
            double[][] matrix = new double[(3*n)-1][]; //minus 1 since a1 = 0
            double[][] finalMatrix = new double[(3 * n) - 1][];
            //so we need to generate 3n equations
            int pow;
            element[][] dummy = new element[(3 * n) - 1][];
            for (int x = 0; x < (3 * n - 1); x++)
            {
                matrix[x] = new double[(3 * n)+1];
                finalMatrix[x] = new double[(3 * n)];
                dummy[x] = new element[polyType + 2];
            }

            for (int i = 0,counter=0,index=2 ; index < coords.Length; i+=2, index++,counter += 3)
            {
                for(int j =0 ; j < polyType+2 ; j++)
                {
                    pow = polyType - j;
                    if (pow < 0)
                        pow = 0;
                    if (j >= polyType + 1)
                    {
                        dummy[i][j].value = coords[index-1].Y;
                        dummy[i][j].column = 3*n;

                        dummy[i+1][j].value = coords[index-1].Y;
                        dummy[i + 1][j].column = 3 * n;
                    }
                    else
                    {
                        dummy[i][j].value = Math.Pow(coords[index-1].X, pow);
                        dummy[i][j].column = j + counter;

                        dummy[i+1][j].value = Math.Pow(coords[index-1].X, pow);
                        dummy[i+1][j].column = j + counter + 3;
                    }
                    
                }
               // textBox3.AppendText("\n");
            }

          
            for (int j = 0 ; j < polyType + 2; j++)
            {
                pow = polyType - j;
                if (j >= polyType + 1)
                {
                    dummy[2*n-2][j].value = coords[0].Y;
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

            for (int i = 2*n, counter = 0,index=2; i < (3*n-1); i++, counter += 3,index++)
            {
                //dummy[i] = new element[polyType+2];
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
                dummy[i][2].column = 0 + counter+3;

                dummy[i][3].value = -1;
                dummy[i][3].column = 1 + counter+3;

            }

            for (int i = 0; i < 3*n-1; i++)
            {
                for (int j = 0; j < polyType+2; j++)
                {
                    matrix[i][dummy[i][j].column] = dummy[i][j].value;
                }
            }

            for (int i = 0; i < 3 * n - 1; i++)
            {
                for (int j = 0; j < 3 * n; j++)
                {
                    finalMatrix[i][j] = matrix[i][j+1];
                }
            }
            /*
            for (int k = 0; k < (3 * n - 1); k++)
            {
                for (int l = 0; l < (3 * n); l++)
                //for (int l = 0; l < polyType + 2; l++)
                {
                    //textBox3.AppendText(dummy[k][l].column.ToString() + "  ");
                    textBox3.AppendText(finalMatrix[k][l].ToString() + "  ");
                }
                textBox3.AppendText("\n");
            }
             */
            return finalMatrix;
        }


        public double[][] solveMatrix(double[][] matrix)
        {
            double[][] finalMatrix = new double[matrix.Length][];
            matrix.CopyTo(finalMatrix,0);
            
            return finalMatrix;
        }


        public void swap(double[] x, double[] y)
        {
           double[] dummy = new double[x.Length];
           x.CopyTo(dummy,0);
           y.CopyTo(x, 0);
           dummy.CopyTo(y,0);

        }
        public void pivot(double[][] matrix, int column)
        {
            int i, j, k;
            if (column < matrix[1].Length)
            {
                for (k = (matrix.Length-column) / 2; k > 0; k /= 2)
                {
                    for (i = k+column; i < matrix.Length; i++)
                    {
                        for (j = i; j > k - 1; j -= k)
                        {
                            if (Math.Abs(matrix[(j - k)][column]) < Math.Abs(matrix[j][column]))
                                swap(matrix[(j - k)], matrix[j]);
                            else break;
                        }
                    }
                }
            }
        }
        public void pivot2(double[][] matrix,int column)
        {
            int indexX;
            double largest;
            largest =  matrix[column][column];
            indexX = column;
            for (int i = column; i < matrix.Length-1; i++)
            {
                if ( Math.Abs(largest) < Math.Abs(matrix[i + 1][column]))
                {
                    largest = matrix[i + 1][column];
                    indexX = i + 1;
                }
            }
            swap(matrix[column], matrix[indexX]);
        }
        public void copyMatrix(double[][] matrix, double[][] copy)
        {
            for (int y = 0; y < matrix.Length; y++)
            {
                copy[y] = new double[matrix[y].Length];
                matrix[y].CopyTo(copy[y], 0);
            }
        }

        public void gaussian(double[][] matrix)
        {
            double[][] dummy = new double[matrix.Length][];
            //copyMatrix(matrix, dummy);
            double x = 0;
            //pivot(dummy, 0);
            for (int k = 0; k < matrix.Length; k++)
            {
                pivot2(matrix, k);
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
        }

        public double[] getValues(double[][] matrix)
        {
            double x = 0,y = 0,f = 0;
            double[] results = new double[matrix.Length];
            for (int i = matrix.Length-1; i >= 0; i--)
            {
                for (int j = matrix[i].Length - 1 ; j >= 0 ; j--)
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
                results[i] = (f-y) / x;
                f=y=x=0;
                //textBox1.AppendText("\n");
            }
            return results;
        }


        public void main()
        {
            double[][] matrix;
            double[] results;
            double[][] finalMatrix;
            //writeTolineMatrix(convertToMatrix(convertToFloat(lines), 3));
            /*
            matrix = solveMatrix(convertToMatrix(convertToFloat(lines), 2));
            gaussian(matrix);
            results = getValues(matrix);
            writeTolineMatrix(matrix);
            writeResults(results);
            */
            finalMatrix = quadSpline(convertToFloat(lines));
           gaussian(finalMatrix);
            results = getValues(finalMatrix);
            writeTolineMatrix(finalMatrix);
            writeResults(results);
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            main();
        }


    }
}
