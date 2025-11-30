using SQLite;

namespace AlquilerCoches.Models;

public class Coche
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string Marca { get; set; }
    public string Modelo { get; set; }
    public int Kilometros { get; set; }

    public string Matricula { get; set; }   

    public bool Alquilado { get; set; } = false;

    public string ClienteNombre { get; set; }
}
