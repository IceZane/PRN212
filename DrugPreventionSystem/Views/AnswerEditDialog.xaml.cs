using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DrugPreventionSystem.Views
{
    /// <summary>
    /// Interaction logic for AnswerEditDialog.xaml
    /// </summary>
    public partial class AnswerEditDialog : Window
    {

        public string AnswerText { get; set; }
        public int Score { get; set; }
        public AnswerEditDialog()
        {
            InitializeComponent();


            Title = "Thêm đáp án mới";
            ScoreTextBox.Text = "0";
        }
        public AnswerEditDialog(SurveyAnswer answer) : this()
        {
            Title = "Chỉnh sửa đáp án";
            AnswerTextBox.Text = answer.AnswerText ?? "";
            ScoreTextBox.Text = answer.Score.ToString();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(AnswerTextBox.Text))
            {
                MessageBox.Show("Vui lòng nhập nội dung đáp án.", "Thông báo",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                AnswerTextBox.Focus();
                return;
            }

            if (!int.TryParse(ScoreTextBox.Text, out int score))
            {
                MessageBox.Show("Vui lòng nhập điểm số hợp lệ.", "Thông báo",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                ScoreTextBox.Focus();
                return;
            }

            AnswerText = AnswerTextBox.Text.Trim();
            Score = score;

            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
