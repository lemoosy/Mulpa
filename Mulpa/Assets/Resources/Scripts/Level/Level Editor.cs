using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelEditor : MonoBehaviour
{
    public Tilemap m_decorations = null;
    public Tilemap m_blocks = null;
    public GameObject m_doors = null;
    public GameObject m_dangers = null;
    public GameObject m_coins = null;
    public GameObject m_lever = null;
    public GameObject m_exit = null;
    public GameObject m_spawn = null;

    public enum CaseID
    {
        CASE_VOID       =       Graph.CaseID.CASE_VOID,
        CASE_BLOCK      =       Graph.CaseID.CASE_BLOCK,    // blocks + doors
        CASE_DANGER     =       Graph.CaseID.CASE_DANGER,   // spades + lava + monsters
        CASE_COIN       =       3,
        CASE_EXIT       =       4,                          // lever + exit
        CASE_PLAYER     =       5
    }

    public void Start()
    {
        Debug.Assert(m_decorations);
        Debug.Assert(m_blocks);
        //Debug.Assert(!(m_doors ^ m_lever));
        Debug.Assert(m_dangers);
        Debug.Assert(m_coins);
        Debug.Assert(m_exit);
        Debug.Assert(m_spawn);
    }

    public Matrix GetMatrix()
    {
        int w = LevelInformation.matrixSize.x;
        int h = LevelInformation.matrixSize.y;

        Matrix matrix = new Matrix(w, h);

        // Blocks.

        for (int j = 0; j < h; j++)
        {
            for (int i = 0; i < w; i++)
            {
                Vector3Int positionIJK = new Vector3Int(i, j, 0);

                if (m_blocks.GetTile(positionIJK))
                {
                    int value = (int)CaseID.CASE_BLOCK;

                    matrix.Set1x1(i, j, value);
                }
            }
        }

        // Doors.

        if (m_doors)
        {
            foreach (Transform _ in m_doors.transform)
            {
                Vector2 doorPosition = (Vector2)_.localPosition;

                int i = (int)doorPosition.x;
                int j = (int)doorPosition.y;

                if (matrix.OutOfDimension(i, j))
                {
                    continue;
                }

                int value = (int)CaseID.CASE_BLOCK;

                matrix.Set1x1(i, j, value);
            }
        }

        // Dangers.

        foreach (Transform _ in m_dangers.transform)
        {
            Vector2 spadePosition = (Vector2)_.localPosition;

            int i = (int)spadePosition.x;
            int j = (int)spadePosition.y;

            if (matrix.OutOfDimension(i, j))
            {
                continue;
            }

            int value = (int)CaseID.CASE_DANGER;

            matrix.Set1x1(i, j, value);
        }

        // Coins.

        foreach (Transform _ in m_coins.transform)
        {
            Vector2 coinPosition = (Vector2)_.localPosition;

            int i = (int)coinPosition.x;
            int j = (int)coinPosition.y;

            if (matrix.OutOfDimension(i, j))
            {
                continue;
            }

            int value = (int)CaseID.CASE_COIN;

            matrix.Set1x1(i, j, value);
        }

        // Lever & Exit.

        {
            GameObject leverOrExit = null;
            
            if (m_lever)
            {
                leverOrExit = m_lever;
            }
            else
            {
                leverOrExit = m_exit;
            }
            
            Vector2 leverOrExitPosition = (Vector2)leverOrExit.transform.localPosition;
            
            int i = (int)leverOrExitPosition.x;
            int j = (int)leverOrExitPosition.y;
            
            if (!matrix.OutOfDimension(i, j))
            {
                int value = (int)CaseID.CASE_EXIT;
            
                matrix.Set1x1(i, j, value);
            }
        }

        return matrix;
    }
}
