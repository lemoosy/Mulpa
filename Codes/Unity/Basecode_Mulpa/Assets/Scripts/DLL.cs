using System.Runtime.InteropServices;

namespace _DLL
{
    public class DLL
    {
        [DllImport("Basecode_DLL.dll")]
        public static extern float DLL_PCC(int[] p_world, int p_w, int p_h, int p_i1, int p_j1, int p_i2, int p_j2, bool p_cross);
    }
}
