using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entidades
{
    [Serializable]
    [ABMClass("Negocio", "ControladorArticulos", "BuscarListTipoArticuloTodos", "ModificarTipoArticulo", "EliminarTipoArticulo", "AgregarTipoArticulo", "BuscarTipoArticulo", "Tipos de Articulos")]
    public class TipoArticulo
    {
        // Referencia: tipo_articulo.idtipo_articulo
        [ABMProperty(false, true, false)]
        public System.Int32 IdtipoArticulo { get; set; }
        // Referencia: tipo_articulo.descripcion
        public System.String Descripcion { get; set; }
        // Referencia: tipo_articulo.fecha_baja
        public System.DateTime? FechaBaja { get; set; }
    }
}
