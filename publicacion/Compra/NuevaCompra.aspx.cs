using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Negocio;
using Entidades;

public partial class Compra_NuevaCompra : System.Web.UI.Page
{
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
    public Compra CompraActual
    {
        get
        {
            return (Compra)ViewState["compraActual"];
        }
        set
        {
            ViewState["compraActual"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        validarCajaAbierta();
        PanelError.Visible = false;
        PanelMensaje.Visible = false;

        if (!IsPostBack)
        {
            try
            {

                if (Request.QueryString["id"] != null)
                {
                    int id = Convert.ToInt32(Request.QueryString["id"]);
                    using (ControladorCompras c_compras = new ControladorCompras())
                    {
                        CompraActual = c_compras.BuscarCompra(id);
                    }
                    txtProveedor.Text = CompraActual.DescProveedor;
                    txtTotal.Text = CompraActual.Total.ToString();
                    txtTotalFP.Text = CompraActual.Total.ToString();
                    hfIdProveedor.Value = CompraActual.Idproveedor.ToString();
                    cbxTipoCompra.SelectedValue = CompraActual.IdtipoCompra.ToString();
                    txtDescripcion.Text = CompraActual.Descripcion;
                    dgvFormaPago.DataSource = CompraActual.ListFormaPago;
                    dgvFormaPago.DataBind();
                    dgvArticulos.DataSource = CompraActual.ListLineaCompra;
                    dgvArticulos.DataBind();

                    if (CompraActual.Comprobante != null)
                    {
                        List<ComprobanteCompra> lisCc = new List<ComprobanteCompra>();
                        lisCc.Add(CompraActual.Comprobante);
                        dgvComprobante.DataSource = lisCc;
                        dgvComprobante.DataBind();
                    }

                    btnAgregarCompra.Text = "Modificar Compra";
                    lblTituloPagina.Text = "Modificando Compra";
                    lblModalConfirmacion.Text = "¿Desea Modificar la compra?";
                }
                else
                {
                    CompraActual = new Compra();
                    CompraActual.Fecha = DateTime.Today;
                    CompraActual.Idusuario = ApplicationSesion.ActiveUser.Idusuario;
                    CompraActual.Sucursal_ = ApplicationSesion.ActiveSucursal;
                    CompraActual.ListLineaCompra = new List<CompraLinea>();
                    btnAgregarCompra.Text = "Agregar Compra";
                    lblTituloPagina.Text = "Nueva Compra";
                    lblModalConfirmacion.Text = "¿Desea Agregar la compra?";
                }

                cargarCbxTipoCompra();
                cargarCbxMarcas();
                cargarCbxTipoArt();
                cargarCbxFormaPago();
                chbxControlarStock.Checked = true;
            }

            catch (ExcepcionPropia ex)
            {
                PanelError.Visible = true;
                lblError.Text = ex.Message;
            }
            catch (FormatException ex)
            {
                PanelError.Visible = true;
                lblError.Text = ex.Message;
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

    private void cargarCbxTipoArt()
    {
        try
        {
            cbxTipoArt.DataValueField = "IdtipoArticulo";
            cbxTipoArt.DataTextField = "Descripcion";
            using (ControladorArticulos c_articulos = new ControladorArticulos())
            {
                cbxTipoArt.DataSource = c_articulos.BuscarListTipoArticulo().Where(m => m.FechaBaja == null).OrderBy(c => c.Descripcion);
                cbxTipoArt.DataBind();
            }
        }
        catch (ExcepcionPropia myEx)
        {
            PanelError.Visible = true;
            lblError.Text = myEx.Message;
        }
    }

    private void cargarCbxMarcas()
    {
        try
        {
            cbxMarca.DataValueField = "Idmarca";
            cbxMarca.DataTextField = "Descripcion";
            using (ControladorMarcas c_marcas = new ControladorMarcas())
            {
                cbxMarca.DataSource = c_marcas.BuscarListMarca().Where(m => m.FechaBaja == null).OrderBy(c => c.Descripcion);
                cbxMarca.DataBind();
            }
        }
        catch (ExcepcionPropia myEx)
        {
            PanelError.Visible = true;
            lblError.Text = myEx.Message;
        }
    }

    private void cargarCbxTipoCompra()
    {
        try
        {
            cbxTipoCompra.DataValueField = "IdtipoCompra";
            cbxTipoCompra.DataTextField = "Descripcion";
            using (ControladorCompras c_compras = new ControladorCompras())
            {
                cbxTipoCompra.DataSource = c_compras.BuscarListTipoCompras().Where(tc => tc.FechaBaja == null).OrderBy(tc => tc.Descripcion).ToList();
                cbxTipoCompra.DataBind();
            }
        }
        catch (ExcepcionPropia)
        {
            PanelError.Visible = true;
            lblError.Text = "No se han encontrado tipos de compra. Debe existir un tipo de compra para realizar una compra";
        }

    }
    protected void btnNuevoArt_Click(object sender, EventArgs e)
    {
        ArticuloActual = null;
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append(@"<script type='text/javascript'>");
        sb.Append("$('#addModal').modal('show');");
        sb.Append(@"</script>");
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddShowModalScript", sb.ToString(), false);
    }
    protected void btnAgregar_Click(object sender, EventArgs e)
    {
        try
        {
            if (ArticuloActual == null)
            {
                ArticuloActual = new Articulo();
                ArticuloActual.Codigo = txtCodigo.Text;
                ArticuloActual.ControlarStock = chbxControlarStock.Checked;
                ArticuloActual.Descripcion = txtDescripcionArtAgregar.Text;
                ArticuloActual.FechaAlta = DateTime.Today;
                Marca m = new Marca();
                m.Idmarca = Convert.ToInt32(cbxMarca.SelectedValue);
                m.Descripcion = cbxMarca.SelectedItem.ToString();
                ArticuloActual.Marca = m;
                TipoArticulo ta = new TipoArticulo();
                ta.IdtipoArticulo = Convert.ToInt32(cbxTipoArt.SelectedValue);
                ta.Descripcion = cbxTipoArt.SelectedItem.ToString();
                ArticuloActual.TipoArticulo = ta;
                if (ucCbxArticulosAgrupacion.SelectedValue != 0)
                {
                    ArticuloAgrupacion aa = new ArticuloAgrupacion();
                    aa.IdagrupacionArticulo = ucCbxArticulosAgrupacion.SelectedValue;
                    ArticuloActual.AgrupacionArticulo = aa;
                }

                ///controlo q no exista el articulo

                using (ControladorArticulos c_articulos = new ControladorArticulos())
                {
                    c_articulos.ValidarArticulo(ArticuloActual.Descripcion, ArticuloActual.Idmarca, ArticuloActual.Idtipoarticulo);
                }
            }

            ArticuloActual.CostoUltimo = Convert.ToDecimal(txtCosto.Text);
            ArticuloActual.Precio = Convert.ToDecimal(txtPrecioVenta.Text);
            CompraLinea lc = new CompraLinea();
            lc.Articulo = ArticuloActual;
            lc.Cantidad = Convert.ToInt32(txtCantidad.Text);
            lc.CostoUnitario = ArticuloActual.CostoUltimo;
            //if (CompraActual == null)
            //{
            //    CompraActual = new Compra();
            //}
            if (CompraActual.ListLineaCompra.Exists(lcc => lcc.Articulo.DescripcionCompleta == ArticuloActual.DescripcionCompleta))
            {
                throw new ExcepcionPropia("El articulo ya existe en la linea de compra");
            }
            CompraActual.ListLineaCompra.Add(lc);

            dgvArticulos.DataSource = CompraActual.ListLineaCompra;
            dgvArticulos.DataBind();

            txtTotal.Text = CompraActual.ListLineaCompra.Sum(lcc => lcc.Subtotal).ToString();

            ArticuloActual = null;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#addModal').modal('hide');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddHideModalScript", sb.ToString(), false);
        }
        catch (ExcepcionPropia myEx)
        {
            PanelError.Visible = true;
            lblError.Text = myEx.Message;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#addModal').modal('hide');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddHideModalScript", sb.ToString(), false);
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
                txtCodigoArtBusqueda.Text = ArticuloActual.Codigo;
                txtDescripcionArtBusqueda.Text = ArticuloActual.Descripcion;
                txtMarcaArtBusqueda.Text = ArticuloActual.Marca.Descripcion;
                //txtStock.Text = ArticuloActual.Stock.ToString();
                txtTipoArtBusqueda.Text = ArticuloActual.TipoArticulo.Descripcion;
                txtCostoBusqueda.Text = ArticuloActual.CostoUltimo.ToString();
                chbControlarStockBusqueda.Checked = ArticuloActual.ControlarStock;
                if (!ArticuloActual.ControlarStock)
                {
                    txtCantidadBusqueda.Enabled = false;
                    txtCantidadBusqueda.Text = "0";
                }
                else
                {
                    txtCantidadBusqueda.Enabled = true;
                    txtCantidadBusqueda.Text = "0";
                }
            }
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#searchModal').modal('show');");
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
                ArticuloActual = c_articulos.BuscarArticulo(Convert.ToInt32(hfIdArticuloDescripcion.Value));
                txtCodigoArtBusqueda.Text = ArticuloActual.Codigo;
                txtDescripcionArtBusqueda.Text = ArticuloActual.Descripcion;
                txtMarcaArtBusqueda.Text = ArticuloActual.Marca.Descripcion;
                //txtStock.Text = ArticuloActual.Stock.ToString();
                txtTipoArtBusqueda.Text = ArticuloActual.TipoArticulo.Descripcion;
                txtCostoBusqueda.Text = ArticuloActual.CostoUltimo.ToString();
                chbControlarStockBusqueda.Checked = ArticuloActual.ControlarStock;
                if (!ArticuloActual.ControlarStock)
                {
                    txtCantidadBusqueda.Enabled = false;
                    txtCantidadBusqueda.Text = "0";
                }
                else
                {
                    txtCantidadBusqueda.Enabled = true;
                    txtCantidadBusqueda.Text = "0";
                }
            }
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#searchModal').modal('show');");
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
    protected void btnAgregarCompra_Click(object sender, EventArgs e)
    {
        try
        {
            using (ControladorCompras c_compras = new ControladorCompras())
            {
                CompraActual.TipoCompra = c_compras.BuscarTipoCompra(Convert.ToInt32(cbxTipoCompra.SelectedValue));
                if (CompraActual.ListLineaCompra == null || CompraActual.ListLineaCompra.Count == 0)
                {
                    throw new ExcepcionPropia("Debe agregar al menos una linea de compra");
                }
                if (txtTotal.Text != txtTotalFP.Text && CompraActual.TipoCompra.GeneraCargo)
                {
                    throw new ExcepcionPropia("Debe coincidir el total de la compra y la forma de pago");
                }
                if ((!CompraActual.TipoCompra.GeneraCargo) && txtTotal.Text != "0" && txtTotal.Text != "0,00")
                {
                    throw new ExcepcionPropia("El tipo de compra seleccionado no genera un cargo. El total debe ser 0");
                }
                CompraActual.Proveedor = c_compras.BuscarProveedor(Convert.ToInt32(hfIdProveedor.Value));
                if (CompraActual.Comprobante != null)
                {
                    CompraActual.Comprobante.Proveedor = CompraActual.Proveedor;
                }
                lblTipoCompra.Text = CompraActual.TipoCompra.Descripcion;
                lblProveedor.Text = CompraActual.Proveedor.Nombre;
                if (CompraActual.TipoCompra.GeneraCargo)
                {
                    lblTotal.Text = txtTotal.Text;
                    CompraActual.Total = Convert.ToDecimal(txtTotal.Text);
                }
                else
                {
                    lblTotal.Text = "0";
                    CompraActual.Total = 0;
                }
                CompraActual.Descripcion = txtDescripcion.Text;
            }
            if (lblTituloPagina.Text == "Modificando Compra")
            {

                btnAgregarCompraFinal.Text = "Modificar";

            }
            else
            {

                btnAgregarCompraFinal.Text = "Agregar";

            }
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#confirmacionModal').modal('show');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddShowModalScript", sb.ToString(), false);
        }
        catch (ExcepcionPropia myEx)
        {
            mostrarExcepcion(myEx.Message);
        }
        catch (FormatException myEx)
        {
            mostrarExcepcion(myEx.Message);
        }

    }
    protected void btnAgregarCompraFinal_Click(object sender, EventArgs e)
    {
        try
        {
            if (lblTituloPagina.Text == "Modificando Compra")
            {
                using (ControladorCompras c_compras = new ControladorCompras())
                {
                    c_compras.ModificarCompra(CompraActual);
                }
                CompraActual = null;
                ArticuloActual = null;
                Response.Redirect(ResolveUrl("~/Compra/Compras.aspx?m='Compra modificada Correctamente'"));
            }
            else
            {
                CompraActual.IdCaja = ApplicationSesion.ActiveCaja.Idcaja;
                using (ControladorCompras c_compras = new ControladorCompras())
                {
                    c_compras.AgregarCompra(CompraActual);
                }

                CompraActual = null;
                ArticuloActual = null;
                Response.Redirect(ResolveUrl("~/Compra/Compras.aspx?m='Compra Agregada Correctamente'"));
            }

        }
        catch (ExcepcionPropia myEx)
        {
            PanelError.Visible = true;
            lblError.Text = myEx.Message;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#confirmacionModal').modal('hide');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddShowModalScript", sb.ToString(), false);
        }



    }
    protected void btnNuevaFormaPago_Click(object sender, EventArgs e)
    {
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
            if (CompraActual.ListFormaPago == null)
            {
                CompraActual.ListFormaPago = new List<FormaPago>();
            }

            CompraActual.ListFormaPago.Add(fp);
            dgvFormaPago.DataSource = CompraActual.ListFormaPago;
            dgvFormaPago.DataBind();
            txtTotalFP.Text = CompraActual.ListFormaPago.Sum(l => l.Monto).ToString();
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
    protected void chbxControlarStock_CheckedChanged(object sender, EventArgs e)
    {
        if (chbxControlarStock.Checked)
        {
            txtCantidad.Enabled = true;
            txtCantidad.Text = "0";
        }
        else
        {
            txtCantidad.Enabled = false;
            txtCantidad.Text = "0";
        }
    }
    protected void btnAgregarArticuloExistente_Click(object sender, EventArgs e)
    {
        try
        {
            if (CompraActual.ListLineaCompra.Exists(lcc => lcc.Articulo.DescripcionCompleta == ArticuloActual.DescripcionCompleta))
            {
                throw new ExcepcionPropia("El articulo ya existe en la linea de compra");
            }
            ArticuloActual.CostoUltimo = Convert.ToDecimal(txtCostoBusqueda.Text);
            ArticuloActual.Precio = Convert.ToDecimal(txtPrecioBusqueda.Text);
            CompraLinea lc = new CompraLinea();
            lc.Articulo = ArticuloActual;
            lc.Cantidad = Convert.ToInt32(txtCantidadBusqueda.Text);
            lc.CostoUnitario = ArticuloActual.CostoUltimo;
            if (CompraActual == null)
            {
                CompraActual = new Compra();
            }

            CompraActual.ListLineaCompra.Add(lc);

            dgvArticulos.DataSource = CompraActual.ListLineaCompra;
            dgvArticulos.DataBind();

            txtTotal.Text = CompraActual.ListLineaCompra.Sum(lcc => lcc.Subtotal).ToString();

            ArticuloActual = null;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#searchModal').modal('hide');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddHideModalScript", sb.ToString(), false);
        }
        catch (ExcepcionPropia myEx)
        {
            mostrarExcepcion(myEx.Message);
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#searchModal').modal('hide');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddHideModalScript", sb.ToString(), false);
        }
    }
    protected void dgvArticulos_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index = Convert.ToInt32(e.CommandArgument);
        if (e.CommandName.Equals("editArticulo"))
        {
            //ArticuloActual = null;
            //ArticuloActual = CompraActual.ListLineaCompra[index].Articulo;
            //txtCodigoArtBusqueda.Text = ArticuloActual.Codigo;
            //txtDescripcionArtBusqueda.Text = ArticuloActual.Descripcion;
            //txtMarcaArtBusqueda.Text = ArticuloActual.Marca.Descripcion;
            //txtStock.Text = ArticuloActual.Stock.ToString();
            //txtTipoArtBusqueda.Text = ArticuloActual.TipoArticulo.Descripcion;
            //txtCostoBusqueda.Text = ArticuloActual.CostoUltimo.ToString();
            //txtCantidadBusqueda.Text = CompraActual.ListLineaCompra[index].Cantidad.ToString();
            //txtCostoBusqueda.Text = CompraActual.ListLineaCompra[index].CostoUnitario.ToString();
            //txtPrecioBusqueda.Text = CompraActual.ListLineaCompra[index].PrecioVenta.ToString();
            //chbControlarStockBusqueda.Checked = ArticuloActual.ControlarStock;
            //if (!ArticuloActual.ControlarStock)
            //{
            //    txtCantidadBusqueda.Enabled = false;
            //    txtCantidadBusqueda.Text = "0";
            //}
            //else
            //{
            //    txtCantidadBusqueda.Enabled = true;
            //    txtCantidadBusqueda.Text = "0";
            //}

            //System.Text.StringBuilder sb = new System.Text.StringBuilder();
            //sb.Append(@"<script type='text/javascript'>");
            //sb.Append("$('#searchModal').modal('show');");
            //sb.Append(@"</script>");
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddShowModalScript", sb.ToString(), false);
        }
        else if (e.CommandName.Equals("deleteArticulo"))
        {
            CompraActual.ListLineaCompra.RemoveAt(index);
            dgvArticulos.DataSource = CompraActual.ListLineaCompra;
            dgvArticulos.DataBind();
            PanelMensaje.Visible = true;
            LabelMensaje.Text = "Linea de compra eliminada Correctamente";
        }
    }
    protected void dgvFormaPago_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index = Convert.ToInt32(e.CommandArgument);
        if (e.CommandName.Equals("deleteFP"))
        {
            CompraActual.ListFormaPago.RemoveAt(index);
            dgvFormaPago.DataSource = CompraActual.ListFormaPago;
            dgvFormaPago.DataBind();
            txtTotalFP.Text = CompraActual.ListFormaPago.Sum(fp => fp.Monto).ToString();
            PanelMensaje.Visible = true;
            LabelMensaje.Text = "Forma de pago eliminada correctamente";
        }
    }
    protected void btnAgregarComprobante_Click(object sender, EventArgs e)
    {
        try
        {
            if (CompraActual.Comprobante != null)
            {
                mostrarExcepcion("La compra ya posee un comprobante");
            }
            else
            {
                txtFechaComprobanteModal.Text = DateTime.Today.ToShortDateString();
                abrirModal("comprobanteModal");
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
            ComprobanteCompra cp = new ComprobanteCompra();
            cp.Fecha = Convert.ToDateTime(txtFechaComprobanteModal.Text);
            cp.Monto = Convert.ToDecimal(txtMontoComprobanteModal.Text);
            cp.Numero = txtNumeroComprobanteModal.Text;
            cp.TipoComprobante = cbxTipoComprobanteModal.SelectedItem.Text;
            CompraActual.Comprobante = cp;
            List<ComprobanteCompra> listCp = new List<ComprobanteCompra>();
            listCp.Add(cp);
            dgvComprobante.DataSource = listCp;
            dgvComprobante.DataBind();
            cerrarModal("comprobanteModal");

        }
        catch (ExcepcionPropia myex)
        {
            cerrarModal("comprobanteModal");
            mostrarExcepcion(myex.Message);
        }
    }

    protected void dgvComprobante_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index = Convert.ToInt32(e.CommandArgument);
        if (e.CommandName.Equals("deleteComp"))
        {
            CompraActual.Comprobante = null;
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
}