using System;
using Example;

namespace Test
{
    class Program
    {

        // example

        static void Main(string[] args)
        {
            
            var customer = new Customer
            {
                Name = "Harun",
                CreatedOn = DateTime.UtcNow
            };
             
            MongoCrud mongo = new MongoCrud();
            mongo.Insert(customer);

            Console.ReadLine();
        }
    }
}
