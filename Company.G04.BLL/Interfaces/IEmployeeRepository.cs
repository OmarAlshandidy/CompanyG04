using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company.G04.DAL.Moudel;

namespace Company.G04.BLL.Interfaces
{
    public interface IEmployeeRepository:IGenericRepository<Employee>
    {
        List<Employee> GetByName(string name);


    }
}
