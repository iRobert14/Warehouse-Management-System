using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryCommon.Models
{
    public enum UserRole
    {
        Admin,
        Cashier,
        Manager
    }
    public class User
    {
        Guid UserId { get; set; }
        public string Username { get; set; }
        public UserRole UserRole { get; set; }
        public string PasswordHash { get; set; }


    }
}
