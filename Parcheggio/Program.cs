using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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
		/// Loads data from database
		/// </summary>
		/// <param name="tableName">Name of the table</param>
		/// <returns>Datatable containing data from <paramref name="tableName"/>table</returns>
		static public DataTable GetDataFromDB(string tableName)
		{
			//variables declaration
			string connectionString;
			string queryString;
			SqlConnection connection;
			SqlDataAdapter adapter;
			DataTable resultTable;

			//variables iniatialization
			connectionString = "Data Source=localhost;Initial Catalog=parcheggio;Integrated Security=True";	
			queryString = $"SELECT * FROM {tableName}";

			using (connection = new SqlConnection(connectionString))
			{
				adapter = new SqlDataAdapter(queryString, connection);

				resultTable = new DataTable();
				adapter.Fill(resultTable);
				return resultTable;

				//foreach (DataRow row in clientiTable.Rows)
				//{
				//	Console.WriteLine(row["Nome"] + " " + row["Cognome"]);
				//}
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
			string connectionString;
			string queryString;
			SqlConnection connection;
			SqlDataAdapter adapter;
			DataTable resultTable;

			//variables iniatialization
			connectionString = "Data Source=localhost;Initial Catalog=parcheggio;Integrated Security=True";	
			queryString = $"SELECT * FROM veicoli where targa='{targa}' and uscita is null";

			using (connection = new SqlConnection(connectionString))
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

		static void Main(string[] args)
		{
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
					ArrivoVeicoli(veicoli);
				}

				if (scelta == "2")
				{
					Console.WriteLine($"I veicoli presenti nel parcheggio sono:");
					bool controllo = false;
					foreach (Veicolo veicolo in veicoli)
					{
						if (veicolo.targa != null)
						{
							Console.WriteLine(veicolo.getInfoVeicolo());
							controllo = true;
						}
					}
					if (!controllo) Console.WriteLine("0");
					Console.ReadLine();
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
		private static void ArrivoVeicoli(List<Veicolo> veicoli)
		{
			Console.Write("Inserire targa veicolo: ");
			string targa = Console.ReadLine().ToLower();
			bool trovato;

			if (SearchCar(targa))
			{
				Console.WriteLine($"Il veicolo con targa {targa} è già presente nel parcheggio.");
				trovato = true;
			} else {
				trovato = false;
			}

			if (targa == "")
			{
				trovato = true;
				Console.Write("Non hai inserito nessuna targa");
			}
			if (!trovato)
			{
				DateTime arrivo = DateTime.Now;
				veicoli.Add(new Veicolo(targa, arrivo));
				Console.WriteLine($"Il veicolo con targa {targa} è stato aggiunto alla lista dei veicoli presenti nel parcheggio");
			}
			Console.ReadLine();
		}
	}
} 