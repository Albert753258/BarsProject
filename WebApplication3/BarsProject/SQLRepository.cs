using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.WebPages;
using IBM.Data.Informix;

namespace InformixConnector
{
    public class SQLRepository
    {
        private string host;
        private string service;
        private string serverName;
        private string DBName;
        private string userName;
        private string password;
        public SQLRepository(string host, string service, string serverName, string DBName, string userName, string password)
        {
            this.host = host;
            this.service = service;
            this.serverName = serverName;
            this.DBName = DBName;
            this.userName = userName;
            this.password = password;
        }

        private IfxConnection createConnection()
        {
            IfxConnection connection = new IfxConnection("Host=" + host + ";Service=" + service + ";Server=" + serverName + ";Database=" + DBName + ";User ID=" + userName + ";password=" + password + ";CLIENT_LOCALE=ru_RU.CP1251;DB_LOCALE=ru_RU.915");
            connection.Open();
            return connection;
        }

        private string addHuman(Human human, IfxConnection connection)
        {
            //https://www.ibm.com/docs/en/informix-servers/12.10?topic=class-ifxconnection-example
            string cmd = $"insert into {Settings.tableName}(last_name, first_name, patronymic, birthday) values({human.toString()})";
            string findLastId = "select DBINFO ('sqlca.sqlerrd1') from table(set{1})";
            using (IfxCommand command = new IfxCommand(cmd, connection))
            {
                command.ExecuteNonQuery();
            }
            using (IfxCommand findLastIdCommand = new IfxCommand(findLastId, connection))
            {
                using (IfxDataReader reader = findLastIdCommand.ExecuteReader())
                {
                    reader.Read();
                    int lastId = reader.GetInt32(0);
                    reader.Close();
                    connection.Close();
                    string toReturn = $"{{ success: true, id: '{lastId}'}}";
                    return toReturn;
                }
            }
        }
        public string addHuman(Human human, string confirm)
        {
            using IfxConnection connection = createConnection();
            if (!confirm.IsEmpty())
            {
                return addHuman(human, connection);
            }
            else
            {
                //проверка на дубликат
                if (getResultsCount(human.surname, human.fname, human.patronymic, human.birthday, human.birthday, 0, "100", connection) == "0")
                {
                    return addHuman(human, connection);
                }
                else
                {
                    connection.Close();
                    return "{'success': true, 'error': 'duplicate'}";;
                }
            }
        }

        private string editHuman(Human human, IfxConnection connection)
        {
            string cmd = $"UPDATE {Settings.tableName} SET (last_name, first_name, patronymic, birthday) = ({human.toString()}) where id={human.id}";
            using (IfxCommand command = new IfxCommand(cmd, connection))
            {
                command.ExecuteNonQuery();
                connection.Close();
            }
            return "";
        }
        public string editHuman(Human human, string confirm)
        {
            using IfxConnection connection = createConnection();
            if (!confirm.IsEmpty())
            {
                return editHuman(human, connection);
            }
            else
            {
                //проверка на дубликат
                if (getResultsCount(human.surname, human.fname, human.patronymic, human.birthday, human.birthday, 0, "100", connection) == "0")
                {
                    return editHuman(human, connection);
                }
                else
                {
                    connection.Close();
                    return "{'success': true, 'error': 'duplicate'}";
                }
            }
        }

