namespace CSharpLib;

public class Helpers
{
    public ulong Distance3D((ulong, ulong, ulong) a, (ulong, ulong, ulong) b)
    {
        var dx = a.Item1 - b.Item1;
        var dy = a.Item2 - b.Item2;
        var dz = a.Item3 - b.Item3;
        return (ulong)Math.Sqrt(dx * dx + dy * dy + dz * dz);
    }

}
