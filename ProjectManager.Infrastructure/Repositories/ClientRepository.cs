using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Domain.Entities;
using ProjectManager.Infrastructure.Base.Repositories;
using ProjectManager.Infrastructure.Interfaces;

namespace ProjectManager.Infrastructure.Repositories
{
    public class ClientRepository : Repository<Client>, IClientRepository
    {
        public ClientRepository(DbFactory dbFactory) : base(dbFactory)
        {

        }

        public Task<Client> GetClientById(string id)
        {
            return Entities.FirstAsync(e => e.Id.Equals(id));
        }

        public Task<List<Client>> GetClients()
        {
            return Entities.Where(_=>_.IsDeleted == false).ToListAsync();
        }
    }
}