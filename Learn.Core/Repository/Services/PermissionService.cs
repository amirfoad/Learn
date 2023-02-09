using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Learn.Core.Services.Interfaces;
using Learn.DataLayer.Context;
using Learn.DataLayer.Entities.Permissions;
using Learn.DataLayer.Entities.User;

namespace Learn.Core.Services
{
    public class PermissionService : IPermissionService
    {
        private LearnContext _context;
        public PermissionService(LearnContext context)
        {
            _context = context;
        }

        public void AddPermissionsToRole(int roleId, List<int> permissions)
        {
            foreach (var item in permissions)
            {
                _context.RolePermission.Add(new RolePermission()
                {
                    PermissionId = item,
                    RoleId = roleId
                });
                _context.SaveChanges();
            }
        }

        public int AddRole(Role role)
        {
            _context.Roles.Add(role);
            _context.SaveChanges();
            return role.RoleId;
        }

        public void AddRolesToUser(List<int> roleIds, int userId)
        {
            foreach (var item in roleIds)
            {
                _context.UserRoles.Add(new UserRole()
                {
                    RoleId = item,
                    UserId = userId
                });

            }
            _context.SaveChanges();
        }

        public bool CheckPermission(int permissionId, string userName)
        {
            int userId = _context.Users.Single(u => u.UserName == userName).UserId;
            List<int> UserRoles = _context.UserRoles.Where(r => r.UserId == userId)
                .Select(r => r.RoleId).ToList();

            if(!UserRoles.Any())
            {
                return false;
            }

            List<int> RolesPermission = _context.RolePermission
                .Where(u => u.PermissionId == permissionId)
                .ToList().Select(p => p.RoleId)
                .ToList();

            return RolesPermission.Any(p => UserRoles.Contains(p));
        }

        public void DeleteRole(Role role)
        {
            role.IsDelete = true;
            UpdateRole(role);
        }

        public void EditRolesUser(int userId, List<int> roleIds)
        {
            //Delete All Roles
            _context.UserRoles.Where(r => r.UserId == userId).ToList().ForEach(r => _context.UserRoles.Remove(r));

            //Add New Roles
            AddRolesToUser(roleIds, userId);

        }

        public List<Permission> GetAllPermissions()
        {
            return _context.Permission.ToList();
        }

        public Role GetRoleByRoleId(int roleId)
        {
            return _context.Roles.Find(roleId);
        }

        public List<Role> GetRoles()
        {
            return _context.Roles.ToList();
        }

        public List<int> PermissionsRole(int roleId)
        {
            return _context.RolePermission
                .Where(p => p.RoleId == roleId)
                .Select(p => p.PermissionId).ToList();
        }

        public void UpdatePermissionsRole(int roleId, List<int> permissions)
        {
            _context.RolePermission
                .Where(r => r.RoleId == roleId)
                .ToList()
                .ForEach(p => _context.Remove(p));
            AddPermissionsToRole(roleId, permissions);
        }

        public void UpdateRole(Role role)
        {
            _context.Roles.Update(role);
            _context.SaveChanges();
        }
    }
}
