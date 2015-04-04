using System;
using System.Collections.Generic;
using System.Linq;
using KladrTask.Domain.Concrete;
using KladrTask.Domain.Entities;

namespace KladrTask.Domain
{
    class Program
    {
        public static void Main(string[] args)
        {
            var rep = new DbKladrContext();
            var houses = rep.Houses.ToList();

            foreach (var house in houses)
            {
                var name = house.Name.Split(',').ToList();
                house.Name = name[0];
                for (var i = 1; i < name.Count; ++i)
                {
                    rep.Houses.Add(new House() { Code = house.Code, Name = name[i], Index = house.Index });
                }
                    Console.WriteLine("yes");
            }

            rep.SaveChanges();
        }
    }
}