        private Pair<string, string> genStrForSearch(string surname, string fname, string patronymic, string birthdayFrom, string birthdayTo, int page, string limit)
        {
            int skipNum;
            //TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
            string command = "";
            if (limit.IsEmpty())
            {
                command = $"select  * from {Settings.tableName} where ";
            }
            else
            {
                skipNum = (page - 1) * Convert.ToInt32(limit);
                command = $"select skip {skipNum} limit {limit} * from {Settings.tableName} where ";
            }
            //команда для нахождения количества результатов поиска
            string countCommand = $"select count(*) from {Settings.tableName} where ";
            //если нет условий поиска( все параметры пустые)
            if (surname.IsEmpty() && fname.IsEmpty() && patronymic.IsEmpty() && birthdayFrom.IsEmpty() && birthdayTo.IsEmpty())
            {
                if (limit.IsEmpty())
                {
                    command = $"select * from {Settings.tableName}";
                    countCommand = $"select count(*) from {Settings.tableName}";
                }
                else
                {
                    skipNum = (page - 1) * Convert.ToInt32(limit);
                    command = $"select skip {skipNum} limit {limit} * from {Settings.tableName}";
                    countCommand = $"select count(*) from {Settings.tableName}";
                }
            }
            else
            {
                if (!surname.IsEmpty())
                {
                    command += $"last_name matches '{surname}' AND";
                    countCommand += $"last_name matches '{surname}' AND";
                }
                if (!fname.IsEmpty())
                {
                    command += $" first_name matches '{fname}' AND";
                    countCommand += $" first_name matches '{fname}' AND";
                }
                if (!patronymic.IsEmpty())
                {
                    command += $" patronymic matches '{patronymic}' AND";
                    countCommand += $" patronymic matches '{patronymic}' AND";
                }
                if (!birthdayFrom.IsEmpty())
                {
                    if (!birthdayTo.IsEmpty())
                    {
                        if(birthdayFrom == birthdayTo)
                        {
                            command += $" birthday = '{birthdayFrom}'";
                            countCommand += $" birthday = '{birthdayFrom}'";
                        }
                        else
                        {
                            command += $" birthday between '{birthdayFrom}' and '{birthdayTo}'";
                            countCommand += $" birthday between '{birthdayFrom}' and '{birthdayTo}'";
                        }
                    }
                    else
                    {
                        command += $" birthday >= '{birthdayFrom}'";
                        countCommand += $" birthday >= '{birthdayFrom}'";
                    }
                    //
                }
                else if (!birthdayTo.IsEmpty())
                {
                    command += $" birthday <= '{birthdayTo}'";
                    countCommand += $" birthday <= '{birthdayTo}'";
                }
            }
            //удаление " AND" в конце
            int length = command.Length;
            if (command[length - 1] == 'D' && command[length - 2] == 'N' && command[length - 3] == 'A' && command[length - 4] == ' ' && command[length - 5] == '\'')
            {
                command = command.Remove(length - 4, 4);
                countCommand = countCommand.Remove(countCommand.Length - 4, 4);
            }
            command += " order by last_name, first_name, patronymic, birthday";
            return new Pair<string, string>
            {
                first = command,
                second = countCommand
            };
        }
        public Pair<List<Human>, string> searchHuman(string surname, string fname, string patronymic, string birthdayFrom, string birthdayTo, int page, string limit)
        {
            using IfxConnection connection = createConnection();
            Pair<string, string> stringPair = genStrForSearch(surname, fname, patronymic, birthdayFrom, birthdayTo, page, limit);
            using IfxDataReader reader = new IfxCommand(stringPair.first, connection).ExecuteReader();
            using IfxDataReader countReader = new IfxCommand(stringPair.second, connection).ExecuteReader();
            countReader.Read();
            Pair<List<Human>, string> pair = new Pair<List<Human>, string>();
            List<Human> humans = new List<Human>();
            while (reader.Read())
            {
                humans.Add(new Human(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4).Replace('-', '.')));
            }
            reader.Close();
            pair.first = humans;
            pair.second = countReader.GetString(0);
            countReader.Close();
            connection.Close();
            return pair;
        }
        
        private string getResultsCount(string surname, string fname, string patronymic, string birthdayFrom, string birthdayTo, int page, string limit, IfxConnection connection)
        {
            Pair<string, string> stringPair = genStrForSearch(surname, fname, patronymic, birthdayFrom, birthdayTo, page, limit);
            using (IfxCommand command = new IfxCommand(stringPair.second, connection))
            {
                using (IfxDataReader countReader = command.ExecuteReader())
                {
                    countReader.Read();
                    string toReturn = countReader.GetString(0);
                    countReader.Close();
                    return toReturn;
                }
            }
        }
        
        public class Pair<T, U>
        {
            public T first;
            public U second;
        }
        public void delHuman(string id)
        {
            using (IfxConnection connection = createConnection())
            {
                string cmd = $"delete from {Settings.tableName} where id={id}";
                using (IfxCommand command = new IfxCommand(cmd, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}