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
    public static class EF_Extention
    {
        public static IQueryable CreateObjectSet(this ObjectContext context, Type T)
        {
              // let's find proper methods because ambigiuty is possible 
            // {System.Data.Entity.DbSet`1[TEntity] Set[TEntity]()}
            var methods = typeof(ObjectContext).GetMethods().Where(p => p.Name == "CreateObjectSet");
            Console.WriteLine(methods);

            MethodInfo method = typeof(ObjectContext).GetMethods()
                .Where(p => p.Name == "CreateObjectSet" && p.ContainsGenericParameters).FirstOrDefault();

            // Build a method with the specific type argument you're interested in
            method = method.MakeGenericMethod(T);

            return method.Invoke(context, null) as IQueryable;
        }

        public static IQueryable<T> Set<T>(this ObjectContext context)
        {
            //MethodInfo method = typeof(ObjectContext).GetMethod(nameof(ObjectContext.Set), BindingFlags.Public | BindingFlags.Instance);
            MethodInfo method = typeof(ObjectContext).GetMethods()
                .Where(p => p.Name == "CreateObjectSet" && p.ContainsGenericParameters).FirstOrDefault();

            // Build a method with the specific type argument you're interested in 
            method = method.MakeGenericMethod(typeof(T));

            return method.Invoke(context, null) as IQueryable<T>;
        }

    }
}
