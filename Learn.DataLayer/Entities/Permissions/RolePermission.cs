using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Learn.DataLayer.Entities.User;

namespace Learn.DataLayer.Entities.Permissions
{
   public  class RolePermission
    {
        [Key]
        public int RPId { get; set; }
        public int RoleId { get; set; }
        public int PermissionId { get; set; }

        #region Relations

        public virtual Role Role { get; set; }
        public virtual Permission Permission { get; set; }

        #endregion

    }
}
