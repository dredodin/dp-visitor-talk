namespace ProductionExamples;

public static class Array2
{
    private static readonly int[] _oneBasedLowerBounds = new int[] { 1, 1 };

    public static T[,] CreateExcelMatrix<T>(int rowsCount, int columnsCount, bool isTransposed)
    {
        var (x, y) = isTransposed ? (columnsCount, rowsCount) : (rowsCount, columnsCount);
        return (T[,])Array.CreateInstance(typeof(T), new int[] { x, y }, _oneBasedLowerBounds);
    }

    /// <param name="createElement">(clm, row) => elem</param>
    public static T[,] CreateExcelMatrix<T>(int rowCount, int clmCount, bool isTransposed, Func<int, int, T> createElement)
    {
        var matrix = CreateExcelMatrix<T>(rowCount, clmCount, isTransposed);
        for (int r = matrix.GetLowerBound(0); r <= matrix.GetLength(0); r++)
        {
            for (int c = matrix.GetLowerBound(1); c <= matrix.GetLength(1); c++)
            {
                matrix[r, c] = isTransposed ? createElement(r - 1, c - 1) : createElement(c - 1, r - 1);
            }
        }

        return matrix;
    }
}
