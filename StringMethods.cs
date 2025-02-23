public static class StringExtension
{
    public static bool EsNumero (this string text)
    {
        if (text == null)
            return false;
        
        return double.TryParse(text, out _);
    }

    public static bool EsNull (this string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            Console.Clear();
            Console.WriteLine("Entrada no válida, intente de nuevo");
            Console.ReadLine();
            return false;
        }
        else
        {
            return true;
        }
    }
}