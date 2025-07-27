using ClinicAppointmentManager.Core.Entities;
using ClinicAppointmentManager.Core.Interfaces;
using ClinicAppointmentManager.Infrastructure.Data;

namespace ClinicAppointmentManager.Infrastructure.Repositories
{
    public class SpecialtyRepository : Repository<Specialty>, ISpecialtyRepository
    {
        private readonly ClinicDbContext _context;

        public SpecialtyRepository(ClinicDbContext context) : base(context)
        {
            _context = context;
        }

    }
}
