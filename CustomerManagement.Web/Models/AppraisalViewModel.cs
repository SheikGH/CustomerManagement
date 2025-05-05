using CustomerManagement.Common.DTOs;

namespace CustomerManagement.Web.Models
{
    public class AppraisalViewModel
    {
        public IList<AppraisalDto>? AppraisalDtos { get; set; }
        public IList<AppraisalTaskDto>? AppaisalTaskDtos { get; set; }
        public IList<AppraisalTaskHistoryDto>? AppraisalTaskHistoryDtos { get; set; }
    }
}
