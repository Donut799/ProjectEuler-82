using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//Date = October 20th 2016
//Author = Brendan, Smith
//Project = MinPath-ThreeWays

//Objective of program: To retreive an array of numbers and to find the 
//path that has the lowest sum going from any point on the far left, to any 
//point on the far right. The path can only go Up, Right, and Down.


namespace PathSum_ThreeWays
{
    public partial class Form1 : Form
    {
        public long[,] solveMatrix;
        public Form1()
        {
            InitializeComponent();
        }
        private void solve_Click(object sender, EventArgs e)
        {
            solveMatrix = null;
            try//get the text off the textField and process it
            {
                solveMatrix = textToArray();
            }
            catch (Exception)
            {
                this.label1.Text = "Matrix was not valid.";
            }

            if (solveMatrix == null)
            {
                Console.WriteLine("Error: Matrix has not been recieved");
            }
            else
            {
                MinPathSum solve = new MinPathSum();
                long[,,] minPathArray = solve.minPath(solveMatrix);
                //long minPathSum = MinPathSum.pathSum(minPath, solveMatrix);

                long minSumRow = long.MaxValue;
                int minRowPlacement = int.MaxValue;
                for (int i = 0; i < minPathArray.GetLength(1); i++)
                {
                    //find the lowest pathSum
                    if (minPathArray[minPathArray.GetLength(0) - 1, i, 1] < minSumRow)
                    {
                        minSumRow = minPathArray[minPathArray.GetLength(0) - 1, i, 1];
                        minRowPlacement = i;
                    }
                }
                String path = recursiveReportPath(new Point(minPathArray.GetLength(0) - 1, minRowPlacement), minPathArray);

                path += " -> SUM = " + minPathArray[minPathArray.GetLength(0) - 1, minRowPlacement, 1];
                MessageBox.Show(path);
                //unecessary, but can be included
                for (int i = 0; i < minPathArray.GetLength(1); i++)
                {
                    for (int j = 0; j < minPathArray.GetLength(0); j++)
                    {
                        Console.Write(minPathArray[j, i, 1] + " ");
                    }
                    Console.WriteLine();
                }
            }
        }
        private String recursiveReportPath(Point startPoint, long[,,] minPathArray)
        {
            Point previousPoint = new Point((int)minPathArray[startPoint.X, startPoint.Y, 2], (int)minPathArray[startPoint.X, startPoint.Y, 3]);
            if (startPoint.X == 0) return "" + minPathArray[startPoint.X, startPoint.Y, 0];
            return recursiveReportPath(previousPoint, minPathArray) + " -> " + minPathArray[startPoint.X, startPoint.Y, 0];
        }
        public long[,] textToArray()
        {
            String matrix = this.richTextBox1.Text.Trim();
            int rows = 0;
            int columns = 0;
            String partial = "";
            String[] rowArray = matrix.Split();
            rows = rowArray.Length;
            foreach (String line in rowArray)
            {
                int verifyColumnsLength = 0;//make sure all columns are same length
                partial = "";
                foreach (char i in line)
                {
                    if (i >= '0' && i <= '9')//if character is a number
                    {
                        partial += i;
                    }
                    else
                    {
                        if (partial != "")//we found the end of a number
                        {
                            verifyColumnsLength++;//count the number
                            partial = "";//reset the string that stored the number
                        }
                    }
                }
                //if the last section of the string was a number
                if (partial != "")
                {
                    verifyColumnsLength++;
                }
                if (columns != 0 && columns != verifyColumnsLength)
                {
                    throw new Exception("Matrix was not valid");
                }
                columns = verifyColumnsLength;
            }
            this.label1.Text = "Verified";
            //we now know that the matrix is valid(same number of rows for each column and vice versa)
            //and we know the dimensions, so we can process the string again and put all the values in an array
            Console.WriteLine("there are " + columns + "columns and " + rows + "rows");
            long[,] returnMatrix = new long[columns, rows];
            int rowPointer = 0;
            int columnPointer = 0;
            foreach (String line in rowArray)
            {
                partial = "";
                foreach (char i in line)
                {
                    if (i >= '0' && i <= '9')//if character is a number
                    {
                        partial += i;
                        continue;
                    }
                    else
                    {
                        if (partial != "")//we found the end of a number
                        {
                            returnMatrix[columnPointer, rowPointer] = Int32.Parse(partial);
                            partial = "";//reset the string that stored the number

                        }
                    }

                    columnPointer++;
                }
                //if the last section of the string was a number
                if (partial != "")
                {
                    returnMatrix[columnPointer, rowPointer] = Int32.Parse(partial);
                    partial = "";//reset the string that stored the number
                }
                columnPointer = 0;
                rowPointer++;
            }
            Console.WriteLine("bottom right most value is = " + returnMatrix[returnMatrix.GetLength(0) - 1, returnMatrix.GetLength(1) - 1]);
            return returnMatrix;

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
