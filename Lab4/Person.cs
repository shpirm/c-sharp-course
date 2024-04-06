using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using WpfApp.Exceptions;


namespace WpfApp.Model
{
    public class Person
    {
        private const int MaxAge = 135;

        private string _firstName;
        private string _lastName;
        private string _email;
        private DateTime _birthdate;

        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
            }
        }
        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
            }
        }
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
            }
        }
        public DateTime BirthDate
        {
            get => _birthdate;
            set
            {
                _birthdate = value;
            }
        }


        public int Age => CalculateAge();
        public string WesternZodiac => GetWesternZodiac(BirthDate);
        public string ChineseZodiac => GetChineseZodiac(BirthDate.Year);
        public bool IsAdult => Age >= 18;
        public bool IsBirthday => BirthDate.Month == DateTime.Today.Month
                                   && BirthDate.Day == DateTime.Today.Day;
        public bool IsAgeValid => Age >= 0 && Age <= 135;

        public Person()
        {
            // Default values
            _firstName = string.Empty;
            _lastName = string.Empty;
            _email = string.Empty;
            _birthdate = DateTime.MinValue;
        }

        public Person(string firstName, string lastName, string email, DateTime birthDate)
            : this()
        {
            _firstName = firstName;
            _lastName = lastName;
            _email = email;
            _birthdate = birthDate;

            ValidatePerson();
        }

        public Person(string firstName, string lastName, string email)
            : this(firstName, lastName, email, DateTime.MinValue)
        {
        }

        public Person(string firstName, string lastName, DateTime birthDate)
            : this(firstName, lastName, string.Empty, birthDate)
        {
        }

        public Person Clone()
        {
            return new Person(FirstName, LastName, Email, BirthDate);
        }

        public void Update(Person other)
        {
            _firstName = other.FirstName;
            _lastName = other.LastName;
            _email = other.Email;
            _birthdate = other.BirthDate;
        }

        public void ValidatePerson()
        {
            if (BirthDate > DateTime.Now)
            {
                throw new FutureBirthDateException("Birth date cannot be in the future.");
            }

            if (CalculateAge() > MaxAge)
            {
                throw new DistantPastBirthDateException($"Maximum age is {MaxAge} years.");
            }

            if (!IsValidEmail(Email))
            {
                throw new InvalidEmailException("Invalid email address format.");
            }
        }

        private bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        private int CalculateAge()
        {
            var today = DateTime.Today;
            var age = today.Year - BirthDate.Year;
            if (BirthDate.Date > today.AddYears(-age)) age--;
            return age;
        }

        private string GetWesternZodiac(DateTime birthDate)
        {
            int[] zodiacStartDates = { 20, 19, 21, 20, 21, 21, 23, 23, 23, 23, 22, 22 };
            string[] zodiacSigns = {
                "Aquarius", "Pisces", "Aries", "Taurus",
                "Gemini", "Cancer", "Leo", "Virgo",
                "Libra", "Scorpio", "Sagittarius", "Capricorn" };

            int index = birthDate.Month - 1;
            if (birthDate.Day < zodiacStartDates[index])
            {
                index = (index + 11) % 12;
            }

            return zodiacSigns[index];
        }

        private string GetChineseZodiac(int year)
        {
            string[] zodiacSigns = {
                "Rat", "Ox", "Tiger", "Rabbit",
                "Dragon", "Snake", "Horse", "Goat",
                "Monkey", "Rooster", "Dog", "Pig" };

            int index = (year - 4) % 12;
            if (index < 0)
            {
                index += 12;
            }
            return zodiacSigns[index % 12];
        }
    }
}
