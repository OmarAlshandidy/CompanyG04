using Company.G04.BLL.Interfaces;
using Company.G04.BLL.Repositries;
using Microsoft.AspNetCore.Mvc;

namespace Company.G04.PL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository _departmentRepository;
        // aSK CLR  Create Object DepartemntRepository 
        public DepartmentController(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
             var departments = _departmentRepository.GetAll();
            return View(departments);
        }
    }
}
