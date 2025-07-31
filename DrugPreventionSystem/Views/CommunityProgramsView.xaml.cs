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
    /// Interaction logic for CommunityProgramsView.xaml
    /// </summary>
    public partial class CommunityProgramsView : UserControl
    {

        public CommunityProgramsViewModel ViewModel { get; set; }
        public CommunityProgramsView()
        {
            InitializeComponent();
            ViewModel = new CommunityProgramsViewModel();
            this.DataContext = ViewModel;
        }
    }
}
