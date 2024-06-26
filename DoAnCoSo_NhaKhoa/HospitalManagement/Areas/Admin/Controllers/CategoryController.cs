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
    public class CategoryController : BaseController
    {
        private const string KeyElement = "Danh mục";

        // GET: Admin/FilmMovieType
        public ActionResult Index()
        {
            ViewBag.Feature = "Danh sách";
            ViewBag.Element = KeyElement;

            if (Request.Url != null) ViewBag.BaseURL = Request.Url.LocalPath;

            using (var workScope = new UnitOfWork(new HospitalManagementDbContext()))
            {
                var listData = workScope.Categories.GetAll().ToList();
                return View(listData);
            }
        }

        [HttpPost]
        public JsonResult GetJson(Guid? id)
        {
            using (var workScope = new UnitOfWork(new HospitalManagementDbContext()))
            {
                var category = workScope.Categories.FirstOrDefault(x => x.Id == id);
                return category == default ?
                    Json(new
                    {
                        status = false,
                        mess = "Có lỗi xảy ra: "
                    }) :
                    Json(new
                    {
                        status = true,
                        mess = "Lấy thành công " + KeyElement,
                        data = category
                    });
            }
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult CreateOrEdit(Category input, bool isEdit)
        {
            try
            {
                if (isEdit)
                {
                    using (var workScope = new UnitOfWork(new HospitalManagementDbContext()))
                    {
                        workScope.Categories.Put(input, input.Id);
                        workScope.Complete();
                        return Json(new { status = true, mess = "Sửa thành công " + KeyElement });
                    }
                }
                else
                {
                    input.Id = Guid.NewGuid();
                    using (var workScope = new UnitOfWork(new HospitalManagementDbContext()))
                    {
                        workScope.Categories.Add(input);
                        workScope.Complete();
                        return Json(new { status = true, mess = "Thêm thành công " + KeyElement });
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = false, mess = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Del(Guid id)
        {
            try
            {
                using (var workScope = new UnitOfWork(new HospitalManagementDbContext()))
                {
                    // Tìm mục theo ID và xóa nó
                    var category = workScope.Categories.FirstOrDefault(x => x.Id == id);
                    if (category == null)
                    {
                        return Json(new { status = false, mess = "Không tìm thấy mục cần xóa." });
                    }
                    workScope.Categories.Remove(category);
                    workScope.Complete();
                    return Json(new { status = true, mess = "Xóa thành công " + KeyElement });
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = false, mess = "Có lỗi xảy ra: " + ex.Message });
            }
        }
    }
}