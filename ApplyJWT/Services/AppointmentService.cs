using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectManager.API.ViewModels.Appointment;
using ProjectManager.Domain.Entities;
using ProjectManager.Infrastructure.Base.Interfaces;
using ProjectManager.Infrastructure.Interfaces;

namespace ProjectManager.API.Services
{
    public class AppointmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProjectRepository _projectRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        public AppointmentService(IUnitOfWork unitOfWork
            , IProjectRepository projectRepository
            , IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
            _unitOfWork = unitOfWork;
            _projectRepository = projectRepository;
        }

        public async Task<List<Appointment>> GetByProjectId(string projectId)
        {
            var project = await _projectRepository.GetAppointmentInProject(projectId);
            return project.Appointments.ToList();
        }

        public async Task<Appointment> GetById(string appointmentId)
        {
            return await _appointmentRepository.GetById(appointmentId);
        }

        public async Task<int> AddAppointment(AppointmentAddingModel model)
        {
            var appointment = new Appointment
            {
                CreatedBy = model.CreatedBy,
                CreatedDate = DateTime.Now,
                Title = model.Title,
                Description = model.Description,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                ProjectId = model.ProjectId
            };
            _appointmentRepository.Add(appointment);
            return await _unitOfWork.SaveChanges();
        }

        public async Task<int> UpdateAppointment(Appointment appointment, AppointmentEditingModel model)
        {
            if (model.Description != null)
                appointment.Description = model.Description;

            if (model.Title != null)
                appointment.Title = model.Title;
            if (model.EndDate != appointment.EndDate)
                appointment.EndDate = model.EndDate;
            appointment.UpdatedBy = model.UpdatedBy;
            appointment.UpdatedDate = DateTime.Now;
            _appointmentRepository.Update(appointment);
            return await _unitOfWork.SaveChanges();
        }
    }
}
