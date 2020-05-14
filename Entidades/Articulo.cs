using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entidades
{
    [Serializable]
    [ABMClass("Negocio", "ControladorArticulos", "BuscarListArticulo", "ModificarArticulo", "baja", "AgregarArticulo", "BuscarArticulo", "Articulo")]
    public class Articulo
    {
        // Referencia: articulo.idarticulo
        [ABMProperty(false, true, false)]
        public System.Int32 Idarticulo { get; set; }

        /// <summary>
        /// Solo Lectura. Tipo articulo+Descripcion+Marca
        /// </summary>
        [ABMProperty(false, false, true)]
        public string DescripcionCompleta
        {
            get { return TipoArticulo.Descripcion + " " + Marca.Descripcion + " " + Descripcion; }
        }

        // Referencia: articulo.descripcion
        public System.String Descripcion { get; set; }


        // Referencia: articulo.idmarca
        [ABMProperty("Negocio", "ControladorMarcas", "BuscarListMarca", "Idmarca", "Descripcion", "Idmarca", true)]
        public Marca Marca { get; set; }
        /// <summary>
        /// Solo Lectura
        /// </summary>
        [ABMProperty(false, false, false)]
        public System.Int32 Idmarca
        {
            get
            {
                if (Marca == null)
                {
                    return 0;
                }
                else
                {
                    return Marca.Idmarca;
                }
            }
        }
        [ABMProperty("Negocio", "ControladorArticulos", "BuscarListTipoArticulo", "IdtipoArticulo", "Descripcion", "Idtipoarticulo", true)]
        public TipoArticulo TipoArticulo { get; set; }
        /// <summary>
        /// Solo Lectura
        /// </summary>
        [ABMProperty(false, false, false)]
        public System.Int32 Idtipoarticulo
        {
            get
            {
                if (TipoArticulo != null)
                {
                    return TipoArticulo.IdtipoArticulo;
                }
                else
                {
                    return 0;
                }
            }
        }
        // Referencia: articulo.fecha_alta
        [ABMProperty(false, false, false)]
        public System.DateTime FechaAlta { get; set; }

        // Referencia: articulo.controlar_stock
        public System.Boolean ControlarStock { get; set; }
        // Referencia: articulo.codigo
        public System.String Codigo { get; set; }
        // Referencia: articulo.fecha_baja
        public System.DateTime? FechaBaja { get; set; }


        [ABMProperty("Negocio", "ControladorArticulos", "BuscarListArticuloAgrupacion", "IdagrupacionArticulo", "DescripcionCompleta", "IdagrupacionArticulo", false)]
        public ArticuloAgrupacion AgrupacionArticulo { get; set; }

        /// <summary>
        /// Solo Lectura
        /// </summary>
        [ABMProperty(false, false, false)]
        public int? IdagrupacionArticulo
        {
            get
            {
                if (AgrupacionArticulo != null)
                {
                    return AgrupacionArticulo.IdagrupacionArticulo;
                }
                else
                {
                    return null;
                }
            }
        }

        //a partir de aca son articulos por sucursal

        [ABMProperty(false, false, false)]
        public int IdSucursal { get; set; }

        [ABMProperty(false, false, false)]
        public System.Decimal CostoUltimo { get; set; }
        [ABMProperty(false, false, false)]
        public System.Decimal Precio { get; set; }
        [ABMProperty(false, false, false)]
        public int Stock { get; set; }
    }
}
