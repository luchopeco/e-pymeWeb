using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Entidades;

namespace Negocio
{
    public class HelperReflection
    {

        public static Type ObtenerTipoEntidad(string entidad)
        {
            ///cargo las entidades
            Assembly assemblyEntidades = Assembly.Load("Entidades");
            ///busco el tipo de entidad en cuestion
            Type tipoEntidad = assemblyEntidades.GetType("Entidades." + entidad);
            return tipoEntidad;
        }
        /// <summary>
        /// Devuelve una nueva instancia del componente encargado de la funcionalidad de la entidad
        /// </summary>
        /// <param name="entidad"></param>
        /// <returns></returns>
        private static Object obtenerInstanciaComponente(string entidad)
        {
            ///cargo las entidades
            Assembly assemblyEntidades = Assembly.Load("Entidades");
            ///busco el tipo de entidad en cuestion
            Type tipoEntidad = assemblyEntidades.GetType("Entidades." + entidad);
            ///Busco el campo del atributo q me indica el componente q se encarga de su funcionalidad
            string componente = ((ABMClassAttribute)tipoEntidad.GetCustomAttributes(false)[0]).Componente;
            ///Busco el campo del atributo q me indica el namespace donde esta el componente
            string ns = ((ABMClassAttribute)tipoEntidad.GetCustomAttributes(false)[0]).NameSpaceComponente;
           
            ///ahora hago lo mismo pero con el compomente encargado de la funcionalidad
            ///cargo el assembly correspondiente
            Assembly assemblyComponente = Assembly.Load(ns);
            ///busco el tipo de componente  en cuestion
            Type tipoComponente = assemblyComponente.GetType(ns + "." + componente);
            ///instancion el componente en cuestion
            object instanciaComponente = Activator.CreateInstance(tipoComponente);
            return instanciaComponente;
        }
        /// <summary>
        /// Devuelve el nombre del nameSpaces donde se encuetra el comoponente encargado de la 
        /// funcionalidad de la entidad
        /// </summary>
        /// <param name="entidad"></param>
        /// <returns></returns>
        private static string obtenerNameSpaceComponente(string entidad)
        {
            ///cargo las entidades
            Assembly assemblyEntidades = Assembly.Load("Entidades");
            ///busco el tipo de entidad en cuestion
            Type tipoEntidad = assemblyEntidades.GetType("Entidades." + entidad);
            ///Busco el campo del atributo q me indica el metodo q buscar todas las entidades en cuestion
            string metodo = ((ABMClassAttribute)tipoEntidad.GetCustomAttributes(false)[0]).NameSpaceComponente;
            return metodo;
        }
        /// <summary>
        /// Devuelve el nombre del Componente donde se encuetra el comoponente encargado de la 
        /// funcionalidad de la entidad
        /// <param name="entidad"></param>
        /// <returns></returns>
        private static string obtenerComponente(string entidad)
        {
            ///cargo las entidades
            Assembly assemblyEntidades = Assembly.Load("Entidades");
            ///busco el tipo de entidad en cuestion
            Type tipoEntidad = assemblyEntidades.GetType("Entidades." + entidad);
            ///Busco el campo del atributo q me indica el metodo q buscar todas las entidades en cuestion
            string metodo = ((ABMClassAttribute)tipoEntidad.GetCustomAttributes(false)[0]).Componente;
            return metodo;
        }      
       
        /// <summary>
        /// Devuelve el nombre del metodo encargado de buscar todoas las entidades en cuestion
        /// </summary>
        /// <param name="entidad"></param>
        /// <returns></returns>
        public static string ObtenerNombreMetodoBuscarTodos(string entidad)
        {
            ///cargo las entidades
            Assembly assemblyEntidades = Assembly.Load("Entidades");
            ///busco el tipo de entidad en cuestion
            Type tipoEntidad = assemblyEntidades.GetType("Entidades." + entidad);
            ///Busco el campo del atributo q me indica el metodo q buscar todas las entidades en cuestion
            string metodo = ((ABMClassAttribute)tipoEntidad.GetCustomAttributes(false)[0]).MetodoBuscarTodos;
            return metodo;
        }       
        /// <summary>
        /// Devuelve el nombre del metodo encargado de Agregar la entidad en cuestion
        /// </summary>
        /// <param name="entidad"></param>
        /// <returns></returns>
        public static string ObtenerNombreMetodoAlta(string entidad)
        {
            ///cargo las entidades
            Assembly assemblyEntidades = Assembly.Load("Entidades");
            ///busco el tipo de entidad en cuestion
            Type tipoEntidad = assemblyEntidades.GetType("Entidades." + entidad);
            ///Busco el campo del atributo q me indica el metodo q buscar todas las entidades en cuestion
            string metodo = ((ABMClassAttribute)tipoEntidad.GetCustomAttributes(false)[0]).MetodoAlta;
            return metodo;
        }
        /// <summary>
        /// Devuelve el nombre del metodo encargado de Eliminar la entidad en cuestion
        /// </summary>
        /// <param name="entidad"></param>
        /// <returns></returns>
        public static string ObtenerNombreMetodoBaja(string entidad)
        {
            ///cargo las entidades
            Assembly assemblyEntidades = Assembly.Load("Entidades");
            ///busco el tipo de entidad en cuestion
            Type tipoEntidad = assemblyEntidades.GetType("Entidades." + entidad);
            ///Busco el campo del atributo q me indica el metodo q buscar todas las entidades en cuestion
            string metodo = ((ABMClassAttribute)tipoEntidad.GetCustomAttributes(false)[0]).MetodoBaja;
            return metodo;
        }
        /// <summary>
        /// Devuelve el nombre del metodo encargado de Modificar la entidad en cuestion
        /// </summary>
        /// <param name="entidad"></param>
        /// <returns></returns>
        public static string ObtenerNombreMetodoModificar(string entidad)
        {
            ///cargo las entidades
            Assembly assemblyEntidades = Assembly.Load("Entidades");
            ///busco el tipo de entidad en cuestion
            Type tipoEntidad = assemblyEntidades.GetType("Entidades." + entidad);
            ///Busco el campo del atributo q me indica el metodo q buscar todas las entidades en cuestion
            string metodo = ((ABMClassAttribute)tipoEntidad.GetCustomAttributes(false)[0]).MetodoModificar;
            return metodo;
        }
        /// <summary>
        /// Devuelve el nombre del metodo encargado de buscarUna la entidad en cuestion
        /// </summary>
        /// <param name="entidad"></param>
        /// <returns></returns>
        public static string ObtenerNombreMetodoBuscar(string entidad)
        {
            ///cargo las entidades
            Assembly assemblyEntidades = Assembly.Load("Entidades");
            ///busco el tipo de entidad en cuestion
            Type tipoEntidad = assemblyEntidades.GetType("Entidades." + entidad);
            ///Busco el campo del atributo q me indica el metodo q buscar todas las entidades en cuestion
            string metodo = ((ABMClassAttribute)tipoEntidad.GetCustomAttributes(false)[0]).MetodoBuscar;
            return metodo;
        }
        /// <summary>
        /// Obtiene el valor de una propiedad, de una instancia de una entidad
        /// </summary>
        /// <returns></returns>
        public static Object ObtenerValorPropiedad(object instanciaEntidad, PropertyInfo propiedad)
        {
            return instanciaEntidad.GetType().GetProperty(propiedad.Name).GetValue(instanciaEntidad, null);
        }

