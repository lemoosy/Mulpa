using System.Runtime.InteropServices;

namespace _DLL
{
    public class DLL
    {
        // Fonctions pour la PG.

        [DllImport("Basecode_DLL.dll")]
        public static extern void DLL_PG_Init(int p_populationSize, int p_selectionSize, int p_childrenSize, int p_mutationRate);

        [DllImport("Basecode_DLL.dll")]
        public static extern void DLL_PG_Quit();
    
        [DllImport("Basecode_DLL.dll")]
        public static extern void DLL_PG_SetScore(int p_populationIndex, float p_score);

        [DllImport("Basecode_DLL.dll")]
        public static extern float DLL_PG_GetScore(int p_populationIndex);
    
        [DllImport("Basecode_DLL.dll")]
        public static extern bool DLL_PG_Update();

        // Fonctions pour le PCC.

        [DllImport("Basecode_DLL.dll")]
        public static extern float DLL_PCC_1(int[] p_world, int p_w, int p_h, int p_i1, int p_j1, int p_i2, int p_j2);

        [DllImport("Basecode_DLL.dll")]
        public static extern float DLL_PCC_2(int[] p_world, int p_w, int p_h, int p_i1, int p_j1, int p_i2, int p_j2);
    }
}
