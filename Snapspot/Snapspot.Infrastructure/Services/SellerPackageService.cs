using Snapspot.Application.Models.Companies;
using Snapspot.Application.Models.SellerPackages;
using Snapspot.Application.Repositories;
using Snapspot.Application.Services;
using Snapspot.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snapspot.Infrastructure.Services
{
    public class SellerPackageService : ISellerPackageService
    {
        private readonly ISellerPackageRepository _sellerPackageRepository;

        public SellerPackageService(ISellerPackageRepository sellerPackageRepository)
        {
            _sellerPackageRepository = sellerPackageRepository;
        }

        public async Task<SellerPackageDto> GetByIdAsync(Guid id)
        {
            var sellerPackage = await _sellerPackageRepository.GetByIdAsync(id);
            return sellerPackage != null ? MapToDto(sellerPackage) : null;
        }

        public async Task<IEnumerable<SellerPackageDto>> GetAllAsync()
        {
            var sellerPackages = await _sellerPackageRepository.GetAllAsync();
            return sellerPackages.Select(MapToDto);
        }

        public async Task<SellerPackageDto> CreateAsync(CreateSellerPackageDto createSellerPackageDto)
        {
            var sellerPackage = new SellerPackage
            {
                Name = createSellerPackageDto.Name,
                Description = createSellerPackageDto.Description,
                MaxAgency = createSellerPackageDto.MaxAgency,
                Price = createSellerPackageDto.Price,
                SellingCount = 0,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _sellerPackageRepository.AddAsync(sellerPackage);
            await _sellerPackageRepository.SaveChangesAsync();

            return MapToDto(sellerPackage);
        }

        public async Task<SellerPackageDto> UpdateAsync(Guid id, UpdateSellerPackageDto updateSellerPackageDto)
        {
            var sellerPackage = await _sellerPackageRepository.GetByIdAsync(id);
            if (sellerPackage == null)
                throw new Exception("SellerPackage not found");

            sellerPackage.Name = updateSellerPackageDto.Name;
            sellerPackage.Description = updateSellerPackageDto.Description;
            sellerPackage.MaxAgency = updateSellerPackageDto.MaxAgency;
            sellerPackage.Price = updateSellerPackageDto.Price;
            sellerPackage.UpdatedAt = DateTime.UtcNow;

            await _sellerPackageRepository.UpdateAsync(sellerPackage);
            await _sellerPackageRepository.SaveChangesAsync();

            return MapToDto(sellerPackage);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var sellerPackage = await _sellerPackageRepository.GetByIdAsync(id);
            if (sellerPackage == null)
                return false;

            await _sellerPackageRepository.DeleteAsync(sellerPackage);
            await _sellerPackageRepository.SaveChangesAsync();

            return true;
        }

        private static SellerPackageDto MapToDto(SellerPackage sellerPackage)
        {
            return new SellerPackageDto
            {
                Id = sellerPackage.Id,
                Name = sellerPackage.Name,
                Description = sellerPackage.Description,
                MaxAgency = sellerPackage.MaxAgency,
                Price = sellerPackage.Price,
                SellingCount = sellerPackage.SellingCount,
                IsDeleted = sellerPackage.IsDeleted,
                CreatedAt = sellerPackage.CreatedAt,
                UpdatedAt = sellerPackage.UpdatedAt,
                Companies = sellerPackage.Companies?.Select(c => new CompanyDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Address = c.Address,
                    Email = c.Email,
                    PhoneNumber = c.PhoneNumber,
                    AvatarUrl = c.AvatarUrl,
                    PdfUrl = c.PdfUrl,
                    Rating = c.Rating,
                    IsApproved = c.IsApproved,
                    UserId = c.UserId,
                    UserName = c.User?.Fullname,
                    Agencies = null, // Tránh vòng lặp vô hạn
                    CreatedAt = c.CreatedAt,
                    UpdatedAt = c.UpdatedAt,
                    IsDeleted = c.IsDeleted
                }).ToList()
            };
        }
    }
} 