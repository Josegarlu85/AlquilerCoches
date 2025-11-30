using SQLite;

namespace AlquilerCoches.Models;

public class Cliente
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string Nombre { get; set; }
    public string Apellidos { get; set; }
    public string DNI { get; set; }
}
