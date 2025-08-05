
using System;
using ClinicAppointmentManager.Core.Interfaces;
using ClinicAppointmentManager.Infrastructure;
using ClinicAppointmentManager.Infrastructure.Data;
using ClinicAppointmentManager.Services;
using ClinicAppointmentManager.Services.Interfaces;
using ClinicAppointmentManager.Services.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<ClinicDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register services
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IDoctorService, DoctorService>();
builder.Services.AddScoped<IClinicService, ClinicService>();
builder.Services.AddScoped<ISpecialtyService, SpecialtyService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();


builder.Services.AddValidatorsFromAssemblyContaining<DoctorPostDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<ClinicPostDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<SpecialtyPostDtoValidator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/openapi/v1.json", "Clinic Appointment Manager API V1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
