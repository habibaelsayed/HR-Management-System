﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOs;
using WebAPI.Interfaces;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendenceController : ControllerBase
    {
        private IAttendence attendenceRepo;
        private IEmployeeRepo employeeRepo;

        public AttendenceController(IAttendence attendenceRepo, IEmployeeRepo employeeRepo)
        {
            this.attendenceRepo = attendenceRepo;
            this.employeeRepo = employeeRepo;
        }


        [HttpGet]
        public ActionResult GetAll()
        {
            List<Attendence> attendences = attendenceRepo.GetAll();
            List<AttendanceDTO> attendanceDTOs = new List<AttendanceDTO>();
            if(attendences.Count == 0)
            {
                return NotFound("Attendence list is empty");
            }
            foreach(var attendence in attendences)
            {
                AttendanceDTO attendanceDTO = new AttendanceDTO()
                {
                    EmpId = attendence.EmpId,
                    Day = attendence.Day,
                    Arrival = attendence.Arrival,
                    Departure = attendence.Departure,
                    EmpName = attendence.Employee.FullName,
                    DeptName = attendence.Employee.Department.Name,
                    Status = (int)attendence.Status
                };

                attendanceDTOs.Add(attendanceDTO);
            }

            return Ok(attendanceDTOs);
        }

        [HttpPost]
        public ActionResult Add(AttendanceDTO attendanceDTO)
        {
            int employeeDepartureHour =(int) employeeRepo.GetById(attendanceDTO.EmpId).Departure.Hour;
            int employeeArrivalHour =(int) employeeRepo.GetById(attendanceDTO.EmpId).Arrival.Hour;

            if(attendanceDTO.Status == 0)
            {
                Attendence attendence = new Attendence()
                {
                    EmpId = attendanceDTO.EmpId,
                    Day = attendanceDTO.Day,
                    Status = (AttendenceStatus)attendanceDTO.Status,
                    Arrival = attendanceDTO.Arrival,
                    Departure = attendanceDTO.Departure,
                    OvertimeInHours = attendanceDTO.Departure?.Hour - employeeDepartureHour,
                    LatetimeInHours = employeeArrivalHour - attendanceDTO.Arrival?.Hour
                };
                attendenceRepo.Add(attendence);
            }
            else
            {
                Attendence attendence = new Attendence()
                {
                    EmpId = attendanceDTO.EmpId,
                    Day = attendanceDTO.Day,
                    Status = (AttendenceStatus)attendanceDTO.Status
                };
                attendenceRepo.Add(attendence);
            }
            attendenceRepo.Save();

            return Ok();
        }
        [HttpPut("{empId}")]
        public IActionResult Update([FromRoute] int empId,[FromQuery] DateOnly date, [FromBody] AttendanceDTO attendenceDTO)
        {
            Attendence existingAttendence = attendenceRepo.GetDayByEmpId(empId,date);
            if(existingAttendence == null)
            {
                return BadRequest("Employee with specified Date Not Found");
            }
            existingAttendence.Arrival = attendenceDTO.Arrival;
            existingAttendence.Departure = attendenceDTO.Departure;
            existingAttendence.Status = (AttendenceStatus)attendenceDTO.Status;
            attendenceRepo.Update(empId,date,existingAttendence);
            attendenceRepo.Save();
            return Ok();
        }
        [HttpDelete("{empId}")]
        public IActionResult Delete([FromRoute] int empId,[FromQuery] DateOnly date)
        {
            Attendence attendence = attendenceRepo.GetDayByEmpId(empId, date);
            if (attendence == null)
            {
                return NotFound();
            }
            attendenceRepo.Delete(empId, date);
            attendenceRepo.Save();
            return NoContent();
        }
        [HttpGet("GetEmployeeDay/{empId}")]
        public IActionResult GetDayByEmpId([FromRoute] int empId, [FromQuery] DateOnly day)
        {
            Attendence? attendence = attendenceRepo.GetDayByEmpId(empId, day);
            if (attendence == null)
            {
                return NotFound();
            }
            AttendanceDTO attendanceDTO = new AttendanceDTO()
            {
                EmpId = attendence.EmpId,
                Day = attendence.Day,
                Arrival = attendence.Arrival,
                Departure = attendence.Departure,
                EmpName = attendence.Employee.FullName,
                DeptName = attendence.Employee.Department.Name,
                Status = (int)attendence.Status
            };

            return Ok(attendanceDTO);
        }
        [HttpGet("{empId}")]
        public IActionResult GetAttendenceByEmpId(int empId)
        {
            List<AttendanceDTO> attendanceDTOs = new List<AttendanceDTO>();
            List<Attendence> attendences = attendenceRepo.GetAttendenceByEmpId(empId);
            if (attendences == null || attendences.Count == 0)
            {
                return NotFound("No attendance records found for the specified employee.");
            }
            foreach (var attendance in attendences)
            {
                AttendanceDTO attendanceDTO = new AttendanceDTO()
                {
                    EmpId = attendance.EmpId,
                    Day = attendance.Day,
                    Arrival = attendance.Arrival,
                    Departure = attendance.Departure,
                    EmpName = attendance.Employee.FullName,
                    DeptName = attendance.Employee.Department.Name,
                    Status = (int)attendance.Status
                };
                attendanceDTOs.Add(attendanceDTO);
            }
            return Ok(attendanceDTOs);
        }
    }
}
