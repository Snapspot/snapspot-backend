using Snapspot.Shared.Common;

namespace Snapspot.Application.Models.Agencies
{
    public class GetAgenciesWithPagedFeedbacksRequest : PagingRequest
    {
        public int FeedbackPageSize { get; set; } = 5; // Mặc định 5 feedbacks mỗi trang
    }
} 