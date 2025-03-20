using AutoMapper;
using Company.G04.BLL.Interfaces;
using Company.G04.BLL.Repositries;
using Company.G04.DAL.Data.Context;
using Company.G04.DAL.Data.Migrations;
using Company.G04.DAL.Moudel;
using Company.G04.PL.Dtos;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace Company.G04.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public EmployeeController(IEmployeeRepository employeeRepository
            ,IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }
        [HttpGet]
        public ActionResult Index()
        {
            var employees = _employeeRepository.GetAll();
            return View(employees);
        }
        [HttpGet]

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateEmployeeDto model)
        {
            if (ModelState.IsValid)
            {
                #region Mapping
                #region  Manaul Mapping
                //var employee = new Employee()
                //{

                //    Name = model.Name,
                //    Age = model.Age,
                //    Address = model.Address,
                //    Email = model.Email,
                //    Phone = model.Phone,
                //    Salary = model.Salary,
                //    IsActive = model.IsActive,
                //    IsDeleted = model.IsDeleted,
                //    HiringDate = model.HiringDate,
                //    CreateAt = model.CreateAt,

                //};
                #endregion
                #region Auto Mapping
                var employee =  _mapper.Map<Employee>(model);
                #endregion
                #endregion

                var count = _employeeRepository.Add(employee);
                if (count > 0)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(model);

        }



        [HttpGet]
        public ActionResult Details(int? id, string ViewName = "Details")
        {
            if (id is null) return BadRequest();
            var employee = _employeeRepository.Get(id.Value);
            if (employee is null) return NotFound(new { StatusCode = 404, Message = $"Department With Id {id} is not Found " });

            return View(ViewName, employee);
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id is null) return BadRequest("Invalid Id ");
            var employee = _employeeRepository.Get(id.Value);
            if (employee is null) return NotFound(new { StatusCode = 404, Message = $"Department With Id {id} is not Found " });
            var employeeDto = _mapper.Map<CreateEmployeeDto>(employee);
            return View(employeeDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([FromRoute] int id, CreateEmployeeDto model)
        {
            if (ModelState.IsValid)
            {
                var  employee  = _mapper.Map<Employee>(model);
                employee.Id = id;
                // if (id != employee.Id) return BadRequest();
                var count = _employeeRepository.Update(employee);
                if (count > 0)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {

            return Details(id, "Delete");

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] int id, Employee employee)
        {
            if (ModelState.IsValid)
            {
                if (id != employee.Id) return BadRequest();
                var count = _employeeRepository.Delete(employee);
                if (count > 0)
                {
                    return RedirectToAction("Index");
                }

            }
            return View(employee);
        }
    }
}

