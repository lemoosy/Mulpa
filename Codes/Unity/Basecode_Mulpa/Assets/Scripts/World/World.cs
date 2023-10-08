using UnityEngine;

using _Matrix;

namespace _World
{
    public class WorldInfo
    {
        // Largeur d'une tuile.
        public const int m_tileW = 16;
    
        // Hauteur d'une tuile.
        public const int m_tileH = 16;
    
        // Largeur de la matrice.
        public const int m_matrixW = 24;
    
        // Hauteur de la matrice.
        public const int m_matrixH = 14;

        // Taille du monde.
        public Vector2 m_size = new Vector2(m_matrixW * m_tileW, m_matrixH * m_tileH);

        // Nombre de mondes.
        public const int m_count = 15;
    }

    public class World
    {

    }
}
