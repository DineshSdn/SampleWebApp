using Xunit;
using Moq;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using App.Common.Abstractions.DbDrivers;
using App.Common.Adapters.Data;
using App.Common.Models;
using App.Core.Abstractions;
using App.Core.Models;
using App.Core.Services;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace SampleWebApp.Tests.Core
{
    public class EmployeeServiceTests
    {
        private readonly Mock<IAppDbContext> _mockDbContext;
        private readonly Mock<IObjectAdapterService> _mockObjectAdapterService;
        private readonly Mock<IQueryAdapter> _mockQueryAdapter; // Though not directly used in methods, it's a dependency
        private readonly EmployeeService _employeeService;

        // Helper method to mock DbSet
        private static Mock<DbSet<T>> CreateMockDbSet<T>(List<T> sourceList) where T : class
        {
            var queryable = sourceList.AsQueryable();
            var mockDbSet = new Mock<DbSet<T>>();

            mockDbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            mockDbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockDbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            // mockDbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator()); // Covered by AsAsyncEnumerable
            mockDbSet.As<IEnumerable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());


            mockDbSet.Setup(d => d.AddAsync(It.IsAny<T>(), It.IsAny<CancellationToken>()))
                .Callback<T, CancellationToken>((s, ct) => {
                    if (s is Employee emp && emp.Id == 0) {
                        emp.Id = new Random().Next(1, 100000); // Simulate DB generating an ID
                    }
                    sourceList.Add(s);
                })
                .ReturnsAsync((T entity, CancellationToken ct) => Mock.Of<EntityEntry<T>>(e => e.Entity == entity));

            mockDbSet.Setup(d => d.Remove(It.IsAny<T>()))
                .Callback<T>(s => sourceList.Remove(s))
                .Returns((T entity) => Mock.Of<EntityEntry<T>>(e => e.Entity == entity));

            // For async operations like FirstOrDefaultAsync, CountAsync
            mockDbSet.As<IAsyncEnumerable<T>>()
                .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                .Returns(new TestAsyncEnumerator<T>(queryable.GetEnumerator())); // Corrected: use queryable.GetEnumerator()

            // This is crucial for methods like FirstOrDefaultAsync, ToListAsync, etc.
            mockDbSet.As<IQueryable<T>>().Setup(m => m.Provider)
                .Returns(new TestAsyncQueryProvider<T>(queryable.Provider));


            return mockDbSet;
        }

        // TestAsyncEnumerator, TestAsyncEnumerable, and TestAsyncQueryProvider are needed for EF Core async LINQ operations on mocked DbSets.
        private class TestAsyncQueryProvider<TEntity> : IAsyncQueryProvider
        {
            private readonly IQueryProvider _inner;

            internal TestAsyncQueryProvider(IQueryProvider inner)
            {
                _inner = inner;
            }

            public IQueryable CreateQuery(Expression expression)
            {
                return new TestAsyncEnumerable<TEntity>(expression);
            }

            public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
            {
                return new TestAsyncEnumerable<TElement>(expression);
            }

            public object Execute(Expression expression)
            {
                return _inner.Execute(expression);
            }

            public TResult Execute<TResult>(Expression expression)
            {
                return _inner.Execute<TResult>(expression);
            }

            public IAsyncEnumerable<TResult> ExecuteAsync<TResult>(Expression expression)
            {
                return new TestAsyncEnumerable<TResult>(expression);
            }

            public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
            {
                // This is a simplification. True async execution might require more complex handling
                // or a library that helps bridge sync IQueryProvider to IAsyncQueryProvider.
                // For many common cases (FirstOrDefaultAsync, ToListAsync, CountAsync),
                // the TestAsyncEnumerable and specific DbSet mocks handle it.
                var expectedResultType = typeof(TResult).GetGenericArguments()[0];
                var executionResult = _inner.Execute(expression);

                // This is a hacky way to handle Task<T> results for FirstOrDefaultAsync etc.
                // A more robust solution might involve a custom expression visitor or a library.
                if (executionResult == null)
                {
                    return default(TResult);
                }
                return (TResult)typeof(Task).GetMethod(nameof(Task.FromResult))
                    .MakeGenericMethod(expectedResultType)
                    .Invoke(null, new[] { executionResult });
            }
        }

        private class TestAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
        {
            public TestAsyncEnumerable(IEnumerable<T> enumerable) : base(enumerable) { }
            public TestAsyncEnumerable(Expression expression) : base(expression) { }
            public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
            {
                return new TestAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
            }
            IQueryProvider IQueryable.Provider => new TestAsyncQueryProvider<T>(this);
        }

        public class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
        {
            private readonly IEnumerator<T> _inner;
            public T Current => _inner.Current;
            public TestAsyncEnumerator(IEnumerator<T> inner) => _inner = inner;
            public ValueTask<bool> MoveNextAsync() => new ValueTask<bool>(_inner.MoveNext());
            public ValueTask DisposeAsync()
            {
                _inner.Dispose();
                return new ValueTask();
            }
        }

        public EmployeeServiceTests()
        {
            _mockDbContext = new Mock<IAppDbContext>();
            _mockObjectAdapterService = new Mock<IObjectAdapterService>();
            _mockQueryAdapter = new Mock<IQueryAdapter>();

            _employeeService = new EmployeeService(
                _mockDbContext.Object,
                _mockObjectAdapterService.Object,
                _mockQueryAdapter.Object);
        }

        // --- Placeholder for tests ---

        [Fact]
        public async Task GetEmployeeByIdAsync_WhenEmployeeExists_ReturnsSuccessWithEmployeeDto()
        {
            // Arrange
            var employeeId = 1;
            var employee = new Employee { Id = employeeId, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", IsDeleted = false };
            var employees = new List<Employee> { employee };
            var mockEmployeeDbSet = CreateMockDbSet(employees);

            _mockDbContext.Setup(db => db.Set<Employee>()).Returns(mockEmployeeDbSet.Object);

            var employeeDto = new EmployeeDto { Id = employeeId, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" };
            _mockObjectAdapterService.Setup(s => s.Adapt<EmployeeDto>(It.IsAny<Employee>())).Returns(employeeDto);

            // Act
            var result = await _employeeService.GetEmployeeById(employeeId);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data.Id.Should().Be(employeeId);
            result.Data.FirstName.Should().Be(employee.FirstName);
            _mockDbContext.Verify(db => db.Set<Employee>(), Times.Once);
            _mockObjectAdapterService.Verify(s => s.Adapt<EmployeeDto>(employee), Times.Once);
        }

        [Fact]
        public async Task GetEmployeeByIdAsync_WhenEmployeeDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var employeeId = 99;
            var employees = new List<Employee>(); // Empty list
            var mockEmployeeDbSet = CreateMockDbSet(employees);

            _mockDbContext.Setup(db => db.Set<Employee>()).Returns(mockEmployeeDbSet.Object);

            // Act
            var result = await _employeeService.GetEmployeeById(employeeId);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.Data.Should().BeNull();
            result.StatusCode.Should().Be(HttpStatusCodes.NotFound);
            result.Messages.Should().Contain("Not found");
            _mockDbContext.Verify(db => db.Set<Employee>(), Times.Once);
            _mockObjectAdapterService.Verify(s => s.Adapt<EmployeeDto>(It.IsAny<Employee>()), Times.Never);
        }

        // Tests for AddEmployeeAsync will go here
        [Fact]
        public async Task AddEmployeeAsync_WithValidData_AddsEmployeeAndReturnsSuccessWithEmployeeDto()
        {
            // Arrange
            var employeeDto = new EmployeeDto
            {
                FirstName = "Jane",
                LastName = "Doe",
                Email = "jane.doe@example.com",
                HireDate = DateTime.UtcNow.Date,
                JobTitle = "Developer",
                Salary = 60000
            };
            var employee = new Employee
            {
                // Id is typically 0 before saving for a new entity.
                // The service expects it to be populated after SaveChangesAsync.
                FirstName = employeeDto.FirstName,
                LastName = employeeDto.LastName,
                Email = employeeDto.Email,
                HireDate = employeeDto.HireDate,
                JobTitle = employeeDto.JobTitle,
                Salary = employeeDto.Salary
            };

            var employees = new List<Employee>();
            var mockEmployeeDbSet = CreateMockDbSet(employees);

            _mockDbContext.Setup(db => db.Set<Employee>()).Returns(mockEmployeeDbSet.Object);
            _mockDbContext.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1); // Simulate one record saved

            _mockObjectAdapterService.Setup(s => s.Adapt<Employee>(It.IsAny<EmployeeDto>())).Returns(employee);
            // The service updates the DTO's Id after saving, so no need to mock Adapt<EmployeeDto> here.

            // Act
            var result = await _employeeService.AddEmployee(employeeDto);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data.Id.Should().NotBe(0); // Assuming Id is generated and non-zero
            result.Data.FirstName.Should().Be(employeeDto.FirstName);

            employees.Should().ContainSingle(); // Verify employee was added to the list backing DbSet
            employees.First().FirstName.Should().Be(employeeDto.FirstName);

            _mockDbContext.Verify(db => db.Set<Employee>(), Times.Once);
            mockEmployeeDbSet.Verify(dbSet => dbSet.AddAsync(It.Is<Employee>(e => e.FirstName == employeeDto.FirstName), It.IsAny<CancellationToken>()), Times.Once);
            _mockDbContext.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            _mockObjectAdapterService.Verify(s => s.Adapt<Employee>(employeeDto), Times.Once);
        }

        // Tests for UpdateEmployeeAsync will go here
        [Fact]
        public async Task UpdateEmployeeAsync_WhenEmployeeExists_UpdatesEmployeeAndReturnsSuccess()
        {
            // Arrange
            var employeeId = 1;
            var existingEmployee = new Employee
            {
                Id = employeeId,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                IsDeleted = false,
                HireDate = DateTime.UtcNow.AddYears(-1),
                JobTitle = "Old Title",
                Salary = 50000
            };
            var employees = new List<Employee> { existingEmployee };
            var mockEmployeeDbSet = CreateMockDbSet(employees);

            _mockDbContext.Setup(db => db.Set<Employee>()).Returns(mockEmployeeDbSet.Object);
            _mockDbContext.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var updatedEmployeeDto = new EmployeeDto
            {
                Id = employeeId,
                FirstName = "Johnathan",
                LastName = "Doe Updated",
                Email = "john.doe.updated@example.com",
                HireDate = existingEmployee.HireDate, // Assuming HireDate is not updatable or tested separately
                JobTitle = "New Title",
                Salary = 55000
            };
            // No need to mock _objectAdapterService for Adapt<Employee> as it's not used in Update path
            // No need to mock _objectAdapterService for Adapt<EmployeeDto> as the input DTO is returned

            // Act
            var result = await _employeeService.UpdateEmployee(updatedEmployeeDto);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data.Id.Should().Be(employeeId);
            result.Data.FirstName.Should().Be(updatedEmployeeDto.FirstName);
            result.Data.LastName.Should().Be(updatedEmployeeDto.LastName);
            result.Data.Email.Should().Be(updatedEmployeeDto.Email);
            result.Data.JobTitle.Should().Be(updatedEmployeeDto.JobTitle);
            result.Data.Salary.Should().Be(updatedEmployeeDto.Salary);

            existingEmployee.FirstName.Should().Be(updatedEmployeeDto.FirstName);
            existingEmployee.LastName.Should().Be(updatedEmployeeDto.LastName);
            existingEmployee.Email.Should().Be(updatedEmployeeDto.Email);
            existingEmployee.JobTitle.Should().Be(updatedEmployeeDto.JobTitle);
            existingEmployee.Salary.Should().Be(updatedEmployeeDto.Salary);

            _mockDbContext.Verify(db => db.Set<Employee>(), Times.Once);
            _mockDbContext.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateEmployeeAsync_WhenEmployeeDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var nonExistentEmployeeId = 99;
            var employees = new List<Employee>(); // Empty list
            var mockEmployeeDbSet = CreateMockDbSet(employees);

            _mockDbContext.Setup(db => db.Set<Employee>()).Returns(mockEmployeeDbSet.Object);

            var employeeDtoToUpdate = new EmployeeDto { Id = nonExistentEmployeeId, FirstName = "Ghost" };

            // Act
            var result = await _employeeService.UpdateEmployee(employeeDtoToUpdate);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.Data.Should().BeSameAs(employeeDtoToUpdate); // Service returns the input DTO on failure
            result.StatusCode.Should().Be(HttpStatusCodes.NotFound);
            result.Messages.Should().Contain("Employee not found");

            _mockDbContext.Verify(db => db.Set<Employee>(), Times.Once);
            _mockDbContext.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        // Tests for DeleteEmployeeAsync will go here
        [Fact]
        public async Task DeleteEmployeeAsync_WhenEmployeeExists_RemovesEmployeeAndReturnsOk()
        {
            // Arrange
            var employeeId = 1;
            var employeeToDelete = new Employee { Id = employeeId, FirstName = "Test", IsDeleted = false };
            var employees = new List<Employee> { employeeToDelete };
            var mockEmployeeDbSet = CreateMockDbSet(employees);

            _mockDbContext.Setup(db => db.Set<Employee>()).Returns(mockEmployeeDbSet.Object);
            _mockDbContext.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            // Act
            var result = await _employeeService.DeleteEmployee(employeeId);

            // Assert
            result.Should().NotBeNull();
            // Current service implementation for DeleteEmployee returns:
            // AppResponseDto.Response(false, "Employee deleted successfully", HttpStatusCodes.OK);
            // This means IsSuccess will be false. If this is intentional, the test should assert that.
            // If IsSuccess should be true for a successful deletion, the service method needs adjustment.
            // Testing as per current implementation:
            result.IsSuccess.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCodes.OK);
            result.Messages.Should().Contain("Employee deleted successfully");

            employees.Should().NotContain(employeeToDelete); // Verify employee was removed from the list

            _mockDbContext.Verify(db => db.Set<Employee>(), Times.Once);
            mockEmployeeDbSet.Verify(dbSet => dbSet.Remove(It.Is<Employee>(e => e.Id == employeeId)), Times.Once);
            _mockDbContext.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteEmployeeAsync_WhenEmployeeDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var nonExistentEmployeeId = 99;
            var employees = new List<Employee>();
            var mockEmployeeDbSet = CreateMockDbSet(employees);

            _mockDbContext.Setup(db => db.Set<Employee>()).Returns(mockEmployeeDbSet.Object);

            // Act
            var result = await _employeeService.DeleteEmployee(nonExistentEmployeeId);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse(); // As per AppResponseDto.Response(false, ...)
            result.StatusCode.Should().Be(HttpStatusCodes.NotFound);
            result.Messages.Should().Contain("Not found");

            _mockDbContext.Verify(db => db.Set<Employee>(), Times.Once);
            mockEmployeeDbSet.Verify(dbSet => dbSet.Remove(It.IsAny<Employee>()), Times.Never);
            _mockDbContext.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        // Tests for GetEmployeeListAsync will go here
        [Fact]
        public async Task GetEmployeeListAsync_WhenEmployeesExist_ReturnsPagedResultWithEmployees()
        {
            // Arrange
            var employeesData = new List<Employee>
            {
                new Employee { Id = 1, FirstName = "John", LastName = "Doe", IsDeleted = false },
                new Employee { Id = 2, FirstName = "Jane", LastName = "Smith", IsDeleted = false },
                new Employee { Id = 3, FirstName = "Deleted", LastName = "User", IsDeleted = true } // Should be filtered out
            };
            var mockEmployeeDbSet = CreateMockDbSet(employeesData);
            _mockDbContext.Setup(db => db.Set<Employee>()).Returns(mockEmployeeDbSet.Object);

            var employeeDtos = new List<EmployeeDto>
            {
                new EmployeeDto { Id = 1, FirstName = "John", LastName = "Doe" },
                new EmployeeDto { Id = 2, FirstName = "Jane", LastName = "Smith" }
            };
            _mockObjectAdapterService.Setup(s => s.Adapt<List<EmployeeDto>>(It.Is<List<Employee>>(list => list.Count == 2))).Returns(employeeDtos);

            var pageFilter = new PageFilterDto { PageNumber = 1, PageSize = 10 };

            // Act
            var result = await _employeeService.GetEmployeeList(pageFilter);

            // Assert
            result.Should().NotBeNull();
            result.TotalRecords.Should().Be(2); // Only non-deleted employees
            result.Records.Should().NotBeNull();
            result.Records.Count.Should().Be(2);
            result.Records.Should().BeEquivalentTo(employeeDtos);
            result.PageNumber.Should().Be(pageFilter.PageNumber);
            result.PageSize.Should().Be(pageFilter.PageSize);

            _mockDbContext.Verify(db => db.Set<Employee>(), Times.Exactly(2)); // Once for CountAsync, once for ToList
            _mockObjectAdapterService.Verify(s => s.Adapt<List<EmployeeDto>>(It.Is<List<Employee>>(list => list.All(e => !e.IsDeleted) && list.Count == 2)), Times.Once);
        }

        [Fact]
        public async Task GetEmployeeListAsync_WhenNoEmployeesExist_ReturnsEmptyPagedResult()
        {
            // Arrange
            var employeesData = new List<Employee>(); // No employees
            var mockEmployeeDbSet = CreateMockDbSet(employeesData);
            _mockDbContext.Setup(db => db.Set<Employee>()).Returns(mockEmployeeDbSet.Object);

            _mockObjectAdapterService.Setup(s => s.Adapt<List<EmployeeDto>>(It.IsAny<List<Employee>>())).Returns(new List<EmployeeDto>());

            var pageFilter = new PageFilterDto { PageNumber = 1, PageSize = 10 };

            // Act
            var result = await _employeeService.GetEmployeeList(pageFilter);

            // Assert
            result.Should().NotBeNull();
            result.TotalRecords.Should().Be(0);
            result.Records.Should().NotBeNull().And.BeEmpty();
            result.PageNumber.Should().Be(pageFilter.PageNumber);
            result.PageSize.Should().Be(pageFilter.PageSize);

            _mockDbContext.Verify(db => db.Set<Employee>(), Times.Exactly(2));
            _mockObjectAdapterService.Verify(s => s.Adapt<List<EmployeeDto>>(It.Is<List<Employee>>(list => list.Count == 0)), Times.Once);
        }

        [Fact]
        public async Task GetEmployeeListAsync_WithPagination_ReturnsCorrectPage()
        {
            // Arrange
            var employeesData = new List<Employee>
            {
                new Employee { Id = 1, FirstName = "Emp1", IsDeleted = false },
                new Employee { Id = 2, FirstName = "Emp2", IsDeleted = false },
                new Employee { Id = 3, FirstName = "Emp3", IsDeleted = false },
                new Employee { Id = 4, FirstName = "Emp4", IsDeleted = false },
                new Employee { Id = 5, FirstName = "Emp5", IsDeleted = false },
            };
            var mockEmployeeDbSet = CreateMockDbSet(employeesData);
            _mockDbContext.Setup(db => db.Set<Employee>()).Returns(mockEmployeeDbSet.Object);

            // Simulate pagination by returning only the expected subset for Adapt
            var pagedEmployeeDtos = new List<EmployeeDto>
            {
                new EmployeeDto { Id = 3, FirstName = "Emp3" },
                new EmployeeDto { Id = 4, FirstName = "Emp4" }
            };
            // This setup is crucial: ensure the list passed to Adapt matches what the service method would filter
            _mockObjectAdapterService.Setup(s => s.Adapt<List<EmployeeDto>>(
                It.Is<List<Employee>>(list => list.Count == 2 && list[0].Id == 3 && list[1].Id == 4)))
                .Returns(pagedEmployeeDtos);

            var pageFilter = new PageFilterDto { PageNumber = 2, PageSize = 2 };

            // Act
            var result = await _employeeService.GetEmployeeList(pageFilter);

            // Assert
            result.Should().NotBeNull();
            result.TotalRecords.Should().Be(5);
            result.Records.Should().NotBeNull();
            result.Records.Count.Should().Be(2);
            result.Records.Should().BeEquivalentTo(pagedEmployeeDtos);
            result.PageNumber.Should().Be(pageFilter.PageNumber);
            result.PageSize.Should().Be(pageFilter.PageSize);

            _mockDbContext.Verify(db => db.Set<Employee>(), Times.Exactly(2));
             _mockObjectAdapterService.Verify(s => s.Adapt<List<EmployeeDto>>(
                It.Is<List<Employee>>(list => list.Count == 2 && list.Any(e => e.Id == 3) && list.Any(e => e.Id == 4))),
                Times.Once);
        }
    }
}
