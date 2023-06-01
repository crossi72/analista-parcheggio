using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
	public partial class Form1 : Form
	{

		#region " proprietà private "

		//proprietà per la gestione dei dati statici
		private int postiTotali;
		private float costoOrario;

		#endregion

		#region " gestori di evento "

		private void button1_Click(object sender, EventArgs e)
		{
			this.txtInput.Text = "hello world!!!";
		}

		private void btnArrivoVeicolo_Click(object sender, EventArgs e)
		{
			this.ArrivoVeicoli();
		}

		private void btnStampaVeicoli_Click(object sender, EventArgs e)
		{
			this.StampaVeicoli();
		}

		private void btnUscitaVeicolo_Click(object sender, EventArgs e)
		{
			this.UscitaVeicolo();
		}

		#endregion

		public Form1() {
			InitializeComponent();

			//valorizzo le proprietà private
			InitProperties();
		}

		private void InitProperties(){
			DataTable parkTable;

			//leggo i dati dal database
			parkTable = GetDataFromDB("parcheggio");

			//assegno i valori alle propreità
			this.postiTotali = Convert.ToInt32(parkTable.Rows[0]["posti"]);
			this.costoOrario = Convert.ToSingle(parkTable.Rows[0]["costo_orario"]);
		}

		/// <summary>
		/// Create a connection to database
		/// </summary>
		/// <returns></returns>
		public SqlConnection GetDBConnection()
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

		private void ExecuteQuery(string query)
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
		private void InsertCar(string targa)
		{
			ExecuteQuery($"INSERT INTO veicoli (targa, ingresso) VALUES ('{targa}', GETDATE())");
		}

		/// <summary>
		/// Toglie un veicolo nel parcheggio
		/// </summary>
		/// <param name="targa">targa del veicolo da togliere</param>
		private void RemoveCar(string targa)
		{
			ExecuteQuery($"UPDATE veicoli SET uscita = GETDATE() WHERE targa = '{targa}' AND uscita IS NULL");
		}

		/// <summary>
		/// Loads data from database
		/// </summary>
		/// <param name="tableName">Name of the table</param>
		/// <returns>Datatable containing data from <paramref name="tableName"/>table</returns>
		private DataTable GetDataFromDB(string tableName)
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
		private DataTable GetCarsIntoParking()
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
		public bool FreePlaces(int postiTotali)
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
		private void UscitaVeicolo()
		{
			//chiedo all'utente la targa del veicolo
			string targa = this.txtInput.Text;
			DataTable carData;
			double hours, cost;

			//cerco nel database il veicolo con la targa indicata
			carData = CarEntryTime(targa);

			if (carData.Rows.Count == 0)
			{
				//il veicolo non è nel parcheggio
				this.txtOutput.Text="Nessun veicolo trovato.";
			}
			else
			{
				//il veicolo è nel parcheggio, calcolo il costo
				//calcolo quante ore (con i decimali) è rimasto nel parcheggio
				hours = Convert.ToSingle(carData.Rows[0]["durata"]) / 60.0;
				//arrotondo le ore all'intero successivo
				hours = Math.Ceiling(hours);
				//calcolo il costo totale
				cost = hours * this.costoOrario;

				this.txtOutput.Text = $"Il veicolo con targa {targa} esce il {DateTime.Now.Date} alle ore {DateTime.Now.TimeOfDay} e deve pagare {cost} euro";

				//tolgo il veicolo dal parcheggio
				this.RemoveCar(targa);
			}
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
		private void ArrivoVeicoli()
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
				if (FreePlaces(this.postiTotali))
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

