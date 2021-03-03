using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ProjectManager.Domain.Entities;

namespace ProjectManager.Domain.Interface
{
    public interface IProjectRepository: IRepository<Project>
    {
        Task<List<Project>> GetProjects();
    }
}
