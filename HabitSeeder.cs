  using Bogus;
  using Microsoft.Data.Sqlite;
  
  namespace habit_tracker;  
    public class HabitSeeder
{
    private readonly string _connectionString;
    private readonly Faker _faker;
    
    public HabitSeeder(string connectionString)
    {
        _connectionString = connectionString;
        _faker = new Faker();
    }
    public void SeedData(int rowCount)
    {
        var habitFaker = new Faker<HabitTracker>()
            .RuleFor(h => h.Date, f => f.Date.Past(1))
            .RuleFor(h => h.Habit, f => f.PickRandom(new[] {
                "Exercise", "Reading", "Meditation", "Water",
                "Sleep Tracking", "Stretching", "Studying", "Walking"
            }))
            .RuleFor(h => h.Quantity, f => f.Random.Int(1, 10));

        var habits = habitFaker.Generate(rowCount);

        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var readerCmd = connection.CreateCommand();
        readerCmd.CommandText=
        $"SELECT * FROM habit_tracker";
        SqliteDataReader reader = readerCmd.ExecuteReader();
        if (reader.HasRows)
        {
           return;
        }
        
        var tableCmd = connection.CreateCommand();  
        tableCmd.CommandText = @"
        INSERT INTO habit_tracker (Date,Habit,Quantity)
        VALUES (@date, @habit, @quantity)";

        foreach (var habit in habits)
        {
            tableCmd.Parameters.Clear();
            tableCmd.Parameters.AddWithValue("@date", habit.Date.ToString("dd-MM-yy"));
            tableCmd.Parameters.AddWithValue("@habit", habit.Habit);
            tableCmd.Parameters.AddWithValue("@quantity", habit.Quantity);
            tableCmd.ExecuteNonQuery();
        }
    }
}