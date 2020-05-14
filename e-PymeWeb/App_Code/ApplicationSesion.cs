using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Entidades;

/// <summary>
/// Descripción breve de ApplicationSesion
/// </summary>
public class ApplicationSesion
{
	public ApplicationSesion()
	{
		//
		// TODO: Agregar aquí la lógica del constructor
		//
	}

    public static Usuario ActiveUser
    {
        get
            {
                if (HttpContext.Current.Session["activeUser"] == null)
                    return null;
                else
                    return (Usuario)HttpContext.Current.Session["activeUser"];
            }
            set
            {
                if (value == null)
                    HttpContext.Current.Session.Remove("activeUser");
                else
                    HttpContext.Current.Session["activeUser"] = value;
            }
    }
    public static Sucursal ActiveSucursal
    {
        get
        {
            if (HttpContext.Current.Session["ActiveSucursal"] == null)
                return null;
            else
                return (Sucursal)HttpContext.Current.Session["ActiveSucursal"];
        }
        set
        {
            if (value == null)
                HttpContext.Current.Session.Remove("ActiveSucursal");
            else
                HttpContext.Current.Session["ActiveSucursal"] = value;
        }
    }
    public static Caja ActiveCaja
    {
        get
        {
            if (HttpContext.Current.Session["ActiveCaja"] == null)
                return null;
            else
                return (Caja)HttpContext.Current.Session["ActiveCaja"];
        }
        set
        {
            if (value == null)
                HttpContext.Current.Session.Remove("ActiveCaja");
            else
                HttpContext.Current.Session["ActiveCaja"] = value;
        }
    }

}