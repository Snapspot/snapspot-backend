using Snapspot.Application.Models.SellerPackages;
using Snapspot.Application.Repositories;
using Snapspot.Application.UseCases.Interfaces.SellerPackage;
using Snapspot.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snapspot.Application.UseCases.Implementations.SellerPackage
{
    public class SellerPackageUseCase : ISellerPackageUseCase
    {
        private readonly ISellerPackageRepository _sellerPackageRepository;

        public SellerPackageUseCase(ISellerPackageRepository sellerPackageRepository)
        {
            _sellerPackageRepository = sellerPackageRepository;
        }

        public async Task<ApiResponse<SellerPackageDto>> GetByIdAsync(Guid id)
        {
            try
            {
                var sellerPackage = await _sellerPackageRepository.GetByIdAsync(id);
                if (sellerPackage == null)
                {
                    return new ApiResponse<SellerPackageDto>
                    {
                        Success = false,
                        MessageId = MessageId.E0000,
                        Message = "SellerPackage not found"
                    };
                }

                var sellerPackageDto = new SellerPackageDto
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
                    Companies = sellerPackage.Companies?.Select(c => new Models.Companies.CompanyDto
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
                    }).ToList() ?? new List<Models.Companies.CompanyDto>()
                };

                return new ApiResponse<SellerPackageDto>
                {
                    Data = sellerPackageDto,
                    Success = true,
                    MessageId = MessageId.I0000,
                    Message = Message.GetMessageById(MessageId.I0000)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<SellerPackageDto>
                {
                    Success = false,
                    MessageId = MessageId.E0000,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<IEnumerable<SellerPackageDto>>> GetAllAsync()
        {
            try
            {
                var sellerPackages = await _sellerPackageRepository.GetAllAsync();
                
                var sellerPackageDtos = sellerPackages.Select(sp => new SellerPackageDto
                {
                    Id = sp.Id,
                    Name = sp.Name,
                    Description = sp.Description,
                    MaxAgency = sp.MaxAgency,
                    Price = sp.Price,
                    SellingCount = sp.SellingCount,
                    IsDeleted = sp.IsDeleted,
                    CreatedAt = sp.CreatedAt,
                    UpdatedAt = sp.UpdatedAt,
                    Companies = sp.Companies?.Select(c => new Models.Companies.CompanyDto
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
                    }).ToList() ?? new List<Models.Companies.CompanyDto>()
                }).ToList();

                return new ApiResponse<IEnumerable<SellerPackageDto>>
                {
                    Data = sellerPackageDtos,
                    Success = true,
                    MessageId = MessageId.I0000,
                    Message = Message.GetMessageById(MessageId.I0000)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<SellerPackageDto>>
                {
                    Success = false,
                    MessageId = MessageId.E0000,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<SellerPackageDto>> CreateAsync(CreateSellerPackageDto createSellerPackageDto)
        {
            try
            {
                // Business logic: Validate price is positive
                if (createSellerPackageDto.Price <= 0)
                {
                    return new ApiResponse<SellerPackageDto>
                    {
                        Success = false,
                        MessageId = MessageId.E0000,
                        Message = "Price must be greater than zero"
                    };
                }

                // Business logic: Validate max agency is positive
                if (createSellerPackageDto.MaxAgency <= 0)
                {
                    return new ApiResponse<SellerPackageDto>
                    {
                        Success = false,
                        MessageId = MessageId.E0000,
                        Message = "Max agency must be greater than zero"
                    };
                }

                var sellerPackage = new Domain.Entities.SellerPackage
                {
                    Id = Guid.NewGuid(),
                    Name = createSellerPackageDto.Name,
                    Description = createSellerPackageDto.Description,
                    MaxAgency = createSellerPackageDto.MaxAgency,
                    Price = createSellerPackageDto.Price,
                    SellingCount = 0,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false
                };

                await _sellerPackageRepository.AddAsync(sellerPackage);
                await _sellerPackageRepository.SaveChangesAsync();

                var sellerPackageDto = new SellerPackageDto
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
                    Companies = new List<Models.Companies.CompanyDto>()
                };

                return new ApiResponse<SellerPackageDto>
                {
                    Data = sellerPackageDto,
                    Success = true,
                    MessageId = MessageId.I0000,
                    Message = Message.GetMessageById(MessageId.I0000)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<SellerPackageDto>
                {
                    Success = false,
                    MessageId = MessageId.E0000,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<SellerPackageDto>> UpdateAsync(Guid id, UpdateSellerPackageDto updateSellerPackageDto)
        {
            try
            {
                var sellerPackage = await _sellerPackageRepository.GetByIdAsync(id);
                if (sellerPackage == null)
                {
                    return new ApiResponse<SellerPackageDto>
                    {
                        Success = false,
                        MessageId = MessageId.E0000,
                        Message = "SellerPackage not found"
                    };
                }

                // Business logic: Validate price is positive
                if (updateSellerPackageDto.Price <= 0)
                {
                    return new ApiResponse<SellerPackageDto>
                    {
                        Success = false,
                        MessageId = MessageId.E0000,
                        Message = "Price must be greater than zero"
                    };
                }

                // Business logic: Validate max agency is positive
                if (updateSellerPackageDto.MaxAgency <= 0)
                {
                    return new ApiResponse<SellerPackageDto>
                    {
                        Success = false,
                        MessageId = MessageId.E0000,
                        Message = "Max agency must be greater than zero"
                    };
                }

                // Update fields
                if (!string.IsNullOrEmpty(updateSellerPackageDto.Name))
                    sellerPackage.Name = updateSellerPackageDto.Name;
                
                if (!string.IsNullOrEmpty(updateSellerPackageDto.Description))
                    sellerPackage.Description = updateSellerPackageDto.Description;
                
                if (updateSellerPackageDto.MaxAgency > 0)
                    sellerPackage.MaxAgency = updateSellerPackageDto.MaxAgency;
                
                if (updateSellerPackageDto.Price > 0)
                    sellerPackage.Price = updateSellerPackageDto.Price;

                sellerPackage.UpdatedAt = DateTime.UtcNow;

                await _sellerPackageRepository.UpdateAsync(sellerPackage);
                await _sellerPackageRepository.SaveChangesAsync();

                var sellerPackageDto = new SellerPackageDto
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
                    Companies = sellerPackage.Companies?.Select(c => new Models.Companies.CompanyDto
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
                    }).ToList() ?? new List<Models.Companies.CompanyDto>()
                };

                return new ApiResponse<SellerPackageDto>
                {
                    Data = sellerPackageDto,
                    Success = true,
                    MessageId = MessageId.I0000,
                    Message = Message.GetMessageById(MessageId.I0000)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<SellerPackageDto>
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
                var sellerPackage = await _sellerPackageRepository.GetByIdAsync(id);
                if (sellerPackage == null)
                {
                    return new ApiResponse<string>
                    {
                        Success = false,
                        MessageId = MessageId.E0000,
                        Message = "SellerPackage not found"
                    };
                }

                await _sellerPackageRepository.DeleteAsync(sellerPackage);
                await _sellerPackageRepository.SaveChangesAsync();

                return new ApiResponse<string>
                {
                    Success = true,
                    MessageId = MessageId.I0000,
                    Message = "SellerPackage deleted successfully"
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

        public async Task<ApiResponse<bool>> ExistsAsync(Guid id)
        {
            try
            {
                var sellerPackage = await _sellerPackageRepository.GetByIdAsync(id);
                var exists = sellerPackage != null && !sellerPackage.IsDeleted;
                
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
    }
} 