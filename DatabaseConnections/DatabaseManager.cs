using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
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
                Console.WriteLine(state.GetState(true,"CreateTable"));// OPERATION SUCCESSFUL                
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
                Console.WriteLine(state.GetState(true,"AddColumn"));
                
            }
            catch (Exception ex)
            {
              state.printState(ex,"AddColumn");
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
                 Console.WriteLine(state.GetState(true,"AddColumn"));

            }
            catch (Exception ex)
            {
                state.printState(ex,"AddColumn");
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
                Console.WriteLine(state.GetState(true,"DropColumn"));
            }

            catch (Exception ex)
            {
                state.printState(ex, "DropColumn");
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
        public String GetState(bool success)
        {
            if (success)            
                return "Operation completed successfully";

            return "An error has been accrued";
        }
        public void printState(Exception ex)
        {
            Console.WriteLine("An error has been accrued : " + ex.Message);
        }

        public String GetState(bool success, [CallerMemberName] string callerName = "")
        {
            if (success)
                return $"{callerName} : Operation completed successfully";

            return $"{callerName} : An error has been accrued";
        }
        public void printState(Exception ex,[CallerMemberName] string callerName="")
        {
            Console.WriteLine($"{callerName}: An error has been accrued : " + ex.Message);
        }

        public bool DataTypeController(string dataType)  // EMİN DEĞİLİM BU GELİŞTİRİLEBİLİR DAHA AZ KARMAŞIK BİR YAPI KURULABİLİR
        {// PROBLEM => TEMEL TİPLER KABUL EDİLİYOR AMA VARCHAR(50) GİBİ BİR DEĞER GELİRSE BU NASIL KABUL EDİLİR 
            dataType = dataType.Trim().ToLower();
            StringBuilder baseTypeBuilder = new StringBuilder(); // BU ÇÖZÜM İŞE YARAYABİLİR 
            foreach (char c in dataType)
            {
                if (c == '(') break;   //(v a r c h a r ) X (50)
                baseTypeBuilder.Append(c);
            }

             dataType = baseTypeBuilder.ToString().Trim(); // DATATYPE İLE İŞİMİZ BİTTİ YENİ HALİ KULLANILABİLİR 

            switch (dataType)
            {
                case "bigint":
                case "binary":
                case "char":
                case "varchar":
                case "nchar":
                case "nvarchar":
                    return true;

                default:return false;
            }
            

        }
    }
   
}
