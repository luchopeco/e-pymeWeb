using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entidades;
using Negocio;
public partial class Caja_Cajas : System.Web.UI.Page
{
    protected static string modalDetalle = "detalleModal";
    protected static string modalReabrirModal = "reabirModal";
    protected int idCajaAReabrir
    {
        get { return (int)ViewState["idCajaAReabrir"]; }
        set { ViewState["idCajaAReabrir"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        ucPanelMensajes.PanelErrorVisible = false;
        ucPanelMensajes.PanelMensajeVisible = false;

        if (!IsPostBack)
        {

        }
    }

    protected void btnBuscarCajas_Click(object sender, EventArgs e)
    {
        try
        {
            bindGrilla();

        }
        catch (Exception ex)
        {
            mostrarExcepcion(ex.Message);
        }
    }

    private void bindGrilla()
    {
        DateTime fechaDesde = Helper.FechaHoraInicial(Convert.ToDateTime(txtFechaDesde.Text));
        DateTime fechaHasta = Helper.FechaHoraFinal(Convert.ToDateTime(txtFechaHasta.Text));
        dgvCajas.DataSource = Caja.BuscarListCajas(fechaDesde, fechaHasta, ApplicationSesion.ActiveUser.Idusuario);
        dgvCajas.DataBind();
    }

    private void mostrarMensaje(string msj)
    {
        ucPanelMensajes.PanelMensajeVisible = true;
        ucPanelMensajes.LblMensaje = msj;
        System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
        sb1.Append(@"<script type='text/javascript'>");
        sb1.Append("var focalizar = $('#foco').position().top;");
        sb1.Append("$('html,body').animate({scrollTop: focalizar}, 500);");
        sb1.Append(@"</script>");
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", sb1.ToString(), false);
    }
    private void mostrarExcepcion(string msj)
    {
        ucPanelMensajes.PanelErrorVisible = true;
        ucPanelMensajes.LblError = msj;
        System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
        sb1.Append(@"<script type='text/javascript'>");
        sb1.Append("var focalizar = $('#foco').position().top;");
        sb1.Append("$('html,body').animate({scrollTop: focalizar}, 500);");
        sb1.Append(@"</script>");
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", sb1.ToString(), false);
    }
    protected void dgvCajas_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index = Convert.ToInt32(e.CommandArgument);
        if (e.CommandName.Equals("detalle"))
        {
            Caja c = new Caja(Convert.ToInt32(dgvCajas.DataKeys[index].Value));
            using (ControladorUsuarios c_usu = new ControladorUsuarios())
            {
                c.Usuario = c_usu.BuscarUsuario(c.Idusuario);
            }
            List<Caja> lisc = new List<Caja>();
            lisc.Add(c);
            dvDatosGeneralesModal.DataSource = lisc;
            dvDatosGeneralesModal.DataBind();

            dgvMovimientosEfectivo.DataSource = c.ListMovimientos;
            dgvMovimientosEfectivo.DataBind();

            dgvMovimientosSinFondos.DataSource = c.ListMovimientosNoEfectivo;
            dgvMovimientosSinFondos.DataBind();

            abrirModal(modalDetalle);

        }
        else if (e.CommandName.Equals("cerrar"))
        {
            Caja c = new Caja(Convert.ToInt32(dgvCajas.DataKeys[index].Value));
            if (c.Idusuario != ApplicationSesion.ActiveUser.Idusuario)
            {
                mostrarExcepcion("No se puede cerrar la caja de otro usuario");
            }
            else if (c.FechaCierre != null)
            {
                mostrarExcepcion("La caja ya se encuentra cerrada");
            }
            else
            {
                Response.Redirect("~/Caja/CajaActual.aspx?id=" + c.Idcaja.ToString());
            }
        }
        else if (e.CommandName.Equals("reabrir"))
        {
            if (ApplicationSesion.ActiveCaja != null)
            {
                mostrarExcepcion("No Puede Reabrir una Caja Teniendo una Caja Abierta");
                return;
            }
            Caja c = new Caja(Convert.ToInt32(dgvCajas.DataKeys[index].Value));
            if (c.Fecha != DateTime.Today)
            {
                mostrarExcepcion("No se puede reabrir una caja de un dia diferente al de hoy ");
                return;
            }
            abrirModal(modalReabrirModal);
            idCajaAReabrir = c.Idcaja;
        }
    }

    private void abrirModal(string idDiv)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append(@"<script type='text/javascript'>");
        string aux = "$('#" + idDiv + "').modal('show')";
        sb.Append(aux);
        sb.Append(@"</script>");
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "suscriptorShowModalScript", sb.ToString(), false);
    }
    private void cerrarModal(string idDiv)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append(@"<script type='text/javascript'>");
        string aux = "$('#" + idDiv + "').modal('hide')";
        sb.Append(aux);
        sb.Append(@"</script>");
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "suscriptorShowModalScript", sb.ToString(), false);
    }
    protected void btnReabrirFinal_Click(object sender, EventArgs e)
    {
        try
        {
            Caja.Reabrir(idCajaAReabrir);
            bindGrilla();
            ApplicationSesion.ActiveCaja = new Caja(idCajaAReabrir);
            cerrarModal(modalReabrirModal);
            mostrarMensaje("Caja Reabierta Con Exito");

        }
        catch (Exception ex)
        {
            mostrarExcepcion(ex.Message);
            cerrarModal(modalReabrirModal);
        }
    }
}