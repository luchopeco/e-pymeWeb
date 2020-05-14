using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entidades
{
    [Serializable]
    public class NotaCredito
    {
        // Referencia: nota_credito.idnota_credito
        public System.Int32 IdnotaCredito { get; set; }
        // Referencia: nota_credito.fecha
        public System.DateTime Fecha { get; set; }
        // Referencia: nota_credito.monto
        public System.Decimal Monto { get; set; }
        // Referencia: nota_credito.descripcion
        public System.String Descripcion { get; set; }
        // Referencia: nota_credito.fecha_vto
        public System.DateTime? FechaVto { get; set; }
        // Referencia: nota_credito.idusuario
        public System.Int32 Idusuario { get; set; }
        // Referencia: nota_credito.numero
        public System.Int32 Numero { get; set; }
        /// <summary>
        /// Ojo no se trae de la DB
        /// </summary>
        public bool UtilizadaEnVenta { get; set; }

        public List<VentaLinea> ListLineasVentaDevueltas { get; set; }
    }
}
