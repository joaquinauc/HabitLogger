using HabbitLogger;
using System.Globalization;

namespace HabitLogger
{
    internal class Helpers
    {
        DatabaseFunctions databaseFunctions = new();

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
            else if (day <= daysEachMonth[month - 1] && (month <= 12 && month >= 1) && (year <= 9999 && year >= 0) && !(day == 0 || month == 0 || year == 0))
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

        internal void GenerateDatabase()
        {
            List<string> tables = databaseFunctions.CreateTable("habit");
            databaseFunctions.CreateTable("habit_log");

            if (tables.Count == 0)
            {
                GenerateSampleData();
            }
        }

        internal void GenerateSampleData()
        {
            List<(string, double, string)> sampleHabits = new()
            {
                ("Exercise", 30, "minutes"),
                ("Read", 20, "pages"),
                ("Meditate", 15, "minutes"),
                ("Drink Water", 8, "glasses"),
                ("Sleep", 8, "hours")
            };

            List<int> sampleMaxQuantity = new() { 60, 100, 30, 12, 12 };

            foreach (var sample in sampleHabits)
            {
                databaseFunctions.InsertHabitType(sample.Item1, sample.Item2, sample.Item3);
            }

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    int quantity = new Random().Next(0, sampleMaxQuantity[i] + 1);
                    bool goalAchieved = GoalAchieved(sampleHabits[i].Item2, quantity);
                    DateTime date = FormatDate(new Random().Next(2020, 2027), new Random().Next(1, 13), new Random().Next(1, 29));

                    databaseFunctions.InsertHabitLog(name: sampleHabits[i].Item1, quantity: quantity, goalAchieved: goalAchieved, date);
                }
            }
        }

        internal bool CheckIfHabitExists(string name)
        {
            List<string> habits = databaseFunctions.ReadHabits("name");

            if (habits.Contains(name))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
