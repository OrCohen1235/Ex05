using Ex05_OriCohen_207008590_AlonZylberberg_315853739;
using System;
using System.Windows.Forms;

namespace B25_Ex05_OriCohen_207008590_AlonZylberberg_315853739
{
    public class FormNumberOfChances : Form
    {
        private Button buttonStart;
        private Button buttonNumberOfChances;
        private const int k_MinimumNumberOfChances = 4;
        private const int k_MaximumNumberOfChances = 10;
        private int CountClickers { get; set; } = k_MinimumNumberOfChances;

        public FormNumberOfChances()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.buttonStart = new Button();
            this.buttonNumberOfChances = new Button();

            this.buttonStart.Location = new System.Drawing.Point(160, 100);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(100, 40);
            this.buttonStart.Text = "Start";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new EventHandler(this.buttonStart_Click);

            this.buttonNumberOfChances.Location = new System.Drawing.Point(40, 20);
            this.buttonNumberOfChances.Name = "buttonNumberOfChances";
            this.buttonNumberOfChances.Size = new System.Drawing.Size(400, 40);
            this.buttonNumberOfChances.Text = $"Number Of Chances: {CountClickers}";
            this.buttonNumberOfChances.UseVisualStyleBackColor = true;
            this.buttonNumberOfChances.Click += new EventHandler(this.buttonNumberOfChances_Click);

            this.ClientSize = new System.Drawing.Size(500, 300);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.buttonNumberOfChances);
            this.Name = "FormNumberOfChances";
            this.Text = "Bool Pgia";
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            FormGameUI formGameUI = new FormGameUI(CountClickers);
            this.Hide();
            formGameUI.ShowDialog();
            this.Show();
            this.Close();
        }

        private void buttonNumberOfChances_Click(object sender, EventArgs e)
        {
            CountClickers++;
            if (CountClickers > k_MaximumNumberOfChances)
            {
                CountClickers = k_MinimumNumberOfChances;
            }

            buttonNumberOfChances.Text = $"Number Of Chances: {CountClickers}";
        }
    }
}