using System.Configuration;
using System.Data;
using System.Windows;

namespace DrugPreventionSystem
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnExit(ExitEventArgs e)
        {
            MessageBox.Show("Ứng dụng đang thoát!", "DEBUG", MessageBoxButton.OK);
            base.OnExit(e);
        }
    }

}
