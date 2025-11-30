using SQLite;
using AlquilerCoches.Models;

namespace AlquilerCoches.Services;

public class DatabaseService
{
    private readonly SQLiteAsyncConnection _db;

    public DatabaseService(string dbPath)
    {
        _db = new SQLiteAsyncConnection(dbPath);
        _db.CreateTableAsync<Coche>().Wait();
        _db.CreateTableAsync<Cliente>().Wait();
    }

    // ============================
    // COCHES
    // ============================

    public Task<List<Coche>> GetCochesDisponiblesAsync() =>
        _db.Table<Coche>().Where(c => !c.Alquilado).ToListAsync();

    public Task<List<Coche>> GetCochesAlquiladosAsync() =>
        _db.Table<Coche>().Where(c => c.Alquilado).ToListAsync();

    public Task AddCocheAsync(Coche coche) =>
        _db.InsertAsync(coche);

    public Task UpdateCocheAsync(Coche coche) =>
        _db.UpdateAsync(coche);

    // ✅ NUEVO: borrar coche
    public Task DeleteCocheAsync(Coche coche) =>
        _db.DeleteAsync(coche);

    // ============================
    // CLIENTES
    // ============================

    public Task AddClienteAsync(Cliente cliente) =>
        _db.InsertAsync(cliente);

    public Task<List<Cliente>> GetClientesAsync() =>
        _db.Table<Cliente>().ToListAsync();

 
    public Task DeleteClienteAsync(Cliente cliente) =>
        _db.DeleteAsync(cliente);
}
