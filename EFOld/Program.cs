using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;

namespace EFOld
{
    class Program
    {
        static pubsEntities entities = new pubsEntities();

        static void Main(string[] args)
        {

            // normal way
            var animalDogs = entities.AnimalDogs;  // ObjectQuery<AnimalDog>
            var sel = animalDogs.Select(r => new { r.Name, r.Age });
            var count = sel.Count();

            foreach (var r in sel)
                Console.Write(r.Name + "; ");
            Console.WriteLine();
            Console.WriteLine();

            // generic way
            var tableName = "AnimalDog";
            Type t = Type.GetType(Assembly.GetExecutingAssembly().GetName().Name + "." + tableName);  // it should be type AnimalDog

            // how to get ObjectQuery<T> from ObjectContext? Probably by using CreateQuery method.

            var objectSet = EF_Extention.CreateObjectSet(entities, t);  // it should be type ObjectSet<AnimalDog>
            Console.WriteLine(objectSet);

            foreach (var r in objectSet)
            {
                PropertyInfo pi = r.GetType().GetProperty("Name", BindingFlags.Instance | BindingFlags.Public);
                // we don't check pi for null because we know it's there
                string name = Convert.ToString(pi.GetValue(r, null));
                Console.Write(name + "; ");
            }



            Console.WriteLine();
            Console.WriteLine("press any key..");
            Console.ReadKey();
        }
    }
}
