using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entidades;
using Negocio;

public partial class Seguridad_Usuarios : System.Web.UI.Page
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
        using (ControladorUsuarios c_usu = new ControladorUsuarios())
        {
            dgvUsuario.DataSource = c_usu.BuscarListUsuario();
            dgvUsuario.DataBind();
        }
    }

    protected void dgvUsuario_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index = Convert.ToInt32(e.CommandArgument);
        if (e.CommandName.Equals("editRecord"))
        {
            string code = dgvUsuario.DataKeys[index].Value.ToString();
            using (ControladorUsuarios c_usu = new ControladorUsuarios())
            {
                Usuario u = c_usu.BuscarUsuario(Convert.ToInt32(code));
                hfId.Value = u.Idusuario.ToString();
                txtFechaBajaModificar.Text = u.FechaBaja.ToString();
                txtNombreModificar.Text = u.NombreApellido;
                txtNombreUsuarioModificar.Text = u.NombreUsuario;
            }
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#editModal').modal('show');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditModalScript", sb.ToString(), false);
        }
        else if (e.CommandName.Equals("deleteRecord"))
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#editModal').modal('show');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditModalScript", sb.ToString(), false);
        }
        else if (e.CommandName == "roles")
        {
            string code = dgvUsuario.DataKeys[index].Value.ToString();
            Response.Redirect(ResolveUrl("~/Seguridad/UsuarioRoles.aspx") + "?idusuario=" + code);
        }
    }
    protected void btnAgregarUsuario_Click(object sender, EventArgs e)
    {
        try
        {
            using (ControladorUsuarios c_usu = new ControladorUsuarios())
            {
                Usuario u = new Usuario();
                u.Clave = txtClave.Text;
                if (txtFechaBaja.Text != string.Empty)
                {
                    u.FechaBaja = Convert.ToDateTime(txtFechaBaja.Text);
                }                
                u.NombreApellido = txtNombre.Text;
                u.NombreUsuario = txtNombreUsuario.Text;
                c_usu.AgregarUsuario(u);
                
            }
            bindGrid();
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#addModal').modal('hide');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddHideModalScript", sb.ToString(), false);
            PanelMensaje.Visible = true;
            LabelMensaje.Text = "Usuario Agregado Con Exito";
            System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
            sb1.Append(@"<script type='text/javascript'>");
            sb1.Append("var focalizar = $('#MainContent_btnNuevoUsuario').position().top;");
            sb1.Append("$('html,body').animate({scrollTop: focalizar}, 500);");
            sb1.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", sb1.ToString(), false);
        }
        catch (ExcepcionPropia myEx)
        {

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#addModal').modal('hide');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddHideModalScript", sb.ToString(), false);
            PanelError.Visible = true;
            lblError.Text = myEx.Message;
            System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
            sb1.Append(@"<script type='text/javascript'>");
            sb1.Append("var focalizar = $('#MainContent_btnNuevoUsuario').position().top;");
            sb1.Append("$('html,body').animate({scrollTop: focalizar}, 500);");
            sb1.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", sb1.ToString(), false);
        }
        catch (FormatException myEx)
        {

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#addModal').modal('hide');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddHideModalScript", sb.ToString(), false);
            PanelError.Visible = true;
            lblError.Text = myEx.Message;
            System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
            sb1.Append(@"<script type='text/javascript'>");
            sb1.Append("var focalizar = $('#MainContent_btnNuevoUsuario').position().top;");
            sb1.Append("$('html,body').animate({scrollTop: focalizar}, 500);");
            sb1.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", sb1.ToString(), false);
        }

    }
    protected void btnNuevoUsuario_Click(object sender, EventArgs e)
    {

        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append(@"<script type='text/javascript'>");
        sb.Append("$('#addModal').modal('show');");
        sb.Append(@"</script>");
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddShowModalScript", sb.ToString(), false);
    }
    protected void btnModificar_Click(object sender, EventArgs e)
    {
        try
        {
            using (ControladorUsuarios c_usu = new ControladorUsuarios())
            {
                Usuario u = new Usuario();
                u.Clave = txtClaveModificar.Text;
                if (txtFechaBaja.Text != string.Empty)
                {
                    u.FechaBaja = Convert.ToDateTime(txtFechaBajaModificar.Text);
                }
                u.Idusuario = Convert.ToInt32(hfId.Value);
                u.NombreApellido = txtNombreModificar.Text;
                u.NombreUsuario = txtNombreUsuarioModificar.Text;
                c_usu.ModificarUsuario(u);

            }
            bindGrid();
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#editModal').modal('hide');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddHideModalScript", sb.ToString(), false);
            PanelMensaje.Visible = true;
            LabelMensaje.Text = "Usuario Modificado Con Exito";
            System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
            sb1.Append(@"<script type='text/javascript'>");
            sb1.Append("var focalizar = $('#MainContent_btnNuevoUsuario').position().top;");
            sb1.Append("$('html,body').animate({scrollTop: focalizar}, 500);");
            sb1.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", sb1.ToString(), false);
        }
        catch (ExcepcionPropia myEx)
        {

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#editModal').modal('hide');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddHideModalScript", sb.ToString(), false);
            PanelError.Visible = true;
            lblError.Text = myEx.Message;
            System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
            sb1.Append(@"<script type='text/javascript'>");
            sb1.Append("var focalizar = $('#MainContent_btnNuevoUsuario').position().top;");
            sb1.Append("$('html,body').animate({scrollTop: focalizar}, 500);");
            sb1.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", sb1.ToString(), false);
        }
        catch (FormatException myEx)
        {

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#edit').modal('hide');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddHideModalScript", sb.ToString(), false);
            PanelError.Visible = true;
            lblError.Text = myEx.Message;
            System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
            sb1.Append(@"<script type='text/javascript'>");
            sb1.Append("var focalizar = $('#MainContent_btnNuevoUsuario').position().top;");
            sb1.Append("$('html,body').animate({scrollTop: focalizar}, 500);");
            sb1.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", sb1.ToString(), false);
        }
    }

}