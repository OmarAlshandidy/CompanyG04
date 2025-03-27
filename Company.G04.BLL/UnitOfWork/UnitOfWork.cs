using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company.G04.BLL.Interfaces;
using Company.G04.BLL.Repositries;
using Company.G04.DAL.Data.Context;

namespace Company.G04.BLL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public IDepartmentRepository DepartmentRepository { get; }
        public IEmployeeRepository EmployeeRepository { get; }
        private readonly CompanyDbContext _context;

       

        public UnitOfWork(CompanyDbContext context) 
        {
            _context = context;
            DepartmentRepository = new DepartmentRepository(_context);
            EmployeeRepository = new EmployeeRepository(_context);  
        }


        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync();
        }
    }
}
