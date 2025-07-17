using ClinicAppointmentManager.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicAppointmentManager.Infrastructure.Data.Configurations
{
    public class PatientConfiguration : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> builder)
        {
            builder.HasKey(e => e.PatientId);

            builder.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.Address)
                .HasMaxLength(255);

            builder.Property(e => e.PhoneNumber)
                .HasMaxLength(255);

            builder.Property(p => p.DateOfBirth)
            .IsRequired();

            builder.Ignore(e => e.FullName);

            builder.HasMany(p => p.Appointments)
               .WithOne(a => a.Patient)
               .HasForeignKey(a => a.PatientId);
        }
    }
}
