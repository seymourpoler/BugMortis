using System;
using System.Linq;
using BugMortis.Data.Entity;
using BugMortis.Data.SqlServer.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BugMortis.Data.SqlServer.Tests
{
    [TestClass]
    public class ProjectRepositoryTest : BaseRepositoryTest
    {
        private ProjectRepository _projectRepository;
        private CompanyRepository _companyRepository;
        private UserRepository _userRepository;
        private BugRepository _bugRepository;
        private Company _simpleCompany;
        private Project _simpleProject, _projectTwo;

        public ProjectRepositoryTest():base()
        {
            _projectRepository = new ProjectRepository(_db);
            _companyRepository = new CompanyRepository(_db);
            _userRepository = new UserRepository(_db);
            _bugRepository = new BugRepository(_db);
        }

        [TestInitialize]
        public void SetUp()
        {
            _simpleCompany = new Company { Name = "New Co." };
            _simpleCompany.IdCompany = _companyRepository.Insert(_simpleCompany);
            _simpleProject = new Project { Name = "New Project", IdCompany = _simpleCompany.IdCompany };
            _projectRepository.Insert(_simpleProject);
            _projectTwo = new Project { Name = "New Project II", IdCompany = _simpleCompany.IdCompany };
        }

        [TestCleanup]
        public void TearDown()
        {
            _companyRepository.Delete(_simpleCompany.IdCompany);
            _projectRepository.Delete(_simpleProject.IdProject);
            _projectRepository.Delete(_projectTwo.IdProject);
        }

        [TestMethod]
        public void ProjectRepository_save_simple_project()
        {
            var  expected = _projectRepository.GetById(_simpleProject.IdProject);

            Assert.AreEqual(expected, _simpleProject);
        }

        [TestMethod]
        public void ProjectRepository_get_all_projects()
        {
            _projectRepository.Insert(_projectTwo);

            var expected = _projectRepository.GetAll();

            Assert.IsTrue(expected.Count() > 1);
        }

        [TestMethod]
        public void ProjectRepository_update_project()
        {
            var modified = _projectRepository.GetById(_simpleProject.IdProject);
            modified.Name = "Custom Name";
            _projectRepository.Update(modified);

            var expected = _projectRepository.GetById(_simpleProject.IdProject);

            Assert.AreEqual(expected, modified);
        }

        [TestMethod]
        public void ProjectRepository_delete_project()
        {
            _projectRepository.Delete(_simpleProject.IdProject);
            var expected = _projectRepository.GetById(_simpleProject.IdProject);

            Assert.IsNull(expected);
        }

        [TestMethod]
        public void ProjectRepository_delete_Project_with_a_user()
        {
            var user = new User { Name = "Name User" };
            user.Projects.Add(_simpleProject);
            _userRepository.Insert(user);
            
            _projectRepository.Delete(_simpleProject.IdProject);
            var expectedProject = _projectRepository.GetById(_simpleProject.IdProject);
            var expectedUser = _userRepository.GetById(user.IdUser);

            Assert.IsNull(expectedProject);
            Assert.IsNull(expectedUser);
        }

        [TestMethod]
        public void ProjectRepository_delete_Project_with_a_bug()
        {
            var bug = new Bug { Name = "Name Bug", IdStatus = new Guid("00000000-0000-0000-0000-000000000001"), IdPriority = new Guid("00000000-0000-0000-0000-000000000001") };
            bug.IdProject = _simpleProject.IdProject;
            _bugRepository.Insert(bug);

            _projectRepository.Delete(_simpleProject.IdProject);
            var expectedProject = _projectRepository.GetById(_simpleProject.IdProject);
            var expectedBug = _bugRepository.GetById(bug.IdBug);

            Assert.IsNull(expectedProject);
            Assert.IsNull(expectedBug);
        }

        [TestMethod]
        public void ProjectRepository_delete_Project_with_bugs_and_users()
        {
            var bugOne = new Bug { Name = "Name BugOne", IdStatus = new Guid("00000000-0000-0000-0000-000000000001"), IdPriority = new Guid("00000000-0000-0000-0000-000000000001") };
            bugOne.IdProject = _simpleProject.IdProject;
            _bugRepository.Insert(bugOne);
            var bugTwo = new Bug { Name = "Name BugOne", IdStatus = new Guid("00000000-0000-0000-0000-000000000001"), IdPriority = new Guid("00000000-0000-0000-0000-000000000001") };
            bugTwo.IdProject = _simpleProject.IdProject;
            _bugRepository.Insert(bugTwo);

            var userOne = new User { Name = "Name UserOne" };
            userOne.Projects.Add(_simpleProject);
            _userRepository.Insert(userOne);
            var userTwo = new User { Name = "Name UserTwo" };
            userTwo.Projects.Add(_simpleProject);
            _userRepository.Insert(userTwo);

            _projectRepository.Delete(_simpleProject.IdProject);
            var expectedProject = _projectRepository.GetById(_simpleProject.IdProject);
            var expectedBugOne = _bugRepository.GetById(bugOne.IdBug);
            var expectedBugTwo = _bugRepository.GetById(bugTwo.IdBug);
            var expectedUserOne = _userRepository.GetById(userOne.IdUser);
            var expectedUserTwo = _userRepository.GetById(userTwo.IdUser);

            Assert.IsNull(expectedProject);
            Assert.IsNull(expectedBugOne);
            Assert.IsNull(expectedBugTwo);
            Assert.IsNull(expectedBugOne);
            Assert.IsNull(expectedUserOne);
            Assert.IsNull(expectedUserTwo);
        }
    }
}
