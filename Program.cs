using System.Text;
using System.IO;
using System;
using curzi.lorenzo._4H.SaveRecord.Models;

namespace curzi.lorenzo._4H.SaveRecord
{
    class Program
    {
        static void Main(string[] args)
        {
            const int NUMERO_RECORD = 5;

            Console.WriteLine("Programma SaveRecord di Lorenzo Curzi 4H, 28/04/2021");

            // 1) Leggere un file CSV con i comuni e trasformarlo in una lista di Comune
            Comuni c = new Comuni("Comuni.csv");
            Console.WriteLine($"Ho letto {c.Count} righe");
            Console.WriteLine($"\nLe informazioni della riga {NUMERO_RECORD}: {c[NUMERO_RECORD]}");
            
            // 2) Scrivere la List<Comune> in un file binario
            Console.WriteLine(c.Save());
            
            // 3) Rileggere file binario in una List<Comune>
            Console.WriteLine(c.Load());
            Console.WriteLine($"\nHo letto {c.Count} dal file binario");
            Console.WriteLine($"\nLe informazioni della riga {NUMERO_RECORD}: {c[NUMERO_RECORD]}");

            Console.WriteLine("\nInserire un numero: ");
            string strNumero = Console.ReadLine();
            Comune cercato = c.RicercaComune(Convert.ToInt32(strNumero));
            Console.WriteLine(cercato.NomeComune);
        }
    }
}
