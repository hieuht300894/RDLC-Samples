using System;
using System.Collections.Generic;
using System.Text;

namespace RDLC.Models
{
    public class AppData
    {
        public List<User> Users { get; } = new List<User>();

        public AppData()
        {
            Users.Add(new User { Id = 1, Name = "HTH", FullName = "Hieu Hoang", IsActive = true });
            Users.Add(new User { Id = 2, Name = "HW", FullName = "Hello World", IsActive = false });
        }
    }
}
