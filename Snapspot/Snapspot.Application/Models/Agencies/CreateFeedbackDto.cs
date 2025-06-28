using System;
using System.ComponentModel.DataAnnotations;

namespace Snapspot.Application.Models.Agencies
{
    public class CreateFeedbackDto
    {
        [Required(ErrorMessage = "Nội dung feedback là bắt buộc")]
        [StringLength(1000, ErrorMessage = "Nội dung không được vượt quá 1000 ký tự")]
        public string Content { get; set; }

        [Required(ErrorMessage = "Điểm đánh giá là bắt buộc")]
        [Range(1, 5, ErrorMessage = "Điểm đánh giá phải từ 1 đến 5")]
        public int Rating { get; set; }

        [Required(ErrorMessage = "ID đại lý là bắt buộc")]
        public Guid AgencyId { get; set; }

        [Required(ErrorMessage = "UserId là bắt buộc")]
        public Guid UserId { get; set; }
    }
} 