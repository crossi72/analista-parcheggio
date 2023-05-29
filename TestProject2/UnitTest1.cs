namespace TestProject2
{
	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		public void TestSearchCar()
		{
			if (Parcheggio.Program.SearchCar("xxx") == true)
			{
				throw new Exception("Error!");
			}
		}

		[TestMethod]
		public void TestGetDBConnection()
		{
			if (Parcheggio.Program.GetDBConnection() is null)
			{
				throw new Exception("Error!");
			}
		}
	}
}