using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DataAccessObject;
using BusinessObjects;
using System.Data;
using System.Windows.Media.Animation;
namespace DrugPreventionSystem
{
    public partial class MainWindow : Window
    {
        private DrugUsePreventionSupportSystemContext _context;

        private string currentRole;
        private string currentUsername;
        private TrainingCourseDAO _courseDAO;
        private User _currentUser;

        public MainWindow()
        {
            InitializeComponent();
        }


        public MainWindow(string role, string username)
        {
            InitializeComponent();

            _context = new DrugUsePreventionSupportSystemContext();
            _courseDAO = new TrainingCourseDAO(); // Bổ sung nếu chưa có
            _currentUser = _context.Users.FirstOrDefault(u => u.Email == username);

            if (_currentUser == null)
            {
                MessageBox.Show("Không tìm thấy người dùng!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            lblWelcome.Content = _currentUser.Role != null
                ? $"Chào {_currentUser.FullName} ({_currentUser.Role.RoleName})"
                : $"Chào {_currentUser.FullName}";

            LoadMenuByRole(_currentUser.Role?.RoleName ?? "Member");
            this.Loaded += MainWindow_Loaded;
        }




        
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadCourses();
        }

        public MainWindow(User user)
        {
            InitializeComponent(); // Luôn phải gọi trước

            _context = new DrugUsePreventionSupportSystemContext();
            _courseDAO = new TrainingCourseDAO();
            _currentUser = user;

            lblWelcome.Content = _currentUser.Role != null
                ? $"Chào {_currentUser.FullName} ({_currentUser.Role.RoleName})"
                : $"Chào {_currentUser.FullName}";

            LoadMenuByRole(_currentUser.Role?.RoleName ?? "Member");
            this.Loaded += MainWindow_Loaded;
        }


        private void LoadMenuByRole(string role)
        {
            mainMenu.Items.Clear();

            // Mục chung cho tất cả
            MenuItem homeItem = new MenuItem { Header = "Trang chủ", Name = "menuHome" };
            homeItem.Click += HomeItem_Click;
            mainMenu.Items.Add(homeItem);

            switch (role)
            {
                case "Member":
                    var myCoursesMenu = new MenuItem { Header = "Khóa học của tôi" };
                    myCoursesMenu.Click += MyCourses_Click;
                    mainMenu.Items.Add(myCoursesMenu);

                    mainMenu.Items.Add(new MenuItem { Header = "Thông tin cá nhân" });
                    break;

                case "Staff":
                    mainMenu.Items.Add(new MenuItem { Header = "Quản lý khóa học" });
                    mainMenu.Items.Add(new MenuItem { Header = "Xem học viên" });
                    break;

                case "Manager":
                    mainMenu.Items.Add(new MenuItem { Header = "Thống kê hệ thống" });
                    mainMenu.Items.Add(new MenuItem { Header = "Phân quyền" });
                    break;

                case "Consultant":
                    mainMenu.Items.Add(new MenuItem { Header = "Tư vấn học viên" });
                    mainMenu.Items.Add(new MenuItem { Header = "Lịch hẹn" });
                    break;

                case "Admin":
                    mainMenu.Items.Add(new MenuItem { Header = "Quản trị hệ thống" });
                    mainMenu.Items.Add(new MenuItem { Header = "Quản lý người dùng" });
                    break;
            }
        }


        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtPlaceholder.Visibility = string.IsNullOrEmpty(txtSearch.Text)
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        private void cboAgeGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selected = (cboAgeGroup.SelectedItem as ComboBoxItem)?.Content.ToString();

            if (selected == "Tất cả")
            {
                LoadCourses();
            }
            else
            {
                var filtered = _courseDAO.FilterByAudience(selected);
                lstCourses.ItemsSource = filtered;
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string keyword = txtSearch.Text.Trim();
            var result = _courseDAO.SearchCourses(keyword);
            lstCourses.ItemsSource = result;
        }

        private void lstCourses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstCourses.SelectedItem is TrainingCourse selectedCourse)
            {
                CourseDetailWindow detailWindow = new CourseDetailWindow(selectedCourse, _currentUser); // truyền user
                detailWindow.ShowDialog();
                lstCourses.SelectedItem = null;
            }
        }

        private void HomeItem_Click(object sender, RoutedEventArgs e)
        {
            // Nếu user là Member thì quay về MemberHomePage
            if (_currentUser != null && _currentUser.Role?.RoleName.Equals("Member", StringComparison.OrdinalIgnoreCase) == true)
            {
                MemberHomePage memberHome = new MemberHomePage(_currentUser);
                memberHome.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Chức năng Trang chủ chưa hỗ trợ cho vai trò này.", "Thông báo");
            }
        }
        private void LoadCourses()
        {
            if (_courseDAO == null)
            {
                MessageBox.Show("_courseDAO chưa được khởi tạo!", "Lỗi nghiêm trọng", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var courses = _courseDAO.GetAllCourses();
            lstCourses.ItemsSource = courses;
        }


        private void MyCourses_Click(object sender, RoutedEventArgs e)
        {
            // Lấy danh sách các khóa học đã đăng ký
            var myCourses = _context.UserCourses
                .Where(uc => uc.UserId == _currentUser.UserId)
                .Select(uc => uc.Course)
                .ToList();

            if (myCourses.Count == 0)
            {
                MessageBox.Show("Bạn chưa đăng ký khóa học nào.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // Hiển thị danh sách trong lstCourses
            lstCourses.ItemsSource = myCourses;
            lblWelcome.Content = $"Khóa học của bạn, {_currentUser.FullName}";
        }


    }
}