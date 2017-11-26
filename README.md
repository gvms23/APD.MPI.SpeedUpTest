# APD.MPI.SpeedUpTest

Software de estudo para a disciplina de Arquiteturas Paralelas e Distribuídas da
Faculdade Anhanguera de Taubaté - Un. 2. Professor: Rogério B. Andrade.

![alt text](https://image.prntscr.com/image/Sedw7lPWSaWDlfl2yOuTqQ.png)

# Comparação em desempenho entre loopings: Foreach e Parallel.Foreach.

Utilizada a fórmula: 

    SU(p) = TS / TP(p)

Onde:

    • TS é o tempo de execução da aplicação na versão seqüencial
    
    • TP é o tempo de execução na versão paralela
    
    • p é o número de unidades ativas (processadores) utilizadas
    
Funções:
     
    GetProcessadores()
        Retorna o número de processadores físicos, quantidade de núcleos e processadores lógicos.

    TesteSpeedUp(int limiteLoop, int threadSleep)
        Efetua o teste de SpeedUp comparando Foreach e Parallel.Foreach() que usa
        computação paralela, retornando o cálculo  S(p) = T(1) / T(p).

    TruncarDoisDigitos(double valorParaTruncar, int precisao)
        Trunca com o valor e a precisão em nºs de casas decimais fornecidos.
