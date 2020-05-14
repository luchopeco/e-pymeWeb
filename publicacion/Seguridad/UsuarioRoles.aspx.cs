using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entidades;
using Negocio;

public partial class UsuarioRoles : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PanelError.Visible = false;
        PanelMensaje.Visible = false;
        if (!IsPostBack)
        {
            int idUsuario = Convert.ToInt32(Request.QueryString["idusuario"]);
            try
            {
                Usuario u;

                using (ControladorUsuarios c_usu = new ControladorUsuarios())
                {
                    u = c_usu.BuscarUsuario(idUsuario);
                    lblUsuario.Text = u.NombreUsuario;
                }
                ///pongo el codigo de operador en un campo oculto
                ViewState["idusuario"] = u.Idusuario;
                ViewState["essu"] = u.EsSuperUsuario;

            }
            catch (ExcepcionPropia ex)
            {
                PanelError.Visible = true;
                lblError.Text = ex.Message;
            }
            bindGrillas();
        }
        establecerPopiedadesBtns();
    }
    private void establecerPopiedadesBtns()
    {
        if (Convert.ToBoolean(ViewState["essu"]))
        {
            btnSuperUsuario.Text = "Quitar como Super Usuario";
        }
        else
        {
            btnSuperUsuario.Text = "Establecer como Super Usuario";
        }
    }

    private void bindGrillas()
    {
        List<Rol> listRolUsuario = new List<Rol>();
        try
        {
            using (ControladorRoles c_roles = new ControladorRoles())
            {
                listRolUsuario = c_roles.BuscarListRoles(Convert.ToInt32(ViewState["idusuario"]));
                dgvRolesAsignados.DataSource = listRolUsuario.OrderBy(r => r.Descripcion).ToList();
                dgvRolesAsignados.DataBind();
            }
        }
        catch (ExcepcionPropia ex)
        {
            dgvRolesAsignados.DataSource = null;
            dgvRolesAsignados.DataBind();
            PanelError.Visible = true;
            lblError.Text = ex.Message;
        }
        try
        {
            using (ControladorRoles c_roles = new ControladorRoles())
            {
                List<Rol> listRoles = c_roles.BuscarListRoles();
                List<Rol> listRolesFiltados = listRoles.Where(r => !listRolUsuario.Select(ru => ru.IdRol).Contains(r.IdRol)).ToList();
                dgvRoles.DataSource = listRolesFiltados.OrderBy(r => r.Descripcion).ToList();
                dgvRoles.DataBind();
            }
        }
        catch (ExcepcionPropia ex)
        {
            PanelError.Visible = true;
            lblError.Text = ex.Message;
        }

    }
    protected void dgvRoles_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index = Convert.ToInt32(e.CommandArgument);
        if (e.CommandName.Equals("asignarRol"))
        {
            string code = dgvRoles.DataKeys[index].Value.ToString();
            try
            {
                using (ControladorRoles c_roles = new ControladorRoles())
                {
                    int idRol = Convert.ToInt32(code);
                    int codOperador = Convert.ToInt32(ViewState["idusuario"]);
                    c_roles.AgregarRolUsuario(idRol, codOperador);
                }
                bindGrillas();
                PanelMensaje.Visible = true;
                LabelMensaje.Text = "Rol Asignado Correctamente";
            }
            catch (ExcepcionPropia ex)
            {

                PanelError.Visible = true;
                lblError.Text = ex.Message;
            }

        }
    }
    protected void dgvRolesAsignados_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index = Convert.ToInt32(e.CommandArgument);
        if (e.CommandName.Equals("eliminarRol"))
        {
            string code = dgvRolesAsignados.DataKeys[index].Value.ToString();
            try
            {
                using (ControladorRoles c_roles = new ControladorRoles())
                {
                    int idRol = Convert.ToInt32(code);
                    int codOperador = Convert.ToInt32(ViewState["idusuario"]);
                    c_roles.EliminarRolUsuario(idRol, codOperador);
                }
                bindGrillas();
                PanelMensaje.Visible = true;
                LabelMensaje.Text = "Rol Eliminado Correctamente";
            }
            catch (ExcepcionPropia ex)
            {

                PanelError.Visible = true;
                lblError.Text = ex.Message;
            }

        }

    }
    protected void btnSuperUsuario_Click(object sender, EventArgs e)
    {
        if (btnSuperUsuario.Text == "Quitar como Super Usuario")
        {
            lblPreguntaModal.Text = "¿Desea quitar la propiedad de Super Usuario al usuario actual?";           
        }
        else
        {
            lblPreguntaModal.Text = "¿Desea establecer como Super Usuario al usuario Actual?";
        }
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append(@"<script type='text/javascript'>");
        sb.Append("$('#deleteModal').modal('show');");
        sb.Append(@"</script>");
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "DeleteModalScript", sb.ToString(), false);
    }
    protected void btnModal_Click(object sender, EventArgs e)
    {
        try
        {
            using (ControladorUsuarios c_usu = new ControladorUsuarios())
            {
                if (btnSuperUsuario.Text == "Quitar como Super Usuario")
                {
                    int idPersona = Convert.ToInt32(Request.QueryString["idUsuario"]);
                    c_usu.ModificarUsuarioEsSU(idPersona,false);
                    ViewState["essu"] = false;
                }
                else
                {
                    int idPersona = Convert.ToInt32(Request.QueryString["idUsuario"]);
                    c_usu.ModificarUsuarioEsSU(idPersona, true);
                    ViewState["essu"] = true;
                }
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append("$('#deleteModal').modal('hide');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "hideDeleteModalScript", sb.ToString(), false);
                PanelMensaje.Visible = true;
                LabelMensaje.Text = "Usuario Modificado con exito";
                establecerPopiedadesBtns();
            }
        }
        catch (ExcepcionPropia ex)
        {

            PanelError.Visible = true;
            lblError.Text = ex.Message;
        }
    }
   
}