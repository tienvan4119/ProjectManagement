using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Interface;

namespace ProjectManager.API.Services
{
    public class MilestoneService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProjectRepository _projectRepository;
        private readonly IMilestoneRepository _milestoneRepository;
        public MilestoneService(IUnitOfWork unitOfWork
            , IProjectRepository projectRepository
            , IMilestoneRepository milestoneRepository)
        {
            _unitOfWork = unitOfWork;
            _projectRepository = projectRepository;
            _milestoneRepository = milestoneRepository;
        }

        public Task<int> InsertMilestone(Milestone milestone)
        {
            _milestoneRepository.Add(milestone);
            return _unitOfWork.SaveChanges();
        }
    }
}
