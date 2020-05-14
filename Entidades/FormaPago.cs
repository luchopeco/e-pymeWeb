using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entidades
{
    [Serializable]
    [ABMClass("Negocio", "ControladorFormaPago", "BuscarListFormaPago", "ModificarFormaPago",
        "EliminarFormaPago", "AgregarFormaPago", "BuscarFormaPago", "Formas de Pago")]
    public class FormaPago
    {
        // Referencia: tipo_forma_pago.idtipo_forma_pago
        [ABMProperty(false, true, false)]
        public System.Int32 IdtipoFormaPago { get; set; }
        // Referencia: tipo_forma_pago.descripcion
        public System.String Descripcion { get; set; }
        // Referencia: tipo_forma_pago.habilitado_venta
        public System.Boolean HabilitadoVenta { get; set; }
        // Referencia: tipo_forma_pago.habilitado_compra
        public System.Boolean HabilitadoCompra { get; set; }
        // Referencia: tipo_forma_pago.habilitado_gasto
        public System.Boolean HabilitadoGasto { get; set; }
        public bool AceptaNotaCredito { get; set; }
        public bool EsEfectivo { get; set; }

        /// <summary>
        /// Propiedad cuando se utiliza la Forma de pago como tal,
        /// y no como Tipo de forma de pago
        /// </summary>
        [ABMProperty(false, false, false)]
        public decimal Monto { get; set; }
        /// <summary>
        /// Propiedad cuando se utiliza la Forma de pago como tal,
        /// y no como Tipo de forma de pago
        /// </summary>
        [ABMProperty(false, false, false)]
        public NotaCredito NotaCredito { get; set; }
        /// <summary>
        /// Solo Lectura
        /// </summary>
        [ABMProperty(false, false, false)]
        public int? IdNotaCredito
        {
            get
            {
                if (NotaCredito != null)
                {
                    return NotaCredito.IdnotaCredito;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
