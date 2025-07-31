using BusinessObjects;
using DataAccessObject.Service;
using DrugPreventionSystem.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BusinessObjects;
using DataAccessObject;
using System.Diagnostics;

namespace DrugPreventionSystem.ViewModels
{
    public class UserManagementViewModel : BaseViewModel
    {
        private User? _selectedUser;

        public ObservableCollection<User> Users { get; set; } = new ObservableCollection<User>();

        public User? SelectedUser
        {
            get => _selectedUser;
            set
            {
                Debug.WriteLine($"SelectedUser changed to: {value?.FullName ?? "NULL"}");
                _selectedUser = value;
                OnPropertyChanged();

                if (value != null)
                {
                    LoadUserDetails(value.UserId);
                }
                else
                {
                    ClearDetails();
                }
            }
        }

        public ObservableCollection<UserSurveyResult> SelectedUserSurveyResults { get; set; } = new ObservableCollection<UserSurveyResult>();
        public ObservableCollection<UserCourse> SelectedUserCourses { get; set; } = new ObservableCollection<UserCourse>();
        public ObservableCollection<UserProgramParticipation> SelectedUserParticipations { get; set; } = new ObservableCollection<UserProgramParticipation>();

        public UserManagementViewModel()
        {
            LoadUsers();
        }

        private void LoadUsers()
        {
            try
            {
                using var context = new DrugUsePreventionSupportSystemContext();
                var users = context.Users.OrderBy(u => u.FullName).ToList();

                Debug.WriteLine($"Loaded {users.Count} users from database");

                Users.Clear();
                foreach (var user in users)
                {
                    Users.Add(user);
                    Debug.WriteLine($"User: {user.FullName} (ID: {user.UserId})");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading users: {ex.Message}");
            }
        }

        private void LoadUserDetails(int userId)
        {
            try
            {
                using var context = new DrugUsePreventionSupportSystemContext();

                Debug.WriteLine($"Loading details for user ID: {userId}");

                // Load Survey Results với debug
                var surveyResults = context.UserSurveyResults
                    .Include(usr => usr.Survey)
                    .Where(usr => usr.UserId == userId)
                    .OrderByDescending(usr => usr.TakenAt)
                    .ToList();

                Debug.WriteLine($"Found {surveyResults.Count} survey results");

                SelectedUserSurveyResults.Clear();
                foreach (var result in surveyResults)
                {
                    SelectedUserSurveyResults.Add(result);
                    Debug.WriteLine($"Survey: {result.Survey?.SurveyName ?? "NULL"}, Score: {result.TotalScore}, Date: {result.TakenAt}");
                }

                // Load Courses với debug
                var userCourses = context.UserCourses
                    .Include(uc => uc.Course)
                    .Where(uc => uc.UserId == userId)
                    .OrderByDescending(uc => uc.RegisteredAt)
                    .ToList();

                Debug.WriteLine($"Found {userCourses.Count} user courses");

                SelectedUserCourses.Clear();
                foreach (var course in userCourses)
                {
                    SelectedUserCourses.Add(course);
                    Debug.WriteLine($"Course: {course.Course?.Title ?? "NULL"}, Progress: {course.ProgressPercent}%, Date: {course.RegisteredAt}");
                }

                // Load Program Participations với debug
                var participations = context.UserProgramParticipations
                    .Include(upp => upp.Program)
                    .Where(upp => upp.UserId == userId)
                    .OrderByDescending(upp => upp.JoinedAt)
                    .ToList();

                Debug.WriteLine($"Found {participations.Count} program participations");

                SelectedUserParticipations.Clear();
                foreach (var participation in participations)
                {
                    SelectedUserParticipations.Add(participation);
                    Debug.WriteLine($"Program: {participation.Program?.ProgramTitle ?? "NULL"}, Date: {participation.JoinedAt}");
                }

                Debug.WriteLine($"Completed loading details for user {userId}");

                // Notify UI that collections have changed
                OnPropertyChanged(nameof(SelectedUserSurveyResults));
                OnPropertyChanged(nameof(SelectedUserCourses));
                OnPropertyChanged(nameof(SelectedUserParticipations));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading user details: {ex.Message}");
                Debug.WriteLine($"Stack trace: {ex.StackTrace}");
            }
        }

        private void ClearDetails()
        {
            Debug.WriteLine("Clearing user details");
            SelectedUserSurveyResults.Clear();
            SelectedUserCourses.Clear();
            SelectedUserParticipations.Clear();
        }
    }
}
