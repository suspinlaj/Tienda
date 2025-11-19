using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tienda.Excepciones;
using Tienda.Modelos;

namespace Tienda.Repositorio
{
    internal class ClientesRepositorio
    {
        private List<Cliente> listaClientes = new List<Cliente>();


        // Cargar el txt (click derecho a "datosClientes.txt" y poner Accion de Compilación en -> MauiAssets
        public List<Cliente> CargarClientes()
        {
            try
            {
                string ruta = Path.Combine(AppContext.BaseDirectory, "Ficheros", "datosClientes.txt");

                listaClientes.Clear();

                // Leer txt
                using (var sr = new StreamReader(ruta))
                {
                    while (!sr.EndOfStream)
                    {
                        string linea = sr.ReadLine();
                        if (!string.IsNullOrWhiteSpace(linea))
                        {
                            // separar por #
                            var campos = linea.Split('#');
                            bool.TryParse(campos[5], out bool vip);
                            listaClientes.Add(new Cliente
                            {
                                Nombre = campos[0],
                                Apellidos = campos[1],
                                Ciudad = campos[2],
                                Correo = campos[3],
                                Comentario = campos[4],
                                Vip = vip
                            });
                        }
                    }
                }
                return listaClientes;
            }
            catch (Exception ex) { 
                throw new Exception(ex.ToString());
            } 
        }

        public void GuardarCliente(Cliente nuevoCliente, string correo)
        {
            string ruta = Path.Combine(AppContext.BaseDirectory, "Ficheros", "datosClientes.txt");

                if (!ComprobarClienteExistente(correo))
                {

                    listaClientes.Add(nuevoCliente);

                    // Formatear la información del cliente
                    string linea = $"{nuevoCliente.Nombre}#{nuevoCliente.Apellidos}#{nuevoCliente.Ciudad}#{nuevoCliente.Correo}#{nuevoCliente.Comentario}#{nuevoCliente.Vip}";

                    // Agregar la línea al archivo
                    File.AppendAllText(ruta, linea + Environment.NewLine);

                }else
            {
                throw new ClienteExistenteException();
            }
        }

        public void BorrarCliente(String correoBorrar)
        {
            // Buscar cliente por correo
            var cliente = listaClientes.FirstOrDefault(c => c.Correo.Equals(correoBorrar.Trim(), StringComparison.OrdinalIgnoreCase));

            if (cliente != null)
            {
                // Eliminar de la lista
                listaClientes.Remove(cliente);

                // Sobrescribir el archivo sin el cliente borrado
                string ruta = Path.Combine(AppContext.BaseDirectory, "Ficheros", "datosClientes.txt");

                var lineas = listaClientes.Select(c =>
                    $"{c.Nombre}#{c.Apellidos}#{c.Ciudad}#{c.Correo}#{c.Comentario}#{c.Vip}"
                ).ToArray();

                File.WriteAllLines(ruta, lineas);
            }
            else
            {
                throw new Exception("Cliente no encontrado");
            }
        }
        
        public void ModificarCliente(String correoModificar, Cliente clienteActualizar)
        {
            // Buscar cliente por correo
            var cliente = listaClientes.FirstOrDefault(c => c.Correo.Equals(correoModificar.Trim(), StringComparison.OrdinalIgnoreCase));

            if (cliente != null)
            {
                // Actualizar los campos
                cliente.Nombre = clienteActualizar.Nombre;
                cliente.Apellidos = clienteActualizar.Apellidos;
                cliente.Ciudad = clienteActualizar.Ciudad;
                cliente.Correo = clienteActualizar.Correo;
                cliente.Comentario = clienteActualizar.Comentario;
                cliente.Vip = clienteActualizar.Vip;

                // Sobrescribir el archivo con todos los clientes actualizados
                string ruta = Path.Combine(AppContext.BaseDirectory, "Ficheros", "datosClientes.txt");

                var lineas = listaClientes.Select(c =>
                    $"{c.Nombre}#{c.Apellidos}#{c.Ciudad}#{c.Correo}#{c.Comentario}#{c.Vip}"
                ).ToArray();

                File.WriteAllLines(ruta, lineas);
            }
            else
            {
                throw new Exception("Cliente no encontrado");
            }
        }

        public bool ComprobarClienteExistente(string correoNuevo)
        {
            return listaClientes.Any(c => c.Correo.Equals(correoNuevo, StringComparison.OrdinalIgnoreCase));
        }
    }
}
