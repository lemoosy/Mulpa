using System.Runtime.InteropServices;

public class Graph
{
    public enum CaseID
    {
        CASE_VOID = 0,
        CASE_BLOCK,     // blocks + doors
        CASE_DANGER     // spades + lava + monsters
    }

    [DllImport("Assets/Resources/DLL/Basecode_Graph.dll")]
    extern static public float GetSP(int[] p_matrix, int p_w, int p_h, int p_i1, int p_j1, int p_i2, int p_j2, bool p_cross, float p_crossValue = 1.0f);
}
