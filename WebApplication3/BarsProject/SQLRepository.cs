﻿using System;
using System.Collections.Generic;
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

        public void addHuman(Human human)
        {
            string cmd = $"insert into {Settings.tableName}(last_name, first_name, patronymic, birthday) values({human.toString()})";
            IfxCommand command = new IfxCommand(cmd, connection);
            command.ExecuteNonQuery();
        }
        public void editHuman(Human human)
        {
            string cmd = $"UPDATE {Settings.tableName} SET (last_name, first_name, patronymic, birthday) = ({human.toString()}) where id={human.id}";
            IfxCommand command = new IfxCommand(cmd, connection);
            command.ExecuteNonQuery();
        }

        // public String searchHumanBySurname(string surname)
        // {
        //     IfxCommand cmd = new IfxCommand($"select * from {Settings.tableName} where last_name='{surname}'", connection);
        //     IfxDataReader reader = cmd.ExecuteReader();
        //     List<Human> humans = new List<Human>();
        //     while (reader.Read())
        //     {
        //         humans.Add(new Human(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4)));
        //     }
        //
        //     string toReturn = JsonConvert.SerializeObject(humans);
        //     return toReturn;
        // }
        public String searchHuman(string surname, string fname, string patronymic, string birthday)
        {
            string command = $"select * from {Settings.tableName} where ";
            if (surname.IsEmpty() && fname.IsEmpty() && patronymic.IsEmpty() && birthday.IsEmpty())
            {
                command = $"select * from {Settings.tableName}";
            }
            else
            {
                if (!surname.IsEmpty())
                {
                    command += $"last_name='{surname}' AND";
                }
                if (!fname.IsEmpty())
                {
                    command += $" first_name='{fname}' AND";
                }
                if (!patronymic.IsEmpty())
                {
                    command += $" patronymic='{patronymic}' AND";
                }
                if (!birthday.IsEmpty())
                {
                    command += $" birthday='{birthday}'";
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

        public void stop()
        {
            connection.Close();
        }
    }
}