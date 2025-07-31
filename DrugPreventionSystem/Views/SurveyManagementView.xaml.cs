    using DrugPreventionSystem.ViewModels;
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
        /// Interaction logic for SurveyManagementView.xaml
        /// </summary>
        public partial class SurveyManagementView : UserControl
        {

        public SurveyManagementViewModel ViewModel => (SurveyManagementViewModel)this.DataContext;
        public SurveyManagementView()
            {
                InitializeComponent();

             

                // Subscribe to Unloaded event to dispose ViewModel
                this.Unloaded += SurveyManagementView_Unloaded;
            }
            private void SurveyManagementView_Unloaded(object sender, RoutedEventArgs e)
            {
                ViewModel?.Dispose();
            }
            private void AddSurveyButton_Click(object sender, RoutedEventArgs e)
            {
                try
                {
                    var dialog = new SurveyEditDialog();
                    if (dialog.ShowDialog() == true)
                    {
                        ViewModel.AddSurvey(dialog.SurveyName, dialog.Description);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi thêm khảo sát: {ex.Message}", "Lỗi",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            private void EditSurveyButton_Click(object sender, RoutedEventArgs e)
            {
            try
            {
                if (ViewModel.SelectedSurvey == null)
                {
                    MessageBox.Show("Vui lòng chọn khảo sát cần sửa.", "Thông báo",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

            //MessageBox.Show($"Creating dialog for: {ViewModel.SelectedSurvey.SurveyName}", "Debug");

                var dialog = new SurveyEditDialog(ViewModel.SelectedSurvey);

             //   MessageBox.Show("Dialog created successfully, about to show", "Debug");

                var result = dialog.ShowDialog();

             //   MessageBox.Show($"Dialog result: {result}", "Debug");

                if (result == true)
                {
                    ViewModel.UpdateSurvey(dialog.SurveyName, dialog.Description);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}\nStackTrace: {ex.StackTrace}", "Error Details");
            }
        }

            private void DeleteSurveyButton_Click(object sender, RoutedEventArgs e)
            {
                try
                {
                    if (ViewModel.SelectedSurvey == null)
                    {
                        MessageBox.Show("Vui lòng chọn khảo sát cần xóa.", "Thông báo",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }

                    var result = MessageBox.Show(
                        $"Bạn có chắc chắn muốn xóa khảo sát '{ViewModel.SelectedSurvey.SurveyName}'?\n" +
                        "Thao tác này sẽ xóa tất cả câu hỏi và đáp án liên quan.",
                        "Xác nhận xóa",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Warning);

                    if (result == MessageBoxResult.Yes)
                    {
                        ViewModel.DeleteSurvey();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi xóa khảo sát: {ex.Message}", "Lỗi",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            private void SurveysDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
            {
                if (ViewModel.SelectedSurvey != null)
                {
                    EditSurveyButton_Click(sender, e);
                }
            }

            // Question Management Events
            private void AddQuestionButton_Click(object sender, RoutedEventArgs e)
            {
                try
                {
                    if (ViewModel.SelectedSurvey == null)
                    {
                        MessageBox.Show("Vui lòng chọn khảo sát trước.", "Thông báo",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }

                    var dialog = new QuestionEditDialog();
                    if (dialog.ShowDialog() == true)
                    {
                        ViewModel.AddQuestion(dialog.QuestionText);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi thêm câu hỏi: {ex.Message}", "Lỗi",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            private void EditQuestionButton_Click(object sender, RoutedEventArgs e)
            {
                try
                {
                    if (ViewModel.SelectedQuestion == null)
                    {
                        MessageBox.Show("Vui lòng chọn câu hỏi cần sửa.", "Thông báo",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }

                    var dialog = new QuestionEditDialog(ViewModel.SelectedQuestion);
                    if (dialog.ShowDialog() == true)
                    {
                        ViewModel.UpdateQuestion(dialog.QuestionText);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi sửa câu hỏi: {ex.Message}", "Lỗi",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            private void DeleteQuestionButton_Click(object sender, RoutedEventArgs e)
            {
                try
                {
                    if (ViewModel.SelectedQuestion == null)
                    {
                        MessageBox.Show("Vui lòng chọn câu hỏi cần xóa.", "Thông báo",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }

                    var result = MessageBox.Show(
                        $"Bạn có chắc chắn muốn xóa câu hỏi này?\n" +
                        "Thao tác này sẽ xóa tất cả đáp án liên quan.",
                        "Xác nhận xóa",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Warning);

                    if (result == MessageBoxResult.Yes)
                    {
                        ViewModel.DeleteQuestion();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi xóa câu hỏi: {ex.Message}", "Lỗi",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            private void SaveQuestionButton_Click(object sender, RoutedEventArgs e)
            {
                try
                {
                    if (ViewModel.SelectedQuestion == null)
                    {
                        MessageBox.Show("Không có câu hỏi nào được chọn.", "Thông báo",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }

                    ViewModel.SaveQuestion();
                    MessageBox.Show("Đã lưu câu hỏi thành công!", "Thành công",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi lưu câu hỏi: {ex.Message}", "Lỗi",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            private void CancelQuestionButton_Click(object sender, RoutedEventArgs e)
            {
                ViewModel.CancelQuestionEdit();
            }

            // Answer Management Events
            private void AddAnswerButton_Click(object sender, RoutedEventArgs e)
            {
                try
                {
                    if (ViewModel.SelectedQuestion == null)
                    {
                        MessageBox.Show("Vui lòng chọn câu hỏi trước.", "Thông báo",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }

                    var dialog = new AnswerEditDialog();
                    if (dialog.ShowDialog() == true)
                    {
                        ViewModel.AddAnswer(dialog.AnswerText, dialog.Score);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi thêm đáp án: {ex.Message}", "Lỗi",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            private void EditAnswerButton_Click(object sender, RoutedEventArgs e)
            {
                try
                {
                    if (ViewModel.SelectedAnswer == null)
                    {
                        MessageBox.Show("Vui lòng chọn đáp án cần sửa.", "Thông báo",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }

                    var dialog = new AnswerEditDialog(ViewModel.SelectedAnswer);
                    if (dialog.ShowDialog() == true)
                    {
                        ViewModel.UpdateAnswer(dialog.AnswerText, dialog.Score);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi sửa đáp án: {ex.Message}", "Lỗi",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            private void DeleteAnswerButton_Click(object sender, RoutedEventArgs e)
            {
                try
                {
                    if (ViewModel.SelectedAnswer == null)
                    {
                        MessageBox.Show("Vui lòng chọn đáp án cần xóa.", "Thông báo",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }

                    var result = MessageBox.Show(
                        "Bạn có chắc chắn muốn xóa đáp án này?",
                        "Xác nhận xóa",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Warning);

                    if (result == MessageBoxResult.Yes)
                    {
                        ViewModel.DeleteAnswer();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi xóa đáp án: {ex.Message}", "Lỗi",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            private void SaveAnswerButton_Click(object sender, RoutedEventArgs e)
            {
                try
                {
                    if (ViewModel.SelectedAnswer == null)
                    {
                        MessageBox.Show("Không có đáp án nào được chọn.", "Thông báo",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }

                    ViewModel.SaveAnswer();
                    MessageBox.Show("Đã lưu đáp án thành công!", "Thành công",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi lưu đáp án: {ex.Message}", "Lỗi",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            private void CancelAnswerButton_Click(object sender, RoutedEventArgs e)
            {
                ViewModel.CancelAnswerEdit();
            }
        }
    }
