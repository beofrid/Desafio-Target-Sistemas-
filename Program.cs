using System.Globalization;
using System.Text.Json;
using System.Xml;

int verification = -1;
string line = "\n \n--------------------------------------------------------------------------------- \n \n";
while (verification != 0)
{
    Console.WriteLine(
        "\n \n Digite o número questão para ver o resultado obtido: " +
        "\n \n \t 1 - Ver o resultado do trecho de código:" +
        "\n \t \t int INDICE = 13, SOMA = 0, K = 0; \n \t \t Enquanto K < INDICE faça { K = K + 1; SOMA = SOMA + K; } " +
        "\n \n \t 2 - Verificar se um número está na Fibonacci" +
        "\n \n \t 3 - Exibir o menor e o maior valor de faturamento em um dia e o número de dias em que o valor de faturamento foi superior à média mensal." +
        "\n \n \t 4 - Calcular o percentual de representação que cada estado teve dentro do valor total mensal da distribuidora" +
        "\n \n \t 5 - Inverter uma string" +
        "\n \n \t 6 - Limpar console" +
        "\n \n \t 0 - Encerrar o programa");
    var option = Console.ReadLine();
    if (int.TryParse(option, out verification))
    {
        switch (verification)
        {
            case 0: Console.WriteLine(line + "Obrigado! Programa encerrado");
                break;
            case 1:
                Console.Clear();
                PrintSoma();
                break;
            case 2:
                Console.Clear();
                VerifyNumber();
                break;
            case 3:
                Console.Clear();
                Console.WriteLine("Para a atividade foram disponibilizados 2 documentos. Digite \"1\" para ler o XML e \"2\" para ler o JSON.");
                var stringOption = Console.ReadLine();
                int parsedOption;
                if(int.TryParse(stringOption, out parsedOption))
                {
                    switch (parsedOption)
                    {
                        case 1:
                            DailyRevenueXML();
                            break;
                        case 2:
                            DailyRevenueJSON();
                            break;
                        default:
                            Console.WriteLine("Opção inválida");
                            break;
                    }

                }
                else
               { Console.WriteLine("não parseou"); }


                break;
            case 4:
                Console.Clear();
                StateShare();
                break;
            case 5:
                Console.Clear();
                ReverseString();
                break;
            case 6:
                Console.Clear();
                break;

            default:
                Console.WriteLine("Opção inválida");
                break;
        }
    }
    else 
    {
        verification = 8;
        Console.Clear();
        Console.WriteLine(line +"Opção inválida!" + line);
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
        SOMA += K;

    }
    Console.WriteLine(line + "O valor de SOMA é igual a " + SOMA + line);


}

