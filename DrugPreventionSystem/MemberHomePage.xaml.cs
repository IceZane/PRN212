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
using BusinessObjects;

namespace DrugPreventionSystem
{
    /// <summary>
    /// Interaction logic for MemberHomePage.xaml
    /// </summary>
    public partial class MemberHomePage : Window
    {
        private User _loginUser;

        public MemberHomePage(User loginUser)  // Nhận đối tượng User từ LoginWindow
        {
            InitializeComponent();
            _loginUser = loginUser; // Gán giá trị
            lblWelcome.Text = $"Chào mừng, {_loginUser.FullName}!";
        }

        private void btnCourses_Click(object sender, RoutedEventArgs e)
        {
            // Truyền user qua MainWindow
            MainWindow mainWindow = new MainWindow(_loginUser); // truyền user đầy đủ thay vì role
            mainWindow.Show();
            this.Close();
        }

        private void btnHistory_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Chức năng lịch sử chưa được triển khai.");
            // Hoặc mở một cửa sổ mới nếu đã có.
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow login = new LoginWindow();
            login.Show();
            this.Close();
        }
    }
}