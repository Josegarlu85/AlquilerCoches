using AlquilerCoches.Services;
using AlquilerCoches.Pages;

namespace AlquilerCoches;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder.UseMauiApp<App>();

        string dbPath = Path.Combine(FileSystem.AppDataDirectory, "alquiler.db3");
        builder.Services.AddSingleton(new DatabaseService(dbPath));

        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddSingleton<CochesAlquiler>();
        builder.Services.AddSingleton<CochesAlquilados>();

        return builder.Build();
    }
}
