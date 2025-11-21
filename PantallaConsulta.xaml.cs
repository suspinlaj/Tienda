using System.Collections.ObjectModel;
using Tienda.Modelos;
using Tienda.Repositorio;

namespace Tienda;

public partial class PantallaConsulta : ContentPage
{
    private ClientesRepositorio clientesRepositorio = new ClientesRepositorio();


    public PantallaConsulta()
	{
		InitializeComponent();
        

        // metodos de los pickers al ser clicados 
        pickerCiudades.SelectedIndexChanged += PickerCiudades_SelectedIndexChanged;
        pickerVip.SelectedIndexChanged += PickerVip_SelectedIndexChanged;

    }

    // Para que al cambiar de pestaña y volver, salga todo limpio
    protected override void OnAppearing()
    {
        base.OnAppearing();
        SacarCiudades();
        SacarVip();

        CargarClientes();
        ResetearPagina();
    }

    private void ResetearPagina()
    {
        pickerCiudades.SelectedIndex = 0;
        pickerVip.SelectedIndex = 0;
    }

    private void CargarClientes()
    {
        collectionClientes.ItemsSource = null;
        List<Cliente> clientes = clientesRepositorio.CargarClientes();

        collectionClientes.ItemsSource = clientes;

    }


    // Sacar las ciudades de los clientes
    private void SacarCiudades()
	{
		var clientes = clientesRepositorio.CargarClientes();

		var ciudades = clientes.Select(c => c.Ciudad).Distinct().OrderBy(c => c).ToList();

        // Añadir opción "Todos" al picker
        ciudades.Insert(0, "Todos");

        // añadir las ciudades al picker
        pickerCiudades.ItemsSource = ciudades;
    }

    private void SacarVip()
    {
        var vip = new List<string> { "Todos", "Vip", "No Vip" };
        pickerVip.ItemsSource = vip;
    }

    // metodo que ocurre al hacer click a una ciudad del picker
    private void PickerCiudades_SelectedIndexChanged(object sender, EventArgs e)
    {
        // obtener ciudad seleccionada en el picker
        string? ciudadSeleccionada = pickerCiudades.SelectedItem?.ToString();

        var clientes = clientesRepositorio.CargarClientes();


        if (ciudadSeleccionada == "Todos")
        {
            collectionClientes.ItemsSource = clientes;
        }
        else
        {
            // Filtrar clientes por ciudad
            var clientesCiudad = clientes.Where(c => c.Ciudad == ciudadSeleccionada).ToList();

            collectionClientes.ItemsSource = clientesCiudad;
        }
    }

    private void PickerVip_SelectedIndexChanged(object sender, EventArgs e)
    {
        // obtener vip o no vip en el picker
        string? vipSeleccionado = pickerVip.SelectedItem?.ToString();

        var clientes = clientesRepositorio.CargarClientes();

        if(vipSeleccionado == "Todos")
        {
            collectionClientes.ItemsSource = clientes;
        }else
        {
            // Filtrar por vip o no
            var clientesVip = clientes.Where(c => c.VipTexto == vipSeleccionado).ToList();

            collectionClientes.ItemsSource= clientesVip;
        }
    }

    

}