using SocialMediaApp.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApp.Application.Models
{
    public class Error
    {
        public ErrorCodes ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}
