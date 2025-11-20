using Tienda.Modelos;
using Tienda.Repositorio;

namespace Tienda;

public partial class PantallaConsultaIndividual : ContentPage
{
    private ClientesRepositorio clientesRepositorio = new ClientesRepositorio();

    public PantallaConsultaIndividual()
	{
		InitializeComponent();
        CargarClientes();
    }

    private void CargarClientes()
    {
        List<Cliente> clientes = clientesRepositorio.CargarClientes();

        collectionClientes.ItemsSource = clientes;

    }

    private void OnClickSiguiente(object sender, EventArgs e)
	{

	}

    private void OnClickAtras(object sender, EventArgs e)
    {

    }
}