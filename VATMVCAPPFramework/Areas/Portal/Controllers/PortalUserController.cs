using log4net;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VATMVCAPPFramework.Controllers;
using VATMVCAPPFramework.Data.Constant;
using VATMVCAPPFramework.Data.Core;
using VATMVCAPPFramework.Data.IdentityModel;
using VATMVCAPPFramework.Repository;
using VATMVCAPPFramework.Repository.CoreRepositories;
using VATMVCAPPFramework.Utilities;
using VATMVCAPPFramework.ViewModel;

namespace VATMVCAPPFramework.Areas.Portal.Controllers
{
    [Authorize(Roles = "PortalAdmin")]
    public class PortalUserController : BaseController
    {

        private readonly IRepositoryQuery<ApplicationUser> _applicationUserQuery;
        private readonly IRepositoryCommand<ApplicationUser> _applicationUserCommand;
        private readonly IRepositoryCommand<ApplicationUserPasswordHistory> _applicationUserPwdhistoryCommand;
        private readonly IActivityLogRepositoryCommand _activityRepo;
        private readonly ILog _log;
        private readonly Utility _utility;

        public PortalUserController(IActivityLogRepositoryCommand activityRepo, 
            IRepositoryCommand<ApplicationUser> applicationUserCommand,
            IRepositoryQuery<ApplicationUser> applicationUser, 
            IRepositoryCommand<ApplicationUserPasswordHistory> applicationUserPwdhistory, Utility utility, ILog log)
        {

            _applicationUserQuery = applicationUser;
            _applicationUserCommand = applicationUserCommand;
            _applicationUserPwdhistoryCommand = applicationUserPwdhistory;
            _activityRepo = activityRepo;
            _utility = utility;
            _log = log;
        }

        // GET: Portal/PortalUser
        public async Task<ActionResult> Index()
        {
            try
            {
                _log.Info("<<< In Portal User Page >>>");
                if (TempData["MESSAGE"] != null)
                {
                    ViewBag.Msg = TempData["MESSAGE"] as string;
                }
                var usermodel = await _applicationUserQuery.ExecuteStoredProdure<UserViewModel>("SpGetAspNetUsers").ToListAsync();
                //var usermodel = _applicationUserQuery.GetAll().Select(UserViewModel.EntityToModels).ToList();
                return View(usermodel);
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                return View("Error");
            }

        }

        public IEnumerable<SelectListItem> GetRoles()
        {

            var types = RoleManager.Roles.ToList().Select(x =>
                                new SelectListItem
                                {
                                    Value = x.Name,
                                    Text = x.Name

                                }).AsEnumerable();
            return new SelectList(types, "Value", "Text");
        }


        // GET: ApplicationRoles/Create
        public ActionResult Create()
        {
            CreateViewBagParams();
            return PartialView("_PartialAddEdit", new UserViewModel { Roles = _utility.GetSpecificRolesStringValue(new long[] { 1 }) });
        }

