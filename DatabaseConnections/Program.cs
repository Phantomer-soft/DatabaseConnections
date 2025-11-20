using DatabaseConnections;

public static class Program
{
    public static void Main(string[] args)
    {
       DatabaseManager dbManager = new DatabaseManager();

        // dbManager.createTable();
         dbManager.addColumn("employee", "denemee", "VARCHAR(50)",false);
       dbManager.addColumn("employee", "deneme2", "VARCHAR(50)", false);
        dbManager.dropColumn("employee","denemee");

        
    }
}