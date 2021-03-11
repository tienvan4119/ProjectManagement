using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ProjectManager.Domain.Entities;
using ProjectManager.Infrastructure.Base.Interfaces;

namespace ProjectManager.Infrastructure.Interfaces
{
    public interface IDocumentRepository : IRepository<Document>
    {
        Task<Document> GetById(string id);
    }
}
