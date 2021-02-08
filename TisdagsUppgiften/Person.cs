using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace TisdagsUppgiften
{
    class Person
    {
        public int ID  { get; set; }
        public string lastName { get; set; }
        public string firstName  { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public int age { get; set; }
        public Person(string lastName, string firstName, string address, string city, int age)
        {       
            this.lastName = lastName;
            this.firstName = firstName;
            this.address = address;
            this.city = city;
            this.age = age;
        }

        public Person()
        {

        }

    }
}
