using System;

namespace PostgreSQLProject
{
    internal class User
    {
        public User() { }

        public User(string name, string number, DateTime date)
        {
            Name = name;
            Number = number;
            Date = date;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
        public DateTime Date { get; set; }
    }
}
