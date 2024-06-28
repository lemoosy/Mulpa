using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LevelMaker
{
    //public bool m_hasLever;
    
    //public Vector2Int m_positionSpawn;
    //public Vector2Int m_positionLever;
    //public Vector2Int m_positionExit;
    
    //static public int s_block2x2Count   =   30;
    //static public int s_block1x1Count   =   30;
    //static public int s_spadesCount     =   20;
    //static public int s_monstersCount   =   5;

    //public enum CaseID
    //{
    //    CASE_VOID,
    //    CASE_VOID_FIX,
    //    CASE_SPAWN,
    //    CASE_LEVER,
    //    CASE_EXIT,
    //    CASE_BLOCK_2x2,
    //    CASE_BLOCK_1x1,
    //    CASE_SPADE,
    //    CASE_MONSTER
    //}

    //public Matrix m_matrix = new Matrix(
    //    LevelInformation.matrixSize.x,
    //    LevelInformation.matrixSize.y
    //);

    //public LevelMaker(bool p_hasLever, Vector2Int p_positionSpawn, Vector2Int p_positionLever, Vector2Int p_positionExit)
    //{
    //    m_hasLever = p_hasLever;

    //    m_positionSpawn = p_positionSpawn;
    //    m_positionLever = p_positionLever;
    //    m_positionExit  = p_positionExit;
    //}

    //public void Matrix_Init()
    //{
    //    m_matrix.Fill(0);

    //    Matrix_CreateSpawn();
    //    Matrix_CreateLever();
    //    Matrix_CreateExit();
    //    Matrix_CreateBlocks2x2();
    //    Matrix_CreateBlocks1x1();
    //    Matrix_CreateSpades();
    //    Matrix_CreateMonsters();
    //}

    //public GameObject Matrix_ToUnity(Transform p_parent)
    //{
    //    GameObject res = GameObject.Instantiate(Resource.s_prefabLevelEmpty, p_parent);
        
    //    LevelEditor levelEditor = res.GetComponent<LevelEditor>();

    //    int w = m_matrix.m_w;
    //    int h = m_matrix.m_h;

    //    for (int j = 0; j < h; j++)
    //    {
    //        for (int i = 0; i < w; i++)
    //        {
    //            int value = m_matrix.Get1x1(i, j);

    //            Vector2Int positionIJ = new Vector2Int(i, j);

    //            Vector2 position = (Vector2)positionIJ;
    //            position.x += 0.5f;
    //            position.y += 0.5f;

    //            switch (value)
    //            {
    //                case (int)CaseID.CASE_VOID:
    //                case (int)CaseID.CASE_VOID_FIX:

    //                    break;

    //                case (int)CaseID.CASE_SPAWN:

    //                    Debug.Assert(!levelEditor.m_spawn);

    //                    levelEditor.m_spawn = GameObject.Instantiate(
    //                        Resource.s_prefabSpawn,
    //                        res.transform
    //                    );

    //                    levelEditor.m_spawn.transform.localPosition = position;
                        
    //                    break;
                    
    //                case (int)CaseID.CASE_LEVER:

    //                    Debug.Assert(!levelEditor.m_lever);

    //                    levelEditor.m_lever = GameObject.Instantiate(
    //                        Resource.s_prefabLever,
    //                        res.transform
    //                    );

    //                    levelEditor.m_lever.transform.localPosition = position;

    //                    break;

    //                case (int)CaseID.CASE_EXIT:

    //                    Debug.Assert(!levelEditor.m_exit);

    //                    levelEditor.m_exit = GameObject.Instantiate(
    //                        Resource.s_prefabExit,
    //                        res.transform
    //                    );

    //                    levelEditor.m_exit.transform.localPosition = position;

    //                    break;

    //                case (int)CaseID.CASE_BLOCK_2x2:
    //                case (int)CaseID.CASE_BLOCK_1x1:
                        
    //                    if (!Matrix_IsOutOrBlock(i - 1, j) &&
    //                        Matrix_IsOutOrBlock(i + 1, j) &&
    //                        !Matrix_IsOutOrBlock(i, j + 1) &&
    //                        Matrix_IsOutOrBlock(i, j - 1)
    //                    )
    //                    {
    //                        if (Utils.Int_Random(1, 5) == 1)
    //                        {
    //                            levelEditor.m_blocks.SetTile((Vector3Int)positionIJ, Resource.s_tilesBlock[6]);
    //                        }
    //                        else
    //                        {
    //                            levelEditor.m_blocks.SetTile((Vector3Int)positionIJ, Resource.s_tilesBlock[24]);
    //                        }
    //                    }

    //                    else if (Matrix_IsOutOrBlock(i - 1, j) &&
    //                             Matrix_IsOutOrBlock(i + 1, j) &&
    //                             !Matrix_IsOutOrBlock(i, j + 1) &&
    //                             Matrix_IsOutOrBlock(i, j - 1)
    //                    )
    //                    {
    //                        if (Utils.Int_Random(1, 5) == 1)
    //                        {
    //                            levelEditor.m_blocks.SetTile((Vector3Int)positionIJ, Resource.s_tilesBlock[7]);
    //                        }
    //                        else
    //                        {
    //                            levelEditor.m_blocks.SetTile((Vector3Int)positionIJ, Resource.s_tilesBlock[25]);
    //                        }
    //                    }

    //                    else if (Matrix_IsOutOrBlock(i - 1, j) &&
    //                             !Matrix_IsOutOrBlock(i + 1, j) &&
    //                             !Matrix_IsOutOrBlock(i, j + 1) &&
    //                             Matrix_IsOutOrBlock(i, j - 1)
    //                    )
    //                    {
    //                        if (Utils.Int_Random(1, 5) == 1)
    //                        {
    //                            levelEditor.m_blocks.SetTile((Vector3Int)positionIJ, Resource.s_tilesBlock[8]);
    //                        }
    //                        else
    //                        {
    //                            levelEditor.m_blocks.SetTile((Vector3Int)positionIJ, Resource.s_tilesBlock[26]);
    //                        }
    //                    }

    //                    else if (!Matrix_IsOutOrBlock(i - 1, j) &&
    //                             Matrix_IsOutOrBlock(i + 1, j) &&
    //                             Matrix_IsOutOrBlock(i, j + 1) &&
    //                             Matrix_IsOutOrBlock(i, j - 1)
    //                    )
    //                    {
    //                        levelEditor.m_blocks.SetTile((Vector3Int)positionIJ, Resource.s_tilesBlock[12]);
    //                    }

    //                    else if (Matrix_IsOutOrBlock(i - 1, j) &&
    //                             Matrix_IsOutOrBlock(i + 1, j) &&
    //                             Matrix_IsOutOrBlock(i, j + 1) &&
    //                             Matrix_IsOutOrBlock(i, j - 1)
    //                    )
    //                    {
    //                        if (!Matrix_IsOutOrBlock(i - 1, j + 1))
    //                        {
    //                            levelEditor.m_blocks.SetTile((Vector3Int)positionIJ, Resource.s_tilesBlock[16]);

    //                        }
    //                        else if (!Matrix_IsOutOrBlock(i + 1, j + 1))
    //                        {
    //                            levelEditor.m_blocks.SetTile((Vector3Int)positionIJ, Resource.s_tilesBlock[15]);

    //                        }
    //                        else if (!Matrix_IsOutOrBlock(i - 1, j - 1))
    //                        {
    //                            levelEditor.m_blocks.SetTile((Vector3Int)positionIJ, Resource.s_tilesBlock[10]);
    //                        }
    //                        else if (!Matrix_IsOutOrBlock(i + 1, j - 1))
    //                        {
    //                            levelEditor.m_blocks.SetTile((Vector3Int)positionIJ, Resource.s_tilesBlock[9]);
    //                        }
    //                        else
    //                        {
    //                            if (Utils.Int_Random(1, 20) == 1)
    //                            {
    //                                levelEditor.m_blocks.SetTile((Vector3Int)positionIJ, Resource.s_tilesBlock[28]);
    //                            }
    //                            else
    //                            {
    //                                levelEditor.m_blocks.SetTile((Vector3Int)positionIJ, Resource.s_tilesBlock[13]);
    //                            }
    //                        }
    //                    }

    //                    else if (Matrix_IsOutOrBlock(i - 1, j) &&
    //                            !Matrix_IsOutOrBlock(i + 1, j) &&
    //                            Matrix_IsOutOrBlock(i, j + 1) &&
    //                            Matrix_IsOutOrBlock(i, j - 1)
    //                    )
    //                    {
    //                        levelEditor.m_blocks.SetTile((Vector3Int)positionIJ, Resource.s_tilesBlock[14]);
    //                    }

    //                    else if (!Matrix_IsOutOrBlock(i - 1, j) &&
    //                             Matrix_IsOutOrBlock(i + 1, j) &&
    //                             Matrix_IsOutOrBlock(i, j + 1) &&
    //                             !Matrix_IsOutOrBlock(i, j - 1)
    //                    )
    //                    {
    //                        levelEditor.m_blocks.SetTile((Vector3Int)positionIJ, Resource.s_tilesBlock[18]);
    //                    }

    //                    else if (Matrix_IsOutOrBlock(i - 1, j) &&
    //                             Matrix_IsOutOrBlock(i + 1, j) &&
    //                             Matrix_IsOutOrBlock(i, j + 1) &&
    //                             !Matrix_IsOutOrBlock(i, j - 1)
    //                    )
    //                    {
    //                        levelEditor.m_blocks.SetTile((Vector3Int)positionIJ, Resource.s_tilesBlock[19]);
    //                    }

    //                    else if (Matrix_IsOutOrBlock(i - 1, j) &&
    //                             !Matrix_IsOutOrBlock(i + 1, j) &&
    //                             Matrix_IsOutOrBlock(i, j + 1) &&
    //                             !Matrix_IsOutOrBlock(i, j - 1)
    //                    )
    //                    {
    //                        levelEditor.m_blocks.SetTile((Vector3Int)positionIJ, Resource.s_tilesBlock[20]);
    //                    }

    //                    else
    //                    {
    //                        levelEditor.m_blocks.SetTile((Vector3Int)positionIJ, Resource.s_tilesBlock[27]);
    //                    }

    //                    break;

    //                case (int)CaseID.CASE_SPADE:

    //                    GameObject spade = null;

    //                    if (Matrix_IsOutOrBlock(i - 1, j))
    //                    {
    //                        spade = GameObject.Instantiate(
    //                            Resource.s_prefabSpade,
    //                            levelEditor.m_dangers.transform
    //                        );

    //                        spade.transform.Rotate(0.0f, 0.0f, -90.0f);
    //                    }
    //                    else if (Matrix_IsOutOrBlock(i + 1, j))
    //                    {
    //                        spade = GameObject.Instantiate(
    //                            Resource.s_prefabSpade,
    //                            levelEditor.m_dangers.transform
    //                        );

    //                        spade.transform.Rotate(0.0f, 0.0f, +90.0f);
    //                    }
    //                    else if (Matrix_IsOutOrBlock(i, j + 1))
    //                    {
    //                        spade = GameObject.Instantiate(
    //                            Resource.s_prefabSpade,
    //                            levelEditor.m_dangers.transform
    //                        );

    //                        spade.transform.Rotate(0.0f, 0.0f, -180.0f);
    //                    }
    //                    else if (Matrix_IsOutOrBlock(i, j - 1))
    //                    {
    //                        spade = GameObject.Instantiate(
    //                            Resource.s_prefabSpade,
    //                            levelEditor.m_dangers.transform
    //                        );
    //                    }
    //                    else
    //                    {
    //                        Debug.Assert(false);
    //                    }

    //                    spade.transform.localPosition = position;

    //                    break;
                    
    //                case (int)CaseID.CASE_MONSTER:

    //                    GameObject monster = GameObject.Instantiate(
    //                        Resource.s_prefabDanger,
    //                        levelEditor.m_dangers.transform
    //                    );

    //                    monster.transform.localPosition = position;

    //                    break;

    //                default:

    //                    Debug.Assert(false);
                        
    //                    break;
    //            }
    //        }
    //    }

    //    return res;
    //}

    //public void Matrix_Completion()
    //{
    //    int w = m_matrix.m_w;
    //    int h = m_matrix.m_h;

    //    Matrix matrix = new Matrix(w, h);
        
    //    // CaseID -> Graph.CaseID

    //    for (int j = 0; j < h; j++)
    //    {
    //        for (int i = 0; i < w; i++)
    //        {
    //            int value = m_matrix.Get1x1(i, j);

    //            switch (value)
    //            {
    //                case (int)CaseID.CASE_VOID:
    //                    break;

    //                case (int)CaseID.CASE_VOID_FIX:
    //                    break;

    //                case (int)CaseID.CASE_SPAWN:
    //                    break;

    //                case (int)CaseID.CASE_LEVER:
    //                    break;

    //                case (int)CaseID.CASE_EXIT:
    //                    break;

    //                case (int)CaseID.CASE_BLOCK_2x2:
    //                case (int)CaseID.CASE_BLOCK_1x1:
    //                    matrix.Set1x1(i, j, (int)Graph.CaseID.CASE_BLOCK);
    //                    break;

    //                case (int)CaseID.CASE_SPADE:
    //                case (int)CaseID.CASE_MONSTER:
    //                    matrix.Set1x1(i, j, (int)Graph.CaseID.CASE_DANGER);
    //                    break;

    //                default:
    //                    Debug.Assert(false);
    //                    break;
    //            }
    //        }
    //    }

    //    // Fill

    //    for (int j = 0; j < h; j++)
    //    {
    //        for (int i = 0; i < w; i++)
    //        {
    //            int value = m_matrix.Get1x1(i, j);

    //            switch (value)
    //            {
    //                case (int)CaseID.CASE_VOID:
    //                case (int)CaseID.CASE_SPADE:
    //                case (int)CaseID.CASE_MONSTER:

    //                    float SP = Graph.GetSP(
    //                        matrix.m_matrix,
    //                        w,
    //                        h,
    //                        i,
    //                        j,
    //                        m_positionSpawn.x,
    //                        m_positionSpawn.y,
    //                        false
    //                    );

    //                    if (SP == -1.0f)
    //                    {
    //                        m_matrix.Set1x1(i, j, (int)CaseID.CASE_BLOCK_1x1);
    //                    }

    //                    break;
    //            }
    //        }
    //    }
    //}

    //public bool Matrix_IsOutOrBlock(int p_i, int p_j)
    //{
    //    return (
    //        m_matrix.OutOfDimension(p_i, p_j) ||
    //        m_matrix.Is1x1(p_i, p_j, (int)CaseID.CASE_BLOCK_1x1) ||
    //        m_matrix.Is1x1(p_i, p_j, (int)CaseID.CASE_BLOCK_2x2)
    //    );
    //}

    //// Create.

    //public void Matrix_CreateSpawn()
    //{
    //    int i = m_positionSpawn.x;
    //    int j = m_positionSpawn.y;

    //    Debug.Assert(!m_matrix.OutOfDimension(i, j));

    //    m_matrix.Set1x1(i, j, (int)CaseID.CASE_SPAWN);
    //}

    //public void Matrix_CreateLever()
    //{
    //    if (!m_hasLever) return;

    //    int i = m_positionLever.x;
    //    int j = m_positionLever.y;
        
    //    Debug.Assert(!m_matrix.OutOfDimension(i, j));

    //    m_matrix.Set1x1(i, j, (int)CaseID.CASE_LEVER);
    //}

    //public void Matrix_CreateExit()
    //{
    //    int i = m_positionExit.x;
    //    int j = m_positionExit.y;

    //    Debug.Assert(!m_matrix.OutOfDimension(i, j));

    //    m_matrix.TrySet2x2(i, j, (int)CaseID.CASE_VOID_FIX);

    //    m_matrix.Set1x1(i, j, (int)CaseID.CASE_EXIT);
    //}

    //public void Matrix_CreateBlocks2x2()
    //{
    //    List<Vector2Int> voids2x2 = m_matrix.GetCases2x2((int)CaseID.CASE_VOID);

    //    Debug.Assert(s_block2x2Count <= voids2x2.Count);

    //    Utils.List_Randomize(voids2x2);

    //    for (int k = 0; k < s_block2x2Count; k++)
    //    {
    //        int i = voids2x2[0].x;
    //        int j = voids2x2[0].y;

    //        Debug.Assert(m_matrix.Is2x2(i, j, (int)CaseID.CASE_VOID));

    //        m_matrix.Set2x2(i, j, (int)CaseID.CASE_BLOCK_2x2);

    //        voids2x2.RemoveAt(0);
    //    }
    //}

    //public void Matrix_CreateBlocks1x1(int p_blocks1x1CountStart = 0)
    //{
    //    List<Vector2Int> voids = m_matrix.GetCases1x1((int)CaseID.CASE_VOID);

    //    Utils.List_Randomize(voids);

    //    int blocks1x1Count = p_blocks1x1CountStart;

    //    while (blocks1x1Count < s_block1x1Count)
    //    {
    //        Debug.Assert(voids.Count > 0);

    //        int i = voids[0].x;
    //        int j = voids[0].y;

    //        Debug.Assert(m_matrix.Is1x1(i, j, (int)CaseID.CASE_VOID));

    //        if (m_matrix.NextToOutOfDimension(i, j) ||
    //            m_matrix.NextToCross(i, j, (int)CaseID.CASE_BLOCK_2x2) ||
    //            m_matrix.NextToCross(i, j, (int)CaseID.CASE_BLOCK_1x1)
    //        )
    //        {
    //            m_matrix.Set1x1(i, j, (int)CaseID.CASE_BLOCK_1x1);
    //            blocks1x1Count++;
    //        }

    //        voids.RemoveAt(0);
    //    }
    //}

    //public void Matrix_CreateSpades(int p_spadesCountStart = 0)
    //{
    //    List<Vector2Int> voids = m_matrix.GetCases1x1((int)CaseID.CASE_VOID);

    //    Utils.List_Randomize(voids);

    //    int spadesCount = p_spadesCountStart;

    //    while (spadesCount < s_spadesCount)
    //    {
    //        Debug.Assert(voids.Count > 0);
            
    //        int i = voids[0].x;
    //        int j = voids[0].y;

    //        Debug.Assert(m_matrix.Is1x1(i, j, (int)CaseID.CASE_VOID));

    //        if (m_matrix.NextToOutOfDimension(i, j) ||
    //            m_matrix.NextToCross(i, j, (int)CaseID.CASE_BLOCK_2x2) ||
    //            m_matrix.NextToCross(i, j, (int)CaseID.CASE_BLOCK_1x1)
    //        )
    //        {
    //            m_matrix.Set1x1(i, j, (int)CaseID.CASE_SPADE);
    //            spadesCount++;
    //        }

    //        voids.RemoveAt(0);
    //    }
    //}

    //public void Matrix_CreateMonsters(int p_monstersCountStart = 0)
    //{
    //    List<Vector2Int> voids = m_matrix.GetCases1x1((int)CaseID.CASE_VOID);

    //    Utils.List_Randomize(voids);

    //    int monstersCount = p_monstersCountStart;

    //    while (monstersCount < s_monstersCount)
    //    {
    //        Debug.Assert(voids.Count > 0);

    //        int i = voids[0].x;
    //        int j = voids[0].y;

    //        Debug.Assert(m_matrix.Is1x1(i, j, (int)CaseID.CASE_VOID));

    //        if (!m_matrix.NextToOutOfDimension(i, j))
    //        {
    //            if (
    //                m_matrix.Is1x1(i, j - 1, (int)CaseID.CASE_BLOCK_2x2) ||
    //                m_matrix.Is1x1(i, j - 1, (int)CaseID.CASE_BLOCK_1x1)
    //            )
    //            {
    //                m_matrix.Set1x1(i, j, (int)CaseID.CASE_MONSTER);
    //                monstersCount++;
    //            }
    //        }

    //        voids.RemoveAt(0);
    //    }
    //}

    //// Crossover.

    //static public LevelMaker operator+(LevelMaker p_levelMaker1, LevelMaker p_levelMaker2)
    //{
    //    LevelMaker res = new LevelMaker(
    //        p_levelMaker1.m_hasLever,
    //        p_levelMaker1.m_positionSpawn,
    //        p_levelMaker1.m_positionLever,
    //        p_levelMaker1.m_positionExit
    //    );

    //    res.m_matrix.Fill(0);

    //    res.Matrix_CreateSpawn();
    //    res.Matrix_CreateLever();
    //    res.Matrix_CreateExit();



    //    // Blocs 2x2.

    //    List<Vector2Int> blocks2x2_1 = p_levelMaker1.m_matrix.GetCases2x2((int)CaseID.CASE_BLOCK_2x2);
    //    List<Vector2Int> blocks2x2_2 = p_levelMaker2.m_matrix.GetCases2x2((int)CaseID.CASE_BLOCK_2x2);

    //    Debug.Assert(blocks2x2_1.Count == blocks2x2_2.Count);
    //    Debug.Assert(blocks2x2_1.Count == s_block2x2Count);

    //    List<Vector2Int> blocks2x2 = new List<Vector2Int>(blocks2x2_1);
    //    blocks2x2.AddRange(blocks2x2_2);
    //    Utils.List_Randomize(blocks2x2);

    //    Debug.Assert(blocks2x2.Count == s_block2x2Count * 2);

    //    int blocks2x2Count = 0;

    //    while (blocks2x2Count < s_block2x2Count)
    //    {
    //        Debug.Assert(blocks2x2.Count > 0);

    //        int i = blocks2x2[0].x;
    //        int j = blocks2x2[0].y;

    //        if (res.m_matrix.Is2x2(i, j, (int)CaseID.CASE_VOID))
    //        {
    //            res.m_matrix.Set2x2(i, j, (int)CaseID.CASE_BLOCK_2x2);
    //            blocks2x2Count++;
    //        }

    //        blocks2x2.RemoveAt(0);
    //    }



    //    // Blocs 1x1.

    //    List<Vector2Int> blocks1x1_1 = p_levelMaker1.m_matrix.GetCases1x1((int)CaseID.CASE_BLOCK_1x1);
    //    List<Vector2Int> blocks1x1_2 = p_levelMaker2.m_matrix.GetCases1x1((int)CaseID.CASE_BLOCK_1x1);

    //    Debug.Assert(blocks1x1_1.Count == blocks1x1_2.Count);
    //    Debug.Assert(blocks1x1_1.Count == s_block1x1Count);

    //    List<Vector2Int> blocks1x1 = new List<Vector2Int>(blocks1x1_1);
    //    blocks1x1.AddRange(blocks1x1_2);
    //    Utils.List_Randomize(blocks1x1);

    //    Debug.Assert(blocks1x1.Count == s_block1x1Count * 2);

    //    int blocks1x1Count = 0;

    //    while (blocks1x1Count < s_block1x1Count)
    //    {
    //        if (blocks1x1.Count == 0)
    //        {
    //            res.Matrix_CreateBlocks1x1(blocks1x1Count);
    //            break;
    //        }

    //        int i = blocks1x1[0].x;
    //        int j = blocks1x1[0].y;

    //        if (res.m_matrix.Is1x1(i, j, (int)CaseID.CASE_VOID))
    //        {
    //            if (
    //                res.m_matrix.NextToOutOfDimension(i, j) ||
    //                res.m_matrix.NextToCross(i, j, (int)CaseID.CASE_BLOCK_2x2) ||
    //                res.m_matrix.NextToCross(i, j, (int)CaseID.CASE_BLOCK_1x1)
    //            )
    //            {
    //                res.m_matrix.Set1x1(i, j, (int)CaseID.CASE_BLOCK_1x1);
    //                blocks1x1Count++;
    //            }
    //        }

    //        blocks1x1.RemoveAt(0);
    //    }



    //    // Spades.

    //    List<Vector2Int> spades_1 = p_levelMaker1.m_matrix.GetCases1x1((int)CaseID.CASE_SPADE);
    //    List<Vector2Int> spades_2 = p_levelMaker2.m_matrix.GetCases1x1((int)CaseID.CASE_SPADE);

    //    Debug.Assert(spades_1.Count == spades_2.Count);
    //    Debug.Assert(spades_1.Count == s_spadesCount);

    //    List<Vector2Int> spades = new List<Vector2Int>(spades_1);
    //    spades.AddRange(spades_2);
    //    Utils.List_Randomize(spades);

    //    Debug.Assert(spades.Count == s_spadesCount * 2);

    //    int spadesCount = 0;

    //    while (spadesCount < s_spadesCount)
    //    {
    //        if (spades.Count == 0)
    //        {
    //            res.Matrix_CreateSpades(spadesCount);
    //            break;
    //        }

    //        int i = spades[0].x;
    //        int j = spades[0].y;

    //        if (res.m_matrix.Is1x1(i, j, (int)CaseID.CASE_VOID))
    //        {
    //            if (
    //                res.m_matrix.NextToOutOfDimension(i, j) ||
    //                res.m_matrix.NextToCross(i, j, (int)CaseID.CASE_BLOCK_2x2) ||
    //                res.m_matrix.NextToCross(i, j, (int)CaseID.CASE_BLOCK_1x1)
    //            )
    //            {
    //                res.m_matrix.Set1x1(i, j, (int)CaseID.CASE_SPADE);
    //                spadesCount++;
    //            }
    //        }

    //        spades.RemoveAt(0);
    //    }



    //    // Monsters.

    //    List<Vector2Int> monsters_1 = p_levelMaker1.m_matrix.GetCases1x1((int)CaseID.CASE_MONSTER);
    //    List<Vector2Int> monsters_2 = p_levelMaker2.m_matrix.GetCases1x1((int)CaseID.CASE_MONSTER);

    //    Debug.Assert(monsters_1.Count == monsters_2.Count);
    //    Debug.Assert(monsters_1.Count == s_monstersCount);

    //    List<Vector2Int> monsters = new List<Vector2Int>(monsters_1);
    //    monsters.AddRange(monsters_2);
    //    Utils.List_Randomize(monsters);

    //    Debug.Assert(monsters.Count == s_monstersCount * 2);

    //    int monstersCount = 0;

    //    while (monstersCount < s_monstersCount)
    //    {
    //        if (monsters.Count == 0)
    //        {
    //            res.Matrix_CreateMonsters(monstersCount);
    //            break;
    //        }

    //        int i = monsters[0].x;
    //        int j = monsters[0].y;

    //        if (
    //            res.m_matrix.Is1x1(i, j, (int)CaseID.CASE_VOID) &&
    //            !res.m_matrix.NextToOutOfDimension(i, j)
    //        )
    //        {
    //            if (
    //                res.m_matrix.Is1x1(i, j - 1, (int)CaseID.CASE_BLOCK_2x2) ||
    //                res.m_matrix.Is1x1(i, j - 1, (int)CaseID.CASE_BLOCK_1x1)
    //            )
    //            {
    //                res.m_matrix.Set1x1(i, j, (int)CaseID.CASE_MONSTER);
    //                monstersCount++;
    //            }
    //        }

    //        monsters.RemoveAt(0);
    //    }



    //    return res;
    //}
}
