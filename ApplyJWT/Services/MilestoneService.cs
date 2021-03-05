using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectManager.Domain.Entities;
using ProjectManager.Infrastructure.Base.Interfaces;
using ProjectManager.Infrastructure.Interfaces;

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
            milestone.Id = Guid.NewGuid().ToString();
            milestone.CreatedDate = DateTime.Now;
            _milestoneRepository.Add(milestone);
            return _unitOfWork.SaveChanges();
        }

        public Task<Milestone> GetMilestoneById(string milestoneId)
        {
            return _milestoneRepository.FindMilestoneById(milestoneId);
        }

        public Task<int> AssignMilestone(string milestoneId, string assignedUserId)
        {
            var milestone = _milestoneRepository.FindMilestoneById(milestoneId);
            milestone.Result.AssignedTo = assignedUserId;
            _milestoneRepository.Update(milestone.Result);
            return _unitOfWork.SaveChanges();
        }
    }
}