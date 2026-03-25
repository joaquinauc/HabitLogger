using HabbitLogger;
using Spectre.Console;
using System.Globalization;

namespace HabitLogger
{
    internal class Helpers
    {
        internal bool LeapYear(int year)
        {
            bool isLeapYear;

            if (year % 4 == 0)
            {
                isLeapYear = true;

                if (year % 100 == 0)
                {
                    if (year % 400 == 0)
                    {
                        isLeapYear = true;
                    }
                    else
                    {
                        isLeapYear = false;
                    }
                }
            }
            else
            {
                isLeapYear = false;
            }

            return isLeapYear;
        }

        internal bool InvalidDateCheck(int year, int month, int day, bool leapYear)
        {
            bool isDateInvalid = true;

            int[] daysEachMonth = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];

            if (leapYear)
            {
                daysEachMonth[1] = 29;
            }

            if (month < 1)
            {
                return isDateInvalid;
            }
            else if (day <= daysEachMonth[month - 1] && (month <= 12 && month >= 1) && (year <= 9999 && year >= 0))
            {
                isDateInvalid = false;
            }

            return isDateInvalid;
        }

        internal double GetQuantityGoal(string habitName)
        {
            DatabaseFunctions databaseFunctions = new();

            int habitIndex = databaseFunctions.ReadHabits("name").IndexOf(habitName);

            double.TryParse(databaseFunctions.ReadHabits("quantity_goal")[habitIndex], out double quantityGoal);

            return quantityGoal;
        }

        internal DateTime FormatDate(int year, int month, int day)
        {
            string formattedYear = $"{year}";
            string formattedMonth = $"{month}";
            string formattedDay = $"{day}";

            if (day < 10)
            {
                formattedDay = "0" + formattedDay;
            }

            if (month < 10)
            {
                formattedMonth = "0" + formattedMonth;
            }

            if (formattedYear.Length < 4)
            {
                for (int i = formattedYear.Length; i < 4; i++)
                {
                    formattedYear = "0" + formattedYear;
                }
            }

            string formattedDate = $"{formattedYear}-{formattedMonth}-{formattedDay}";

            DateTime date = DateTime.ParseExact(formattedDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            return date;
        }

        internal bool GoalAchieved(double quantityGoal, double quantity)
        {
            if (quantity >= quantityGoal)
                return true;
            else
                return false;
        }

        internal Table GetLogsTable(List<(int, string, double, bool, DateTime)> logs)
        {
            Table logsTable = new Table();

            logsTable.AddColumn("ID");
            logsTable.AddColumn("Name");
            logsTable.AddColumn("Quantity");
            logsTable.AddColumn("Goal");
            logsTable.AddColumn("Date");

            for (int i = 0; i < logs.Count(); i++)
            {
                logsTable.AddRow(logs[i].Item1.ToString(), logs[i].Item2, logs[i].Item3.ToString("F2"), logs[i].Item4 ? "[green]OK[/]" : "[red]X[/]", logs[i].Item5.ToString("yyyy-MM-dd"));
            }

            return logsTable;
        }
    }
}
