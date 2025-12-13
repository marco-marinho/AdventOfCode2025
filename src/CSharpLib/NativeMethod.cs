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
    private const string FortLibName = "fort_lib";
    private const string CppLibName = "highs_cpp";

    [LibraryImport(FortLibName, EntryPoint = "rectangle_areas")]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
    public static unsafe partial void RectangleAreas(
        Coordinate* points,
        DistanceResult* results,
        int num_points
    );

    [LibraryImport(CppLibName, EntryPoint = "mip_solve")]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
    public static unsafe partial double MIPCall(
        double* c,
        double* A,
        double* b,
        int rows,
        int cols
    );
}