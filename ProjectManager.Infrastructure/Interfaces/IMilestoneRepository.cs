using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ProjectManager.Domain.Entities;
using ProjectManager.Infrastructure.Base.Interface;

namespace ProjectManager.Infrastructure.Interface
{
    public interface IMilestoneRepository : IRepository<Milestone>
    {
        Task<List<Milestone>> GetMilestones(string projectId);
        Task<Milestone> FindMilestoneById(string id);
    }
}
