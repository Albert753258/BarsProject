using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.WebPages;
using IBM.Data.Informix;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace InformixConnector
{
    public class SQLRepository
    {
        public IfxConnection connection;
        public SQLRepository(string host, string service, string serverName, string DBName, string userName, string password)
        {
            connection = new IfxConnection("Host=" + host + ";Service=" + service + ";Server=" + serverName + ";Database=" + DBName + ";User ID=" + userName + ";password=" + password + ";CLIENT_LOCALE=ru_RU.CP1251;DB_LOCALE=ru_RU.915");
            connection.Open();
        }

        public string addHuman(Human human, string confirm)
        {
            if (!confirm.IsEmpty())
            {
                string cmd = $"insert into {Settings.tableName}(last_name, first_name, patronymic, birthday) values({human.toString()})";
                IfxCommand command = new IfxCommand(cmd, connection);
                command.ExecuteNonQuery();
                string findLastId = "select DBINFO ('sqlca.sqlerrd1') from table(set{1})";
                IfxCommand findLastIdCommand = new IfxCommand(findLastId, connection);
                IfxDataReader reader = findLastIdCommand.ExecuteReader();
                reader.Read();
                int lastId = reader.GetInt32(0);
                string toReturn = $"{{ success: true, id: '{lastId}'}}";
                return toReturn;
            }
            else
            {
                if (genStrForSearch(human.surname, human.fname, human.patronymic, human.birthday, human.birthday, 0, "100")
                        .count == "0")
                {
                    string cmd = $"insert into {Settings.tableName}(last_name, first_name, patronymic, birthday) values({human.toString()})";
                    IfxCommand command = new IfxCommand(cmd, connection);
                    command.ExecuteNonQuery();
                    string findLastId = "select DBINFO ('sqlca.sqlerrd1') from table(set{1})";
                    IfxCommand findLastIdCommand = new IfxCommand(findLastId, connection);
                    IfxDataReader reader = findLastIdCommand.ExecuteReader();
                    reader.Read();
                    int lastId = reader.GetInt32(0);
                    string toReturn = $"{{ success: true, id: '{lastId}'}}";
                    return toReturn;
                }
                else
                {
                    return "{'success': true, 'error': 'duplicate'}";;
                }
            }

        }
        public string editHuman(Human human, string confirm)
        {
            if (!confirm.IsEmpty())
            {
                string cmd = $"UPDATE {Settings.tableName} SET (last_name, first_name, patronymic, birthday) = ({human.toString()}) where id={human.id}";
                IfxCommand command = new IfxCommand(cmd, connection);
                command.ExecuteNonQuery();
                return "";
            }
            else
            {
                if (genStrForSearch(human.surname, human.fname, human.patronymic, human.birthday, human.birthday, 0, "100")
                        .count == "0")
                {
                    string cmd = $"UPDATE {Settings.tableName} SET (last_name, first_name, patronymic, birthday) = ({human.toString()}) where id={human.id}";
                    IfxCommand command = new IfxCommand(cmd, connection);
                    command.ExecuteNonQuery();
                    return "{'success': true}";
                }
                else
                {
                    return "{'success': true, 'error': 'duplicate'}";;
                }
            }
        }

        public Pair<string> genStrForSearch(string surname, string fname, string patronymic, string birthdayFrom, string birthdayTo, int page, string limit)
        {
            int skipNum;
            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
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
            string countCommand = $"select count(*) from {Settings.tableName} where ";
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
                    command += $"last_name matches '{textInfo.ToUpper(surname)}' AND";
                    countCommand += $"last_name matches '{textInfo.ToUpper(surname)}' AND";
                }
                if (!fname.IsEmpty())
                {
                    command += $" first_name matches '{textInfo.ToUpper(fname)}' AND";
                    countCommand += $" first_name matches '{textInfo.ToUpper(fname)}' AND";
                }
                if (!patronymic.IsEmpty())
                {
                    command += $" patronymic matches '{textInfo.ToUpper(patronymic)}' AND";
                    countCommand += $" patronymic matches '{textInfo.ToUpper(patronymic)}' AND";
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
                        command += $" birthday >= '{textInfo.ToUpper(birthdayFrom)}'";
                        countCommand += $" birthday >= '{textInfo.ToUpper(birthdayFrom)}'";
                    }
                    //
                }
                else if (!birthdayTo.IsEmpty())
                {
                    command += $" birthday <= '{textInfo.ToUpper(birthdayTo)}'";
                    countCommand += $" birthday <= '{textInfo.ToUpper(birthdayTo)}'";
                    //
                }
            }

            int length = command.Length;
            if (command[length - 1] == 'D' && command[length - 2] == 'N' && command[length - 3] == 'A' && command[length - 4] == ' ' && command[length - 5] == '\'')
            {
                command = command.Remove(length - 4, 4);
                countCommand = countCommand.Remove(countCommand.Length - 4, 4);
            }
            command += " order by last_name, first_name, patronymic, birthday";
            IfxCommand countCmd = new IfxCommand(countCommand, connection);
            IfxDataReader countReader = countCmd.ExecuteReader();
            countReader.Read();
            Pair<string> pair = new Pair<string>();
            pair.humans = command;
            pair.count = countReader.GetString(0);
            return pair;
        }
        public Pair<List<Human>> searchHuman(string surname, string fname, string patronymic, string birthdayFrom, string birthdayTo, int page, string limit)
        {
            Pair<string> stringPair = genStrForSearch(surname, fname, patronymic, birthdayFrom, birthdayTo, page, limit);
            IfxCommand cmd = new IfxCommand(stringPair.humans, connection);
            IfxDataReader reader = cmd.ExecuteReader();
            Pair<List<Human>> pair = new Pair<List<Human>>();
            List<Human> humans = new List<Human>();
            while (reader.Read())
            {
                humans.Add(new Human(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4).Replace('-', '.')));
            }
            pair.humans = humans;
            pair.count = stringPair.count;
            return pair;
        }

        public class Pair<T>
        {
            public T humans;
            public string count;
        }
        public void delHuman(string id)
        {
            string cmd = $"delete from {Settings.tableName} where id={id}";
            IfxCommand command = new IfxCommand(cmd, connection);
            command.ExecuteNonQuery();
        }
    }
}