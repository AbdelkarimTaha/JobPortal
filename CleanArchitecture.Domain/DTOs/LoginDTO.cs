using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Domain.DTOs
{
    public class LoginDTO
    {
        public object Username { get; set; }
        public object Token { get; set; }
    }
}
