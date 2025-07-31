using BusinessObjects;
using DataAccessObject.Service;
using DrugPreventionSystem.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace DrugPreventionSystem.ViewModels
{
    public class SurveyManagementViewModel : INotifyPropertyChanged, IDisposable
    {

        private ICollectionView _surveyCollectionView;
        public ICollectionView SurveyCollectionView => _surveyCollectionView ??= CollectionViewSource.GetDefaultView(Surveys);
        private readonly SurveyService _surveyService;
        private Survey _selectedSurvey;
        private SurveyQuestion _selectedQuestion;
        private SurveyAnswer _selectedAnswer;
      

        public ObservableCollection<Survey> Surveys { get; set; }
        public ObservableCollection<SurveyQuestion> Questions { get; set; }
        public ObservableCollection<SurveyAnswer> Answers { get; set; }

        public Survey SelectedSurvey
        {
            get => _selectedSurvey;
            set
            {
                if (_selectedSurvey != value)
                {
                    _selectedSurvey = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(HasSelectedSurvey));
               //     MessageBox.Show($"Selected: {_selectedSurvey?.SurveyName}");

                    // Nếu muốn load câu hỏi khi chọn khảo sát:
                    if (_selectedSurvey != null)
                        LoadQuestionsForSurvey();
                }
            }
        }
      

        public SurveyQuestion SelectedQuestion
        {
            get => _selectedQuestion;
            set
            {
                _selectedQuestion = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasSelectedQuestion));
                LoadAnswersForQuestion();
            }
        }

        public SurveyAnswer SelectedAnswer
        {
            get => _selectedAnswer;
            set
            {
                _selectedAnswer = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasSelectedAnswer));
            }
        }

        public bool HasSelectedSurvey => SelectedSurvey != null;
        public bool HasSelectedQuestion => SelectedQuestion != null;
        public bool HasSelectedAnswer => SelectedAnswer != null;

        // Additional property for UI display
        public int QuestionCount => SelectedSurvey?.SurveyQuestions?.Count ?? 0;

        public SurveyManagementViewModel()
        {
            _surveyService = new SurveyService();
            Surveys = new ObservableCollection<Survey>();
            Questions = new ObservableCollection<SurveyQuestion>();
            Answers = new ObservableCollection<SurveyAnswer>();

            LoadSurveys();
        }

        // Survey Methods
        public async void LoadSurveys()
        {
            try
            {
                var surveys = await _surveyService.GetAllSurveysAsync();

                // Xóa các khảo sát hiện tại trong ObservableCollection
                Surveys.Clear();

                // Thêm lại các khảo sát từ cơ sở dữ liệu vào ObservableCollection
                foreach (var survey in surveys)
                {
                    Surveys.Add(survey);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể tải danh sách khảo sát: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public async void AddSurvey(string surveyName, string description)
        {
            try
            {
                var newSurvey = new Survey
                {
                    SurveyName = surveyName,
                    Description = description
                };

                // Thêm khảo sát vào cơ sở dữ liệu
                var addedSurvey = await _surveyService.AddSurveyAsync(newSurvey);

                // Thêm khảo sát mới vào ObservableCollection
                Surveys.Add(addedSurvey);

                // Cập nhật UI (mặc dù ObservableCollection đã tự động thông báo, nhưng đây là bước chắc chắn)
                OnPropertyChanged(nameof(Surveys));

                // Nếu cần thiết, gọi lại LoadSurveys để làm mới danh sách khảo sát từ cơ sở dữ liệu
                LoadSurveys();  // Đảm bảo tải lại tất cả khảo sát sau khi thêm
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể thêm khảo sát: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public async void UpdateSurvey(string surveyName, string description)
        {
            try
            {
                if (SelectedSurvey == null) return;  // Nếu không có khảo sát nào được chọn, thoát phương thức

                // Cập nhật thông tin khảo sát
                SelectedSurvey.SurveyName = surveyName;
                SelectedSurvey.Description = description;

                // Gọi dịch vụ để cập nhật khảo sát trong cơ sở dữ liệu
                await _surveyService.UpdateSurveyAsync(SelectedSurvey);

                // Cập nhật UI sau khi sửa khảo sát
                OnPropertyChanged(nameof(Surveys));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể cập nhật khảo sát: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public async void DeleteSurvey()
        {
            try
            {
                if (SelectedSurvey == null) return;

                await _surveyService.DeleteSurveyAsync(SelectedSurvey.SurveyId);
                Surveys.Remove(SelectedSurvey);

                Questions.Clear();
                Answers.Clear();
                SelectedSurvey = null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Không thể xóa khảo sát: {ex.Message}");
            }
        }

        // Question Methods
        private async void LoadQuestionsForSurvey()
        {
            try
            {
                Questions.Clear();
                Answers.Clear();
                SelectedQuestion = null;

                if (SelectedSurvey == null) return;

                var questions = await _surveyService.GetQuestionsBySurveyAsync(SelectedSurvey.SurveyId);
                foreach (var question in questions)
                {
                    Questions.Add(question);
                }

                OnPropertyChanged(nameof(QuestionCount));
            }
            catch (Exception ex)
            {
                throw new Exception($"Không thể tải câu hỏi: {ex.Message}");
            }
        }

        public async void AddQuestion(string questionText)
        {
            try
            {
                if (SelectedSurvey == null) return;

                var newQuestion = new SurveyQuestion
                {
                    SurveyId = SelectedSurvey.SurveyId,
                    QuestionText = questionText
                };

                var addedQuestion = await _surveyService.AddQuestionAsync(newQuestion);
                Questions.Add(addedQuestion);

                // Update the navigation property
                SelectedSurvey.SurveyQuestions.Add(addedQuestion);
                OnPropertyChanged(nameof(QuestionCount));
                OnPropertyChanged(nameof(Surveys));
            }
            catch (Exception ex)
            {
                throw new Exception($"Không thể thêm câu hỏi: {ex.Message}");
            }
        }

        public async void UpdateQuestion(string questionText)
        {
            try
            {
                if (SelectedQuestion == null) return;

                SelectedQuestion.QuestionText = questionText;
                await _surveyService.UpdateQuestionAsync(SelectedQuestion);
                OnPropertyChanged(nameof(Questions));
            }
            catch (Exception ex)
            {
                throw new Exception($"Không thể cập nhật câu hỏi: {ex.Message}");
            }
        }

        public async void DeleteQuestion()
        {
            try
            {
                if (SelectedQuestion == null) return;

                var questionId = SelectedQuestion.QuestionId;

                await _surveyService.DeleteQuestionAsync(questionId);
                Questions.Remove(SelectedQuestion);

                // Update navigation property
                if (SelectedSurvey != null)
                {
                    var questionToRemove = SelectedSurvey.SurveyQuestions
                        .FirstOrDefault(q => q.QuestionId == questionId);
                    if (questionToRemove != null)
                    {
                        SelectedSurvey.SurveyQuestions.Remove(questionToRemove);
                    }
                    OnPropertyChanged(nameof(QuestionCount));
                    OnPropertyChanged(nameof(Surveys));
                }

                Answers.Clear();
                SelectedQuestion = null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Không thể xóa câu hỏi: {ex.Message}");
            }
        }

        public async void SaveQuestion()
        {
            try
            {
                if (SelectedQuestion == null) return;
                await _surveyService.UpdateQuestionAsync(SelectedQuestion);
            }
            catch (Exception ex)
            {
                throw new Exception($"Không thể lưu câu hỏi: {ex.Message}");
            }
        }

        public void CancelQuestionEdit()
        {
            // Reload questions to cancel any unsaved changes
            LoadQuestionsForSurvey();
        }

        // Answer Methods
        private async void LoadAnswersForQuestion()
        {
            try
            {
                Answers.Clear();
                SelectedAnswer = null;

                if (SelectedQuestion == null) return;

                var answers = await _surveyService.GetAnswersByQuestionAsync(SelectedQuestion.QuestionId);
                foreach (var answer in answers)
                {
                    Answers.Add(answer);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Không thể tải đáp án: {ex.Message}");
            }
        }

        public async void AddAnswer(string answerText, int score)
        {
            try
            {
                if (SelectedQuestion == null) return;

                var newAnswer = new SurveyAnswer
                {
                    QuestionId = SelectedQuestion.QuestionId,
                    AnswerText = answerText,
                    Score = score
                };

                var addedAnswer = await _surveyService.AddAnswerAsync(newAnswer);
                Answers.Add(addedAnswer);

                // Update navigation property
                SelectedQuestion.SurveyAnswers.Add(addedAnswer);
            }
            catch (Exception ex)
            {
                throw new Exception($"Không thể thêm đáp án: {ex.Message}");
            }
        }

        public async void UpdateAnswer(string answerText, int score)
        {
            try
            {
                if (SelectedAnswer == null) return;

                SelectedAnswer.AnswerText = answerText;
                SelectedAnswer.Score = score;

                await _surveyService.UpdateAnswerAsync(SelectedAnswer);
                OnPropertyChanged(nameof(Answers));
            }
            catch (Exception ex)
            {
                throw new Exception($"Không thể cập nhật đáp án: {ex.Message}");
            }
        }

        public async void DeleteAnswer()
        {
            try
            {
                if (SelectedAnswer == null) return;

                // Lưu trước AnswerId để dùng sau khi SelectedAnswer bị gán null
                var answerId = SelectedAnswer.AnswerId;

                await _surveyService.DeleteAnswerAsync(answerId);
                Answers.Remove(SelectedAnswer);

                // Update navigation property
                if (SelectedQuestion != null)
                {
                    var answerToRemove = SelectedQuestion.SurveyAnswers
                        .FirstOrDefault(a => a.AnswerId == answerId);
                    if (answerToRemove != null)
                    {
                        SelectedQuestion.SurveyAnswers.Remove(answerToRemove);
                    }
                }

                SelectedAnswer = null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Không thể xóa đáp án: {ex.Message}");
            }
        }

        public async void SaveAnswer()
        {
            try
            {
                if (SelectedAnswer == null) return;
                await _surveyService.UpdateAnswerAsync(SelectedAnswer);
            }
            catch (Exception ex)
            {
                throw new Exception($"Không thể lưu đáp án: {ex.Message}");
            }
        }

        public void CancelAnswerEdit()
        {
            // Reload answers to cancel any unsaved changes
            LoadAnswersForQuestion();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Dispose()
        {
            _surveyService?.Dispose();
        }
    }
}
