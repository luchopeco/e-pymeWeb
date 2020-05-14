using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entidades
{
    [Serializable]
    public class Gasto
    {
        public int IdGasto { get; set; }
        public string Descripcion { get; set; }
        public decimal Monto { get; set; }
        public List<FormaPago> ListFormaPago { get; set; }
        public TipoGasto TipoGasto { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime? FechaAnulado { get; set; }
        /// <summary>
        /// Solo Lectura
        /// </summary>
        public string DescFormaPago
        {
            get
            {
                if (ListFormaPago != null && ListFormaPago.Count > 0)
                {
                    return ListFormaPago[0].Descripcion;
                }
                else
                {
                    return string.Empty;
                }
            }

        }
        /// <summary>
        /// Solo Lectura
        /// </summary>
        public string DescTipoGasto
        {
            get
            {
                if (TipoGasto != null)
                {
                    return TipoGasto.Descripcion;
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        /// <summary>
        /// Solo Lectura
        /// </summary>
        public bool Anulado
        {
            get
            {
                if (FechaAnulado == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        public int IdCaja { get; set; }

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
