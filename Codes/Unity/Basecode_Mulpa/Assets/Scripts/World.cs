using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using _Matrix;
using System.Runtime.InteropServices;

public class World : MonoBehaviour
{



    #region DLL



    [DllImport("Basecode_DLL.dll")]
    private static extern float DLL_World_GetShortestPath(int[] p_world, int p_w, int p_h, int p_i1, int p_j1, int p_i2, int p_j2);

    [DllImport("Basecode_DLL.dll")]
    private static extern float DLL_World_GetShortestPathHard(int[] p_world, int p_w, int p_h, int p_i1, int p_j1, int p_i2, int p_j2);



    #endregion



    #region Variables



    // Enum�ration des diff�rents types de cases dans un monde.
    public enum CaseID
    {
        CASE_VOID,
        CASE_WALL,
        CASE_ATTACK,
        CASE_COIN,
        CASE_PLAYER,
        CASE_EXIT,
        CASE_COUNT = 5
    }



    // Largeur d'une tuile.
    public int m_tileWidth = 16;

    // Hauteur d'une tuile.
    public int m_tileHeight = 16;
    


    // Largeur de la matrice.
    public int m_matrixWidth = 24;

    // Hauteur de la matrice.
    public int m_matrixHeight = 14;



    // Taille du monde.
    public Vector2 m_size;



    // Nombre de mod�les par difficult�.
    public int m_modelsCount = 4;



    // Chemin vers les mondes faciles (fichiers TXT).
    private string m_modelsPathEasy = "Models/Easy/"; 

    // Mondes faciles.
    public Matrix[] m_modelsEasy;



    // Chemin vers les mondes normaux (fichiers TXT).
    private string m_modelsPathNormal = "Models/Normal/";

    // Mondes normaux.
    public Matrix[] m_modelsNormal;



    // Chemin vers les mod�les difficiles (fichiers TXT).
    private string m_modelsPathHard = "Models/Hard/";

    // Mondes difficiles.
    public Matrix[] m_modelsHard;



    // Monde actuel.
    public Matrix m_current;



    #endregion



    #region Functions



    void Start()
    {


        m_size = new Vector2(m_matrixWidth * m_tileWidth, m_matrixHeight * m_tileHeight);

        Debug.Assert(m_modelsCount < 10, "ERROR - m_modelsCount >= 10");

        m_modelsEasy = new Matrix[m_modelsCount];

        for (int i = 0; i < m_modelsCount; i++)
        {
            m_modelsEasy[i] = new Matrix(m_modelsPathEasy + i.ToString() + ".txt");
        }

        m_modelsNormal = new Matrix[m_modelsCount];

        for (int i = 0; i < m_modelsCount; i++)
        {
            m_modelsNormal[i] = new Matrix(m_modelsPathNormal + i.ToString() + ".txt");
        }

        m_modelsHard = new Matrix[m_modelsCount];

        for (int i = 0; i < m_modelsCount; i++)
        {
            m_modelsEasy[i] = new Matrix(m_modelsPathEasy + i.ToString() + ".txt");
        }


    }

    void Update()
    {

    }

    public Matrix SceneToMatrix()
    {
        Matrix res = new Matrix(m_matrixWidth, m_matrixHeight);



        return res;
    }

    public void Load(Matrix p_world)
    {

    }

    // Calcule le PCC entre p_start et p_end.
    public float ShortestPath(Vector2 p_start, Vector2 p_end)
    {

        return 0.0f;
    }

    // Calcule le PCC entre p_start et p_end en prennant en compte les murs.
    public float ShortestPathHard(Vector2 p_start, Vector2 p_end)
    {
        return 0.0f;
    }

    // G�n�re un monde al�atoire.
    public Matrix GenWorld()
    {
        Matrix res = new Matrix(m_matrixWidth, m_matrixHeight);

        // ...

        return res;
    }



    #endregion



}
