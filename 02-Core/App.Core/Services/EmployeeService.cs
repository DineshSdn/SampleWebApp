using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using App.Common.Abstractions.DbDrivers;
using App.Common.Adapters.Data;
using App.Common.Attributes;
using App.Common.Constants;
using App.Common.Models;
using App.Core.Abstractions;
using App.Core.Models;
using Domain.Entities;

namespace App.Core.Services
{
    [TransientService]
    public class EmployeeService : IEmployeeService
    {
        private readonly IAppDbContext _dbContext;
        private readonly IObjectAdapterService _objectAdapterService;
        private readonly IQueryAdapter _queryAdapter;

        public EmployeeService(IAppDbContext dbContext, IObjectAdapterService objectAdapterService, IQueryAdapter queryAdapter)
        {
            _dbContext = dbContext;
            _objectAdapterService = objectAdapterService;
            _queryAdapter = queryAdapter;
        }

        public async Task<AppResponseDto<EmployeeDto>> AddEmployee(EmployeeDto model, CancellationToken cancellationToken = default)
        {
            var employee = _objectAdapterService.Adapt<Employee>(model);

            await _dbContext.Set<Employee>().AddAsync(employee, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            model.Id = employee.Id;

            return AppResponseDto.Success(model);
        }

        public async Task<AppResponseDto<EmployeeDto>> UpdateEmployee(EmployeeDto model, CancellationToken cancellationToken = default)
        {
            var employee = await _dbContext.Set<Employee>()
                .FirstOrDefaultAsync(x => x.Id == model.Id && !x.IsDeleted, cancellationToken);

            if (employee == null)
                return AppResponseDto.Fail(model, "Employee not found", HttpStatusCodes.NotFound);

            employee.FirstName = model.FirstName;
            employee.LastName = model.LastName;
            employee.Email = model.Email;
            employee.HireDate = model.HireDate;
            employee.JobTitle = model.JobTitle;
            employee.Salary = model.Salary;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return AppResponseDto.Success(model);
        }

        public async Task<AppResponseDto<EmployeeDto>> GetEmployeeById(int id, CancellationToken cancellationToken = default)
        {
            var employee = await _dbContext.Set<Employee>()
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, cancellationToken);

            if (employee == null)
                return AppResponseDto.Fail((EmployeeDto)null, "Not found", HttpStatusCodes.NotFound);

            return AppResponseDto.Success(_objectAdapterService.Adapt<EmployeeDto>(employee));
        }

        public async Task<PagedResultDto<EmployeeDto>> GetEmployeeList(PageFilterDto model, CancellationToken cancellationToken = default)
        {
            var totalRecords = await _dbContext.Set<Employee>()
                .CountAsync(x => !x.IsDeleted, cancellationToken);

            var records = _dbContext.Set<Employee>()
                .Where(x => !x.IsDeleted)
                .ToList();

            var filtered = records
                .Skip((model.PageNumber - 1) * model.PageSize)
                .Take(model.PageSize)
                .ToList();

            return new PagedResultDto<EmployeeDto>
            {
                Records = _objectAdapterService.Adapt<List<EmployeeDto>>(filtered),
                PageNumber = model.PageNumber,
                PageSize = model.PageSize,
                TotalRecords = totalRecords
            };
        }

        public async Task<AppResponseDto> DeleteEmployee(int id, CancellationToken cancellationToken = default)
        {
            var employee = await _dbContext.Set<Employee>()
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, cancellationToken);

            if (employee == null)
                return AppResponseDto.Response(false, "Not found", HttpStatusCodes.NotFound);

            _dbContext.Set<Employee>().Remove(employee);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return AppResponseDto.Response(false, "Employee deleted successfully", HttpStatusCodes.OK);
        }
    }
}
