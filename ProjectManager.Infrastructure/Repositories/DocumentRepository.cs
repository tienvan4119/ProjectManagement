using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Domain.Entities;
using ProjectManager.Infrastructure.Base.Repositories;
using ProjectManager.Infrastructure.Interfaces;

namespace ProjectManager.Infrastructure.Repositories
{
    public class DocumentRepository : Repository<Document>, IDocumentRepository
    {
        public DocumentRepository(DbFactory dbFactory) : base(dbFactory)
        {

        }

        public Task<Document> GetById(string id)
        {
            return Entities.FirstAsync(_ => _.Id.Equals(id));
        }
    }
}
