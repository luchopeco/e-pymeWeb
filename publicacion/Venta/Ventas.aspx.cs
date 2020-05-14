using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Negocio;
using Entidades;

public partial class Venta_Ventas : System.Web.UI.Page
{
    private static string modalHistorial = "historiaCambiosModal";
    private static string modalModificarDescripcion = "modificarDescripcionModal";
    private static string modalEliminar = "eliminarModal";

    protected void Page_Load(object sender, EventArgs e)
    {
        PanelError.Visible = false;
        PanelMensaje.Visible = false;
        if (!IsPostBack)
        {
            if (Request.QueryString["m"] != null)
            {
                PanelMensaje.Visible = true;
                LabelMensaje.Text = Request.QueryString["m"].ToString();
            }
            txtFechaDesde.Text = DateTime.Today.ToShortDateString();
            txtFechaHasta.Text = DateTime.Today.ToShortDateString();
            bindGrid();
        }
    }

    private void bindGrid()
    {
        try
        {
            dgvVentas.DataSource = null;
            dgvVentas.DataBind();
            lblTotal.Text = string.Empty;
            DateTime fechaDesde = Convert.ToDateTime(txtFechaDesde.Text);
            DateTime fechaHasta = Convert.ToDateTime(txtFechaHasta.Text);
            List<Venta> listV = new List<Venta>();
            using (ControladorVentas c_ventas = new ControladorVentas())
            {
                listV = c_ventas.BuscarListVentas(fechaDesde, fechaHasta, ApplicationSesion.ActiveSucursal.IdSucursal);
            }

            List<FormaPago> listFP = new List<FormaPago>();
            foreach (Venta v in listV)
            {
                listFP.AddRange(v.ListFormaPago);
            }

            string textoGastoAMostrar = "Totales [ ";
            lblTotal.Text = "";
            var groupBy = listFP.GroupBy(lv => new { lv.Descripcion });
            foreach (var grupo in groupBy)
            {
                string tipoGast = grupo.Key.Descripcion;
                string total = listFP.Where(c => c.Descripcion == tipoGast).Sum(c => c.Monto).ToString();
                textoGastoAMostrar = textoGastoAMostrar + tipoGast + ": $" + total + " | ";
            }
            textoGastoAMostrar = textoGastoAMostrar + "] Total: $" + listV.Sum(gg => gg.Total);
            lblTotal.Text = textoGastoAMostrar;

            dgvVentas.DataSource = listV;
            dgvVentas.DataBind();
           // lblTotal.Text = "Total: $" + listV.Sum(v => v.Total).ToString();
        }
        catch (ExcepcionPropia mye)
        {
            PanelError.Visible = true;
            lblError.Text = mye.Message;

        }
        catch (FormatException mye)
        {
            PanelError.Visible = true;
            lblError.Text = mye.Message;

        }
    }
    protected void btnNuevaVenta_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Venta/VentaNueva.aspx");
    }
    protected void btnBuscarVentas_Click(object sender, EventArgs e)
    {
        bindGrid();
    }
    protected void dgvVentas_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName.Equals("detVenta"))
            {
                Venta v;
                using (ControladorVentas c_ventas = new ControladorVentas())
                {
                    v = c_ventas.BuscarVenta(Convert.ToInt32(dgvVentas.DataKeys[index].Value));

                }

                hfIdVenta.Value = v.Idventa.ToString();
                List<Venta> listv = new List<Venta>();
                listv.Add(v);
                dvDetalleVenta.DataSource = listv;
                dvDetalleVenta.DataBind();
                dgvArticulos.DataSource = v.ListLineaVenta;
                dgvArticulos.DataBind();

                dgvFormaPago.DataSource = v.ListFormaPago;
                dgvFormaPago.DataBind();

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append("$('#detModal').modal('show');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "confirmaciohowModalScript", sb.ToString(), false);
            }
            else if (e.CommandName.Equals("cambio"))
            {
                Response.Redirect("~/Venta/VentaCambio.aspx?id=" + dgvVentas.DataKeys[index].Value.ToString());
            }
            else if (e.CommandName.Equals("notaCredito"))
            {
                Response.Redirect("~/Venta/VentaNotaCredito.aspx?id=" + dgvVentas.DataKeys[index].Value.ToString());
            }
            else if (e.CommandName.Equals("cambios"))
            {
                int idVenta = Convert.ToInt32(dgvVentas.DataKeys[index].Value);
                List<VentaLineaCambio> listV;
                using (ControladorVentas c_venta = new ControladorVentas())
                {
                    listV = c_venta.BuscarListCambios(idVenta);
                }
                dgvHistorialCambio.DataSource = listV;
                dgvHistorialCambio.DataBind();
                abrirModal(modalHistorial);
            }
            else if (e.CommandName.Equals("editarDescripcion"))
            {
                Venta v;
                using (ControladorVentas c_ventas = new ControladorVentas())
                {
                    v = c_ventas.BuscarVenta(Convert.ToInt32(dgvVentas.DataKeys[index].Value));
                }
                hfIdVentaModificarDescripcion.Value = v.Idventa.ToString();
                txtDescripcionModificarModal.Text = v.Descripcion;
                abrirModal(modalModificarDescripcion);
            }
            else if (e.CommandName.Equals("eliminar"))
            {
                hfIdVentaAeliminar.Value = dgvVentas.DataKeys[index].Value.ToString();
                abrirModal(modalEliminar);
            }
            else if (e.CommandName.Equals("editarFormaPago"))
            {
                Response.Redirect("~/Venta/FormaPagoModificar.aspx?id=" + dgvVentas.DataKeys[index].Value.ToString());
            }


        }
        catch (ExcepcionPropia ex)
        {
            mostrarExcepcion(ex.Message);
        }
    }
    //protected void dgvArticulos_RowCommand(object sender, GridViewCommandEventArgs e)
    //{                  
    //}
    //protected void btnAceptarModalConfirmacion_Click(object sender, EventArgs e)
    //{      
    //}
    protected void dgvArticulos_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            //string sssss;
            //sssss = e.Row.Cells[5].Controls[0].ToString();
            //bool prueba = true;
            //prueba = ((System.Web.UI.WebControls.CheckBox)(e.Row.Cells[4].Controls[0])).Checked;
            if (((System.Web.UI.WebControls.CheckBox)(e.Row.Cells[4].Controls[0])).Checked)
            {
                e.Row.CssClass = "danger";
            }
        }
        catch (Exception)
        {


        }
    }
    protected void btnExoprtarAExcel_Click(object sender, EventArgs e)
    {
        if (dgvVentas.Rows.Count == 0)
        {
            mostrarExcepcion("Debe Realizar una busqueda");
        }
        else
        {
            string datestyle = @"<style> .date{ mso-number-format:\#\.000; }</style>";
            //foreach (GridViewRow oItem in dgvArticulos.Rows)
            //    oItem.Cells[3].Attributes.Add("class", "date");

            Response.Clear();
            Response.AddHeader("content-disposition", "attachment; filename=Ventas.xls");
            Response.ContentType = "application/vnd.xls";
            System.IO.StringWriter WriteItem = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter htmlText = new HtmlTextWriter(WriteItem);
            Response.Write(datestyle);
            dgvVentas.RenderControl(htmlText);
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

    private void mostrarMensaje(string msj)
    {
        PanelMensaje.Visible = true;
        LabelMensaje.Text = msj;
        System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
        sb1.Append(@"<script type='text/javascript'>");
        sb1.Append("var focalizar = $('#foco').position().top;");
        sb1.Append("$('html,body').animate({scrollTop: focalizar}, 500);");
        sb1.Append(@"</script>");
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", sb1.ToString(), false);
    }

    private void mostrarExcepcion(string msj)
    {
        PanelError.Visible = true;
        lblError.Text = msj;
        System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
        sb1.Append(@"<script type='text/javascript'>");
        sb1.Append("var focalizar = $('#foco').position().top;");
        sb1.Append("$('html,body').animate({scrollTop: focalizar}, 500);");
        sb1.Append(@"</script>");
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", sb1.ToString(), false);
    }
    protected void dgvVentas_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[5].ToolTip = "Detalle Venta";
            e.Row.Cells[6].ToolTip = "Modificar Descripcion";
            e.Row.Cells[7].ToolTip = "Generar Cambio";
            e.Row.Cells[8].ToolTip = "Modificar Formas de Pago";
            e.Row.Cells[9].ToolTip = "Generar Nota Credito";
            e.Row.Cells[10].ToolTip = "Historial de Cambios";
            e.Row.Cells[11].ToolTip = "Eliminar Venta";
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
    protected void btnAceptarModificarDescModal_Click(object sender, EventArgs e)
    {
        try
        {
            int idVenta = Convert.ToInt32(hfIdVentaModificarDescripcion.Value);
            using (ControladorVentas c_ventas = new ControladorVentas())
            {
                c_ventas.ModificarVenta(idVenta, txtDescripcionModificarModal.Text);
            }
            cerrarModal(modalModificarDescripcion);
            mostrarMensaje("Descripcion de venta Modificada con exito");
            bindGrid();
        }
        catch (ExcepcionPropia ex)
        {
            cerrarModal(modalModificarDescripcion);
            mostrarExcepcion(ex.Message);
        }
    }
    protected void btnAceptarEliminar_Click(object sender, EventArgs e)
    {
        try
        {
            using (ControladorVentas c_ventas = new ControladorVentas())
            {
                c_ventas.EliminarVenta(Convert.ToInt32(hfIdVentaAeliminar.Value));
            }
            mostrarMensaje("Venta Eliminada Con Exito");
            bindGrid();
            cerrarModal(modalEliminar);
        }
        catch (ExcepcionPropia ex)
        {
            mostrarExcepcion(ex.Message);
            cerrarModal(modalEliminar);
        }
    }
}