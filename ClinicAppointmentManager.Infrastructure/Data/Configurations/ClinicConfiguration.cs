using ClinicAppointmentManager.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicAppointmentManager.Infrastructure.Data.Configurations
{
    public class ClinicConfiguration : IEntityTypeConfiguration<Clinic>
    {
        public void Configure(EntityTypeBuilder<Clinic> builder)
        {
            builder.HasKey(e => e.ClinicId);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(e => e.Address)
                .HasMaxLength(255);

            builder.Property(e => e.PhoneNumber)
                .HasMaxLength(20);

            builder.HasMany(c => c.Doctors)
               .WithOne(d => d.Clinic)
               .HasForeignKey(d => d.ClinicId);
        }
    }
}
