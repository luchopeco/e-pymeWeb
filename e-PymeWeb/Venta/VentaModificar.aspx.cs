using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Negocio;
using Entidades;

public partial class Venta_VentaModificar : System.Web.UI.Page
{
    private Venta ventaActual
    {
        get { return (Venta)ViewState["ventaActual"]; }
        set
        {
            ViewState["ventaActual"] = value;
            if (ventaActual != null)
            {
                dgvArticulos.DataSource = ventaActual.ListLineaVenta;
                dgvArticulos.DataBind();

                dgvFormaPagoActuales.DataSource = ventaActual.ListFormaPago;
                dgvFormaPagoActuales.DataBind();

                txtTotal.Text = ventaActual.Total.ToString();
                txtTotalFPActual.Text = ventaActual.ListFormaPago.Sum(fp => fp.Monto).ToString();
            }
            else
            {

            }
        }
    }
    private Articulo articuloActual
    {
        get { return (Articulo)ViewState["articuloActual"]; }
        set
        {
            ViewState["articuloActual"] = value;
            if (articuloActual != null)
            {
                txtArticuloModal.Text = articuloActual.DescripcionCompleta;
                txtCantidadModal.Text = "1";
                txtPrecioModal.Text = articuloActual.Precio.ToString();
            }
            else
            {

            }
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        PanelError.Visible = false;
        PanelMensaje.Visible = false;
        if (!IsPostBack)
        {
            if (Request.QueryString["id"] != null)
            {
                int idVenta = Convert.ToInt32(Request.QueryString["id"].ToString());
                using (ControladorVentas c_ventas = new ControladorVentas())
                {
                    ventaActual = c_ventas.BuscarVenta(idVenta);
                }
            }
        }
    }
    private void cargarCbxFormaPago()
    {
        try
        {
            using (ControladorFormaPago c_fp = new ControladorFormaPago())
            {
                cbxTipoFormaPago.DataValueField = "IdtipoFormaPago";
                cbxTipoFormaPago.DataTextField = "Descripcion";
                cbxTipoFormaPago.DataSource = c_fp.BuscarListFormaPago().Where(f => f.HabilitadoCompra == true).OrderBy(f => f.Descripcion).ToList();
                cbxTipoFormaPago.DataBind();
            }

        }
        catch (ExcepcionPropia myEx)
        {
            PanelError.Visible = true;
            lblError.Text = myEx.Message;
        }
    }
    protected void btnBuscarArtXCodigo_Click(object sender, EventArgs e)
    {
        try
        {
            articuloActual = null;
            using (ControladorArticulos c_articulos = new ControladorArticulos())
            {
                articuloActual = c_articulos.BuscarArticulo(txtArticuloCodigo.Text, ApplicationSesion.ActiveSucursal.IdSucursal);
            }
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#articuloModal').modal('show');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddShowModalScript", sb.ToString(), false);

        }
        catch (ExcepcionPropia myEx)
        {
            PanelError.Visible = true;
            lblError.Text = myEx.Message;
        }
        catch (FormatException myEx)
        {
            PanelError.Visible = true;
            lblError.Text = myEx.Message;
        }
    }
    protected void btnBuscarArtXDesc_Click(object sender, EventArgs e)
    {
        try
        {
            articuloActual = null;
            using (ControladorArticulos c_articulos = new ControladorArticulos())
            {
                articuloActual = c_articulos.BuscarArticulo(Convert.ToInt32(hfIdArticuloDescripcion.Value));
            }
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#articuloModal').modal('show');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddShowModalScript", sb.ToString(), false);

        }
        catch (ExcepcionPropia myEx)
        {
            PanelError.Visible = true;
            lblError.Text = myEx.Message;
        }
        catch (FormatException myEx)
        {
            PanelError.Visible = true;
            lblError.Text = myEx.Message;
        }
    }
    protected void btnNuevaFormaPago_Click(object sender, EventArgs e)
    {
        txtMontoFP.Text = txtTotal.Text;
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append(@"<script type='text/javascript'>");
        sb.Append("$('#formaPagoModal').modal('show');");
        sb.Append(@"</script>");
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "FPShowModalScript", sb.ToString(), false);
    }

