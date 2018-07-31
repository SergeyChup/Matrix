using System;
using System.Text;
using WebApplication1.Interfaces;

namespace WebApplication1.Services
{
    public class CreateCSV : ICreateFile
    {
        public StringBuilder CreateFile(int[][] matrix)
        {
            var sbCSV = new StringBuilder();

            if (matrix != null)
            {
                for (int i = 0; i < matrix.Length; ++i)
                {
                    var sb = new StringBuilder();
                    for (int j = 0; j < matrix[i].Length; ++j)
                    {
                        var row = matrix[i][j] + ",";
                        sb.Append(row);
                    }
                    sbCSV.Append(sb + Environment.NewLine);
                }
            }
            return sbCSV;
        }
    }
}