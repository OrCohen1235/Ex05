using B25_Ex05_OriCohen_207008590_AlonZylberberg_315853739;
using Ex05_OriCohen_207008590_AlonZylberberg_315853739;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Ex05_OriCohen_207008590_AlonZylberberg_315853739
{
    public class FormGameUI : Form
    {
        private const int k_SlotSize = 80;
        private const int k_SlotGap = 10; 
        private const int k_SubmitButtonWidth = 60;
        private const int k_FeedbackSize = 38;


        private readonly int r_NumOfCols;
        private readonly int r_NumOfRows;
        private readonly GameLogic r_GameLogic;

        private readonly Button[,] r_SlotButtons;
        private readonly Button[] r_SubmitButtons;
        private readonly Button[,] r_FeedbackButtons;
        private readonly Button[] r_SecretButtons;

        private int m_ActiveRow;

        internal FormGameUI(int i_NumOfRows)
        {
            r_GameLogic = new GameLogic();
            r_NumOfRows = i_NumOfRows;
            r_NumOfCols = r_GameLogic.GetSecret().Length;

            r_SlotButtons = new Button[r_NumOfRows, r_NumOfCols];
            r_SubmitButtons = new Button[r_NumOfRows];
            r_FeedbackButtons = new Button[r_NumOfRows, r_NumOfCols];
            r_SecretButtons = new Button[r_NumOfCols];

            m_ActiveRow = 0;
            initializeComponent();
        }

        private void initializeComponent()
        {
            Text = "Bool Pgia – Game";
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            AutoSize = false;
            BackColor = Color.Beige;

            int totalWidth = r_NumOfCols * (k_SlotSize + k_SlotGap) + k_SlotGap
                                                                    + k_SubmitButtonWidth + k_SlotGap
                                                                    + 2 * k_FeedbackSize + 2 * 28;

            int totalHeight = (r_NumOfRows + 2) * (k_SlotSize + k_SlotGap);

            Size = new Size(totalWidth, totalHeight);


            intializeSecret();

            for (int row = 0; row < r_NumOfRows; row++)
            {
                addRowControls(row);
            }
        }

        private void intializeSecret()
        {
            for (int col = 0; col < r_NumOfCols; col++)
            {
                Button button = new Button();
                button.Size = new Size(k_SlotSize, k_SlotSize);
                button.Left = col * (k_SlotSize + k_SlotGap) + k_SlotGap;
                button.Top = k_SlotGap;
                button.BackColor = Color.Black;
                button.Enabled = false;
                button.Font = new Font("Arial", 14, FontStyle.Bold);

                Controls.Add(button);
                r_SecretButtons[col] = button;
            }
        }

        private void addRowControls(int i_Row)
        {
            int top = (i_Row + 1) * (k_SlotSize + k_SlotGap) + k_SlotGap;

            for (int col = 0; col < r_NumOfCols; col++)
            {
                Button slot = new Button();
                slot.Size = new Size(k_SlotSize, k_SlotSize);
                slot.Left = col * (k_SlotSize + k_SlotGap) + k_SlotGap;
                slot.Top = top;
                slot.BackColor = Color.White;
                slot.Enabled = i_Row == 0;
                slot.Tag = new Point(i_Row, col);
                slot.Click += slot_Click;
                slot.Font = new Font("Arial", 16, FontStyle.Bold);

                Controls.Add(slot);
                r_SlotButtons[i_Row, col] = slot;
            }


            Button submit = new Button();
            submit.Size = new Size(k_SubmitButtonWidth, k_SlotSize);
            submit.Left = r_NumOfCols * (k_SlotSize + k_SlotGap) + k_SlotGap;
            submit.Top = top;
            submit.Text = "→";
            submit.Font = new Font("Arial", 24, FontStyle.Bold);
            submit.Enabled = false;
            submit.Tag = i_Row;
            submit.Click += submit_Click;

            Controls.Add(submit);
            r_SubmitButtons[i_Row] = submit;

            for (int i = 0; i < r_NumOfCols; i++)
            {
                Button feedback = new Button();
                feedback.Size = new Size(k_FeedbackSize, k_FeedbackSize);
                feedback.Left = submit.Right + 15 + (i % 2) * (k_FeedbackSize + 2);
                feedback.Top = top + (i / 2) * (k_FeedbackSize + 2);
                feedback.Enabled = false;
                feedback.BackColor = Color.Transparent;
                feedback.Font = new Font("Arial", 14);

                Controls.Add(feedback);
                r_FeedbackButtons[i_Row, i] = feedback;
            }
        }


        private void slot_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            Point tag = (Point)button.Tag;

            using (FormPickAColor picker = new FormPickAColor())
            {
                if (picker.ShowDialog() == DialogResult.OK)
                {
                    if (!isColorUsed(tag.X, picker.ChosenColor))
                    {
                        button.BackColor = picker.ChosenColor;

                        if (isRowFilled(tag.X))
                        {
                            r_SubmitButtons[tag.X].Enabled = true;
                        }
                    }
                    else
                    {
                        MessageBox.Show("You cannot use the same color twice in a row.");
                    }
                }
            }
        }

        private void submit_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            int row = (int)button.Tag;

            GameLogic.eColor[] guess = new GameLogic.eColor[r_NumOfCols];
            for (int i = 0; i < r_NumOfCols; i++)
            {
                guess[i] = toEnum(r_SlotButtons[row, i].BackColor);
            }

            GameLogic.eFeedback[] feedback = r_GameLogic.GetGradeByGuess(guess);

            for (int i = 0; i < feedback.Length; i++)
            {
                r_FeedbackButtons[row, i].BackColor = feedback[i] == GameLogic.eFeedback.CorrectPosition ? Color.Black :
                                                       feedback[i] == GameLogic.eFeedback.CorrectColorWrongPosition ? Color.Yellow :
                                                       Color.Transparent;
            }

            lockRow(row);

            if (r_GameLogic.GetDidUserWin())
            {
                endGame(true);
            }
            else if (row == r_NumOfRows - 1)
            {
                endGame(false);
            }
            else
            {
                enableRow(row + 1);
            }
        }

        private bool isColorUsed(int i_Row, Color i_Color)
        {
            for (int i = 0; i < r_NumOfCols; i++)
            {
                if (r_SlotButtons[i_Row, i].BackColor == i_Color)
                {
                    return true;
                }
            }

            return false;
        }

        private bool isRowFilled(int i_Row)
        {
            for (int i = 0; i < r_NumOfCols; i++)
            {
                if (r_SlotButtons[i_Row, i].BackColor == Color.White)
                {
                    return false;
                }
            }

            return true;
        }

        private void lockRow(int i_Row)
        {
            for (int i = 0; i < r_NumOfCols; i++)
            {
                r_SlotButtons[i_Row, i].Enabled = false;
            }

            r_SubmitButtons[i_Row].Enabled = false;
        }

        private void enableRow(int i_Row)
        {
            for (int i = 0; i < r_NumOfCols; i++)
            {
                r_SlotButtons[i_Row, i].Enabled = true;
            }
        }

        private static GameLogic.eColor toEnum(Color i_Color)
        {
            GameLogic.eColor returnValue = GameLogic.eColor.A;
            bool found = true;
            
            if (i_Color == Color.Red) returnValue = GameLogic.eColor.A;
            else if (i_Color == Color.Blue) returnValue = GameLogic.eColor.B;
            else if (i_Color == Color.Green) returnValue = GameLogic.eColor.C;
            else if (i_Color == Color.Yellow) returnValue = GameLogic.eColor.D;
            else if (i_Color == Color.Purple) returnValue = GameLogic.eColor.E;
            else if (i_Color == Color.Orange) returnValue = GameLogic.eColor.F;
            else if (i_Color == Color.Brown) returnValue = GameLogic.eColor.G;
            else if (i_Color == Color.Pink) returnValue = GameLogic.eColor.H;
            else found = false;
            if (!found)
            {
            throw new ArgumentException("Unsupported color");
            }

            return returnValue;
        }

        private Color mapColor(GameLogic.eColor i_Color)
        {
            switch (i_Color)
            {
                case GameLogic.eColor.A: return Color.Red;
                case GameLogic.eColor.B: return Color.Blue;
                case GameLogic.eColor.C: return Color.Green;
                case GameLogic.eColor.D: return Color.Yellow;
                case GameLogic.eColor.E: return Color.Purple;
                case GameLogic.eColor.F: return Color.Orange;
                case GameLogic.eColor.G: return Color.Brown;
                case GameLogic.eColor.H: return Color.Pink;
                default: return Color.Black;
            }
        }


        private void revealGuess()
        {
            GameLogic.eColor[] secret = r_GameLogic.GetSecret();

            for (int i = 0; i < secret.Length; i++)
            {
                r_SecretButtons[i].BackColor = mapColor(secret[i]);
            }
        }

        private void endGame(bool won)
        {
            string message = won ? "You cracked the code!" : "Out of guesses. Check Secret ON TOP: ";
            MessageBox.Show(message);
            revealGuess();
        }
    }
}