    protected void btnAgregarFormaPAgo_Click(object sender, EventArgs e)
    {
        try
        {
            FormaPago fp = new FormaPago();
            fp.Descripcion = cbxTipoFormaPago.SelectedItem.Text;
            fp.IdtipoFormaPago = Convert.ToInt32(cbxTipoFormaPago.SelectedValue);
            fp.Monto = Convert.ToDecimal(txtMontoFP.Text);
            if (ventaActual.ListFormaPago == null)
            {
                ventaActual.ListFormaPago = new List<FormaPago>();
            }
            ventaActual.ListFormaPago.Add(fp);
            dgvFormaPago.DataSource = ventaActual.ListFormaPago;
            dgvFormaPago.DataBind();
            txtTotalFP.Text = ventaActual.ListFormaPago.Sum(l => l.Monto).ToString();
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#formaPagoModal').modal('hide');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "FPShowModalScript", sb.ToString(), false);

        }
        catch (ExcepcionPropia myEx)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#formaPagoModal').modal('hide');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "FPShowModalScript", sb.ToString(), false);
            PanelError.Visible = true;
            lblError.Text = myEx.Message;
        }
        catch (FormatException myEx)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#formaPagoModal').modal('hide');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "FPShowModalScript", sb.ToString(), false);
            PanelError.Visible = true;
            lblError.Text = myEx.Message;
        }
    }
    protected void btnAgregar_Click(object sender, EventArgs e)
    {
        try
        {
            if (ventaActual.ListLineaVenta != null && ventaActual.ListLineaVenta.Exists(l => l.Articulo.Idarticulo == articuloActual.Idarticulo))
            {
                throw new ExcepcionPropia("El articulo articuloActualya existe en la venta");
            }
            VentaLinea lv = new VentaLinea();
            lv.Articulo = articuloActual;
            lv.Cantidad = Convert.ToInt32(txtCantidadModal.Text);
            lv.PrecioUnitario = articuloActual.Precio;
            if (ventaActual.ListLineaVenta == null)
            {
                ventaActual.ListLineaVenta = new List<VentaLinea>();
            }
            ventaActual.ListLineaVenta.Add(lv);
            txtTotal.Text = ventaActual.ListLineaVenta.Sum(l => l.PrecioUnitario * l.Cantidad).ToString("0.00");
            dgvArticulos.DataSource = ventaActual.ListLineaVenta;
            dgvArticulos.DataBind();
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#articuloModal').modal('hide');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddShowModalScript", sb.ToString(), false);
        }
        catch (ExcepcionPropia mye)
        {
            PanelError.Visible = true;
            lblError.Text = mye.Message;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#articuloModal').modal('hide');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddShowModalScript", sb.ToString(), false);
        }
    }

    protected void btnGuardarVenta_Click(object sender, EventArgs e)
    {
        lblTotal.Text = txtTotal.Text;
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append(@"<script type='text/javascript'>");
        sb.Append("$('#confirmModal').modal('show');");
        sb.Append(@"</script>");
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "confirmaciohowModalScript", sb.ToString(), false);


        //try
        //{
        //    if (VentaActual.ListLineaVenta == null || VentaActual.ListLineaVenta.Count == 0)
        //    {
        //        throw new ExcepcionPropia("Agregue al menos una linea de venta");
        //    }
        //    if (VentaActual.ListFormaPago == null || VentaActual.ListFormaPago.Count == 0)
        //    {
        //        throw new ExcepcionPropia("Agregue al menos una Forma de pago");
        //    }
        //    if (Convert.ToDecimal(txtTotal.Text) != VentaActual.ListFormaPago.Sum(l => l.Monto))
        //    {
        //        throw new ExcepcionPropia("La suma de las formas de pago debe ser igual al total de la venta");
        //    }
        //    VentaActual.Descripcion = txtDescripcionVenta.Text;
        //    VentaActual.Fecha = DateTime.Today;
        //    VentaActual.Idusuario = Convert.ToInt32(Session["idusuario"]);
        //    VentaActual.Total = Convert.ToDecimal(txtTotal.Text);

        //    System.Text.StringBuilder sb = new System.Text.StringBuilder();
        //    sb.Append(@"<script type='text/javascript'>");
        //    sb.Append("$('#confirmacionModal').modal('show');");
        //    sb.Append(@"</script>");
        //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "confirmacionShowModalScript", sb.ToString(), false);
        //}
        //catch (ExcepcionPropia mye)
        //{
        //    PanelError.Visible = true;
        //    lblError.Text = mye.Message;
        //}


    }
    protected void btnAgregarVentaFinal_Click(object sender, EventArgs e)
    {
        try
        {
            if (ventaActual.ListLineaVenta == null || ventaActual.ListLineaVenta.Count == 0)
            {
                throw new ExcepcionPropia("Debe agregar una linea de venta");
            }
            if (ventaActual.ListFormaPago == null || ventaActual.ListFormaPago.Count == 0)
            {
                throw new ExcepcionPropia("Debe agregar al menos uan forma de pago");
            }
            ventaActual.Descripcion = txtDescripcionVenta.Text;
            ventaActual.Idusuario = ApplicationSesion.ActiveUser.Idusuario;
            ventaActual.Fecha = DateTime.Today;
            ventaActual.Total = Convert.ToDecimal(txtTotal.Text);
            if (ventaActual.Total != ventaActual.ListFormaPago.Sum(fp => fp.Monto))
            {
                throw new ExcepcionPropia("La suma de las formas de pago debe ser igual al total de la venta");
            }
            using (ControladorVentas c_ventas = new ControladorVentas())
            {
                c_ventas.AgregarVenta(ventaActual);
            }
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#confirmModal').modal('hide');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "confirmaciohowModalScript", sb.ToString(), false);
            ventaActual = null;
            articuloActual = null;
            Response.Redirect(ResolveUrl("~/Venta/Ventas.aspx?m='Venta Agregada Correctamente'"));
        }
        catch (ExcepcionPropia mye)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#confirmModal').modal('hide');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "confirmaciohowModalScript", sb.ToString(), false);
            PanelError.Visible = true;
            lblError.Text = mye.Message;
        }
    }

    protected void dgvArticulos_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index = Convert.ToInt32(e.CommandArgument);
        if (e.CommandName.Equals("editArticulo"))
        {

        }
        else if (e.CommandName.Equals("deleteArticulo"))
        {
            ventaActual.ListLineaVenta.RemoveAt(index);
            dgvArticulos.DataSource = ventaActual.ListLineaVenta;
            dgvArticulos.DataBind();
            PanelMensaje.Visible = true;
            LabelMensaje.Text = "Linea de venta eliminada Correctamente";
        }
    }
    protected void dgvFormaPago_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index = Convert.ToInt32(e.CommandArgument);
        if (e.CommandName.Equals("deleteFP"))
        {
            ventaActual.ListFormaPago.RemoveAt(index);
            dgvFormaPago.DataSource = ventaActual.ListFormaPago;
            dgvFormaPago.DataBind();
            txtTotalFP.Text = ventaActual.ListFormaPago.Sum(fp => fp.Monto).ToString();
            PanelMensaje.Visible = true;
            LabelMensaje.Text = "Forma de pago eliminada correctamente";
        }
    }
}