using Snapspot.Application.Models.Styles;
using Snapspot.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snapspot.Application.UseCases.Interfaces.Style
{
    public interface IStyleUseCase
    {
        // CRUD Operations
        Task<ApiResponse<StyleDto>> GetByIdAsync(Guid id);
        Task<ApiResponse<IEnumerable<StyleDto>>> GetAllAsync();
        Task<ApiResponse<StyleDto>> CreateAsync(CreateStyleDto createStyleDto);
        Task<ApiResponse<StyleDto>> UpdateAsync(Guid id, UpdateStyleDto updateStyleDto);
        Task<ApiResponse<string>> DeleteAsync(Guid id);

        // Business Operations
        Task<ApiResponse<IEnumerable<StyleDto>>> GetByCategoryAsync(string category);
        Task<ApiResponse<string>> AssignStyleToSpotAsync(Guid styleId, Guid spotId);
        Task<ApiResponse<string>> RemoveStyleFromSpotAsync(Guid styleId, Guid spotId);
    }
}
