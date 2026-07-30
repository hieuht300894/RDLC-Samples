using System.Collections.Generic;

namespace RDLC.Models
{
    public class AppData
    {
        public List<User> Users { get; } = new List<User>();
        public List<Report> Reports { get; } = new List<Report>();

        public AppData()
        {
            Users.Add(new User { Id = 1, Name = "HTH", FullName = "Hieu Hoang", IsActive = true });
            Users.Add(new User { Id = 2, Name = "HW", FullName = "Hello World", IsActive = false });

            Reports.Add(new Report { Id = 1, Name = "User", FileName = "User.rdlc" });
            Reports.Add(new Report { Id = 2, Name = "Barcode", FileName = "Barcode.rdlc" });
        }
    }
}
