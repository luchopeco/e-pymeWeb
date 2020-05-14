using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using Entidades;
using Negocio;

public partial class SiteMaster : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        panelMens.Visible = false;
        panelErr.Visible = false;
        if (!IsPostBack)
        {
            try
            {
                StringBuilder oStringBuilder = new StringBuilder();
                using (ControladorMenus mnu = new ControladorMenus())
                {
                    int idOperador = ApplicationSesion.ActiveUser.Idusuario;
                    int idSucursal = ApplicationSesion.ActiveSucursal.IdSucursal;
                    DataTable dtMenus = mnu.ObtenerMenusUsuario(idOperador, idSucursal, 1); // (Convert.ToInt32(Session["codusuario"]));
                    //DataTable dtMenus = mnu.ObtenerMenusUsuario(50237, 1, 1);
                    DataRow[] rowsMenu = dtMenus.Select("nivel = 1");
                    string menu = GenerarMenu(rowsMenu, dtMenus, oStringBuilder);
                    LiteralMenu.Text = menu;
                }
                lblSucursal.Text = ApplicationSesion.ActiveSucursal.Descripcion;


                mostrarCaja();


            }
            catch (Exception)
            {
                System.Web.Security.FormsAuthentication.SignOut();
                Session.Clear();
                Session.Abandon();
                Session.Clear();
                Response.Redirect("~/Login.aspx");
            }            

        }
    }
    private string GenerarMenu(DataRow[] drParentMenu, DataTable oDataTable, StringBuilder oStringBuilder)
    {
        oStringBuilder.AppendLine("<ul class='main-menu'>");
        if (drParentMenu.Length > 0)
        {
            foreach (DataRow dr in drParentMenu)
            {
                string MenuID = dr["iditem_menu"].ToString();
                string ParentID = dr["iditem_padre"].ToString();
                DataRow[] subMenu = oDataTable.Select(String.Format("iditem_padre = {0}", MenuID));
                if (subMenu.Length > 0 && !MenuID.Equals(ParentID))
                {
                    string MenuURL = dr["pagina"].ToString();
                    string MenuName = dr["etiqueta"].ToString();
                    bool esDivision = Convert.ToBoolean(dr["es_division"]);
                    string line;
                    if (!esDivision)
                    {
                        if (MenuURL == "")
                        {
                            line = String.Format(@"<li ><a href='#' class='js-sub-menu-toggle'><i class='fa fa-clipboard fa-fw'>
                                </i><span class='text'>{0}</span><i class='toggle-icon fa fa-angle-left'></i></a>", MenuName);
                        }
                        else
                        {

                            line = String.Format(@"<li ><a href='{0}'><span class='text'>{1}</span></a>", ResolveUrl(MenuURL), MenuName);
                        }
                        oStringBuilder.Append(line);
                    }

                    if (subMenu.Length > 0 && !MenuID.Equals(ParentID))
                    {
                        var subMenuBuilder = new StringBuilder();
                        oStringBuilder.Append(GenerarSubMenu(subMenu, oDataTable, subMenuBuilder));
                    }
                    oStringBuilder.Append("</li>");
                }

            }
        }
        oStringBuilder.Append("</ul>");
        return oStringBuilder.ToString();
    }
    private string GenerarSubMenu(DataRow[] drParentMenu, DataTable oDataTable, StringBuilder oStringBuilder)
    {

        oStringBuilder.AppendLine("<ul class='sub-menu'>");
        if (drParentMenu.Length > 0)
        {
            foreach (DataRow dr in drParentMenu)
            {

                string MenuURL = dr["pagina"].ToString();
                string MenuName = dr["etiqueta"].ToString();
                bool esDivision = Convert.ToBoolean(dr["es_division"]);
                string line;
                if (!esDivision)
                {
                    if (MenuURL == "")
                    {
                        line = String.Format(@"<li ><a href='#' class='js-sub-menu-toggle'>{0}<span class='text'>                                
                                </span><i class='toggle-icon fa fa-angle-left'></i></a>", MenuName);
                    }
                    else
                    {
                        line = String.Format(@"<li ><a href='{0}'><span class='text'>{1}</span></a>", ResolveUrl(MenuURL), MenuName);
                    }
                    oStringBuilder.Append(line);
                }
                string MenuID = dr["iditem_menu"].ToString();
                string ParentID = dr["iditem_padre"].ToString();
                DataRow[] subMenu = oDataTable.Select(String.Format("iditem_padre = {0}", MenuID));
                if (subMenu.Length > 0 && !MenuID.Equals(ParentID))
                {
                    var subMenuBuilder = new StringBuilder();
                    oStringBuilder.Append(GenerarSubSubMenu(subMenu, oDataTable, subMenuBuilder));
                } oStringBuilder.Append("</li>");
            }
        }
        oStringBuilder.Append("</ul>");
        return oStringBuilder.ToString();
    }
    private string GenerarSubSubMenu(DataRow[] drParentMenu, DataTable oDataTable, StringBuilder oStringBuilder)
    {

        oStringBuilder.AppendLine("<ul class='sub-menu'>");
        if (drParentMenu.Length > 0)
        {
            foreach (DataRow dr in drParentMenu)
            {

                string MenuURL = dr["pagina"].ToString();
                string MenuName = dr["etiqueta"].ToString();
                bool esDivision = Convert.ToBoolean(dr["es_division"]);
                string line;
                if (!esDivision)
                {
                    if (MenuURL == "")
                    {
                        line = String.Format(@"<li ><a href='#' class='js-sub-menu-toggle'><span class='text'>{0}                             
                                </span><i class='toggle-icon fa fa-angle-left'></i></a>", MenuName);
                    }
                    else
                    {
                        line = String.Format(@"<li ><a href='{0}'><span class='text'>{1}</span></a>", ResolveUrl(MenuURL), MenuName);
                    }
                    oStringBuilder.Append(line);
                }
                string MenuID = dr["iditem_menu"].ToString();
                string ParentID = dr["iditem_padre"].ToString();
                DataRow[] subMenu = oDataTable.Select(String.Format("iditem_padre = {0}", MenuID));
                if (subMenu.Length > 0 && !MenuID.Equals(ParentID))
                {
                    var subMenuBuilder = new StringBuilder();
                    oStringBuilder.Append(GenerarSubSubMenu(subMenu, oDataTable, subMenuBuilder));
                } oStringBuilder.Append("</li>");
            }
        }
        oStringBuilder.Append("</ul>");
        return oStringBuilder.ToString();
    }
    protected void btnModificar_Click(object sender, EventArgs e)
    {
        try
        {
            using (ControladorUsuarios c_usu = new ControladorUsuarios())
            {
                Usuario u = c_usu.BuscarUsuario(ApplicationSesion.ActiveUser.Idusuario);
                if (u.Clave != txtClaveAnterior.Text)
                {
                    throw new ExcepcionPropia("La clave actual no coincide con la anterior");
                }
                u.Clave = txtClaveModificar.Text;
                c_usu.ModificarUsuario(u);
            }
            panelMens.Visible = true;
            lblMens.Text = "Clave modificada con exito";
            txtClaveAnterior.Text = "";
            txtClaveModificar.Text = "";
            txtClaveReingresadaModificar.Text = "";
        }
        catch (ExcepcionPropia myEx)
        {
            panelErr.Visible = true;
            lblErr.Text = myEx.Message;
        }

    }
    protected void mostrarCaja()
    {
        if (ApplicationSesion.ActiveCaja != null)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<li class='notification-item inbox'>");
            sb.Append("<div class='btn-group'>");
            sb.Append("<a aria-expanded='false' href='#' title='Caja Abierta' class='dropdown-toggle' data-toggle='dropdown'>");
            sb.Append("<i class='fa fa-inbox'></i><span class='count'>ok</span>");
            sb.Append("<span class='circle'></span>");
            sb.Append("</a>");
            sb.Append("</div>");
            sb.Append("</li>");
            literalCaja.Text = sb.ToString();

        }
    }
    protected void btnPerfil_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Perfil.aspx");
    }
}
