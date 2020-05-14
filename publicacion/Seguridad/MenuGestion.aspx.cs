using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Negocio;
using Entidades;

public partial class GestionMenu : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PanelError.Visible = false;
        PanelMensaje.Visible = false;
        if (!IsPostBack)
        {
            try
            {
                bindGrid();
            }
            catch (ExcepcionPropia ex)
            {
                PanelError.Visible = true;
                lblError.Text = ex.Message;
            }
        }
    }

    private void bindGrid()
    {
        using (ControladorMenus c_menus = new ControladorMenus())
        {
            dgvMenus.DataSource = c_menus.BuscarListMenus();
            dgvMenus.DataBind();
        }
    }
    protected void dgvMenus_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index = Convert.ToInt32(e.CommandArgument);
        if (e.CommandName.Equals("editRecord"))
        {
            string code = dgvMenus.DataKeys[index].Value.ToString();
            Response.Redirect("MenuAM.aspx?idMenu=" + code);
        }
        else if (e.CommandName.Equals("deleteRecord"))
        {
            string code = dgvMenus.DataKeys[index].Value.ToString();
            hfCode.Value = code;
            lblMenuEliminar.Text = dgvMenus.Rows[index].Cells[0].Text;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#deleteModal').modal('show');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "DeleteModalScript", sb.ToString(), false);
        }
        else if (e.CommandName.Equals("editMenu"))
        {
            string code = dgvMenus.DataKeys[index].Value.ToString();
            HiddenFieldModificar.Value = code;            
            GridViewRow gvrow = dgvMenus.Rows[index];
            txtNombreModif.Text = HttpUtility.HtmlDecode(gvrow.Cells[0].Text).ToString();  
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#editModal').modal('show');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditModalScript", sb.ToString(), false);
        }
        

        
    }

    protected void btnNuevoMenu_Click(object sender, EventArgs e)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append(@"<script type='text/javascript'>");
        sb.Append("$('#addModal').modal('show');");
        sb.Append(@"</script>");
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddShowModalScript", sb.ToString(), false);
    }
    protected void btnAgregarMenu_Click(object sender, EventArgs e)
    {
        try
        {
            using (ControladorMenus c_menu = new ControladorMenus())
            {
                Menuu m = new Menuu();
                m.Nombre = txtNombre.Text;
                c_menu.AgregarMenu(m);
                txtNombre.Text = "";
                bindGrid();
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append("$('#addModal').modal('hide');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScript", sb.ToString(), false);
                PanelMensaje.Visible = true;
                LabelMensaje.Text = "Menu Agregado con Exito";
            }
        }
        catch (ExcepcionPropia ex)
        {
            PanelError.Visible = true;
            lblError.Text = ex.Message;
        }
    }
    protected void btnEliminarMenu_Click(object sender, EventArgs e)
    {
        try
        {
            int id = Convert.ToInt32(hfCode.Value);
            using (ControladorMenus c_menu = new ControladorMenus())
            {
                c_menu.EliminarMenu(id);
            }
            bindGrid();
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#deleteModal').modal('hide');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "delHideModalScript", sb.ToString(), false);
            PanelMensaje.Visible = true;
            LabelMensaje.Text = "Menu Eliminado con Exito";
        }
        catch (ExcepcionPropia ex)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#deleteModal').modal('hide');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "delHideModalScript", sb.ToString(), false);
            PanelError.Visible = true;
            lblError.Text = ex.Message;            
        }
    }
    protected void btnModificarMenu_Click(object sender, EventArgs e)
    {
        try
        {
            using (ControladorMenus c_menu = new ControladorMenus())
            {
                Menuu m = new Menuu();
                m.IdMenu = Convert.ToInt32(HiddenFieldModificar.Value);
                m.Nombre = txtNombreModif.Text;
                c_menu.ModificarMenu(m);
                txtNombreModif.Text = "";
                bindGrid();
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append("$('#editModal').modal('hide');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScript", sb.ToString(), false);
                PanelMensaje.Visible = true;
                LabelMensaje.Text = "Menu Modificado con Exito";

            }
        }
        catch (ExcepcionPropia ex)
        {

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#editModal').modal('hide');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "delHideModalScript", sb.ToString(), false);
            PanelError.Visible = true;
            lblError.Text = ex.Message;
        }
    }
}