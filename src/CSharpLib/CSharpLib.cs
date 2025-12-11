using System.Runtime.InteropServices;

namespace CSharpLib;

[StructLayout(LayoutKind.Sequential)]
public struct Coordinate
{
    public int X;
    public int Y;
    public override string ToString()
    {
        return $"Coordinate {{ X = {X}, Y = {Y}}}";
    }
}

[StructLayout(LayoutKind.Sequential)]
public struct DistanceResult
{
    public int I;
    public int J;
    public long Area; // int64_t maps to C# long

    public override string ToString()
    {
        return $"DistanceResult {{ I = {I}, J = {J}, Area = {Area} }}";
    }
}

public static partial class NativeMethods
{
    private const string LibName = "libfort_lib.dll";

    [LibraryImport(LibName, EntryPoint = "rectangle_areas")]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
    public static unsafe partial void RectangleAreas(
        Coordinate* points,
        DistanceResult* results,
        int num_points
    );
}

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

}
