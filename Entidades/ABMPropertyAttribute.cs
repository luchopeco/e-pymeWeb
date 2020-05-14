using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entidades
{
    /// <summary>
    /// Atributo que utilizo para indicar a propiedades de entidades
    /// caracterisitcas si es utilizado el abmGenerico
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ABMPropertyAttribute : System.Attribute
    {
        /// <summary>
        /// Me indica si la propiedad es la clave
        /// </summary>
        public bool EsClave;
        /// <summary>
        /// Me indica si mostrar o no la propiedad
        /// </summary>
        public bool MostrarSiempre;
        /// <summary>
        /// Me indica si la propiedad se mapea con un combobox
        /// </summary>
        public bool EsComboBox;
        /// <summary>
        /// Me indica el NameSpace del metodo q completa el comboBox
        /// </summary>
        public string NameSpace;
        /// <summary>
        /// Me indica el componente del metodo q completa el comboBox
        /// </summary>
        public string Componente;
        /// <summary>
        /// Me indica el motodo q completa el comboBox
        /// </summary>
        public string MetodoCargaCombo;
        /// <summary>
        /// Me indica el dataValueField del comboBox
        /// </summary>
        public string DataValueField;
        /// <summary>
        /// Me indica el datatextField de combobox
        /// </summary>
        public string DataTextField;
        /// <summary>
        /// Me indica si la propiedad es no nula
        /// </summary>
        public bool NoNulo;
        public bool MostrarEnGrilla;        
        /// <summary>
        /// Me indica cual seria el selected value en caso de q la propiedad q se carga en un combo
        /// fuera del tipo entidad.
        /// </summary>
        public string SelectedValue;


        /// <summary>
        /// Constructor a usar si la propiedad no se debe mostrar
        /// Me indica que propiedades no mostrar en el ABM
        /// </summary>
        public ABMPropertyAttribute(bool mostrarSiempre, bool esClave, bool mostrarEnGrilla)
        {
            MostrarSiempre = mostrarSiempre;
            EsComboBox = false;
            EsClave = esClave;
            MostrarEnGrilla = mostrarEnGrilla;
        }
        /// <summary>
        /// Constructor a usar si la propiedad Se mapea a un comboBox
        /// Ademas me indica si la propiedad es obligatoria o no
        /// </summary>
        public ABMPropertyAttribute(string nameSpace, string componente, string metodoCargaCombo,
         string dataValueField, string dataTextField, string selectedValue, bool noNulo)
        {
            MostrarSiempre = true;
            EsComboBox = true;
            NameSpace = nameSpace;
            Componente = componente;
            MetodoCargaCombo = metodoCargaCombo;
            DataValueField = dataValueField;
            DataTextField = dataTextField;
            NoNulo = noNulo;
            MostrarEnGrilla = false;
            SelectedValue = selectedValue;
        }
        /// <summary>
        /// Constructor a usar si la propiedad Se muestra solo en la grilla.
        /// Util poe ejemplo cuiando la propiedad es de tipo entidad, y solo quiero mostrar alguna 
        /// descripcion
        /// </summary>
        public ABMPropertyAttribute(bool mostrarSoloEnGrilla)
        {
            MostrarEnGrilla = mostrarSoloEnGrilla;
            MostrarSiempre = false;
        }
    }
}
