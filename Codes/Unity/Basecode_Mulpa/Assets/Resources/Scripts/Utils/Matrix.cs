using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Matrix
{
    public int m_w;

    public int m_h;
    
    public int[] m_matrix;

    public Matrix(int p_w, int p_h)
    {
        Debug.Assert(p_w > 0);
        Debug.Assert(p_h > 0);

        m_w = p_w;
        m_h = p_h;

        m_matrix = new int[p_w * p_h];
    }

    public bool OutOfDimension(int p_i, int p_j)
    {
        return ((p_i < 0) || (p_i >= m_w) || (p_j < 0) || (p_j >= m_h));
    }

    public int Get1x1(int p_i, int p_j)
    {
        Debug.Assert(!OutOfDimension(p_i, p_j));

        return m_matrix[p_j * m_w + p_i];
    }

    // Set.

    public void Set1x1(int p_i, int p_j, int p_value)
    {
        Debug.Assert(!OutOfDimension(p_i, p_j));
        
        m_matrix[p_j * m_w + p_i] = p_value;
    }

    public void Set2x2(int p_i, int p_j, int p_value)
    {
        Set1x1(p_i, p_j, p_value);
        Set1x1(p_i + 1, p_j, p_value);
        Set1x1(p_i, p_j + 1, p_value);
        Set1x1(p_i + 1, p_j + 1, p_value);
    }

    public void TrySet1x1(int p_i, int p_j, int p_value)
    {
        if (!OutOfDimension(p_i, p_j))
        {
            Set1x1(p_i, p_j, p_value);
        }
    }

    public void TrySet2x2(int p_i, int p_j, int p_value)
    {
        TrySet1x1(p_i, p_j, p_value);
        TrySet1x1(p_i + 1, p_j, p_value);
        TrySet1x1(p_i, p_j + 1, p_value);
        TrySet1x1(p_i + 1, p_j + 1, p_value);
    }

    // Is.

    public bool Is1x1(int p_i, int p_j, int p_value)
    {
        return (Get1x1(p_i, p_j) == p_value);
    }

    public bool Is2x2(int p_i, int p_j, int p_value)
    {
        return (
            Is1x1(p_i, p_j, p_value) &&
            Is1x1(p_i + 1, p_j, p_value) &&
            Is1x1(p_i, p_j + 1, p_value) &&
            Is1x1(p_i + 1, p_j + 1, p_value)
        );
    }

    // GetCases.

    public List<Vector2Int> GetCases1x1(int p_value)
    {
        List<Vector2Int> cases = new List<Vector2Int>();

        for (int j = 0; j < m_h; j++)
        {
            for (int i = 0; i < m_w; i++)
            {
                if (Is1x1(i, j, p_value))
                {
                    Vector2Int positionIJ = new Vector2Int(i, j);

                    cases.Add(positionIJ);
                }
            }
        }

        return cases;
    }

    public List<Vector2Int> GetCases2x2(int p_value)
    {
        List<Vector2Int> cases = new List<Vector2Int>();

        for (int j = 0; j < m_h; j += 2)
        {
            for (int i = 0; i < m_w; i += 2)
            {
                if (Is2x2(i, j, p_value))
                {
                    Vector2Int positionIJ = new Vector2Int(i, j);

                    cases.Add(positionIJ);
                }
            }
        }

        return cases;
    }

    // Fill.

    public void Fill(int p_value)
    {
        Array.Fill(m_matrix, p_value);
    }

    // Sum.

    public int Sum()
    {
        return m_matrix.Sum();
    }

    // NextTo.

    public bool NextToOutOfDimension(int p_i, int p_j)
    {
        Debug.Assert(!OutOfDimension(p_i, p_j));

        return (
            (p_i == 0) ||
            (p_j == 0) ||
            (p_i == m_w - 1) ||
            (p_j == m_h - 1)
        );
    }

    public bool NextToCross(int p_i, int p_j, int p_value)
    {
        Debug.Assert(!NextToOutOfDimension(p_i, p_j));

        return (
            Is1x1(p_i + 1, p_j, p_value) ||
            Is1x1(p_i - 1, p_j, p_value) ||
            Is1x1(p_i, p_j + 1, p_value) ||
            Is1x1(p_i, p_j - 1, p_value)
        );
    }
}
