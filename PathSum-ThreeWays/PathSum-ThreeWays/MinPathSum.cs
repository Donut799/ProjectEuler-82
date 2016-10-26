using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathSum_ThreeWays
{
    class MinPathSum
    {
        private Boolean isValid(Point checkPoint, long[,] matrix)//checks if the point is valid for the given matrix
        {
            return (checkPoint.X < matrix.GetLength(0) && checkPoint.X > -1 && checkPoint.Y < matrix.GetLength(1) && checkPoint.Y > -1);
        }
        public long[,,] minPath(long[,] matrix)
        {
            //add the matrix to a new matrix that has a third dimension that contains data for solving the shortest path
            long[,,] minPathMatrix = new long[matrix.GetLength(0), matrix.GetLength(1), 4];//long[whichColumn,whichRow,(0=value,1=minSumAtPoint,2=xValueParent,3=yValueParent)]
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    minPathMatrix[i, j, 0] = matrix[i, j];//copy the 2d matrix into the first layer of the minPathMatrix
                    if (i == 0)//the sum in the first row is equal to its value
                    {
                        minPathMatrix[i, j, 1] = matrix[i, j];
                    }
                    else//the sum is marked with a negative 1 to designate it is not yet calculated
                    {
                        minPathMatrix[i, j, 1] = -1;
                    }
                    //we are not setting the sumPointer, since that will not be known until it is calculated
                }
            }
            for (int i = 1; i < minPathMatrix.GetLength(0); i++)//x
            {
                for (int j = minPathMatrix.GetLength(1) - 1; j > -1; j--)//y
                {
                    long upSum = long.MaxValue;//these will be lower if they exist, they will be disquilified based on their size if they aren't
                    long leftSum = long.MaxValue;
                    long downSum = long.MaxValue;

                    if (isValid(new Point(i, j + 1), matrix))//up
                    {
                        upSum = minPathMatrix[i, j + 1, 1];
                    }
                    if (isValid(new Point(i - 1, j), matrix))//left
                    {
                        leftSum = minPathMatrix[i - 1, j, 1];
                    }
                    if (isValid(new Point(i, j - 1), matrix))//down
                    {
                        downSum = recursiveMinSumDown(new Point(i, j - 1), minPathMatrix, matrix);
                    }

                    //set the current nodes sum to the lowest sum

                    long smallestSum = long.MaxValue;
                    if (upSum < leftSum && upSum < downSum)//if up is the lowest
                    {
                        smallestSum = upSum;
                        minPathMatrix[i, j, 2] = i;//x coordinate of previous node (doesn't change for up)
                        minPathMatrix[i, j, 3] = j + 1;//y coordinate of previous node
                    }
                    else if (leftSum < downSum)//if left is the lowest
                    {
                        smallestSum = leftSum;
                        minPathMatrix[i, j, 2] = i - 1;//x coordinate of previous node
                        minPathMatrix[i, j, 3] = j;//y coordinate of previous node (doesn't change for left)
                    }
                    else//if down is the lowest
                    {
                        smallestSum = downSum;
                        minPathMatrix[i, j, 2] = i;//x coordinate of previous node (doesn't change for down)
                        minPathMatrix[i, j, 3] = j - 1;//y coordinate of previous node
                    }
                    minPathMatrix[i, j, 1] = minPathMatrix[i, j, 0] + smallestSum;//the nodes sum is itself + the shortest path to get to it
                }
            }
            return minPathMatrix;
        }
        private long recursiveMinSumDown(Point distanceTo, long[,,] minPathMatrix, long[,] matrix)//this will keep looking further down and to the left, deciding whether down or left is better for each
        {
            long leftSum = minPathMatrix[distanceTo.X - 1, distanceTo.Y, 1];//left sum is always previously calculated, so we just need to find out what it is
            long downSum = long.MaxValue;
            if (isValid(new Point(distanceTo.X, distanceTo.Y - 1), matrix))//if moving down isn't valid, then it will remain at long.MaxValue
            {
                downSum = recursiveMinSumDown(new Point(distanceTo.X, distanceTo.Y - 1), minPathMatrix, matrix);//recursive call
            }
            return minPathMatrix[distanceTo.X, distanceTo.Y, 0] + ((leftSum < downSum) ? leftSum : downSum);//return itself + the sum of the path to get to itself
        }
    }
}