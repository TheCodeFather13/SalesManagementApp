﻿using SalesManagementApp.Data;
using SalesManagementApp.Entities;
using SalesManagementApp.Extensions;
using SalesManagementApp.Models;
using SalesManagementApp.Services.Contracts;

namespace SalesManagementApp.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly SalesManagementDbContext _salesManagementDbContext;

        public AppointmentService(SalesManagementDbContext salesManagementDbContext)
        {
            _salesManagementDbContext = salesManagementDbContext;
        }

        public async Task AddAppointment(AppointmentModel appointmentModel)
        {
            try
            {
                Appointment appointment = appointmentModel.Convert();
                await _salesManagementDbContext.AddAsync(appointment);
                await _salesManagementDbContext.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task DeleteAppointment(int id)
        {
            try
            {
                Appointment? appointment = await _salesManagementDbContext.Appointments.FindAsync(id);
                if (appointment != null)
                {
                    _salesManagementDbContext.Remove(appointment);
                    await _salesManagementDbContext.SaveChangesAsync();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<AppointmentModel>> GetAppointments()
        {
            try
            {
                return await _salesManagementDbContext.Appointments.Where(e => e.EmployeeId == 9).Convert();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task UpdateAppointment(AppointmentModel appointmentModel)
        {
            try
            {
                Appointment? appointment = await _salesManagementDbContext.Appointments.FindAsync(appointmentModel.Id);
                if(appointment != null)
                {
                    appointment.Description = appointmentModel.Description;
                    appointment.IsAllDay = appointmentModel.IsAllDay;
                    appointment.RecurrenceId = appointmentModel.RecurrenceId;
                    appointment.RecurrenceRule = appointmentModel.RecurrenceRule;
                    appointment.RecurrenceException = appointmentModel.RecurrenceException;
                    appointment.StartTime = appointmentModel.StartTime;
                    appointment.EndTime = appointmentModel.EndTime;
                    appointment.Location = appointmentModel.Location;
                    appointment.Subject = appointmentModel.Subject;

                    await _salesManagementDbContext.SaveChangesAsync(); 
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
