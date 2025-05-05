using CustomerManagement.Core.Entities;
using CustomerManagement.Common.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerManagement.Application.Interfaces 
{
    public interface IGenericService<TDto>
    {
        Task<IEnumerable<TDto>> GetAllAsync();
        Task<TDto?> GetByIdAsync(int id);
        Task AddAsync(TDto dto);
        Task UpdateAsync(TDto dto);
        Task DeleteAsync(int id);
    }
}