using ClinicAppointmentManager.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicAppointmentManager.Infrastructure.Data.Configurations
{
    public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            builder.HasKey(e => e.AppointmentId);

            builder.Property(a => a.AppointmentDate)
            .IsRequired();

            builder.Property(a => a.DurationMinutes)
            .IsRequired();

            builder.Property(a => a.Status)
            .IsRequired();

            builder.Property(e => e.Notes)
                .HasMaxLength(500);

            builder.HasOne(e => e.Doctor)
                .WithMany(d => d.Appointments)
                .HasForeignKey(e => e.DoctorId);

            builder.HasOne(e => e.Patient)
                .WithMany(p => p.Appointments)
                .HasForeignKey(e => e.PatientId);
        }
    }
}
