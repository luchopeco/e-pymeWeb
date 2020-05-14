using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Entidades
{
    /// <summary>
    /// Atributo que utilizo para indicar a entidades
    /// caracterisitcas si es utilizado en el abmGenerico
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ABMClassAttribute:System.Attribute
    {
        /// <summary>
        /// Entidad en cuestion
        /// </summary>
        public string Entidad;
        /// <summary>
        /// Me indica el nameSpace donde se encuentra el componente que maneja esta entidad
        /// </summary>
        public string NameSpaceComponente;
        /// <summary>
        /// Me indica que comoponente se encarga de manejar esta entidad
        /// </summary>
        public string Componente;
        /// <summary>
        /// Me indica el metodo buscar todos
        /// </summary>
        public string MetodoBuscarTodos;
        /// <summary>
        /// Me indica el metodo modificar
        /// </summary>
        public string MetodoModificar;
        /// <summary>
        /// Me indica el metodo baja
        /// </summary>
        public string MetodoBaja;
        /// <summary>
        /// Me indica el metodo Alta
        /// </summary>
        public string MetodoAlta;
        /// <summary>
        /// Me indica metodo q devuelve un modelo de lista de la entidad con una entidad vacia
        /// </summary>
        //public string MetodoListaModelo;
        /// <summary>
        /// Me indica el titulo a utilizar en lugar del nombre de la entidad
        /// </summary>
        public string Titulo;
        /// <summary>
        /// Me indica el namespace de la entidad
        /// </summary>
        public static string NameSpaceEntidad = "Sico.Negocio.Entidades.";
        /// <summary>
        /// Me indica el metodo buscar 
        /// </summary>
        public string MetodoBuscar;

        public ABMClassAttribute(string nameSpace, string componente, string metodoBuscarTodos,
                            string metodoModificar, string metodoBaja, string metodoAlta, string metodoBuscar, string titulo)
        {            
            this.NameSpaceComponente = nameSpace;
            this.Componente = componente;
            this.MetodoBuscarTodos = metodoBuscarTodos;
            this.MetodoModificar = metodoModificar;
            MetodoBaja = metodoBaja;
            MetodoAlta = metodoAlta;
            MetodoBuscar = metodoBuscar;
            Titulo = titulo;          
        }        

    }

    //public enum TipoCampo
    //{ 
    //    Boolean,
    //    String,
    //    Text,
    //    Numerico,
    //    Decimal,
    //    Lista
    //}


    //[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    //public class ABMField : Attribute
    //{

    //    public string MascaraValidaciion { get; set; }
    //    public bool Requerido { get; set; }


    //    [Obsolete("No usar mas")]
    //    public ABMField()
    //    {

    //    }

    //    public ABMField(string x)
    //    {

    //    }
    //}
}
