using AutoMapper;
using Company.G04.BLL.Interfaces;
using Company.G04.BLL.Repositries;
using Company.G04.DAL.Moudel;
using Company.G04.PL.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore.Migrations.Internal;

namespace Company.G04.PL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;

        // aSK CLR  Create Object DepartemntRepository 
        public DepartmentController(IDepartmentRepository departmentRepository
            ,IMapper mapper)
        {
            _departmentRepository = departmentRepository;
          _mapper = mapper;
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
                var  departments = _mapper.Map<Department>(model);
                var count = _departmentRepository.Add(departments);
                if(count > 0)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }

        [HttpGet]     
        public IActionResult Details(int? id , string ViewName = "Details")
        {
            if (id is null) return BadRequest("Invaliad Id");
            var departments = _departmentRepository.Get(id.Value);
            if (departments is null) return NotFound(new { StatusCode = 404, Message = $"Department With Id {id} is not Found " });

            return View(ViewName,departments);      
        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id is null) return BadRequest("Invalid Id ");
            var departments = _departmentRepository.Get(id.Value);
            if (departments is null) return NotFound(new { StatusCode = 404, Message = $"Department With Id {id} is not Found " });
           var departmentDto = _mapper.Map<CreateDepartmentDto>(departments);
            
            return View(departmentDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id , CreateDepartmentDto model)
        {
            if (ModelState.IsValid)
            {
                //var department = new Department()
                //{
                //    Id=id,
                //    Code = model.Code,
                //    Name = model.Name,
                //    CreateAt = model.CreateAt
                //};
                //if (id != department.Id) return BadRequest();
                var  department  =  _mapper.Map<Department>(model);
                department.Id = id;
                var count = _departmentRepository.Update(department);
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
        public IActionResult Delete([FromRoute] int id, Department department)
        {
            if (ModelState.IsValid)
            {
                if (id != department.Id) return BadRequest();
                var count = _departmentRepository.Delete(department);
                if (count > 0)
                {
                    return RedirectToAction("Index");
                }

            }
            return View(department);
        }
    }
}
