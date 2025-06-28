using Snapspot.Application.Models.Companies;
using Snapspot.Application.Repositories;
using Snapspot.Application.UseCases.Interfaces.Company;
using Snapspot.Domain.Entities;
using Snapspot.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snapspot.Application.UseCases.Implementations.Company
{
    public class CompanyUseCase : ICompanyUseCase
    {
        private readonly ICompanyRepository _companyRepository;

        public CompanyUseCase(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public async Task<ApiResponse<CompanyDto>> GetByIdAsync(Guid id)
        {
            try
            {
                var company = await _companyRepository.GetByIdAsync(id);
                if (company == null)
                {
                    return new ApiResponse<CompanyDto>
                    {
                        Success = false,
                        MessageId = MessageId.E0000,
                        Message = "Company not found"
                    };
                }

                var companyDto = new CompanyDto
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
                    UserName = company.User?.Fullname ?? "",
                    CreatedAt = company.CreatedAt,
                    UpdatedAt = company.UpdatedAt,
                    IsDeleted = company.IsDeleted
                };

                return new ApiResponse<CompanyDto>
                {
                    Data = companyDto,
                    Success = true,
                    MessageId = MessageId.I0000,
                    Message = Message.GetMessageById(MessageId.I0000)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<CompanyDto>
                {
                    Success = false,
                    MessageId = MessageId.E0000,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<IEnumerable<CompanyDto>>> GetAllAsync()
        {
            try
            {
                var companies = await _companyRepository.GetAllAsync();
                
                var companyDtos = companies.Select(c => new CompanyDto
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
                    UserName = c.User?.Fullname ?? "",
                    CreatedAt = c.CreatedAt,
                    UpdatedAt = c.UpdatedAt,
                    IsDeleted = c.IsDeleted
                }).ToList();

                return new ApiResponse<IEnumerable<CompanyDto>>
                {
                    Data = companyDtos,
                    Success = true,
                    MessageId = MessageId.I0000,
                    Message = Message.GetMessageById(MessageId.I0000)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<CompanyDto>>
                {
                    Success = false,
                    MessageId = MessageId.E0000,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<CompanyDto>> CreateAsync(CreateCompanyDto createCompanyDto)
        {
            try
            {
                // Business logic: Validate email format
                if (!string.IsNullOrEmpty(createCompanyDto.Email) && !IsValidEmail(createCompanyDto.Email))
                {
                    return new ApiResponse<CompanyDto>
                    {
                        Success = false,
                        MessageId = MessageId.E0000,
                        Message = "Invalid email format"
                    };
                }

                var company = new Domain.Entities.Company
                {
                    Id = Guid.NewGuid(),
                    Name = createCompanyDto.Name,
                    Address = createCompanyDto.Address,
                    Email = createCompanyDto.Email,
                    PhoneNumber = createCompanyDto.PhoneNumber,
                    AvatarUrl = createCompanyDto.AvatarUrl,
                    PdfUrl = createCompanyDto.PdfUrl,
                    UserId = createCompanyDto.UserId,
                    Rating = 0,
                    IsApproved = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false
                };

                await _companyRepository.AddAsync(company);
                await _companyRepository.SaveChangesAsync();

                var companyDto = new CompanyDto
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
                    UserName = "",
                    CreatedAt = company.CreatedAt,
                    UpdatedAt = company.UpdatedAt,
                    IsDeleted = company.IsDeleted
                };

                return new ApiResponse<CompanyDto>
                {
                    Data = companyDto,
                    Success = true,
                    MessageId = MessageId.I0000,
                    Message = Message.GetMessageById(MessageId.I0000)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<CompanyDto>
                {
                    Success = false,
                    MessageId = MessageId.E0000,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<CompanyDto>> UpdateAsync(Guid id, UpdateCompanyDto updateCompanyDto)
        {
            try
            {
                var company = await _companyRepository.GetByIdAsync(id);
                if (company == null)
                {
                    return new ApiResponse<CompanyDto>
                    {
                        Success = false,
                        MessageId = MessageId.E0000,
                        Message = "Company not found"
                    };
                }

                // Business logic: Validate email format
                if (!string.IsNullOrEmpty(updateCompanyDto.Email) && !IsValidEmail(updateCompanyDto.Email))
                {
                    return new ApiResponse<CompanyDto>
                    {
                        Success = false,
                        MessageId = MessageId.E0000,
                        Message = "Invalid email format"
                    };
                }

                // Update fields
                if (!string.IsNullOrEmpty(updateCompanyDto.Name))
                    company.Name = updateCompanyDto.Name;
                
                if (!string.IsNullOrEmpty(updateCompanyDto.Address))
                    company.Address = updateCompanyDto.Address;
                
                if (!string.IsNullOrEmpty(updateCompanyDto.Email))
                    company.Email = updateCompanyDto.Email;
                
                if (!string.IsNullOrEmpty(updateCompanyDto.PhoneNumber))
                    company.PhoneNumber = updateCompanyDto.PhoneNumber;
                
                if (!string.IsNullOrEmpty(updateCompanyDto.AvatarUrl))
                    company.AvatarUrl = updateCompanyDto.AvatarUrl;
                
                if (!string.IsNullOrEmpty(updateCompanyDto.PdfUrl))
                    company.PdfUrl = updateCompanyDto.PdfUrl;

                company.UpdatedAt = DateTime.UtcNow;

                await _companyRepository.UpdateAsync(company);
                await _companyRepository.SaveChangesAsync();

                var companyDto = new CompanyDto
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
                    UserName = company.User?.Fullname ?? "",
                    CreatedAt = company.CreatedAt,
                    UpdatedAt = company.UpdatedAt,
                    IsDeleted = company.IsDeleted
                };

                return new ApiResponse<CompanyDto>
                {
                    Data = companyDto,
                    Success = true,
                    MessageId = MessageId.I0000,
                    Message = Message.GetMessageById(MessageId.I0000)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<CompanyDto>
                {
                    Success = false,
                    MessageId = MessageId.E0000,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<string>> DeleteAsync(Guid id)
        {
            try
            {
                var company = await _companyRepository.GetByIdAsync(id);
                if (company == null)
                {
                    return new ApiResponse<string>
                    {
                        Success = false,
                        MessageId = MessageId.E0000,
                        Message = "Company not found"
                    };
                }

                await _companyRepository.DeleteAsync(company);
                await _companyRepository.SaveChangesAsync();

                return new ApiResponse<string>
                {
                    Success = true,
                    MessageId = MessageId.I0000,
                    Message = "Company deleted successfully"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    MessageId = MessageId.E0000,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<IEnumerable<CompanyDto>>> SearchCompaniesAsync(string searchTerm)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    return new ApiResponse<IEnumerable<CompanyDto>>
                    {
                        Success = false,
                        MessageId = MessageId.E0000,
                        Message = "Search term cannot be empty"
                    };
                }

                // Since SearchAsync method doesn't exist, we'll implement a simple search
                var companies = await _companyRepository.GetAllAsync();
                var filteredCompanies = companies.Where(c => 
                    c.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    c.Email.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    c.Address.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
                );
                
                var companyDtos = filteredCompanies.Select(c => new CompanyDto
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
                    UserName = c.User?.Fullname ?? "",
                    CreatedAt = c.CreatedAt,
                    UpdatedAt = c.UpdatedAt,
                    IsDeleted = c.IsDeleted
                }).ToList();

                return new ApiResponse<IEnumerable<CompanyDto>>
                {
                    Data = companyDtos,
                    Success = true,
                    MessageId = MessageId.I0000,
                    Message = Message.GetMessageById(MessageId.I0000)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<CompanyDto>>
                {
                    Success = false,
                    MessageId = MessageId.E0000,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<bool>> ExistsAsync(Guid id)
        {
            try
            {
                var company = await _companyRepository.GetByIdAsync(id);
                var exists = company != null && !company.IsDeleted;
                
                return new ApiResponse<bool>
                {
                    Data = exists,
                    Success = true,
                    MessageId = MessageId.I0000,
                    Message = Message.GetMessageById(MessageId.I0000)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    MessageId = MessageId.E0000,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
} 