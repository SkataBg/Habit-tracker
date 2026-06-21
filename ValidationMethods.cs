using System.Globalization;
namespace habit_tracker;
public class ValidationMethods
{
        internal static string GetDateInput()
    {
        Console.WriteLine("\n\nPlease insert the date: (Format: dd-mm-yy). Type 1 to enter today's date. Type 0 to return to main menu.");
        string dateInput = Console.ReadLine();
        if (dateInput == "0") 
        {
            Program.MainMenu();
        }
        if (dateInput == "1") 
        {
            dateInput = DateTime.Now.ToString("dd-MM-yy");
            return dateInput;
        }

        while (!DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
        {
            Console.WriteLine("\n\nInvalid Date. Try again. (Format: dd-mm-yy). Type 0 to return.\n\n");
            dateInput = Console.ReadLine();
            if (dateInput == "0") Program.MainMenu();
        }
         return dateInput;
    } 
    
    public static string GetHabitInput()
    {
        Console.WriteLine("\n\nPlease insert the habit you want to track. Type 0 to return to main menu.");
        string habitInput = Console.ReadLine();
        if (habitInput == "0") Program.MainMenu();
        while (string.IsNullOrWhiteSpace(habitInput) || !LetterCheck(habitInput))
        {
            Console.WriteLine("\n\nInvalid input. Try again. Type 0 to return.\n\n");
            habitInput = Console.ReadLine();
            if (habitInput == "0") Program.MainMenu();
        }
        habitInput = char.ToUpper(habitInput[0]) + habitInput.Substring(1).ToLower();
         return habitInput;
    }

    public static bool LetterCheck(string input)
        {
            foreach (char c in input)
            {
                if (!char.IsLetter(c) && c != ' ')
                    return false;
            }
            return true;
        }
    public static int GetNumberInput(string message)
    {
        Console.WriteLine(message);
        string numberInput = Console.ReadLine();
        while (!Int32.TryParse(numberInput, out _) || Convert.ToInt32(numberInput) < 0) 
        {
            Console.WriteLine("\n\nInvalid number. Try again. Type 0 to return.\n\n");
            numberInput = Console.ReadLine();
        }
        if(numberInput=="0") Program.MainMenu();
        int finalInput = Convert.ToInt32(numberInput);
        return finalInput;
    }
}