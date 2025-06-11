using APBD11.Data;
using APBD11.Interfaces;
using APBD11.Models;
using Microsoft.EntityFrameworkCore;

namespace APBD11.Services;

public class EmployeeService : IEmployeeService
{
    private readonly ApplicationDbContext _ctx;
    public EmployeeService(ApplicationDbContext ctx) => _ctx = ctx;

    public async Task<IEnumerable<Employee>> GetAllAsync()
        => await _ctx.Employee.AsNoTracking().ToListAsync();

    public async Task<Employee?> GetByIdAsync(Guid id)
        => await _ctx.Employee
            .Include(e => e.Position)
            .FirstOrDefaultAsync(e => e.Id.Equals(id));
}