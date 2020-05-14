using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entidades
{
    [Serializable]
    public class VentaLinea
    {
        // Referencia: venta_detalle.idventa
        public System.Int32 Idventa { get; set; }
        public Articulo Articulo { get; set; }
        /// <summary>
        /// Solo Lectura
        /// </summary>
        public System.Int32 Idarticulo
        {
            get
            {
                if (Articulo != null)
                {
                    return Articulo.Idarticulo;
                }
                else
                {
                    return 0;
                }
            }
        }
        // Referencia: venta_detalle.precio_unitario
        public System.Decimal PrecioUnitario { get; set; }
        // Referencia: venta_detalle.cantidad
        public System.Int32 Cantidad { get; set; }
        // Referencia: venta_detalle.fecha_baja
        public System.DateTime? FechaBaja { get; set; }
        // Referencia: venta_detalle.idusuario_baja
        public System.Int32? IdusuarioBaja { get; set; }

        /// <summary>
        /// Solo lectura. cantidad por precio venta
        /// </summary>
        public decimal Subtotal { get { return Cantidad * PrecioUnitario; } }
        /// <summary>
        /// Solo Lectura
        /// </summary>
        public string DescArticulo { get { return Articulo.DescripcionCompleta; } }

        /// <summary>
        /// Solo Lectura
        /// </summary>
        public bool DadoBaja
        {
            get
            {
                if (FechaBaja == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public NotaCredito NotaCredito { get; set; }
        /// <summary>
        /// Solo Lectura. Si tiene nota Credito fue devuelto
        /// </summary>
        public bool Devuelto
        {
            get
            {
                if (NotaCredito != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        /// <summary>
        /// Contiene informacion de articulos anteriores a los cambios
        /// </summary>
        public VentaLineaCambio ArticuloCambiado { get; set; }

    }
}
