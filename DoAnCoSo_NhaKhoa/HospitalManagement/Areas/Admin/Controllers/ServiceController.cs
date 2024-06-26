﻿using BELibrary.Core.Entity;
using BELibrary.Core.Utils;
using BELibrary.DbContext;
using BELibrary.Entity;
using HospitalManagement.Areas.Admin.Authorization;
using System;
using System.Linq;
using System.Web.Mvc;

namespace HospitalManagement.Areas.Admin.Controllers
{
    [Permission(Role = RoleKey.Admin)]
    public class ServiceController : BaseController
    {
        private readonly string KeyElement = "Dịch vụ";

        // GET: Admin/Gallery
        public ActionResult Index(Guid? categoryId)
        {
            ViewBag.Feature = "Danh sách";
            ViewBag.Element = KeyElement;

            if (Request.Url != null) ViewBag.BaseURL = Request.Url.LocalPath;

            using (var workScope = new UnitOfWork(new HospitalManagementDbContext()))
            {
                var categories = workScope.Categories.GetAll().ToList();
                ViewBag.Categories = new SelectList(categories, "Id", "Name");

                if (!categoryId.HasValue)
                {
                    if (categories.Count > 0)
                    {
                        categoryId = categories[0].Id;
                    }
                }
                ViewBag.CategoryId = categoryId;

                var listData = workScope.Services.Query(x => x.CategoryId == categoryId).ToList();

                return View(listData);
            }
        }

        public ActionResult Create()
        {
            ViewBag.Feature = "Thêm mới";
            ViewBag.Element = KeyElement;
            if (Request.Url != null)
                ViewBag.BaseURL = string.Join("", Request.Url.Segments.Take(Request.Url.Segments.Length - 1));

            ViewBag.IsEdit = false;
            using (var workScope = new UnitOfWork(new HospitalManagementDbContext()))
            {
                var categories = workScope.Categories.GetAll().ToList();
                ViewBag.Categories = new SelectList(categories, "Id", "Name");
            }

            return View();
        }

        public ActionResult Update(Guid id)
        {
            ViewBag.isEdit = true;
            ViewBag.Feature = "Cập nhật";
            ViewBag.Element = KeyElement;
            if (Request.Url != null)
            {
                ViewBag.BaseURL = Request.Url.LocalPath;

                ViewBag.BaseURL = string.Join("", Request.Url.Segments.Take(Request.Url.Segments.Length - 1));
            }

            using (var workScope = new UnitOfWork(new HospitalManagementDbContext()))
            {
                var categories = workScope.Categories.GetAll().ToList();
                ViewBag.Categories = new SelectList(categories, "Id", "Name");

                var medicine = workScope.Services
                    .FirstOrDefault(x => x.Id == id);

                if (medicine != null)
                {
                    return View("Create", medicine);
                }
                else
                {
                    return RedirectToAction("Create", "Item");
                }
            }
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult CreateOrEdit(Service input, bool isEdit)
        {
            try
            {
                if (isEdit) //update
                {
                    using (var workScope = new UnitOfWork(new HospitalManagementDbContext()))
                    {
                        var elm = workScope.Services.Get(input.Id);

                        if (elm != null) //update
                        {
                            //Che bien du lieu
                            input.CreatedBy = elm.CreatedBy;
                            input.CreatedDate = elm.CreatedDate;
                            input.ModifiedDate = DateTime.Now;
                            input.ModifiedBy = GetCurrentUser().FullName;

                            elm = input;

                            workScope.Services.Put(elm, elm.Id);
                            workScope.Complete();

                            return Json(new { status = true, mess = "Cập nhập thành công " });
                        }
                        else
                        {
                            return Json(new { status = false, mess = "Không tồn tại " + KeyElement });
                        }
                    }
                }
                else //Thêm mới
                {
                    using (var workScope = new UnitOfWork(new HospitalManagementDbContext()))
                    {
                        //Che bien du lieu
                        input.Id = Guid.NewGuid();

                        input.CreatedBy = GetCurrentUser().FullName;
                        input.CreatedDate = DateTime.Now;
                        input.ModifiedDate = DateTime.Now;
                        input.ModifiedBy = GetCurrentUser().FullName;

                        workScope.Services.Add(input);
                        workScope.Complete();
                    }
                    return Json(new { status = true, mess = "Thêm thành công " + KeyElement });
                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    status = false,
                    mess = "Có lỗi xảy ra: " + ex.Message
                });
            }
        }

        [HttpPost]
        public JsonResult Del(Guid id)
        {
            try
            {
                using (var workScope = new UnitOfWork(new HospitalManagementDbContext()))
                {
                    var elm = workScope.Services.Get(id);
                    if (elm != null)
                    {
                        //del
                        workScope.Services.Remove(elm);
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