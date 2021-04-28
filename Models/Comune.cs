using System.Xml;
using System.Globalization;
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
            string[] colonne = s.Split(",");

            ID = id;
            NomeComune = colonne[0];
            CodiceCatastale = colonne[1];
        }
    }

    public class Comuni : List<Comune> //Comuni è una List<Comune>
    {
        public string NomeFile {get;}

        public Comuni()
        {
            
        }

        public Comuni(string fileName)
        {
            using(FileStream fin = new FileStream(fileName, FileMode.Open))
            {
                NomeFile = fileName;

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

        public void Save()
        {
            string fileName = NomeFile.Split(".")[0] + ".bin";
            Save(fileName);
        }

        public void Save(string fileName)
        {
            FileStream fout = new FileStream(fileName, FileMode.Create);
            BinaryWriter writer = new BinaryWriter (fout);

            foreach (Comune comune in this)
            {
                writer.Write(comune.ID);
                writer.Write(comune.CodiceCatastale); 
                writer.Write(comune.NomeComune);
            }

            writer.Flush();
            writer.Close();
        }

        public void Load ()
        {
            string fileName = NomeFile.Split(".")[0] + ".bin";
            Load(fileName);
        }

        public void Load(string fileName)
        {
            this.Clear();

            FileStream streamReader = new FileStream(fileName, FileMode.Create);
            BinaryReader reader = new BinaryReader(streamReader);

            Comune c = new Comune();
            //Leggo l'ID
            c.ID = reader.ReadInt32();

            //Leggo il codice catastale
            c.CodiceCatastale = reader.ReadString();
            
            //Leggo il nome del comune
            c.NomeComune = reader.ReadString();

            this.Add(c);

            //Manca un while che legge tutte le righe
            //Come si fa ad accorgersi della fine del file...?
        }
    }
}