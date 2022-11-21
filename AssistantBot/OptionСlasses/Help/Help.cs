using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptionСlasses.Help;

public class Help
{
    private string command;
    public Help(string com)
    {
        command = com;
    }
    public Help()
    {
        command = "all";
    }
    private static string GetDescriptionOfcommandAll()
    {   
        StringBuilder sb = new StringBuilder();
        string sqlExpression = "SELECT name_command,description_command FROM DescriptionOfCommands";
        using (var connection = new SqliteConnection(@"Data Source=AssistentData\AssistentBotDataBase.db"))
        {
            connection.Open();
            SqliteCommand sqliteCommand = new SqliteCommand(sqlExpression, connection);

            using (SqliteDataReader reader = sqliteCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    sb.Append(reader.GetString(0));
                    sb.Append(reader.GetString(1));
                    sb.Append(Environment.NewLine);
                    sb.Append("----------------------------------------------------");
                    sb.Append(Environment.NewLine);
                }
            }
        }
        return sb.ToString();
    }
    private static string GetDescriptionOfcommand(string p)
    {
        StringBuilder sb = new StringBuilder();
        string sqlExpression = $"SELECT name_command,description_command FROM DescriptionOfCommands WHERE name_command='{p}'";
        using (var connection = new SqliteConnection(@"Data Source=AssistentData\AssistentBotDataBase.db"))
        {
            connection.Open();
            SqliteCommand sqliteCommand = new SqliteCommand(sqlExpression, connection);

            using (SqliteDataReader reader = sqliteCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    sb.Append(reader.GetString(0));
                    sb.Append(reader.GetString(1));
                    sb.Append(Environment.NewLine);
                    sb.Append("----------------------------------------------------");
                    sb.Append(Environment.NewLine);
                }
            }
        }
        return sb.ToString();
    }
    public string GetDescriptionOfcommandParad()
    {
        if (command == "all")
            return GetDescriptionOfcommandAll();
        else
            return GetDescriptionOfcommand(command);
    }
}

