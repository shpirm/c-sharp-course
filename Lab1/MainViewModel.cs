using System;
using System.ComponentModel;
using System.Windows;
using WpfApp.Model;

namespace WpfApp.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private Person person;
        public Person Person
        {
            get { return person; }
            set
            {
                person = value;
                OnPropertyChanged("Person");
                CalculateAge(); 
            }
        }

        private string birthdayMessage;
        public string BirthdayMessage
        {
            get { return birthdayMessage; }
            set
            {
                birthdayMessage = value;
                OnPropertyChanged("BirthdayMessage");
            }
        }

        public MainViewModel()
        {
            Person = new Person
            {
                // Дефолтна дата - 1 січня 2000 року
                BirthDate = new DateTime(2000, 1, 1) 
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void CalculateAge()
        {
            if (Person.BirthDate != null)
            {
                // Прорахунок зодіаку
                Person.WesternZodiac = GetWesternZodiac(Person.BirthDate);
                Person.ChineseZodiac = GetChineseZodiac(Person.BirthDate.Year);

                var today = DateTime.Today;
                var age = today.Year - Person.BirthDate.Year;

                // Перерахунок віку, якщо цього року дата народження ще не настала
                if (Person.BirthDate > today.AddYears(-age))
                {
                    age--;
                }

                // Перевірка на валідність віку
                if (age < 0)
                {
                    MessageBox.Show("You haven't been born yet!", "Invalid Age", MessageBoxButton.OK, MessageBoxImage.Error);
                    Person.Age = 0;
                }
                else if (age > 135)
                {
                    MessageBox.Show("Invalid age! Age cannot be more than 135 years.", "Invalid Age", MessageBoxButton.OK, MessageBoxImage.Error);
                    Person.Age = 0;
                }
                else
                {
                    Person.Age = age;
                }

                if (Person.BirthDate.Month == DateTime.Today.Month && Person.BirthDate.Day == DateTime.Today.Day)
                {
                    BirthdayMessage = "Happy Birthday!";
                }
                else
                {
                    BirthdayMessage = "";
                }

                OnPropertyChanged("Person");
            }
        }

        private string GetWesternZodiac(DateTime birthDate)
        {
            int[] zodiacStartDates = { 20, 19, 21, 20, 21, 21, 23, 23, 23, 23, 22, 22 };
            string[] zodiacSigns = { "Aquarius", "Pisces", "Aries", "Taurus", "Gemini", "Cancer", "Leo", "Virgo", "Libra", "Scorpio", "Sagittarius", "Capricorn" };

            int index = birthDate.Month - 1;
            if (birthDate.Day < zodiacStartDates[index])
            {
                index = (index + 11) % 12;
            }

            return zodiacSigns[index];
        }

        private string GetChineseZodiac(int year)
        {
            string[] zodiacSigns = { "Rat", "Ox", "Tiger", "Rabbit", "Dragon", "Snake", "Horse", "Goat", "Monkey", "Rooster", "Dog", "Pig" };
            return zodiacSigns[(year - 4) % 12];
        }

        public void ClearData()
        {
            Person = new Person
            {
                BirthDate = new DateTime(2000, 1, 1)
            };
            BirthdayMessage = "";
        }

    }
}
