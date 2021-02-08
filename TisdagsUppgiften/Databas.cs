using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace TisdagsUppgiften
{
    class Databas
    {
       internal string connectionString { get; set; } = @"Data Source =.\SQLExpress; Integrated Security = true; database = {0}";

        internal string databaseName { get; set; } = "Humans"; // CHANGE THIS IF YOU WANT TO ACCESS ANOTHER DATABASE OR USE MENU CHOICE 6 

        public void Menu()
        {
            Console.WriteLine("Hi, what do you want to do?");
            Console.WriteLine("1. Create a database");
            Console.WriteLine("2. Create a table in an existing database");
            Console.WriteLine("3. Add a field in an existing table" );
            Console.WriteLine("4.Delete a database");
            Console.WriteLine( "5. Delete a table");
            Console.WriteLine("6. Check which database I'm currently connected to");
            Console.WriteLine("7. I want to go to population database to play with people instead");
            int answer = Convert.ToInt32(Console.ReadLine());

            switch (answer)
            {
                case 1:
                    Console.WriteLine("What do you want to call your database?");
                    string dbName = Console.ReadLine();
                    CreateDatabase(dbName);
                    break;
                case 2:
                    Console.WriteLine($"You're currently in database: {databaseName} and the table will be added there");
                    Console.WriteLine("What do you want to call your table?");
                    string tableName = Console.ReadLine();
                    Console.WriteLine("And what field do you want to add?");
                    string fieldName = Console.ReadLine();
                    Console.WriteLine("What type of variable is it?");
                    string variable = Console.ReadLine();

                    string field = $"{fieldName} {variable}";
                    if (variable != "int")
                    {
                        Console.WriteLine("Max amount of characters:");
                        int maxChar = Convert.ToInt32(Console.ReadLine());
                         field = $"{fieldName} {variable} ({maxChar})";
                    }                   
                    CreateTable(tableName, field);

                    break;
                case 3:
                    Console.WriteLine("Which table do you want to add a field to?");
                    tableName = Console.ReadLine();
                    Console.WriteLine("And which field do you want to add?");
                    fieldName = Console.ReadLine();
                   
                    Console.WriteLine("What type of variable is it?");
                    variable = Console.ReadLine();

                    field = $"{fieldName} {variable}";
                    if (variable != "int")
                    {
                        Console.WriteLine("Max amount of characters:");
                        int maxChar = Convert.ToInt32(Console.ReadLine());
                        field = $"{fieldName} {variable} ({maxChar})";
                    }
                    AlterTable(tableName, field);
                    break;
                case 4:
                    Console.WriteLine("Which database do you want to delete?");
                    string database = Console.ReadLine();
                    DropDatabase(database);
                    break;
                case 5:
                    Console.WriteLine("Which table do you want to delete?");
                    tableName = Console.ReadLine();
                    DropTable(tableName);
                    break;
                case 6:
                    CheckConnection();
                    break;

                case 7:
                    People newPeople = new People();
                    newPeople.Menu();
                    break;
                default:
                    Console.WriteLine("Non existing option, please choose a number between 1-5");
                    break;
            }
        }

        public void SQLQuery()         
        {
            var db = new Databas();
            Console.Write("Insert SQL command:");
            string command = Console.ReadLine();
            Console.Write("Insert parameter:");  //behövs inte om man skriver allt i insert SQL command 
            string parameter = Console.ReadLine();
            /*DataTable dtb = db.RunSQLDT(command, parameter);
            Databas.ShowTable(dtb);*/
            //Console.ReadKey();
        }

        /*  Skapa en metod som tar emot SQL string och parametrar och som exekverar din SQL kod och
              returnerar ingenting(ExecuteNonQuery)
              2. Skapa en metod som tar emot SQL string och parametrar och som exekverar din SQL kod och
              returnerar en DataTable*/
        internal long RunSQL(string sqlString, params(string, string)[] parameters)
        {
            string db = string.Format(connectionString, databaseName);
            using (var conn = new SqlConnection(db))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sqlString, conn);
                SetParameters(parameters, cmd);// cmd.Parameters.AddWithValue("@input", parameters);
                long rows = cmd.ExecuteNonQuery(); 

                return rows; 
                //conn.Close();
            }
        }

        public DataTable RunSQLDT (string s, string p) 
        {
            string db = string.Format(connectionString, databaseName);
            using (var conn = new SqlConnection(db))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(s, conn); 
                cmd.Parameters.AddWithValue("@input", p); 
                //long rows = cmd.ExecuteNonQuery();

                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
                //conn.Close();
            }
        }

        public static void ShowTable(DataTable dtb)
        {
            //för sys.databases
            foreach (DataRow row in dtb.Rows)
            {
                Console.WriteLine($"Name: {row["Name"]} ");
            }
            

            //för sys.database_files
            /* foreach (DataRow row in dtb.Rows)
             {
                 Console.WriteLine($"Physical name: {row["physical_name"]} Size: {row["size"]} ");
             }*/


            /* try
            {   //för dbo.person
                foreach (DataRow row in dtb.Rows)
                {
                    Console.WriteLine($"ID: {row["id"]} NAME: {row["name"]} AGE: {row["age"]} ");
                }
            }
            catch (Exception)
            {

                Console.WriteLine("Sorry, can't help you with that...");
                SQLQuery();
            }*/


        }

        public long CreateDatabase(string database)
        {
            string create = $"CREATE DATABASE {database}";
            string inputString = string.Format(connectionString, databaseName);
            using (SqlConnection myConn = new SqlConnection(inputString))
            {
                myConn.Open();
                SqlCommand cmd = new SqlCommand(create, myConn);
                cmd.Parameters.AddWithValue("@input", System.DBNull.Value);
                long rows = cmd.ExecuteNonQuery();
                Console.WriteLine("There you go - did that just for you!");
                return rows;
            }
            
        }

        public long CreateTable(string table, string field)//gör denna mer allround för att ta emot flera fält på en gång
        {            
            string create = $"CREATE TABLE {table}({field})";
            string inputString = string.Format(connectionString, databaseName);
            using (SqlConnection myConn = new SqlConnection(inputString))
            {
                myConn.Open();
                SqlCommand cmd = new SqlCommand(create, myConn);
                cmd.Parameters.AddWithValue("@input", System.DBNull.Value);
                long rows = cmd.ExecuteNonQuery();
                Console.WriteLine("There you go - did that just for you!");
                return rows;
            }
            
        }
      
        public long AlterTable(string table, string field)
        {
            string alter = $"ALTER TABLE {table} ADD {field}";
            string inputString = string.Format(connectionString, databaseName);
            using (SqlConnection myConn = new SqlConnection(inputString))
            {
                myConn.Open();
                SqlCommand cmd = new SqlCommand(alter, myConn);
                cmd.Parameters.AddWithValue("@input", System.DBNull.Value);                
                long rows = cmd.ExecuteNonQuery();
                Console.WriteLine("There you go - did that just for you!");
                return rows;
            }
        }

        public long DropDatabase(string database)
        {
            string drop = $"DROP DATABASE {database}";
            string inputString = string.Format(connectionString, databaseName);
            using (SqlConnection myConn = new SqlConnection(inputString))
            {
                myConn.Open();
                SqlCommand cmd = new SqlCommand(drop, myConn);
                cmd.Parameters.AddWithValue("@input", System.DBNull.Value);
                long rows = cmd.ExecuteNonQuery();
                Console.WriteLine("There you go - did that just for you!");
                return rows;
            }

        }

        public long DropTable(string table)
        {
            string drop = $"DROP TABLE {table}";
            string inputString = string.Format(connectionString, databaseName);
            using (SqlConnection myConn = new SqlConnection(inputString))
            {
                myConn.Open();
                SqlCommand cmd = new SqlCommand(drop, myConn);
                cmd.Parameters.AddWithValue("@input", System.DBNull.Value);
                long rows = cmd.ExecuteNonQuery();
                Console.WriteLine("There you go - did that just for you!");
                return rows;
            }
        }

        public string CheckConnection()
        {
            Console.WriteLine($"You're currently in database: {databaseName}");
            Console.WriteLine("Want to change to another one?");
            Console.WriteLine("1. Yes, I want to change");
            Console.WriteLine("2. Nah, that's fine");
            int answer = Convert.ToInt32(Console.ReadLine());
            switch (answer)
            {
                case 1:
                    Console.WriteLine("Which database do you want to go to?");
                    string database = Console.ReadLine();
                    databaseName = database;
                    Console.WriteLine("There you go - did that just for you!");
                    Console.WriteLine($"You're now in {databaseName}");
                    break;                                      

                default:
                    break;
            }
            
            Menu();
            return databaseName;
        }

        private static void SetParameters((string, string)[] parameters, SqlCommand command)
        {
            foreach (var item in parameters)
            {
                command.Parameters.AddWithValue(item.Item1, item.Item2);
            }
        }
        internal DataTable GetDataTable(string sqlString, params (string, string)[] parameters)
        {
            var dt = new DataTable();
            var connString = string.Format(connectionString, databaseName);

            try
            {
                using (var cnn = new SqlConnection(connString))
                {
                    cnn.Open();
                    using (var command = new SqlCommand (sqlString, cnn))
                    {
                        SetParameters(parameters, command);

                        try
                        {
                            using (var adapter = new SqlDataAdapter(command))
                            {
                                adapter.Fill(dt);
                            }

                        }
                        catch (Exception)
                        {

                            Console.WriteLine("Could not fill");
                        }
                    }
                }
            }
            catch (Exception)
            {

                Console.WriteLine("Could not fill 2");
            }

            return dt;
        }
    }
}
