﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOs;
using WebAPI.Interfaces;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DepartmentController : ControllerBase
    {
        private IDepartmentRepo departmentRepo;

        public DepartmentController(IDepartmentRepo departmentRepo)
        {
            this.departmentRepo = departmentRepo;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Department> departments = departmentRepo.GetAll();
            if (departments.Count == 0)
            {
                return NotFound();
            }

            List<DepartmentDTO> departmentDTOs = new List<DepartmentDTO>();

            foreach (var dept in departments)
            {
                DepartmentDTO deptDTO = new DepartmentDTO()
                {
                    Id = dept.Id,
                    Name = dept.Name,
                };
                departmentDTOs.Add(deptDTO);
            }

            return Ok(departmentDTOs);
        }
    }
}
