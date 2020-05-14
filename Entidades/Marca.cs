using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entidades
{
    [Serializable]
    [ABMClass("Negocio", "ControladorMarcas", "BuscarListMarcaTodas", "ModificarMarca", "EliminarMarca", "AgregarMarca", "BuscarMarca", "Marcas")]
    public class Marca
    {
        // Referencia: marca.idmarca
        [ABMProperty(false, true, false)]
        public System.Int32 Idmarca { get; set; }
        // Referencia: marca.descripcion
        public System.String Descripcion { get; set; }
        // Referencia: marca.fecha_baja
        public System.DateTime? FechaBaja { get; set; }
    }
}
