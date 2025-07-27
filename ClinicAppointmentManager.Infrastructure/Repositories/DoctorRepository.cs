using ClinicAppointmentManager.Core.Entities;
using ClinicAppointmentManager.Core.Interfaces;
using ClinicAppointmentManager.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ClinicAppointmentManager.Infrastructure.Repositories
{
    public class DoctorRepository : Repository<Doctor>, IDoctorRepository
    {
        private readonly ClinicDbContext _context;

        public DoctorRepository(ClinicDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Doctor>> GetDoctorsWithSpecialtiesAsync()
        {
            return await _context.Doctors.Include(d => d.Specialty).ToListAsync();
        }
    }
}
