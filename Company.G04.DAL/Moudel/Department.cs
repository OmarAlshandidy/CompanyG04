﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.G04.DAL.Moudel
{
    public class Department :BaseEntity
    {
        public string Code{ get; set; }
       public string Name { get; set; }
        public DateTime CreateAt { get; set; }
        public List<Employee>? Employees { get; set; }
    }

}
