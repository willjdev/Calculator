using System.Globalization;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using NCalc;
using static Operations.Operations;

CultureInfo culturaUsuario = CultureInfo.CurrentCulture;
List<double> listaDeNumeros = new List<double>();
string operationToExecute = "";
string historial = "";
double resultadoTotal = 0;
double numeroAnterior = 0;
int contadorOperaciones = 0;
bool exitProgram = true;
bool squareTotalExecuted = true;
bool errorDivision = false;


do
{
    Console.Clear();
    Console.WriteLine("Calculadora Wilds!!");
    Console.WriteLine("Presione 1 si desea utilizar la calculadora detallada, o presione 2 si desea utilizar la calculadora directa.\nPresione ESC en cualquier momento para finalizar el proceso ");
    string modoCalculadoraElegido = LeerEntrada();

    if (modoCalculadoraElegido == "1")
    {
        break;
    }
    else if (modoCalculadoraElegido == "2")
    {
        try
        {
            SecondMode();
            Console.Clear();
            Console.WriteLine("Hasta luego!");
            Environment.Exit(0);
        }
        catch (InvalidOperationException ex)
        {
            Console.Clear();
            Console.WriteLine($"Error en operación: {ex.Message}. Ingrese una operación valida");
            Console.ReadLine();
        }
        catch (DivideByZeroException ex)
        {
            Console.Clear();
            Console.WriteLine($"Error en operación: {ex.Message}. Ingrese una operación válida");
            Console.ReadLine();
        }

    }     
} while (true);




do
{
    try
    {
        if (squareTotalExecuted)
        {
            var userEntry = SolicitarNumero();

            ExecuteOperation(operationToExecute);
        }
    }
    catch (DivideByZeroException ex)
    {
        Console.WriteLine(ex.Message);
        errorDivision = true;
        exitProgram = false;
    }

    if (errorDivision)
        continue;
    
    operationToExecute = Menu();

    ExecuteSquareAndShowResult(operationToExecute);

} while (exitProgram);


string Menu ()
{
    bool cerrarCiclo = true;
    string? userEntry = null;
    do
    {
        Console.Clear();
        Console.WriteLine("Ingrese el signo de operación a realizar entre: ");
        Console.WriteLine("Suma (+)");
        Console.WriteLine("Resta (-)");
        Console.WriteLine("Multiplicación (*)");
        Console.WriteLine("División (/)");
        Console.WriteLine("Potencia (^)");
        Console.WriteLine("Raíz cuadrada (r)");
        Console.WriteLine("Resultado (=)");

        if (resultadoTotal != 0)
            Console.WriteLine("\nMostrar historial (h)");
            Console.WriteLine("Exportar historial (c)");

        if (contadorOperaciones > 1)
            Console.WriteLine("Deshacer operación anterior (d)");

        Console.WriteLine("\nO presione la tecla Esc en cualquier momento para terminar la operación");

        userEntry = LeerEntrada();

        var operadores = "+-*/^r=hdc";    

        if (userEntry.EsNull())
        {
            if (!operadores.Contains(userEntry))
            {
                Console.Clear();
                Console.WriteLine("Dato ingresado inválido, intente de nuevo");
                Console.ReadLine();
                cerrarCiclo = true;
                continue;
            }
            cerrarCiclo = false;
        } 

    } while (cerrarCiclo);

    squareTotalExecuted = true;

    char[] opcionesExtra = ['h', 'd', 'c'];

    if (userEntry.IndexOfAny(opcionesExtra) == -1)
        historial = historial + " " + userEntry;

    return userEntry;
}

string SolicitarNumero ()
{
    contadorOperaciones++;

    string? segundoNumero = null;
    Console.Clear();
    if (listaDeNumeros.Count == 1 && resultadoTotal == 0)
    {
        Console.WriteLine("Ingresa un numero: ");
        Console.Write($"{listaDeNumeros[0]} {operationToExecute} ");
    }
    else if (listaDeNumeros.Count == 0 && resultadoTotal != 0)
    {
        Console.WriteLine("Ingresa un número: ");
        if (contadorOperaciones > 3)
            Console.Write($"{resultadoTotal} {operationToExecute} ");
        else
            Console.Write(historial + " ");
    }
    else
    {
        Console.WriteLine("Ingresa un número");
    }

    segundoNumero = LeerEntrada();

    if (!segundoNumero.EsNumero())
    {
        Console.Clear();
        Console.WriteLine($"El valor ingresado '{segundoNumero}' no es un dato válido. Intente de nuevo");
        Console.ReadLine();
        return SolicitarNumero();
    }

    double.TryParse(segundoNumero, NumberStyles.Float, culturaUsuario, out double numeroConvertido);
    listaDeNumeros.Add(numeroConvertido);

    historial += " " + segundoNumero;
    return segundoNumero;
}

