using CustomerManagement.Core.Entities;
using CustomerManagement.Common.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerManagement.Application.Interfaces {
  public interface ICustomerService
  {
    Task<IEnumerable<CustomerDto>> GetAllCustomersAsync();
    Task<CustomerDto> GetCustomerByIdAsync(int id);
    Task AddCustomerAsync(CustomerDto customer);
    Task UpdateCustomerAsync(CustomerDto customer);
    Task DeleteCustomerAsync(int id);
  }
}
