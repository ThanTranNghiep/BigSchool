﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BigSchool.Models;
using BigSchool.ViewModels;
using Microsoft.AspNet.Identity;

namespace BigSchool.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public CoursesController()
        {
            _dbContext = new ApplicationDbContext();
        }
        // GET: Courses
        [Authorize]
        public ActionResult Create()
        {
            var viewModel = new CourseViewModel
            {
                Categories = _dbContext.Categories.ToList(),
                Heading = "Add Course"
            };
            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CourseViewModel model)
        {
            if(!ModelState.IsValid)
            {
                model.Categories = _dbContext.Categories.ToList();
                return View("Create", model);
            }
            var course = new Course
            {
                LecturerId = User.Identity.GetUserId(),
                DateTime = model.GetDateTime(),
                CategoryId = model.Category,
                Place = model.Place

            };
            _dbContext.Courses.Add(course);
            _dbContext.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public ActionResult Attending()
        {
            var UserId = User.Identity.GetUserId();
            var courses = _dbContext.Attendances
                .Where(a => a.AttendeeId == UserId)
                .Select(a => a.Course)
                .Include(a => a.Lecturer)
                .Include(a => a.Category)
                .ToList();
            var viewModel = new CoursesViewModel
            {
                UpcommingCourses = courses,
                ShowAction = User.Identity.IsAuthenticated
            };
            return View(viewModel);
        }
        [Authorize]
        public ActionResult Mine()
        {
            var UserId = User.Identity.GetUserId();
            var courses = _dbContext.Courses
                .Where(c => c.LecturerId == UserId && c.DateTime > DateTime.Now)
                .Include(l=>l.Lecturer)
                .Include(c => c.Category)
                .ToList();
            return View(courses);
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            var userId = User.Identity.GetUserId();
            var course = _dbContext.Courses.Single(x => x.Id == id && x.LecturerId == userId);
            var viewModel = new CourseViewModel
            {
                Categories = _dbContext.Categories.ToList(),
                Date = course.DateTime.ToString("dd/M/YYYY"),
                Time = course.DateTime.ToString("HH:mm"),
                Category = course.CategoryId,
                Place = course.Place,
                Heading = "Edit Course",
                Id = course.Id
            };
            return View("Create", viewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(CourseViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Categories = _dbContext.Categories.ToList();
                return View("Create", viewModel);
            }

            var userId = User.Identity.GetUserId();
            var course = _dbContext.Courses.Single(x => x.Id == viewModel.Id && x.LecturerId == userId);

            course.Place = viewModel.Place;
            course.DateTime = viewModel.GetDateTime();
            course.CategoryId = viewModel.Category;

            _dbContext.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
    }
}