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
    /// Interaction logic for QuestionEditDialog.xaml
    /// </summary>
    public partial class QuestionEditDialog : Window
    {

        public string QuestionText { get; set; }
        public QuestionEditDialog()
        {
            InitializeComponent();
            Title = "Thêm câu hỏi mới";

        }
        public QuestionEditDialog(SurveyQuestion question) : this()
        {
            Title = "Chỉnh sửa câu hỏi";
            QuestionTextBox.Text = question.QuestionText ?? "";
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(QuestionTextBox.Text))
            {
                MessageBox.Show("Vui lòng nhập nội dung câu hỏi.", "Thông báo",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                QuestionTextBox.Focus();
                return;
            }

            QuestionText = QuestionTextBox.Text.Trim();
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
