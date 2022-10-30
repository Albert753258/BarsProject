using System.Globalization;
using Newtonsoft.Json;

namespace InformixConnector
{
    public class Human
    {
        public int id;
        public string surname;
        public string fname;
        public string patronymic;
        public string birthday;
[JsonConstructor]
        public Human(int id, string surname, string fname, string patronymic, string birthday)
        {
            this.id = id;
            this.surname = surname.Trim();
            this.fname = fname.Trim();
            this.patronymic = patronymic.Trim();
            this.birthday = birthday;
        }
        public Human(string surname, string fname, string patronymic, string birthday)
        {
            this.surname = surname;
            this.fname = fname;
            this.patronymic = patronymic;
            this.birthday = birthday;
        }

        public string toString()
        {
            return $"'{surname}', '{fname}', '{patronymic}', '{birthday}'";
        }
    }
}