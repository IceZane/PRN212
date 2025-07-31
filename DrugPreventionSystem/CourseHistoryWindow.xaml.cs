using BusinessObjects;
using DataAccessObject;
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

namespace DrugPreventionSystem
{
    /// <summary>
    /// Interaction logic for CourseHistoryWindow.xaml
    /// </summary>
    public partial class CourseHistoryWindow : Window
    {
        private User _currentUser;
        private UserCourseDAO _ucd;
        public CourseHistoryWindow(User user)
        {           
            InitializeComponent();
            _ucd = new UserCourseDAO();
            _currentUser = user;
            LoadCourseHistory();
        }
        public void LoadCourseHistory()
        {
            dg_UserCourses.ItemsSource = _ucd.GetUserCoursesByUserId(_currentUser);
        }
    }
}
