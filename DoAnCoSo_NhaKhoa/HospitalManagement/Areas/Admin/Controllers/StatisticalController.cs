using BELibrary.Core.Entity;
using BELibrary.Core.Utils;
using BELibrary.DbContext;
using BELibrary.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace HospitalManagement.Areas.Admin.Controllers
{
    public class StatisticalController : BaseController
    {
        // GET: Admin/Statistical
        public ActionResult Index()
        {
            var user = GetCurrentUser();

            using (var workScope = new UnitOfWork(new HospitalManagementDbContext()))
            {
                var patients = workScope.Patients.GetAll().ToList();

                if (user.Role == RoleKey.Doctor)
                {
                    var patientOfDoctors =
                        workScope.PatientDoctors.Query(x => x.DoctorId == user.DoctorId) ?? new List<PatientDoctor>();

                    var patientOfDoctorIds = patientOfDoctors.Select(x => x.PatientId);

                    patients = patients.Where(x => patientOfDoctorIds.Contains(x.Id)).ToList();
                }

                var schedules = workScope.DoctorSchedules.GetAll().ToList();

                if (user.Role == RoleKey.Doctor)
                {
                    schedules = schedules.Where(x => x.DoctorId == user.DoctorId).ToList();
                }

                ViewBag.PatientCount = patients.Count;
                ViewBag.ScheduleCount = schedules.Count;

                var now = DateTime.Now;

                ViewBag.ScheduleTodayCount = schedules.Count(x => x.ScheduleBook.Day == now.Day && x.ScheduleBook.Month == now.Month && x.ScheduleBook.Year == now.Year);
                ViewBag.ScheduleMonthCount = schedules.Count(x => x.ScheduleBook.Month == now.Month && x.ScheduleBook.Year == now.Year);

                ViewBag.PatientTodayCount = patients.Count(x => x.JoinDate.Day == now.Day && x.JoinDate.Month == now.Month && x.JoinDate.Year == now.Year);
                ViewBag.PatientMonthCount = patients.Count(x => x.JoinDate.Month == now.Month && x.JoinDate.Year == now.Year);

                // new patient

                var patientsNew = workScope.Patients.Query(x => x.Status).OrderByDescending(x => x.JoinDate).Take(6).ToList();

                if (user.Role == RoleKey.Doctor)
                {
                    var patientOfDoctors =
                        workScope.PatientDoctors.Query(x => x.DoctorId == user.DoctorId) ?? new List<PatientDoctor>();

                    var patientOfDoctorIds = patientOfDoctors.Select(x => x.PatientId);

                    patientsNew = patientsNew.Where(x => patientOfDoctorIds.Contains(x.Id)).ToList();
                }

                ViewBag.PatientsNew = patientsNew;

                var categories = workScope.Categories.GetAll().ToList();
                ViewBag.Categories = new SelectList(categories, "Id", "Name");
            }

            return View();
        }

        [HttpPost]
        public JsonResult GetRegByYear(int year)
        {
            var user = GetCurrentUser();
            using (var workScope = new UnitOfWork(new HospitalManagementDbContext()))
            {
                var patients = workScope.Patients.GetAll();

                if (user.Role == RoleKey.Doctor)
                {
                    var patientOfDoctors =
                        workScope.PatientDoctors.Query(x => x.DoctorId == user.DoctorId) ?? new List<PatientDoctor>();

                    var patientOfDoctorIds = patientOfDoctors.Select(x => x.PatientId);

                    patients = patients.Where(x => patientOfDoctorIds.Contains(x.Id)).ToList();
                }

                var date = new DateTime(year, 1, 1);
                var months = Enumerable.Range(0, 11)
                    .Select(x => new
                    {
                        month = date.AddMonths(x).Month,
                        year = date.AddMonths(x).Year
                    }).ToList();

                var dataPerYearAndMonth =
                    months.GroupJoin(patients,
                        m => new { m.month, m.year },
                        patient => new
                        {
                            month = patient.JoinDate.Month,
                            year = patient.JoinDate.Year
                        },
                        (p, g) => new
                        {
                            month = "Tháng " + p.month,
                            p.year,
                            count = g.Count()
                        });

                return
                    Json(new
                    {
                        status = true,
                        mess = "Thành công ",
                        data = dataPerYearAndMonth.ToList()
                    });
            }
        }
    }
}