using System;
using System.Data;

namespace TisdagsUppgiften
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Insert SQL command:");
            string command = Console.ReadLine();
            Console.Write("Insert parameter:"); //behövs inte om man skriver allt i insert SQL command
            string parameter = Console.ReadLine();
            DataTable dtb = Databas.RunSQLDT(command, parameter);
            Databas.ShowTable(dtb);
            Console.ReadKey();
        }


    }
}
