using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entidades;
using Negocio;

public partial class PaginasGestion : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PanelError.Visible = false;
        PanelMensaje.Visible = false;
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

    private void bindGrid()
    {
        dgvPagina.DataSource = null;
        using (ControladorMenus c_menus = new ControladorMenus())
        {
            dgvPagina.DataSource = c_menus.BuscarListPaginas();
            dgvPagina.DataBind();
        }
    }

    protected void btnNuevaPagina_Click(object sender, EventArgs e)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append(@"<script type='text/javascript'>");
        sb.Append("$('#addModal').modal('show');");
        sb.Append(@"</script>");
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddShowModalScript", sb.ToString(), false);       

    }   
    protected void dgvPagina_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index = Convert.ToInt32(e.CommandArgument);
        if (e.CommandName.Equals("deleteRecord"))
        {
            string code = dgvPagina.DataKeys[index].Value.ToString();
            hfCode.Value = code;
            lblPaginaEliminar.Text = dgvPagina.Rows[index].Cells[0].Text;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#deleteModal').modal('show');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "DeleteModalScript", sb.ToString(), false);
        }
        else if (e.CommandName.Equals("editRecord"))
        {
            string code = dgvPagina.DataKeys[index].Value.ToString();
            HiddenFieldModificar.Value = code;
            GridViewRow gvrow = dgvPagina.Rows[index];           
            txtNombreModif.Text = HttpUtility.HtmlDecode(gvrow.Cells[0].Text).ToString();
            txtDescripcionModificar.Text = HttpUtility.HtmlDecode(gvrow.Cells[1].Text).ToString();            
            txtCodigoModificar.Text = HttpUtility.HtmlDecode(gvrow.Cells[2].Text).ToString();
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#editModal').modal('show');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditModalScript", sb.ToString(), false);
           // ScriptManager.RegisterStartupScript(Page, GetType(), "MyScript", "alert('message here');", true);

        }
    }
    protected void btnEliminarPagina_Click(object sender, EventArgs e)
    {
        try
        {
            int id = Convert.ToInt32(hfCode.Value);
            using (ControladorMenus c_menus = new ControladorMenus())
            {
                c_menus.EliminarPagina(id);
            }
            bindGrid();
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#deleteModal').modal('hide');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "delHideModalScript", sb.ToString(), false);
            PanelMensaje.Visible = true;
            LabelMensaje.Text = "Pagina Eliminada con Exito";
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
    protected void btnAgregarPagina_Click(object sender, EventArgs e)
    {
        try
        {
            using (ControladorMenus c_menu = new ControladorMenus())
            {
                Pagina p = new Pagina();
                p.NombrePagina = txtNombre.Text;
                p.Descripcion = txtDescripcion.Text;
                p.Codigo = txtCodigo.Text;
                c_menu.AgregarPagina(p);
                txtDescripcion.Text = "";
                txtNombre.Text = "";
                bindGrid();
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append("$('#addModal').modal('hide');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScript", sb.ToString(), false);
                PanelMensaje.Visible = true;
                LabelMensaje.Text = "Pagina Agregada con Exito";

            }
        }
        catch (ExcepcionPropia ex)
        {

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#addModal').modal('hide');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "delHideModalScript", sb.ToString(), false);
            PanelError.Visible = true;
            lblError.Text = ex.Message;
        }
    }
    protected void btnModificarPagina_Click(object sender, EventArgs e)
    {
        try
        {
            using (ControladorMenus c_menu = new ControladorMenus())
            {
                Pagina p = new Pagina();
                p.Idpagina = Convert.ToInt32(HiddenFieldModificar.Value);
                p.NombrePagina =txtNombreModif.Text;
                p.Descripcion = txtDescripcionModificar.Text;
                p.Codigo = txtCodigoModificar.Text;
                c_menu.ModificarPagina(p);
                txtDescripcionModificar.Text = "";
                txtNombreModif.Text = "";
                bindGrid();
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append("$('#editModal').modal('hide');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScript", sb.ToString(), false);
                PanelMensaje.Visible = true;
                LabelMensaje.Text = "Pagina Modificada con Exito";

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