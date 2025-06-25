using System.ComponentModel.DataAnnotations;

namespace Snapspot.Application.Models.Agencies
{
    public class ApproveFeedbackDto
    {
        [Required(ErrorMessage = "Trạng thái phê duyệt là bắt buộc")]
        public bool IsApproved { get; set; }
    }
} 