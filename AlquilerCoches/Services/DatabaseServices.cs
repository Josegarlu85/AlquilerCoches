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

    public Task<List<Coche>> GetCochesDisponiblesAsync()
    {
        return _db.QueryAsync<Coche>("SELECT * FROM Coche WHERE Alquilado = 0");
    }

    public Task<List<Coche>> GetCochesAlquiladosAsync()
    {
        return _db.QueryAsync<Coche>("SELECT * FROM Coche WHERE Alquilado = 1");
    }

    public Task AddCocheAsync(Coche coche)
    {
        return _db.InsertAsync(coche);
    }

    public Task UpdateCocheAsync(Coche coche)
    {
        return _db.UpdateAsync(coche);
    }

    public Task DeleteCocheAsync(Coche coche)
    {
        return _db.DeleteAsync(coche);
    }

    // ============================
    // CLIENTES
    // ============================

    public Task AddClienteAsync(Cliente cliente)
    {
        return _db.InsertAsync(cliente);
    }

    public Task<List<Cliente>> GetClientesAsync()
    {
        return _db.QueryAsync<Cliente>("SELECT * FROM Cliente");
    }

    public Task DeleteClienteAsync(Cliente cliente)
    {
        return _db.DeleteAsync(cliente);
    }
}
