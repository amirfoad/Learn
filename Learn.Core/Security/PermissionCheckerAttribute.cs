using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using Learn.Core.Services.Interfaces;

namespace Learn.Core.Security
{
    public class PermissionCheckerAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private int _PermissionId = 0;
        private IPermissionService _permissionService;
        public PermissionCheckerAttribute(int permissionId)
        {
            _PermissionId = permissionId;
       
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //Inject kardane interface ba aye estefade az constructor ba estefade az context
            _permissionService = (IPermissionService)context.HttpContext.RequestServices.GetService(typeof(IPermissionService));
            if(context.HttpContext.User.Identity.IsAuthenticated)
            {
                string userName = context.HttpContext.User.Identity.Name;
                if(!_permissionService.CheckPermission(_PermissionId,userName))
                {
                    context.Result = new RedirectResult("/Login?"+context.HttpContext.Request.Path);

                }
            }
            else
            {
                context.Result = new RedirectResult("/Login");
            }
        }
    }
}
