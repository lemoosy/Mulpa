using System;
using System.Diagnostics;
using System.IO;

namespace _Matrix
{
    // Classe représentant une matrice 2D.
    public class Matrix
    {
        // Variable pour générer des nombres aléatoires.
        private Random rnd = new Random();

        // Matrice.
        private int[] m_matrix;
    
        // Largeur de la matrice.
        public int m_w;
    
        // Hauteur de la matrice.
        public int m_h;
    
        // Crée une matrice remplie de 0 de taille (p_w, p_h).
        public Matrix(int p_w, int p_h)
        {
            Debug.Assert((p_w > 0) && (p_h > 0), "ERROR - Matrix::Matrix()");

            m_w = p_w;
            m_h = p_h;

            m_matrix = new int[p_w * p_h];
        }

        // Crée et importe une matrice depuis un fichier TXT.
        public Matrix(string p_path)
        {
            string line = File.ReadAllText(p_path);

            string[] values = line.Split(',');

            m_w = int.Parse(values[0]);
            m_h = int.Parse(values[1]);

            for (int i = 0; i < m_w * m_h; i++)
            {
                m_matrix[i] = int.Parse(values[i + 2]);
            }
        }

        // Crée une matrice remplie de valeurs aléatoires entre p_a et p_b de taille (p_w, p_h).
        public Matrix(int p_w, int p_h, int p_a, int p_b)
        {
            m_w = p_w;
            m_h = p_h;

            m_matrix = new int[p_w * p_h];

            for (int k = 0; k < p_w * p_h; k++)
            {
                m_matrix[k] = rnd.Next(p_a, p_b + 1);
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

        // Exporte une matrice dans un fichier TXT (exemple: "m_w, m_h, 0, 0, 1, 0, 1, ...").
        public void Export(string p_path)
        {
            string w = m_w.ToString() + ", ";

            string h = m_h.ToString() + ", ";

            string matrix = string.Join(",", m_matrix);

            File.WriteAllText(p_path, w + h + matrix);
        }

        // Mélange les valeurs de la matrice this et p_mSource (p_mSource n'est pas modifié).
        public void Crossover(Matrix p_mSource)
        {
            for (int j = 0; j < m_h; j++)
            {
                for (int i = 0; i < m_w; i++)
                {
                    if (rnd.Next(2) == 0)
                    {
                        Set(i, j, p_mSource.Get(i, j));
                    }
                }
            }
        }

        // Modifie 'rate' fois une valeur aléatoire de la matrice par une valeur aléatoire entre a et b.
        public void Mutation(int p_rate, int p_a, int p_b)
        {
            for (int k = 0; k < p_rate; k++)
            {
                int i = rnd.Next(m_w);
                int j = rnd.Next(m_h);

                Set(i, j, rnd.Next(p_a, p_b + 1));
            }
        }
    }
}
