using System.Threading.Tasks;
using AutoMapper;
using Company.G04.BLL.Interfaces;
using Company.G04.BLL.Repositries;
using Company.G04.BLL.UnitOfWork;
using Company.G04.DAL.Data.Context;
using Company.G04.DAL.Data.Migrations;
using Company.G04.DAL.Moudel;
using Company.G04.PL.Dtos;
using Company.G04.PL.Healper;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;


namespace Company.G04.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeController(IEmployeeRepository employeeRepository
            ,IDepartmentRepository departmentRepository
            , IMapper mapper ,IUnitOfWork unitOfWork)
        {
            _employeeRepository = employeeRepository;
           _departmentRepository = departmentRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public async Task<ActionResult> Index( string SearchInput)
         {

            IEnumerable<Employee> employees;
            if (string.IsNullOrEmpty(SearchInput))
            {
                employees =await _unitOfWork.EmployeeRepository.GetAllAsync();
            }
            else
            {
                employees = await _unitOfWork.EmployeeRepository.GetByNameAsync(SearchInput);
            }
            return View(employees);
        }
        [HttpGet]

        public async Task<ActionResult> Create()
            { 
            var departments =await _unitOfWork.DepartmentRepository.GetAllAsync();
            ViewData["departments"] = departments; 

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateEmployeeDto model)
        {
            if (ModelState.IsValid)
            {
                if (model.Image is not null)
                {
                 model.ImageName = DocumentSettings.UplodFile(model.Image, "images");
                }
            
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
                var employee = _mapper.Map<Employee>(model);
                #endregion
                #endregion
                await _unitOfWork.EmployeeRepository.AddAsync(employee);
                var count = await _unitOfWork.CompleteAsync();
                if (count > 0)
                {
                    TempData["Message"] = "Employee Is Created !!";

                    return RedirectToAction("Index");
                }
            }
            return View(model);

        }



        [HttpGet]
        public async Task<ActionResult> Details(int? id, string ViewName = "Details")
        {
            if (id is null) return BadRequest();
            var employee =await _unitOfWork.EmployeeRepository.GetAsync(id.Value);
            if (employee is null) return NotFound(new { StatusCode = 404, Message = $"Department With Id {id} is not Found " });
           
            return View(ViewName, employee);
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int? id)
        {
            var departments = _unitOfWork.DepartmentRepository.GetAllAsync();
            ViewData["departments"] = departments;
            if (id is null) return BadRequest("Invalid Id ");
            var employee = await  _unitOfWork.EmployeeRepository.GetAsync(id.Value);
            if (employee is null) return NotFound(new { StatusCode = 404, Message = $"Department With Id {id} is not Found " });
            var employeeDto = _mapper.Map<CreateEmployeeDto>(employee);
            return View(employeeDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([FromRoute] int id, CreateEmployeeDto model)
        {
            if (ModelState.IsValid)
            {

                if(model.ImageName is not null && model.Image is not null)
                {
                    DocumentSettings.DeleteFile(model.ImageName, "images");
                }
                if (model.Image is not null)
                model.ImageName=DocumentSettings.UplodFile(model.Image, "images");

                var employee = _mapper.Map<Employee>(model);
                employee.Id = id;
                // if (id != employee.Id) return BadRequest();
                _employeeRepository.Update(employee);
                var count = await _unitOfWork.CompleteAsync();
                if (count > 0)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return BadRequest();
            var employee = await _unitOfWork.EmployeeRepository.GetAsync(id.Value);
            if (employee is null) return NotFound(new { StatusCode = 404, Message = $"Department With Id {id} is not Found " });


            return View(employee);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int id, Employee employee)
        {
            if (ModelState.IsValid)
            {
                if (id != employee.Id) return BadRequest();
                _employeeRepository.Delete(employee);
                var count = await _unitOfWork.CompleteAsync();  
                if (count > 0)
                {
                    if (employee.ImageName is not null)
                    {
                        DocumentSettings.DeleteFile(employee.ImageName, "images");
                    }
                    return RedirectToAction("Index");
                }

            }
            return View(employee);
        }
    }
}