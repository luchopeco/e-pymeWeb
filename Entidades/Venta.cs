using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entidades
{
    [Serializable]
    public class Venta
    {
        // Referencia: venta.idventa
        public System.Int32 Idventa { get; set; }
        // Referencia: venta.descripcion
        public System.String Descripcion { get; set; }
        public System.String DescripcionArticulos
        {
            get
            {
                string desc = "";
                if (ListLineaVenta != null && ListLineaVenta.Count > 0)
                {
                    foreach (var lv in ListLineaVenta)
                    {
                        desc = desc + lv.Articulo.DescripcionCompleta + "-";
                    }
                    int cant = desc.Count();
                    string des = desc.Remove(cant - 1);
                    if (desc.Count() > 100)
                    {
                        return desc.Remove(100);
                    }
                    return des;
                }
                return "";
            }
        }
        // Referencia: venta.fecha
        public System.DateTime Fecha { get; set; }
        // Referencia: venta.total
        public System.Decimal Total { get; set; }
        public Cliente Cliente { set; get; }
        /// <summary>
        /// Solo Lectura
        /// </summary>
        public System.Int32? Idcliente
        {
            get
            {
                if (Cliente != null)
                {
                    return Cliente.Idcliente;
                }
                else
                {
                    return null;
                }
            }
        }
        public string DescCliente
        {
            get
            {
                if (Cliente != null)
                {
                    return Cliente.NombreApellido;
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        // Referencia: venta.idusuario
        public System.Int32 Idusuario { get; set; }
        public Usuario Usuario { get; set; }
        /// <summary>
        /// Solo Lectura
        /// </summary>
        public string DescUsuario
        {
            get
            {
                if (Usuario != null)
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

        public List<VentaLinea> ListLineaVenta { get; set; }
        public List<FormaPago> ListFormaPago { get; set; }

        public Comprobante ComprobanteVenta { get; set; }
        public string NumeroComprobante
        {
            get
            {
                if (ComprobanteVenta != null)
                {
                    return ComprobanteVenta.Numero;
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
        /// <summary>
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
