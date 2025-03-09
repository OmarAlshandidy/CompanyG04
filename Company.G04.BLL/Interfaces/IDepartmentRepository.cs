using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company.G04.DAL.Moudel;

namespace Company.G04.BLL.Interfaces
{
    public interface IDepartmentRepository
    {
        IEnumerable<Department> GetAll();

        Department Get(int id); 

        int Add(Department Model);

        int Update(Department Model);
        int  Delete(Department Model);
    }
}
