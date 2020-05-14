using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Negocio;
using Entidades;

public partial class Venta_VentaNueva : System.Web.UI.Page
{
    static string modalComprobante = "comprobanteModal";
    public Articulo ArticuloActual
    {
        get
        {
            return (Articulo)ViewState["articuloActual"];
        }
        set
        {
            ViewState["articuloActual"] = value;
        }
    }
    public Venta VentaActual
    {
        get
        {
            return (Venta)ViewState["ventaActual"];
        }
        set
        {
            ViewState["ventaActual"] = value;
        }
    }
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

    protected void Page_Load(object sender, EventArgs e)
    {
        validarCajaAbierta();
        PanelError.Visible = false;
        PanelMensaje.Visible = false;
        if (!IsPostBack)
        {
            panelNotaCredito.Visible = false;
            VentaActual = new Venta();
            cargarCbxFormaPago();
            txtTotal.Enabled = false;
            try
            {
                hfIdSucursal.Value = ApplicationSesion.ActiveSucursal.IdSucursal.ToString();
            }
            catch (FormatException ex)
            {
                mostrarExcepcion(ex.Message);

            }
        }
    }
    private void cargarCbxFormaPago()
    {
        try
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
            ArticuloActual = null;
            using (ControladorArticulos c_articulos = new ControladorArticulos())
            {
                ArticuloActual = c_articulos.BuscarArticulo(txtArticuloCodigo.Text, ApplicationSesion.ActiveSucursal.IdSucursal);
                txtArticuloModal.Text = ArticuloActual.DescripcionCompleta;
                txtCantidadModal.Text = "1";
                txtPrecioModal.Text = ArticuloActual.Precio.ToString();
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
            ArticuloActual = null;
            using (ControladorArticulos c_articulos = new ControladorArticulos())
            {
                ArticuloActual = c_articulos.BuscarArticulo(Convert.ToInt32(hfIdArticuloDescripcion.Value), ApplicationSesion.ActiveSucursal.IdSucursal);
                txtArticuloModal.Text = ArticuloActual.DescripcionCompleta;
                txtCantidadModal.Text = "1";
                txtPrecioModal.Text = ArticuloActual.Precio.ToString();

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

        notaCreditoActual = null;
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
                if (VentaActual.ListFormaPago == null)
                {
                    VentaActual.ListFormaPago = new List<FormaPago>();
                }

                VentaActual.ListFormaPago.Add(fp);
                dgvFormaPago.DataSource = VentaActual.ListFormaPago;
                dgvFormaPago.DataBind();
                txtTotalFP.Text = VentaActual.ListFormaPago.Sum(l => l.Monto).ToString();
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append("$('#formaPagoModal').modal('hide');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "FPShowModalScript", sb.ToString(), false);
            }



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
            if (VentaActual.ListLineaVenta != null && VentaActual.ListLineaVenta.Exists(l => l.Articulo.Idarticulo == ArticuloActual.Idarticulo))
            {
                throw new ExcepcionPropia("El articulo ya existe en la venta");
            }
            VentaLinea lv = new VentaLinea();
            lv.Articulo = ArticuloActual;
            lv.Cantidad = Convert.ToInt32(txtCantidadModal.Text);
            lv.PrecioUnitario = Convert.ToDecimal(txtPrecioModal.Text);
            if (VentaActual.ListLineaVenta == null)
            {
                VentaActual.ListLineaVenta = new List<VentaLinea>();
            }
            VentaActual.ListLineaVenta.Add(lv);
            txtTotal.Text = VentaActual.ListLineaVenta.Sum(l => l.PrecioUnitario * l.Cantidad).ToString("0.00");
            dgvArticulos.DataSource = VentaActual.ListLineaVenta;
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
            if (VentaActual.ListLineaVenta == null || VentaActual.ListLineaVenta.Count == 0)
            {
                throw new ExcepcionPropia("Debe agregar una linea de venta");
            }
            if (VentaActual.ListFormaPago == null || VentaActual.ListFormaPago.Count == 0)
            {
                throw new ExcepcionPropia("Debe agregar al menos uan forma de pago");
            }
            VentaActual.Descripcion = txtDescripcionVenta.Text;
            VentaActual.Idusuario = ApplicationSesion.ActiveUser.Idusuario;
            VentaActual.Fecha = DateTime.Today;
            VentaActual.Total = Convert.ToDecimal(txtTotal.Text);
            if (VentaActual.ListFormaPago.Exists(fp => fp.AceptaNotaCredito))
            {
                if (VentaActual.Total > VentaActual.ListFormaPago.Sum(fp => fp.Monto))
                {
                    throw new ExcepcionPropia("La suma de las formas de pago debe ser igual al total de la venta");
                }
            }
            else if (VentaActual.Total != VentaActual.ListFormaPago.Sum(fp => fp.Monto))
            {
                throw new ExcepcionPropia("La suma de las formas de pago debe ser igual al total de la venta");
            }
            VentaActual.Sucursal_ = ApplicationSesion.ActiveSucursal;
            VentaActual.IdCaja = ApplicationSesion.ActiveCaja.Idcaja;
            using (ControladorVentas c_ventas = new ControladorVentas())
            {
                c_ventas.AgregarVenta(VentaActual);
            }
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#confirmModal').modal('hide');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "confirmaciohowModalScript", sb.ToString(), false);
            VentaActual = null;
            ArticuloActual = null;
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
            VentaActual.ListLineaVenta.RemoveAt(index);
            dgvArticulos.DataSource = VentaActual.ListLineaVenta;
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
            VentaActual.ListFormaPago.RemoveAt(index);
            dgvFormaPago.DataSource = VentaActual.ListFormaPago;
            dgvFormaPago.DataBind();
            txtTotalFP.Text = VentaActual.ListFormaPago.Sum(fp => fp.Monto).ToString();
            PanelMensaje.Visible = true;
            LabelMensaje.Text = "Forma de pago eliminada correctamente";
        }
    }
    protected void cbxTipoFormaPago_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int idfp = Convert.ToInt32(cbxTipoFormaPago.SelectedValue);
            FormaPago fp;
            using (ControladorFormaPago c_fp = new ControladorFormaPago())
            {
                fp = c_fp.BuscarFormaPago(idfp);
            }
            if (fp.AceptaNotaCredito)
            {
                panelNotaCredito.Visible = true;
                txtMontoFP.Text = "0";
            }
            else
            {
                panelNotaCredito.Visible = false;
                txtMontoFP.Text = "0";

            }
        }
        catch (ExcepcionPropia myex)
        {
            mostrarExcepcion(myex.Message);
            cerrarModal("formaPagoModal");
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

    protected void btnAgregarComprobante_Click(object sender, EventArgs e)
    {
        try
        {
            if (VentaActual.ComprobanteVenta != null)
            {
                mostrarExcepcion("La Venta ya posee un comprobante");
            }
            else
            {
                txtFechaComprobanteModal.Text = DateTime.Today.ToShortDateString();
                abrirModal(modalComprobante);
            }

        }
        catch (ExcepcionPropia myex)
        {
            mostrarExcepcion(myex.Message);
        }
    }
    protected void btnAceptarComprobanteModal_Click(object sender, EventArgs e)
    {
        try
        {
            Comprobante cp = new Comprobante();
            cp.Fecha = Convert.ToDateTime(txtFechaComprobanteModal.Text);
            ///El monto lo calculo despues
            //cp.Monto = Convert.ToDecimal(txtMontoComprobanteModal.Text);
            cp.Numero = txtNumeroComprobanteModal.Text;
            cp.TipoComprobante = cbxTipoComprobanteModal.SelectedItem.Text;
            VentaActual.ComprobanteVenta = cp;
            List<Comprobante> listCp = new List<Comprobante>();
            listCp.Add(cp);
            dgvComprobante.DataSource = listCp;
            dgvComprobante.DataBind();
            cerrarModal(modalComprobante);

        }
        catch (ExcepcionPropia myex)
        {
            cerrarModal(modalComprobante);
            mostrarExcepcion(myex.Message);
        }
    }
    protected void dgvComprobante_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index = Convert.ToInt32(e.CommandArgument);
        if (e.CommandName.Equals("deleteComp"))
        {
            VentaActual.ComprobanteVenta = null;
            dgvComprobante.DataSource = null;
            dgvComprobante.DataBind();
        }
    }

    public void validarCajaAbierta()
    {
        if (ApplicationSesion.ActiveCaja == null)
        {
            Response.Redirect("~/MensajeCajaCerrada.aspx");
        }
    }
}