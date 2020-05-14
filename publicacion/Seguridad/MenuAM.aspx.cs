using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Entidades;
using Negocio;


public partial class MenuAM : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PanelError.Visible = false;
        PanelMensaje.Visible = false;
        PanelPrimerItem.Visible = false;
        if (!IsPostBack)
        {
            if (Request.QueryString["idMenu"] != null)
            {
                try
                {
                    int idMenu = Convert.ToInt32(Request.QueryString["idMenu"]);
                    using (ControladorMenus c_menus = new ControladorMenus())
                    {
                        //im.IditemMenu, im.IdPadre, im.AlInicio, im.DespuesDe, true
                        if (Request.QueryString["iditemMenu"]!=null)
                        {
                            if (Request.QueryString["despuesDe"]=="error")
                            {
                                PanelError.Visible = true;
                                lblError.Text = "No se puede Realizar la operacion";
                            }
                            else
                            {
                                int idItemMenu = Convert.ToInt32(Request.QueryString["iditemMenu"]);
                                int idPadre = Convert.ToInt32(Request.QueryString["idpadre"]);
                                bool alInicio = Convert.ToBoolean(Request.QueryString["alInicio"]);
                                int despuesDe = Convert.ToInt32(Request.QueryString["despuesDe"]);
                                ItemMenu im = new ItemMenu();
                                im.IditemMenu = idItemMenu;
                                im.IdPadre = idPadre;
                                im.AlInicio = alInicio;
                                im.DespuesDe = despuesDe;
                                c_menus.MoverItemMenu(im);
                            }
                            
                        }
                        Menuu m = c_menus.BuscarMenuFormateadoCompleto(idMenu);
                        lblTitulo.Text = "Modificar Menu: " + m.Nombre;                        
                        //StringBuilder oStringBuilder = new StringBuilder();  
                        if (m.ListItems!=null && m.ListItems.Count>0)
                        {
                            PanelModificacion.Visible = true;
                            List<ItemMenu> listI = m.ListItems;
                            string menu = mostrarMenuEnLista(listI);
                            LiteralMenu.Text = menu;
                        }
                        else
                        {
                            PanelModificacion.Visible = false;
                            PanelPrimerItem.Visible = true;
                        }
                    }
                    ///Si estoy recibiendo un mensaje
                    if (Request.QueryString["mensaje"] != null)
                    {
                        PanelMensaje.Visible = true;
                        LabelMensaje.Text = Request.QueryString["mensaje"];
                    }
                }
                catch (ExcepcionPropia ex)
                {
                    PanelError.Visible = true;
                    lblError.Text = ex.Message;
                }
            }
            else
            {
                lblTitulo.Visible = false;
                PanelModificacion.Visible = false;                
            }
        }
    }

    private string mostrarMenuEnLista( List<ItemMenu> listI)
    {
        StringBuilder oStringBuilder = new StringBuilder();
        if (listI != null && listI.Count > 0)
        {
            oStringBuilder.Append("<ul>");
            foreach (ItemMenu i in listI)
            {
                string despuesDeAbajo;
                string despuesDeArriba;
                try
                {
                    despuesDeAbajo = listI[listI.IndexOf(i) + 1].IditemMenu.ToString();
                }
                catch (ArgumentOutOfRangeException)
                {
                    despuesDeAbajo= "error";
                }
                try 
	            {
                    despuesDeArriba = listI[listI.IndexOf(i) - 1].IditemMenu.ToString();
	            }
                catch (ArgumentOutOfRangeException)
	            {

                    despuesDeArriba = "error";
	            }
                
                string line = String.Format(@"<li>");
                oStringBuilder.Append(line);
                oStringBuilder.Append(i.Etiqueta);
                oStringBuilder.Append("  |  ");
                oStringBuilder.Append(string.Format("<a href='ItemMenuAM.aspx?idItemMenu={0}'><i class='fa fa-edit' title='Editar Item'></i></a>",i.IditemMenu));
                oStringBuilder.Append("  |  ");
                oStringBuilder.Append(string.Format("<a href='ItemMenuAM.aspx?idPadre={0}&despuesDe={1}&idMenu={2}'><i class='fa fa-plus-square-o' title='Agregar Item Abajo'></i></a>", i.IdPadre.ToString(), i.IditemMenu.ToString(), i.Idmenu.ToString()));
                oStringBuilder.Append("  |  ");
                oStringBuilder.Append(string.Format("<a href='ItemMenuAM.aspx?idPadre={0}&despuesDe={1}&idMenu={2}'><i class='fa fa-level-down' title='Agregar Sub Menu'></i></a>", i.IditemMenu.ToString(), null, i.Idmenu.ToString()));
                oStringBuilder.Append("  |  ");
                oStringBuilder.Append(string.Format("<a href='MenuAM.aspx?idPadre={0}&despuesDe={1}&idItemMenu={2}&idMenu={3}'><i class='fa fa-sort-asc' title='Mover Rama Abajo'></i></a>", i.IdPadre.ToString(), despuesDeAbajo, i.IditemMenu.ToString(), i.Idmenu.ToString()));
                oStringBuilder.Append("  |  ");
                oStringBuilder.Append(string.Format("<a href='MenuAM.aspx?idPadre={0}&despuesDe={1}&idItemMenu={2}&idMenu={3}'><i class='fa fa-sort-desc' title='Mover Rama Arriba'></i></a>", i.IdPadre.ToString(), despuesDeArriba, i.IditemMenu.ToString(), i.Idmenu.ToString()));
                oStringBuilder.Append("  |  ");
                oStringBuilder.Append(string.Format("<a href='ItemMenuAM.aspx?idItemMenuEliminar={0}'><i class='fa fa-times' title='Eliminar Item'></i></a>",i.IditemMenu.ToString()));
                oStringBuilder.Append("  |  ");                
                oStringBuilder.Append(mostrarMenuEnLista(i.ListItemHijos));
                oStringBuilder.Append("</li>");
            }
            oStringBuilder.Append("</ul>");
        }
        return oStringBuilder.ToString();
    }
    protected void btnAgregarItem_Click(object sender, EventArgs e)
    {
        Response.Redirect(string.Format("ItemMenuAM.aspx?idPadre={0}&despuesDe={1}&idMenu={2}",null, null,Request.QueryString["idMenu"]));
    }
}