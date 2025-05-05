using CustomerManagement.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using CustomerManagement.Common.DTOs;

namespace CustomerManagement.Core.Interfaces
{
    public interface IAppraisalRepository
    {
        #region Workflow
        Task<IEnumerable<AppraisalTaskDto>> GetByUIdAsync(SearchInParamDto searchInParam);
        Task<AppraisalDetailDto> GetDetailsAsync(SearchInParamDto searchInParam);
        Task<AppraisalTaskDto> GetWFBySearchAsync(SearchInParamDto searchInParam);
        Task<AppraisalDto> AddWFAsync(Appraisal dto, string dbAction);
        #endregion Workflow

        #region CRUD
        Task<IEnumerable<Appraisal>> GetAllAsync();
        Task<Appraisal> GetByIdAsync(int id);
        Task AddAsync(Appraisal appraisal);
        Task UpdateAsync(Appraisal appraisal);
        Task DeleteAsync(int id);
        #endregion CRUD
    }
}