void ExecuteSquareAndShowResult (string userOperator)
{
    if (resultadoTotal == 0 && listaDeNumeros.Count == 1 && userOperator == "r")
    {
        resultadoTotal = RaizCuadrada(listaDeNumeros[0]);
        numeroAnterior = listaDeNumeros[0];
        listaDeNumeros.Clear();
        Console.Clear();
        Console.WriteLine(resultadoTotal);
        exitProgram = EndOperation();
        contadorOperaciones++;
    }
    else if (resultadoTotal == 0 && listaDeNumeros.Count == 1 && userOperator == "=")
    {
        Console.Clear();
        Console.WriteLine(listaDeNumeros[0]);
        exitProgram = EndOperation();
    }
    else if  (resultadoTotal != 0 && listaDeNumeros.Count == 0 && userOperator == "=")
    {
        Console.Clear();
        Console.WriteLine(resultadoTotal);
        exitProgram = EndOperation();
    }
    else if (resultadoTotal != 0 && listaDeNumeros.Count == 0 && userOperator == "r")
    {
        Console.Clear();
        numeroAnterior = resultadoTotal;
        resultadoTotal = RaizCuadrada(resultadoTotal);
        Console.WriteLine(resultadoTotal);
        exitProgram = EndOperation();
    }
    else if (userOperator == "h")
    {
        Console.Clear();
        Console.WriteLine($"Historial de operaciones:\n{historial} = {resultadoTotal}");
        Console.ReadLine();
        exitProgram = EndOperation();
    }
    else if (userOperator == "d")
    {
        Console.Clear();
        UnDoOperation();
        Console.ReadLine();
    }
    else if (userOperator == "c")
    {
        EscribirHistorialArchivo();
    }
}

void ShowResult()
{
    Console.Clear();
    Console.WriteLine(resultadoTotal);
    Console.ReadLine();
}

void ExecuteOperation(string userOperator)
{
    double primerNumero = 0;
    double segundoNumero = 0;

    if (resultadoTotal == 0 && listaDeNumeros.Count == 1)
    {
        return;
    }   
    else if (resultadoTotal != 0 && listaDeNumeros.Count == 1)
    {
        primerNumero = resultadoTotal;
        segundoNumero = listaDeNumeros[0];
    }
    else if (resultadoTotal == 0 && listaDeNumeros.Count == 2)
    {
        primerNumero = listaDeNumeros[0];
        segundoNumero = listaDeNumeros[1];
    }
    
    Dictionary<string, Func<double, double, double>> operations = new Dictionary<string, Func<double, double, double>>()
    {
        { "+", Sumar },
        { "-", Restar },
        { "*", Multiplicar },
        { "/", (a, b) => b == 0 ? throw new DivideByZeroException("Error al dividir entre 0.\nSaliendo...") : Dividir(a, b) },
        { "^", Potencia }
    };

    if (operations.ContainsKey(userOperator))
    {
        numeroAnterior = primerNumero;
        resultadoTotal = operations[userOperator](primerNumero, segundoNumero);
        listaDeNumeros.Clear();
        ShowResult();
    }

}

bool EndOperation()
{
        string? continuarOperando;
        squareTotalExecuted = false;
    do
    {
        Console.WriteLine("¿Quieres continuar operando? Escibre 'Y' para sí o 'N' para no");
        continuarOperando = LeerEntrada();

    } while (!continuarOperando.EsNull());
    
    if (continuarOperando.ToLower() == "y")
    {
        return true;
    }
    else if (continuarOperando.ToLower() == "n")
    {
        Console.WriteLine("Hasta luego!");
        return false;
    }

    return false;
}

