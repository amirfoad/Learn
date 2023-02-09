using System;
using System.Collections.Generic;
using System.Text;
using Learn.DataLayer.Entities.Permissions;
using Learn.DataLayer.Entities.User;

namespace Learn.Core.Services.Interfaces
{
    public interface IPermissionService
    {
        #region Roles

        List<Role> GetRoles();
        int AddRole(Role role);
        void AddRolesToUser(List<int> roleIds, int userId);
        Role GetRoleByRoleId(int roleId);
        void UpdateRole(Role role);
        void DeleteRole(Role role);
        void EditRolesUser(int userId, List<int> roleIds);

        #endregion

        #region Permissions

        List<Permission> GetAllPermissions();
        void AddPermissionsToRole(int roleId, List<int> permissions);
        List<int> PermissionsRole(int roleId);
        void UpdatePermissionsRole(int roleId, List<int> permissions);
        bool CheckPermission(int permissionId, string userName);
        #endregion
    }
}
