using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Lab1_Survey_WinForms
{
    public partial class Form1 : Form
    {
        private List<string> _questions = new List<string>
        {
            "1) Як вас звати?",
            "2) Скільки вам років?",
            "3) Які цілі на цей семестр?",
            "4) Скільки лабораторних вже зробили?",
            "5) Який прогрес з дипломом?"
        };

        private Dictionary<int, string> _answers = new Dictionary<int, string>();
        private int _index = 0;

        private string _outputFilePath = null;

        public Form1()
        {
            InitializeComponent();

            txtAnswer.Multiline = true;
            txtAnswer.ScrollBars = ScrollBars.Vertical;

            UpdateUI();
        }

        private void UpdateUI()
        {
            lblQuestion.Text = _questions[_index];

            string a;
            txtAnswer.Text = _answers.TryGetValue(_index, out a) ? a : "";

            btnPrev.Enabled = _index > 0;
            btnNext.Enabled = _index < _questions.Count - 1;

            lblStatus.Text = "Питання " + (_index + 1) + " з " + _questions.Count;
        }

        private void SaveCurrentAnswer()
        {
            _answers[_index] = (txtAnswer.Text ?? "").Trim();
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            SaveCurrentAnswer();
            if (_index > 0) _index--;
            UpdateUI();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            SaveCurrentAnswer();
            if (_index < _questions.Count - 1) _index++;
            UpdateUI();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveCurrentAnswer();

            if (string.IsNullOrWhiteSpace(_outputFilePath))
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Title = "Куди зберегти відповіді?";
                sfd.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                sfd.FileName = "survey_answers.txt";

                if (sfd.ShowDialog() != DialogResult.OK)
                {
                    lblStatus.Text = "Збереження скасовано.";
                    return;
                }

                _outputFilePath = sfd.FileName;
            }

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("=== Опитування ===");
                sb.AppendLine("Дата/час: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                sb.AppendLine();

                for (int i = 0; i < _questions.Count; i++)
                {
                    string ans;
                    if (!_answers.TryGetValue(i, out ans)) ans = "";

                    sb.AppendLine(_questions[i]);
                    sb.AppendLine("Відповідь: " + ans);
                    sb.AppendLine(new string('-', 40));
                }

                sb.AppendLine();
                sb.AppendLine("=== Кінець запису ===");
                sb.AppendLine();

                File.AppendAllText(_outputFilePath, sb.ToString(), Encoding.UTF8);

                lblStatus.Text = "Збережено: " + _outputFilePath;
                MessageBox.Show("Відповіді збережено успішно!", "OK",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка збереження: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


    }
}