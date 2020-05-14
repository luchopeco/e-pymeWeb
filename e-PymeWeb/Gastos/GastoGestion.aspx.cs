using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Negocio;
using Entidades;

public partial class Gastos_GastoGestion : System.Web.UI.Page
{
    static string modalGasto = "gastoModal";
    static string modalConfirmacion = "confirmacionModal";

    private Gasto gastoActual
    {
        get { return (Gasto)ViewState["gastoActual"]; }
        set { ViewState["gastoActual"] = value; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        ucPanelMensajes.PanelErrorVisible = false;
        ucPanelMensajes.PanelMensajeVisible = false;
        if (!IsPostBack)
        {
            txtFechaModal.Text = DateTime.Today.ToShortDateString();

            DateTime fechaDesde = DateTime.Today;
            DateTime fechaHasta = DateTime.Today;
            txtFechaDesde.Text = fechaDesde.ToShortDateString();
            txtFechaHasta.Text = fechaHasta.ToShortDateString();
            bindGrillaGasto();
            cargaCbxTipoGasto();
            cargaCbxFormaPago();
        }
    }

    private void cargaCbxFormaPago()
    {
        try
        {
            cbxFormaPago.DataTextField = "Descripcion";
            cbxFormaPago.DataValueField = "IdtipoFormaPago";
            using (ControladorFormaPago c_fp = new ControladorFormaPago())
            {
                cbxFormaPago.DataSource = c_fp.BuscarListFormaPago().Where(fp => fp.HabilitadoGasto == true);
                cbxFormaPago.DataBind();
            }
        }
        catch (ExcepcionPropia myex)
        {
            mostrarExcepcion(myex.Message);
        }
    }

    private void cargaCbxTipoGasto()
    {
        try
        {

            cbxTipoGasto.DataTextField = "Descripcion";
            cbxTipoGasto.DataValueField = "IdTipoGasto";
            using (ControladorGastos c_gastos = new ControladorGastos())
            {
                cbxTipoGasto.DataSource = c_gastos.BuscarListTipoGasto();
                cbxTipoGasto.DataBind();
            }
        }
        catch (ExcepcionPropia myex)
        {
            mostrarExcepcion(myex.Message);
        }
    }

    private void bindGrillaGasto()
    {
        try
        {
            List<Gasto> listG;
            using (ControladorGastos c_gastos = new ControladorGastos())
            {
                listG = c_gastos.BuscarListGastos(Convert.ToDateTime(txtFechaDesde.Text), Convert.ToDateTime(txtFechaHasta.Text), ApplicationSesion.ActiveSucursal.IdSucursal);
            }
            lblTotalGastos.Text = string.Empty;
            dgvGastos.DataSource = null;
            dgvGastos.DataSource = listG;
            dgvGastos.DataBind();

            if (listG != null && listG.Count > 0)
            {
                string textoGastoAMostrar = "Totales [ ";
                lblTotalGastos.Text = "";
                var groupBy = listG.GroupBy(lg => new { lg.DescTipoGasto });
                foreach (var grupo in groupBy)
                {
                    string tipoGast = grupo.Key.DescTipoGasto;
                    string total = listG.Where(g => g.Anulado == false && g.DescTipoGasto == tipoGast).Sum(g => g.Monto).ToString();
                    textoGastoAMostrar = textoGastoAMostrar + tipoGast + ": $" + total + " | ";
                }
                textoGastoAMostrar = textoGastoAMostrar + "] Total: $" + listG.Sum(gg => gg.Monto);
                lblTotalGastos.Text = textoGastoAMostrar;
            }
        }
        catch (ExcepcionPropia myex)
        {
            mostrarExcepcion(myex.Message);
            dgvGastos.DataSource = null;
            dgvGastos.DataBind();
            lblTotalGastos.Text = "";
        }
    }

    protected void btnAceptarModalGasto_Click(object sender, EventArgs e)
    {
        try
        {
            ///Si estoy agregando
            if (gastoActual == null)
            {
                Gasto g = new Gasto();
                g.Fecha = Convert.ToDateTime(txtFechaModal.Text);
                //g.ListFormaPago
                g.Monto = Convert.ToDecimal(txtMontoModal.Text);
                g.Descripcion = txtObservaionGastoModal.Text;

                g.TipoGasto = new TipoGasto();
                g.TipoGasto.Descripcion = cbxTipoGasto.SelectedItem.Text;
                g.TipoGasto.IdTipoGasto = Convert.ToInt32(cbxTipoGasto.SelectedValue);

                FormaPago fp = new FormaPago();
                fp.Descripcion = cbxFormaPago.SelectedItem.Text;
                fp.IdtipoFormaPago = Convert.ToInt32(cbxFormaPago.SelectedValue);
                fp.Monto = g.Monto;

                g.ListFormaPago = new List<FormaPago>();
                g.ListFormaPago.Add(fp);
                g.Sucursal_ = ApplicationSesion.ActiveSucursal;
                g.IdCaja = ApplicationSesion.ActiveCaja.Idcaja;
                using (ControladorGastos c_gastos = new ControladorGastos())
                {
                    c_gastos.AgregarGasto(g);
                }
                mostrarMensaje("Gasto Agregado con Exito");
                cerrarModal(modalGasto);
                txtFechaDesde.Text = DateTime.Today.AddDays(-15).ToShortDateString();
                txtFechaHasta.Text = DateTime.Today.ToShortDateString();
                bindGrillaGasto();
            }
            ///Si estoy modificando
            else
            {

                gastoActual.Fecha = Convert.ToDateTime(txtFechaModal.Text);
                //g.ListFormaPago
                gastoActual.Monto = Convert.ToDecimal(txtMontoModal.Text);
                gastoActual.Descripcion = txtObservaionGastoModal.Text;

                gastoActual.TipoGasto = new TipoGasto();
                gastoActual.TipoGasto.Descripcion = cbxTipoGasto.SelectedItem.Text;
                gastoActual.TipoGasto.IdTipoGasto = Convert.ToInt32(cbxTipoGasto.SelectedValue);

                FormaPago fp = new FormaPago();
                fp.Descripcion = cbxFormaPago.SelectedItem.Text;
                fp.IdtipoFormaPago = Convert.ToInt32(cbxFormaPago.SelectedValue);
                fp.Monto = gastoActual.Monto;

                gastoActual.ListFormaPago = new List<FormaPago>();
                gastoActual.ListFormaPago.Add(fp);
                using (ControladorGastos c_gastos = new ControladorGastos())
                {
                    c_gastos.ModificarGasto(gastoActual);
                }
                mostrarMensaje("Gasto Modificado con Exito");
                cerrarModal(modalGasto);
                txtFechaDesde.Text = DateTime.Today.AddDays(-15).ToShortDateString();
                txtFechaHasta.Text = DateTime.Today.ToShortDateString();
                bindGrillaGasto();
            }


        }
        catch (ExcepcionPropia myex)
        {
            mostrarExcepcion(myex.Message);
            cerrarModal(modalGasto);
        }
    }
    protected void btnNuevoGasto_Click(object sender, EventArgs e)
    {
        validarCajaAbierta();
        lblTituloModalGasto.Text = "Agregando Gasto";
        lblPanelModalGasto.Text = "Gasto";
        txtMontoModal.Text = "0";
        abrirModal(modalGasto);
        btnAceptarModalGasto.Text = "Agregar";
        gastoActual = null;

    }
    public void validarCajaAbierta()
    {
        if (ApplicationSesion.ActiveCaja == null)
        {
            Response.Redirect("~/MensajeCajaCerrada.aspx");
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
    protected void btnBuscarGastos_Click(object sender, EventArgs e)
    {
        try
        {
            bindGrillaGasto();
        }
        catch (ExcepcionPropia myex)
        {
            mostrarExcepcion(myex.Message);
        }
    }
    protected void dgvGastos_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        validarCajaAbierta();
        int index = Convert.ToInt32(e.CommandArgument);
        if (e.CommandName.Equals("editar"))
        {
            try
            {
                int id = Convert.ToInt32(dgvGastos.DataKeys[index].Value);
                using (ControladorGastos c_gasto = new ControladorGastos())
                {
                    gastoActual = c_gasto.BuscarGasto(id);
                }
                txtFechaModal.Text = gastoActual.Fecha.ToShortDateString();
                txtMontoModal.Text = gastoActual.Monto.ToString();
                txtObservaionGastoModal.Text = gastoActual.Descripcion;
                cbxFormaPago.SelectedValue = gastoActual.ListFormaPago[0].IdtipoFormaPago.ToString();
                cbxTipoGasto.SelectedValue = gastoActual.TipoGasto.IdTipoGasto.ToString();

                lblTituloModalGasto.Text = "Modificando Gasto";
                lblPanelModalGasto.Text = "Gasto";
                abrirModal(modalGasto);
                btnAceptarModalGasto.Text = "Modificar";
            }
            catch (ExcepcionPropia myex)
            {
                mostrarExcepcion(myex.Message);
            }


        }
        else if (e.CommandName.Equals("eliminar"))
        {
            try
            {
                hfidGastoConfirmacionModal.Value = dgvGastos.DataKeys[index].Value.ToString();
                abrirModal(modalConfirmacion);

            }
            catch (ExcepcionPropia myex)
            {
                mostrarExcepcion(myex.Message);
            }
        }
    }
    protected void btnAceptarConfirmar_Click(object sender, EventArgs e)
    {
        try
        {
            using (ControladorGastos c_gastos = new ControladorGastos())
            {
                c_gastos.AnularGasto(Convert.ToInt32(hfidGastoConfirmacionModal.Value));
            }
            cerrarModal(modalConfirmacion);
            mostrarMensaje("Gasto Anulado Con Exito");
            bindGrillaGasto();
        }
        catch (ExcepcionPropia myex)
        {
            mostrarExcepcion(myex.Message);
            cerrarModal(modalConfirmacion);
        }
    }
    protected void btnExoprtarAExcel_Click(object sender, EventArgs e)
    {

        if (dgvGastos.Rows.Count == 0)
        {
            mostrarExcepcion("Debe Realizar una busqueda");
        }
        else
        {
            //string datestyle = @"<style> .date{ mso-number-format:\#\.000; }</style>";
            //foreach (GridViewRow oItem in dgvArticulos.Rows)
            //    oItem.Cells[3].Attributes.Add("class", "date");

            Response.Clear();
            Response.AddHeader("content-disposition", "attachment; filename=Gastos.xls");
            Response.ContentType = "application/vnd.xls";
            System.IO.StringWriter WriteItem = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter htmlText = new HtmlTextWriter(WriteItem);
            //esponse.Write(datestyle);
            dgvGastos.RenderControl(htmlText);
            Response.Write(WriteItem.ToString());
            Response.End();
        }

    }

    /// <summary>
    /// Para q funcione el exportar a excel
    /// </summary>
    /// <param name="control"></param>
    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */
    }
}