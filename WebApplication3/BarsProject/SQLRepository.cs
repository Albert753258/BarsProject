using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.WebPages;
using IBM.Data.Informix;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;

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

        public string addHuman(Human human)
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
        public void editHuman(Human human)
        {
            string cmd = $"UPDATE {Settings.tableName} SET (last_name, first_name, patronymic, birthday) = ({human.toString()}) where id={human.id}";
            IfxCommand command = new IfxCommand(cmd, connection);
            command.ExecuteNonQuery();
        }
        
        public String searchHuman(string surname, string fname, string patronymic, string birthdayFrom, string birthdayTo)
        {
            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
            string command = $"select * from {Settings.tableName} where ";
            if (surname.IsEmpty() && fname.IsEmpty() && patronymic.IsEmpty() && birthdayFrom.IsEmpty() && birthdayTo.IsEmpty())
            {
                command = $"select * from {Settings.tableName}";
            }
            else
            {
                if (!surname.IsEmpty())
                {
                    command += $"last_name matches '{textInfo.ToUpper(surname)}' AND";
                }
                if (!fname.IsEmpty())
                {
                    command += $" first_name matches '{textInfo.ToUpper(fname)}' AND";
                }
                if (!patronymic.IsEmpty())
                {
                    command += $" patronymic matches '{textInfo.ToUpper(patronymic)}' AND";
                }
                if (!birthdayFrom.IsEmpty())
                {
                    if (!birthdayTo.IsEmpty())
                    {
                        command += $" birthday between '{birthdayFrom}' and '{birthdayTo}'";
                    }
                    else
                    {
                        command += $" birthday > '{textInfo.ToUpper(birthdayFrom)}'";
                    }
                    //
                }
                else if (!birthdayTo.IsEmpty())
                {
                    command += $" birthday > '{textInfo.ToUpper(birthdayTo)}'";
                    //
                }
            }

            int length = command.Length;
            if (command[length - 1] == 'D' && command[length - 2] == 'N' && command[length - 3] == 'A' && command[length - 4] == ' ' && command[length - 5] == '\'')
            {
                command = command.Remove(length - 4, 4);
            }
            IfxCommand cmd = new IfxCommand(command, connection);
            IfxDataReader reader = cmd.ExecuteReader();
            List<Human> humans = new List<Human>();
            while (reader.Read())
            {
                humans.Add(new Human(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4)));
            }

            string toReturn = JsonConvert.SerializeObject(humans);
            return toReturn;
        }
        public void delHuman(string id)
        {
            string cmd = $"delete from {Settings.tableName} where id={id}";
            IfxCommand command = new IfxCommand(cmd, connection);
            command.ExecuteNonQuery();
        }
    }
}