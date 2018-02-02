using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Text;
using VATMVCAPPFramework.Utilities;
using VATMVCAPPFramework.Data.IdentityModel;
using VATMVCAPPFramework.Data.IdentityService;
using VATMVCAPPFramework.Repository;
using VATMVCAPPFramework.Repository.CoreRepositories;
using VATMVCAPPFramework.ViewModel;
using VATMVCAPPFramework.Controllers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Net;
using log4net;
using System.Data.SqlClient;

namespace VATMVCAPPFramework.Areas.Portal.Controllers
{
    [Authorize(Roles = "PortalAdmin")]
    public class PortalRoleController : BaseController
    {
        private readonly Utility _utility;
        private readonly IRepositoryQuery<RolePermission> _rolePermissionyQuery;
        private readonly IRepositoryCommand<RolePermission> _rolePermissionCommand;
        private readonly IActivityLogRepositoryCommand _activityRepo;
        private readonly ILog _log;
        
        public PortalRoleController(IActivityLogRepositoryCommand activityRepo, IRepositoryCommand<RolePermission> rolePermissionCommand, IRepositoryQuery<RolePermission> rolePermissionQuery, Utility utility, ILog log)
        {
            _rolePermissionCommand = rolePermissionCommand;
            _rolePermissionyQuery = rolePermissionQuery;
            _activityRepo = activityRepo;
            _utility = utility;
            _log = log;
        }

        public ActionResult Index()
        {
            _log.Info("<<< In Portal Role Page >>>");
            if (TempData["MESSAGE"] != null)
            {
                ViewBag.Msg = TempData["MESSAGE"] as string;
            }
            //RoleManager
            var result = RoleManager.Roles.ToList();
            var model = result.Select(e => new ApplicationRoleViewModel()
            {
                Id = e.Id,
                Name = e.Name
            });
            return View(model);

        }

        // GET: ApplicationRoles/Create
        public ActionResult Create()
        {
            CreateViewBagParams();
            return PartialView("_PartialAddEdit", new ApplicationRoleViewModel { Permissions = _utility.GetPermissions() });
        }

        [HttpPost]
        public ActionResult Create(ApplicationRoleViewModel model)
        {
            try
            {
                CreateViewBagParams();
                if (ModelState.IsValid)
                {
                    ApplicationRole applicationRole = new ApplicationRole
                    {
                        Name = model.Name
                    };
                    var roleResult = RoleManager.Create(applicationRole);
                    if (!roleResult.Succeeded)
                    {
                        ModelState.AddModelError("", roleResult.Errors.First());
                        return PartialView("_PartialAddEdit", new ApplicationRoleViewModel());
                    }
                    else
                    {
                        _activityRepo.CreateActivityLog(string.Format("Created Portal Role with Name:{0}", applicationRole.Name), this.GetContollerName(), this.GetActionName(), User.Identity.GetUserId<Int64>(), applicationRole);

                        _rolePermissionyQuery.ExecuteStoreprocedure("DeletePermissionByRoleID @RoleId", new SqlParameter("RoleId", applicationRole.Id));
                        if (model.SelectedPermissionId != null && model.SelectedPermissionId.Any())
                        {
                            foreach (var permissionId in model.SelectedPermissionId)
                            {
                                _rolePermissionCommand.Insert(new RolePermission()
                                {
                                    RoleId = applicationRole.Id,
                                    PermissionId = permissionId,
                                });
                                _rolePermissionCommand.SaveChanges();
                               
                            }
                        }
                      

                    }

                    TempData["MESSAGE"] = "Portal Role " + applicationRole.Name + " was successfully created";
                    ModelState.Clear();
                    return Json(new { success = true });
                }
                else
                {
                    StringBuilder errorMsg = new StringBuilder();

                    foreach (var modelError in ModelState.Values.SelectMany(modelState => modelState.Errors))
                    {
                        errorMsg.AppendLine(modelError.ErrorMessage);
                        ModelState.AddModelError(string.Empty, modelError.ErrorMessage);
                    }
                    ViewBag.ErrMsg = errorMsg.ToString();
                    return PartialView("_PartialAddEdit", model);
                }
            }
            catch (Exception exp)
            {
                _log.Error(exp);
                return View("Error");
            }

        }

