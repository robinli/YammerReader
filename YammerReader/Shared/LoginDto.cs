using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YammerReader.Shared
{
    public class AppUserDto
    {
        public AppUserDto()
        {
            this.Roles = new List<string>();
        }

        public int ID { get; set; }
        public string LOGIN_ID { get; set; }
        public string USER_NAME { get; set; }
        public string USER_EMAIL { get; set; }
        public List<string> Roles { get; set; }
    }

    public class LoginDto
    {
        [Required]
        public string LOGIN_ID { get; set; }

        [Required]
        public string USER_PASSWORD { get; set; }
    }
}
