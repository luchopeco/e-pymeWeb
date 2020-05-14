using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entidades;
using Negocio;

public partial class Articulo_MovimientosArticulos : System.Web.UI.Page
{
    static string modalConfirmacion = "confirmacionModal";
    private bool esAjujsteStock
    {
        get { return (bool)ViewState["esAjujsteStock"]; }
        set { ViewState["esAjujsteStock"] = value; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        ucPanelMensajes.PanelErrorVisible = false;
        ucPanelMensajes.PanelMensajeVisible = false;
        if (!IsPostBack)
        {
            hfIdSucursal.Value = ApplicationSesion.ActiveSucursal.IdSucursal.ToString();
            ucCbxSucursalesDesde.SelectedValue = Convert.ToInt32(hfIdSucursal.Value);
            ucCbxSucursalesDesde.CbxArticuloAgrupacionEnable = false;

            ucCbxSucursalesStock.SelectedValue = Convert.ToInt32(hfIdSucursal.Value);
            ucCbxSucursalesStock.CbxArticuloAgrupacionEnable = false;

            txtFechaDesde.Text = DateTime.Today.AddDays(-15).ToShortDateString();
            txtFechaHasta.Text = DateTime.Today.ToShortDateString();
            bindGrillaMovimientos();
        }
    }

    private void bindGrillaMovimientos()
    {
        try
        {
            DateTime fechaDesde = Convert.ToDateTime(txtFechaDesde.Text);
            DateTime fechaHasta = Convert.ToDateTime(txtFechaHasta.Text);
            List<MovimientosArticulos> listm = new List<MovimientosArticulos>();
            using (ControladorMovimientos c_mov = new ControladorMovimientos())
            {
                listm = c_mov.BuscarListMovimientosArtculos(fechaDesde, fechaHasta, ApplicationSesion.ActiveSucursal.IdSucursal);
            }
            if (listm == null || listm.Count == 0)
            {
                dgvMovimientos.DataSource = null;
                dgvMovimientos.DataBind();
                mostrarExcepcion("No se han encontrado Movimientos");
            }
            else
            {
                dgvMovimientos.DataSource = listm;
                dgvMovimientos.DataBind();
            }
        }
        catch (ExcepcionPropia ex)
        {
            mostrarExcepcion(ex.Message);
        }
    }
    protected void btnAgregarMovimiento_Click(object sender, EventArgs e)
    {
        esAjujsteStock = true;
        lblModal.Text = "¿Desea agregar el Ajuste de stock?";
        abrirModal(modalConfirmacion);
    }
    protected void btnAgregarMovimientoSucursal_Click(object sender, EventArgs e)
    {
        esAjujsteStock = false;
        lblModal.Text = "¿Desea agregar el movimiento entre sucursales?";
        abrirModal(modalConfirmacion);
    }
    protected void btnAceptarModal_Click(object sender, EventArgs e)
    {
        try
        {
            if (esAjujsteStock)
            {
                MovimientosArticulos m = new MovimientosArticulos();
                m.Articulo_ = new Articulo();
                m.Articulo_.Idarticulo = Convert.ToInt32(hfIdArticuloStock.Value);

                m.Cantidad = Convert.ToInt32(txtCantidadStock.Text);
                m.Fecha = DateTime.Today;
                m.IdUsuario = ApplicationSesion.ActiveUser.Idusuario;

                m.Observacion = txtObservacionesStock.Text;
                m.SucursalDesde = new Sucursal();
                m.SucursalDesde.IdSucursal = ucCbxSucursalesStock.SelectedValue;

                m.TipoMovimiento = new TipoMovimientoArticulo();
                m.TipoMovimiento.IdTipoMovimiento = ucCbxTipoMovimientoArticulo.SelectedValue;

                using (ControladorMovimientos c_mov = new ControladorMovimientos())
                {
                    c_mov.AgregarAjusteStock(m);
                }

                txtDescArtciuloStock.Text = "";
                txtObservacionesStock.Text = "";
                txtCantidadStock.Text = "";
                hfIdArticuloStock.Value = "";
                cerrarModal(modalConfirmacion);

                mostrarMensaje("Ajuste de Stock agregado con exito");

            }
            else
            {
                MovimientosArticulos m = new MovimientosArticulos();
                m.Articulo_ = new Articulo();
                m.Articulo_.Idarticulo = Convert.ToInt32(hfIdArticuloDescripcion.Value);

                m.Cantidad = Convert.ToInt32(txtCantidadSucursal.Text);
                m.Fecha = DateTime.Today;
                m.IdUsuario = ApplicationSesion.ActiveUser.Idusuario;

                m.Observacion = txtObservacionesSucursal.Text;
                m.SucursalDesde = new Sucursal();
                m.SucursalDesde.IdSucursal = ucCbxSucursalesDesde.SelectedValue;

                m.SucursalHasta = new Sucursal();
                m.SucursalHasta.IdSucursal = ucCbxSucursalHasta.SelectedValue;

                using (ControladorMovimientos c_mov = new ControladorMovimientos())
                {
                    c_mov.AgregarMovimientoEntreSucursales(m);
                }

                txtDescArticulo.Text = "";
                txtCantidadSucursal.Text = "";
                txtObservacionesSucursal.Text = "";
                hfIdArticuloDescripcion.Value = "";

                cerrarModal(modalConfirmacion);

                mostrarMensaje("Movimiento de Articulo entre sucursal realizado con exito");
            }
            bindGrillaMovimientos();
        }
        catch (ExcepcionPropia ex)
        {
            mostrarExcepcion(ex.Message);
            cerrarModal(modalConfirmacion);
        }
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
    protected void btnBuscarMovimientos_Click(object sender, EventArgs e)
    {
        bindGrillaMovimientos();
    }
    protected void btnExoprtarAExcel_Click(object sender, EventArgs e)
    {
        if (dgvMovimientos.Rows.Count == 0)
        {
            mostrarExcepcion("Debe Realizar una busqueda");
        }
        else
        {
        //    string datestyle = @"<style> .date{ mso-number-format:\#\.000; }</style>";
            //foreach (GridViewRow oItem in dgvArticulos.Rows)
            //    oItem.Cells[3].Attributes.Add("class", "date");

            Response.Clear();
            Response.AddHeader("content-disposition", "attachment; filename=Ventas.xls");
            Response.ContentType = "application/vnd.xls";
            System.IO.StringWriter WriteItem = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter htmlText = new HtmlTextWriter(WriteItem);
            //Response.Write(datestyle);
            dgvMovimientos.RenderControl(htmlText);
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