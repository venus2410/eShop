using eShop.ViewModel.Catalog.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.ViewModel.System.Users
{
    public class UserRoleAssignRequest
    {
        public Guid Id { get; set; }
        public List<SelectItem> Roles { get; set; }=new List<SelectItem>();
    }
}
