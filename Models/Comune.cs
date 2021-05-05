using System.Xml;
using System.Globalization;
using System.Collections.Generic;
using System.IO;

namespace curzi.lorenzo._4H.SaveRecord.Models
{
    public class Comune
    {
        public int ID { get;  set;}

        private string _nomeComune;
        public string NomeComune 
        {
            get => _nomeComune;

            set
            {
                /*
                Lunghezza Record  = 32
                Id = 4 byte
                CodiceCatastale = 4 + 1
                NomeComune = 22 + 1 
                */
                if(value.Length < 22)    
                    value = value.PadRight(22);
                else if(value.Length > 22)    
                    value = value.Substring(0,22);
                
                _nomeComune = value;
            }
        }

        private string _codicecatastale;
        public string CodiceCatastale 
        {
            get => _codicecatastale;
            
            set
            {
                if(value.Length == 4)
                    _codicecatastale = value;

                if(value.Length < 4)    
                    value = value.PadRight(4);
                else if(value.Length > 4)    
                    value = value.Substring(0,4);
                
                _codicecatastale = value;
            }
        }

        public Comune() {}

        public Comune (string s, int id)
        {
            //Creo l'oggetto comune partendo da una riga CSV
            string[] colonne = s.Split(',');

            ID = id;
            NomeComune = colonne[1];
            CodiceCatastale = colonne[0];
        }

         public override string ToString()
        {
            return $"{ID}, {NomeComune}, {CodiceCatastale}";
        }
    }

    public class Comuni : List<Comune> //Comuni è una List<Comune>
    {
        public string NomeFile {get;}

        public Comuni()
        {
            
        }

        //costruttore che carica una lista di comuni caricandoli da file
        public Comuni(string fileName)
        {
            using(FileStream fin = new FileStream(fileName, FileMode.Open))
            {
                NomeFile = fileName; //Mi salvo il nome del file per i prossimi metodi

                StreamReader reader = new StreamReader(fin);

                int id = 1;

                while(!reader.EndOfStream)
                {
                    //leggo la prima riga 
                    string riga = reader.ReadLine();

                    //e creo l'istanza di COmune
                    Comune c = new Comune(riga, id);

                    //Comuni è una lista di Comune perciò ho ereditato i metodi delle liste
                    this.Add(c);
                    id++;
                }
            }
        }

        //Metodo che consente di convertire la lista di Comuni in un file binario
        //Quando viene richiamato crea il nome del file basandosi sulla Property NomeFile
        //e poi richiama il Metodo Save(fileName)
        public string Save()
        {
            string fileName = NomeFile.Split(".")[0] + ".bin";
            return Save(fileName);;
        }

        //metodo che crea un file binario e vi salva al suo interno le informazioni del comuni
        public string Save(string fileName)
        {
            string retVal = "Operazione di salvataggio riuscita!";

            try
            {
                FileStream fout = new FileStream(fileName, FileMode.Create);
                BinaryWriter writer = new BinaryWriter (fout);

                foreach (Comune comune in this)
                {
                    writer.Write(comune.ID);
                    writer.Write(comune.CodiceCatastale); 
                    writer.Write(comune.NomeComune);

                    //Riepimento del file binario con un record di 32 bit
                    // fout.Seek((comune.ID) * 32, SeekOrigin.Begin);
                    // writer.Write(comune.ID);
                    // writer.Write(comune.NomeComune);
                    // writer.Write(comune.CodiceCatastale);
                }

                writer.Flush();
                writer.Close();
            }
            catch   
            {
                retVal = "Operazione di salvataggio non riuscita!";
            }

            return retVal;
        }

        //Metodo che consente di caricare una lista di comuni partendo da un file binario
        //Se richiamato compone il nome del file e poi richiama il metodo Load(fileName)
        public string Load ()
        {
            string fileName = NomeFile.Split(".")[0] + ".bin"; 
            return Load(fileName);;
        }

        //Metodo che partendo da un file binario carico all'interno della classe le informazioni di più comuni
        public string Load(string fileName)
        {
            string retVal = "Operazione di ripristino riuscita!";

            this.Clear(); //pulisco la lista interna della classe

            try
            {
                //FileStream impostato a fileMode.Open in modo da poter aprire un file già presente nella cartella
                //se fosse FileMode.Create eliminerebbe il precdente file con il conseguente errore di lettura
                FileStream streamReader = new FileStream(fileName, FileMode.Open);
                BinaryReader reader = new BinaryReader(streamReader);

                Comune c;

                while (reader.BaseStream.Position != reader.BaseStream.Length)
                {
                    c = new Comune();

                    //Leggo l'ID
                    c.ID = reader.ReadInt32();

                    //Leggo il codice catastale
                    c.CodiceCatastale = reader.ReadString();

                    //Leggo il nome del comune
                    c.NomeComune = reader.ReadString();

                    this.Add(c); //aggiungo un record all'interno della lista
                }
            }
            catch
            {
                retVal = "Operazione di ripristino non riuscita! Assicurarsi di star utilizzo il file giusto.";
            }

            return retVal;
        }

        public Comune RicercaComune (int numero)
        {
            FileStream streamReader = new FileStream("Comuni.bin", FileMode.Open);
            BinaryReader reader = new BinaryReader(streamReader);

            streamReader.Seek((numero - 1) * 32, SeekOrigin.Begin);
            Comune c = new Comune();
            c.ID = reader.ReadInt32();
            c.CodiceCatastale = reader.ReadString();
            c.NomeComune = reader.ReadString();

            return c;
        }
    }
}