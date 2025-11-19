using DatabaseConnections;

public static class Program
{
    public static void Main(string[] args)
    {
       DatabaseManager dbManager = new DatabaseManager();
         dbManager.createTable();
    }
}