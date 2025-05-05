using AutoMapper;
using CustomerManagement.Common.DTOs;
using CustomerManagement.Application.Interfaces;
using CustomerManagement.Core.Entities;
using CustomerManagement.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerManagement.Application.Services
{
    public class AppraisalService : IAppraisalService
    {
        private readonly IAppraisalRepository _appraisalRepository;
        private readonly IMapper _mapper;

        public AppraisalService(IAppraisalRepository appraisalRepository, IMapper mapper)
        {
            _appraisalRepository = appraisalRepository;
            _mapper = mapper;
        }

        #region Workflow
        public async Task<IEnumerable<AppraisalTaskDto>> GetAppraisalByUIdAsync(SearchInParamDto searchInParam)
        {
            var appraisal = await _appraisalRepository.GetByUIdAsync(searchInParam);
            return appraisal;
        }
        public async Task<AppraisalDetailDto> GetAppraisalDetailsAsync(SearchInParamDto searchInParam)
        {
            var appraisal = await _appraisalRepository.GetDetailsAsync(searchInParam);
            return appraisal;
        }
        public async Task<AppraisalTaskDto> GetAppraisalWFBySearchAsync(SearchInParamDto searchInParam)
        {
            var appraisal = await _appraisalRepository.GetWFBySearchAsync(searchInParam);
            return appraisal; //_mapper.Map<AppraisalTaskDto>(appraisal);
        }
        public async Task<AppraisalDto> AddAppraisalWFAsync(AppraisalDto dto)
        {
            var appraisal = _mapper.Map<Appraisal>(dto);
            return await _appraisalRepository.AddWFAsync(appraisal, dto.DBAction);
        }
        #endregion Workflow

        #region CRUD
        public async Task<IEnumerable<AppraisalDto>> GetAllAppraisalsAsync()
        {
            var appraisals = await _appraisalRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<AppraisalDto>>(appraisals);
        }

        public async Task<AppraisalDto> GetAppraisalByIdAsync(int id)
        {
            var appraisal = await _appraisalRepository.GetByIdAsync(id);
            return _mapper.Map<AppraisalDto>(appraisal);
        }

        public async Task AddAppraisalAsync(AppraisalDto dto)
        {
            var appraisal = _mapper.Map<Appraisal>(dto);
            await _appraisalRepository.AddAsync(appraisal);
        }

        public async Task UpdateAppraisalAsync(AppraisalDto dto)
        {
            var appraisal = _mapper.Map<Appraisal>(dto);
            await _appraisalRepository.UpdateAsync(appraisal);
        }

        public async Task DeleteAppraisalAsync(int id)
        {
            await _appraisalRepository.DeleteAsync(id);
        }
        #endregion CRUD
    }
}