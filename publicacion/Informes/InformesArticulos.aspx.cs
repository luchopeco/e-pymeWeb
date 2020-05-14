using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Negocio;
using Entidades;

public partial class Informes_InformesArticulos : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ucPanelMensajesArticuloMuchoStock.PanelErrorVisible = false;
        ucPanelMensajesArticuloMuchoStock.PanelMensajeVisible = false;
        ucPanelMensajesPocoSotck.PanelErrorVisible = false;
        ucPanelMensajesPocoSotck.PanelMensajeVisible = false;
        if (!IsPostBack)
        {
            ///busco la sucursal
            using (ControladorSucursal c_suc = new ControladorSucursal())
            {
                lblSucursalEnTitulo.Text = "Sucursal: " + ApplicationSesion.ActiveSucursal.Descripcion;
            }
        }

    }
    protected void btnExoprtarAExcelPocoSotck_Click(object sender, EventArgs e)
    {
        if (dgvArticulosPocoStock.Rows.Count == 0)
        {
            mostrarExcepcionPocoStock("Debe Realizar una busqueda");
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
            dgvArticulosPocoStock.RenderControl(htmlText);
            Response.Write(WriteItem.ToString());
            Response.End();
        }
    }
    protected void btnExoprtarAExcelTipoArticuloPocoSotck_Click(object sender, EventArgs e)
    {
        if (dgvTipoArticulosPocoStock.Rows.Count == 0)
        {
            mostrarExcepcionPocoStock("Debe Realizar una busqueda");
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
            dgvTipoArticulosPocoStock.RenderControl(htmlText);
            Response.Write(WriteItem.ToString());
            Response.End();
        }
    }    
    protected void btnBuscarArticulosPocoStock_Click(object sender, EventArgs e)
    {
        try
        {
            int stockMenorA = Convert.ToInt32(txtCantidadMenorA.Text);
            int cantidadArticulosAMostrar = Convert.ToInt32(txtArticulosAmostrarPocoStock.Text);
            int idScursal = ApplicationSesion.ActiveSucursal.IdSucursal;
            dgvArticulosPocoStock.DataSource = null;
            dgvArticulosPocoStock.DataBind();
            using (ControladorInformes c_info = new ControladorInformes())
            {
                dgvArticulosPocoStock.DataSource = c_info.BuscarArticulosConPocoStock(stockMenorA, cantidadArticulosAMostrar, idScursal);
                dgvArticulosPocoStock.DataBind();

            }
        }
        catch (ExcepcionPropia ex)
        {
            mostrarExcepcionPocoStock(ex.Message);
        }
    }

    protected void btnBuscarTiposArticulosPocoStock_Click(object sender, EventArgs e)
    {
        try
        {
            int stockMenorA = Convert.ToInt32(txtTipoArticuloCantidadMenorA.Text);
            int cantidadArticulosAMostrar = Convert.ToInt32(txtTipoArticulosAmostrarPocoStock.Text);
            int idScursal = ApplicationSesion.ActiveSucursal.IdSucursal;
            dgvTipoArticulosPocoStock.DataSource = null;
            dgvTipoArticulosPocoStock.DataBind();
            using (ControladorInformes c_info = new ControladorInformes())
            {
                dgvTipoArticulosPocoStock.DataSource = c_info.BuscarTiposArticulosConPocoStock(stockMenorA, cantidadArticulosAMostrar, idScursal);
                dgvTipoArticulosPocoStock.DataBind();

            }
        }
        catch (ExcepcionPropia ex)
        {
            mostrarExcepcionTipoArticuloPocoStock(ex.Message);
        }
    }

    protected void btnBuscarArticulosMuchoStock_Click(object sender, EventArgs e)
    {
        try
        {
            int articulosConStockMayorA = Convert.ToInt32(txtArticulosConStockMayorA.Text);
            int cantidadArticulos = Convert.ToInt32(txtCantidadArticulosAMostrarMuchoStock.Text);
            int idScursal = ApplicationSesion.ActiveSucursal.IdSucursal;
            dgvArticulosMuchoStock.DataSource = null;
            dgvArticulosMuchoStock.DataBind();
            using (ControladorInformes c_info = new ControladorInformes())
            {
                dgvArticulosMuchoStock.DataSource = c_info.BuscarArticulosConMuchoStock(articulosConStockMayorA, cantidadArticulos, idScursal);
                dgvArticulosMuchoStock.DataBind();

            }
        }
        catch (ExcepcionPropia ex)
        {
            mostrarExcepcionMNuchoStock(ex.Message);
        }
    }

    protected void btnBuscarTiposArticulosMuchoStock_Click(object sender, EventArgs e)
    {
        try
        {
            int articulosConStockMayorA = Convert.ToInt32(txtTiposArticulosConStockMayorA.Text);
            int cantidadArticulos = Convert.ToInt32(txtCantidadTiposArticulosAMostrarMuchoStock.Text);
            int idScursal = ApplicationSesion.ActiveSucursal.IdSucursal;
            dgvTiposArticulosMuchoStock.DataSource = null;
            dgvTiposArticulosMuchoStock.DataBind();
            using (ControladorInformes c_info = new ControladorInformes())
            {
                dgvTiposArticulosMuchoStock.DataSource = c_info.BuscarTiposArticulosConMuchoStock(articulosConStockMayorA, cantidadArticulos, idScursal);
                dgvTiposArticulosMuchoStock.DataBind();

            }
        }
        catch (ExcepcionPropia ex)
        {
            mostrarExcepcionTipoArticuloMuchoStock(ex.Message);
        }
    }


    

    protected void btnExoprtarAExcelArticulosMuchoStock_Click(object sender, EventArgs e)
    {
        if (dgvArticulosMuchoStock.Rows.Count == 0)
        {
            mostrarExcepcionMNuchoStock("Debe Realizar una busqueda");
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
            dgvArticulosMuchoStock.RenderControl(htmlText);
            Response.Write(WriteItem.ToString());
            Response.End();
        }
    }
   

    protected void btnExoprtarAExcelTiposArticulosMuchoStock_Click(object sender, EventArgs e)
    {
        if (dgvTiposArticulosMuchoStock.Rows.Count == 0)
        {
            mostrarExcepcionTipoArticuloMuchoStock("Debe Realizar una busqueda");
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
            dgvTiposArticulosMuchoStock.RenderControl(htmlText);
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


    private void mostrarExcepcionPocoStock(string msj)
    {
        ucPanelMensajesPocoSotck.PanelErrorVisible = true;
        ucPanelMensajesPocoSotck.LblError = msj;
        System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
        sb1.Append(@"<script type='text/javascript'>");
        sb1.Append("var focalizar = $('#foco').position().top;");
        sb1.Append("$('html,body').animate({scrollTop: focalizar}, 500);");
        sb1.Append(@"</script>");
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", sb1.ToString(), false);
    }

    private void mostrarExcepcionTipoArticuloPocoStock(string msj)
    {
        ucPanelMensajesTipoArticuloPocoSotck.PanelErrorVisible = true;
        ucPanelMensajesTipoArticuloPocoSotck.LblError = msj;
        System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
        sb1.Append(@"<script type='text/javascript'>");
        sb1.Append("var focalizar = $('#foco').position().top;");
        sb1.Append("$('html,body').animate({scrollTop: focalizar}, 500);");
        sb1.Append(@"</script>");
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", sb1.ToString(), false);
    }

    private void mostrarExcepcionMNuchoStock(string msj)
    {

        ucPanelMensajesArticuloMuchoStock.PanelErrorVisible = true;
        ucPanelMensajesArticuloMuchoStock.LblError = msj;
        System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
        sb1.Append(@"<script type='text/javascript'>");
        sb1.Append("var focalizar = $('#foco').position().top;");
        sb1.Append("$('html,body').animate({scrollTop: focalizar}, 500);");
        sb1.Append(@"</script>");
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", sb1.ToString(), false);
    }

    private void mostrarExcepcionTipoArticuloMuchoStock(string msj)
    {

        ucPanelMensajesTipoArticuloMuchoStock.PanelErrorVisible = true;
        ucPanelMensajesTipoArticuloMuchoStock.LblError = msj;
        System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
        sb1.Append(@"<script type='text/javascript'>");
        sb1.Append("var focalizar = $('#foco').position().top;");
        sb1.Append("$('html,body').animate({scrollTop: focalizar}, 500);");
        sb1.Append(@"</script>");
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", sb1.ToString(), false);
    }
}