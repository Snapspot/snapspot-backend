using Microsoft.EntityFrameworkCore;
using Snapspot.Application.Models.Agencies;
using Snapspot.Application.Models.AgencyServices;
using Snapspot.Application.Models.Companies;
using Snapspot.Application.Repositories;
using Snapspot.Application.Services;
using Snapspot.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snapspot.Infrastructure.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;

        public CompanyService(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public async Task<CompanyDto> GetByIdAsync(Guid id)
        {
            var company = await _companyRepository.GetByIdAsync(id);
            return company != null ? MapToDto(company) : null;
        }

        public async Task<IEnumerable<CompanyDto>> GetAllAsync()
        {
            var companies = await _companyRepository.GetAllAsync();
            return companies.Select(MapToDto);
        }

        public async Task<CompanyDto> GetByUserIdAsync(Guid userId)
        {
            var company = await _companyRepository.GetByUserIdAsync(userId);
            return company != null ? MapToDto(company) : null;
        }

        public async Task<CompanyDto> CreateAsync(CreateCompanyDto createCompanyDto)
        {
            var company = new Company
            {
                Name = createCompanyDto.Name,
                Address = createCompanyDto.Address,
                Email = createCompanyDto.Email,
                PhoneNumber = createCompanyDto.PhoneNumber,
                AvatarUrl = createCompanyDto.AvatarUrl,
                PdfUrl = createCompanyDto.PdfUrl,
                Rating = 0,
                IsApproved = false,
                UserId = createCompanyDto.UserId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsDeleted = false
            };

            await _companyRepository.AddAsync(company);
            await _companyRepository.SaveChangesAsync();

            return MapToDto(company);
        }

        public async Task<CompanyDto> UpdateAsync(Guid id, UpdateCompanyDto updateCompanyDto)
        {
            var company = await _companyRepository.GetByIdAsync(id);
            if (company == null)
                throw new Exception("Company not found");

            company.Name = updateCompanyDto.Name;
            company.Address = updateCompanyDto.Address;
            company.Email = updateCompanyDto.Email;
            company.PhoneNumber = updateCompanyDto.PhoneNumber;
            company.AvatarUrl = updateCompanyDto.AvatarUrl;
            company.PdfUrl = updateCompanyDto.PdfUrl;
            company.UpdatedAt = DateTime.UtcNow;

            await _companyRepository.UpdateAsync(company);
            await _companyRepository.SaveChangesAsync();

            return MapToDto(company);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var company = await _companyRepository.GetByIdAsync(id);
            if (company == null)
                return false;

            await _companyRepository.DeleteAsync(company);
            await _companyRepository.SaveChangesAsync();

            return true;
        }

        private static CompanyDto MapToDto(Company company)
        {
            return new CompanyDto
            {
                Id = company.Id,
                Name = company.Name,
                Address = company.Address,
                Email = company.Email,
                PhoneNumber = company.PhoneNumber,
                AvatarUrl = company.AvatarUrl,
                PdfUrl = company.PdfUrl,
                Rating = company.Rating,
                IsApproved = company.IsApproved,
                UserId = company.UserId,
                UserName = company.User?.Fullname,
                Agencies = company.Agencies?.Select(a => new AgencyDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    Address = a.Address,
                    Fullname = a.Fullname,
                    PhoneNumber = a.PhoneNumber,
                    AvatarUrl = a.AvatarUrl,
                    Rating = a.Rating,
                    CompanyId = a.CompanyId,
                    CompanyName = company.Name,
                    SpotId = a.SpotId,
                    SpotName = a.Spot?.Name,
                    Services = a.Services?.Select(s => new AgencyServiceDto
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Color = s.Color
                    }).ToList(),
                    Feedbacks = a.Feedbacks?.Select(f => new FeedbackDto
                    {
                        Id = f.Id,
                        Content = f.Content,
                        Rating = f.Rating,
                        IsApproved = f.IsApproved,
                        UserId = f.UserId,
                        UserName = f.User?.Fullname,
                        AgencyId = f.AgencyId,
                        CreatedAt = f.CreatedAt,
                        UpdatedAt = f.UpdatedAt,
                        IsDeleted = f.IsDeleted
                    }).ToList(),
                    CreatedAt = a.CreatedAt,
                    UpdatedAt = a.UpdatedAt,
                    IsDeleted = a.IsDeleted,
                    Description = a.Description
                }).ToList(),
                CreatedAt = company.CreatedAt,
                UpdatedAt = company.UpdatedAt,
                IsDeleted = company.IsDeleted
            };
        }
    }
} 