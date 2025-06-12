using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex05_OriCohen_207008590_AlonZylberberg_315853739
{
    internal class GameLogic
    {
        internal enum eColor
        {
            A,
            B,
            C,
            D,
            E,
            F,
            G,
            H
        }

        internal enum eFeedback
        {
            None,                       // No match for this position
            CorrectColorWrongPosition,  // Colour exists elsewhere in the secret
            CorrectPosition             // Exact match: colour & position
        }

        private int k_SequenceLengthMax = 4;

        private static readonly Random sr_Random = new Random();

        private readonly eColor[] r_Secret;
        private bool m_HasWon;

        public GameLogic()
        {
            r_Secret = generateSecret();
            m_HasWon = false;
        }

        int SequenceLength
        {
            get { return k_SequenceLengthMax; }
            set
            {
                if (value < 1 || value > 10)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Sequence length must be between 1 and 10.");
                }
                k_SequenceLengthMax = value;
            }
        }

        public eColor[] GetSecret()
        {
            return (eColor[])r_Secret.Clone();
        }

        private eColor[] generateSecret()
        {
            eColor[] secret = new eColor[k_SequenceLengthMax];

            for (int i = 0; i < k_SequenceLengthMax; i++)
            {
                eColor candidate;
                bool isDuplicate;

                do
                {
                    candidate = (eColor)sr_Random.Next(Enum.GetValues(typeof(eColor)).Length);
                    isDuplicate = secret.Take(i).Contains(candidate);
                }
                while (isDuplicate);

                secret[i] = candidate;
            }

            return secret;
        }

        public int getCorrectLettersInPosition(string i_GuessFromUser)
        {
            int numCorrectLetters = 0;

            for (int i = 0; i < k_SequenceLengthMax; i++)
            {
                if ((char)r_Secret[i] == i_GuessFromUser[i])
                {
                    numCorrectLetters++;
                }
            }

            if (numCorrectLetters == k_SequenceLengthMax)
            {
                m_HasWon = true;
            }

            return numCorrectLetters;
        }

        public int getCorrectLettersInWrongPosition(string i_GuessFromUser)
        {
            int countLettersInBadPosition = 0;
            bool[] matchedInSecret = new bool[k_SequenceLengthMax];
            bool[] matchedInGuess = new bool[k_SequenceLengthMax];

            for (int i = 0; i < k_SequenceLengthMax; i++)
            {
                if ((char)r_Secret[i] == i_GuessFromUser[i])
                {
                    matchedInSecret[i] = true;
                    matchedInGuess[i] = true;
                }
            }

            for (int i = 0; i < k_SequenceLengthMax; i++)
            {
                if (matchedInGuess[i])
                {
                    continue;
                }

                for (int j = 0; j < k_SequenceLengthMax; j++)
                {
                    if (!matchedInSecret[j] && i_GuessFromUser[i] == (char)r_Secret[j])
                    {
                        countLettersInBadPosition++;
                        matchedInSecret[j] = true;
                        break;
                    }
                }
            }

            return countLettersInBadPosition;
        }

        public eFeedback[] getGradeByGuess(eColor[] i_GuessFromUser)
        {
            if (i_GuessFromUser == null || i_GuessFromUser.Length != k_SequenceLengthMax)
            {
                throw new ArgumentException($"Guess must have exactly {k_SequenceLengthMax} colours.");
            }

            eFeedback[] feedback = new eFeedback[k_SequenceLengthMax];
            bool[] matchedInSecret = new bool[k_SequenceLengthMax];
            bool[] matchedInGuess = new bool[k_SequenceLengthMax];

            for (int i = 0; i < k_SequenceLengthMax; i++)
            {
                if (i_GuessFromUser[i] == r_Secret[i])
                {
                    feedback[i] = eFeedback.CorrectPosition;
                    matchedInSecret[i] = true;
                    matchedInGuess[i] = true;
                }
            }

            for (int i = 0; i < k_SequenceLengthMax; i++)
            {
                if (matchedInGuess[i])
                {
                    continue;
                }

                for (int j = 0; j < k_SequenceLengthMax; j++)
                {
                    if (!matchedInSecret[j] && i_GuessFromUser[i] == r_Secret[j])
                    {
                        feedback[i] = eFeedback.CorrectColorWrongPosition;
                        matchedInSecret[j] = true;
                        break;
                    }
                }
            }

            if (feedback.All(f => f == eFeedback.CorrectPosition))
            {
                m_HasWon = true;
            }

            return feedback;
        }


        public bool GetDidUserWin()
        {
            return m_HasWon;
        }
    }
}
