using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entidades
{
    [Serializable]
    public class Comprobante
    {
        // Referencia: comprobante.idcomprobante
        public System.Int32 Idcomprobante { get; set; }
        // Referencia: comprobante.numero
        public System.String Numero { get; set; }
        // Referencia: comprobante.tipo_comprobante
        public System.String TipoComprobante { get; set; }
        // Referencia: comprobante.fecha
        public System.DateTime Fecha { get; set; }
        // Referencia: comprobante.monto
        public System.Decimal Monto { get; set; }
    }
}
