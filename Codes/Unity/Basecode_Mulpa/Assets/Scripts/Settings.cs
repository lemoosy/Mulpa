using System;

namespace _Settings
{
    public class Settings
    {
        // Enumération des différents modes de jeu.
        public enum ModeID
        {
            MODE_SOLO,
            MODE_TRAINING_EASY,
            MODE_TRAINING_MEDIUM,
            MODE_TRAINING_HARD,
            MODE_GENERATOR_EASY,
            MODE_GENERATOR_MEDIUM,
            MODE_GENERATOR_HARD
        }

        // Enumération des différentes cases d'une matrice dans un monde.
        public enum CaseID
        {
            CASE_VOID,
            CASE_WALL,
            CASE_ATTACK,
            CASE_COIN,
            CASE_SPAWN,
            CASE_EXIT,
            CASE_COUNT
        }

        // Enumération des différentes cases d'une matrice dans un monde (char).
        public enum CaseCharID
        {
            CASE_VOID      =   ' ',
            CASE_WALL      =   'O',
            CASE_ATTACK    =   '!',
            CASE_COIN      =   '.',
            CASE_PLAYER    =   'A',
            CASE_EXIT      =   'B',
            CASE_COUNT     =   5
        }

        // Variable pour générer des nombres aléatoires.
        public static Random m_randomGenerator = new Random();

        // Retourne un entier aléatoire entre p_a et p_b.
        public static int IntRandom(int p_a, int p_b)
        {
            return m_randomGenerator.Next(p_a, p_b + 1);
        }
    }
}
