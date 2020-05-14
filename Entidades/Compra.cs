using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entidades
{
    [Serializable]
    public class Compra
    {
        // Referencia: compra.idcompra
        public System.Int32 Idcompra { get; set; }
        // Referencia: compra.fecha
        public System.DateTime Fecha { get; set; }
        // Referencia: compra.total
        public System.Decimal Total { get; set; }
        // Referencia: compra.descripcion
        public System.String Descripcion { get; set; }
        // Referencia: compra.idproveedor
        public Proveedor Proveedor { get; set; }
        /// <summary>
        /// Solo Lectura
        /// </summary>
        public System.Int32 Idproveedor
        {
            get
            {
                if (Proveedor != null)
                {
                    return Proveedor.Idproveedor;
                }
                else
                {
                    return 0;
                }
            }
        }
        /// <summary>
        /// Solo Lectura
        /// </summary>
        public string DescProveedor
        {
            get
            {
                if (Proveedor != null)
                {
                    return Proveedor.Nombre;
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        // Referencia: compra.idusuario
        public System.Int32 Idusuario { get; set; }
        public Usuario Usuario { get; set; }
        /// <summary>
        /// Solo Lectura
        /// </summary>
        public string DescUsuario
        {
            get
            {
                if (Usuario!=null)
                {
                    return Usuario.NombreUsuario;
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public int IdCaja { get; set; }
        // Referencia: compra.idtipo_compra
        public TipoCompra TipoCompra { get; set; }
        public System.Int32 IdtipoCompra
        {
            get
            {
                if (TipoCompra != null)
                {
                    return TipoCompra.IdtipoCompra;
                }
                else
                {
                    return 0;
                }
            }
        }
        public string DescTipoCompra
        {
            get
            {
                if (TipoCompra != null)
                {
                    return TipoCompra.Descripcion;
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public List<CompraLinea> ListLineaCompra { get; set; }

        public List<FormaPago> ListFormaPago { get; set; }

        public ComprobanteCompra Comprobante { get; set; }
        /// <summary>
        /// Solo Lectura
        /// </summary>
        public int? IdComprobante
        {
            get
            {
                if (Comprobante == null)
                {
                    return null;
                }
                else
                {
                    return Comprobante.Idcomprobante;
                }
            }
        }
        /// <summary>
        /// Solo Lectura. Tipo + numero
        /// </summary>
        public string DescComprobate
        {
            get
            {
                if (Comprobante != null)
                {
                    return Comprobante.TipoComprobante + " - " + Comprobante.Numero;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public Sucursal Sucursal_ { get; set; }
        /// <summary>
        /// Solo Lecutra
        /// </summary>
        public int IdSucursal
        {
            get
            {
                if (Sucursal_ != null)
                {
                    return Sucursal_.IdSucursal;
                }
                else
                {
                    return 0;
                }
            }
        }
        /// Solo Lecutra
        /// </summary>
        public string DescSucursal
        {
            get
            {
                if (Sucursal_ != null)
                {
                    return Sucursal_.Descripcion;
                }
                else
                {
                    return string.Empty;
                }
            }
        }
    }
}
