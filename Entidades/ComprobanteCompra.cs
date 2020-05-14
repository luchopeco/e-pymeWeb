using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entidades
{
    [Serializable]
    public class ComprobanteCompra
    {
        // Referencia: comprobante_compra.idcomprobante
        public System.Int32 Idcomprobante { get; set; }
        // Referencia: comprobante_compra.numero
        public System.String Numero { get; set; }

        public Proveedor Proveedor { get; set; }
        /// <summary>
        /// Solo Lectura
        /// </summary>
        public System.Int32 Idproveedor
        {
            get
            {
                if (Proveedor == null)
                {
                    return 0;
                }
                else
                {
                    return Proveedor.Idproveedor;
                }
            }
        }
        // Referencia: comprobante_compra.fecha
        public System.DateTime Fecha { get; set; }
        // Referencia: comprobante_compra.monto
        public System.Decimal Monto { get; set; }
        // Referencia: comprobante_compra.tipo_comprobante
        public System.String TipoComprobante { get; set; }
    }
}
