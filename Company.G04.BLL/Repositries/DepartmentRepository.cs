﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company.G04.BLL.Interfaces;
using Company.G04.DAL.Data.Context;
using Company.G04.DAL.Moudel;

namespace Company.G04.BLL.Repositries
{
    internal class DepartmentRepository : IDepartmentRepository
    {
        private readonly CompanyDbContext _context; //null 
        DepartmentRepository()
        {
            _context = new CompanyDbContext();
        }
        public IEnumerable<Department> GetAll()
        {
            return _context.Departments.ToList();
        }

        public Department? Get(int id)
        {
            return _context.Departments.Find(id);
        }
        public int Add(Department Model)
        {
             _context.Departments.Add(Model);
            return _context.SaveChanges();
 
        }

        public int Update(Department Model)
        {
            _context.Departments.Update(Model);
            return _context.SaveChanges();  
        }

        public int Delete(Department Model)
        {
            _context.Departments.Remove(Model);
            return _context.SaveChanges();
        }

     

      
    }
}
