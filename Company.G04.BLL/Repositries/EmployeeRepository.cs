using Company.G04.BLL.Interfaces;
using Company.G04.DAL.Data.Context;
using Company.G04.DAL.Moudel;
using Microsoft.EntityFrameworkCore;

namespace Company.G04.BLL.Repositries
{
    public class EmployeeRepository : GenericRepository<Employee>,IEmployeeRepository
    {
        private readonly CompanyDbContext _context;

        public EmployeeRepository(CompanyDbContext context) : base(context) 
        {
           _context = context;
        }

        public async Task<List<Employee>> GetByNameAsync(string name)
        {
            return await _context.Employees.Include(D => D.Department).Where(E => E.Name.ToLower().Contains(name.ToLower())).ToListAsync();
        }
    }
}
