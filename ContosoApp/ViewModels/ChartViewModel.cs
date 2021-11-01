using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyCauDuong.ViewModels
{
    public class ChartViewModel
    {
        public class Person
        {
            public string Name { get; set; }

            public double Height { get; set; }
        }

        public List<Person> Data { get; set; }

        public ChartViewModel()
        {
            Data = new List<Person>()
            {
                new Person { Name = "David", Height = 180 },
                new Person { Name = "Michael", Height = 170 },
                new Person { Name = "Steve", Height = 160 },
                new Person { Name = "Joel", Height = 182 }
            };
        }
    }
}
