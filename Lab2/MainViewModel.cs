using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WpfApp.Model;

namespace WpfApp.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private Person _person;
        private bool _isUIEnabled = true;

        public MainViewModel()
        {
            _person = new Person();
            ProceedCommand = new RelayCommand(async () => await ProceedAsync());
            PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName != nameof(IsValid))
                {
                    OnPropertyChanged(nameof(IsValid));
                }
            };
        }

        public string FirstName
        {
            get => _person.FirstName;
            set
            {
                _person.FirstName = value;
                OnPropertyChanged();
            }
        }

        public string LastName
        {
            get => _person.LastName;
            set
            {
                _person.LastName = value;
                OnPropertyChanged();
            }
        }

        public string Email
        {
            get => _person.Email;
            set
            {
                _person.Email = value;
                OnPropertyChanged();
            }
        }

        public DateTime BirthDate
        {
            get => _person.BirthDate;
            set
            {
                _person.BirthDate = value;
                OnPropertyChanged();
            }
        }

        public bool IsValid => !string.IsNullOrEmpty(_person.FirstName)
                            && !string.IsNullOrEmpty(_person.LastName)
                            && !string.IsNullOrEmpty(_person.Email)
                            && BirthDate != DateTime.MinValue;

        public bool IsUIEnabled
        {
            get => _isUIEnabled;
            set
            {
                _isUIEnabled = value;
                OnPropertyChanged();
            }
        }

        public ICommand ProceedCommand { get; }

        private async Task ProceedAsync()
        {
            IsUIEnabled = false;

            await Task.Run(() =>
            {
                if (!_person.IsAgeValid)
                {
                    MessageBox.Show("Invalid age. The age should be between 0 and 135 years.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                string info = $"First Name: {_person.FirstName}\n" +
                              $"Last Name: {_person.LastName}\n" +
                              $"Email: {_person.Email}\n" +
                              $"Birth Date: {_person.BirthDate.ToShortDateString()}\n" +
                              $"Age: {_person.Age}\n" +
                              $"Western Zodiac: {_person.WesternZodiac}\n" +
                              $"Chinese Zodiac: {_person.ChineseZodiac}\n" +
                              $"Is Adult: {_person.IsAdult}\n" +
                              $"Is Birthday: {_person.IsBirthday}";

                MessageBox.Show(info, "User Information", MessageBoxButton.OK, MessageBoxImage.Information);
            });

            IsUIEnabled = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