        /// <summary>
        /// Ejecuta el metodo y devuleve lo que tenga q devolver
        /// </summary>
        /// <param name="entidad"></param>
        /// <param name="nombreMetodo"></param>
        /// <param name="instanciaComponente"></param>
        /// <returns></returns>
        public static Object EjecutarMetodo(string entidad,string nombreMetodo, List<object> parametros)
        {
            
            ///cargo el assembly correspondiente
            Assembly assemblyComponente = Assembly.Load(obtenerNameSpaceComponente(entidad));
            ///busco el tipo de componente  en cuestion
            Type tipoComponente = assemblyComponente.GetType(obtenerNameSpaceComponente(entidad) + "." +obtenerComponente(entidad));
            ///instancion el componente en cuestion
            object instanciaComponente = obtenerInstanciaComponente(entidad);
            Object objeto;
            if (parametros !=null && parametros.Count>0)
            {
                object[] arg = parametros.ToArray();
                objeto = tipoComponente.InvokeMember(nombreMetodo, BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public, null, instanciaComponente, arg);    
            }
            else
            {
                objeto = tipoComponente.InvokeMember(nombreMetodo, BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public, null, instanciaComponente, null);    
            }
            return objeto;
        }
        /// <summary>
        /// Obtiene la lista de propiedades de una entidad
        /// </summary>
        /// <param name="entidad"></param>
        /// <returns></returns>
        public static PropertyInfo[] ObtenerListPropiedades(string entidad)
        {
            ///cargo las entidades
            Assembly assemblyEntidades = Assembly.Load("Entidades");
            ///busco el tipo de entidad en cuestion
            Type tipoEntidad = assemblyEntidades.GetType("Entidades." + entidad);
            PropertyInfo[]  listPropiedades = tipoEntidad.GetProperties();
            return listPropiedades;
        }
        /// <summary>
        /// Ejecuta el metodo indicado en una propiedad y devuleve lo que tenga q devolver.
        /// Este metodo es el q se encarga de llenar el combo
        /// </summary>
        /// <param name="entidad"></param>
        /// <param name="nombreMetodo"></param>
        /// <param name="instanciaComponente"></param>
        /// <returns></returns>
        public static Object EjecutarMetodo(PropertyInfo p)
        {
          
                ///cargo el assembly correspondiente
                Assembly assembly = Assembly.Load(((ABMPropertyAttribute)p.GetCustomAttributes(false)[0]).NameSpace);
                ///busco el tipo de componente  en cuestion
                Type tipoComp = assembly.GetType(((ABMPropertyAttribute)p.GetCustomAttributes(false)[0]).NameSpace + "." + ((ABMPropertyAttribute)p.GetCustomAttributes(false)[0]).Componente);
                ///instancion el componente en cuestion
                object instanciaComp = Activator.CreateInstance(tipoComp);

                return tipoComp.InvokeMember(((ABMPropertyAttribute)p.GetCustomAttributes(false)[0]).MetodoCargaCombo, BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public, null, instanciaComp, null);
            
        }

        public static string[] ObtenerClave(string entidad)
        {
            List<string> listClave = new List<string>();
            PropertyInfo[] listProp = ObtenerListPropiedades(entidad);
            foreach (PropertyInfo p in listProp)
            {
                if (p.GetCustomAttributes(false).Count()>0)
                {
                    if (((ABMPropertyAttribute)p.GetCustomAttributes(false)[0]).EsClave)
                    {
                        listClave.Add(p.Name);
                    } 
                }
                
            }
            return listClave.ToArray();
        }

    }
}
