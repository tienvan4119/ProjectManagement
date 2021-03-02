using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Interface;

namespace ProjectManager.Infrastructure.Repositories
{
    public class ClientRepository: Repository<Client>, IClientRepository
    {
        public ClientRepository(DbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}