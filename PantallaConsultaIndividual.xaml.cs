using System.Collections.ObjectModel;
using Tienda.Modelos;
using Tienda.Repositorio;

namespace Tienda;

public partial class PantallaConsultaIndividual : ContentPage
{
    private ClientesRepositorio clientesRepositorio = new ClientesRepositorio();
    private int indice = 0;


    public PantallaConsultaIndividual()
	{
		InitializeComponent();
    }

    // Para que al cambiar de pestaña y volver, salga todo limpio
    protected override void OnAppearing()
    {
        base.OnAppearing();

        CargarClientes();
        ResetearPagina();
    }

    private void ResetearPagina()
    {
        entryBuscarCliente.Text = string.Empty;
    }

    // metodo del entry que se ejecuta cada vez que el usuario cambia el texto del entry
    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        // e.NewTextValue es lo que acaba de escribir el usuario
        string texto = e.NewTextValue?.ToLower();

        // filtrar clientes por nombre o apellido
        var filtrados = clientesRepositorio.CargarClientes()
            .Where(c => c.Nombre.ToLower().Contains(texto) || c.Apellidos.ToLower().Contains(texto)).ToList();

        collectionClientes.ItemsSource = filtrados;
    }

    private void CargarClientes()
    {
        List<Cliente> clientes = clientesRepositorio.CargarClientes();

        collectionClientes.ItemsSource = clientes;

    }

    private void OnClickSiguiente(object sender, EventArgs e)
	{
        var cliente = collectionClientes.ItemsSource as IList<Cliente>;
        if (indice < cliente.Count - 1)
        {
            indice++;
        }

        collectionClientes.SelectedItem = cliente[indice];
        collectionClientes.ScrollTo(cliente[indice]);
	}

    private void OnClickAtras(object sender, EventArgs e)
    {
        var items = collectionClientes.ItemsSource as IList<Cliente>;

        if (indice > 0)
            indice--;

        collectionClientes.SelectedItem = items[indice];
        collectionClientes.ScrollTo(items[indice]);
    }
}