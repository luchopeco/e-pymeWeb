using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entidades;
using Negocio;

public partial class RolesPaginas : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            cargarcbxMenu();
        }
        PanelError.Visible = false;
        PanelMensaje.Visible = false;
        try
        {
            bindGridPaginasRoles();            
            bindGridPaginasMenu();
        }
        catch (ExcepcionPropia ex)
        {
            PanelError.Visible = true;
            lblError.Text = ex.Message;
        }        
        
        
    }

    private void bindGridPaginasRoles()
    {
        using (ControladorRoles c_roles = new ControladorRoles())
        {
            int idRol = Convert.ToInt32(Request.QueryString["idRol"]);
            Rol r = c_roles.BuscarRolCompleto(idRol);
            lblRol.Text = r.Descripcion;
            List<Pagina> listP= r.ListPaginas.OrderBy(p => p.NombrePagina).ToList();
            dgvPaginasRoles.DataSource = null;
            dgvPaginasRoles.DataSource = listP; 
            dgvPaginasRoles.DataBind();
        }
    }

    private void cargarcbxMenu()
    {
        try
        {
            using (ControladorMenus c_menus = new ControladorMenus())
            {
                cbxMenu.DataSource = c_menus.BuscarListMenus().OrderBy(m => m.Nombre);
                cbxMenu.DataValueField = "IdMenu";
                cbxMenu.DataTextField = "Nombre";
                cbxMenu.DataBind();

            }
        }
        catch (ExcepcionPropia ex)
        {

            PanelError.Visible = true;
            lblError.Text = ex.Message;
        }
        
    }
    protected void cbxMenu_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindGridPaginasMenu();
    }
    private void bindGridPaginasMenu()
    {
        using (ControladorMenus c_menu = new ControladorMenus())
        {
            int idRol = Convert.ToInt32(Request.QueryString["idRol"]);
            int idMenu = Convert.ToInt32(cbxMenu.SelectedValue);
            List<Pagina> listp = c_menu.BuscarListPaginas(idMenu, idRol).OrderBy(p => p.NombrePagina).ToList();
            dgvPaginasMenu.DataSource = null;
            dgvPaginasMenu.DataSource = listp;
            dgvPaginasMenu.DataBind();
        }
    }

    protected void btnAsignarPagina_Click(object sender, EventArgs e)
    {
        Pagina p = new Pagina();
        p.Idpagina = Convert.ToInt32(hfIdPagina.Value);
        p.PideAutorizacion = chbxPideAutorizacion.Checked;
        p.ReingresaClave = chbxReingresaClave.Checked;
        p.Restringido = chbxRestringido.Checked;
        p.SoloLectura = chbxSoloLectura.Checked;

        try
        {
            using (ControladorRoles c_roles = new ControladorRoles())
            {
                int idRol =Convert.ToInt32(Request.QueryString["idRol"]);
                c_roles.AgregarPaginaRol(p, idRol);
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append("$('#addModal').modal('hide');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "delHideModalScript", sb.ToString(), false);
                PanelMensaje.Visible = true;
                LabelMensaje.Text = "Pagina Asignada A Rol Correctamente";
                bindGridPaginasMenu();
                bindGridPaginasRoles();
            }
        }
        catch (ExcepcionPropia ex)
        {

            PanelError.Visible = true;
            lblError.Text = ex.Message;
        }
    }
    protected void dgvPaginasMenu_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index = Convert.ToInt32(e.CommandArgument);
        if (e.CommandName.Equals("asignarPagina"))
        {
            string code = dgvPaginasMenu.DataKeys[index].Value.ToString();
            hfIdPagina.Value = code;
            try
            {
                using (ControladorMenus c_menu = new ControladorMenus())
                {
                    Pagina p = c_menu.BuscarPagina(Convert.ToInt32(hfIdPagina.Value));
                    txtPagina.Text = p.NombrePagina;
                }
            }
            catch (ExcepcionPropia ex)
            {
                PanelError.Visible = true;
                lblError.Text = ex.Message;
            }            
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#addModal').modal('show');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "DeleteModalScript", sb.ToString(), false);
        }
    }
    protected void btnQuitarPagina_Click(object sender, EventArgs e)
    {
        try
        {
            using (ControladorRoles c_roles = new ControladorRoles())
            {
                int idRol = Convert.ToInt32(Request.QueryString["idRol"]);
                int idPagina =Convert.ToInt32(hfCode.Value);
                c_roles.EliminarPaginaRol(idRol,idPagina);
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append("$('#deleteModal').modal('hide');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "delHideModalScript", sb.ToString(), false);
                PanelMensaje.Visible = true;
                LabelMensaje.Text = "Pagina Eliminada de Rol Correctamente";
                bindGridPaginasMenu();
                bindGridPaginasRoles();
            }
        }
        catch (ExcepcionPropia ex)
        {

            PanelError.Visible = true;
            lblError.Text = ex.Message;
        }

    }
    protected void dgvPaginasRoles_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index = Convert.ToInt32(e.CommandArgument);
        if (e.CommandName.Equals("deletePagina"))
        {
            string code = dgvPaginasRoles.DataKeys[index].Value.ToString();
            hfCode.Value = code;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#deleteModal').modal('show');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "DeleteModalScript", sb.ToString(), false);
        }
        else if (e.CommandName.Equals("editPagina"))
        {
            GridViewRow gvrow = dgvPaginasRoles.Rows[index]; 
            txtPaginaModif.Text=HttpUtility.HtmlDecode(gvrow.Cells[0].Text).ToString();
            CheckBox ch = gvrow.Cells[1].Controls[0] as CheckBox;
            chbxSoloLecturaModif.Checked = ch.Checked;
            CheckBox ch1 = gvrow.Cells[2].Controls[0] as CheckBox;
            chbxReingresaClaveModif.Checked = ch1.Checked;
            CheckBox ch2 = gvrow.Cells[3].Controls[0] as CheckBox;
            chbxPideAutModif.Checked = ch2.Checked;
            CheckBox ch3= gvrow.Cells[4].Controls[0] as CheckBox;
            chbxRestringidoModif.Checked = ch3.Checked;
            string code = dgvPaginasRoles.DataKeys[index].Value.ToString();
            hfidPaginaModif.Value = code;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#editModal').modal('show');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "DeleteModalScript", sb.ToString(), false);
        }
    }
    protected void btnModificar_Click(object sender, EventArgs e)
    {
        try
        {
            using (ControladorRoles c_roles = new ControladorRoles())
            {
                int idRol = Convert.ToInt32(Request.QueryString["idRol"]);
                Pagina p = new Pagina();
                p.Idpagina = Convert.ToInt32(hfidPaginaModif.Value);
                p.PideAutorizacion = chbxPideAutModif.Checked;
                p.ReingresaClave = chbxReingresaClaveModif.Checked;
                p.Restringido = chbxRestringidoModif.Checked;
                p.SoloLectura = chbxSoloLecturaModif.Checked;
                c_roles.ModificarPaginaRol(p,idRol);

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append("$('#editModal').modal('hide');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "delHideModalScript", sb.ToString(), false);
                PanelMensaje.Visible = true;
                LabelMensaje.Text = "Pagina Modificada Correctamente";
                bindGridPaginasMenu();
                bindGridPaginasRoles();
            }
        }
        catch (ExcepcionPropia ex)
        {

            PanelError.Visible = true;
            lblError.Text = ex.Message;
        }

    }
}