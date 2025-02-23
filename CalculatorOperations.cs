namespace Operations;

public static class Operations
{
    // Sobrecargar de método de suma
    public static int Sumar(int primerNumero, int segundoNumero) => primerNumero + segundoNumero;
    public static long Sumar(long primerNumero, long segundoNumero) => primerNumero + segundoNumero;
    public static double Sumar(double primerNumero, double segundoNumero) => primerNumero + segundoNumero;

    // Sobrecargas de método de resta
    public static int Restar(int primerNumero, int segundoNumero) => primerNumero - segundoNumero;
    public static long Restar(long primerNumero, long segundoNumero) => primerNumero - segundoNumero;
    public static double Restar(double primerNumero, double segundoNumero) => primerNumero - segundoNumero;
    
    // Sobrecargas de método de multiplicación
    public static int Multiplicar(int primerNumero, int segundoNumero) => primerNumero * segundoNumero;
    public static long Multiplicar(long primerNumero, long segundoNumero) => primerNumero * segundoNumero;
    public static double Multiplicar(double primerNumero, double segundoNumero) => primerNumero * segundoNumero;

    // Sobrecargas de método de división
    public static int Dividir(int primerNumero, int segundoNumero) => primerNumero / segundoNumero;
    public static long Dividir(long primerNumero, long segundoNumero) => primerNumero / segundoNumero;
    public static double Dividir(double primerNumero, double segundoNumero) => primerNumero / segundoNumero;

    // Método de potencia
    public static double Potencia(double primerNumero, double potencia) => Math.Pow(primerNumero, potencia);

    // Método raíz cuadrada
    public static double RaizCuadrada(double primerNumero) => Math.Sqrt(primerNumero);
}