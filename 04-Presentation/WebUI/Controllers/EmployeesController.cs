using System.Threading;
using System.Threading.Tasks;
using App.Common.Constants;
using App.Common.Models;
using App.Core.Abstractions;
using App.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        private readonly ILogger<EmployeesController> _logger;

        public EmployeesController(ILogger<EmployeesController> logger, IEmployeeService employeeService)
        {
            _logger = logger;
            _employeeService = employeeService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> AddEmployee(EmployeeDto model, CancellationToken cancellationToken)
        {
            var result = await _employeeService.AddEmployee(model, cancellationToken);

            return Ok(result);
        }

        [HttpPost("[action]/{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, EmployeeDto model, CancellationToken cancellationToken)
        {
            if (id != model.Id)
                return Ok(AppResponseDto.Fail(model, "Bad request", HttpStatusCodes.BadRequest));

            var result = await _employeeService.UpdateEmployee(model, cancellationToken);

            return Ok(result);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> GetEmployeeList(PageFilterDto model, CancellationToken cancellationToken)
        {
            var result = await _employeeService.GetEmployeeList(model, cancellationToken);

            return Ok(result);
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetEmployeeById(int id, CancellationToken cancellationToken)
        {
            var result = await _employeeService.GetEmployeeById(id, cancellationToken);

            return Ok(result);
        }

        [HttpPost("[action]/{id}")]
        public async Task<IActionResult> DeleteEmployee(int id, CancellationToken cancellationToken)
        {
            var result = await _employeeService.DeleteEmployee(id, cancellationToken);

            return Ok(result);
        }
    }
}
