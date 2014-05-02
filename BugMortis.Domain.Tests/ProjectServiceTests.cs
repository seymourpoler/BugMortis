using System;
using System.Linq;
using System.Collections.Generic;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BugMortis.Data;
using BugMortis.Data.Entity;

namespace BugMortis.Domain.Tests
{
    [TestClass]
    public class ProjectServiceTests 
    {
        private Guid _idProject;
        private Domain.Entity.Project _project;
        Mock<IRepository<Project>> _projectRepository;
        ProjectService _projectService;

        public ProjectServiceTests()
        {
            _idProject = Guid.NewGuid();
            _project = new Domain.Entity.Project { IdProject = _idProject, Name = "Name Project" };
            _projectRepository = new Mock<IRepository<Project>>();
        }

        [TestInitialize]
        public void SetUp()
        {
            _projectService = new ProjectService(_projectRepository.Object);
        }

        [TestMethod]
        public void Projectservice_IsValid_return_false_when_name_is_empty()
        {
            var project = new Domain.Entity.Project();
            var errorMessages = new List<string>();

            var expected = _projectService.IsValid(project, out errorMessages);
            Assert.IsFalse(expected);
            Assert.IsTrue(errorMessages.Any());
        }

        [TestMethod]
        public void Projectservice_IsValid_return_false_when_project_is_null()
        {
            var errorMessages = new List<string>();

            var expected = _projectService.IsValid(null, out errorMessages);
            Assert.IsFalse(expected);
            Assert.IsTrue(errorMessages.Any());
        }

        [TestMethod]
        public void ProjectService_save_project()
        {
            _projectRepository.Setup(x => x.Insert(It.IsAny<Data.Entity.Project>())).Returns(_idProject);
            var projectService = new ProjectService(_projectRepository.Object);

            projectService.Insert(_project);

            _projectRepository.Verify(x => x.Insert(It.IsAny<Data.Entity.Project>()), Times.Once());
        }

        [TestMethod]
        public void ProjectService_getbyid_project()
        {
            _projectRepository.Setup(x => x.GetById(It.IsAny<Guid>())).Returns(new Data.Entity.Project());
            var projectService = new ProjectService(_projectRepository.Object);

            projectService.GetById(_project.IdProject);
            
            _projectRepository.Verify(x => x.GetById(It.IsAny<Guid>()), Times.Once());
        }

        [TestMethod]
        public void ProjectService_get_all_projects()
        {
             var projects = new List <Data.Entity.Project>{
                 new Data.Entity.Project{ IdProject  = Guid.NewGuid(), Name = "Project One"},
                 new Data.Entity.Project{ IdProject  = Guid.NewGuid(), Name = "Project Two"},
                 new Data.Entity.Project{ IdProject  = Guid.NewGuid(), Name = "Project Three"}
             };

            _projectRepository.Setup(x => x.GetAll()).Returns(projects);
            var projectService = new ProjectService(_projectRepository.Object);

            var expected = projectService.GetAll();

            Assert.IsTrue(expected.Any());
            _projectRepository.Verify(x => x.GetAll(), Times.Once());
        }

        [TestMethod]
        public void ProjectService_update_project()
        {
            _projectRepository.Setup(x => x.Update(It.IsAny<Project>()));
            var projectService = new ProjectService(_projectRepository.Object);

            projectService.Update(_project);
           
            _projectRepository.Verify(x => x.Update(It.IsAny<Project>()), Times.Once());
        }

        [TestMethod]
        public void ProjectService_delete_project()
        {
            _projectRepository.Setup(x => x.Delete(It.IsAny<Guid>()));
            var projectService = new ProjectService(_projectRepository.Object);

            projectService.Delete(_idProject);
            
            _projectRepository.Verify(x => x.Delete(It.IsAny<Guid>()), Times.Once());
        }
    }
}
