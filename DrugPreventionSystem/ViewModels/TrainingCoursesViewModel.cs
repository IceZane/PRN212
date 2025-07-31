using BusinessObjects;
using DataAccessObject;
using DataAccessObject.Service;
using DrugPreventionSystem.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DrugPreventionSystem.ViewModels
{
    public class TrainingCoursesViewModel : BaseViewModel
    {
        #region Properties
        private ObservableCollection<TrainingCourse> _courses = new();
        public ObservableCollection<TrainingCourse> Courses
        {
            get => _courses;
            set
            {
                _courses = value;
                OnPropertyChanged();
            }
        }

        private TrainingCourse? _selectedCourse;
        public TrainingCourse? SelectedCourse
        {
            get => _selectedCourse;
            set
            {
                _selectedCourse = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsEditMode));
                OnPropertyChanged(nameof(CanEdit));
                OnPropertyChanged(nameof(CanDelete));

                if (value != null)
                {
                    // Copy selected course to editing course
                    EditingCourse = new TrainingCourse
                    {
                        CourseId = value.CourseId,
                        Title = value.Title,
                        Description = value.Description,
                        TargetAudience = value.TargetAudience
                    };
                }
            }
        }

        private TrainingCourse _editingCourse = new();
        public TrainingCourse EditingCourse
        {
            get => _editingCourse;
            set
            {
                _editingCourse = value;
                OnPropertyChanged();
            }
        }

        private bool _isEditMode = false;
        public bool IsEditMode
        {
            get => _isEditMode;
            set
            {
                _isEditMode = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(FormTitle));
            }
        }

        public string FormTitle => IsEditMode ? "Sửa khóa học" : "Thêm khóa học mới";
        public bool CanEdit => SelectedCourse != null;
        public bool CanDelete => SelectedCourse != null;

        #endregion

        #region Commands
        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        #endregion

        public TrainingCoursesViewModel()
        {
            // Initialize commands
            AddCommand = new RelayCommand(_ => ExecuteAdd());
            EditCommand = new RelayCommand(_ => ExecuteEdit(), _ => CanExecuteEdit());
            DeleteCommand = new RelayCommand(_ => ExecuteDelete(), _ => CanExecuteDelete());
            SaveCommand = new RelayCommand(_ => ExecuteSave(), _ => CanExecuteSave());
            CancelCommand = new RelayCommand(_ => ExecuteCancel());

            LoadCourses();
        }

        #region Methods
        private void LoadCourses()
        {
            try
            {
                using var context = new DrugUsePreventionSupportSystemContext();
                var courses = context.TrainingCourses.OrderBy(c => c.Title).ToList();

                Courses.Clear();
                foreach (var course in courses)
                {
                    Courses.Add(course);
                }

                Debug.WriteLine($"Loaded {courses.Count} training courses");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading courses: {ex.Message}");
                MessageBox.Show($"Lỗi khi tải danh sách khóa học: {ex.Message}", "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteAdd()
        {
            IsEditMode = false;
            EditingCourse = new TrainingCourse();
        }

        private void ExecuteEdit()
        {
            if (SelectedCourse == null) return;
            IsEditMode = true;
        }

        private bool CanExecuteEdit()
        {
            return SelectedCourse != null;
        }

        private void ExecuteDelete()
        {
            if (SelectedCourse == null) return;

            var result = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa khóa học '{SelectedCourse.Title}'?",
                "Xác nhận xóa",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    using var context = new DrugUsePreventionSupportSystemContext();
                    var courseToDelete = context.TrainingCourses.Find(SelectedCourse.CourseId);

                    if (courseToDelete != null)
                    {
                        context.TrainingCourses.Remove(courseToDelete);
                        context.SaveChanges();

                        Courses.Remove(SelectedCourse);
                        SelectedCourse = null;

                        MessageBox.Show("Xóa khóa học thành công!", "Thông báo",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error deleting course: {ex.Message}");
                    MessageBox.Show($"Lỗi khi xóa khóa học: {ex.Message}", "Lỗi",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private bool CanExecuteDelete()
        {
            return SelectedCourse != null;
        }

        private void ExecuteSave()
        {
            if (!ValidateCourse()) return;

            try
            {
                using var context = new DrugUsePreventionSupportSystemContext();

                if (IsEditMode)
                {
                    // Update existing course
                    var existingCourse = context.TrainingCourses.Find(EditingCourse.CourseId);
                    if (existingCourse != null)
                    {
                        existingCourse.Title = EditingCourse.Title;
                        existingCourse.Description = EditingCourse.Description;
                        existingCourse.TargetAudience = EditingCourse.TargetAudience;

                        context.SaveChanges();

                        // Update in ObservableCollection
                        var courseInList = Courses.FirstOrDefault(c => c.CourseId == EditingCourse.CourseId);
                        if (courseInList != null)
                        {
                            courseInList.Title = EditingCourse.Title;
                            courseInList.Description = EditingCourse.Description;
                            courseInList.TargetAudience = EditingCourse.TargetAudience;
                        }

                        MessageBox.Show("Cập nhật khóa học thành công!", "Thông báo",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    // Add new course
                    var newCourse = new TrainingCourse
                    {
                        Title = EditingCourse.Title,
                        Description = EditingCourse.Description,
                        TargetAudience = EditingCourse.TargetAudience,
                        CreatedAt = DateTime.Now,
                        CreatedBy = 1 // Replace with actual user ID
                    };

                    context.TrainingCourses.Add(newCourse);
                    context.SaveChanges();

                    // Add to ObservableCollection
                    Courses.Add(newCourse);

                    MessageBox.Show("Thêm khóa học thành công!", "Thông báo",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }

                // Reset form
                ExecuteCancel();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error saving course: {ex.Message}");
                MessageBox.Show($"Lỗi khi lưu khóa học: {ex.Message}", "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CanExecuteSave()
        {
            return !string.IsNullOrWhiteSpace(EditingCourse?.Title);
        }

        private void ExecuteCancel()
        {
            EditingCourse = new TrainingCourse();
            IsEditMode = false;
            SelectedCourse = null;
        }

        private bool ValidateCourse()
        {
            if (string.IsNullOrWhiteSpace(EditingCourse.Title))
            {
                MessageBox.Show("Vui lòng nhập tiêu đề khóa học!", "Thông báo",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(EditingCourse.Description))
            {
                MessageBox.Show("Vui lòng nhập mô tả khóa học!", "Thông báo",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (EditingCourse.Title.Length > 200)
            {
                MessageBox.Show("Tiêu đề không được vượt quá 200 ký tự!", "Thông báo",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }
        #endregion
    }
}
