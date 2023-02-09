﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Learn.DataLayer.Entities.Permissions
{
    public class Permission
    {
        [Key]
        public int PermissionId { get; set; }
        [Display(Name = "عنوان نقش")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد.")]
        public string PermissionTitle { get; set; }
        public int? ParentId { get; set; }

        #region Relations

        [ForeignKey("ParentId")]
        public virtual List<Permission> permissions { get; set; }

        public virtual List<RolePermission> RolePermissions { get; set; }


        #endregion

    }
}
