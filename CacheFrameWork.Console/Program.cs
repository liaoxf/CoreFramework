using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CacheFrameWork.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var redis = RedisHelper.Instance;

            redis.DefaultDb = 1;

            redis.Install();
    
            var persons = new List<Person>();

            for (int i = 1; i <= 5000; i++)
            {
                persons.Add(new Person(i.ToString().PadLeft(6, '0'), string.Format("{0}{1}", "jienny", i), i));
            }

            redis.AddList<Person>("DictPerson", persons);

            var result = redis.GetList<Person>("DictPerson");                                                                                                                                                                                                                                                                                                                                                                                                                                                                         
            
            Trace.WriteLine("历史读取Redis:" + result.Count());

            redis.DefaultDb = 2;

            redis.Install();

            redis.Set<Person>("person", new Person("1", string.Format("{0}{1}", "jienny", 1), 1), 0);


            var current = redis.Get<Person>("person");

            Trace.WriteLine("最新读取Redis:" + JsonConvert.SerializeObject(current));

        }
    }

    class Person
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public int Age { get; set; }

        public string Birth { get; set; }

        public Person(string id,string name, int age)
        {
            this.Id = id;
            this.Name = name;
            this.Age = age;
            this.Birth = DateTime.Now.ToString();
        }
    }
}
