using Microsoft.Maui.Layouts;
using Tienda.Excepciones;
using Tienda.Modelos;
using Tienda.Repositorio;

namespace Tienda;

public partial class PantallaAlta : ContentPage
{
    static bool hayVacio = false;
    private ClientesRepositorio clientesRepositorio = new ClientesRepositorio();

    public PantallaAlta()
	{
		InitializeComponent();
        CargarClientes();
    }

    // Para que al cambiar de pestaña y volver, salga todo limpio
    protected override void OnAppearing()
    {
        base.OnAppearing();
        ResetearPagina();
    }

    private void CargarClientes()
    {
            //Cargar clientes en la List View
            ClientesView.ItemsSource = clientesRepositorio.CargarClientes();
    }
   
    private void GuardarCliente()
    {
        try
        {
            if(!clientesRepositorio.ComprobarClienteExistente(entryCorreo.Text))
            {
                // crear cliente con los datos de los entry
                var cliente = new Cliente
                {
                    Nombre = entryNombre.Text,
                    Apellidos = entryApellidos.Text,
                    Ciudad = entryCiudad.Text,
                    Correo = entryCorreo.Text,
                    Comentario = editorComentario.Text,
                    Vip = chkVip.IsChecked
                };

                clientesRepositorio.GuardarCliente(cliente, entryCorreo.Text);

                imgRobot.Source = "imgbien.png";
                textoRobot.Text = "Cliente añadido correctamente";
            }else
            {
                imgRobot.Source = "imgmal.png";
                textoRobot.Text = "Error. Ya existe el cliente";
            }
        }
        catch (Exception ex) {

            imgRobot.Source = "imgmal.png";
            textoRobot.Text = "Error al guardar cliente";
        }
    }   



    // Cargar los datos del cliente desde la LISTVIEW al pulsar un cliente
    private void ClientesView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        var clienteSeleccionado = e.SelectedItem as Cliente;
        if (clienteSeleccionado == null)
            return;

        // Asignar los datos del cliente a los entry y demás
        entryNombre.Text = clienteSeleccionado.Nombre;
            entryApellidos.Text = clienteSeleccionado.Apellidos;
            entryCiudad.Text = clienteSeleccionado.Ciudad;
            entryCorreo.Text = clienteSeleccionado.Correo;
            editorComentario.Text = clienteSeleccionado.Comentario;
            chkVip.IsChecked = clienteSeleccionado.Vip;

            
            imgVip.Source = clienteSeleccionado.Vip ? "vipimg.png" : "novipimg.png";
            imgRobot.Source = "robot.png";
            textoRobot.Text = "Cliente seleccionado";

    }


    

    private void ResetearPagina()
    {
        entryNombre.Text = "";
        entryApellidos.Text = "";
        entryCiudad.Text = "";
        entryCorreo.Text = "";
        textoRobot.Text = "Rellene todos los campos";
        editorComentario.Text = "";
        chkVip.IsChecked = false;
        imgRobot.Source = "robot.png";
    }

    private void ImgVipCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if(e.Value)
        {
            imgVip.Source = "vipimg.png";  // está marcado
        }else
        {
            imgVip.Source = "novipimg.png";
        }
    }

    public bool ComprobarEntrys()
    {
        List<String> listaDatos = new List<String>();

        listaDatos.Add(entryNombre.Text);
        listaDatos.Add(entryApellidos.Text);
        listaDatos.Add(entryCiudad.Text);
        listaDatos.Add(entryCorreo.Text);
        listaDatos.Add(editorComentario.Text);


        // Verificar si alguno está vacío
        foreach (var entry in listaDatos)
        {
            if (string.IsNullOrWhiteSpace(entry))
            {
                hayVacio = true;
                break;
            }else
            {
                hayVacio = false;
            }
        }
        return hayVacio;
    }

    private void OnClickAnadir(object sender, EventArgs e)
	{
        try
        {
            if (ComprobarEntrys())
            {
                imgRobot.Source = "imgprohibido.png";
                textoRobot.Text = "Debe rellenar\ntodos los campos";
            }
            else
            {
                // Crear cliente con datos entrys
                Cliente nuevoCliente = new Cliente()
                {
                    Nombre = entryNombre.Text,
                    Apellidos = entryApellidos.Text,
                    Ciudad = entryCiudad.Text,
                    Correo = entryCorreo.Text,
                    Comentario = editorComentario.Text
                };

                clientesRepositorio.GuardarCliente(nuevoCliente, entryCorreo.Text);

                imgRobot.Source = "imgbien.png";
                textoRobot.Text = "Cliente añadido correctamente";

            }
        }catch (Exception ex)
        {
            textoRobot.Text = "Error. Ya existe el cliente";
            imgRobot.Source = "imgmal.png";
        }
        
    }

    
    private void OnClickModificar(object sender, EventArgs e)
    {
        try
        {
            // Crear cliente con los datos actuales de los entrys
            var clienteActualizado = new Cliente
            {
                Nombre = entryNombre.Text,
                Apellidos = entryApellidos.Text,
                Ciudad = entryCiudad.Text,
                Correo = entryCorreo.Text,
                Comentario = editorComentario.Text,
                Vip = chkVip.IsChecked
            };

            // Editar usando el correo como pk
            clientesRepositorio.ModificarCliente(entryCorreo.Text, clienteActualizado);

            // Actualizar ListView
            ClientesView.ItemsSource = null;
            ClientesView.ItemsSource = clientesRepositorio.CargarClientes();

            imgRobot.Source = "imgbien.png";
            textoRobot.Text = "Cliente modificado correctamente";
        }
        catch (Exception ex)
        {
            imgRobot.Source = "imgmal.png";
            textoRobot.Text = "Error al modificar cliente";
        }
    }

    private async void OnClickBorrar(object sender, EventArgs e)
    {
        try
        {
            bool respuesta = await DisplayAlert("Confirmar borrado", "¿Estás seguro de que deseas borrar este cliente?", "Sí", "No");

            if(respuesta)
            {
                string correo = entryCorreo.Text;

                clientesRepositorio.BorrarCliente(correo);

                // Actualizar ListView
                ClientesView.ItemsSource = null;
                ClientesView.ItemsSource = clientesRepositorio.CargarClientes();

                imgRobot.Source = "imgbien.png";
                textoRobot.Text = "Cliente borrado correctamente";
                ResetearPagina();
            }else
            {
                imgRobot.Source = "imgprohibido.png";
                textoRobot.Text = "Borrado cancelado";
            }
                
           
        }
        catch (Exception ex)
        {
            imgRobot.Source = "imgmal.png";
            textoRobot.Text = "Error al borrar cliente";
        }
    }

    private void OnClickLimpiar(object sender, EventArgs e)
    {
        ResetearPagina();
    }

    private void onClickGuardar(object sender, EventArgs e)
    {
        if(!ComprobarEntrys())
        {
            GuardarCliente();

            // Actualizar ListView
            ClientesView.ItemsSource = null;
            ClientesView.ItemsSource = clientesRepositorio.CargarClientes();
        }else
        {
            imgRobot.Source = "imgprohibido.png";
            textoRobot.Text = "Debe rellenar\n los campos primero";
        }
           
    }
}