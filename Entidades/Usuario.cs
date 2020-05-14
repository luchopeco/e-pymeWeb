using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entidades
{
    [Serializable]
    public class Usuario
    {
        // Referencia: usuario.idusuario
        public System.Int32 Idusuario { get; set; }
        // Referencia: usuario.nombre_usuario
        public System.String NombreUsuario { get; set; }
        // Referencia: usuario.nombre_apellido
        public System.String NombreApellido { get; set; }
        // Referencia: usuario.clave
        public System.String Clave { get; set; }
        // Referencia: usuario.es_super_usuario
        public System.Boolean EsSuperUsuario { get; set; }
        // Referencia: usuario.fecha_baja
        public System.DateTime? FechaBaja { get; set; }
        public string Imagen { get; set; }
    }
}
