using System.Collections.Generic;
using System.Threading.Tasks;
using ProjectManager.Domain.Entities;

namespace ProjectManager.Domain.Interface
{
    public interface IClientRepository: IRepository<Client>
    {
        Task<Client> GetClientById(string id);
        Task<List<Client>> GetClients();
    }
}