using HabbitLogger;

DatabaseFunctions databaseFunctions = new();
HabitInterface habitInterface = new();

databaseFunctions.CreateTable("habit");
databaseFunctions.CreateTable("habit_log");

do
{
    bool exit = habitInterface.MainMenu();
    if (exit)
    {
        break;
    }
} while (true);
