using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company.G04.BLL.Interfaces;

namespace Company.G04.BLL.UnitOfWork
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        public IDepartmentRepository DepartmentRepository { get; }

        public IEmployeeRepository EmployeeRepository { get; }

        Task<int> CompleteAsync();
    }
}
