using HabbitLogger;

namespace HabitLogger;

internal class HabitLoggerFunctionality
{
    internal void InsertHabitLog(HabitInterface habitInterface)
    {
        habitInterface.InsertMenu();
    }

    internal void UpdateHabitLog(HabitInterface habitInterface)
    {
        habitInterface.UpdateHabitLog();
    }

    internal void DeleteHabitLog(HabitInterface habitInteface)
    {
        habitInteface.DeleteHabitLog();
    }

    internal void ReadHabitLogs(HabitInterface habitInterface)
    {
        habitInterface.ReadHabitLogs(true);
    }
}
