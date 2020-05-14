using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Negocio;
using Entidades;

public partial class Venta_Informes_InformesVentas : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ucPanelMensajesArticulo.PanelErrorVisible = false;
        ucPanelMensajesArticulo.PanelMensajeVisible = false;
        ucPanelMensajesMarcas.PanelErrorVisible = false;
        ucPanelMensajesMarcas.PanelMensajeVisible = false;
        ucPanelMensajesTipoArticulo.PanelErrorVisible = false;
        ucPanelMensajesTipoArticulo.PanelMensajeVisible = false;
        if (!IsPostBack)
        {
            ///busco la sucursal
            using (ControladorSucursal c_suc = new ControladorSucursal())
            {
                lblSucursalTitulo.Text = "Sucursal: " + ApplicationSesion.ActiveSucursal.Descripcion;
            }
        }
    }


    protected void btnBuscarArticulos_Click(object sender, EventArgs e)
    {
        try
        {
            DateTime fechaDesde = Convert.ToDateTime(txtFechaDesdeArticulos.Text);
            DateTime fechaHasta = Convert.ToDateTime(txtFechaHastaArticulos.Text);
            int idSucursal = ApplicationSesion.ActiveSucursal.IdSucursal;
            dgvTotalesPorArticulo.DataSource = null;
            dgvTotalesPorArticulo.DataBind();
            using (ControladorInformes c_info = new ControladorInformes())
            {
                dgvTotalesPorArticulo.DataSource = c_info.BuscarVentasCantidadesYTotalesPorArticulos(fechaDesde, fechaHasta, idSucursal);
                dgvTotalesPorArticulo.DataBind();

            }

        }
        catch (ExcepcionPropia ex)
        {
            mostrarExcepcionArticulo(ex.Message);
        }
    }
    protected void btnBuscarMarcas_Click(object sender, EventArgs e)
    {
        try
        {
            DateTime fechaDesde = Convert.ToDateTime(txtFechaDesdeMarcas.Text);
            DateTime fechaHasta = Convert.ToDateTime(txtFechaHastaMarcas.Text);
            int idSucursal = ApplicationSesion.ActiveSucursal.IdSucursal;
            dgvTotalesPorMarca.DataSource = null;
            dgvTotalesPorMarca.DataBind();
            using (ControladorInformes c_info = new ControladorInformes())
            {
                dgvTotalesPorMarca.DataSource = c_info.BuscarVentasCantidadesYTotalesPorMarcas(fechaDesde, fechaHasta, idSucursal);
                dgvTotalesPorMarca.DataBind();

            }
        }
        catch (ExcepcionPropia ex)
        {
            mostrarExcepcionMarcas(ex.Message);
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
    protected void btnExoprtarAExcelArticulos_Click(object sender, EventArgs e)
    {
        if (dgvTotalesPorArticulo.Rows.Count == 0)
        {
            mostrarExcepcionArticulo("Debe Realizar una busqueda");
        }
        else
        {
            //string datestyle = @"<style> .date{ mso-number-format:\#\.000; }</style>";
            //foreach (GridViewRow oItem in dgvArticulos.Rows)
            //    oItem.Cells[3].Attributes.Add("class", "date");

            Response.Clear();
            Response.AddHeader("content-disposition", "attachment; filename=Archivo.xls");
            Response.ContentType = "application/vnd.xls";
            System.IO.StringWriter WriteItem = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter htmlText = new HtmlTextWriter(WriteItem);
            //Response.Write(datestyle);
            dgvTotalesPorArticulo.RenderControl(htmlText);
            Response.Write(WriteItem.ToString());
            Response.End();
        }
    }
    protected void btnExoprtarAExcelMarcas_Click(object sender, EventArgs e)
    {
        if (dgvTotalesPorMarca.Rows.Count == 0)
        {
            mostrarExcepcionMarcas("Debe Realizar una busqueda");
        }
        else
        {
            //string datestyle = @"<style> .date{ mso-number-format:\#\.000; }</style>";
            //foreach (GridViewRow oItem in dgvArticulos.Rows)
            //    oItem.Cells[3].Attributes.Add("class", "date");

            Response.Clear();
            Response.AddHeader("content-disposition", "attachment; filename=Archivo.xls");
            Response.ContentType = "application/vnd.xls";
            System.IO.StringWriter WriteItem = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter htmlText = new HtmlTextWriter(WriteItem);
            //Response.Write(datestyle);
            dgvTotalesPorMarca.RenderControl(htmlText);
            Response.Write(WriteItem.ToString());
            Response.End();
        }
    }


    private void mostrarExcepcionArticulo(string msj)
    {
        ucPanelMensajesArticulo.PanelErrorVisible = true;
        ucPanelMensajesArticulo.LblError = msj;
        System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
        sb1.Append(@"<script type='text/javascript'>");
        sb1.Append("var focalizar = $('#foco').position().top;");
        sb1.Append("$('html,body').animate({scrollTop: focalizar}, 500);");
        sb1.Append(@"</script>");
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", sb1.ToString(), false);
    }
    private void mostrarExcepcionMarcas(string msj)
    {

        ucPanelMensajesMarcas.PanelErrorVisible = true;
        ucPanelMensajesMarcas.LblError = msj;
        System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
        sb1.Append(@"<script type='text/javascript'>");
        sb1.Append("var focalizar = $('#foco').position().top;");
        sb1.Append("$('html,body').animate({scrollTop: focalizar}, 500);");
        sb1.Append(@"</script>");
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", sb1.ToString(), false);
    }
    private void mostrarExcepcionTipoArticulo(string msj)
    {

        ucPanelMensajesMarcas.PanelErrorVisible = true;
        ucPanelMensajesMarcas.LblError = msj;
        System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
        sb1.Append(@"<script type='text/javascript'>");
        sb1.Append("var focalizar = $('#foco').position().top;");
        sb1.Append("$('html,body').animate({scrollTop: focalizar}, 500);");
        sb1.Append(@"</script>");
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", sb1.ToString(), false);
    }
    private void mostrarExcepcionFormaPago(string msj)
    {

        ucPanelMensajesVentas.PanelErrorVisible = true;
        ucPanelMensajesVentas.LblError = msj;
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
    protected void btnBuscarVentasTipoArt_Click(object sender, EventArgs e)
    {
        try
        {
            DateTime fechaDesde = Convert.ToDateTime(txtFechaDesdeTipoArt.Text);
            DateTime fechaHasta = Convert.ToDateTime(txtFechaHastaTipoArt.Text);
            int idSucursal = ApplicationSesion.ActiveSucursal.IdSucursal;
            dgvTotalesTipoArt.DataSource = null;
            dgvTotalesTipoArt.DataBind();
            using (ControladorInformes c_info = new ControladorInformes())
            {
                dgvTotalesTipoArt.DataSource = c_info.BuscarVentasCantidadesYTotalesPorTipoArticulo(fechaDesde, fechaHasta,idSucursal);
                dgvTotalesTipoArt.DataBind();

            }
        }
        catch (ExcepcionPropia ex)
        {
            mostrarExcepcionTipoArticulo(ex.Message);
        }
    }
    protected void btnExportarAExcelTipoArticulo_Click(object sender, EventArgs e)
    {
        if (dgvTotalesTipoArt.Rows.Count == 0)
        {
            mostrarExcepcionTipoArticulo("Debe Realizar una busqueda");
        }
        else
        {
            //string datestyle = @"<style> .date{ mso-number-format:\#\.000; }</style>";
            //foreach (GridViewRow oItem in dgvArticulos.Rows)
            //    oItem.Cells[3].Attributes.Add("class", "date");

            Response.Clear();
            Response.AddHeader("content-disposition", "attachment; filename=Archivo.xls");
            Response.ContentType = "application/vnd.xls";
            System.IO.StringWriter WriteItem = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter htmlText = new HtmlTextWriter(WriteItem);
            //Response.Write(datestyle);
            dgvTotalesTipoArt.RenderControl(htmlText);
            Response.Write(WriteItem.ToString());
            Response.End();
        }
    }
    protected void btnExportarAExcelFormaPago_Click(object sender, EventArgs e)
    {
        if (dgvFormaPago.Rows.Count == 0)
        {
            mostrarExcepcionFormaPago("Debe Realizar una busqueda");
        }
        else
        {
            //string datestyle = @"<style> .date{ mso-number-format:\#\.000; }</style>";
            //foreach (GridViewRow oItem in dgvArticulos.Rows)
            //    oItem.Cells[3].Attributes.Add("class", "date");

            Response.Clear();
            Response.AddHeader("content-disposition", "attachment; filename=Archivo.xls");
            Response.ContentType = "application/vnd.xls";
            System.IO.StringWriter WriteItem = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter htmlText = new HtmlTextWriter(WriteItem);
            //Response.Write(datestyle);
            dgvFormaPago.RenderControl(htmlText);
            Response.Write(WriteItem.ToString());
            Response.End();
        }
    }
    protected void btnBuscarFormasPago_Click(object sender, EventArgs e)
    {
        try
        {
            DateTime fechaDesde = Convert.ToDateTime(txtFechaDesdeFormaPago.Text);
            DateTime fechaHasta = Convert.ToDateTime(txtFechaHastaFormaPago.Text);
            int idSucursal = ApplicationSesion.ActiveSucursal.IdSucursal;
            dgvFormaPago.DataSource = null;
            dgvFormaPago.DataBind();
            using (ControladorInformes c_info = new ControladorInformes())
            {
                dgvFormaPago.DataSource = c_info.BuscarVentasTotalesPorFormaPago(fechaDesde, fechaHasta,idSucursal);
                dgvFormaPago.DataBind();

            }
        }
        catch (ExcepcionPropia ex)
        {
            mostrarExcepcionFormaPago(ex.Message);
        }
    }
}