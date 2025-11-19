using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
namespace DatabaseConnections
{
    public class DatabaseManager
    {
        AlterTable alterTable = new AlterTable();

        public void createTable(string tableName, string columnName, string dataType, bool primaryKey)=> 
            alterTable.CreateTable(tableName,columnName,dataType,primaryKey);
        public void addColumn(string tableName, string columnName, string dataType, bool primaryKey) => 
            alterTable.AddColumn(tableName,columnName,dataType,primaryKey);
        public void addColumn(string tableName, string columnName, string dataType, bool primaryKey,string nullAble) => 
            alterTable.AddColumn(tableName, columnName, dataType, primaryKey,nullAble);
        public void dropColumn(string tableName,string columnName) => 
            alterTable.DropColumn(tableName,columnName);

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
        public void CreateTable(string tableName, string columnName, string dataType, bool primaryKey)
        {
            tableName= tableName.Trim().ToUpper();
            columnName= columnName.Trim().ToUpper();
            dataType= dataType.Trim().ToUpper();
            switch (primaryKey)
                {
                    case true:
                        dataType += " PRIMARY KEY";
                        break;
                    case false:
                        break;
                }//PRİMARY KEY KONTROLU VARSAYILAN FALSE - Primary key check
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
                state.printState(ex);               

            }


        }
        public void AddColumn(string tableName,string columnName, string dataType,bool primaryKey)
        {        // NULLABLE KONTROLÜ YOK
            
            tableName = tableName.Trim().ToUpper();
            columnName = columnName.Trim().ToUpper();
            dataType = dataType.Trim().ToUpper();
            try
            {
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = $"ALTER TABLE {tableName} ADD {columnName} {dataType}";// SQL INJECTION VULNERABILITY               
                cmd.ExecuteNonQuery();
                state.GetState(true);
                
            }
            catch (Exception ex)
            {
              state.printState(ex);
            }
            finally {con.Close(); }



        }//NULLABLE
        public void AddColumn(string tableName, string columnName, string dataType, bool primaryKey, string nullAble)
        {       // NULLABLE KONTROLÜ VAR
            nullAble = nullAble.ToUpper();
            if (nullAble.StartsWith("NOT")) { nullAble = "NOT NULL"; } //NOT İLE BAŞLAMASI YETERLİ NULL OLMAMASI İÇİN
            else { nullAble = ""; }//NULL OLABİLİR
            tableName = tableName.Trim().ToUpper();
            columnName = columnName.Trim().ToUpper();
            dataType = dataType.Trim().ToUpper();
            try
            {
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = $"ALTER TABLE {tableName} ADD {columnName} {dataType} {nullAble}";// SQL INJECTION VULNERABILITY               
                cmd.ExecuteNonQuery();
                state.GetState(true);

            }
            catch (Exception ex)
            {
                state.printState(ex);
            }
            finally { con.Close(); }



        }//CAN USE FOR NOT NULL
        public void DropColumn(string tableName,string columnName)
        {
            tableName = tableName.Trim().ToUpper();
            columnName = columnName.Trim().ToUpper();
            try
            {
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = $"ALTER TABLE {tableName} DROP COLUMN {columnName}";
                cmd.Parameters.AddWithValue("@",columnName);
                cmd.ExecuteNonQuery();
                state.GetState(true);
            }

            catch (Exception ex)
            {
                state.printState(ex);
            }
            finally { con.Close(); }

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

            return"An error has been occured";
        }
        public void printState(Exception ex)
        {
            Console.WriteLine("An error has been occured : "+ex.Message);
        }

        public bool DataTypeController(string dataType)  // EMİN DEĞİLİM BU GELİŞTİRİLEBİLİR DAHA AZ KARMAŞIK BİR YAPI KURULABİLİR
        {// PROBLEM => TEMEL TİPLER KABUL EDİLİYOR AMA VARCHAR(50) GİBİ BİR DEĞER GELİRSE BU NASIL KABUL EDİLİR 
            dataType = dataType.Trim().ToLower();
            for(int i = 0; i < dataType.Length;)
            {
                char c = dataType[i];
                if(c == '(')
                {

                }
            }

            switch (dataType)
            {
                case "bigint":
                case "binary":
                    return true;

                default:return false;
            }
            

        }
    }
   
}
