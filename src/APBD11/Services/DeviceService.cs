 using APBD11.DTOs;
 using APBD11.Interfaces;
 using APBD11.Models;
 
 using APBD11.Data;
 using Microsoft.AspNetCore.Mvc;
 using Microsoft.EntityFrameworkCore;

 public class DeviceService : IDeviceService
 {
     private readonly ApplicationDbContext _context;

     public DeviceService(ApplicationDbContext context)
     {
         _context = context;
     }

   

     public async Task<IEnumerable<Role>> GetRolesAsync()
     {
         return await _context.Roles
             .AsNoTracking()
             .ToListAsync();
    }

     public async Task<IEnumerable<Position>> GetPositionsAsync()
     {
         return await _context.Positions
             .AsNoTracking()
             .ToListAsync();
     }

     public async Task<IEnumerable<DeviceDto>> GetAllAsync()
        {
            
            var entities = await _context.Devices
                                         .AsNoTracking()
                                         .ToListAsync();

            return entities.Select(d => new DeviceDto
            {
                Id = d.Id,
                Name = d.Name,
                AdditionalProperties = d.AdditionalProperties,
                IsEnabled = d.IsEnabled,
                DeviceTypeId = d.DeviceTypeId
            });
        }

        public async Task<DeviceDto?> GetByIdAsync(Guid id)
        {
            var entity = await _context.Devices
                                       .AsNoTracking()
                                       .FirstOrDefaultAsync(d => d.Id == id);
            if (entity == null)
                return null;

            return new DeviceDto
            {
                Id = entity.Id,
                Name = entity.Name,
                AdditionalProperties = entity.AdditionalProperties,
                IsEnabled = entity.IsEnabled,
                DeviceTypeId = entity.DeviceTypeId
            };
        }

        public async Task<DeviceDto> CreateAsync(DeviceDto dto)
        {
            var deviceTypeExists = await _context.DeviceTypes
                                                 .AnyAsync(dt => dt.Id == dto.DeviceTypeId);
            if (!deviceTypeExists)
                throw new ArgumentException($"DeviceType with Id '{dto.DeviceTypeId}' not found.");

            var entity = new Device
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                AdditionalProperties = dto.AdditionalProperties,
                IsEnabled = dto.IsEnabled,
                DeviceTypeId = dto.DeviceTypeId
            };

            _context.Devices.Add(entity);
            await _context.SaveChangesAsync();

            return new DeviceDto
            {
                Id = entity.Id,
                Name = entity.Name,
                AdditionalProperties = entity.AdditionalProperties,
                IsEnabled = entity.IsEnabled,
                DeviceTypeId = entity.DeviceTypeId
            };
        }

        public async Task UpdateAsync(Guid id, DeviceDto dto)
        {
            var entity = await _context.Devices.FirstOrDefaultAsync(d => d.Id == id);
            if (entity == null)
                throw new KeyNotFoundException($"Device with Id '{id}' not found.");

            if (dto.DeviceTypeId != entity.DeviceTypeId)
            {
                var typeExists = await _context.DeviceTypes
                                               .AnyAsync(dt => dt.Id == dto.DeviceTypeId);
                if (!typeExists)
                    throw new ArgumentException($"DeviceType with Id '{dto.DeviceTypeId}' not found.");
            }

            entity.Name = dto.Name;
            entity.AdditionalProperties = dto.AdditionalProperties;
            entity.IsEnabled = dto.IsEnabled;
            entity.DeviceTypeId = dto.DeviceTypeId;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _context.Devices.FirstOrDefaultAsync(d => d.Id == id);
            if (entity == null)
                throw new KeyNotFoundException($"Device with Id '{id}' not found.");

            _context.Devices.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }