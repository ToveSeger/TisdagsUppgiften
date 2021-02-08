using System;
using System.Data;
using System.Diagnostics;

namespace TisdagsUppgiften
{
    class Program
    {
        static void Main(string[] args)
        {
           Databas newDatabase = new Databas();
            newDatabase.Menu();
           // Databas.SQLQuery();          
        }


    }
}
