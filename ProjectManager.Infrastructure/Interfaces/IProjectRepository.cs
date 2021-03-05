using System.Collections.Generic;
using System.Threading.Tasks;
using ProjectManager.Domain.Entities;
using ProjectManager.Infrastructure.Base.Interfaces;

namespace ProjectManager.Infrastructure.Interfaces
{
    public interface IProjectRepository: IRepository<Project>
    {
        Task<List<Project>> GetProjects();
        Task<Project> GetProjectById(string projectId);
    }
}
