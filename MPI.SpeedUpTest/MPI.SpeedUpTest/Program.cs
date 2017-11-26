using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace APD.MPI.SpeedUpTest
{
    class Program
    {
        /// <summary>
        /// Desenvolvimento de Gabriel Vicente Moreira da Silva | RA: 1595885018
        /// Curso de Ciência da Computação da Faculdade Anhanguera de Taubaté - UN 2.
        /// </summary>
        static void Main()
        {
            Console.Title = "APD.MPI.SpeedUpTest | Gabriel Vicente";

            //Analisa os processadores existentes.
            GetProcessadores();

            CheckInt:
            Console.WriteLine("\nDigite a pausa em milissegundos do thread: ");
            string strThread = Console.ReadLine();

            Console.WriteLine("\nDigite um limite para a contagem: ");
            string strLimite = Console.ReadLine();

            if (int.TryParse(strThread, out int tSleep) && int.TryParse(strThread, out int loopLimite))
            {
                Console.WriteLine("Aguarde, efetuando o teste.");
                TesteSpeedUp(loopLimite, tSleep);
            }
            else
            {
                Console.WriteLine("Somente números, por gentileza.");
                goto CheckInt;
            }
        }

        #region Função GetProcessadores()
        /// <summary>
        /// Retorna os tipos de Processadores e suas quantidades
        /// </summary>
        public static void GetProcessadores()
        {
            foreach (var item in new System.Management.ManagementObjectSearcher("Select * from Win32_ComputerSystem").Get())
            {
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"Processadores Físicos: {item["NumberOfProcessors"]}");
            }

            int coreCount = 0;
            foreach (var item in new System.Management.ManagementObjectSearcher("Select * from Win32_Processor").Get())
            {
                coreCount += int.Parse(item["NumberOfCores"].ToString());
            }
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"Quantidade de Núcleos: {coreCount}");

            Console.BackgroundColor = ConsoleColor.DarkCyan;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"Processadores Lógicos: { Environment.ProcessorCount}\n\n");

            Console.ResetColor();

        }
        #endregion

        #region Função TesteSpeedUp()
        /// <summary>
        /// Efetua o teste de SpeedUp comparando For, Foreach e Parallel.Foreach() que usa
        /// computação paralela, retornando o cálculo  S(p) = T(1) / T(p).
        /// </summary>
        /// <param name="threadSleep">Quantidade em Milissegundos para pausa do Thread.</param>
        /// <param name="limiteLoop">Limite do loop for, quantos números serão analisados.</param>
        public static void TesteSpeedUp(int limiteLoop, int threadSleep)
        {
           
            List<int> numerosList = new List<int>();
            for (int i = 0; i < limiteLoop; i++)
            {
                numerosList.Add(i);
            }
            int[] numeros = numerosList.ToArray();


            //-----------------------Loop FOR, não é usado na fórmula, só a nível de estatística -----------//
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < numeros.Length; i++)
            {
                //Console.WriteLine($"{i}, Thread Id= {Thread.CurrentThread.ManagedThreadId}");
                Thread.Sleep(threadSleep);
            }
            var segundosFor = sw.Elapsed.TotalSeconds;
            Console.WriteLine($"FOR Loop: {segundosFor} segundos\n");

            //-----------------------Loop FOREACH sem paralelismo -----------//
            sw = Stopwatch.StartNew();
            foreach (int num in numeros)
            {
                //Console.WriteLine($"{num}, Thread Id= {Thread.CurrentThread.ManagedThreadId}");
                Thread.Sleep(threadSleep);
            }
            var segundosForeach = sw.Elapsed.TotalSeconds;
            Console.WriteLine($"FOREACH Loop: {segundosForeach} segundos\n");

            //-----------------------Loop FOREACH COM paralelismo -----------//
            sw = Stopwatch.StartNew();
            Parallel.ForEach(numeros, num =>
            {
                //Console.WriteLine($"{num}, Thread Id= {Thread.CurrentThread.ManagedThreadId}");
                Thread.Sleep(threadSleep);
            });
            var segundosParallelForeach = sw.Elapsed.TotalSeconds;
            Console.WriteLine($"PARALLEL.FOREACH: {segundosParallelForeach} segundos\n\n");


            var p = Environment.ProcessorCount;
            Console.WriteLine("Cálculo: S(p) = T(1) / T(p)");
            Console.WriteLine("Onde T(1) = é o tempo de execução com um processador");
            Console.WriteLine($"T(p) = é o tempo de execução com p processadores (lógicos): {p}.\n");

            Console.WriteLine($"S(p) = T(1) = {TruncarDoisDigitos(segundosForeach, 3)} / T(p) = {TruncarDoisDigitos(segundosParallelForeach, 3)}");

            var sP = segundosForeach / segundosParallelForeach;
            Console.WriteLine($"\nS(p) = {sP}");

            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine($"\nCom o paralelismo, o desempenho foi melhorado em {TruncarDoisDigitos(sP, 3)} segundos.");

            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\nDesenvolvimento de Gabriel Vicente | RA: 15 95 88 50 18. (;");
            Console.Read();
        }

        #endregion

        #region Função TruncarDoisDigitos()
        /// <summary>
        /// Trunca com o valor e a precisão em nºs de casas decimais fornecidos.
        /// </summary>
        /// <param name="valor">Valor a ser truncado.</param>
        /// <param name="precisao">Qtde. de casas decimais depois da vírgula.</param>
        /// <returns></returns>
        public static double TruncarDoisDigitos(double valorParaTruncar, int precisao)
        {
            double step = Math.Pow(10, precisao);
            int tmp = (int)Math.Truncate(step * valorParaTruncar);
            return tmp / step;
        }
        #endregion

    }
}