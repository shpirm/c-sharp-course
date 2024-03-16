using System;

namespace WpfApp.Model
{
    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }

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
            FirstName = string.Empty;
            LastName = string.Empty;
            Email = string.Empty;
            BirthDate = DateTime.MinValue;
        }

        public Person(string firstName, string lastName, string email, DateTime birthDate)
            : this()
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            BirthDate = birthDate;
        }

        public Person(string firstName, string lastName, string email)
            : this(firstName, lastName, email, DateTime.MinValue)
        {
        }

        public Person(string firstName, string lastName, DateTime birthDate)
            : this(firstName, lastName, string.Empty, birthDate)
        {
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
