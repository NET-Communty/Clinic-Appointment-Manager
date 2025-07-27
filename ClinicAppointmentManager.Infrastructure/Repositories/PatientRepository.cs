using ClinicAppointmentManager.Core.Entities;
using ClinicAppointmentManager.Core.Interfaces;
using ClinicAppointmentManager.Infrastructure.Data;

namespace ClinicAppointmentManager.Infrastructure.Repositories
{
    public class PatientRepository : Repository<Patient>, IPatientRepository
    {
        private readonly ClinicDbContext _context;

        public PatientRepository(ClinicDbContext context) : base(context)
        {
            _context = context;
        }

    }
}
