using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Parcheggio
{
	internal class Program
	{

		/// <summary>
		/// Create a connection to database
		/// </summary>
		/// <returns></returns>
		static private SqlConnection GetDBConnection(){
			string connectionString;
			SqlConnection connection;

			//variables iniatialization
			connectionString = "Data Source=localhost;Initial Catalog=parcheggio;Integrated Security=True";
			
			//create connection to database
			connection = new SqlConnection(connectionString);

			//return connection to caller
			return connection;
		}

		/// <summary>
		/// Inserisce un veicolo nel parcheggio
		/// </summary>
		/// <param name="targa">targa del veicolo da inserire</param>
		static private void InsertCar(string targa){
				//variables declaration
				string queryString;
				SqlConnection connection;
				SqlCommand command;

				//variables iniatialization
				queryString = $"INSERT INTO veicoli (targa, ingresso) VALUES ('{targa}', GETDATE())";

				using (connection = GetDBConnection())
				{
					command = new SqlCommand(queryString, connection);
					connection.Open();
					command.ExecuteNonQuery();
				}
			}

			/// <summary>
			/// Loads data from database
			/// </summary>
			/// <param name="tableName">Name of the table</param>
			/// <returns>Datatable containing data from <paramref name="tableName"/>table</returns>
			static private DataTable GetDataFromDB(string tableName)
		{
			//variables declaration
			string queryString;
			SqlConnection connection;
			SqlDataAdapter adapter;
			DataTable resultTable;

			//variables iniatialization
			queryString = $"SELECT * FROM {tableName}";

			using (connection = GetDBConnection())
			{
				adapter = new SqlDataAdapter(queryString, connection);

				resultTable = new DataTable();
				adapter.Fill(resultTable);
				return resultTable;
			}
		}
		
		/// <summary>
		/// Loads data from database
		/// </summary>
		/// <param name="tableName">Name of the table</param>
		/// <returns>Datatable containing data from <paramref name="tableName"/>table</returns>
		static private DataTable GetCarsIntoParking()
		{
			//variables declaration
			string queryString;
			SqlConnection connection;
			SqlDataAdapter adapter;
			DataTable resultTable;

			//variables iniatialization
			queryString = $"SELECT * FROM veicoli WHERE uscita IS NULL";

			using (connection = GetDBConnection())
			{
				adapter = new SqlDataAdapter(queryString, connection);

				resultTable = new DataTable();
				adapter.Fill(resultTable);
				return resultTable;
            }
        }

		/// <summary>
		/// Search a car
		/// </summary>
		/// <param name="targa">Targa dell'automobile</param>
		/// <returns>True if <paramref name="targa"/> is in the parking</returns>
		static public bool SearchCar(string targa)
		{
			//variables declaration
			string queryString; 
			SqlConnection connection;
			SqlDataAdapter adapter;
			DataTable resultTable;

			//variables iniatialization
			queryString = $"SELECT * FROM veicoli where targa='{targa}' and uscita is null";

			using (connection = GetDBConnection())
			{
				adapter = new SqlDataAdapter(queryString, connection);

				resultTable = new DataTable();
				adapter.Fill(resultTable);

				if (resultTable.Rows.Count == 0)
				{
					return false;
				}
				else
				{
					return true;
				}
			}
		}

		static void Main(string[] args){
			DataTable parkingTable;
			float costoOrario;
			int posti;

			parkingTable = GetDataFromDB("parcheggio");
			costoOrario = Convert.ToSingle(parkingTable.Rows[0]["costo_orario"]);
			posti = Convert.ToInt32(parkingTable.Rows[0]["posti"]);

			List<Veicolo> veicoli = new List<Veicolo>();

			while (true)
			{
				Console.Clear();
				Console.WriteLine("1- Arrivo nuovo veicolo");
				Console.WriteLine("2- Stampa tutti i veicoli presenti nel parcheggio");
				Console.WriteLine("3- Uscita veicolo presente nel parcheggio");
				Console.WriteLine("9- Uscita");
				string scelta = Console.ReadLine();

				if (scelta == "1")
				{
					ArrivoVeicoli();
				}

				if (scelta == "2")
				{
					Print();
				}

				if (scelta == "3")
				{
					Console.WriteLine("Inserire la targa del veicolo che sta uscendo:");
					string targa = Console.ReadLine().ToLower();
					bool veicoloTrovato = false;
					foreach (Veicolo veicolo in veicoli)
					{
                        
						if (targa == "")
						{
							break;
						}
                        
						if (veicolo.targa != null && veicolo.targa.ToLower() == targa)
						{
							double tariffa = veicolo.Tempoparcheggio();
							Console.WriteLine($"Il veicolo con targa {veicolo.targa} esce il {veicolo.uscita.Date.ToString("dd/MM/yyyy")} alle ore {veicolo.uscita.ToString("HH:mm")} e deve pagare {tariffa} euro");
							veicolo.targa = null;
							veicoloTrovato = true;
							break;
						}
					}

					if (!veicoloTrovato)
					{
						Console.WriteLine("Nessun veicolo trovato.");
					}
					Console.ReadLine();

				}
				if (scelta == "9")
				{
					break;
				}
			}
		}

		/// <summary>
		/// Stampa i veicoli presenti nel parcheggio
		/// </summary>
		private static void Print()
		{
			DataTable cars;

			Console.WriteLine($"I veicoli presenti nel parcheggio sono:");

			cars = GetCarsIntoParking();

			foreach (DataRow row in cars.Rows){
				Console.WriteLine(row["targa"] + " " + row["ingresso"] + "\n");
			}

		Console.ReadLine();
		}

		private static void ArrivoVeicoli()
		{
			Console.Write("Inserire targa veicolo: ");
			string targa = Console.ReadLine().ToLower();
			bool trovato;

			if (targa == "")
			{
				//l'utente non ha inserito la targa
				trovato = true;
				Console.Write("Non hai inserito nessuna targa");
			}
			else {
				//l'utente ha inserito la targa: cerco il veicolo
				if (SearchCar(targa))
				{
					Console.WriteLine($"Il veicolo con targa {targa} è già presente nel parcheggio.");
					trovato = true;
				} else {
					trovato = false;
				}
			}

			if (!trovato)
			{
				//Il veicolo non è nel parcheggio: lo inserisco
				InsertCar(targa);
				Console.WriteLine($"Il veicolo con targa {targa} è stato aggiunto alla lista dei veicoli presenti nel parcheggio");
			}
			Console.ReadLine();
		}
	}
} 