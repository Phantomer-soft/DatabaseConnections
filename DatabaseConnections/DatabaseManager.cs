using System.Data.SqlClient;
using System.Data;
using System.Runtime.InteropServices;
namespace DatabaseConnections
{
    public class DatabaseManager
    {
        AlterTable alterTable = new AlterTable();

        public void createTable() =>alterTable.createTable("Employee", "EmployeeID", "INT", true);
    }

    public class DatabaseConnection
    {
        
        public static SqlConnection con = new SqlConnection("Server=(local)\\sqlexpress;initial catalog=DATA;trusted_connection=True");
            


    }
    public class AlterTable 
    {
        public AlterTable() { }

        State state = new State();

        SqlCommand cmd = new SqlCommand();
        SqlConnection con = DatabaseConnection.con;
        public void createTable(string tableName, string columnName, string dataType, bool primaryKey)
        {
                switch (primaryKey)
                {
                    case true:
                        dataType += " PRIMARY KEY";
                        break;
                    case false:
                        break;
                }
            try 
            {
                con.Open();                
                cmd.Connection = con;
                cmd.CommandText = $"CREATE TABLE {tableName} ({columnName} {dataType})";// SQL INJECTION VULNERABILITY
                cmd.Parameters.AddWithValue("@" + columnName, dataType);// SQL INJECTION PROTECTION
                cmd.ExecuteNonQuery();
                cmd.StatementCompleted += (sender, e) =>Console.WriteLine(state.GetState(true));// OPERATION SUCCESSFUL                
                con.Close();


            }
            catch (Exception ex)
            {
                Console.WriteLine(state.GetState(false)+ "\t"+ ex.Message);
                
            }


        }
        public void AddColumn(string tableName,string columnName, string dataType,bool primaryKey)
        {
           con.Open();

        }
        public void DropColumn(string columnName)
        {
            // Implementation for dropping a column
        }
        public void RenameColumn(string oldName, string newName)
        {
            // Implementation for renaming a column
        }
        public void UpdateColumnType(string columnName, string newDataType)
        {
            // Implementation for updating a column type
        }
    }

    public class CRUDE
    {
        public void Create()
        {
            // Implementation for Create operation
        }
        public void Read()
        {
            // Implementation for Read operation
        }
        public void Update()
        {
            // Implementation for Update operation
        }
        public void Delete()
        {
            // Implementation for Delete operation
        }
    }
    public class State
    {
        bool success = false;
        public String GetState(bool success)
        {
            if (success)            
                return "Operation completed successfully";
            
            else           
                return "Operation failed";           
            
        }      
    }
}
