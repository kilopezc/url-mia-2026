# Laboratorio 2 - Azure Blob Storage

## Objetivo
Desarrollar una aplicación de consola en C# (.NET) capaz de conectarse a Azure Blob Storage utilizando un Connection String para gestionar archivos en la nube (subir, listar, descargar y eliminar).

## Tecnologías Utilizadas
Lenguaje: C# / .NET
SDK Oficial: "Azure.Storage.Blobs"
Plataforma de Nube: Microsoft Azure Storage Account

## Configuración de Azure
1. Se creó la cuenta de almacenamiento en Azure Portal ("miaarchivoskl")
2. Se creó un contenedor de bloques con el nombre "miaarchivos" configurado con nivel de acceso Private.
3. Se obtuvo la clave de acceso (Connection String) desde la sección **Security + networking -> Access keys**.

## Arquitectura de la Solución
El programa utiliza las tres clases principales del SDK de Azure:
`BlobServiceClient`: Representa la conexión principal a la cuenta de almacenamiento de Azure.
`BlobContainerClient`: Administra las operaciones al nivel del contenedor `miaarchivos`.
`BlobClient`: Permite interactuar directamente con un archivo individual (subida, descarga, consulta y eliminación).

## Descripción de Operaciones
1. `Subir archivo:` Valida que la ruta local exista, abre un flujo de lectura y sube el archivo a Azure Blob Storage con opción de sobrescribir.
2. `Listar archivos:` Recorre en bucle todos los blobs dentro del contenedor y muestra un listado con su nombre y tamaño exacto en bytes.
3. `Descargar archivo:` Comprueba la existencia del blob en la nube, asegura la creación del directorio local de destino y descarga el archivo.
4. `Eliminar archivo:` Confirma con el usuario antes de invocar la eliminación definitiva del archivo en Azure.

## Manejo de Errores
Toda la ejecución del menú está envuelta en bloques try-catch para capturar excepciones inesperadas de red o de sistema de archivos sin que el programa se detenga de forma abrupta.

## Protección de la Connection String
Para proteger las credenciales sensibles y evitar su divulgación en GitHub:

* Se mantuvieron las credenciales almacenadas de manera local durante el desarrollo.

* Al subir el repositorio, la variable `connectionString` se limpia o reemplaza por un marcador de posición (ej. `"TU_CONNECTION_STRING_AQUI"`), evitando subir claves privadas directas en el código fuente.

## Instrucciones para Ejecutar el Proyecto
1. Clonar el repositorio.
2. Abrir la solución en la terminal y restaurar paquetes:
   ```bash
   dotnet restore