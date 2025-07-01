using System.Threading;
using System.Threading.Tasks;
using App.Common.Models;
using App.Core.Models;

namespace App.Core.Abstractions
{
    public interface IEmployeeService
    {
        Task<AppResponseDto<EmployeeDto>> AddEmployee(EmployeeDto model, CancellationToken cancellationToken = default);
        Task<AppResponseDto<EmployeeDto>> UpdateEmployee(EmployeeDto model, CancellationToken cancellationToken = default);
        Task<PagedResultDto<EmployeeDto>> GetEmployeeList(PageFilterDto model, CancellationToken cancellationToken = default);
        Task<AppResponseDto<EmployeeDto>> GetEmployeeById(int id, CancellationToken cancellationToken = default);
        Task<AppResponseDto> DeleteEmployee(int id, CancellationToken cancellationToken = default);
    }
}
