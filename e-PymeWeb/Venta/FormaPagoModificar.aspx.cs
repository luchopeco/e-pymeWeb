using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entidades;
using Negocio;

public partial class Venta_FormaPagoModificar : System.Web.UI.Page
{
    protected static string modalFormaPagoModal = "formaPagoModal";
    protected static string modalConfirmarModal = "confirmarModal";
    private NotaCredito notaCreditoActual
    {
        get
        {
            return (NotaCredito)ViewState["notaCreditoActual"];
        }
        set
        {
            ViewState["notaCreditoActual"] = value;
        }
    }
    private Venta ventaActual
    {
        get { return (Venta)ViewState["ventaActual"]; }
        set { ViewState["ventaActual"] = value; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ucPanelMensajes.PanelErrorVisible = false;
            ucPanelMensajes.PanelMensajeVisible = false;
            if (!IsPostBack)
            {
                if (Request.QueryString["id"] != null)
                {
                    cargarCbxFormaPago();
                    using (ControladorVentas c_ventas = new ControladorVentas())
                    {
                        ventaActual = c_ventas.BuscarVenta(Convert.ToInt32(Request.QueryString["id"]));
                    }
                    dgvArticulos.DataSource = ventaActual.ListLineaVenta;
                    dgvArticulos.DataBind();

                    bindGrillaFP();

                    List<Venta> listV = new List<Venta>();
                    listV.Add(ventaActual);
                    dvDetalleVenta.DataSource = listV;
                    dvDetalleVenta.DataBind();
                    lblTotal.Text = "Total : $" + ventaActual.Total.ToString();
                }
            }
        }
        catch (Exception ex)
        {
            mostrarExcepcion(ex.Message);
        }

    }

