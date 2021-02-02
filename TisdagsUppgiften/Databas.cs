using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace TisdagsUppgiften
{
    class Databas
    {
        public static string connectionString { get; set; } = @"Data Source =.\SQLExpress; Integrated Security = true; database = {0}";

        public static string databaseName { get; set; } = "Population";


      /*  Skapa en metod som tar emot SQL string och parametrar och som exekverar din SQL kod och
            returnerar ingenting(ExecuteNonQuery)
            2. Skapa en metod som tar emot SQL string och parametrar och som exekverar din SQL kod och
            returnerar en DataTable*/
        public static long RunSQL(string s, string p)
        {
            string db = string.Format(connectionString, databaseName);
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(s, conn);
                cmd.Parameters.AddWithValue("@input", p);
                long rows = cmd.ExecuteNonQuery();

                return rows;
                //conn.Close();
            }
        }

        public static DataTable RunSQLDT(string s, string p)
        {
            string db = string.Format(connectionString, databaseName);
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(s, conn);
                cmd.Parameters.AddWithValue("@input", p);
                long rows = cmd.ExecuteNonQuery();

                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
                //conn.Close();
            }
        }

        public static void ShowTable(DataTable dtb)
        {
            foreach (DataRow row in dtb.Rows)
            {
                Console.WriteLine($"{row["id"]} {row["name"]} {row["age"]} ");
            }
        }
    }
}
