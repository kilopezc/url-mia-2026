using System.Text.Json;
using System; 

class Program
{
    static void Main()
    {
        //Variables
        string nombreUsuario;
        string rutaArchivo;
        int numLineas, numCaracteres, numVocales;
        //Entrada
        Console.WriteLine("Ingrese su nombre completo: ");
        nombreUsuario=Console.ReadLine();
        nombreUsuario = nombreUsuario.Replace(" ", "_");
        Console.WriteLine("Ingrese su la ruta: ");
        rutaArchivo=Console.ReadLine();
        //Proceso
        if(!File.Exists(rutaArchivo))
        {
            Console.WriteLine("El archivo no existe");
            return;
        }

        string contenido = File.ReadAllText(rutaArchivo);
        string[] lineas = File.ReadAllLines(rutaArchivo);
        numLineas = lineas.Length;
        numCaracteres = contenido.Length;
        numVocales = ContarVocales(contenido);
        //Salida
        Console.WriteLine("======== RESULTADOS ========");
        Console.WriteLine($"Líneas: {numLineas}");
        Console.WriteLine($"Vocales: {numVocales}");
        Console.WriteLine($"Caracteres: {numCaracteres}");

        string carpeta = Path.GetDirectoryName(rutaArchivo);
        string nombreCSV = $"resultados_{nombreUsuario}.csv";
        string rutaCSV = Path.Combine(carpeta, nombreCSV);
        string csv = "nombre,lineas,vocales,caracteres\n" +
                     $"{nombreUsuario},{numLineas},{numVocales},{numCaracteres}";

        File.WriteAllText(rutaCSV, csv);
        Console.WriteLine($"\nArchivo CSV guardado en:");
        Console.WriteLine(rutaCSV);
    }

    static int ContarVocales(string texto)
    {
        int contador = 0;
        foreach (char c in texto.ToLower())
        {
            if ("aeiouáéíóú".Contains(c))
            {
                contador++;
            }
        }
        return contador;
    }
}