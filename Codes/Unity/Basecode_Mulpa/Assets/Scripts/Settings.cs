using System;
using System.Diagnostics;

namespace _Settings
{
    public class Settings
    {
        // Énumération des différents modes de jeu.
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

        // Énumération des différentes cases dans la matrice m_matrixChar.
        public enum CaseIDChar
        {
            CASE_VOID       =   ' ',
            CASE_LIGHT      =   '¤',
            CASE_WALL       =   'O',
            CASE_DOOR       =   '|',
            CASE_MONSTER    =   'M',
            CASE_SPADE      =   '!',
            CASE_LAVA       =   'L',
            CASE_COIN       =   '.',
            CASE_SPAWN      =   'A',
            CASE_LEVER      =   '/',
            CASE_EXIT       =   'B'
        }

        // Énumération des différentes cases dans la matrice m_matrixBin.
        public enum CaseIDBin
        {
            CASE_VOID       =   0b000,
            CASE_LIGHT      =   0b000,
            CASE_WALL       =   0b001,
            CASE_DOOR       =   0b001,
            CASE_MONSTER    =   0b010,
            CASE_SPADE      =   0b010,
            CASE_LAVA       =   0b010,
            CASE_COIN       =   0b011,
            CASE_SPAWN      =   0b000,
            CASE_LEVER      =   0b101,
            CASE_EXIT       =   0b101,
            CASE_PLAYER     =   0b100
        }

        // Énumération des différentes difficultés.
        public enum Difficulty
        {
            EASY,
            MEDIUM,
            HARD,
            ALL
        }

        // Variable pour générer des nombres aléatoires.
        public static Random m_randomGenerator = new Random();

        // Retourne un entier aléatoire entre p_a et p_b.
        public static int IntRandom(int p_a, int p_b)
        {
            Debug.Assert(p_a < p_b);

            return m_randomGenerator.Next(p_a, p_b + 1);
        }
    }
}
