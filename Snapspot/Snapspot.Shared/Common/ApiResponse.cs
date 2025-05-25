using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snapspot.Shared.Common
{
    public class ApiResponse<T> where T : class
    {
        public bool Success { get; set; }
        public string MessageId { get; set; }
        public string Message { get; set; }
        public T? Data { get; set; }
        public List<DetailError>? ListDetailError { get; set; }
    }
}