        [HttpPost]
        public async Task<ActionResult> Create(UserViewModel model)
        {
            string code = string.Empty;
            model.Roles = _utility.GetSpecificRolesStringValue(new long[] { 1 });
            try
            {
                CreateViewBagParams();
                if (ModelState.IsValid)
                {
                    //checking if emailaddress does not exist b4
                    var organizerAdminEmailExist = _applicationUserQuery.GetAllList(m => m.Email.ToLower().Trim() == model.Email.ToLower().Trim()).ToList();
                    if (organizerAdminEmailExist.Any())
                    {
                        ModelState.AddModelError("", "email address already exist");
                        return PartialView("_PartialAddEdit", model);
                    }

                    //checking if username does not exist b4
                    var organizerAdminUsernameExist = _applicationUserQuery.GetAllList(m => m.UserName.ToLower().Trim() == model.UserName.ToLower().Trim()).ToList();
                    if (organizerAdminUsernameExist.Any())
                    {
                        ModelState.AddModelError("", "username already exist");
                        return PartialView("_PartialAddEdit", model);
                    }
                    ApplicationUser usermodel = UserViewModel.ModeltoEntity(model);

                    var result = await UserManager.CreateAsync(usermodel, "Password");
                    if (result.Succeeded)
                    {
                        _activityRepo.CreateActivityLog(string.Format("Assinging User Id:{0} with Name :{1} to role Id's:{2}", usermodel.Id, (usermodel.LastName + " " + usermodel.FirstName), model.SelectedRole), this.GetContollerName(), this.GetActionName(), usermodel.Id, null);

                        ApplicationUserPasswordHistory passwordModel = new ApplicationUserPasswordHistory();
                        passwordModel.UserId = usermodel.Id;
                        passwordModel.DateCreated = DateTime.Now;
                        passwordModel.HashPassword = ExtentionUtility.Encrypt("Password");
                        passwordModel.CreatedBy = usermodel.Id;
                        _applicationUserPwdhistoryCommand.Insert(passwordModel);
                        _applicationUserPwdhistoryCommand.Save();

                        var addRoleResult = await UserManager.AddToRolesAsync(usermodel.Id, model.SelectedRole.ToArray<string>());
                        if (addRoleResult.Succeeded)
                        {
                            //send user reset mail
                            code = await UserManager.GeneratePasswordResetTokenAsync(usermodel.Id);
                            string portalUrl = System.Web.HttpContext.Current.Request.Url.Scheme + "://" + System.Web.HttpContext.Current.Request.Url.Authority + System.Web.HttpContext.Current.Request.ApplicationPath.TrimEnd('/') + "/";

                            var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = usermodel.Id, code = code });
                            string mPre = portalUrl + callbackUrl;
                            _log.Info(string.Format("Reset URL:{0}", mPre));
                            if (!string.IsNullOrEmpty(usermodel.Email))
                            {
                                _utility.SendWelcomeAndPasswordResetEmail(usermodel, mPre);
                            }


                            TempData["MESSAGE"] = "Portal User " + (usermodel.LastName + " " + usermodel.FirstName) + " was successfully created";
                            ModelState.Clear();
                            return Json(new { success = true });
                        }

                    }
                    else
                    {
                        ModelState.AddModelError("", result.Errors.FirstOrDefault().ToString());
                    }
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
                //return View("Error");
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


        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                EditViewBagParams();
                if (id <= 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ApplicationUser userModel = await _applicationUserQuery.GetAsync(id);
                UserViewModel userdataModel = UserViewModel.EntityToModels(userModel);
                if (userModel == null)
                {
                    return HttpNotFound();
                }
                userdataModel.Roles = GetRoles();
                userdataModel.SelectedRole = _applicationUserQuery.StoreprocedureQueryFor<UserRoleInfo>(AppConstant.FetchUserRoleByUserId + " @UserId", new SqlParameter("UserId", id)).Select(c => c.Name).ToList();
                return PartialView("_PartialAddEdit", userdataModel);
            }
            catch (Exception exp)
            {
                _log.Error(exp);
                return View("Error");
            }
        }


