using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace B25_Ex05_OriCohen_207008590_AlonZylberberg_315853739
{
    public class FormPickAColor : Form
    {
        private List<Button> m_ColorButtons;
        private Color m_ChosenColor;

        private System.ComponentModel.IContainer components = null;

        public Color ChosenColor
        {
            get { return m_ChosenColor; }
        }

        public Action<Color> ColorChosenAction;

        public FormPickAColor()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = AutoScaleMode.Font;
            this.Text = "Choose Color";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            m_ColorButtons = new List<Button>();

            Color[] colors = new Color[]
            {
                Color.Red, Color.Green, Color.Blue, Color.Yellow,
                Color.Purple, Color.Orange, Color.Brown, Color.Pink
            };

            int totalButtons = colors.Length;
            int buttonsPerRow = 4;
            int buttonSize = 60;
            int padding = 10;

            int rows = (int)Math.Ceiling((double)totalButtons / buttonsPerRow);
            int formWidth = buttonsPerRow * (buttonSize + padding) + padding;
            int formHeight = rows * (buttonSize + padding) + padding + 40;

            this.ClientSize = new Size(formWidth, formHeight);

            for (int i = 0; i < colors.Length; i++)
            {
                Button colorButton = new Button();
                colorButton.BackColor = colors[i];
                colorButton.Width = colorButton.Height = buttonSize;
                colorButton.FlatStyle = FlatStyle.Flat;
                colorButton.FlatAppearance.BorderSize = 1;
                colorButton.Location = new Point(
                    (i % buttonsPerRow) * (buttonSize + padding) + padding,
                    (i / buttonsPerRow) * (buttonSize + padding) + padding
                );
                colorButton.Tag = colors[i];
                colorButton.Click += colorButton_Click;

                m_ColorButtons.Add(colorButton);
                this.Controls.Add(colorButton);
            }
        }

        private void colorButton_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            Color chosenColor = (Color)clickedButton.Tag;

            m_ChosenColor = chosenColor;

            // Call the action if it's assigned
            if (ColorChosenAction != null)
            {
                ColorChosenAction(chosenColor);
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
