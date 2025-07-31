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
    /// Interaction logic for SurveyEditDialog.xaml
    /// </summary>
    public partial class SurveyEditDialog : Window
    {

        public string SurveyName { get; set; }
        public string Description { get; set; }
        public SurveyEditDialog()
        {
            InitializeComponent();
            Title = "Thêm khảo sát mới";
        }
        public SurveyEditDialog(Survey survey) : this()
        {
            try
            {
               // MessageBox.Show($"SurveyEditDialog constructor called with: {survey?.SurveyName}", "Dialog Debug");

                Title = "Chỉnh sửa khảo sát";

               // MessageBox.Show("About to set SurveyNameTextBox.Text", "Dialog Debug");
                SurveyNameTextBox.Text = survey.SurveyName ?? "";

              //  MessageBox.Show("About to set DescriptionTextBox.Text", "Dialog Debug");
                DescriptionTextBox.Text = survey.Description ?? "";

             //   MessageBox.Show("SurveyEditDialog constructor completed successfully", "Dialog Debug");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in SurveyEditDialog constructor: {ex.Message}", "Dialog Error");
                throw;
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SurveyNameTextBox.Text))
            {
                MessageBox.Show("Vui lòng nhập tên khảo sát.", "Thông báo",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                SurveyNameTextBox.Focus();
                return;
            }

            SurveyName = SurveyNameTextBox.Text.Trim();
            Description = DescriptionTextBox.Text.Trim();

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
