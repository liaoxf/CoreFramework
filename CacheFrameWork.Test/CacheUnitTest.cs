using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Newtonsoft.Json;

namespace CacheFrameWork.Test
{
    [TestClass]
    public class CacheUnitTest
    {
        private RedisHelper redis = RedisHelper.Instance;

        [TestMethod]
        public void Add() {

            redis.DefaultDb = 1;

            redis.Install();

            var person = new Person(1, string.Format("{0}{1}", "jienny", 1), 1);

            redis.Set("person", person, 0);

            var result = redis.Get<Person>("person");

            Trace.WriteLine(result);
        }
        [TestMethod]
        public void AddList()
        {
            redis.DefaultDb = 2;

            redis.Install();

            //redis.RemoveEntityFromList<Person>("DictPerson");

            redis.RemoveEntityFromList<Person>("DictPerson", f => f.Id >= 10);

            var count = redis.GetList<Person>("DictPerson").Count();

            Trace.WriteLine(count);


            var persons = new List<Person>();

            for (int i = 1; i <= 20; i++)
            {
                persons.Add(new Person(i, string.Format("{0}{1}", "jienny", i), i));
            }
            

            redis.AddList("DictPerson", persons);

            //for (int i = 1001; i <= 6000; i++)
            //{
            //    var item=new Person(i.ToString().PadLeft(6, '0'), string.Format("{0}{1}", "jienny", i), i);

            //    redis.AddEntityToList("DictPerson", item, 0);
            //}
            
            var result = redis.GetList<Person>("DictPerson").ToList();

            Trace.WriteLine(result.Count());
        }
    }

    class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int Age { get; set; }

        public string Birth { get; set; }

        public Person(int id, string name, int age)
        {
            this.Id = id;
            this.Name = name;
            this.Age = age;
            this.Birth = DateTime.Now.ToString();
        }
    }
}
