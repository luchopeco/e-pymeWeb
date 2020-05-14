using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Negocio;
using Entidades;

public partial class Articulo_GestionArtciulosSucursal : System.Web.UI.Page
{
    private Articulo ArticuloActual
    {
        get { return (Articulo)ViewState["ArticuloActual"]; }
        set { ViewState["ArticuloActual"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        ucPanelMensajes.PanelErrorVisible = false;
        ucPanelMensajes.PanelMensajeVisible = false;
        if (!IsPostBack)
        {
            try
            {

                lblSucursal.Text = "Sucursal " + ApplicationSesion.ActiveSucursal.Descripcion;
                hfIdSucursal.Value = ApplicationSesion.ActiveSucursal.IdSucursal.ToString();
            }
            catch (ExcepcionPropia ex)
            {
                mostrarExcepcion(ex.Message);

            }
            catch (FormatException ex)
            {
                mostrarExcepcion(ex.Message);

            }

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

            }
            txtArticuloModal.Text = ArticuloActual.DescripcionCompleta;
            txtStockModal.Text = ArticuloActual.Stock.ToString(); ;
            txtPrecioModal.Text = ArticuloActual.Precio.ToString();
            txtCostoUltimo.Text = ArticuloActual.CostoUltimo.ToString();
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#articuloModal').modal('show');");
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
    protected void btnBuscarArtXDesc_Click(object sender, EventArgs e)
    {
        try
        {
            ArticuloActual = null;
            using (ControladorArticulos c_articulos = new ControladorArticulos())
            {
                ArticuloActual = c_articulos.BuscarArticulo(Convert.ToInt32(hfIdArticuloDescripcion.Value), ApplicationSesion.ActiveSucursal.IdSucursal);
            }
            txtArticuloModal.Text = ArticuloActual.DescripcionCompleta;
            txtStockModal.Text = ArticuloActual.Stock.ToString();
            txtPrecioModal.Text = ArticuloActual.Precio.ToString();
            txtCostoUltimo.Text = ArticuloActual.CostoUltimo.ToString();
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#articuloModal').modal('show');");
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

    protected void btnModificarArticulo_Click(object sender, EventArgs e)
    {
        try
        {
            decimal precio = Convert.ToDecimal(txtPrecioModal.Text);
            decimal costo = Convert.ToDecimal(txtCostoUltimo.Text);
            using (ControladorArticulos c_art = new ControladorArticulos())
            {
                c_art.ModificarArticuloSucursal(costo, precio, ArticuloActual.Idarticulo, ArticuloActual.IdSucursal);
            }
            ArticuloActual = null;
            txtArticuloCodigo.Text = string.Empty;
            txtDescArticulo.Text = string.Empty;

            txtArticuloModal.Text = string.Empty;
            txtCostoUltimo.Text = string.Empty;
            txtPrecioModal.Text = string.Empty;
            txtStockModal.Text = string.Empty;
            mostrarMensaje("Articulo Modificado con exito");
            cerrarModal("articuloModal");


        }
        catch (ExcepcionPropia ex)
        {
            mostrarExcepcion(ex.Message);

        }
        catch (FormatException ex)
        {
            mostrarExcepcion(ex.Message);

        }
    }
}