using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Entidades
{
    public class MovimientosArticulos
    {
        public int IdMovimiento { get; set; }
        public Sucursal SucursalDesde { get; set; }
        /// <summary>
        /// Solo Lecutra
        /// </summary>
        public int IdSucursalDesde
        {
            get
            {
                if (SucursalDesde != null)
                {
                    return SucursalDesde.IdSucursal;
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
        public string DescSucursalDesde
        {
            get
            {
                if (SucursalDesde != null)
                {
                    return SucursalDesde.Descripcion;
                }
                else
                {
                    return "";
                }
            }
        }
        public Sucursal SucursalHasta { get; set; }
        /// <summary>
        /// Solo Lecutra
        /// </summary>
        public int? IdSucursalHasta
        {
            get
            {
                if (SucursalHasta != null)
                {
                    return SucursalHasta.IdSucursal;
                }
                else
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// Solo Lectura
        /// </summary>
        public string DescSucursalHasta
        {
            get
            {
                if (SucursalHasta != null)
                {
                    return SucursalHasta.Descripcion;
                }
                else
                {
                    return "";
                }
            }
        }

        public TipoMovimientoArticulo TipoMovimiento { get; set; }
        /// <summary>
        /// Solo Lectra
        /// </summary>
        public int? IdTipoMovimiento
        {
            get
            {
                if (TipoMovimiento != null)
                {
                    return TipoMovimiento.IdTipoMovimiento;
                }
                else
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// Solo Lectura
        /// </summary>
        public string DescTipoMovimiento
        {
            get
            {
                if (TipoMovimiento != null)
                {
                    return TipoMovimiento.Descripcion;
                }
                else
                {
                    return "";
                }
            }
        }
        public Articulo Articulo_ { get; set; }
        /// <summary>
        /// Solo Lecutra
        /// </summary>
        public int IdArticulo_
        {
            get
            {
                if (Articulo_ != null)
                {
                    return Articulo_.Idarticulo;
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
        public string DescArticulo
        {
            get
            {
                if (Articulo_ != null)
                {
                    return Articulo_.DescripcionCompleta;
                }
                else
                {
                    return "";
                }
            }
        }

        public int Cantidad { get; set; }
        public DateTime Fecha { get; set; }
        public int IdUsuario { get; set; }

        public string Observacion { get; set; }
    }
}
