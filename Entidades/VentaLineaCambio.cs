using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entidades
{
    [Serializable]
    public class VentaLineaCambio
    {
        public int IdVenta { get; set; }
        public Articulo Articulo { get; set; }
        /// <summary>
        /// Solo Lectura
        /// </summary>
        public int IdArticulo
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
        /// <summary>
        /// Solo Lectura
        /// </summary>
        public string DescArticulo
        {
            get
            {
                if (Articulo != null)
                {
                    return Articulo.DescripcionCompleta;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public Articulo ArticuloAnterior { get; set; }
        /// <summary>
        /// Solo Lectura
        /// </summary>
        public int IdArticuloAnterior
        {
            get
            {
                if (ArticuloAnterior != null)
                {
                    return ArticuloAnterior.Idarticulo;
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
        public string DescArticuloAnterior
        {
            get
            {
                if (ArticuloAnterior != null)
                {
                    return ArticuloAnterior.DescripcionCompleta;
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public int Cantidad { get; set; }
        public int IdUsuario { get; set; }
        public DateTime FechaCambio { get; set; }
    }
}
