using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectManager.API.ViewModels.Document;
using ProjectManager.Domain.Entities;
using ProjectManager.Infrastructure.Base.Interfaces;
using ProjectManager.Infrastructure.Interfaces;

namespace ProjectManager.API.Services
{
    public class DocumentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProjectRepository _projectRepository;
        private readonly IDocumentRepository _documentRepository;
        public DocumentService(IUnitOfWork unitOfWork
            , IProjectRepository projectRepository
            , IDocumentRepository documentRepository)
        {
            _unitOfWork = unitOfWork;
            _projectRepository = projectRepository;
            _documentRepository = documentRepository;
        }

        public async Task<int> AddDocument(DocumentAddingModel model)
        {
            var document = new Document
            {
                CreatedBy = model.CreatedBy,
                CreatedDate = DateTime.Now,
                IsDeleted = false,
                Name = model.Name,
                ProjectId = model.ProjectId,
                Url = model.Url
            };
            _documentRepository.Add(document);
            return await _unitOfWork.SaveChanges();
        }

        public async Task<Document> GetById(string id)
        {
            return await _documentRepository.GetById(id);
        }

        public async Task<int> DeleteDocument(string id)
        {
            var document = await GetById(id);
            _documentRepository.Delete(document);
            return await _unitOfWork.SaveChanges();
        }
    }
}
