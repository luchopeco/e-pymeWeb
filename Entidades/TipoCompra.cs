using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entidades
{
    [Serializable]
    [ABMClass("Negocio", "ControladorCompras", "BuscarListTipoCompras", "ModificarTipoCompra",
        "EliminarTipoCompra", "AgregarTipoCompra", "BuscarTipoCompra","Tipos de Compras")]
   public class TipoCompra
    {
        // Referencia: tipo_compra.idtipo_compra
       [ABMProperty(false,true,false)]
        public System.Int32 IdtipoCompra { get; set; }
        // Referencia: tipo_compra.descripcion
        public System.String Descripcion { get; set; }
        // Referencia: tipo_compra.genera_cargo
        public System.Boolean GeneraCargo { get; set; }
        // Referencia: tipo_compra.fecha_baja
        public System.DateTime? FechaBaja { get; set; }
    }
}
