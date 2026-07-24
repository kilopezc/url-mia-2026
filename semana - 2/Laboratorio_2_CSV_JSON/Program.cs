using System.Text.Json;

string[] leer = File.ReadAllLines("estudiantes.csv");

List<Estudiante> listaEstudiantes = new List<Estudiante>();

for(int i=1;i<leer.Length;i++) 
{
    string[] subs = leer[i].Split(',');
    
    Estudiante est = new Estudiante()
    {
        Id=int.Parse(subs[0]), 
        Nombre=subs[1],
        Carrera=subs[2]
    };

    listaEstudiantes.Add(est);
}   

foreach (Estudiante e in listaEstudiantes)
{
    Console.WriteLine($"{e.Id} - {e.Nombre} - {e.Carrera}");
}

var options = new JsonSerializerOptions{WriteIndented=true};
string jsonString = JsonSerializer.Serialize(listaEstudiantes,options);
File.WriteAllText("estudiantes.json",jsonString);
Console.WriteLine("Archivo estudiantes.json creado correctamente");
