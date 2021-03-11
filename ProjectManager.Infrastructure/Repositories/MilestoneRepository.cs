using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Domain.Entities;
using ProjectManager.Infrastructure.Base.Repositories;
using ProjectManager.Infrastructure.Interfaces;

namespace ProjectManager.Infrastructure.Repositories
{
    public class MilestoneRepository : Repository<Milestone>, IMilestoneRepository
    {
        public MilestoneRepository(DbFactory dbFactory) : base(dbFactory)
        {

        }

        public Task<List<Milestone>> GetMilestones(string projectId)
        {
            return Entities.Where(_ => _.ProjectId.Equals(projectId)).ToListAsync();
        }

        public Task<Milestone> FindMilestoneById(string id)
        {
            return Entities.FirstAsync(_ => _.Id.Equals(id));
        }

    }
}