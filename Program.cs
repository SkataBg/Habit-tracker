using Microsoft.Data.Sqlite;

namespace habit_tracker;
class Program
{
    static string connectionString = @"Data Source=habit-Tracker.db";

    static void Main(string[] args)
    {
        //DropTable()
        TableMethods.CreateTableIfNull();
        MainMenu();
    }
    public static void MainMenu()
    {
        bool closeApp=false;
        while (closeApp == false)
        {
            Console.Clear();
            Console.WriteLine("\n\nMAIN MENU");
            Console.WriteLine("\nWhat would you like to do?");
            Console.WriteLine("\nType 0 to Close Application.");
            Console.WriteLine("Type 1 to View all Records.");
            Console.WriteLine("Type 2 to Insert Record.");
            Console.WriteLine("Type 3 to Delete Record.");
            Console.WriteLine("Type 4 to Update Record.");
            Console.WriteLine("------------------------------------------------\n");

            string commandInput = Console.ReadLine();

            switch (commandInput)
            {
                case "0":
                    Console.WriteLine("\nGoodbye!\n");
                    closeApp=true;
                    Environment.Exit(1);
                    break;
                case "1":
                    TableMethods.GetAllRecords();
                    break;
                case "2":
                    TableMethods.Insert();
                    break;
                case "3":
                    TableMethods.Delete();
                    break;
                case "4":
                    TableMethods.Update();
                    break;
                default:
                    Console.WriteLine("Invalid command. Please type a command between 0 and 4");
                    break;
            }
        Console.ReadKey(true);
        }
    }
}


