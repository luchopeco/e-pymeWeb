using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entidades
{
    public class ItemMenu
    {
        // Referencia: items_menus.iditem_menu
        public System.Int32 IditemMenu { get; set; }
        // Referencia: items_menus.idmenu
        public System.Int32 Idmenu { get; set; }
        public Pagina Pagina { get; set; }
        // Referencia: items_menus.idpagina
        /// <summary>
        /// Solo Lectura
        /// </summary>
        public System.Int32? Idpagina
        {
            get
            {
                if (Pagina!=null)
                {
                    return Pagina.Idpagina;
                }
                else
                {
                    return null;
                }
            }
        }
        // Referencia: items_menus.etiqueta
        public System.String Etiqueta { get; set; }
        // Referencia: items_menus.es_division
        public System.Boolean EsDivision { get; set; }

        /// <summary>
        /// Propiedad que se utiliza para generar el menu
        /// </summary>
        public int? IdPadre { get; set; }
        /// <summary>
        /// Propiedad que se utiliza para generar el menu
        /// </summary>
        public bool AlInicio {get;set;}
        /// <summary>
        /// Propiedad que se utiliza para generar el menu
        /// </summary>
        public int? DespuesDe  {get;set;}

        /// <summary>
        /// Cuando Traigo el menu formateado completo los hijos de cada item si es q tiene
        /// </summary>
        public List<ItemMenu> ListItemHijos { get; set; }
    }
}
