using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

class Program
{
    // Coloca aquí la Cadena de conexión que copiaste de Azure Portal
    private static string connectionString = "DefaultEndpointsProtocol=https;AccountName=miaarchivoskl;AccountKey=TU_ACCOUNT_KEY_AQUI;EndpointSuffix=core.windows.net";    
    // Nombre del contenedor solicitado en la práctica
    private static string containerName = "miaarchivos";

    static async Task Main(string[] args)
    {
        // Conexión directa usando el fragmento de tu ingeniero
        BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
        BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

        // Crear el contenedor si aún no existe en Azure
        await containerClient.CreateIfNotExistsAsync();

        bool salir = false;

        while (!salir)
        {
            Console.Clear();
            Console.WriteLine("=================================");
            Console.WriteLine("     MIA - AZURE BLOB STORAGE    ");
            Console.WriteLine("=================================");
            Console.WriteLine("1. Subir archivo");
            Console.WriteLine("2. Listar archivos");
            Console.WriteLine("3. Descargar archivo");
            Console.WriteLine("4. Eliminar archivo");
            Console.WriteLine("5. Salir");
            Console.WriteLine("=================================");
            Console.Write("Seleccione una opción: ");

            string opcion = Console.ReadLine();

            try
            {
                switch (opcion)
                {
                    case "1":
                        await SubirArchivo(containerClient);
                        break;
                    case "2":
                        await ListarArchivos(containerClient);
                        break;
                    case "3":
                        await DescargarArchivo(containerClient);
                        break;
                    case "4":
                        await EliminarArchivo(containerClient);
                        break;
                    case "5":
                        salir = true;
                        Console.WriteLine("Saliendo del programa...");
                        break;
                    default:
                        Console.WriteLine("Opción no válida. Presione ENTER para continuar.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nOcurrio un error: {ex.Message}");
            }

            if (!salir)
            {
                Console.WriteLine("\nPresione ENTER para regresar al menú...");
                Console.ReadLine();
            }
        }
    }

    // 1. Subir archivo
    static async Task SubirArchivo(BlobContainerClient containerClient)
    {
        Console.WriteLine("\n--- SUBIR ARCHIVO ---");
        Console.Write("Ingrese la ruta del archivo local: ");
        string rutaLocal = Console.ReadLine().Trim('"');

        if (!File.Exists(rutaLocal))
        {
            Console.WriteLine("El archivo no existe.");
            return;
        }

        string nombreArchivo = Path.GetFileName(rutaLocal);
        BlobClient blobClient = containerClient.GetBlobClient(nombreArchivo);

        Console.WriteLine("Subiendo archivo a Azure...");
        using FileStream stream = File.OpenRead(rutaLocal);
        await blobClient.UploadAsync(stream, overwrite: true);

        Console.WriteLine($"¡Éxito! El archivo '{nombreArchivo}' fue subido a la nube.");
    }

    // 2. Listar archivos
    static async Task ListarArchivos(BlobContainerClient containerClient)
    {
        Console.WriteLine("\n--- LISTA DE ARCHIVOS ---");
        Console.WriteLine("{0,-30} {1,-15}", "Nombre", "Tamaño");
        Console.WriteLine(new string('-', 48));

        int cantidad = 0;
        await foreach (BlobItem blob in containerClient.GetBlobsAsync())
        {
            long tamanoBytes = blob.Properties.ContentLength ?? 0;
            Console.WriteLine("{0,-30} {1,-15}", blob.Name, $"{tamanoBytes} bytes");
            cantidad++;
        }

        if (cantidad == 0)
        {
            Console.WriteLine("No hay archivos en el contenedor.");
        }
    }

    // 3. Descargar archivo
    static async Task DescargarArchivo(BlobContainerClient containerClient)
    {
        Console.WriteLine("\n--- DESCARGAR ARCHIVO ---");
        Console.Write("Ingrese el nombre del archivo en la nube: ");
        string nombreArchivo = Console.ReadLine().Trim();

        BlobClient blobClient = containerClient.GetBlobClient(nombreArchivo);

        if (!await blobClient.ExistsAsync())
        {
            Console.WriteLine("El archivo no existe en Azure.");
            return;
        }

        Console.Write("Ingrese la carpeta local de destino: ");
        string carpetaDestino = Console.ReadLine().Trim('"');

        if (!Directory.Exists(carpetaDestino))
        {
            Directory.CreateDirectory(carpetaDestino);
        }

        string rutaGuardado = Path.Combine(carpetaDestino, nombreArchivo);

        Console.WriteLine("Descargando...");
        await blobClient.DownloadToAsync(rutaGuardado);

        Console.WriteLine($"¡Archivo descargado en: {Path.GetFullPath(rutaGuardado)}!");
    }

    // 4. Eliminar archivo
    static async Task EliminarArchivo(BlobContainerClient containerClient)
    {
        Console.WriteLine("\n--- ELIMINAR ARCHIVO ---");
        Console.Write("Ingrese el nombre del archivo a borrar: ");
        string nombreArchivo = Console.ReadLine().Trim();

        BlobClient blobClient = containerClient.GetBlobClient(nombreArchivo);

        if (!await blobClient.ExistsAsync())
        {
            Console.WriteLine("El archivo no existe en Azure.");
            return;
        }

        Console.Write($"¿Está seguro de eliminar '{nombreArchivo}'? (S/N): ");
        string confirmacion = Console.ReadLine().Trim().ToUpper();

        if (confirmacion == "S")
        {
            await blobClient.DeleteIfExistsAsync();
            Console.WriteLine($"El archivo '{nombreArchivo}' fue eliminado.");
        }
        else
        {
            Console.WriteLine("Operación cancelada.");
        }
    }
}