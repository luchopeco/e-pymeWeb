using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Negocio;
using Entidades;

public partial class Venta_VentaCambio : System.Web.UI.Page
{
    private static string modalArticulo = "articuloCambioModal";
    private Venta ventaActual
    {
        get { return (Venta)ViewState["ventaActual"]; }
        set { ViewState["ventaActual"] = value; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        ucPanelMensajes.PanelErrorVisible = false;
        ucPanelMensajes.PanelMensajeVisible = false;
        if (!IsPostBack)
        {
            if (Request.QueryString["id"] != null)
            {
                int idVenta = Convert.ToInt32(Request.QueryString["id"]);
                using (ControladorVentas c_ventas = new ControladorVentas())
                {
                    ventaActual = c_ventas.BuscarVenta(idVenta);
                }
                List<Venta> listV = new List<Venta>();
                listV.Add(ventaActual);
                dgvVEnta.DataSource = listV;
                dgvVEnta.DataBind();

                dgvArticulosACambiar.DataSource = ventaActual.ListLineaVenta;
                dgvArticulosACambiar.DataBind();
            }

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
    protected void dgvArticulosACambiar_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName.Equals("cambiar"))
            {
                int id = Convert.ToInt32(dgvArticulosACambiar.DataKeys[index].Value);
                List<Articulo> listA;
                Articulo a;
                using (ControladorArticulos c_art = new ControladorArticulos())
                {
                    listA = c_art.BuscarListArticulosACambiar(id, ApplicationSesion.ActiveSucursal.IdSucursal);

                }
                if (listA != null)
                {
                    txtAgrupacionArticuloModal.Text = listA[0].AgrupacionArticulo.Descripcion;
                    cbxArticulosModal.DataTextField = "DescripcionCompleta";
                    cbxArticulosModal.DataValueField = "Idarticulo";
                    cbxArticulosModal.DataSource = listA;
                    cbxArticulosModal.DataBind();
                }
                else
                {
                    cbxArticulosModal.Items.Clear();
                    cbxArticulosModal.DataBind();
                }
                a = ventaActual.ListLineaVenta.FirstOrDefault(v => v.Idarticulo == id).Articulo;
                txtCantidad.Text = ventaActual.ListLineaVenta.FirstOrDefault(v => v.Idarticulo == id).Cantidad.ToString();
                txtArticuloACambiar.Text = a.DescripcionCompleta;
                hfIdArticuloACambiar.Value = id.ToString();
                abrirModal(modalArticulo);
            }
        }
        catch (ExcepcionPropia ex)
        {

            mostrarExcepcion(ex.Message);            
        }
    }
    protected void btnAceptarModalCambio_Click(object sender, EventArgs e)
    {
        try
        {
            Articulo artAModificar = new Articulo();
            artAModificar.Idarticulo = Convert.ToInt32(hfIdArticuloACambiar.Value);

            Articulo artNuevo = new Articulo();
            artNuevo.Idarticulo = Convert.ToInt32(cbxArticulosModal.SelectedValue);

            VentaLineaCambio vdc = new VentaLineaCambio();
            vdc.IdVenta = ventaActual.Idventa;
            vdc.Articulo = artNuevo;
            vdc.ArticuloAnterior = artAModificar;
            vdc.Cantidad = Convert.ToInt32(txtCantidad.Text);
            vdc.FechaCambio = DateTime.Today;
            vdc.IdUsuario = ApplicationSesion.ActiveUser.Idusuario;


            VentaLinea vl = new VentaLinea();
            vl.Articulo = artNuevo;
            vl.Cantidad = Convert.ToInt32(txtCantidad.Text);
            vl.Idventa = ventaActual.Idventa;
            vl.PrecioUnitario = ventaActual.ListLineaVenta.FirstOrDefault(lv => lv.Idarticulo == artAModificar.Idarticulo).PrecioUnitario;

            using (ControladorVentas c_vemtas = new ControladorVentas())
            {
                c_vemtas.RealizarCambio(vl, vdc);
            }
            Response.Redirect(ResolveUrl("~/Venta/Ventas.aspx?m='Cambio Realizado Correctamete'"));

        }
        catch (ExcepcionPropia ex)
        {
            cerrarModal(modalArticulo);
            mostrarExcepcion(ex.Message);
        }
        catch (FormatException ex)
        {
            cerrarModal(modalArticulo);
            mostrarExcepcion(ex.Message);
        }
    }
}