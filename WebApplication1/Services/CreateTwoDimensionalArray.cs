using System;
using System.Text;
using WebApplication1.Interfaces;

namespace WebApplication1.Services
{
    public class CreateTwoDimensionalArray : ICreateRandomArray
    {
        static Random random = new Random();
        int[,] array;
        
        public int[,] CreateRandomArray(int m, int n, int min, int max)
        { 
            lock (random)
            {
                array = new int[m, n];
                for (int i = 0; i < m; i++)
                    for (int j = 0; j < n; j++)
                        array[i, j] = random.Next(min, max);
            }

            return array;
        }
        
        //override public string ToString()
        //{
        //    var sb = new StringBuilder();
        //    for (int i = 0; i < array.GetLength(0); i++)
        //    {
        //        for (int j = 0; j < array.GetLength(1); j++)
        //            sb.AppendFormat("{0,2}\t", array[i, j]);
        //        sb.AppendLine();
        //    }
        //    return sb.ToString();
        //}
    }
}