using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ProjectManager.API.ViewModels.Project;
using ProjectManager.Domain.Authentication;
using ProjectManager.Domain.Entities;
using ProjectManager.Infrastructure.Base.Interfaces;
using ProjectManager.Infrastructure.Interfaces;
using AutoMapper;

namespace ProjectManager.API.Services
{
    public class ProjectService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProjectRepository _projectRepository;
        private readonly ITodoRepository _todoRepository;
        private readonly IClientRepository _clientRepository;
        private readonly UserManager<User> _userManager;
        public ProjectService(IUnitOfWork unitOfWork
            , IProjectRepository projectRepository
            , ITodoRepository todoRepository
            , IClientRepository clientRepository
            , UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _projectRepository = projectRepository;
            _todoRepository = todoRepository;
            _clientRepository = clientRepository;
            _userManager = userManager;

        }

        public Task<IdentityResult> InsertProject(Project project)
        {
            project.Id = Guid.NewGuid().ToString();
            project.CreatedDate = DateTime.Now;
            project.UpdatedDate = DateTime.Now;
            project.Status = (int)Project.Statuses.Open;
            var user = _userManager.FindByIdAsync(project.CreatedBy).Result;
            _projectRepository.Add(project);
            _unitOfWork.SaveChanges();
            if (user.Projects == null)
                user.Projects = new List<Project> { project };
            else
                user.Projects.Add(project);
            return _userManager.UpdateAsync(user);
        }

        public Task<List<Project>> GetProjects(Project.Statuses status)
        {

            return _projectRepository.GetProjects(status);
        }

        public Task<Project> GetProjectById(string projectId)
        {
            return _projectRepository.GetProjectById(projectId);
        }


        public async Task<IdentityResult> AddMemberToProject(Project project, string userId)
        {
            var user = _userManager.FindByIdAsync(userId).Result;
            if (user.Projects != null)
            {
                if (user.Projects.Contains(project))
                {
                    return IdentityResult.Failed();
                }
                user.Projects.Add(project);
                return IdentityResult.Success;
            }
            user.Projects = new List<Project> { project };
            return await _userManager.UpdateAsync(user);
        }

        public List<User> GetMembers(Project project, string userId)
        {
            return project.Users.Contains(_userManager.FindByIdAsync(userId).Result) ? project.Users.ToList() : null;
        }

        public bool IsMemberInProject(Project project, string userId)
        {
            return project.Users.Contains(_userManager.FindByIdAsync(userId).Result);
        }

        public Task<List<Project>> GetAllProjects()
        {
            return _projectRepository.GetAllProjects();
        }

        public Task<List<Project>> GetProjectByClient(string clientId)
        {
            return _projectRepository.GetProjectByClient(clientId);
        }

        public Task<int> UpdateProject(Project project, ProjectEditingModel model)
        {
            if (model.ProjectName != null)
            {
                project.ProjectName = model.ProjectName;
            }

            else if (model.Status != project.Status)
            {
                project.Status = model.Status;
            }
            else if (model.EndDate != null)
            {
                project.EndDate = model.EndDate;
            }
            else if (model.Description != null)
            {
                project.Description = model.Description;
            }

            project.UpdatedBy = model.UpdatedBy;
            project.UpdatedDate = model.UpdatedDate;
            _projectRepository.Update(project);
            return _unitOfWork.SaveChanges();
        }
    }
}
