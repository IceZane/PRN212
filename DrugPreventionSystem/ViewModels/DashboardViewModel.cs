using Microsoft.EntityFrameworkCore;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Linq;
using System.Text.Json;
using DrugPreventionSystem.Helpers;
using BusinessObjects;
using DataAccessObject;
using System.Windows;

namespace DrugPreventionSystem.ViewModels
{
    public class DashboardViewModel : BaseViewModel
    {
        private int _totalUsers;
        public int TotalUsers
        {
            get => _totalUsers;
            set { _totalUsers = value; OnPropertyChanged(); }
        }

        private int _highRiskUsers;
        public int HighRiskUsers
        {
            get => _highRiskUsers;
            set { _highRiskUsers = value; OnPropertyChanged(); }
        }

        private int _totalCourses;
        public int TotalCourses
        {
            get => _totalCourses;
            set { _totalCourses = value; OnPropertyChanged(); }
        }

        public DashboardViewModel()
        {
            LoadStatistics();
        }

        private void LoadStatistics()
        {
            using (var context = new DrugUsePreventionSupportSystemContext())
            {
                TotalUsers = context.Users.Count();
                HighRiskUsers = context.UserSurveyResults.Count(r => r.TotalScore >= 8);
                TotalCourses = context.TrainingCourses.Count();
            }
        }
    
}
}
