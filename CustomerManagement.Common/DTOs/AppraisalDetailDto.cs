using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManagement.Common.DTOs
{
    public class AppraisalDetailDto
    {
        public AppraisalDto AppraisalDto { get; set; }
        public AppraisalTaskDto AppraisalTaskDto { get; set; }
        public List<AppraisalTaskHistoryDto> AppraisalTaskHistoryDtos { get; set; }

        public AppraisalDetailDto()
        {
            AppraisalDto = new AppraisalDto();
            AppraisalTaskDto = new AppraisalTaskDto();
            AppraisalTaskHistoryDtos = new List<AppraisalTaskHistoryDto>();
        }
    }
}
