using Snapspot.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snapspot.Application.Repositories
{
    public interface ICompanySellerPackageRepository
    {
       
        Task<IEnumerable<Guid>> GetCompaniesBySellerPackageAsync(Guid sellerPackageId);
        Task<IEnumerable<Guid>> GetSellerPackagesByCompanyAsync(Guid companyId);
        Task<CompanySellerPackage?> CompanyInSellerPackageAsync(Guid companyId, Guid sellerPackageId)

        Task Add(CompanySellerPackage companySellerPackage);
        Task SaveChangesAsync();
    }
}
