using System;
using System.Diagnostics;
using System.IO;

using _Settings;

namespace _Matrix
{
    // Classe représentant une matrice 2D.
    public class Matrix
    {
        // Largeur de la matrice.
        public int m_w;
    
        // Hauteur de la matrice.
        public int m_h;
        
        // Matrice.
        private int[] m_matrix;
    
        // Crée une matrice p_w x p_h (toutes les valeurs sont initialisées à 0).
        public Matrix(int p_w, int p_h)
        {
            Debug.Assert((p_w > 0) && (p_h > 0), "ERROR - Matrix::Matrix()");

            m_w = p_w;
            m_h = p_h;

            m_matrix = new int[p_w * p_h];
        }
        
        // Crée une matrice p_w x p_h (toutes les valeurs sont initialisées entre p_a et p_b).
        public Matrix(int p_w, int p_h, int p_a, int p_b)
        {
            m_w = p_w;
            m_h = p_h;

            m_matrix = new int[p_w * p_h];

            for (int k = 0; k < p_w * p_h; k++)
            {
                m_matrix[k] = Settings.IntRandom(p_a, p_b);
            }
        }

        // Vérifie si les coordonnées sont en dehors de la matrice.
        public bool OutOfDimension(int p_i, int p_j)
        {
            return ((p_i < 0) || (p_i >= m_w) || (p_j < 0) || (p_j >= m_h));
        }
    
        // Retourne la valeur à la position (p_i, p_j).
        public int Get(int p_i, int p_j)
        {
            Debug.Assert(!OutOfDimension(p_i, p_j), "ERROR - Matrix::Get()");
    
            return m_matrix[(p_j * m_w) + p_i];
        }
    
        // Modifie la valeur à la position (p_i, p_j) par p_value.
        public void Set(int p_i, int p_j, int p_value)
        {
            Debug.Assert(!OutOfDimension(p_i, p_j), "ERROR - Matrix::Set()");
            
            m_matrix[(p_j * m_w) + p_i] = p_value;
        }

        // Mélange les valeurs de la matrice this et p_mSource (p_mSource n'est pas modifiée).
        public void Crossover(Matrix p_mSource)
        {
            for (int j = 0; j < m_h; j++)
            {
                for (int i = 0; i < m_w; i++)
                {
                    if (Settings.IntRandom(0, 1) == 0)
                    {
                        Set(i, j, p_mSource.Get(i, j));
                    }
                }
            }
        }

        // Modifie une case aléatoire de la matrice par une valeur entre p_a et p_b.
        public void Mutation(int p_a, int p_b)
        {
            int i = Settings.IntRandom(0, m_w - 1);
            int j = Settings.IntRandom(0, m_h - 1);

            Set(i, j, Settings.IntRandom(p_a, p_b));
        }

        // Importe une matrice depuis un fichier TXT.
        public void Import(string p_path)
        {
            string file = File.ReadAllText(p_path);

            string[] lines = file.Split('\n');

            m_w = int.Parse(lines[0]);
            m_h = int.Parse(lines[1]);

            for (int j = 0; j < m_h; j++)
            {
                for (int i = 0; i < m_w; i++)
                {
                    int index = (m_h - j - 1) * m_w + i;

                    m_matrix[index] = (int)lines[j + 2][i];
                }
            }
        }

        // Exporte une matrice dans un fichier TXT.
        public void Export(string p_path)
        {
            string file = "";

            file += m_w.ToString() + "\n";
            file += m_h.ToString() + "\n";

            for (int j = 0; j < m_h; j++)
            {
                for (int i = 0; i < m_w; i++)
                {
                    int index = j * m_w + i;

                    file += m_matrix[index].ToString();
                }

                file += '\n';
            }

            File.WriteAllText(p_path, file);
        }
    }
}
