using System.Text;

namespace CSharpLib;

public class Helpers
{

    public static ulong Distance3D((ulong, ulong, ulong) a, (ulong, ulong, ulong) b)
    {
        var dx = a.Item1 - b.Item1;
        var dy = a.Item2 - b.Item2;
        var dz = a.Item3 - b.Item3;
        return (ulong)Math.Sqrt(dx * dx + dy * dy + dz * dz);
    }

    public static void ComputeAreas(
        ReadOnlySpan<Coordinate> points,
        Span<DistanceResult> results)
    {
        unsafe
        {
            fixed (Coordinate* pPoints = points)
            fixed (DistanceResult* pResults = results)
            {
                NativeMethods.RectangleAreas(pPoints, pResults, points.Length);
            }
        }
    }

    public static double MIPSolve(
        double[] c,
        double[,] A,
        double[] b)
    {
        int rows = A.GetLength(0);
        int cols = A.GetLength(1);
        unsafe
        {
            fixed (double* pC = c)
            fixed (double* pA = A)
            fixed (double* pB = b)
            {
                return NativeMethods.MIPCall(pC, pA, pB, rows, cols);
            }
        }
    }

    public static void FillPolygonGrid(Coordinate[][] vertices, int[,] grid)
    {
        foreach (Coordinate[] pair in vertices)
        {
            Coordinate p1 = pair[0];
            Coordinate p2 = pair[1];

            if (p1.X == p2.X)
            {
                int yStart = Math.Min(p1.Y, p2.Y);
                int yEnd = Math.Max(p1.Y, p2.Y);

                for (int y = yStart; y <= yEnd; y++)
                {
                    grid[p1.X, y] = 1;
                }
            }
            else if (p1.Y == p2.Y)
            {
                int xStart = Math.Min(p1.X, p2.X);
                int xEnd = Math.Max(p1.X, p2.X);

                for (int x = xStart; x <= xEnd; x++)
                {
                    grid[x, p1.Y] = 1;
                }
            }
        }
    }

    public static void PrintGrid(int[,] grid)
    {
        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);
        var sb = new StringBuilder(capacity: cols);

        for (int i = 0; i < rows; i++)
        {
            sb.Clear();
            for (int j = 0; j < cols; j++)
            {
                sb.Append(grid[i, j] == 1 ? '#' : '.');
            }
            Console.WriteLine(sb.ToString());
        }
    }

    public static void FloodFill(int[,] grid, int y)
    {
        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);
        var stack = new Stack<(int x, int y)>();
        stack.Push((1, y));
        while (stack.Count > 0)
        {
            var (x, yPos) = stack.Pop();
            if (x < 0 || x >= rows || yPos < 0 || yPos >= cols)
                continue;
            if (grid[x, yPos] != 0)
                continue;

            grid[x, yPos] = 1; // Mark as filled

            stack.Push((x + 1, yPos));
            stack.Push((x - 1, yPos));
            stack.Push((x, yPos + 1));
            stack.Push((x, yPos - 1));
        }
    }

    public static int[,] IntegralImage(int[,] grid)
    {
        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);
        int[,] integral = new int[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                integral[i, j] = grid[i, j];
                if (i > 0) integral[i, j] += integral[i - 1, j];
                if (j > 0) integral[i, j] += integral[i, j - 1];
                if (i > 0 && j > 0) integral[i, j] -= integral[i - 1, j - 1];
            }
        }

        return integral;
    }
    
}
