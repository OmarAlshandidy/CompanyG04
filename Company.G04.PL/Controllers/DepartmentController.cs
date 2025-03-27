using System.Threading.Tasks;
using AutoMapper;
using Company.G04.BLL.Interfaces;
using Company.G04.BLL.Repositries;
using Company.G04.BLL.UnitOfWork;
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
        private readonly IUnitOfWork _unitOfWork;

        // aSK CLR  Create Object DepartemntRepository 
        public DepartmentController(IDepartmentRepository departmentRepository
            , IMapper mapper , IUnitOfWork unitOfWork)
        {
            _departmentRepository = departmentRepository;
            _mapper = mapper;
          _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var departments = await _unitOfWork.DepartmentRepository.GetAllAsync();
            // Dictionary  : 3.Property  
            // 1. ViewData : Transfer Extra Information From Controller (Action) To View
            ViewData["Message"] = "Hello From View Data";

            // 2. ViewBag  : Transfer Extra Information From Controller (Action) To Vie
            ViewBag.Message = "HEllo From View Bag";
            // 3.TempData
            return View(departments);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateDepartmentDto model)
        {
            if (ModelState.IsValid)
            {
                var departments = _mapper.Map<Department>(model);
                await _unitOfWork.DepartmentRepository.AddAsync(departments);
                var count = await _unitOfWork.CompleteAsync();
                if (count > 0)
                {
                    TempData["Message"] = "Department Is Created !!";
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id, string ViewName = "Details")
        {
            if (id is null) return BadRequest("Invaliad Id");
            var departments = await _unitOfWork.DepartmentRepository.GetAsync (id.Value);
            if (departments is null) return NotFound(new { StatusCode = 404, Message = $"Department With Id {id} is not Found " });

            return View(ViewName, departments);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return BadRequest("Invalid Id ");
            var departments = await _unitOfWork.DepartmentRepository.GetAsync(id.Value);
            if (departments is null) return NotFound(new { StatusCode = 404, Message = $"Department With Id {id} is not Found " });
            var departmentDto = _mapper.Map<CreateDepartmentDto>(departments);

            return View(departmentDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, CreateDepartmentDto model)
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
                var department = _mapper.Map<Department>(model);
                department.Id = id;
               _unitOfWork.DepartmentRepository.Update(department);
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
            if (id is null) return BadRequest("Invaliad Id");
            var departments = await _unitOfWork.DepartmentRepository.GetAsync(id.Value);
            if (departments is null) return NotFound(new { StatusCode = 404, Message = $"Department With Id {id} is not Found " });
            return View(departments);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int id, Department department)
        {
            if (ModelState.IsValid)
            {
                if (id != department.Id) return BadRequest();
               _unitOfWork.DepartmentRepository.Delete(department);
                var count = await _unitOfWork.CompleteAsync();  
                if (count > 0)
                {
                    return RedirectToAction("Index");
                }

            }
            return View(department);
        }
    }
}