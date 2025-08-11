using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snapspot.Application.Models.Styles
{
    public class CreateStyleDto
    {
        public string Category { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
    }
}
