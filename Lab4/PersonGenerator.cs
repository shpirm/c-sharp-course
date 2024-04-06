using System;
using System.Collections.ObjectModel;
using WpfApp.Model;

namespace WpfApp
{
    public class PersonGenerator
    {
        private static readonly Random random = new Random();

        public static ObservableCollection<Person> GeneratePersons(int count)
        {
            var persons = new ObservableCollection<Person>();
            for (int i = 0; i < count; i++)
            {
                persons.Add(CreateRandomPerson());
            }
            return persons;
        }

        private static Person CreateRandomPerson()
        {
            string[] firstNames = { "John", "Jane", "Michael", "Emily", "William", "Emma", "David", "Olivia", "James", "Sophia" };
            string[] lastNames = { "Smith", "Johnson", "Williams", "Jones", "Brown", "Davis", "Miller", "Wilson", "Moore", "Taylor" };
            string[] emails = { "@gmail.com", "@yahoo.com", "@outlook.com", "@hotmail.com", "@mail.com" };

            string firstName = firstNames[random.Next(firstNames.Length)];
            string lastName = lastNames[random.Next(lastNames.Length)];
            string email = $"{firstName}.{lastName}{emails[random.Next(emails.Length)]}";
            DateTime birthDate = RandomBirthDate();

            return new Person(firstName, lastName, email, birthDate);
        }

        private static DateTime RandomBirthDate()
        {
            DateTime start = new DateTime(1950, 1, 1);
            int range = (DateTime.Today - start).Days;
            return start.AddDays(random.Next(range));
        }
    }
}
