using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ProjectManager.Domain.Entities;

namespace ProjectManager.Domain.Interface
{
    public interface IMilestoneRepository : IRepository<Milestone>
    {
        Task<List<Milestone>> GetMilestones(string projectId);
        Task<Milestone> FindMilestoneById(string id);
    }
}
