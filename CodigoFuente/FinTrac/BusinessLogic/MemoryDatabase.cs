using Domain;

namespace BusinessLogic
{
	public class MemoryDatabase
	{
		public bool isLoggedIn { get; set; } = false;
		public List<User> Users { get; set; } = new List<User>();
		public User currentUser { get; set; }
		public Account currentAccount { get; set; }
		public Workspace currentWorkspace { get; set; }
		public MemoryDatabase()
		{
			Users = new List<User>();
			addUsers();
		}

		public void addUsers()
		{
			User otro = new User
			{
				Name = "Admin",
				LastName = "Test",
				Email = "admin@test.com",
				Password = "1234123412",
				Address = "123"
			};
			Workspace defaultWorkspace = new Workspace(otro, $"Personal {otro.Name} {otro.LastName}");
			otro.WorkspaceList.Add(defaultWorkspace);
			Users.Add(otro);
		}

	}

}