using HabitLogger;
using Spectre.Console;
using static HabbitLogger.Enums;

namespace HabbitLogger;

internal class HabitInterface
{
    DatabaseFunctions databaseFunctions = new();
    Helpers helpers = new();

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

    internal void InsertHabitLog(string name)
    {
        Console.Clear();

        // Solo la cantidad hecha y la fecha, porque el nombre es el del habito que nomas se le pasa como valor, y si se logró o no es una comparación

        Console.Write("Insert the quantity you achieved of this habit: ");
        string quantity = Console.ReadLine();

        double.TryParse(quantity, out double quantityParsed);

        Console.Clear();

        // if *Parsed == 0 sería la condición de error para la fecha
        // Hacer las funciones de año bisiesto, y fecha invalida en Helpers
        // Estas funciones, ver la manera que todas corran en Helpers

        Console.WriteLine("Please, insert the date this habit was done:");
        Console.Write("Year: ");
        string? year = Console.ReadLine();

        int.TryParse(year, out int yearParsed);

        Console.Write("Month (1-12): ");
        string? month = Console.ReadLine();

        int.TryParse(month, out int monthParsed);

        Console.Write("Day: ");
        string? day = Console.ReadLine();

        int.TryParse(day, out int dayParsed);

        if (yearParsed == 0 || monthParsed == 0 || dayParsed == 0 || helpers.InvalidDateCheck(yearParsed, monthParsed, dayParsed, helpers.LeapYear(yearParsed)))
        {
            Console.Clear();
            Console.WriteLine("The inserted date is invalid, please try again.");
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }
        else
        {
            // Crear una metodo en Helpers para dar formato a la fecha
            databaseFunctions.InsertHabitLog(name, quantityParsed, helpers.GoalAchieved(helpers.GetQuantityGoal(name), quantityParsed), helpers.FormatDate(yearParsed, monthParsed, dayParsed));
        }
    }

    internal void InsertHabitType()
    {
        Console.Clear();

        Console.Write("Insert a name for this habit: ");
        string? name = Console.ReadLine();

        Console.Write("Insert a quantity goal for this habit (numeric value): ");
        string? quantityGoal = Console.ReadLine();

        Console.Write("Insert a unit this goal is going to be measured with: ");
        string? unit = Console.ReadLine();

        if (!double.TryParse(quantityGoal, out double quantity) || name == "" || unit == "") // Maybe ponerla en otra clase, porque se me hace que aqui no va
        {
            Console.Clear();
            Console.WriteLine("The inserted data is invalid, please try again.");
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }
        else
        {
            databaseFunctions.InsertHabitType(name, quantity, unit);
        }
    }

    internal void InsertMenu()
    {
        Console.Clear();

        var habitSelected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Select which habit type you wish to insert from")
            .AddChoices(databaseFunctions.ReadHabits("name"))
        );

        if (habitSelected == "Insert new habit type")
        {   // Mandar a llamar a una función para recopilar los datos necesarios para insertar un nuevo hábito, o hacerlos aqui mismo
            // Se puede hacer una mini interfaz en esta clase para recopilar esos datos, y luego pasar esos datos a la función InsertHabit en DatabaseFunctions
            InsertHabitType(); //Pasar argumentos con los valores que se van a agregar a la base de datos
        }

        else
        {
            InsertHabitLog(habitSelected);
        }
    }
}
