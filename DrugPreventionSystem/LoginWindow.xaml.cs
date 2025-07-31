using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore; // Bổ sung để dùng Include
using BusinessObjects;
using DataAccessObject;

namespace DrugPreventionSystem
{
    public partial class LoginWindow : Window
    {
        private readonly DrugUsePreventionSupportSystemContext _context;

        public LoginWindow()
        {
            InitializeComponent();
            _context = new DrugUsePreventionSupportSystemContext();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text.Trim().ToLower();
            string password = txtPassword.Password.Trim();

            // Truy vấn và Include Role
            var user = _context.Users
                .Include(u => u.Role)
                .FirstOrDefault(u => u.Email.ToLower() == username && u.PasswordHash == password);

            if (user != null)
            {
                if (user.Role != null && user.Role.RoleName.Equals("Member", StringComparison.OrdinalIgnoreCase))
                {
                    MemberHomePage memberHomePage = new MemberHomePage(user);
                    memberHomePage.Show();
                }

                this.Close();
            }
            else
            {
                MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
