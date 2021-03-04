using System.Collections.Generic;
using System.Threading.Tasks;
using ProjectManager.Domain.Entities;
using ProjectManager.Infrastructure.Base.Interface;

namespace ProjectManager.Infrastructure.Interface
{
    public interface IClientRepository: IRepository<Client>
    {
        Task<Client> GetClientById(string id);
        Task<List<Client>> GetClients();
    }
}