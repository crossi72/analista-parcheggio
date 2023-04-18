using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Parcheggio
{
    internal class Program
    {
        static void Main(string[] args)
        {
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
                    Console.Write("Inserire targa veicolo: ");
                    string targa = Console.ReadLine().ToLower();

                    bool trovato = false;
                    foreach (Veicolo veicolo in veicoli)
                    {
                        
                        if (veicolo.targa != null && veicolo.targa.ToLower() == targa)
                        {
                            Console.WriteLine($"Il veicolo con targa {veicolo.targa} è già presente nel parcheggio.");
                            trovato = true;
                            break;
                        }
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
    }
} 