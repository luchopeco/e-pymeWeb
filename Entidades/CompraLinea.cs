using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entidades
{
    [Serializable]
    public class CompraLinea
    {
        // Referencia: compra_detalle.idcompra
        public System.Int32 Idcompra { get; set; }
        // Referencia: compra_detalle.idarticulo
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
        // Referencia: compra_detalle.costo_unitario
        public System.Decimal CostoUnitario { get; set; }
        // Referencia: compra_detalle.cantidad
        public System.Int32 Cantidad { get; set; }
        // Referencia: compra_detalle.idusuario_baja
        public System.Int32? IdusuarioBaja { get; set; }
        // Referencia: compra_detalle.fecha_baja
        public System.DateTime? FechaBaja { get; set; }
        /// <summary>
        /// Solo Lectura
        /// </summary>
        public bool DadoBaja {
            get
            {
                if (FechaBaja ==null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// Solo Lectura
        /// </summary>
        public string DescArticulo { get { return Articulo.DescripcionCompleta; } }
        /// <summary>
        /// Solo lectura. cantidad por costo Unitario
        /// </summary>
        public decimal Subtotal { get { return Cantidad * CostoUnitario; } }
        /// <summary>
        /// Solo Lectura
        /// </summary>
        public decimal PrecioVenta { get { return Articulo.Precio; } }
    }
}
