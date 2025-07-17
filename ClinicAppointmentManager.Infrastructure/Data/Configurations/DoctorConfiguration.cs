using ClinicAppointmentManager.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicAppointmentManager.Infrastructure.Data.Configurations
{
    public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> builder)
        {
            builder.HasKey(e => e.DoctorId);

            builder.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(e => e.LicenseNumber)
                .IsRequired()
                .HasMaxLength(50);

            builder.Ignore(e => e.FullName);

            builder.HasOne(d => d.Specialty)
               .WithMany(s => s.Doctors)
               .HasForeignKey(d => d.SpecialtyId);

            builder.HasOne(d => d.Clinic)
                   .WithMany(c => c.Doctors)
                   .HasForeignKey(d => d.ClinicId);

            builder.HasMany(d => d.Appointments)
                   .WithOne(a => a.Doctor)
                   .HasForeignKey(a => a.DoctorId);        }
    }
}
