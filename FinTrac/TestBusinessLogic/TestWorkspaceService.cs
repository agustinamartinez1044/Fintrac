﻿using Domain;
using BusinessLogic;
using Domain.Exceptions;

namespace TestBusinessLogic
{
    [TestClass]
    public class TestWorkspaceService
    {
        private WorkspaceService _service;
        private UserService _userService;
        private MemoryDatabase newMemory;
        [TestInitialize]
        public void SetUp()
        {
            newMemory = new MemoryDatabase();
            _service = new WorkspaceService(newMemory);
            _userService = new UserService(newMemory);

        }
        [TestMethod]
        public void AddWorkspace()
        {
            User useradmin = new User { Name = "Test", LastName = "Test", Email = "a@a.com", Password = "12345678909" };
            var workspace = new Workspace(useradmin, "Test");
            _userService.Add(useradmin);
            _service.Add(useradmin, workspace);
            Assert.AreEqual(workspace, useradmin.WorkspaceList.First(x => x == workspace));

        }
        [TestMethod]
        [ExpectedException(typeof(WorkspaceAlreadyExistsException))]
        public void AddWorkspaceAlreadyExists()
        {
            User useradmin = new User { Name = "Test", LastName = "Test", Email = "a@a.com", Password = "12345678909" };
            var workspace = new Workspace(useradmin, "Test");
            _userService.Add(useradmin);
            _service.Add(useradmin, workspace);
            _service.Add(useradmin, workspace);
        }

        [TestMethod]
        public void GetWorkspace()
        {
            User useradmin = new User { Name = "Test", LastName = "Test", Email = "a@a.com", Password = "12345678909" };
            var workspace = new Workspace(useradmin, "Test");
            _userService.Add(useradmin);
            _service.Add(useradmin,workspace);
            Assert.AreEqual(workspace, _service.Get(workspace.ID));
        }

        [TestMethod]
        public void UpdateWorkspaceName()
        {
            User useradmin = new User { Name = "Test", LastName = "Test", Email = "a@a.com", Password = "12345678909" };
            var workspace = new Workspace(useradmin, "Test");
            _service.Add(useradmin, workspace);
            _service.UpdateName(workspace, "Nuevo Workspace");
            Assert.AreEqual("Nuevo Workspace", workspace.Name);
        }


        [TestMethod]
        public void DeleteWorkspaceWithOtherUsers()
        {
            User useradmin = new User { Name = "Test", LastName = "Test", Email = "a@a.com", Password = "12345678909" };
            User otherUser = new User { Name = "Other", LastName = "Test", Email = "test@a.com", Password = "12345678909" };
            var workspace = new Workspace(useradmin, "Test");
            _userService.Add(useradmin);
            _userService.Add(otherUser);
            _service.Add(useradmin, workspace);
            _service.Add(otherUser, workspace);
            _service.DeleteWorkspace(workspace);

            Assert.AreEqual(workspace.UserAdmin, otherUser);
        }
    }

}
