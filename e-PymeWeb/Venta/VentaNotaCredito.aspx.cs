using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Negocio;
using Entidades;

public partial class Venta_VentaNotaCredito : System.Web.UI.Page
{
    private Venta ventaActual
    {
        get { return (Venta)ViewState["ventaActual"]; }
        set { ViewState["ventaActual"] = value; }
    }
    private NotaCredito notaCreditoActual
    {
        get { return (NotaCredito)ViewState["notaCreditoActual"]; }
        set { ViewState["notaCreditoActual"] = value; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        ucPanelMensajes.PanelErrorVisible = false;
        ucPanelMensajes.PanelMensajeVisible = false;
        if (!IsPostBack)
        {
            try
            {
                if (Request.QueryString["id"] != null)
                {
                    int id = Convert.ToInt32(Request.QueryString["id"]);
                    using (ControladorVentas c_venta = new ControladorVentas())
                    {
                        ventaActual = c_venta.BuscarVenta(id);
                    }
                    txtFechaVto.Text = DateTime.Today.AddMonths(3).ToShortDateString();

                    List<Venta> listV = new List<Venta>();
                    listV.Add(ventaActual);
                    dvVentas.DataSource = listV;
                    dvVentas.DataBind();

                    dgvFormaPago.DataSource = ventaActual.ListFormaPago;
                    dgvFormaPago.DataBind();

                    dgvLinaVenta.DataSource = ventaActual.ListLineaVenta;
                    dgvLinaVenta.DataBind();
                }
                else
                {
                    Response.Redirect("");
                }
            }
            catch (ExcepcionPropia myex)
            {
                mostrarExcepcion(myex.Message);
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

    protected void dgvLinaVenta_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index = Convert.ToInt32(e.CommandArgument);
        if (e.CommandName.Equals("seleccionar"))
        {
            try
            {
                int idArt = Convert.ToInt32(dgvLinaVenta.DataKeys[index].Value);
                if (ventaActual.ListLineaVenta[index].Devuelto)
                {
                    mostrarExcepcion("El Articulo ya se encuentra en una nota de credito");
                }
                else
                {
                    if (notaCreditoActual == null)
                    {
                        notaCreditoActual = new NotaCredito();
                        notaCreditoActual.ListLineasVentaDevueltas = new List<VentaLinea>();
                    }
                    notaCreditoActual.ListLineasVentaDevueltas.Add(ventaActual.ListLineaVenta[index]);
                    ventaActual.ListLineaVenta.RemoveAt(index);

                    dgvLinaVenta.DataSource = ventaActual.ListLineaVenta;
                    dgvLinaVenta.DataBind();

                    dgvNotaCredito.DataSource = notaCreditoActual.ListLineasVentaDevueltas;
                    dgvNotaCredito.DataBind();
                    establecerTotalNotaCredito();
                }


            }
            catch (ExcepcionPropia myex)
            {
                mostrarExcepcion(myex.Message);

            }


        }
    }

    protected void dgvNotaCredito_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index = Convert.ToInt32(e.CommandArgument);
        if (e.CommandName.Equals("quitar"))
        {
            try
            {
                ventaActual.ListLineaVenta.Add(notaCreditoActual.ListLineasVentaDevueltas[index]);
                notaCreditoActual.ListLineasVentaDevueltas.RemoveAt(index);

                dgvLinaVenta.DataSource = ventaActual.ListLineaVenta;
                dgvLinaVenta.DataBind();

                dgvNotaCredito.DataSource = notaCreditoActual.ListLineasVentaDevueltas;
                dgvNotaCredito.DataBind();
                establecerTotalNotaCredito();
            }
            catch (ExcepcionPropia myex)
            {
                mostrarExcepcion(myex.Message);
            }
        }
    }

    private void establecerTotalNotaCredito()
    {
        if (notaCreditoActual.ListLineasVentaDevueltas == null || notaCreditoActual.ListLineasVentaDevueltas.Count == 0)
        {
            txtTotal.Text = "0";
        }
        else
        {
            txtTotal.Text = notaCreditoActual.ListLineasVentaDevueltas.Sum(l => l.Subtotal).ToString();
        }
    }
    protected void btnGenerarNotaCredito_Click(object sender, EventArgs e)
    {
        try
        {
            if (DateTime.Now >= Convert.ToDateTime(txtFechaVto.Text))
            {
                mostrarExcepcion("La fecha de vencimiento debe ser mayor a la fecha actual");
            }
            else
            {
                int idUsuario = ApplicationSesion.ActiveUser.Idusuario;
                using (ControladorVentas c_ventas = new ControladorVentas())
                {
                  notaCreditoActual.IdnotaCredito =  c_ventas.AgregarNotaCredito(idUsuario, Convert.ToDateTime(txtFechaVto.Text), txtDescripcionNotaCredito.Text, Convert.ToDecimal(txtTotal.Text), notaCreditoActual.ListLineasVentaDevueltas);
                }
                Response.Redirect("~/Venta/Ventas.aspx?m=Nota de credito creada correctamente");
            }

        }
        catch (ExcepcionPropia myex)
        {
            mostrarExcepcion(myex.Message);
        }
    }
}