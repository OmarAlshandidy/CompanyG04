using Company.G04.BLL.Interfaces;
using Company.G04.BLL.Repositries;
using Company.G04.DAL.Moudel;
using Company.G04.PL.Dtos;
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
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(CreateDepartmentDto model)
        {
            if (ModelState.IsValid)
            {
                var departments = new Department()
                {
                    Code = model.Code,
                    Name = model.Name,
                    CreateAt = model.CreateAt
                };
                var count = _departmentRepository.Add(departments);
                if(count > 0)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }
    }
}
