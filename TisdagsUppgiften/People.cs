using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Reflection.Metadata;
using System.Data;
using System.Reflection;
using System.ComponentModel.Design;
using System.Diagnostics;

namespace TisdagsUppgiften
{
    class People : Person
    {
        //public static string connectionString { get; set; } = @"Data Source =.\SQLExpress; Integrated Security = true; database = {0}";

        public static string databaseName { get; set; } = "Population"; // CHANGE THIS IF YOU WANT TO ACCESS ANOTHER DATABASE 

        
        static List<Person> peopleList = new List<Person>();

        public void Menu()
        {
            Console.WriteLine($"Hello there, what do you want to do in the {databaseName} database?");
            Console.WriteLine("1. Create a person");
            Console.WriteLine("2. Search for a specific name");
            Console.WriteLine("3. Update a specific person");
            Console.WriteLine("4. Delete a specific person");
            Console.WriteLine("5. Take a look at all the people I've created");
            int menuChoice = Convert.ToInt32(Console.ReadLine());

            switch (menuChoice)
            {
                case 1:
                    Console.WriteLine("Enter below parameters to create a new person:");
                    Console.Write("Last name:");
                    string lastName = Console.ReadLine();
                    Console.Write("Fist name:");
                    string firstName = Console.ReadLine();
                    Console.Write("Address:");
                    string address = Console.ReadLine();
                    Console.Write("City:");
                    string city = Console.ReadLine();
                    Console.Write("Age:");
                    int age = Convert.ToInt32(Console.ReadLine());
                    Person newPerson = new Person(lastName, firstName, address, city, age);
                    CreatePerson(newPerson);
                    peopleList.Add(newPerson);
                    break;

                case 2:
                    Console.WriteLine("Write the name of who you're looking for");
                    var person = Console.ReadLine();
                    ReadPerson(person);                                   
                    break;

                case 3:
                    Console.WriteLine("Write the first and last name of the person you want to update");
                    person = Console.ReadLine();
                    Person foundPerson = ReadPerson(person);
                    UpdatePerson(foundPerson);
                    break;

                case 4:
                    Console.WriteLine("Write the first and last name of the person you want to delete");
                    person = Console.ReadLine();
                    Person personToDelete = ReadPerson(person);
                    DeletePerson(personToDelete);
                    break;

                case 5:
                    GetPeopleList();
                    break;

                default:
                    break;
            }
        }


        public void CreatePerson(Person person)
        {
            var db = new Databas();
            try
            {
                string connection = string.Format(db.connectionString, databaseName);
                using (var cnn = new SqlConnection(connection))
                {
                    cnn.Open();
                    var query = "INSERT INTO People VALUES(@LastName, @FirstName, @Address, @City, @Age)";
                    using(var command = new SqlCommand(query, cnn))
                    {
                        command.Parameters.AddWithValue("@LastName", person.lastName);
                        command.Parameters.AddWithValue("@FirstName", person.firstName);
                        command.Parameters.AddWithValue("@Address", person.address);
                        command.Parameters.AddWithValue("@City", person.city);
                        command.Parameters.AddWithValue("@Age", person.age);
                        command.ExecuteNonQuery();
                    }
                }
                Console.WriteLine($"{person.firstName} {person.lastName} created successfully!");
            }
            catch (Exception)
            {

                Console.WriteLine("Could not do that for you I'm afraid..."); 
            }
            

        }

        public Person ReadPerson(string name)
        {
            var db = new Databas
            {
                databaseName = databaseName
            };

            DataTable dt;
            if (name.Contains(" "))
            {
                var names = name.Split(' ');
                dt = db.GetDataTable("SELECT TOP 1 * from People Where firstName LIKE @fname AND lastName LIKE @lname",
                    ("@fname", names[0]),
                    ("@lname", names[^1])
                    );
            }
            else
            {
                dt = db.GetDataTable("SELECT TOP 1 * from People Where firstName LIKE @name OR lastName LIKE @name ", ("@name", name));
            }

            if (dt.Rows.Count == 0)
                return null;

           Person personObject = GetPersonObject(dt.Rows[0]);
           Print(personObject);
           return personObject;
        }

        public static Person GetPersonObject(DataRow row)
        {
            Person newPerson = new Person
            {
                address = row["Address"].ToString(),
                age = (int)row["Age"],
                city = row["City"].ToString(),
                firstName = row["FirstName"].ToString(),
                lastName = row["LastName"].ToString(),
                ID = (int)row["id"]
            };            
            return newPerson;
        } 
        public void UpdatePerson(Person person)
        {
            var db = new Databas
            {
                databaseName = databaseName
            };
         
            Console.WriteLine("What do you want to change?");
            Console.WriteLine("1. LastName");
            Console.WriteLine("2. FirstName");
            Console.WriteLine("3. Address");
            Console.WriteLine("4. City");
            Console.WriteLine("5. Age");
            int answer = Convert.ToInt32(Console.ReadLine());

            switch (answer)
            {

                case 1:
                    Console.WriteLine("Enter new last name:");
                    person.lastName = Console.ReadLine();                    
                    db.RunSQL("UPDATE PEOPLE SET lastName=@LastName WHERE ID = @Id", ("@LastName", person.lastName), ("@Id", person.ID.ToString()));
                    Console.WriteLine("Last name updated successfully");
                    break;
                case 2:
                    Console.WriteLine("Enter new first name:");
                    person.firstName = Console.ReadLine();
                    db.RunSQL(@"UPDATE PEOPLE SET firstName=@firstName WHERE Id = @id", ("@firstName", person.firstName), ("@Id", person.ID.ToString()));
                    Console.WriteLine("First name updated successfully");
                    break;
                case 3:
                    Console.WriteLine("Enter new address:");
                    person.address = Console.ReadLine();
                    db.RunSQL(@"UPDATE PEOPLE SET address=@address WHERE Id = @id", ("@address", person.address), ("@Id", person.ID.ToString()));
                    Console.WriteLine("Address updated successfully");
                    break;
                case 4:                  
                    Console.WriteLine("Enter new City:");
                    person.city = Console.ReadLine();
                    db.RunSQL(@"UPDATE PEOPLE SET city=@city WHERE Id = @id", ("@city", person.city), ("@Id", person.ID.ToString()));
                    Console.WriteLine("City updated successfully");
                    break;
                case 5:
                    Console.WriteLine("Enter new age:");
                    person.age = Convert.ToInt32(Console.ReadLine());
                    db.RunSQL(@"UPDATE PEOPLE SET age=@age WHERE Id = @id", ("@age", person.age.ToString()), ("@Id", person.ID.ToString()));
                    Console.WriteLine("Age updated successfully");
                    break;
                   
                default:

                    Console.WriteLine("Invalid option");
                    break;
            }

           
        }

        public void DeletePerson(Person person)
        {
            var db = new Databas
            {
                databaseName = databaseName
            };
            db.RunSQL("DELETE PEOPLE WHERE ID = @Id", ("@Id", person.ID.ToString()));
            Console.WriteLine("Person deleted successfully");

        }

        private static void Print(Person person)
        {
            if (person != null)
                Console.WriteLine($"{person.firstName} {person.lastName}, är {person.age} år och bor på {person.address}, {person.city}. Id is {person.ID}");
            else
                Console.WriteLine("Person not found");
        }

        private void GetPeopleList() // EJ FÄRDIG
        {
            foreach (People item in peopleList)
            {
                Console.WriteLine(person.firstName);
            }
            
        }
    }
}