        [HttpPost]
        public async Task<ActionResult> Edit(UserViewModel model)
        {
            string code = string.Empty;
            model.Roles = GetRoles();
            try
            {
                EditViewBagParams();
                if (ModelState.IsValid)
                {
                    ////checking if emailaddress does not exist b4
                    //var organizerAdminEmailExist = _applicationUserQuery.GetAllList(m => m.Email.ToLower().Trim() == model.Email.ToLower().Trim() && m.Id!=model.Id).ToList();
                    //if (organizerAdminEmailExist.Any())
                    //{
                    //    ModelState.AddModelError("", "email address already exist");
                    //    return PartialView("_PartialAddEdit", model);
                    //}

                    ////checking if username does not exist b4
                    //var organizerAdminUsernameExist = _applicationUserQuery.GetAllList(m => m.UserName.ToLower().Trim() == model.UserName.ToLower().Trim() && m.Id != model.Id).ToList();
                    //if (organizerAdminUsernameExist.Any())
                    //{
                    //    ModelState.AddModelError("", "username already exist");
                    //    return PartialView("_PartialAddEdit", model);
                    //}

                    ApplicationUser usermodel =await _applicationUserQuery.GetAsync(model.Id);
                    if(usermodel!= null)
                    {
                        usermodel.FirstName = model.FirstName;
                        usermodel.LastName = model.LastName;
                        usermodel.MiddleName = model.MiddleName;
                        usermodel.MobileNumber = model.MobileNumber;
                        usermodel.PhoneNumber = model.PhoneNumber;

                        //try
                        //{
                        //    if (!string.IsNullOrEmpty(model.DOB))
                        //    {
                        //        usermodel.DOB = ExtentionUtility.GetDateValue(model.DOB);
                        //    }

                        //}
                        //catch 
                        //{

                        //}
                        //usermodel.Address = model.Address;
                        await _applicationUserCommand.UpdateAsync(usermodel);
                        await _applicationUserCommand.SaveChangesAsync();
                        _activityRepo.CreateActivityLog(string.Format("Updtae User Id:{0} with Name :{1}", usermodel.Id, (usermodel.LastName + " " + usermodel.FirstName)), this.GetContollerName(), this.GetActionName(), usermodel.Id, null);


                        //update UserRole
                        //.First delete role if exist
                        //Add the new role
                        _applicationUserQuery.ExecuteStoreprocedure(AppConstant.DeleteUserRoleByUserId + " @UserId", new SqlParameter("UserId", usermodel.Id));
                        await UserManager.AddToRolesAsync(usermodel.Id, model.SelectedRole.ToArray<string>());

                        _activityRepo.CreateActivityLog(string.Format("Assinging User Id:{0} with Name :{1} to role Id's:{2}", usermodel.Id, (usermodel.LastName + " " + usermodel.FirstName), model.SelectedRole), this.GetContollerName(), this.GetActionName(), usermodel.Id, null);
                        
                        TempData["MESSAGE"] = "Portal User " + (usermodel.LastName + " " + usermodel.FirstName) + " was successfully updated";
                        ModelState.Clear();
                        return Json(new { success = true });

                    }
                    else
                    {
                        ModelState.AddModelError("", "An error occurred while processing your request");
                    }
                    ModelState.Clear();
                    return Json(new { success = true });

                }
                else
                {
                    StringBuilder errorMsg = new StringBuilder();

                    try
                    {
                        foreach (var modelError in ModelState.Values.SelectMany(modelState => modelState.Errors))
                        {
                            errorMsg.AppendLine(modelError.ErrorMessage);
                            ModelState.AddModelError(string.Empty, modelError.ErrorMessage);
                        }
                        ViewBag.ErrMsg = errorMsg.ToString();
                    }
                    catch
                    {
                        
                    }
                   
                    
                    return PartialView("_PartialAddEdit", ViewBag.ErrMsg ?? "An error occurred while processing your request");
                }
            }
            catch (Exception exp)
            {
                _log.Error(exp);
                //return View("Error");
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


        // GET: ApplicationUser/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            ApplicationUser usermodel = null;
            try
            {
                if (id <= 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                usermodel = await _applicationUserQuery.GetAsync(id);
                if (usermodel == null)
                {
                    return HttpNotFound();
                }
                return View(usermodel);
            }
            catch (Exception exp)
            {
                _log.Error(exp);
                return View("Error");
            }

        }

        // POST: ApplicationUser/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, ApplicationUser user)
        {
            try
            {
                user = await _applicationUserQuery.GetAsync(id);
                if (user.Id == 1)
                {
                    ModelState.AddModelError("", "You cannot delete the a portal user.");
                    return RedirectToAction("Index");
                }
                user.IsActive = false;
                user.IsDeleted = true;
                await _applicationUserCommand.UpdateAsync(user);
                await _applicationUserCommand.SaveChangesAsync();
                TempData["MESSAGE"] = "Portal user " + user.LastName + " " + user.FirstName + " was successfully deleted";
                ModelState.Clear();
                return RedirectToAction("Index");

            }
            catch (Exception exp)
            {
                _log.Error(exp);
                return View("Error");
            }
        }


        public async Task<ActionResult> Deactivate(string code)
        {
            long userId = ExtentionUtility.DecryptID(code);
            try
            {
                ApplicationUser usermodel = await _applicationUserQuery.GetAsync(userId);
                usermodel.IsActive = false;
                await _applicationUserCommand.UpdateAsync(usermodel);
                await _applicationUserCommand.SaveChangesAsync();

                TempData["MESSAGE"] = "User " + (usermodel.LastName + ' ' + usermodel.FirstName) + " was successfully deactivated";
                ModelState.Clear();

                return RedirectToAction("Index");

            }
            catch (Exception exp)
            {
                _log.Error(exp);
                StringBuilder errorMsg = new StringBuilder();
                foreach (var modelError in ModelState.Values.SelectMany(modelState => modelState.Errors))
                {
                    errorMsg.AppendLine(modelError.ErrorMessage);
                    ModelState.AddModelError(string.Empty, modelError.ErrorMessage);
                }
                ViewBag.ErrMsg = errorMsg.ToString();
                return View("Error");
            }
        }


        public async Task<ActionResult> Activate(string code)
        {
            long userId = ExtentionUtility.DecryptID(code);
            try
            {
                ApplicationUser usermodel = await _applicationUserQuery.GetAsync(userId);
                usermodel.IsActive = false;
                await _applicationUserCommand.UpdateAsync(usermodel);
                await _applicationUserCommand.SaveChangesAsync();

                TempData["MESSAGE"] = "User " + (usermodel.LastName + ' ' + usermodel.FirstName) + " was successfully activated";
                ModelState.Clear();

                return RedirectToAction("Index");
                
            }
            catch (Exception exp)
            {
                _log.Error(exp);
                StringBuilder errorMsg = new StringBuilder();
                foreach (var modelError in ModelState.Values.SelectMany(modelState => modelState.Errors))
                {
                    errorMsg.AppendLine(modelError.ErrorMessage);
                    ModelState.AddModelError(string.Empty, modelError.ErrorMessage);
                }
                ViewBag.ErrMsg = errorMsg.ToString();
                return View("Error");
            }
        }

    }
}