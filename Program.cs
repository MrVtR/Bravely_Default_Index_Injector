using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.Specialized;
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
                if (args.Length < 1)
                {
                    Console.WriteLine("Precisa de pelo menos 1 Argumento");
                    return;
                }
                var isCrowd = args.Length >= 1 && args[0] == "-c";
                var isIndex = args.Length >= 1 && args[0] == "-i";
                if (isCrowd)
                {
                    //Cria crowd.fs novo com Append dos arquivos novos
                    DirectoryInfo di = Directory.CreateDirectory(args[1]+@"\Crowd");
                    string fileName = args[1] + @"\Crowd\crowd.fs";
                    FileStream writeStream = new FileStream(fileName, FileMode.Create);//Criação do arquivo
                    string[] dir = Directory.GetFiles(args[1]);

                    for(int j = 0;j<dir.Length;j++)
                    {
                        byte[] lerArquivo = System.IO.File.ReadAllBytes(dir[j]);//Pega cada arquivo de dirs
                        for (int k = 0; k < lerArquivo.Length; k++)//pega cada byte do arquivo coletado
                            writeStream.WriteByte(lerArquivo[k]); //Coloca os dados dos arquivos novos em um Crowd.fs       
                    }
                    Console.WriteLine("Crowd.fs criado com sucesso");
                    writeStream.Close();
                }
                if (isIndex)
                {
                    string[] dirAntigo = Directory.GetFiles(args[1]);
                    string[] dirNovo = Directory.GetFiles(args[2]);
                    string[] vetTamanhoAntigo = new string[dirAntigo.Length];
                    string[] vetTamanhoNovo = new string[dirNovo.Length];
                    string[] vetCrowdAntigo = new string[dirAntigo.Length];
                    string[] vetCrowdNovo = new string[dirNovo.Length];
                    string fileName = args[3];
                    byte[] byteIndex= ReadAllBytes(args[4]);
                    byte[] byteCrowdAntigo = ReadAllBytes(args[5]);
                    byte[] byteCrowdNovo = ReadAllBytes(args[6]);

                    criarArquivoCopiarVetor("index.fs", fileName, byteIndex);
                    setVetor(dirAntigo, byteCrowdAntigo, vetTamanhoAntigo, vetCrowdAntigo);
                    Console.WriteLine("Tamanhos antigos alocados em vetor: Feito");
                    Console.WriteLine("Criado vetor do crowd.fs antigo");
                    Console.WriteLine("---------------------------------------------");
                    setVetor(dirNovo,byteCrowdNovo,vetTamanhoNovo,vetCrowdNovo);
                    Console.WriteLine("Tamanhos novos alocados em vetor: Feito");
                    Console.WriteLine("Criado vetor do crowd.fs antigo");
                    Console.WriteLine("---------------------------------------------");
                    imprimeIndex(byteIndex, "Original");
                    Console.WriteLine("\n---------------------------------------------\n");
                    reinsereByte(byteIndex, vetTamanhoAntigo, vetTamanhoNovo, fileName);
                    Console.WriteLine("\n---------------------------------------------\n");
                    reinsereByte(byteIndex, vetCrowdAntigo, vetCrowdNovo, fileName);
                    Console.WriteLine("---------------------------------------------");
                    imprimeIndex(byteIndex,"Modificado");//Arrumar
                    Console.WriteLine("\nPrograma finalizado");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public static string LittleEndian(string num)
        {
            int number = Convert.ToInt32(num, 16);
            byte[] bytes = BitConverter.GetBytes(number);
            string retval = "";
            foreach (byte b in bytes)
                retval += b.ToString("X2");
            return retval;
        }
        public static byte[] ReadAllBytes(string fileName)
        {
            byte[] buffer = null;
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                buffer = new byte[fs.Length];
                fs.Read(buffer, 0, (int)fs.Length);
            }
            return buffer;
        }
        public static byte[] HexToByteUsingByteManipulation(string s)
        {
            byte[] bytes = new byte[s.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                int hi = s[i * 2] - 65;
                hi = hi + 10 + ((hi >> 31) & 7);

                int lo = s[i * 2 + 1] - 65;
                lo = lo + 10 + ((lo >> 31) & 7) & 0x0f;

                bytes[i] = (byte)(lo | hi << 4);
            }
            return bytes;
        }
        public static int Search(byte[] src, byte[] pattern)
        {
            int c = src.Length - pattern.Length + 1;
            int j;
            for (int i = 0; i < c; i++)
            {
                if (src[i] != pattern[0]) continue;
                for (j = pattern.Length - 1; j >= 1 && src[i + j] == pattern[j]; j--) ;
                if (j == 0) return i;
            }
            return -1;
        }
        public static byte[] FileToByteArray(string fileName)
        {
            byte[] buff = null;
            FileStream fs = new FileStream(fileName,
                                           FileMode.Open,
                                           FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            long numBytes = new FileInfo(fileName).Length;
            buff = br.ReadBytes((int)numBytes);
            return buff;
        }
        public static void imprimeIndex(byte[] vetorIndex,string print)
        {
            int j = 0;
            Console.WriteLine("Arquivo index.fs "+print);
            foreach (byte by in vetorIndex)//Amostragem do arquivo index.fs modificado,que será depois salvo em um novo arquivo
            {
                if (j == 16)
                {
                    Console.WriteLine("");
                    j = 0;
                }
                if (by >= 0 && by <= 9)
                    Console.Write("0" + by + " ");
                else
                    Console.Write(by + " ");
                j++;
            }
            Console.WriteLine("");
        }
        public static void reinsereByte(byte[] vetorIndex,string[] vetorAntigo,string[] vetorNovo,string fileName)
        {
            byte[] byAntigo;
            byte[] byNovo;

            //Laço para verificar se os bytes de tamanho antigo estão presentes no index.fs,se estiverem, é trocado os bytes antigos do index.fs pelos bytes novos
            for (int k = 0; k < vetorAntigo.Length; k++)
            {
                byAntigo = HexToByteUsingByteManipulation(vetorAntigo[k]);// Converte a string em Hex para Byte,assim,permitindo a manipulação direta pro index.fs
                byNovo = HexToByteUsingByteManipulation(vetorAntigo[k]);// Converte a string em Hex para Byte,assim,permitindo a manipulação direta pro index.fs
                int varSearch = Search(vetorIndex, byAntigo);//Função para verificar se o Pattern btAntigo está presente em b,retorna a posição i de onde o Pattern começa ou -1 se não for achado   
                if (varSearch >= 0)//Se houver o Pattern de btAntigo em b,é trocado os bytes antigos pelos bytes novo
                {
                    for (int l = 0; l < byAntigo.Length; l++)
                        using (var stream = File.Open(fileName, FileMode.Open))
                        {
                            stream.Position = varSearch + l;
                            stream.WriteByte(byNovo[l]);//Trocar para 0x99 se for debugar ou para byNovo[l] se for para fazer o programa funcionar normalmente
                            stream.Close();
                        }
                    Console.Write("Inseriu: ");
                }
                else
                    Console.Write("NÃO ACHOU ");
                Console.WriteLine(vetorAntigo[k] + " para " + vetorNovo[k]);
            }
        }
        public static void setVetor(string[] path,byte[] byteVetor,string[] vetorTamanho,string[] vetorCrowd)
        {
            int i = 0;
            foreach (string dir in path)// Verifica o tamanho dos arquivos novo e coloca a informação em um vetor
            {
                using (BinaryReader br = new BinaryReader(File.Open(dir, FileMode.Open)))
                {
                    string hexValue = br.BaseStream.Length.ToString("X");
                    vetorTamanho[i] = LittleEndian(hexValue);//Converte a string hex para little endian
                    br.Close();
                }
                byte[] byteDir = FileToByteArray(dir);
                vetorCrowd[i] = LittleEndian((Search(byteVetor, byteDir)).ToString("X")); // Vetor de String do Crowd antigo
                i++;
            }
        }
        public static void criarArquivoCopiarVetor(string nomeArquivo,string fileName,byte[] byteVetor)
        {
            BinaryWriter indexFile;
            Console.WriteLine("\n\n-------------------Criação do novo "+nomeArquivo+"-------------------");
            FileStream writeStream = new FileStream(fileName, FileMode.Create);//Criação do arquivo
            indexFile = new BinaryWriter(writeStream, Encoding.Unicode);
            indexFile.Write(byteVetor);
            indexFile.Close();
        }
    }
}


