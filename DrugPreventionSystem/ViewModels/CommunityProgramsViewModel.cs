using BusinessObjects;
using DataAccessObject.Service;
using DrugPreventionSystem.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace DrugPreventionSystem.ViewModels
{
    public class CommunityProgramsViewModel : BaseViewModel
    {
        #region Private Fields
        private readonly CommunityProgramService _service;
        private ObservableCollection<CommunityProgram> _programs;
        private CommunityProgram? _selectedProgram;
        private CommunityProgram _editingProgram;
        private bool _isEditMode;
        #endregion

        #region Properties
        public ObservableCollection<CommunityProgram> Programs
        {
            get => _programs;
            set
            {
                _programs = value;
                OnPropertyChanged();
            }
        }

        public CommunityProgram? SelectedProgram
        {
            get => _selectedProgram;
            set
            {
                _selectedProgram = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanEdit));
                OnPropertyChanged(nameof(CanDelete));

                if (value != null)
                {
                    EditingProgram = new CommunityProgram
                    {
                        ProgramId = value.ProgramId,
                        ProgramTitle = value.ProgramTitle,
                        Description = value.Description,
                        StartDate = value.StartDate,
                        EndDate = value.EndDate
                    };

                    OnPropertyChanged(nameof(StartDateTime));
                    OnPropertyChanged(nameof(EndDateTime));
                }
            }
        }

        public CommunityProgram EditingProgram
        {
            get => _editingProgram;
            set
            {
                _editingProgram = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(StartDateTime));
                OnPropertyChanged(nameof(EndDateTime));
            }
        }

        public bool IsEditMode
        {
            get => _isEditMode;
            set
            {
                _isEditMode = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(FormTitle));
            }
        }

        public DateTime? StartDateTime
        {
            get => EditingProgram?.StartDate?.ToDateTime(TimeOnly.MinValue);
            set
            {
                if (EditingProgram != null)
                {
                    EditingProgram.StartDate = value.HasValue ? DateOnly.FromDateTime(value.Value) : null;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime? EndDateTime
        {
            get => EditingProgram?.EndDate?.ToDateTime(TimeOnly.MinValue);
            set
            {
                if (EditingProgram != null)
                {
                    EditingProgram.EndDate = value.HasValue ? DateOnly.FromDateTime(value.Value) : null;
                    OnPropertyChanged();
                }
            }
        }

        public string FormTitle => IsEditMode ? "Sửa chương trình" : "Thêm chương trình mới";
        public bool CanEdit => SelectedProgram != null;
        public bool CanDelete => SelectedProgram != null;
        #endregion

        #region Commands
        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        #endregion

        #region Constructor
        public CommunityProgramsViewModel()
        {
            _service = new CommunityProgramService();
            _programs = new ObservableCollection<CommunityProgram>();
            _editingProgram = new CommunityProgram();

            AddCommand = new RelayCommand(_ => ExecuteAdd());
            EditCommand = new RelayCommand(_ => ExecuteEdit(), _ => CanEdit);
            DeleteCommand = new RelayCommand(_ => ExecuteDelete(), _ => CanDelete);
            SaveCommand = new RelayCommand(_ => ExecuteSave(), _ => CanSave());
            CancelCommand = new RelayCommand(_ => ExecuteCancel());

            LoadPrograms();
        }
        #endregion

        #region Methods
        private void LoadPrograms()
        {
            try
            {
                var programs = _service.GetAllPrograms();
                Programs.Clear();
                foreach (var program in programs)
                {
                    Programs.Add(program);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách chương trình: {ex.Message}", "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteAdd()
        {
            IsEditMode = false;
            EditingProgram = new CommunityProgram();
        }

        private void ExecuteEdit()
        {
            if (SelectedProgram == null) return;
            IsEditMode = true;
        }

        private void ExecuteDelete()
        {
            if (SelectedProgram == null) return;

            var result = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa chương trình '{SelectedProgram.ProgramTitle}'?",
                "Xác nhận xóa",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                bool success = _service.DeleteProgram(SelectedProgram.ProgramId);
                if (success)
                {
                    Programs.Remove(SelectedProgram);
                    SelectedProgram = null;
                    MessageBox.Show("Xóa chương trình thành công!", "Thông báo");
                }
                else
                {
                    MessageBox.Show("Không thể xóa chương trình!", "Lỗi");
                }
            }
        }

        private void ExecuteSave()
        {
            if (!ValidateProgram()) return;

            bool success = false;

            if (IsEditMode)
            {
                success = _service.UpdateProgram(EditingProgram);
                if (success)
                {
                    var programInList = Programs.FirstOrDefault(p => p.ProgramId == EditingProgram.ProgramId);
                    if (programInList != null)
                    {
                        programInList.ProgramTitle = EditingProgram.ProgramTitle;
                        programInList.Description = EditingProgram.Description;
                        programInList.StartDate = EditingProgram.StartDate;
                        programInList.EndDate = EditingProgram.EndDate;
                    }
                    MessageBox.Show("Cập nhật chương trình thành công!", "Thông báo");
                }
            }
            else
            {
                var newProgram = new CommunityProgram
                {
                    ProgramTitle = EditingProgram.ProgramTitle,
                    Description = EditingProgram.Description,
                    StartDate = EditingProgram.StartDate,
                    EndDate = EditingProgram.EndDate,
                    CreatedBy = 1
                };

                success = _service.AddProgram(newProgram);
                if (success)
                {
                    Programs.Add(newProgram);
                    MessageBox.Show("Thêm chương trình thành công!", "Thông báo");
                }
            }

            if (success)
            {
                ExecuteCancel();
            }
            else
            {
                MessageBox.Show("Không thể lưu chương trình!", "Lỗi");
            }
        }

        private void ExecuteCancel()
        {
            EditingProgram = new CommunityProgram();
            IsEditMode = false;
            SelectedProgram = null;
        }

        private bool CanSave()
        {
            return !string.IsNullOrWhiteSpace(EditingProgram?.ProgramTitle);
        }

        private bool ValidateProgram()
        {
            if (string.IsNullOrWhiteSpace(EditingProgram?.ProgramTitle))
            {
                MessageBox.Show("Vui lòng nhập tiêu đề chương trình!", "Thông báo");
                return false;
            }

            if (string.IsNullOrWhiteSpace(EditingProgram.Description))
            {
                MessageBox.Show("Vui lòng nhập mô tả chương trình!", "Thông báo");
                return false;
            }

            if (EditingProgram.StartDate.HasValue && EditingProgram.EndDate.HasValue &&
                EditingProgram.StartDate > EditingProgram.EndDate)
            {
                MessageBox.Show("Ngày bắt đầu không thể sau ngày kết thúc!", "Thông báo");
                return false;
            }

            return true;
        }
        #endregion
    }
}