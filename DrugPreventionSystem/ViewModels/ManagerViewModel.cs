using DrugPreventionSystem.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Controls;
using DrugPreventionSystem.Views;



namespace DrugPreventionSystem.ViewModels
{
    public class ManagerViewModel : BaseViewModel
    {
        public ICommand ShowTrainingCommand { get; }
        public ICommand ShowCommunityCommand { get; }
        public ICommand ShowSurveyCommand { get; }
        public ICommand ShowUsersCommand { get; }
        public ICommand ShowDashboardCommand { get; }

        private UserControl _currentView;
        public UserControl CurrentView
        {
            get => _currentView;
            set { _currentView = value; OnPropertyChanged(); }
        }

        public ManagerViewModel()
        {
            ShowTrainingCommand = new RelayCommand(_ => ShowTraining());
            ShowCommunityCommand = new RelayCommand(_ => ShowCommunity());
            ShowSurveyCommand = new RelayCommand(_ => ShowSurvey());
            ShowUsersCommand = new RelayCommand(_ => ShowUsers());
            ShowDashboardCommand = new RelayCommand(_ => ShowDashboard());

            ShowTraining(); // Mặc định
        }

        private void ShowTraining() =>
            CurrentView = new Views.TrainingCoursesView { DataContext = new TrainingCoursesViewModel() };

        private void ShowCommunity() =>
            CurrentView = new Views.CommunityProgramsView { DataContext = new CommunityProgramsViewModel() };

        private void ShowSurvey() =>
            CurrentView = new Views.SurveyManagementView { DataContext = new SurveyManagementViewModel() };

        private void ShowUsers() =>
            CurrentView = new Views.UserManagementView { DataContext = new UserManagementViewModel() };

        private void ShowDashboard() =>
            CurrentView = new Views.DashboardView { DataContext = new DashboardViewModel() };
    }

}
