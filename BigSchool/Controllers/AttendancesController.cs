using BigSchool.DTOs;
using BigSchool.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.ModelBinding;

namespace BigSchool.Controllers
{
    public class AttendancesController : ApiController
    {
        private ApplicationDbContext _dbContex;
        public AttendancesController()
        {
            _dbContex = new ApplicationDbContext();
        }
        [HttpPost]
        public IHttpActionResult Attend([FromBody] int courseId)
        {
            var UserId = User.Identity.GetUserId();
            if(_dbContex.Attendances.Any( x => x.AttendeeId == UserId && x.CourseId== courseId))
                return BadRequest("The Attendance already exists!");
            var attendance = new Attendance()
            {
                CourseId = courseId,
                AttendeeId = UserId,
            };
            _dbContex.Attendances.Add(attendance);
            _dbContex.SaveChanges();
            return Ok();
        }

        [HttpPost]
        public IHttpActionResult Attend(AttendanceDto attendanceDto)
        {
            var UserId = User.Identity.GetUserId();
            if (_dbContex.Attendances.Any(x => x.AttendeeId == UserId && x.CourseId == attendanceDto.CourseId))
                return BadRequest("The Attendance already exists!");
            var attendance = new Attendance()
            {
                CourseId = attendanceDto.CourseId,
                AttendeeId = UserId,
            };
            _dbContex.Attendances.Add(attendance);
            _dbContex.SaveChanges();
            return Ok();
        }

    }
}