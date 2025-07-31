using System;
using System.Linq;
using System.Windows;
using BusinessObjects;
using DataAccessObject;

namespace DrugPreventionSystem
{
    public partial class CourseDetailWindow : Window
    {
        private readonly TrainingCourse _course;
        private readonly User _currentUser;
        private readonly DrugUsePreventionSupportSystemContext _context;

        public CourseDetailWindow(TrainingCourse course, User user)
        {
            InitializeComponent();
            _course = course;
            _currentUser = user;
            _context = new DrugUsePreventionSupportSystemContext();
            ShowCourseDetails();
        }

        private void ShowCourseDetails()
        {
            txtTitle.Text = _course.Title;
            txtDescription.Text = _course.Description;
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            // Kiểm tra nếu đã đăng ký rồi
            var existing = _context.UserCourses
                .FirstOrDefault(uc => uc.UserId == _currentUser.UserId && uc.CourseId == _course.CourseId);

            if (existing != null)
            {
                MessageBox.Show("Bạn đã đăng ký khóa học này rồi.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // Thêm mới đăng ký
            var userCourse = new UserCourse
            {
                UserId = _currentUser.UserId,
                CourseId = _course.CourseId,
                RegisteredAt = DateTime.Now,
                ProgressPercent = 0
            };

            _context.UserCourses.Add(userCourse);
            _context.SaveChanges();

            MessageBox.Show("Đăng ký khóa học thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void btnUnregister_Click(object sender, RoutedEventArgs e)
        {
            var existing = _context.UserCourses
                            .FirstOrDefault(uc => uc.UserId == _currentUser.UserId && uc.CourseId == _course.CourseId);

            if (existing != null)
            {
                MessageBoxResult result = MessageBox.Show("Bạn có chắc muốn hủy đăng ký khóa học này?",
                                                          "Xác nhận",
                                                          MessageBoxButton.YesNo,
                                                          MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    _context.UserCourses.Remove(existing);
                    _context.SaveChanges();
                    MessageBox.Show("Đã hủy đăng ký khóa học.");
                    this.Close(); // Đóng cửa sổ chi tiết
                }
            }
            else
            {
                MessageBox.Show("Bạn chưa đăng ký khóa học này.");
            }
        }
    }
}
