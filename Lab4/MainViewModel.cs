using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WpfApp.Exceptions;
using WpfApp.Model;

namespace WpfApp.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Person> _users = new ObservableCollection<Person>();
        private ObservableCollection<Person> _filteredUsers = new ObservableCollection<Person>();
        private string _filterString;
        private Person _selectedUser;
        private bool _isUIEnabled = true;

        public MainViewModel()
        {
            LoadData();
            //Users = PersonGenerator.GeneratePersons(50);

            AddUserCommand = new RelayCommand(_ => EditUser(null));
            DeleteUserCommand = new RelayCommand(DeleteUser);
            SaveDataCommand = new RelayCommand(_ => SaveData());
            EditUserCommand = new RelayCommand(EditUser);
        }

        public Person SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                OnPropertyChanged();
            }
        }

        public string FilterString
        {
            get => _filterString;
            set
            {
                _filterString = value;
                OnPropertyChanged();
                SortAndFilter();
            }
        }

        public bool IsUIEnabled
        {
            get => _isUIEnabled;
            set
            {
                _isUIEnabled = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Person> Users
        {
            get => _users;
            set
            {
                _users = value;
                OnPropertyChanged();
                SortAndFilter();
            }
        }

        public ObservableCollection<Person> FilteredUsers
        {
            get => _filteredUsers;
            set
            {
                _filteredUsers = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddUserCommand { get; }
        public ICommand DeleteUserCommand { get; }
        public ICommand SaveDataCommand { get; }
        public ICommand EditUserCommand { get; }

        public async Task AddUserAsync(Person newUser)
        {
            IsUIEnabled = false;
            try
            {
                await Task.Run(() => AddUser(newUser));
            }
            finally
            {
                IsUIEnabled = true;
            }
        }

        public async Task UpdateUserAsync(Person originalUser, Person editedUser)
        {
            IsUIEnabled = false;
            try
            {
                await Task.Run(() => UpdateUser(originalUser, editedUser));
            }
            finally
            {
                IsUIEnabled = true;
            }
        }

        public void SortAndFilter(string sortBy = "FirstName", bool ascending = true)
        {
            var query = Users.AsEnumerable();
            if (!string.IsNullOrEmpty(FilterString))
            {
                query = query.Where(user => user.FirstName.ToLower().Contains(FilterString.ToLower()) ||
                            user.LastName.ToLower().Contains(FilterString.ToLower()) ||
                            user.Email.ToLower().Contains(FilterString.ToLower()));

            }

            query = ascending ? query.OrderBy(user => user.GetType().GetProperty(sortBy)?.GetValue(user))
                              : query.OrderByDescending(user => user.GetType().GetProperty(sortBy)?.GetValue(user));

            FilteredUsers = new ObservableCollection<Person>(query);
        }

        private void AddUser(Person newUser)
        {
            Users.Add(newUser);
            SortAndFilter();
        }

        private void UpdateUser(Person originalUser, Person editedUser)
        {
            originalUser.Update(editedUser);
            SortAndFilter();
        }

        private void DeleteUser(object parameter)
        {
            if (parameter is Person userToDelete)
            {
                Users.Remove(userToDelete);
                SortAndFilter();
            }
        }

        private void EditUser(object parameter)
        {
            var userViewModel = SelectedUser != null ? new UserViewModel(this, SelectedUser) : new UserViewModel(this);
            var editWindow = new UserWindow
            {
                DataContext = userViewModel
            };
            editWindow.ShowDialog();
            editWindow.Close();
        }

        public void SaveData()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(Users, options);
            File.WriteAllText("users.json", json);
            MessageBox.Show("Data saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void LoadData()
        {
            if (File.Exists("users.json"))
            {
                string json = File.ReadAllText("users.json");
                Users = JsonSerializer.Deserialize<ObservableCollection<Person>>(json);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
