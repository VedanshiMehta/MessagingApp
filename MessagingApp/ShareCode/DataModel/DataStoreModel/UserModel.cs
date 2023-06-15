using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingApp
{
    public class UserModel
    {
        public string Id { get; set;}
        public string Name { get; set; }
        public string MobileNumber { get; set; }
        public DateTime DateOfBirth { get; set; }

        public string Bio {  get; set; }
        public string ProfileImage { get; set; }

        public bool IsProfileImageRemoved { get; set; }
    }
}
