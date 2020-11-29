using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Teste
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
        }

        private void BtnCrowd_Click(object sender, EventArgs e)
        {
            try
            {
                folderBrowserDialog1.Description = "Selecione o diretório com o Crowd.fs, index.fs e arquivos antigos extraídos: ";
                DialogResult dr = folderBrowserDialog1.ShowDialog();
                string path = folderBrowserDialog1.SelectedPath.ToString();
                string crowdFsCheck = path + @"\antigo\Crowd\crowd.fs";
                string indexFsCheck = path + @"\antigo\Crowd\index.fs";
                string[] dir = Directory.GetFiles(path + @"\novo");

                if (File.Exists(crowdFsCheck) && File.Exists(indexFsCheck) && dir.Length>0)
                {
                    DirectoryInfo di = Directory.CreateDirectory(path + @"\novo\Crowd");
                    //Cria crowd.fs novo com Append dos arquivos novos
                    string fileName = path + @"\novo\Crowd\crowd.fs";
                    FileStream writeStream = new FileStream(fileName, FileMode.Create);//Criação do arquivo
                    
                    for (int j = 0; j < dir.Length; j++)
                    {
                        byte[] lerArquivo = File.ReadAllBytes(dir[j]);//Pega cada arquivo de dirs
                        for (int k = 0; k < lerArquivo.Length; k++)//pega cada byte do arquivo coletado
                            writeStream.WriteByte(lerArquivo[k]); //Coloca os dados dos arquivos novos em um Crowd.fs   
                        Thread.Sleep(20);
                        Console.WriteLine("Inseriu: " + Path.GetFileName(dir[j]));
                    }
                    Console.WriteLine("\nPrograma finalizado");
                    MessageBox.Show("Crowd.fs criado com sucesso","Concluído");
                    writeStream.Close();
                }
                else
                    MessageBox.Show("Arquivos necessários não encontrados","ERRO");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"ERRO");
            }
        }

        private void BtnIndex_Click(object sender, EventArgs e)
        {
            try
            {
                folderBrowserDialog1.Description = "Selecione o diretório com o Crowd.fs, index.fs e arquivos antigos extraídos: ";
                DialogResult dr = folderBrowserDialog1.ShowDialog();
                string path = folderBrowserDialog1.SelectedPath.ToString();

                string crowdFsAntigoCheck = path + @"\antigo\Crowd\crowd.fs";
                string crowdFsNovoCheck = path + @"\novo\Crowd\crowd.fs";
                string indexFsAntigoCheck = path + @"\antigo\Crowd\index.fs";
                string indexFsNovo = path + @"\novo\Crowd\index.fs";

                string[] dirAntigo = Directory.GetFiles(path + @"\antigo");//args[1]
                string[] dirNovo = Directory.GetFiles(path + @"\novo");//agrs[2]

                if (File.Exists(crowdFsAntigoCheck) && File.Exists(crowdFsNovoCheck) && File.Exists(indexFsAntigoCheck) && dirAntigo.Length > 0 && dirNovo.Length > 0)
                {
                    string[] vetTamanhoAntigo = new string[dirAntigo.Length];
                    string[] vetTamanhoNovo = new string[dirNovo.Length];
                    string[] vetCrowdAntigo = new string[dirAntigo.Length];
                    string[] vetCrowdNovo = new string[dirNovo.Length];
                    string fileName = indexFsNovo;//args[3]
                    byte[] byteIndex = ReadAllBytes(indexFsAntigoCheck);//args[4] 
                    byte[] byteCrowdAntigo = ReadAllBytes(crowdFsAntigoCheck);//args[5] 
                    byte[] byteCrowdNovo = ReadAllBytes(crowdFsNovoCheck);//args[6] 

                    criarArquivoCopiarVetor("index.fs", fileName, byteIndex);
                    setVetor(dirAntigo, byteCrowdAntigo, vetTamanhoAntigo, vetCrowdAntigo);
                    Console.WriteLine("Tamanhos antigos alocados em vetor: Feito");
                    Console.WriteLine("Criado vetor do crowd.fs antigo");
                    Console.WriteLine("---------------------------------------------");
                    setVetor(dirNovo, byteCrowdNovo, vetTamanhoNovo, vetCrowdNovo);
                    Console.WriteLine("Tamanhos novos alocados em vetor: Feito");
                    Console.WriteLine("Criado vetor do crowd.fs antigo");
                    Console.WriteLine("---------------------------------------------");
                    Thread.Sleep(200); //one second
                    imprimeIndex(byteIndex, "Original");
                    Thread.Sleep(200); //one second
                    Console.WriteLine("\n---------------------------------------------\n");
                    reinsereByte(byteIndex, vetTamanhoAntigo, vetTamanhoNovo, fileName);
                    Console.WriteLine("\n---------------------------------------------\n");
                    reinsereByte(byteIndex, vetCrowdAntigo, vetCrowdNovo, fileName);
                    Console.WriteLine("---------------------------------------------");
                    Thread.Sleep(200); //one second
                    byteIndex = ReadAllBytes(indexFsNovo);//args[4] 
                    imprimeIndex(byteIndex, "Modificado");//Arrumar
                    Console.WriteLine("\nPrograma finalizado");
                    MessageBox.Show("Index.fs criado com sucesso", "Concluído");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message,"ERRO");
            }
        }

        private void contatoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo("https://github.com/MrVtR");
            Process.Start(sInfo);
        }

        private void repositórioDoCódigoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo("https://github.com/MrVtR/Bravely_Default_Index_Injector");
            Process.Start(sInfo);
        }

        private void comoUtilizarAEdiçãoDoCrowdfsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Tutorial:\n\n"+
                "Escolha a pasta em que os arquivos extraídos e editados do Crowd.fs estejam com o seguinte caminho:\n\n" +

                "\"Nome da pasta escolhida\"\\novo\\arquivos a serem compactados\n\n" +

                "Será criado uma pasta \"Crowd\" com um novo arquivo crowd.fs, tendo um Append de todos os arquivos que estiverem na pasta escolhida\n\n" +
                "No console de debug exibido, você poderá conferir quais arquivos foram inseridos, para verificar se tudo saiu como planejado", 
                "Tutorial de uso da edição do Crowd.fs\n",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void comoUtilizarAEdiçãoDoIndexfsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Tutorial:\n\n" +
                "Escolha a pasta em que exista os seguintes arquivos nos caminhos determinados abaixo:\n\n" +
                "Crowd.fs, index.fs e arquivos extraídos do Crowd.fs Originais:\n" +
                "\"Nome da pasta escolhida\"\\antigo\\Crowd\\crowd.fs\n" +
                "\"Nome da pasta escolhida\"\\antigo\\Crowd\\index.fs\n" +
                "\"Nome da pasta escolhida\"\\antigo\\arquivos extraídos do crowd.fs\n\n" +

                "Crowd.fs e arquivos extraídos do Crowd.fs Modificados:\n" +
                "\"Nome da pasta escolhida\"\\novo\\Crowd\\crowd.fs\n" +
                "\"Nome da pasta escolhida\"\\novo\\arquivos extraídos do crowd.fs\n\n\n" +

                "Será criado uma cópia do arquivo Antigo index.fs e alterado os bytes que representam as seguintes operações:\n\n" +

                "Tamanho do arquivo extraído do crowd.fs original -> Tamanho do arquivo extraído do crowd.fs modificado\n\n" +

                "Em qual local o arquivo extraído se inicia no crowd.fs original -> Em qual local o arquivo extraído se inicia no crowd.fs modificado\n\n" +
                "No console de debug, você poderá conferir quais os bytes que foram trocados, o vetor de bytes do index.fs original e o vetor de bytes do novo index.fs para verificar se tudo saiu como planejado",

                "Tutorial de uso da edição do Index.fs",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Funções para o Index.fs
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
        public static void imprimeIndex(byte[] vetorIndex, string print)
        {
            int j = 0;
            Console.WriteLine("Arquivo index.fs " + print);
            foreach (byte by in vetorIndex)//Amostragem do arquivo index.fs modificado,que será depois salvo em um novo arquivo
            {
                if (j == 16)
                {
                    Console.WriteLine("");
                    Thread.Sleep(20);
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
        public static void reinsereByte(byte[] vetorIndex, string[] vetorAntigo, string[] vetorNovo, string fileName)
        {
            byte[] byAntigo;
            byte[] byNovo;

            //Laço para verificar se os bytes de tamanho antigo estão presentes no index.fs,se estiverem, é trocado os bytes antigos do index.fs pelos bytes novos
            for (int k = 0; k < vetorAntigo.Length; k++)
            {
                byAntigo = HexToByteUsingByteManipulation(vetorAntigo[k]);// Converte a string em Hex para Byte,assim,permitindo a manipulação direta pro index.fs
                byNovo = HexToByteUsingByteManipulation(vetorNovo[k]);// Converte a string em Hex para Byte,assim,permitindo a manipulação direta pro index.fs
                int varSearch = Search(vetorIndex, byAntigo);//Função para verificar se o Pattern btAntigo está presente em b,retorna a posição i de onde o Pattern começa ou -1 se não for achado   
                if (varSearch >= 0)//Se houver o Pattern de btAntigo em b,é trocado os bytes antigos pelos bytes novo
                {
                    for (int l = 0; l < byAntigo.Length; l++)
                        using (var stream = File.Open(fileName, FileMode.Open))
                        {
                            stream.Position = varSearch + l;
                            stream.WriteByte(byNovo[l]);//Trocar para 99 se for debugar ou para byNovo[l] se for para fazer o programa funcionar normalmente
                            stream.Close();
                        }
                    Console.Write("Inseriu: ");
                    Thread.Sleep(20);
                }
                else
                    Console.Write("NÃO ACHOU ");
                Console.WriteLine(vetorAntigo[k] + " para " + vetorNovo[k]);
                Thread.Sleep(20);
            }
        }
        public static void setVetor(string[] path, byte[] byteVetor, string[] vetorTamanho, string[] vetorCrowd)
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
        public static void criarArquivoCopiarVetor(string nomeArquivo, string fileName, byte[] byteVetor)
        {
            BinaryWriter indexFile;
            Console.WriteLine("\n\n-------------------Criação do novo " + nomeArquivo + "-------------------");
            FileStream writeStream = new FileStream(fileName, FileMode.Create);//Criação do arquivo
            indexFile = new BinaryWriter(writeStream, Encoding.Unicode);
            indexFile.Write(byteVetor);
            indexFile.Close();
            writeStream.Close();
        }
    }

}
