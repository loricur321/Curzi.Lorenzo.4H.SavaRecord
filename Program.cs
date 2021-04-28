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
            Console.WriteLine("Programma SaveRecord di Lorenzo Curzi 4H, 28/04/2021");

            // 1) Leggere un file CSV con i comuni e trasformarlo in una lista di Comune
            Comuni c = new Comuni("Comuni.csv");
            Console.WriteLine($"Ho letto {c.Count} righe");
            
            // 2) Scrivere la List<Comune> in un file binario
            c.Save();
            
            // 3) Rileggere file binario in una List<Comune>
            c.Load();
            Console.WriteLine($"Ho letto {c.Count} dal file binario");
            
        }
    }
}
