using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ProjectManager.Domain.Entities;
using ProjectManager.Infrastructure.Base.Interface;

namespace ProjectManager.Infrastructure.Interface
{
    public interface IProjectRepository: IRepository<Project>
    {
        Task<List<Project>> GetProjects();
        Task<Project> GetProjectById(string projectId);
    }
}
