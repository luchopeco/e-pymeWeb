using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.Script.Serialization;
using AjaxControlToolkit;
using Entidades;
using Negocio;

/// <summary>
/// Descripción breve de Autocomplete
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// Para permitir que se llame a este servicio Web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
[System.Web.Script.Services.ScriptService]
public class Autocomplete : System.Web.Services.WebService
{

    [WebMethod]
    [ScriptMethod]
    public string[] BuscarClientes(string prefixText)
    {
        List<string> items = new List<string>(50);
        List<Persona> listP;
        List<Cliente> listC;
        try
        {
            listC = Cliente.BuscarLikeNombreApellido(prefixText);
            if (listC != null)
            {
                items.AddRange(listC.Select(c => AutoCompleteExtender.CreateAutoCompleteItem(c.Apellido + ", " + c.Nombre, c.Idcliente.ToString())));
            }

        }
        catch (ExcepcionPropia)
        {
            //items.Add("ASDASD");

        }
        return items.ToArray();
    }
    [WebMethod]
    [ScriptMethod]
    public string[] BuscarProveedores(string prefixText)
    {
        List<string> items = new List<string>(50);
        List<Proveedor> listP;
        try
        {
            using (ControladorCompras c_compras = new ControladorCompras())
            {
                listP = c_compras.BuscarListProveedores(prefixText);
                if (listP != null)
                {
                    foreach (Proveedor p in listP)
                    {
                        // items.Add(p.Nombre);
                        items.Add(AutoCompleteExtender.CreateAutoCompleteItem(p.Nombre, p.Idproveedor.ToString()));
                    }
                }

            }
        }
        catch (ExcepcionPropia)
        {
            //items.Add("ASDASD");

        }
        return items.ToArray();
    }

    [WebMethod]
    [ScriptMethod]
    public string[] BuscarArticulosVenta(string prefixText, string contextKey)
    {
        List<string> items = new List<string>(50);
        List<Articulo> listA;
        try
        {
            int idSucu = Convert.ToInt32(contextKey);
            using (ControladorArticulos c_artculos = new ControladorArticulos())
            {
                listA = c_artculos.BuscarListArticuloVenta(prefixText, idSucu);
            }
            if (listA != null)
            {
                foreach (Articulo a in listA)
                {
                    //items.Add(a.TipoArticulo.Descripcion+"-"+a.Descripcion+"-"+a.Marca.Descripcion);
                    items.Add(AutoCompleteExtender.CreateAutoCompleteItem(a.TipoArticulo.Descripcion + "-" + a.Marca.Descripcion + "-" + a.Descripcion, a.Idarticulo.ToString()));
                }
            }
        }
        catch (ExcepcionPropia)
        {
            // items.Add("ASDASD");

        }
        catch (NullReferenceException)
        {
            // items.Add("ASDASD");

        }
        return items.ToArray();
    }



    [WebMethod]
    [ScriptMethod]
    public string[] BuscarArticulosCompra(string prefixText)
    {
        List<string> items = new List<string>(50);
        List<Articulo> listA;
        try
        {
            using (ControladorArticulos c_artculos = new ControladorArticulos())
            {
                listA = c_artculos.BuscarListArticuloCompra(prefixText);
                if (listA != null)
                {
                    foreach (Articulo a in listA)
                    {
                        //items.Add(a.TipoArticulo.Descripcion+"-"+a.Descripcion+"-"+a.Marca.Descripcion);
                        items.Add(AutoCompleteExtender.CreateAutoCompleteItem(a.TipoArticulo.Descripcion + "-" + a.Marca.Descripcion + "-" + a.Descripcion, a.Idarticulo.ToString()));
                    }
                }

            }
        }
        catch (ExcepcionPropia)
        {
            // items.Add("ASDASD");

        }
        catch (NullReferenceException)
        {
            // items.Add("ASDASD");

        }
        return items.ToArray();
    }

}