void UnDoOperation ()
{
    Console.WriteLine("Operación deshecha!\n");
    Console.WriteLine($"Nuevo resultado:\n{numeroAnterior}");
    if ( (contadorOperaciones == 2) || (contadorOperaciones == 1 && operationToExecute == "r") )
    {
        resultadoTotal = 0;
        listaDeNumeros.Clear();
        listaDeNumeros.Add(numeroAnterior);
    }
    else if (contadorOperaciones > 2)
    {
        resultadoTotal = numeroAnterior;
        listaDeNumeros.Clear();
    }
        squareTotalExecuted = false;
}

static string LeerEntrada ()
{
    string entrada = "";

    while (true)
    {
        var tecla = Console.ReadKey(true);
        if (tecla.Key == ConsoleKey.Escape)
        {
            Console.Clear();
            Console.WriteLine("Hasta luego!");
            Environment.Exit(0);
        }
        if (tecla.Key == ConsoleKey.Enter) 
            break;
        if (tecla.Key == ConsoleKey.Backspace && entrada.Length > 0)
        {
            entrada = entrada[..^1];
            Console.Write("\b \b");
        }
        else if (!char.IsControl(tecla.KeyChar))
        {
            entrada += tecla.KeyChar;
            Console.Write(tecla.KeyChar);
        }
    }

    entrada = entrada.Trim();
    Console.WriteLine();
    return entrada;
}

void EscribirHistorialArchivo()
{
    string ruta = "Historial.txt";
    string contenido = $"Historial de operaciones:\n{historial} = {resultadoTotal}";

    File.WriteAllText(ruta, contenido);

    Console.Clear();
    Console.WriteLine("Archivo de historial creado!");
    exitProgram = EndOperation();

}

void SecondMode ()
{
    do
    {
        Console.Clear();
        Console.WriteLine("Ingrese la operación que desea realizar, luego presine ENTER para obtener el resultado");

        string userEntry = LeerEntrada();
        string temporalUserEntry = userEntry;
        string numeroOperarRaiz = "";

        for (int i = 0; i < userEntry.Length; i++)
        {
            if (Regex.IsMatch(userEntry[i].ToString(), "[^0-9.+\\-*/^r]"))
            {
                throw new ArgumentException("Syntax error");
            }
        }

        for (int i = 0; i < temporalUserEntry.Length; i++)
        {
            if (temporalUserEntry[i] == 'r' && temporalUserEntry[i+1] != 't') 
            {
                for (int j = i + 1; j < userEntry.Length; j++)
                {
                    if (Regex.IsMatch(userEntry[j].ToString(), "[0-9\\.]"))
                    {
                        numeroOperarRaiz += userEntry[j];
                    }
                    else
                    {
                        break;
                    }
                }

                temporalUserEntry = $"{temporalUserEntry.Substring(0, i)}Sqrt({numeroOperarRaiz}){temporalUserEntry.Substring(i + numeroOperarRaiz.Length + 1)}";


            }
        }

        temporalUserEntry = OperarPotencia(temporalUserEntry);

        if (temporalUserEntry.Contains("/0"))
            throw new DivideByZeroException("División por cero invalida");
        if (temporalUserEntry.Length < 3)
            throw new InvalidOperationException("Operación incompleta");


        NCalc.Expression expresion = new NCalc.Expression(temporalUserEntry);
        Console.Clear();
        Console.WriteLine(expresion.Evaluate());
        exitProgram = EndOperation();
    } while (exitProgram);

}

string OperarPotencia(string operacion)
{
    string primerNumero = "";
    string segundoNumero = "";
        for (int i = 0; i < operacion.Length; i++)
        {
            if (operacion[i] == '^') 
            {
                for (int j = i-1; j >= 0; j--)
                {
                    if (Regex.IsMatch(operacion[j].ToString(), "[0-9\\.]"))
                        primerNumero += operacion[j];
                    else
                        break;
                }

                for (int k = i + 1; k < operacion.Length; k++)
                {
                    if (Regex.IsMatch(operacion[k].ToString(), "[0-9\\.]"))
                        segundoNumero += operacion[k];
                    else
                        break;
                }

                operacion = $"{operacion.Substring(0, i - primerNumero.Length)}Pow({primerNumero.Reverse()},{segundoNumero}){operacion.Substring(i + segundoNumero.Length + 1)}";
            }
        }

        return operacion;
}