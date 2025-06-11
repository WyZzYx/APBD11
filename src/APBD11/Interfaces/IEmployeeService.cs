using APBD11.Models;

namespace APBD11.Interfaces;

public interface IEmployeeService
{
    Task<IEnumerable<Employee>> GetAllAsync();
    Task<Employee?>           GetByIdAsync(Guid id);
}