    private void bindGrillaFP()
    {
        dgvFormaPago.DataSource = ventaActual.ListFormaPago;
        dgvFormaPago.DataBind();
    }
    protected void dgvFormaPago_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index = Convert.ToInt32(e.CommandArgument);
        if (e.CommandName.Equals("deleteFP"))
        {
            int idfp = Convert.ToInt32(dgvFormaPago.DataKeys[index].Value);
            ventaActual.ListFormaPago.RemoveAll(fp => fp.IdtipoFormaPago == idfp);
            bindGrillaFP();
            mostrarMensaje("Forma de pago eliminada con exito");
        }
    }

    protected void btnNuevaFormaPago_Click(object sender, EventArgs e)
    {
        notaCreditoActual = null;
        txtMontoFP.Text = "0";
        abrirModal(modalFormaPagoModal);
    }
    protected void btnAgregarFormaPAgo_Click(object sender, EventArgs e)
    {

        try
        {
            FormaPago fp;
            using (ControladorFormaPago c_fp = new ControladorFormaPago())
            {
                fp = c_fp.BuscarFormaPago(Convert.ToInt32(cbxTipoFormaPago.SelectedValue));
            }
            if (fp.AceptaNotaCredito && notaCreditoActual == null)
            {
                mostrarExcepcionFormaPago("La forma de pago exige una nota de credito");
            }
            else
            {
                fp.Monto = Convert.ToDecimal(txtMontoFP.Text);
                fp.NotaCredito = notaCreditoActual;
                if (ventaActual.ListFormaPago == null)
                {
                    ventaActual.ListFormaPago = new List<FormaPago>();
                }

                ventaActual.ListFormaPago.Add(fp);
                dgvFormaPago.DataSource = ventaActual.ListFormaPago;
                dgvFormaPago.DataBind();
                cerrarModal(modalFormaPagoModal);
            }

        }
        catch (Exception ex)
        {
            mostrarExcepcion(ex.Message);
            cerrarModal(modalFormaPagoModal);
        }
    }

    private void cargarCbxFormaPago()
    {
        List<FormaPago> listFp;
        using (ControladorFormaPago c_fp = new ControladorFormaPago())
        {
            listFp = c_fp.BuscarListFormaPago().Where(f => f.HabilitadoVenta == true).OrderBy(f => f.Descripcion).ToList();
        }
        cbxTipoFormaPago.DataValueField = "IdtipoFormaPago";
        cbxTipoFormaPago.DataTextField = "Descripcion";
        cbxTipoFormaPago.DataSource = listFp;
        cbxTipoFormaPago.DataBind();

        int idFp = Convert.ToInt32(cbxTipoFormaPago.SelectedValue);
        if (listFp.FirstOrDefault(fp => fp.IdtipoFormaPago == idFp).AceptaNotaCredito)
        {
            panelNotaCredito.Visible = true;
        }
        else
        {
            panelNotaCredito.Visible = false;
        }

    }


    protected void cbxTipoFormaPago_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

        }
        catch (Exception ex)
        {
            mostrarExcepcion(ex.Message);
            cerrarModal(modalFormaPagoModal);
        }
    }

    protected void btnResetarNotaCredito_Click(object sender, EventArgs e)
    {
        notaCreditoActual = null;
        txtNroNotaCredito.Text = string.Empty;
        txtMontoFP.Enabled = true;
        txtNroNotaCredito.Enabled = true;
    }
    protected void btnBuscarNroCredito_Click(object sender, EventArgs e)
    {
        try
        {
            NotaCredito nc;
            using (ControladorVentas c_ventas = new ControladorVentas())
            {
                nc = c_ventas.BuscarNotaCreditoXNumero(Convert.ToInt32(txtNroNotaCredito.Text));
            }
            if (nc.UtilizadaEnVenta)
            {
                mostrarExcepcionFormaPago("La nota de credito se encuentra utilizada");
            }
            else if (nc.FechaVto < DateTime.Today)
            {
                mostrarExcepcionFormaPago("La nota de credito se encuentra vencida");
            }
            else
            {
                notaCreditoActual = nc;
                txtNroNotaCredito.Enabled = false;
                txtMontoFP.Text = notaCreditoActual.Monto.ToString();
                txtMontoFP.Enabled = false;

            }
        }
        catch (ExcepcionPropia myex)
        {
            mostrarExcepcionFormaPago(myex.Message);
        }
        catch (FormatException myex)
        {
            mostrarExcepcionFormaPago(myex.Message);
        }
    }

    private void mostrarMensajeFormaPago(string msj)
    {
        ucPanelMensajesFormaPago.PanelMensajeVisible = true;
        ucPanelMensajesFormaPago.LblMensaje = msj;
        System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
        sb1.Append(@"<script type='text/javascript'>");
        sb1.Append("var focalizar = $('#foco').position().top;");
        sb1.Append("$('html,body').animate({scrollTop: focalizar}, 500);");
        sb1.Append(@"</script>");
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", sb1.ToString(), false);
    }
    private void mostrarExcepcionFormaPago(string msj)
    {
        ucPanelMensajesFormaPago.PanelErrorVisible = true;
        ucPanelMensajesFormaPago.LblError = msj;
        System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
        sb1.Append(@"<script type='text/javascript'>");
        sb1.Append("var focalizar = $('#foco').position().top;");
        sb1.Append("$('html,body').animate({scrollTop: focalizar}, 500);");
        sb1.Append(@"</script>");
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", sb1.ToString(), false);
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
    protected void btnAceptarFinal_Click(object sender, EventArgs e)
    {
        try
        {
            abrirModal(modalConfirmarModal);
        }
        catch (Exception ex)
        {
            mostrarExcepcion(ex.Message);
            cerrarModal(modalFormaPagoModal);
        }
    }
    protected void btnAceptarModalFinal_Click(object sender, EventArgs e)
    {
        try
        {
            if (ventaActual.Total != ventaActual.ListFormaPago.Sum(fp => fp.Monto))
            {
                mostrarExcepcion("El total de las formas de pago debe ser igual al total de la venta");
                cerrarModal(modalConfirmarModal);
                return;
            }
            else
            {
                using (ControladorVentas c_v = new ControladorVentas())
                {
                    c_v.ModificarFormasPago(ventaActual.ListFormaPago, ventaActual.Idventa);
                }
                Response.Redirect("~/Default.aspx?m=Formas de pago modificadas con exito");
            }
        }
        catch (Exception ex)
        {
            mostrarExcepcion(ex.Message);
            cerrarModal(modalConfirmarModal);
        }
    }
}