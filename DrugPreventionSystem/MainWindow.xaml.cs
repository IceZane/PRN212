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
namespace DrugPreventionSystem
{
    public partial class MainWindow : Window
    {
        private DrugUsePreventionSupportSystemContext _context;

        public MainWindow()
        {
            InitializeComponent();
            LoadUsers();
        }

        private void LoadUsers()
        {
            try
            {
                _context = new DrugUsePreventionSupportSystemContext();
                var users = _context.Users.ToList();
                UserDataGrid.ItemsSource = users;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu người dùng: " + ex.Message);
            }
        }
    }
}