using Learn.Core.DTOs;
using Learn.Core.Security;
using Learn.Core.Services.Interfaces;
using Learn.DataLayer.Entities.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Learn.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [PermissionChecker(2)]
    public class UserController : Controller
    {
        private IUserService _userService;
        private IPermissionService _permissionService;
        public UserController(IUserService userService, IPermissionService permissionService)
        {
            _userService = userService;
            _permissionService = permissionService;
        }
        public IActionResult Index(int? roleId, int pageId = 1, string filterEmail = "", string filterUserName = "")
        {
            //Sub Group
            List<SelectListItem> Roles = new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Text="نقش کاربران (خالی)",
                    Value=""
                }
            };
            Roles.AddRange(_userService.GetAllUserRoles());


            ViewData["Roles"] = new SelectList(Roles, "Value", "Text");
            return View(_userService.GetUsers(roleId, pageId, filterEmail, filterUserName));
        }
        public IActionResult DeletedUsers(int pageId = 1, string filterEmail = "", string filterUserName = "")
        {

            return View(_userService.GetDeleteUsers(pageId, filterEmail, filterUserName));
        }



        #region CreateUser
        public IActionResult CreateUser()
        {
            ViewData["Roles"] = _permissionService.GetRoles();
            return View();
        }

        [HttpPost]
        public IActionResult CreateUser(CreateUserViewModel createUserViewModel, List<int> SelectedRole)
        {
            ViewData["Roles"] = _permissionService.GetRoles();

            if (!ModelState.IsValid)
                return View(createUserViewModel);

            int userId = _userService.AddUserFromAdmin(createUserViewModel);

            //Add Roles
            if (SelectedRole != null)
                _permissionService.AddRolesToUser(SelectedRole, userId);

            return Redirect("/Admin/User/Index");
        }
        #endregion


        #region EditUser

        public IActionResult EditUser(int id)
        {
            ViewData["Roles"] = _permissionService.GetRoles();
            return View(_userService.GetUserForShowMode(id));
        }
        [HttpPost]
        public IActionResult EditUser(EditUserViewModel editUserViewModel, List<int> SelectedRole)
        {
            ViewData["Roles"] = _permissionService.GetRoles();

            if (!ModelState.IsValid)
                return View(editUserViewModel);

            _userService.EditUserFromAdmin(editUserViewModel);

            //EditRoles
            _permissionService.EditRolesUser(editUserViewModel.UserId, SelectedRole);

            return Redirect("/admin/user");
        }

        #endregion

        #region DeleteUser

        public IActionResult Delete(int id)
        {
            ViewData["UserId"] = id;

            return View(_userService.GetUserInformation(id));
        }
        [HttpPost]
        public IActionResult Delete(InformationUserViewModel informationUserViewModel, int userId)
        {
            _userService.DeleteUser(userId);
            return Redirect("/admin/user");
        }

        #endregion



        #region Roles

        public IActionResult RolesIndex()
        {
            return View(_permissionService.GetRoles());
        }
        public IActionResult CreateRole()
        {
            ViewData["Permissions"] = _permissionService.GetAllPermissions();
            return View();
        }
        [HttpPost]
        public IActionResult CreateRole(Role role, List<int> SelectedPermission)
        {
            ViewData["Permissions"] = _permissionService.GetAllPermissions();

            if (!ModelState.IsValid)
                return View(role);

            int roleId = _permissionService.AddRole(role);

            //TODO Add Permission
            _permissionService.AddPermissionsToRole(roleId, SelectedPermission);

            return Redirect("/Admin/user/RolesIndex");
        }
        public IActionResult EditRole(int id)
        {
            ViewData["Permissions"] = _permissionService.GetAllPermissions();
            ViewData["SelectedPermissions"] = _permissionService.PermissionsRole(id);
            return View(_permissionService.GetRoleByRoleId(id));
        }
        [HttpPost]
        public IActionResult EditRole(Role role, List<int> SelectedPermission)
        {
            ViewData["Permissions"] = _permissionService.GetAllPermissions();

            if (!ModelState.IsValid)
                return View(role);

            _permissionService.UpdateRole(role);

            //TODO Edit Permission
            _permissionService.UpdatePermissionsRole(role.RoleId, SelectedPermission);

            return Redirect("/admin/user/RolesIndex");
        }
        public IActionResult DeleteRole(int id)
        {
            return View(_permissionService.GetRoleByRoleId(id));
        }
        [HttpPost]
        public IActionResult DeleteRole(Role role)
        {
            _permissionService.DeleteRole(role);

            return Redirect("/admin/user/rolesIndex");
        }
        #endregion


    }
}
