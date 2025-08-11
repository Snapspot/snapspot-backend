using Snapspot.Application.Models.Styles;
using Snapspot.Application.Repositories;
using Snapspot.Application.UseCases.Interfaces.Style;
using Snapspot.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StyleEntity = Snapspot.Domain.Entities.Style;

namespace Snapspot.Application.UseCases.Implementations.Style
{
    public class StyleUseCase : IStyleUseCase
    {
        private readonly IStyleRepository _styleRepository;
        private readonly ISpotRepository _spotRepository;

        public StyleUseCase(IStyleRepository styleRepository, ISpotRepository spotRepository)
        {
            _styleRepository = styleRepository;
            _spotRepository = spotRepository;
        }

        public async Task<ApiResponse<StyleDto>> CreateAsync(CreateStyleDto createStyleDto)
        {
            try
            {
                // Validate input
                if (string.IsNullOrWhiteSpace(createStyleDto.Category))
                {
                    return new ApiResponse<StyleDto>
                    {
                        Success = false,
                        MessageId = MessageId.E0003,
                        Message = "Category không được để trống"
                    };
                }

                // Check if style with same category already exists
                var existingStyles = await _styleRepository.GetByCategoryAsync(createStyleDto.Category);
                if (existingStyles.Any())
                {
                    return new ApiResponse<StyleDto>
                    {
                        Success = false,
                        MessageId = MessageId.E0000,
                        Message = $"Phong cách '{createStyleDto.Category}' đã tồn tại"
                    };
                }

                // Create new style
                var newStyle = new StyleEntity
                {
                    Id = Guid.NewGuid(),
                    Category = createStyleDto.Category.Trim(),
                    Description = createStyleDto.Description?.Trim(),
                    Image = createStyleDto.Image?.Trim(),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false
                };

                // Add to repository
                await _styleRepository.AddAsync(newStyle);
                await _styleRepository.SaveChangesAsync();

                // Convert to DTO and return
                var styleDto = new StyleDto
                {
                    Id = newStyle.Id,
                    Category = newStyle.Category,
                    Description = newStyle.Description,
                    Image = newStyle.Image,
                    CreatedAt = newStyle.CreatedAt,
                    UpdatedAt = newStyle.UpdatedAt,
                    IsDeleted = newStyle.IsDeleted
                };

                return new ApiResponse<StyleDto>
                {
                    Data = styleDto,
                    Success = true,
                    MessageId = MessageId.I0001,
                    Message = Message.GetMessageById(MessageId.I0001)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<StyleDto>
                {
                    Success = false,
                    MessageId = MessageId.E0002,
                    Message = $"An unexpected error occurred: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<StyleDto>> GetByIdAsync(Guid id)
        {
            try
            {
                var style = await _styleRepository.GetByIdAsync(id);
                if (style == null)
                {
                    return new ApiResponse<StyleDto>
                    {
                        Success = false,
                        MessageId = MessageId.E0001,
                        Message = "Style not found"
                    };
                }

                var styleDto = new StyleDto
                {
                    Id = style.Id,
                    Category = style.Category,
                    Description = style.Description,
                    Image = style.Image,
                    CreatedAt = style.CreatedAt,
                    UpdatedAt = style.UpdatedAt,
                    IsDeleted = style.IsDeleted
                };

                return new ApiResponse<StyleDto>
                {
                    Data = styleDto,
                    Success = true,
                    MessageId = MessageId.I0000,
                    Message = Message.GetMessageById(MessageId.I0000)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<StyleDto>
                {
                    Success = false,
                    MessageId = MessageId.E0002,
                    Message = $"An unexpected error occurred: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<IEnumerable<StyleDto>>> GetAllAsync()
        {
            try
            {
                var styles = await _styleRepository.GetAllAsync();

                var styleDtos = styles.Select(s => new StyleDto
                {
                    Id = s.Id,
                    Category = s.Category,
                    Description = s.Description,
                    Image = s.Image,
                    CreatedAt = s.CreatedAt,
                    UpdatedAt = s.UpdatedAt,
                    IsDeleted = s.IsDeleted
                });

                return new ApiResponse<IEnumerable<StyleDto>>
                {
                    Data = styleDtos,
                    Success = true,
                    MessageId = MessageId.I0000,
                    Message = Message.GetMessageById(MessageId.I0000)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<StyleDto>>
                {
                    Success = false,
                    MessageId = MessageId.E0002,
                    Message = $"An unexpected error occurred: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<IEnumerable<StyleDto>>> GetByCategoryAsync(string category)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(category))
                {
                    return new ApiResponse<IEnumerable<StyleDto>>
                    {
                        Success = false,
                        MessageId = MessageId.E0003,
                        Message = "Category không được để trống"
                    };
                }

                var styles = await _styleRepository.GetByCategoryAsync(category);

                var styleDtos = styles.Select(s => new StyleDto
                {
                    Id = s.Id,
                    Category = s.Category,
                    Description = s.Description,
                    Image = s.Image,
                    CreatedAt = s.CreatedAt,
                    UpdatedAt = s.UpdatedAt,
                    IsDeleted = s.IsDeleted
                });

                return new ApiResponse<IEnumerable<StyleDto>>
                {
                    Data = styleDtos,
                    Success = true,
                    MessageId = MessageId.I0000,
                    Message = Message.GetMessageById(MessageId.I0000)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<StyleDto>>
                {
                    Success = false,
                    MessageId = MessageId.E0002,
                    Message = $"An unexpected error occurred: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<StyleDto>> UpdateAsync(Guid id, UpdateStyleDto updateStyleDto)
        {
            try
            {
                var style = await _styleRepository.GetByIdAsync(id);
                if (style == null)
                {
                    return new ApiResponse<StyleDto>
                    {
                        Success = false,
                        MessageId = MessageId.E0001,
                        Message = "Style not found"
                    };
                }

                // Validate input
                if (string.IsNullOrWhiteSpace(updateStyleDto.Category))
                {
                    return new ApiResponse<StyleDto>
                    {
                        Success = false,
                        MessageId = MessageId.E0003,
                        Message = "Category không được để trống"
                    };
                }

                // Check if new category already exists (excluding current style)
                var existingStyles = await _styleRepository.GetByCategoryAsync(updateStyleDto.Category);
                var duplicateStyle = existingStyles.FirstOrDefault(s => s.Id != id);
                if (duplicateStyle != null)
                {
                    return new ApiResponse<StyleDto>
                    {
                        Success = false,
                        MessageId = MessageId.E0000,
                        Message = $"Phong cách '{updateStyleDto.Category}' đã tồn tại"
                    };
                }

                // Update style
                style.Category = updateStyleDto.Category.Trim();
                style.Description = updateStyleDto.Description?.Trim();
                style.Image = updateStyleDto.Image?.Trim();
                style.UpdatedAt = DateTime.UtcNow;

                await _styleRepository.UpdateAsync(style);
                await _styleRepository.SaveChangesAsync();

                var styleDto = new StyleDto
                {
                    Id = style.Id,
                    Category = style.Category,
                    Description = style.Description,
                    Image = style.Image,
                    CreatedAt = style.CreatedAt,
                    UpdatedAt = style.UpdatedAt,
                    IsDeleted = style.IsDeleted
                };

                return new ApiResponse<StyleDto>
                {
                    Data = styleDto,
                    Success = true,
                    MessageId = MessageId.I0002,
                    Message = Message.GetMessageById(MessageId.I0002)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<StyleDto>
                {
                    Success = false,
                    MessageId = MessageId.E0002,
                    Message = $"An unexpected error occurred: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<string>> DeleteAsync(Guid id)
        {
            try
            {
                var style = await _styleRepository.GetByIdAsync(id);
                if (style == null)
                {
                    return new ApiResponse<string>
                    {
                        Success = false,
                        MessageId = MessageId.E0001,
                        Message = "Style not found"
                    };
                }

                await _styleRepository.DeleteAsync(style);
                await _styleRepository.SaveChangesAsync();

                return new ApiResponse<string>
                {
                    Data = "Style deleted successfully",
                    Success = true,
                    MessageId = MessageId.I0000,
                    Message = Message.GetMessageById(MessageId.I0000)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    MessageId = MessageId.E0002,
                    Message = $"An unexpected error occurred: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<string>> AssignStyleToSpotAsync(Guid styleId, Guid spotId)
        {
            try
            {
                // Check if style exists
                var style = await _styleRepository.GetByIdAsync(styleId);
                if (style == null)
                {
                    return new ApiResponse<string>
                    {
                        Success = false,
                        MessageId = MessageId.E0001,
                        Message = "Style not found"
                    };
                }

                // Check if spot exists
                var spot = await _spotRepository.GetByIdAsync(spotId);
                if (spot == null)
                {
                    return new ApiResponse<string>
                    {
                        Success = false,
                        MessageId = MessageId.E0001,
                        Message = "Spot not found"
                    };
                }

                // Assign style to spot
                var result = await _styleRepository.AssignStyleToSpotAsync(styleId, spotId);
                if (!result)
                {
                    return new ApiResponse<string>
                    {
                        Success = false,
                        MessageId = MessageId.E0000,
                        Message = "Style đã được gán cho Spot này"
                    };
                }

                await _styleRepository.SaveChangesAsync();

                return new ApiResponse<string>
                {
                    Data = "Style assigned to spot successfully",
                    Success = true,
                    MessageId = MessageId.I0000,
                    Message = Message.GetMessageById(MessageId.I0000)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    MessageId = MessageId.E0002,
                    Message = $"An unexpected error occurred: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<string>> RemoveStyleFromSpotAsync(Guid styleId, Guid spotId)
        {
            try
            {
                // Check if style exists
                var style = await _styleRepository.GetByIdAsync(styleId);
                if (style == null)
                {
                    return new ApiResponse<string>
                    {
                        Success = false,
                        MessageId = MessageId.E0001,
                        Message = "Style not found"
                    };
                }

                // Check if spot exists
                var spot = await _spotRepository.GetByIdAsync(spotId);
                if (spot == null)
                {
                    return new ApiResponse<string>
                    {
                        Success = false,
                        MessageId = MessageId.E0001,
                        Message = "Spot not found"
                    };
                }

                // Remove style from spot
                var result = await _styleRepository.RemoveStyleFromSpotAsync(styleId, spotId);
                if (!result)
                {
                    return new ApiResponse<string>
                    {
                        Success = false,
                        MessageId = MessageId.E0000,
                        Message = "Style chưa được gán cho Spot này"
                    };
                }

                await _styleRepository.SaveChangesAsync();

                return new ApiResponse<string>
                {
                    Data = "Style removed from spot successfully",
                    Success = true,
                    MessageId = MessageId.I0000,
                    Message = Message.GetMessageById(MessageId.I0000)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    MessageId = MessageId.E0002,
                    Message = $"An unexpected error occurred: {ex.Message}"
                };
            }
        }
    }
}