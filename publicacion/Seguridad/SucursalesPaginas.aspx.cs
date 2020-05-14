using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entidades;
using Negocio;

public partial class Seguridad_SucursalesPaginas : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PanelError.Visible = false;
        PanelMensaje.Visible = false;
        if (!IsPostBack)
        {
            cargarCbxSucursales();
            bindGridPaginas();
            bindGridPaginasBloqueadas();
        }
    }

    private void cargarCbxSucursales()
    {
        try
        {
            using (ControladorSucursal c_suc = new ControladorSucursal())
            {
                cbxSucursales.DataSource = c_suc.BuscarListSucursales();
                cbxSucursales.DataValueField = "IdSucursal";
                cbxSucursales.DataTextField = "Descripcion";
                cbxSucursales.DataBind();

            }
        }
        catch (ExcepcionPropia ex)
        {
            PanelError.Visible = true;
            lblError.Text = ex.Message;
        }
    }

    private void bindGridPaginasBloqueadas()
    {
        try
        {
            using (ControladorRoles c_roles = new ControladorRoles())
            {
                int idSucrsal = Convert.ToInt32(cbxSucursales.SelectedValue);
                dgvPaginasRestringidas.DataSource = c_roles.BuscarListPaginasBloqueadas(idSucrsal);
                dgvPaginasRestringidas.DataBind();
                //dgvPaginasMenu.DataSource = null;
                //dgvPaginasMenu.DataSource = listp;
                //dgvPaginasMenu.DataBind();
            }
        }
        catch (ExcepcionPropia ex)
        {
            dgvPaginasRestringidas.DataSource = null;
            dgvPaginasRestringidas.DataBind();
            PanelError.Visible = true;
            lblError.Text = ex.Message;
        }
    }
    private void bindGridPaginas()
    {
        try
        {
            using (ControladorRoles c_roles = new ControladorRoles())
            {
                int idSucrsal = Convert.ToInt32(cbxSucursales.SelectedValue);
                dgvPaginas.DataSource = c_roles.BuscarListPaginasSinBloquear(idSucrsal);
                dgvPaginas.DataBind();
            }
        }
        catch (ExcepcionPropia ex)
        {
            dgvPaginas.DataSource = null;
            dgvPaginas.DataBind();
            PanelError.Visible = true;
            lblError.Text = ex.Message;
        }
    }
    protected void dgvPaginas_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index = Convert.ToInt32(e.CommandArgument);
        if (e.CommandName.Equals("agregar"))
        {
            string code = dgvPaginas.DataKeys[index].Value.ToString();
            try
            {
                using (ControladorRoles c_roles = new ControladorRoles())
                {
                    c_roles.AgregarSucursalPagina(Convert.ToInt32(code), Convert.ToInt32(cbxSucursales.SelectedValue));
                }
                bindGridPaginas();
                bindGridPaginasBloqueadas();
                PanelMensaje.Visible = true;
                LabelMensaje.Text = "Pagina Asignada Correctamente";
            }
            catch (ExcepcionPropia ex)
            {
                PanelError.Visible = true;
                lblError.Text = ex.Message;
            }
        }
    }
    protected void cbxSucursales_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindGridPaginas();
        bindGridPaginasBloqueadas();
    }
    protected void dgvPaginasRestringidas_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index = Convert.ToInt32(e.CommandArgument);
        if (e.CommandName.Equals("quitar"))
        {
            string code = dgvPaginasRestringidas.DataKeys[index].Value.ToString();
            try
            {
                using (ControladorRoles c_roles = new ControladorRoles())
                {
                    c_roles.EliminarSucursalPagina(Convert.ToInt32(code), Convert.ToInt32(cbxSucursales.SelectedValue));
                }
                bindGridPaginas();
                bindGridPaginasBloqueadas();
                PanelMensaje.Visible = true;
                LabelMensaje.Text = "Pagina Desasignada Correctamente";
            }
            catch (ExcepcionPropia ex)
            {
                PanelError.Visible = true;
                lblError.Text = ex.Message;
            }
        }
    }
}