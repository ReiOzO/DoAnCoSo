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
    public class DoctorScheduleController : BaseController
    {
        // GET: Admin/DoctorSchedule
        private const string KeyElement = "Đặt lịch";

        // GET: Admin/Event
        /*public ActionResult Index()
        {
            ViewBag.Feature = "Danh sách";
            ViewBag.Element = KeyElement;

            var user = GetCurrentUser();

            if (Request.Url != null) ViewBag.BaseURL = Request.Url.LocalPath;

            using (var workScope = new UnitOfWork(new HospitalManagementDbContext()))
            {
                ViewBag.Status = new SelectList(new List<object>
                {
                    new
                    {
                        Id = BookingStatusKey.Active,
                        Name = BookingStatusKey.GetText(BookingStatusKey.Active),
                    },
                    new
                    {
                        Id = BookingStatusKey.Pending,
                        Name = BookingStatusKey.GetText(BookingStatusKey.Pending),
                    },
                    new
                    {
                        Id = BookingStatusKey.Reject,
                        Name = BookingStatusKey.GetText(BookingStatusKey.Reject),
                    },
                }, "Id", "Name");

                var listPatient = workScope.Patients.Query(x => !x.IsDeleted);

                var doctorSchedules = workScope.DoctorSchedules.GetAll();

                var doctors = workScope.Doctors.GetAll();

                // Nếu người dùng là bác sĩ, chỉ hiển thị lịch đặt của bác sĩ đó
                if (user.Role == RoleKey.Doctor)
                {
                    doctorSchedules = doctorSchedules.Where(x => x.DoctorId == user.DoctorId);
                    doctors = doctors.Where(x => x.Id == user.DoctorId);
                }

                var listData = (from ds in doctorSchedules
                                join p in listPatient on ds.PatientId equals p.Id
                                join d in doctors on ds.DoctorId equals d.Id
                                select ds).ToList();

                return View(listData);
            }
        }*/
        public ActionResult Index()
        {
            ViewBag.Feature = "Danh sách";
            ViewBag.Element = KeyElement;

            var user = GetCurrentUser();

            if (Request.Url != null) ViewBag.BaseURL = Request.Url.LocalPath;

            using (var workScope = new UnitOfWork(new HospitalManagementDbContext()))
            {
                ViewBag.Status = new SelectList(new List<object>
                {
                    new
                    {
                        Id = BookingStatusKey.Active,
                        Name = BookingStatusKey.GetText(BookingStatusKey.Active),
                    },
                    new
                    {
                        Id = BookingStatusKey.Pending,
                        Name = BookingStatusKey.GetText(BookingStatusKey.Pending),
                    },
                    new
                    {
                        Id = BookingStatusKey.Reject,
                        Name = BookingStatusKey.GetText(BookingStatusKey.Reject),
                    },
                }, "Id", "Name");

                var listPatient = workScope.Patients.Query(x => !x.IsDeleted);

                // Lấy danh sách bác sĩ từ DB
                var doctors = workScope.Doctors.GetAll().Select(x => new
                {
                    id = x.Id,
                    FullName = x.Name
                });

                ViewBag.Doctors = user.Role == RoleKey.Doctor ?
                      new SelectList(doctors, "Id", "FullName", user.DoctorId)
                    : new SelectList(doctors, "Id", "FullName");

                // Nếu người dùng là bác sĩ, chỉ hiển thị lịch đặt của bác sĩ đó
                if (user.Role == RoleKey.Doctor)
                {
                    doctors = doctors.Where(x => x.id == user.DoctorId);
                }

                // Tạo SelectList cho DoctorId
                ViewBag.DoctorId = new SelectList(doctors, "Id", "Name");

                // Lấy danh sách DoctorSchedules từ DB
                var doctorSchedules = workScope.DoctorSchedules.GetAll();

                // Nếu người dùng là bác sĩ, chỉ lấy lịch đặt của bác sĩ đó
                if (user.Role == RoleKey.Doctor)
                {
                    doctorSchedules = doctorSchedules.Where(x => x.DoctorId == user.DoctorId);
                }

                // Kết hợp dữ liệu cần thiết để hiển thị trong view
                var listData = (from ds in doctorSchedules
                                join p in listPatient on ds.PatientId equals p.Id
                                join d in doctors on ds.DoctorId equals d.id
                                select ds).ToList();

                return View(listData);
            }
        }

        [HttpPost]
        public JsonResult GetJson(int id)
        {
            using (var workScope = new UnitOfWork(new HospitalManagementDbContext()))
            {
                var doctor = workScope.DoctorSchedules.FirstOrDefault(x => x.Id == id);

                return doctor == default ?
                    Json(new
                    {
                        status = false,
                        mess = "Có lỗi xảy ra: "
                    }) :
                    Json(new
                    {
                        status = true,
                        mess = "Lấy thành công " + KeyElement,
                        data = new
                        {
                            doctor.Id,
                            doctor.Status
                        }
                    });
            }
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult CreateOrEdit(DoctorSchedule input, bool isEdit)
        {
            try
            {
                if (isEdit)
                {
                    using (var workScope = new UnitOfWork(new HospitalManagementDbContext()))
                    {
                        var elm = workScope.DoctorSchedules.Get(input.Id);

                        if (elm != null) //update
                        {
                            input.DoctorId = elm.DoctorId;
                            input.PatientId = elm.PatientId;
                            input.ScheduleBook = elm.ScheduleBook;
                            elm = input;

                            workScope.DoctorSchedules.Put(elm, elm.Id);
                            workScope.Complete();

                            return Json(new { status = true, mess = "Đăng ký thành công " });
                        }
                        else
                        {
                            return Json(new { status = false, mess = "Không tồn tại " + KeyElement });
                        }
                    }
                }
                else
                {
                    return Json(new { status = true, mess = "Method not allow" + KeyElement });
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = false, mess = "Có lỗi xảy ra: " + ex.Message });
            }
        }
        /*public JsonResult CreateOrEdit(DoctorSchedule input, bool isEdit)
        {
            try
            {
                if (isEdit)
                {
                    using (var workScope = new UnitOfWork(new HospitalManagementDbContext()))
                    {
                        var elm = workScope.DoctorSchedules.Get(input.Id);

                        if (elm != null) // Nếu lịch tồn tại, cập nhật thông tin
                        {
                            input.DoctorId = elm.DoctorId;
                            input.PatientId = elm.PatientId;
                            input.ScheduleBook = elm.ScheduleBook;
                            elm = input;

                            workScope.DoctorSchedules.Put(elm, elm.Id);
                            workScope.Complete();

                            return Json(new { status = true, mess = "Cập nhật thành công " });
                        }
                        else // Nếu lịch không tồn tại
                        {
                            return Json(new { status = false, mess = "Không tồn tại " + KeyElement });
                        }
                    }
                }
                else // Nếu không cho phép method này
                {
                    return Json(new { status = true, mess = "Phương thức không được phép " + KeyElement });
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = false, mess = "Có lỗi xảy ra: " + ex.Message });
            }
        }*/

        [HttpPost]
        public JsonResult Del(int id)
        {
            try
            {
                using (var workScope = new UnitOfWork(new HospitalManagementDbContext()))
                {
                    var elm = workScope.DoctorSchedules.Get(id);
                    if (elm != null)
                    {
                        //del
                        workScope.DoctorSchedules.Remove(elm);
                        workScope.Complete();
                        return Json(new { status = true, mess = "Xóa thành công " + KeyElement });
                    }
                    else
                    {
                        return Json(new { status = false, mess = "Không tồn tại " + KeyElement });
                    }
                }
            }
            catch
            {
                return Json(new { status = false, mess = "Thất bại" });
            }
        }
    }
}