using System.Collections.Generic;
using System.Threading.Tasks;
using ProjectManager.Domain.Entities;
using ProjectManager.Infrastructure.Base.Interfaces;

namespace ProjectManager.Infrastructure.Interfaces
{
    public interface IMilestoneRepository : IRepository<Milestone>
    {
        Task<List<Milestone>> GetMilestones(string projectId);
        Task<Milestone> FindMilestoneById(string id);
    }
}
