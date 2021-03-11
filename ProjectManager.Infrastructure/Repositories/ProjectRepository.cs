using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Domain.Entities;
using ProjectManager.Infrastructure.Base.Repositories;
using ProjectManager.Infrastructure.Interfaces;

namespace ProjectManager.Infrastructure.Repositories
{
    public class ProjectRepository : Repository<Project>, IProjectRepository
    {
        public ProjectRepository(DbFactory dbFactory) : base(dbFactory)
        {

        }

        public Task<List<Project>> GetAllProjects()
        {
            return Entities.ToListAsync();
        }

        public async Task<List<Project>> GetProjects(Project.Statuses status)
        {
            return await Entities.Include(_ => _.Users)
                .Include(_ => _.Tasks)
                .Include(_ => _.Milestones)
                .Include(_ => _.Users)
                .Where(_ => _.Status == (int) status).ToListAsync();
        }

        public async Task<Project> GetProjectById(string projectId)
        {
            return await Entities.Include(_ => _.Users)
                
                .FirstAsync(_ => _.Id.Equals(projectId));
        }

        public Task<List<Project>> GetProjectByClient(string clientId)
        {
            return Entities.Where(_ => _.ClientId.Equals(clientId)).ToListAsync();
        }

        public async Task<Project> GetMilestoneInProject(string projectId)
        {
            return await Entities.Include(_ => _.Milestones)
                .FirstAsync(_ => _.Id == projectId);
        }

        public async Task<Project> GetAppointmentInProject(string projectId)
        {
            return await Entities.Include(_ => _.Appointments)
                .FirstAsync(_ => _.Id == projectId);
        }
    }
}
