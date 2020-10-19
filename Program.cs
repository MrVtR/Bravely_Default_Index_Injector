using System;
using System.Collections.Immutable;
using System.IO;
using System.IO.Enumeration;
using System.Linq;
using System.Text;

namespace BraveIndexInject
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                int i = 1;
                if (args.Length < 1)
                {
                    Console.WriteLine("Precisa de pelo menos 1 Argumento");
                    return;
                }
                var isIndex = args.Length > 1 && args[0] == "-i";
                var isSwap = args.Length > 1 && args[0] == "-s";
                
                if(isIndex)
                {
                    //Saber o tamanho de cada arquivo original 
                    string[] dirs = Directory.GetFiles(args[1]);
                    foreach (string dir in dirs)
                        tamanhoArquivo(dir);

                    Console.WriteLine("---------------------------------------------");

                    //Criar crowd.fs novo com Append dos arquivos novos
                    DirectoryInfo di = Directory.CreateDirectory(args[2]+@"\Crowd");
                    FileStream writeStream;
                    string fileName;
                   
                    fileName = args[2] + @"\Crowd\crowd.fs";
                    writeStream = new FileStream(fileName, FileMode.Create);//Criação do arquivo

                    dirs = Directory.GetFiles(args[2]);
                    int pos = 0;
                    for(int j = 0;j<dirs.Length;j++)
                    {

                        using (BinaryReader br = new BinaryReader(File.Open(dirs[j], FileMode.Open)))
                        {
                            int length = (int)br.BaseStream.Length;//Pegar tamanho do arquivo
                            Console.WriteLine("Onde arquivo começa é: " + Path.GetFileName(dirs[j]) + " é: " + pos.ToString("X"));
                            pos += length;
                        }

                        byte[] lerArquivo = System.IO.File.ReadAllBytes(dirs[j]);
                        for (int k = 0; k < lerArquivo.Length; k++)
                            writeStream.WriteByte(lerArquivo[k]); //Coloca os dados dos arquivos novos em um Crowd.fs       
                    }
                    writeStream.Close();

                }
                if (isSwap)
                {
                    string[] dirs = Directory.GetFiles(args[1]);
                    foreach (string dir in dirs)
                    {
                       string aux = Path.GetFileName(dir);
                        if (aux == "index.fs")
                        {
                            byte[] lerArquivo = System.IO.File.ReadAllBytes(dir);
                            int cont = 0;
                            for (int k = 0; k < lerArquivo.Length; k++)
                            {
                                Console.Write(lerArquivo[k].ToString("X"));

                            }
                                    
                        }

                    }
                       



                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public static void tamanhoArquivo(string dir)
        {
            using (BinaryReader br = new BinaryReader(File.Open(dir, FileMode.Open)))
            {
                int length = (int)br.BaseStream.Length;//Pegar tamanho do arquivo
                string hexValue = length.ToString("X");


                Console.WriteLine("Tamanho do arquivo " + Path.GetFileName(dir) + " é: " + hexValue);
            }
        }
        public static byte[] FileToByteArray(string fileName)
        {
            return File.ReadAllBytes(fileName);
        }
    }
}


