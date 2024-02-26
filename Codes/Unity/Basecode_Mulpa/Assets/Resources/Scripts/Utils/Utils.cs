using System.Collections.Generic;
using UnityEngine;

public class Utils
{
    static public System.Random m_randomGenerator = new System.Random();

    static public int Int_Random(int p_a, int p_b)
    {
        Debug.Assert(p_a <= p_b);

        return m_randomGenerator.Next(p_a, p_b + 1);
    }

    static public float Vector2_Distance(Vector2 p_v1, Vector2 p_v2)
    {
        float dx = (p_v1.x - p_v2.x);
        float dy = (p_v1.y - p_v2.y);

        return Mathf.Sqrt(dx * dx + dy * dy);
    }

    static public void List_Randomize<T>(List<T> p_tab)
    {
        int size = p_tab.Count;

        for (int k = 0; k < size - 1; k++)
        {
            int r = Int_Random(k, size - 1);

            T tmp = p_tab[k];
            p_tab[k] = p_tab[r];
            p_tab[r] = tmp;
        }
    }
}
