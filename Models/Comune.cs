using System.Collections.Generic;
using System.IO;

namespace curzi.lorenzo._4H.SaveRecord.Models
{
    public class Comune
    {
        public int ID { get;  set;}

        public string NomeComune {get; set;}

        public string CodiceCatastale {get; set;}

        public Comune() {}

        public Comune (string s, int id)
        {
            //Creo l'oggetto comune partendo da una riga CSV
            string[] colonne = s.Split(',');

            ID = id;
            NomeComune = colonne[0];
            CodiceCatastale = colonne[1];
        }
    }

    public class Comuni : List<Comune> //Comuni è una List<Comune>
    {
        public Comuni()
        {
            
        }

        public Comuni(string fileName)
        {
            using(FileStream fin = new FileStream(fileName, FileMode.Open))
            {
                StreamReader reader = new StreamReader(fin);

                int id = 1;

                while(!reader.EndOfStream)
                {
                    string riga = reader.ReadLine();

                    Comune c = new Comune(riga, id);

                    //Comuni è una lista di Comune perciò ho ereditato i metodi delle liste
                    this.Add(c);

                    riga = reader.ReadLine();
                    id++;
                }
            }
        }
    }
}