using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WpfApp.Exceptions;
using WpfApp.Model;

namespace WpfApp.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private Person _person;

        private string _firstName = "";
        private string _lastName = "";
        private string _email = "";
        private DateTime _birthdate = DateTime.MinValue;

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
            get => _firstName;
            set
            {
                _firstName = value;
                OnPropertyChanged();
            }
        }

        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                OnPropertyChanged();
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        public DateTime BirthDate
        {
            get => _birthdate;
            set
            {
                _birthdate = value;
                OnPropertyChanged();
            }
        }

        public bool IsValid => !string.IsNullOrEmpty(_firstName)
                            && !string.IsNullOrEmpty(_lastName)
                            && !string.IsNullOrEmpty(_email)
                            && _birthdate != DateTime.MinValue;

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
                try
                {
                    _person = new Person(FirstName, LastName, Email, BirthDate);

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
                }
                catch (FutureBirthDateException ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (DistantPastBirthDateException ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (InvalidEmailException ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
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
