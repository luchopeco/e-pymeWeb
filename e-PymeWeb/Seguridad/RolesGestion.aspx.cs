using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entidades;
using Negocio;

public partial class RolesGestion : System.Web.UI.Page
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
        using (ControladorRoles c_roles = new ControladorRoles())
        {
            dgvRoles.DataSource = null;
            dgvRoles.DataSource = c_roles.BuscarListRoles();
            dgvRoles.DataBind();
        }
    }
    protected void dgvRoles_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index = Convert.ToInt32(e.CommandArgument);
        if (e.CommandName.Equals("deleteRecord"))
        {
            string code = dgvRoles.DataKeys[index].Value.ToString();
            hfCode.Value = code;
            lblRolEliminar.Text = dgvRoles.Rows[index].Cells[0].Text;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#deleteModal').modal('show');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "DeleteModalScript", sb.ToString(), false);
        }
        else if (e.CommandName.Equals("editPaginas"))
        {
            string code = dgvRoles.DataKeys[index].Value.ToString();
            Response.Redirect("RolesPaginas.aspx?idRol="+code);
        }
        else if (e.CommandName.Equals("editRol"))
        {
            string code = dgvRoles.DataKeys[index].Value.ToString();
            hfCode.Value = code;
            GridViewRow gvrow = dgvRoles.Rows[index];
            txtDescripcionModificar.Text = gvrow.Cells[0].Text;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#editModal').modal('show');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditModalScript", sb.ToString(), false);
        }
    }
    protected void btnNuevoRol_Click(object sender, EventArgs e)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append(@"<script type='text/javascript'>");
        sb.Append("$('#addModal').modal('show');");
        sb.Append(@"</script>");
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddShowModalScript", sb.ToString(), false);       
    }
    protected void btnAgregarRol_Click(object sender, EventArgs e)
    {
        try
        {
            using (ControladorRoles c_roles=new ControladorRoles())
            {
                Rol r = new Rol();                
                r.Descripcion = txtDescripcion.Text;
                c_roles.AgregarRol(r);
                txtDescripcion.Text = "";
                bindGrid();
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append("$('#addModal').modal('hide');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScript", sb.ToString(), false);
                PanelMensaje.Visible = true;
                LabelMensaje.Text = "Rol Agregado con Exito";
                
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
    protected void btnEliminarRol_Click(object sender, EventArgs e)
    {
        try
        {
            int id = Convert.ToInt32(hfCode.Value);
            using (ControladorRoles c_roles = new ControladorRoles())
            {
                c_roles.EliminarRol(id);
            }
            bindGrid();
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#deleteModal').modal('hide');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "delHideModalScript", sb.ToString(), false);
            PanelMensaje.Visible = true;
            LabelMensaje.Text = "Rol Eliminado con Exito";
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
    protected void btnModificarRol_Click(object sender, EventArgs e)
    {
        try
        {
            using (ControladorRoles c_roles=new ControladorRoles())
            {
                Rol r = new Rol();
                r.IdRol =Convert.ToInt32(hfCode.Value);
                r.Descripcion = txtDescripcionModificar.Text;
                c_roles.ModificarRolABM(r);

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append("$('#editModal').modal('hide');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScript", sb.ToString(), false);
                PanelMensaje.Visible = true;
                LabelMensaje.Text = "Rol Modificado Correctamente";
                bindGrid();
            }
        }
        catch (ExcepcionPropia ex)
        {

            PanelError.Visible = true;
            lblError.Text = ex.Message;
        }

    }
}