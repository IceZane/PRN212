using BusinessObjects;
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
    /// Interaction logic for UserManagementView.xaml
    /// </summary>
    public partial class UserManagementView : UserControl
    {
        public UserManagementViewModel ViewModel { get; set; }

        public UserManagementView()
        {
            InitializeComponent();
            ViewModel = new UserManagementViewModel();
            this.DataContext = ViewModel;
        }

      //private void UserList_SelectionChanged(object sender, SelectionChangedEventArgs e)
      //  {
      //      if (e.AddedItems.Count > 0 && e.AddedItems[0] is User selectedUser)
      //      {
      //          ViewModel.OnUserSelected(selectedUser);
      //      }
      //  }
    }
}
