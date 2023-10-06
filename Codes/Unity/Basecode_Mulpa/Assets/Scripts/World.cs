using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using _Matrix;



// Enumération des différents types de cases dans un monde.
enum CaseID
{
    CASE_VOID,
    CASE_WALL,
    CASE_ATTACK,
    CASE_COIN,
    CASE_PLAYER,
    CASE_EXIT,
    CASE_COUNT = 5
}



public class World : MonoBehaviour
{



    #region Variables



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



    // Nombre de modèles par difficulté.
    public int m_modelsCount = 4;



    // Chemin vers les mondes faciles (fichiers TXT).
    private string m_modelsPathEasy = "Models/Easy/"; 

    // Mondes faciles.
    public Matrix[] m_modelsEasy;



    // Chemin vers les mondes normaux (fichiers TXT).
    private string m_modelsPathNormal = "Models/Normal/";

    // Mondes normaux.
    public Matrix[] m_modelsNormal;



    // Chemin vers les modèles difficiles (fichiers TXT).
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

    // Génère un monde aléatoire.
    public Matrix GenWorld()
    {
        Matrix res = new Matrix(m_matrixWidth, m_matrixHeight);



        return res;
    }



    #endregion



}
