﻿using Company.G04.BLL.Interfaces;
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
            //if (id is null) return BadRequest("Invalid Id ");
            //var departments = _departmentRepository.Get(id.Value);
            //if (departments is null) return NotFound(new { StatusCode = 404, Message = $"Department With Id {id} is not Found " });

            return Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id , Department department)
        {
            if (ModelState.IsValid)
            {
                if (id != department.Id) return BadRequest();
                var count = _departmentRepository.Update(department);
                if (count > 0)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(department);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            //if (id is null) return BadRequest("Invalid Id ");
            //var departments = _departmentRepository.Get(id.Value);
            //if (departments is null) return NotFound(new { StatusCode = 404, Message = $"Department With Id {id} is not Found " });

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
