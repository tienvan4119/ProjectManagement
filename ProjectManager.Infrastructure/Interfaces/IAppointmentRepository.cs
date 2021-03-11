using System.Threading.Tasks;
using ProjectManager.Domain.Entities;
using ProjectManager.Infrastructure.Base.Interfaces;

namespace ProjectManager.Infrastructure.Interfaces
{
    public interface IAppointmentRepository : IRepository<Appointment>
    {
        Task<Appointment> GetById(string appointmentId);
    }
}