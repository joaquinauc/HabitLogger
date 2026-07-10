using HabbitLogger;
using HabitLogger;

HabitInterface habitInterface = new();
Helpers helpers = new();

helpers.GenerateDatabase();

do
{
    bool exit = habitInterface.MainMenu();
    if (exit)
    {
        break;
    }
} while (true);
