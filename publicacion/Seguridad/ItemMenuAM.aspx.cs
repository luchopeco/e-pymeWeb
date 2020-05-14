using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entidades;
using Negocio;

public partial class ItemMenuAM : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PanelError.Visible = false;
        PanelMensaje.Visible = false;
        PanelEliminacion.Visible = false;
        if (!IsPostBack)
        {
            cargarComboPaginas();
            if (Request.QueryString["idItemMenu"] == null)
            {
                lblTitulo.Text = "Agregar Item Menu";
            }
            else
            {
                lblTitulo.Text = "Modificar Item Menu";
                try
                {
                    using (ControladorMenus c_menu = new ControladorMenus())
                    {
                        int idItemMenu = Convert.ToInt32(Request.QueryString["idItemMenu"]);
                        ItemMenu im = c_menu.BuscarItemMenu(idItemMenu);
                        if (Request.QueryString["idItemPadre"] != null)
                        {
                            im.IdPadre = Convert.ToInt32(Request.QueryString["idItemPadre"]);
                        }
                        if (Request.QueryString["alInicio"] != null)
                        {
                            im.AlInicio = Convert.ToBoolean(Request.QueryString["alInicio"]);
                        }
                        if (Request.QueryString["despuesDe"] != null)
                        {
                            im.DespuesDe = Convert.ToInt32(Request.QueryString["alInicio"]);
                        }
                        txtEtiqueta.Text = im.Etiqueta;
                        if (im.Idpagina != null)
                        {
                            cbxPaginas.SelectedValue = im.Pagina.Idpagina.ToString();
                        }
                        chbxEsDivision.Checked = im.EsDivision;
                    }
                }
                catch (ExcepcionPropia ex)
                {
                    PanelError.Visible = true;
                    lblError.Text = ex.Message;
                }


            }
            if (Request.QueryString["idItemMenuEliminar"] != null)
            {
                lblTitulo.Text = "Eliminacion Item Menu";
                PanelAM.Visible = false;
                PanelEliminacion.Visible = true;
                try
                {
                    using (ControladorMenus c_menu = new ControladorMenus())
                    {
                        ItemMenu im = c_menu.BuscarItemMenu(Convert.ToInt32(Request.QueryString["idItemMenuEliminar"]));
                        lblMensajeElimnacion.Text = "¿Desea eliminar el item "+ im.Etiqueta+"?";
                    }
                }
                catch (ExcepcionPropia ex)
                {
                    PanelError.Visible = true;
                    lblError.Text = ex.Message;
                }
            }
        }
    }
    private void cargarComboPaginas()
    {
        using (ControladorMenus c_menus = new ControladorMenus())
        {
            List<Pagina> listP = c_menus.BuscarListPaginas().OrderByDescending(p => p.NombrePagina).ToList();
            Pagina pVacia = new Pagina();
            pVacia.NombrePagina = "Sin Pagina";
            pVacia.Idpagina = 0;
            listP.Add(pVacia);
            listP.Reverse();
            cbxPaginas.DataSource = listP;
            cbxPaginas.DataValueField = "Idpagina";
            cbxPaginas.DataTextField = "NombrePagina";
            cbxPaginas.DataBind();
        }

    }
    protected void btnAgregar_Click(object sender, EventArgs e)
    {
        ///Si estoy Agregando
        if (Request.QueryString["idItemMenu"] == null)
        {
            try
            {
                ItemMenu im = new ItemMenu();
                using (ControladorMenus c_menu = new ControladorMenus())
                {                    
                    im.Etiqueta = txtEtiqueta.Text;
                    im.EsDivision = chbxEsDivision.Checked;
                    im.Idmenu = Convert.ToInt32(Request.QueryString["idMenu"]);
                    if (cbxPaginas.SelectedValue != "0")
                    {
                        im.Pagina = c_menu.BuscarPagina(Convert.ToInt32(cbxPaginas.SelectedValue));
                    }
                    if (Request.QueryString["idPadre"] != string.Empty)
                    {
                        im.IdPadre = Convert.ToInt32(Request.QueryString["idPadre"]);
                    }
                    if (Request.QueryString["alInicio"] != string.Empty)
                    {
                        im.AlInicio = Convert.ToBoolean(Request.QueryString["alInicio"]);
                    }
                    if (Request.QueryString["despuesDe"] != string.Empty)
                    {
                        im.DespuesDe = Convert.ToInt32(Request.QueryString["despuesDe"]);
                    }
                    List<ItemMenu> listI = new List<ItemMenu>();
                    listI.Add(im);
                    c_menu.AgregarListItemMenu(listI);                    
                }
                Response.Redirect(string.Format("MenuAM.aspx?idMenu={0}&mensaje=Item Agregado Con Exito",im.Idmenu));
            }
            catch (ExcepcionPropia ex)
            {

                PanelError.Visible = true;
                lblError.Text = ex.Message;
            }
            
        }
        ///Si estoy modificando
        else
        {
            try
            {
                ItemMenu im = new ItemMenu();
                using (ControladorMenus c_menu = new ControladorMenus())
                {
                    im = c_menu.BuscarItemMenu(Convert.ToInt32(Request.QueryString["idItemMenu"]));
                    im.Etiqueta = txtEtiqueta.Text;
                    im.EsDivision = chbxEsDivision.Checked;                                        
                    if (cbxPaginas.SelectedValue != "0")
                    {
                        im.Pagina = c_menu.BuscarPagina(Convert.ToInt32(cbxPaginas.SelectedValue));
                    }                    
                    List<ItemMenu> listI = new List<ItemMenu>();
                    listI.Add(im);
                    c_menu.ModificarListItemMenu(listI);
                }
                Response.Redirect(string.Format("MenuAM.aspx?idMenu={0}&mensaje=Item Modificado Con Exito", im.Idmenu));
            }
            catch (ExcepcionPropia ex)
            {

                PanelError.Visible = true;
                lblError.Text = ex.Message;
            }
        }
    }
    protected void btnEliminar_Click(object sender, EventArgs e)
    {
        try
        {
            using (ControladorMenus c_menu = new ControladorMenus())
            {
                ItemMenu im = new ItemMenu();
                im = c_menu.BuscarItemMenu(Convert.ToInt32(Request.QueryString["idItemMenuEliminar"]));               
                List<ItemMenu> listI = new List<ItemMenu>();
                listI.Add(im);
                c_menu.EliminarListItemMenu(listI);
                Response.Redirect(string.Format("MenuAM.aspx?idMenu={0}", im.Idmenu));
            }
        }
        catch (ExcepcionPropia ex)
        {
            PanelError.Visible = true;
            lblError.Text = ex.Message;
        }
    }
}