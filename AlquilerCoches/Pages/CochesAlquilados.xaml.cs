using AlquilerCoches.Models;
using AlquilerCoches.Services;

namespace AlquilerCoches.Pages;

public partial class CochesAlquilados : ContentPage
{
    private readonly DatabaseService _db;
    private Coche cocheSeleccionado;

    public CochesAlquilados(DatabaseService db)
    {
        InitializeComponent();
        _db = db;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        CochesAlquiladosView.ItemsSource = await _db.GetCochesAlquiladosAsync();
    }

    private void CochesAlquiladosView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        cocheSeleccionado = e.CurrentSelection.FirstOrDefault() as Coche;
    }

    private async void OnDevolverClicked(object sender, EventArgs e)
    {
        if (cocheSeleccionado == null)
        {
            await DisplayAlert("Error", "Selecciona un coche", "OK");
            return;
        }

        cocheSeleccionado.Alquilado = false;
        cocheSeleccionado.ClienteNombre = null;

        await _db.UpdateCocheAsync(cocheSeleccionado);

        await DisplayAlert("OK", "Coche devuelto correctamente", "Aceptar");

        // Refrescar lista
        cocheSeleccionado = null;
        CochesAlquiladosView.SelectedItem = null;
        CochesAlquiladosView.ItemsSource = await _db.GetCochesAlquiladosAsync();

        // ? Volver a la pestaña de coches disponibles
        await Shell.Current.GoToAsync("//CochesAlquiler");
    }
}
