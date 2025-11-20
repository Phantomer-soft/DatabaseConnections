using DatabaseConnections;

public static class Program
{
    public static void Main(string[] args)
    {
       DatabaseManager dbManager = new DatabaseManager();

        //dbManager.createTable("DenemeTablo", "denemekolon", "int", true);
        //dbManager.addColumn("DenemeTablo", "denemekolon6", "varchar", false,"null");
        //dbManager.addColumn("DenemeTablo", "denemekolon7", "float", false,"not null");
        //dbManager.addColumn("DenemeTablo", "denemekolon8", "datetime", false,"not");
        //dbManager.addColumn("DenemeTablo", "denemekolon9", "float", false);
        dbManager.updateColumnType("denemetablo", "deneme", "int");
    }
}