// See https://aka.ms/new-console-template for more information


/*
 1) Observe o trecho de código abaixo: int INDICE = 13, SOMA = 0, K = 0;
Enquanto K < INDICE faça { K = K + 1; SOMA = SOMA + K; }
Imprimir(SOMA);
Ao final do processamento, qual será o valor da variável SOMA?
 */
using static System.Runtime.InteropServices.JavaScript.JSType;

int verification = -1;
while (verification != 0)
{
    Console.WriteLine("Digite a questão para ver o resultado obtido:");
    string option = Console.ReadLine();
    if (int.TryParse(option, out verification))
    {
        switch (verification)
        {
            case 0: Console.WriteLine("Obrigado! Programa encerrado");
                break;
            case 1:
                PrintSoma();
                break;
            case 2:
                VerifyNumber();
                break;
            default:
                Console.WriteLine("opção inválida");
                break;
        }
    }
    else 
    {
        Console.WriteLine("opção inválida");
    }



}

void PrintSoma ()
{
    int INDICE = 13;
    int SOMA = 0;
    int K = 0;
    while (K < INDICE)
    {
        K += 1;
        SOMA = SOMA + K;

    }
    Console.WriteLine("O valor de SOMA é igual a " + SOMA);


}



/*
 2) Dado a sequência de Fibonacci, onde se inicia por 0 e 1 e o próximo valor sempre será a soma dos 2 valores anteriores
(exemplo: 0, 1, 1, 2, 3, 5, 8, 13, 21, 34...), escreva um programa na linguagem que desejar onde, informado um número,
ele calcule a sequência de Fibonacci e retorne uma mensagem avisando se o número informado pertence ou não a sequência.
 */

void VerifyNumber()
{
    Console.Write("coloque um numero pra ver se pertence à Fibonacci: ");
    if (int.TryParse(Console.ReadLine(), out int number))
    {
        if (IsFibonacci(number))
            Console.WriteLine($"{number} tá na fibonacci.");
        else
            Console.WriteLine($"{number} não tá na fibonacci");
    }
    else
    {
        Console.WriteLine("Invalido.");
    }
}

static bool IsFibonacci(int n)
{
    int prev = 0, current = 1, temp;
    while (prev < n)
    {
        temp = prev;
        prev = current;
        current  = temp + current;
    }
    return prev == n;
}




string ReverseString (string s)
{
    int l = s.Length;
    string newString = string.Empty;
    if (l == 0) return s;
    else
    {
        for (int i = l - 1; i >= 0; i--)  
        {
            newString += s[i];  // recebe primeiro o último elemento do comprimento da string s e vai diminuindo
        }
    }
    return newString;
}