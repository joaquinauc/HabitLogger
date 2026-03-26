using Microsoft.Data.Sqlite;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HabbitLogger;

internal class DatabaseFunctions
{
    internal void CreateTable(string table_name)
    {
        List<string> tables = new();

        using (var connection = new SqliteConnection($"Data Source='habit_logger.db'"))
        {
            connection.Open();

            var command = connection.CreateCommand();

            command.CommandText = "SELECT name FROM sqlite_master WHERE type='table';";
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    tables.Add(reader.GetString(0));
                }
            }

            if (!tables.Contains(table_name))
            {
                if (table_name == "habit")
                {
                    command.CommandText =
                    $@"
                        CREATE TABLE {table_name} (
                            id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                            name TEXT NOT NULL,
                            quantity_goal DOUBLE NOT NULL,
                            unit TEXT NOT NULL
                        );
                    ";
                }
                else if (table_name == "habit_log")
                {
                    command.CommandText =
                    $@"
                        CREATE TABLE {table_name} (
                            id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                            name TEXT NOT NULL,
                            quantity DOUBLE NOT NULL,
                            goal_achieved BOOLEAN NOT NULL,
                            date DATE NOT NULL
                        );
                    ";
                }

                command.ExecuteNonQuery();
            }
        }
    }

    internal void GetTableRecords()
    {

    }

    internal void InsertHabitLog(string name, double quantity, bool goalAchieved, DateTime date)
    {
        using (var connection = new SqliteConnection("Data Source=habit_logger.db"))
        {
            connection.Open();

            var command = connection.CreateCommand();

            command.CommandText =
            @"
                INSERT INTO habit_log (name, quantity, goal_achieved, date)
                values (@Name, @Quantity, @Goal_achieved, @Date)
            ";

            command.Parameters.AddWithValue("@Name", name);
            command.Parameters.AddWithValue("@Quantity", quantity);
            command.Parameters.AddWithValue("@Goal_achieved", goalAchieved);
            command.Parameters.AddWithValue("@Date", date);

            command.ExecuteNonQuery();
        }
    }

    internal void InsertHabitType(string name, double quantityGoal, string unit) // Que este metodo tenga parametros con los valores que se van a agregar a la base de datos
    {
        using (var connection = new SqliteConnection("Data Source=habit_logger.db"))
        {
            connection.Open();

            var command = connection.CreateCommand();

            command.CommandText =
            @"
                INSERT INTO habit (name, quantity_goal, unit)
                values (@Name, @Quantity_goal, @Unit)
            ";

            command.Parameters.AddWithValue("@Name", name);
            command.Parameters.AddWithValue("@Quantity_goal", quantityGoal);
            command.Parameters.AddWithValue("@Unit", unit);

            command.ExecuteNonQuery();
        }
    }

    internal void UpdateHabitLog(int id, double quantity, bool goalAchieved, DateTime date)
    {
        using (var connection = new SqliteConnection("Data Source=habit_logger.db"))
        {
            connection.Open();

            var command = connection.CreateCommand();

            command.CommandText =
            @"
                UPDATE habit_log
                SET quantity = @Quantity,
                    goal_achieved = @Goal_achieved,
                    date = @Date
                WHERE id = @Id
            ";

            command.Parameters.AddWithValue("@Quantity", quantity);
            command.Parameters.AddWithValue("@Goal_achieved", goalAchieved);
            command.Parameters.AddWithValue("@Date", date);
            command.Parameters.AddWithValue("@Id", id);

            command.ExecuteNonQuery();
        }
    }

    internal void DeleteHabitLog()
    {

    }

    internal List<string> ReadHabits(string column)
    {
        List<string> habits = new();

        using (var connection = new SqliteConnection("Data Source=habit_logger.db"))
        {
            connection.Open();

            var command = connection.CreateCommand();

            command.CommandText = $"SELECT {column} FROM habit;";

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    habits.Add(reader.GetString(0));
                }
            }
        }

        return habits;
    }

    internal List<(int, string, double, bool, DateTime)> ReadHabitLogs(string habit)
    {
        List<(int, string, double, bool, DateTime)> logs = new();

        using (var connection = new SqliteConnection("Data Source=habit_logger.db"))
        {
            connection.Open();

            var command = connection.CreateCommand();

            command.CommandText =
            @"
                SELECT id, name, quantity, goal_achieved, date
                FROM habit_log
                WHERE name = @Name
            ";

            command.Parameters.AddWithValue("@Name", habit);

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    logs.Add((reader.GetInt16(0), reader.GetString(1), reader.GetDouble(2), reader.GetBoolean(3), reader.GetDateTime(4)));
                }
            }
        }

        return logs;
    }
}
