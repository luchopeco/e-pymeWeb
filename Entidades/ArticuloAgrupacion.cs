using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entidades
{
    /// <summary>
    /// Entidad q Agrupa varios articulos, me sirve para los cambios Directos
    /// </summary>
    [Serializable]
    [ABMClass("Negocio", "ControladorArticulos", "BuscarListArticuloAgrupacionTodos", "ModificarArticuloAgrupacion", "baja", "AgregarArticuloAgrupacion", "BuscarArticuloAgrupacion", "Articulo Agrupacion")]
    public class ArticuloAgrupacion
    {
        // Referencia: articulo_agrupacion.idagrupacion_articulo
        [ABMProperty(false, true, false)]
        public System.Int32 IdagrupacionArticulo { get; set; }

        /// <summary>
        /// Solo Lectura
        /// </summary>
        [ABMProperty(false, false, true)]
        public string DescripcionCompleta
        {
            get
            {
                if (Marca != null && TipoArticulo != null)
                {
                    return TipoArticulo.Descripcion + "-" + Marca.Descripcion + "-" + Descripcion;
                }
                else
                {
                    return string.Empty;
                }

            }
        }

        // Referencia: articulo_agrupacion.descripcion
        public System.String Descripcion { get; set; }
        [ABMProperty("Negocio", "ControladorMarcas", "BuscarListMarca", "Idmarca", "Descripcion", "Idmarca", true)]
        public Marca Marca { get; set; }
        /// <summary>
        /// Solo lectura
        /// </summary>
        [ABMProperty(false, false, false)]
        public System.Int32 Idmarca
        {
            get
            {
                if (Marca != null)
                {
                    return Marca.Idmarca;
                }
                else
                {
                    return 0;
                }
            }
        }
        [ABMProperty("Negocio", "ControladorArticulos", "BuscarListTipoArticulo", "IdtipoArticulo", "Descripcion", "IdtipoArticulo", true)]
        public TipoArticulo TipoArticulo { get; set; }
        /// <summary>
        /// Solo Lectura
        /// </summary>
        [ABMProperty(false, false, false)]
        public System.Int32 IdtipoArticulo
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
        // Referencia: articulo_agrupacion.fecha_alta
        public System.DateTime FechaAlta { get; set; }
        // Referencia: articulo_agrupacion.fecha_baja
        public System.DateTime? FechaBaja { get; set; }
      
    }
}
