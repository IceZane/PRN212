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
namespace DrugPreventionSystem
{
    public partial class MainWindow : Window
    {
        private DrugUsePreventionSupportSystemContext _context;

        private string currentRole;
        private string currentUsername;

        public MainWindow(string role, string username)
        {
            InitializeComponent();

            currentRole = role;
            currentUsername = username;

            lblWelcome.Content = $"Chào mừng, {username} ({role})!";
            LoadMenuByRole(currentRole); // Gọi hàm để hiển thị menu theo vai trò
        }



        private User _currentUser;

        public MainWindow(User user)
        {
            InitializeComponent();
            _currentUser = user;

            lblWelcome.Content = $"Chào {_currentUser.FullName} ({_currentUser.Role.RoleName})";

            LoadMenuByRole(_currentUser.Role.RoleName);
        }

        private void LoadMenuByRole(string role)
        {
            mainMenu.Items.Clear();

            // Mục chung cho tất cả
            mainMenu.Items.Add(new MenuItem { Header = "Trang chủ" });

            switch (role)
            {
                case "Member":
                    mainMenu.Items.Add(new MenuItem { Header = "Khóa học của tôi" });
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

            // Thêm nút Đăng xuất
            mainMenu.Items.Add(new MenuItem
            {
                Header = "Đăng xuất",
                Foreground = Brushes.Red
            });
        }


        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtPlaceholder.Visibility = string.IsNullOrEmpty(txtSearch.Text)
                ? Visibility.Visible
                : Visibility.Collapsed;
        }


    }
}