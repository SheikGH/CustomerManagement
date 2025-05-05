using CustomerManagement.Common.DTOs;

namespace CustomerManagement.Web.Models
{
    public class DetailViewModel
    {
        public DetailViewModel()
        {
            AppraisalDto = new AppraisalDto();
            AppraisalTaskDto = new AppraisalTaskDto();
            AppraisalTaskHistoryDtos = new List<AppraisalTaskHistoryDto>();
        }
        public AppraisalDto? AppraisalDto { get; set; }
        public AppraisalTaskDto? AppraisalTaskDto { get; set; }
        public IList<AppraisalTaskHistoryDto>? AppraisalTaskHistoryDtos { get; set; }
    }
}
