using HabitLogger;
using Spectre.Console;
using static HabbitLogger.Enums;

namespace HabbitLogger;

internal class HabitInterface
{
    internal void MainMenu()
    {
        Console.Clear();

        var mainMenuOption = AnsiConsole.Prompt(
            new SelectionPrompt<MainMenuOptions>()
            .Title("Select which action you wish to do: ")
            .AddChoices(Enum.GetValues<MainMenuOptions>())
        );

        HabitLoggerFunctionality habitLoggerFunctionality = new();

        switch (mainMenuOption)
        {
            case MainMenuOptions.InsertRecord:
                habitLoggerFunctionality.InsertHabitLog(this);
                break;

            case MainMenuOptions.DeleteRecord:
                break;

            case MainMenuOptions.UpdateRecord:
                break;

            case MainMenuOptions.ViewAllRecords:
                break;

            default:
                break;
        }
    }

    internal void InsertHabitLog()
    {
        Console.Clear();

        // Solo la cantidad hecha y la fecha, porque el nombre es el del habito que nomas se le pasa como valor, y si se logró o no es una comparación

        DatabaseFunctions databaseFunctions = new();

        Console.Write("Insert a quantity goal for this habit: ");
        string quantity = Console.ReadLine();

        Console.Write("");
        string date = Console.ReadLine();
    }

    internal void InsertHabit()
    {
        Console.Clear();

        DatabaseFunctions databaseFunctions = new();

        Console.Write("Insert a name for this habit: ");
        string name = Console.ReadLine();

        Console.Write("Insert a quantity goal for this habit: ");
        string quantityGoal = Console.ReadLine();

        Console.Write("Insert a unit this goal is going to be measured with: ");
        string unit = Console.ReadLine();

        double.TryParse(quantityGoal, out double quantity); // Maybe ponerla en otra clase, porque se me hace que aqui no va

        databaseFunctions.InsertHabit(name, quantity, unit);
    }

    internal void InsertMenu()
    {
        Console.Clear();

        var insertHabitLog = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Select which habit type you wish to insert from")
            .AddChoices("NewHabitType")
        );

        DatabaseFunctions databaseFunctions = new();

        if (insertHabitLog == "NewHabitType")
        {   // Mandar a llamar a una función para recopilar los datos necesarios para insertar un nuevo hábito, o hacerlos aqui mismo
            // Se puede hacer una mini interfaz en esta clase para recopilar esos datos, y luego pasar esos datos a la función InsertHabit en DatabaseFunctions
            InsertHabit(); //Pasar argumentos con los valores que se van a agregar a la base de datos
        }
    }
}
