
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tienda.Modelos
{
    class Cliente
    {
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Ciudad {  get; set; }
        public string Correo { get; set; }

        public string Comentario { get; set; }
        public bool Vip { get; set; }

        // cambiar True o False por texto bien
        public string VipTexto => Vip ? "Vip" : "No Vip";

        //Nombre + Apellido para mostrar en el ListView
        public string NombreCompleto => $"{Apellidos} {Nombre}";

        public override bool Equals(object? obj)
        {
            return obj is Cliente cliente &&
                   Correo == cliente.Correo;
        }
    }
}
