using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Discovery;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace TestProject2
{
	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		public void TestSearchCar()
		{
			if (Parcheggio.Program.SearchCar("ab123xy") == false)
			{
				//l'auto ab123xy deve essere nel parcheggio
				throw new Exception("L'auto di prova non è stata trovata!");
			}
			if (Parcheggio.Program.SearchCar("xxx") == true)
			{
				throw new Exception("Error!");
			}
		}

		[TestMethod]
		public void TestGetDBConnection()
		{
			SqlConnection connection;

			connection = Parcheggio.Program.GetDBConnection();

			if (connection is null)
			{
				throw new Exception("Can't create connection!");
			} else {
				connection.Open();
				if(connection.State != System.Data.ConnectionState.Open) {
					throw new Exception("Can't open connection!");
				}
				connection.Close();
			}
		}
		[TestMethod]
		public void TestCarEntryTime()
		{
			//- se passando ab123xy come argomento il metodo restituisce una tabella vuota, il risultato è errato
			//- se passando ab123xy come argomento il metodo restituisce una tabella con un record, il risultato è corretto
			//- se passando aa111xx come argomento il metodo restituisce una tabella con un record, il risultato è errato
		if (Parcheggio.Program.CarEntryTime("ab123xy").Rows.Count == 0)
			{
				//risultato errato: la macchina ab123xy deve essere presente
				throw new Exception("Veicolo ab123xy non trovato!");
			}
		if (Parcheggio.Program.CarEntryTime("ab123xy").Rows.Count != 1)
			{
				//risultato errato: la macchina ab123xy deve essere presente
				throw new Exception("Veicolo ab123xy non trovato!");
			}
		if (Parcheggio.Program.CarEntryTime("aa111xx").Rows.Count == 1)
			{
				//risultato errato: la macchina aa111xx non deve essere presente
				throw new Exception("Veicolo aa111xx trovato!");
			}
		}

		[TestMethod]
		public void TestFreePlaces()
		{
			if (Parcheggio.Program.FreePlaces(10) == true){
				//risultato corretto
			}
			else {
				//risultato errato
				throw new Exception("Calcolo dei posti liberi errato!");
			}
			if (Parcheggio.Program.FreePlaces(20) == true){
				//risultato corretto
			}
			else {
				//risultato errato
				throw new Exception("Calcolo dei posti liberi errato!");
			}

		}
	}
}