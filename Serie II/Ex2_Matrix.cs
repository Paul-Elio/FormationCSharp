using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serie_II
{
    public static class Matrix
    {
        public static int[][] BuildingMatrix(int[] leftVector, int[] rightVector)
        {
            int[][] mat = new int[leftVector.Length][];
            for (int i =0; i < leftVector.Length; i++)
            {
                mat[i] = new int[rightVector.Length];
                for (int j = 0; j<rightVector.Length; j++)
                {
                    mat[i][j] = leftVector[i] * rightVector[j];
                }
            }
            return mat;
        }

        public static int[][] Addition(int[][] leftMatrix, int[][] rightMatrix)
        {
            int n = leftMatrix[0].Length;
            int m = leftMatrix.Length;
            int[][] mat = new int[m][];
            for (int i = 0; i < m; i++)
            {
                mat[i] = new int[n];
                for (int j = 0; j < n; j++)
                {
                    mat[i][j] = leftMatrix[i][j] + rightMatrix[i][j];
                }
            }
            return mat;
        }

        public static int[][] Substraction(int[][] leftMatrix, int[][] rightMatrix)
        {
            int n = leftMatrix[0].Length;
            int m = leftMatrix.Length;
            int[][] mat = new int[m][];
            for (int i = 0; i < m; i++)
            {
                mat[i] = new int[n];
                for (int j = 0; j < n; j++)
                {
                    mat[i][j] = leftMatrix[i][j] - rightMatrix[i][j];
                }
            }
            return mat;
        }

        public static int[][] Multiplication(int[][] leftMatrix, int[][] rightMatrix)
        {
            int n = rightMatrix.Length;
            int m = leftMatrix.Length;
            int[][] mat = new int[m][];
            for (int i = 0; i < m; i++)
            {
                mat[i] = new int[m];
                for (int j = 0; j < m; j++)
                {
                    int sum = 0;
                    for (int k = 0; k < n; k++)
                    {
                        sum += leftMatrix[i][k] * rightMatrix[k][j];
                    }
                    mat[i][j] = sum;
                }
            }
            return mat;
        }

        public static void DisplayMatrix(int[][] matrix)
        {
            string s = string.Empty;
            for (int i = 0; i < matrix.Length; ++i)
            {
                for (int j = 0; j < matrix[i].Length; ++j)
                {
                    s += matrix[i][j].ToString().PadLeft(5) + " ";
                }
                s += Environment.NewLine;
            }
            Console.WriteLine(s);
        }
    }
}
