using AlquilerCoches.Models;
using AlquilerCoches.Services;

namespace AlquilerCoches.Pages;

public partial class CochesAlquiler : ContentPage
{
    private readonly DatabaseService _db;
    private Coche cocheSeleccionado;

    public CochesAlquiler(DatabaseService db)
    {
        InitializeComponent();
        _db = db;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        CochesView.ItemsSource = await _db.GetCochesDisponiblesAsync();
    }

    private void CochesView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        cocheSeleccionado = e.CurrentSelection.FirstOrDefault() as Coche;
    }

    private async void OnAddClicked(object sender, EventArgs e)
    {
        string marca = await DisplayPromptAsync("Nuevo coche", "Marca:");
        if (string.IsNullOrWhiteSpace(marca)) return;

        string modelo = await DisplayPromptAsync("Nuevo coche", "Modelo:");
        if (string.IsNullOrWhiteSpace(modelo)) return;

        string matricula = await DisplayPromptAsync("Nuevo coche", "Matrícula:");
        if (string.IsNullOrWhiteSpace(matricula)) return;

        string kmsStr = await DisplayPromptAsync("Nuevo coche", "Kilómetros:");
        if (!int.TryParse(kmsStr, out int kms)) return;

        var coche = new Coche
        {
            Marca = marca,
            Modelo = modelo,
            Matricula = matricula,
            Kilometros = kms,
            Alquilado = false
        };

        await _db.AddCocheAsync(coche);
        CochesView.ItemsSource = await _db.GetCochesDisponiblesAsync();
    }

    private async void OnAlquilarClicked(object sender, EventArgs e)
    {
        if (cocheSeleccionado == null)
        {
            await DisplayAlert("Error", "Selecciona un coche", "OK");
            return;
        }

        string nombre = await DisplayPromptAsync("Alquilar coche", "Nombre del cliente:");
        if (string.IsNullOrWhiteSpace(nombre)) return;

        string apellidos = await DisplayPromptAsync("Alquilar coche", "Apellidos del cliente:");
        if (string.IsNullOrWhiteSpace(apellidos)) return;

        string dni = await DisplayPromptAsync("Alquilar coche", "DNI del cliente:");
        if (string.IsNullOrWhiteSpace(dni)) return;

        var cliente = new Cliente
        {
            Nombre = nombre,
            Apellidos = apellidos,
            DNI = dni
        };

        await _db.AddClienteAsync(cliente);

        cocheSeleccionado.Alquilado = true;
        cocheSeleccionado.ClienteNombre = $"{nombre} {apellidos}";

        await _db.UpdateCocheAsync(cocheSeleccionado);

        await DisplayAlert("OK", "Coche alquilado correctamente", "Aceptar");

        cocheSeleccionado = null;
        CochesView.SelectedItem = null;
        CochesView.ItemsSource = await _db.GetCochesDisponiblesAsync();
    }

    
    private async void OnModificarKmClicked(object sender, EventArgs e)
    {
        if (cocheSeleccionado == null)
        {
            await DisplayAlert("Error", "Selecciona un coche", "OK");
            return;
        }

        string kmsStr = await DisplayPromptAsync("Modificar kilómetros",
            $"Kilómetros actuales: {cocheSeleccionado.Kilometros}\nIntroduce los nuevos km:");

        if (!int.TryParse(kmsStr, out int nuevosKm)) return;

        cocheSeleccionado.Kilometros = nuevosKm;
        await _db.UpdateCocheAsync(cocheSeleccionado);

        await DisplayAlert("OK", "Kilómetros actualizados", "Aceptar");

        CochesView.ItemsSource = await _db.GetCochesDisponiblesAsync();
    }


    private async void OnBorrarClicked(object sender, EventArgs e)
    {
        if (cocheSeleccionado == null)
        {
            await DisplayAlert("Error", "Selecciona un coche", "OK");
            return;
        }

        bool confirmar = await DisplayAlert("Confirmar",
            $"¿Seguro que quieres borrar el coche {cocheSeleccionado.Marca} {cocheSeleccionado.Modelo}?",
            "Sí", "No");

        if (!confirmar) return;

        await _db.DeleteCocheAsync(cocheSeleccionado);

        await DisplayAlert("OK", "Coche eliminado", "Aceptar");

        cocheSeleccionado = null;
        CochesView.SelectedItem = null;
        CochesView.ItemsSource = await _db.GetCochesDisponiblesAsync();
    }
}
