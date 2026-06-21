using Microsoft.Data.Sqlite;
using System.Globalization;



namespace habit_tracker;
public class TableMethods
{
    static string connectionString = @"Data Source=habit-Tracker.db";

    public static void CreateTableIfNull()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = @"CREATE TABLE IF NOT EXISTS habit_tracker(
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Date TEXT,
            Habit TEXT,
            Quantity INTEGER
            )";
            tableCmd.ExecuteNonQuery();
            connection.Close();
        }
        HabitSeeder seed = new HabitSeeder(connectionString);
        seed.SeedData(20);
    }
    public static void Insert()
    {
        Console.Clear();
        string date = ValidationMethods.GetDateInput();
        string habit = ValidationMethods.GetHabitInput();
        int quantity = ValidationMethods.GetNumberInput("\n\nPlease insert number of glasses or other measure of choice (no decimals allowed)\n\n");
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = 
            "INSERT INTO habit_tracker(date,habit, quantity) VALUES (@date,@habit, @quantity)";
            tableCmd.Parameters.AddWithValue("@date", date);
            tableCmd.Parameters.AddWithValue("@habit", habit);
            tableCmd.Parameters.AddWithValue("@quantity", quantity);

            tableCmd.ExecuteNonQuery();
            connection.Close();
        }
        Console.WriteLine("Successfully added new entry.");
    }

    public static void GetAllRecords()
    {
        Console.Clear();
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            
            tableCmd.CommandText=
            $"SELECT * FROM habit_tracker ";
            List<HabitTracker> tableData = new();
            SqliteDataReader reader = tableCmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    tableData.Add(
                    new HabitTracker
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        Date = DateTime.ParseExact(reader.GetString(reader.GetOrdinal("Date")), "dd-MM-yy", new CultureInfo("en-US")),
                        Habit = reader.GetString(reader.GetOrdinal("Habit")),
                        Quantity = reader.GetInt32(reader.GetOrdinal("Quantity"))
                    }); ;
                }
            }
            else 
            {
                Console.WriteLine("No rows found");
            }
            connection.Close();

            Console.WriteLine("-----------------------------");
            foreach (var dw in tableData)
            {
                Console.WriteLine($"{dw.Id} - {dw.Date.ToString("dd-MMM-yyyy")} - {dw.Habit} - Quantity: {dw.Quantity}");
            }
            Console.WriteLine("-----------------------------\n");
        }
    }

    public static void Delete()
    {
        Console.Clear();
        GetAllRecords();
        bool idDoesntExist = true;
        while (idDoesntExist)
        {
            var recordId = ValidationMethods.GetNumberInput("\n\nPlease type the Id of the record you want to delete. Press 0 to return.");
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = 
                $"DELETE FROM habit_tracker WHERE Id= @id";
                tableCmd.Parameters.AddWithValue("@id", recordId);

                int rowCount = tableCmd.ExecuteNonQuery();
                connection.Close();
                if (rowCount == 0)
                {
                    Console.WriteLine($"\n\nRecord with Id: {recordId} doesnt exist\n\n");
                    continue;
                }
            }
            Console.WriteLine($"\n\nRecord with Id: {recordId} was deleted.\n\n");
            return;
        }
    }
    public static void Update()
    {
        Console.Clear();
        GetAllRecords();
        var recordId = ValidationMethods.GetNumberInput("\n\nPlease type the Id of the record you want to update. Press 0 to return.");
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var checkCmd = connection.CreateCommand();
            checkCmd.CommandText = $"SELECT EXISTS (SELECT 1 FROM habit_tracker WHERE Id = @id)";
            checkCmd.Parameters.AddWithValue("@id", recordId);
            int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());
            if (checkQuery == 0)
            {
                Console.WriteLine($"\n\nRecord with Id {recordId} doesnt exist.\n\n");
                connection.Close();
                Update();
            }   
            string date = ValidationMethods.GetDateInput();
            string habit = ValidationMethods.GetHabitInput();
            int quantity = ValidationMethods.GetNumberInput("\n\nPlease insert number of glasses or other measure of choice (no decimals allowed)\n\n");

            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"UPDATE habit_tracker SET date = @date, habit = @habit, quantity = @quantity WHERE Id = @id";
            tableCmd.Parameters.AddWithValue("@id", recordId);
            tableCmd.Parameters.AddWithValue("@date", date);
            tableCmd.Parameters.AddWithValue("@habit", habit);
            tableCmd.Parameters.AddWithValue("@quantity", quantity);
            
            tableCmd.ExecuteNonQuery();
            connection.Close();        
        }
        Console.WriteLine($"Successfuly updated entry with Id: {recordId}");
    }
    public static void DropTable()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = $"DROP TABLE IF EXISTS habit_tracker";
            tableCmd.ExecuteNonQuery();
            connection.Close();
        }
    }
}