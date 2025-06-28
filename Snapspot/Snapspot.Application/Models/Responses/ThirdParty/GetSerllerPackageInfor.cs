using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snapspot.Application.Models.Responses.ThirdParty
{
    public class GetSerllerPackageInfor
    {
        public string PackageName { get; set; } = string.Empty;
        public string PackageImageUrl { get; set; } = string.Empty;
        public int CurrentAgency { get; set; } = 0;
        public int TotalAgency { get; set; } = 0;
        public int RemainingDay { get; set; } = 0;
    }
}
