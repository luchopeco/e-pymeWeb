using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Negocio;
using Entidades;

public partial class Venta_NotasCreditosGestion : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ucPanelMensajes.PanelErrorVisible = false;
        ucPanelMensajes.PanelMensajeVisible = false;
        if (!IsPostBack)
        {
            txtFechaDesde.Text = DateTime.Today.AddMonths(-1).ToShortDateString();
            txtFechaHasta.Text = DateTime.Today.ToShortDateString();
            bindGrillaNotaCredito();

        }
    }
    protected void btnBuscarVentas_Click(object sender, EventArgs e)
    {
        bindGrillaNotaCredito();
    }

    private void bindGrillaNotaCredito()
    {
        try
        {
            DateTime fechaDesde = Convert.ToDateTime(txtFechaDesde.Text);
            DateTime fechaHasta = Convert.ToDateTime(txtFechaHasta.Text);
            List<NotaCredito> listNc;
            using (ControladorVentas c_ventas = new ControladorVentas())
            {
                listNc = c_ventas.BuscarListNotaCredito(fechaDesde, fechaHasta);
            }
            dgvNotaCredito.DataSource = null;
            dgvNotaCredito.DataSource = listNc;
            dgvNotaCredito.DataBind();
        }
        catch (ExcepcionPropia myex)
        {

            mostrarExcepcion(myex.Message);
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
}