        // GET: ApplicationRoles/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                EditViewBagParams();
                if (id <= 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ApplicationRole applicationRole = await RoleManager.FindByIdAsync(id);
                if (applicationRole == null)
                {
                    return HttpNotFound();
                }
                ApplicationRoleViewModel applicationRoleViewModel = new ApplicationRoleViewModel
                {
                    Id = applicationRole.Id,
                    Name = applicationRole.Name,
                    Permissions = _utility.GetPermissions(),
                    SelectedPermissionId = _rolePermissionyQuery.GetAllList(c => c.RoleId == id).Select(c => c.PermissionId).ToList(),
                };
                return PartialView("_PartialAddEdit", applicationRoleViewModel);
            }
            catch (Exception exp)
            {
                _log.Error(exp);
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, ApplicationRoleViewModel applicationRoleViewModel)
        {
            try
            {
                EditViewBagParams();
                if (id <= 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                if (ModelState.IsValid)
                {
                    ApplicationRole applicationRole = await RoleManager.FindByIdAsync(applicationRoleViewModel.Id);
                    string originalName = applicationRole.Name;

                    if (originalName == "PortalAdmin" && applicationRoleViewModel.Name != "PortalAdmin")
                    {
                        ModelState.AddModelError("", "You cannot change the name of the Portal Admin role.");
                        return PartialView("_PartialAddEdit", applicationRoleViewModel);
                    }

                    if (originalName != "PortalAdmin" && applicationRoleViewModel.Name == "PortalAdmin")
                    {
                        ModelState.AddModelError("", "You cannot change the name of a role to Portal Admin.");
                        return PartialView("_PartialAddEdit", applicationRoleViewModel);
                    }

                    applicationRole.Name = applicationRoleViewModel.Name;
                    await RoleManager.UpdateAsync(applicationRole);

                    _rolePermissionyQuery.ExecuteStoreprocedure("DeletePermissionByRoleID @RoleId", new SqlParameter("RoleId", applicationRole.Id));
                    if (applicationRoleViewModel.SelectedPermissionId != null && applicationRoleViewModel.SelectedPermissionId.Any())
                    {
                        foreach (var permissionId in applicationRoleViewModel.SelectedPermissionId)
                        {
                            _rolePermissionCommand.Insert(new RolePermission()
                            {
                                RoleId = applicationRole.Id,
                                PermissionId = permissionId,
                            });
                            _rolePermissionCommand.SaveChanges();
                        }
                    }
                    _activityRepo.CreateActivityLog(string.Format("updated Portal Role with Name:{0}", applicationRole.Name), this.GetContollerName(), this.GetActionName(), User.Identity.GetUserId<Int64>(), applicationRole);

                    //TempData["MESSAGE"] = "Portal Role " + applicationRole.Name + " was successfully updated";
                    ModelState.Clear();

                    return Json(new { success = true });
                }else
                {
                    StringBuilder errorMsg = new StringBuilder();

                    foreach (var modelError in ModelState.Values.SelectMany(modelState => modelState.Errors))
                    {
                        errorMsg.AppendLine(modelError.ErrorMessage);
                        ModelState.AddModelError(string.Empty, modelError.ErrorMessage);
                    }
                    ViewBag.ErrMsg = errorMsg.ToString();
                    return PartialView("_PartialAddEdit", applicationRoleViewModel);
                }
            }
            catch (Exception exp)
            {
                _log.Error(exp);
                return View("Error");
            }
           
        }

        // GET: ApplicationRoles/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            ApplicationRole applicationRole = null;
            try
            {
                if (id <= 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                applicationRole = await RoleManager.FindByIdAsync(id);
                if (applicationRole == null)
                {
                    return HttpNotFound();
                }
                return View(applicationRole);
            }
            catch (Exception exp)
            {
                _log.Error(exp);
                return View("Error");
            }
           
        }

        // POST: ApplicationRoles/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, ApplicationRole applicationRole)
        {
            try
            {
                applicationRole = await RoleManager.FindByIdAsync(id);
                if (applicationRole.Name == "Admin")
                {
                    ModelState.AddModelError("", "You cannot delete the Admin role.");
                    return RedirectToAction("Index");
                }
                await RoleManager.DeleteAsync(applicationRole);
                TempData["MESSAGE"] = "Portal role " + applicationRole.Name + " was successfully deleted";
                ModelState.Clear();
                return RedirectToAction("Index");

            }
            catch (Exception exp)
            {
                _log.Error(exp);
                return View("Error");
            }
        }
    }
}