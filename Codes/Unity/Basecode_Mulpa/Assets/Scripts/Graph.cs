using System.Runtime.InteropServices;

namespace _Graph
{
    public class Graph
    {
        [DllImport("Basecode_Graph.dll")]
        public static extern float GetSP(int[] p_world, int p_w, int p_h, int p_i1, int p_j1, int p_i2, int p_j2, bool p_cross);
    }
}
