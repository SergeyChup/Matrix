namespace WebApplication1.Services
{
    //этот функционал не используется так как - решил поворачивать масив на клиенте 
    //чтоб не загружать сервер той работой которую можно сделать на клиенте
    //генерировать рандомную матрицу тоже лучше на стороне клиента
    public static class RotateMatrix90
    {
        public static int[,] Rotate(this int[,] array)
        {
            int n = array.GetLength(0);
            int m = array.GetLength(1);

            int[,] res = new int[m, n];

            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                    res[j, n - i - 1] = array[i, j];

            return res;
        }
    }
}