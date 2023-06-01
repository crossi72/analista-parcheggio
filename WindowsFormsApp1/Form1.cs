using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			this.txtInput.Text = "hello world!!!";
		}

		private void btnArrivoVeicolo_Click(object sender, EventArgs e)
		{
			this.ArrivoVeicoli(10);
		}

		private void btnStampaVeicoli_Click(object sender, EventArgs e)
		{
			this.StampaVeicoli();
		}

		private void btnUscitaVeicolo_Click(object sender, EventArgs e)
		{

		}

		/// <summary>
		/// Create a connection to database
		/// </summary>
		/// <returns></returns>
		static public SqlConnection GetDBConnection()
		{
			string connectionString;
			SqlConnection connection;

			//variables iniatialization
			connectionString = "Data Source=localhost;Initial Catalog=parcheggio;Integrated Security=True";

			//create connection to database
			connection = new SqlConnection(connectionString);

			//return connection to caller
			return connection;
		}

		static private void ExecuteQuery(string query)
		{
			//variables declaration
			string queryString;
			SqlConnection connection;
			SqlCommand command;

			//variables iniatialization
			queryString = $"{query}";

			using (connection = GetDBConnection())
			{
				command = new SqlCommand(queryString, connection);
				connection.Open();
				command.ExecuteNonQuery();
			}
		}

		/// <summary>
		/// Inserisce un veicolo nel parcheggio
		/// </summary>
		/// <param name="targa">targa del veicolo da inserire</param>
		static private void InsertCar(string targa)
		{
			ExecuteQuery($"INSERT INTO veicoli (targa, ingresso) VALUES ('{targa}', GETDATE())");
		}

		/// <summary>
		/// Toglie un veicolo nel parcheggio
		/// </summary>
		/// <param name="targa">targa del veicolo da togliere</param>
		static private void RemoveCar(string targa)
		{
			ExecuteQuery($"UPDATE veicoli SET uscita = GETDATE() WHERE targa = '{targa}' AND uscita IS NULL");
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
		/// Carica l'orario di ingresso di un veicolo
		/// </summary>
		/// <param name="targa">targa del veicolo da cercare</param>
		/// <returns>La DataTable contenente le informazioni sull'ingresso del veicolo con <paramref name="targa"/> indicata </returns>
		public DataTable CarEntryTime(string targa)
		{
			//variables declaration
			string queryString;
			SqlConnection connection;
			SqlDataAdapter adapter;
			DataTable resultTable;

			//variables iniatialization
			queryString = $"SELECT *, DATEDIFF(MI, ingresso, GETDATE()) AS durata FROM veicoli WHERE targa='{targa}' AND uscita IS NULL";

			using (connection = GetDBConnection())
			{
				adapter = new SqlDataAdapter(queryString, connection);

				resultTable = new DataTable();
				adapter.Fill(resultTable);
			}

			return resultTable;
		}

		/// <summary>
		/// Indica se ci sono posti liberi
		/// </summary>
		/// <returns>La DataTable contenente le informazioni sull'ingresso del veicolo</returns>
		static public bool FreePlaces(int postiTotali)
		{
			//variables declaration
			string queryString;
			SqlConnection connection;
			SqlDataAdapter adapter;
			DataTable resultTable;
			bool result;

			//variables iniatialization
			queryString = $"SELECT COUNT(*) AS postiOccupati FROM veicoli WHERE uscita IS NULL";

			using (connection = GetDBConnection())
			{
				adapter = new SqlDataAdapter(queryString, connection);

				resultTable = new DataTable();
				adapter.Fill(resultTable);

				if (Convert.ToInt32(resultTable.Rows[0]["postiOccupati"]) < postiTotali)
				{
					//ci sono posti liberi
					result = true;
				}
				else
				{
					//non ci sono posti liberi
					result = false;
				}
			}

			return result;
		}

		/// <summary>
		/// Search a car
		/// </summary>
		/// <param name="targa">Targa dell'automobile</param>
		/// <returns>True if <paramref name="targa"/> is in the parking</returns>
		public bool SearchCar(string targa)
		{
			if (CarEntryTime(targa).Rows.Count == 0)
			{
				return false;
			}
			else
			{
				return true;
			}
		}

		/// <summary>
		/// Toglie un veicolo dal parcheggio
		/// Calcola il costo della sosta
		/// </summary>
		/// <param name="costoOrario">Costo per un'ora di sosta</param>
		private void Exit(float costoOrario)
		{
			//chiedo all'utente la targa del veicolo
			Console.WriteLine("Inserire la targa del veicolo che sta uscendo:");
			string targa = Console.ReadLine().ToLower();
			DataTable carData;
			double hours, cost;

			//cerco nel database il veicolo con la targa indicata
			carData = CarEntryTime(targa);

			if (carData.Rows.Count == 0)
			{
				//il veicolo non è nel parcheggio
				Console.WriteLine("Nessun veicolo trovato.");
			}
			else
			{
				//il veicolo è nel parcheggio, calcolo il costo
				//calcolo quante ore (con i decimali) è rimasto nel parcheggio
				hours = Convert.ToSingle(carData.Rows[0]["durata"]) / 60.0;
				//arrotondo le ore all'intero successivo
				hours = Math.Ceiling(hours);
				//calcolo il costo totale
				cost = hours * costoOrario;

				Console.WriteLine($"Il veicolo con targa {targa} esce il {DateTime.Now.Date} alle ore {DateTime.Now.TimeOfDay} e deve pagare {cost} euro");

				//tolgo il veicolo dal parcheggio
				RemoveCar(targa);
			}

			Console.ReadLine();
		}

		/// <summary>
		/// Stampa i veicoli presenti nel parcheggio
		/// </summary>
		private void StampaVeicoli()
		{
			DataTable cars;

			this.txtOutput.Text = $"I veicoli presenti nel parcheggio sono:\r\n";

			cars = GetCarsIntoParking();

			foreach (DataRow row in cars.Rows)
			{
				this.txtOutput.Text += row["targa"] + " " + row["ingresso"] + "\r\n";
			}
		}

		/// <summary>
		/// Gestisce l'ingresso di un veicolo nel parcheggio
		/// </summary>
		/// <param name="posti">numero di posti disponibili nel parcheggio</param>
		private void ArrivoVeicoli(int posti)
		{
			string targa = this.txtInput.Text;
			bool trovato;

			if (targa == "")
			{
				//l'utente non ha inserito la targa
				trovato = true;
				this.txtOutput.Text = "Non hai inserito nessuna targa";
			}
			else
			{
				//l'utente ha inserito la targa: cerco il veicolo
				if (SearchCar(targa))
				{
					this.txtOutput.Text = $"Il veicolo con targa {targa} è già presente nel parcheggio.";
					trovato = true;
				}
				else
				{
					trovato = false;
				}
			}

			if (!trovato)
			{
				//verifico se ci sono posti residui nel parcheggio
				if (FreePlaces(posti))
				{
					//se ci sono posti, inserisco il veicolo
					//Il veicolo non è nel parcheggio: lo inserisco
					InsertCar(targa);
					this.txtOutput.Text = $"Il veicolo con targa {targa} è stato aggiunto alla lista dei veicoli presenti nel parcheggio";
				}
				else
				{
					//se non ci sono posti, avviso l'utente
					this.txtOutput.Text = $"Il parcheggio è pieno";
				}
			}
		}
	}
}

