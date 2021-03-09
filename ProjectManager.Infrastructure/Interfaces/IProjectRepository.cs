using System.Collections.Generic;
using System.Threading.Tasks;
using ProjectManager.Domain.Entities;
using ProjectManager.Infrastructure.Base.Interfaces;

namespace ProjectManager.Infrastructure.Interfaces
{
    public interface IProjectRepository: IRepository<Project>
    {
        Task<List<Project>> GetAllProjects();
        Task<List<Project>> GetProjects(Project.Statuses status);
        Task<Project> GetProjectById(string projectId);
        Task<List<Project>> GetProjectByClient(string clientId);
    }
}