#region Fibonacci
void VerifyNumber()
{
    Console.Write("\n Coloque um numero pra ver se pertence à Fibonacci: ");
    if (int.TryParse(Console.ReadLine(), out int number))
    {
        if (IsFibonacci(number))
        {
   
            Console.WriteLine(line +$"{number} está na fibonacci." + line);
  

        }

        else
        {
           
            Console.WriteLine(line + $"{number} não está na fibonacci" + line);
       
        }
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
#endregion

#region leitura do xml

void PrintFileResult (List<double> values, string docType)
{

    double minValue = FindMinValue(values);
    double maxValue = FindMaxValue(values);
    Console.WriteLine(line + "De acordo com o " + docType + " recebido: \n");

    Console.WriteLine
        (
            "\n \t --> O menor valor registrado em um dia com faturamento foi:\n \t \t --> "
            + minValue + "\n \t \t --> Em moeda brasileira fica " + string.Format(new CultureInfo("pt-BR"), "{0:C}", minValue)
            + "\n \t \t --> Esse faturamento ocorreu no " + (values.IndexOf(minValue) + 1) + "º dia da lista"
        );
    Console.WriteLine
        (
            "\n \t --> O maior valor registrado em um dia com faturamento foi: \n \t \t --> "
            + maxValue + "\n \t \t --> Em moeda brasileira fica " + string.Format(new CultureInfo("pt-BR"), "{0:C}", maxValue)
            + "\n \t \t --> Esse faturamento ocorreu no " + (values.IndexOf(maxValue) + 1) + "º dia da lista"

        );
    Console.WriteLine("\n\t --> O faturamento passou a média no período em " + FindAboveAverage(values) + " dias." + line);

}

void DailyRevenueXML()
{
    string XMLpath = Path.Combine(AppContext.BaseDirectory, "dados.xml"); //usei caminho relativo para funcionar em outras máquinas
    List<double> values = new List<double>();
    string verifyElement = "";
    double value;

    using (XmlTextReader reader = new XmlTextReader(XMLpath))
    {
        while (reader.Read())
        {
            if(reader.NodeType == XmlNodeType.Element)
            {
                verifyElement = reader.Name;
            }
            if (reader.NodeType == XmlNodeType.Text)
            {
                if (verifyElement == "valor")
                {
                    //Console.WriteLine(reader.Value);
                    value = double.Parse(reader.Value, CultureInfo.InvariantCulture); // o segundo parâmetro foi adicionado pois o parse estava desconsiderando o "." e transformando em um número inteiro
                    values.Add(value);
                }
            }
        }
    PrintFileResult(values, "XML");
    };



}


void DailyRevenueJSON()
{
    string JSONpath = Path.Combine(AppContext.BaseDirectory, "dados.json");
    List<double> values = new List<double>();

    try
    {
        string jsonContent = File.ReadAllText(JSONpath);
        Revenue[] revenues = JsonSerializer.Deserialize<Revenue[]>(jsonContent);
        Console.WriteLine("Valores carregados:");
        int index = 1;
        foreach (var revenue in revenues)
        {
            values.Add(revenue.valor);
           
        }


    }
    catch (Exception ex)
    {
        Console.WriteLine("Erro: " + ex.Message);
    }

    PrintFileResult(values, "JSON");

}

double FindAverage(List<double> values)
{
    double sum = 0, average;
    int listLenght = values.Count;
    int dayWithoutRevenue = 0;
    for (var i = 0; i < listLenght; i++)
    {
        if (values[i] == 0) dayWithoutRevenue++;
        sum += values[i];

    }
    // aqui calcula a média com a soma de todos os valores da lista, dividido pelo total de dias na lista menos os dias sem receita, de acordo com o enunciado
    average = sum / (listLenght-dayWithoutRevenue);

    return average;
}

double FindAboveAverage(List<double> values) 
{
    int days = 0;
    double average = FindAverage(values);
    for (var i = 0; i < values.Count; i++)
    {
        if (values[i] > average) days++;
    }
    return days;
}


double FindMaxValue(List<double> values) 
{
    double maxValue = 0;
    for (var i = 0; i < values.Count; i++)
    {
        if (maxValue < values[i]) maxValue = values[i];
    }
    return maxValue;
}
double FindMinValue(List<double> values) 
{
    double minValue = 0;
    double temp;
    for (var i = 0; i < values.Count; i++ )
    {
        if (values[i] > 0)
        {
            temp = values[i];
            if (minValue == 0)
            {
                minValue = temp;
            }
            else if (minValue > temp)
            {
                minValue = temp;
            }        
}
       
    }
    return minValue;
}


#endregion






void StateShare()
{
    double SP = 67836.43, RJ = 36678.66, MG = 29229.88, ES = 27165.48, others = 19849.53;

    double totalAmount = SP + RJ + MG + ES + others;

    Console.WriteLine
        (   line +
            "\n Percentual de participação por estado: \n" +
            "\n \t --> SP: " + Math.Round(SP * 100/totalAmount,2) +"%" +
            "\n \t --> RJ: " + Math.Round(RJ * 100/totalAmount,2) +"%" +
            "\n \t --> MG: " + Math.Round(MG * 100/totalAmount,2) +"%" +
            "\n \t --> ES: " + Math.Round(ES * 100/totalAmount,2) +"%" +
            "\n \t --> outros estados: " + Math.Round(others * 100/totalAmount,2) + "%"
        );
    Console.WriteLine(line);





}

void ReverseString ()
{
    Console.WriteLine("\n Insira uma palavra para inverter: \n \n");
    string s = Console.ReadLine();
    int l = s.Length;
    string newString = string.Empty;
    if (l == 0) Console.WriteLine(line + "Infelizmente não foi fornecido uma string para inverter" + line);
    else
    {
        for (int i = l - 1; i >= 0; i--)  
        {
            newString += s[i];  // recebe primeiro o último elemento do comprimento da string s e vai diminuindo
        }
    }
    Console.WriteLine(line + "A palavra inserida invertida fica: " + newString + line);
}




public class Revenue
{
    public int dia { get; set; }
    public double valor { get; set; }

}


