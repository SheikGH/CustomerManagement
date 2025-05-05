using CustomerManagement.Core.Entities;
using CustomerManagement.Common.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerManagement.Application.Interfaces
{
    public interface IAppraisalService
    {
        #region Workflow
        Task<IEnumerable<AppraisalTaskDto>> GetAppraisalByUIdAsync(SearchInParamDto searchInParam);
        Task<AppraisalDetailDto> GetAppraisalDetailsAsync(SearchInParamDto searchInParam);
        Task<AppraisalTaskDto> GetAppraisalWFBySearchAsync(SearchInParamDto searchInParam);
        Task<AppraisalDto> AddAppraisalWFAsync(AppraisalDto dto);
        #endregion Workflow

        #region CRUD
        Task<IEnumerable<AppraisalDto>> GetAllAppraisalsAsync();
        Task<AppraisalDto> GetAppraisalByIdAsync(int id);
        Task AddAppraisalAsync(AppraisalDto dto);
        Task UpdateAppraisalAsync(AppraisalDto dto);
        Task DeleteAppraisalAsync(int id);
        #endregion CRUD
    }
}