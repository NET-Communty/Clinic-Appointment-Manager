using ClinicAppointmentManager.Core.Entities;
using ClinicAppointmentManager.Core.Interfaces;
using ClinicAppointmentManager.Infrastructure.Data;
using ClinicAppointmentManager.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;

public class ClinicRepository : Repository<Clinic>, IClinicRepository
{
    private readonly ClinicDbContext _context;

    public ClinicRepository(ClinicDbContext context) : base(context)
    {
        _context = context;
    }


    public async Task<IEnumerable<Clinic>> GetAllAsync()
        => await _context.Clinics.ToListAsync();

    public async Task<Clinic?> GetByIdAsync(int id)
        => await _context.Clinics.FindAsync(id);

    public async Task AddAsync(Clinic clinic)
        => await _context.Clinics.AddAsync(clinic);

    public Task UpdateAsync(Clinic clinic)
    {
        _context.Clinics.Update(clinic);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(int id)
    {
        var clinic = await _context.Clinics.FindAsync(id);
        if (clinic is not null)
        {
            _context.Clinics.Remove(clinic);
        }
    }
}
