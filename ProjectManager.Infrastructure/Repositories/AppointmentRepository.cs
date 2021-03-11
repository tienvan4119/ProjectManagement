using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Domain.Entities;
using ProjectManager.Infrastructure.Base.Repositories;
using ProjectManager.Infrastructure.Interfaces;

namespace ProjectManager.Infrastructure.Repositories
{
    public class AppointmentRepository : Repository<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(DbFactory dbFactory) : base(dbFactory)
        {

        }

        public Task<Appointment> GetById(string appointmentId)
        {
            return Entities.FirstAsync(e => e.Id.Equals(appointmentId));
        }
    }
}