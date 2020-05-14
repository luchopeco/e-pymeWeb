using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entidades
{
    [Serializable]
    [ABMClass("Negocio", "ControladorCompras", "BuscarListProveedores", "ModificarProveedor", "EliminarProveedor", "AgregarProveedor",
        "BuscarProveedor","Proveedores")]
    public class Proveedor
    {
        // Referencia: proveedor.idproveedor
        [ABMProperty(false, true, false)]
        public System.Int32 Idproveedor { get; set; }
        // Referencia: proveedor.nombre
        public System.String Nombre { get; set; }
        // Referencia: proveedor.direccion
        public System.String Direccion { get; set; }
        // Referencia: proveedor.telefonos
        public System.String Telefonos { get; set; }

        public string Mail { get; set; }
        // Referencia: proveedor.observaciones
        public System.String Observaciones { get; set; }
        public DateTime? FechaBaja { get; set; }
    }
}
