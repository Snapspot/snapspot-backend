using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snapspot.Shared.Common
{
    public class DetailError
    {
        public string Field { get; set; }
        public string Value { get; set; }
        public string MessageId { get; set; }
        public string Message { get; set; }
    }
}
