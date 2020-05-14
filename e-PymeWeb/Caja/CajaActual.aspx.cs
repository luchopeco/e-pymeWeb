using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entidades;

public partial class Caja_CajaActual : System.Web.UI.Page
{

    private Caja CajaACerrar
    {
        get { return (Caja)ViewState["CajaACerrar"]; }
        set { ViewState["CajaACerrar"] = value; }
    }
    private static string modaRetiroCaja = "retiroCajaModal";
    private static string modalIngresoCajaCaja = "ingresoCajaModal";
    private static string modalConfirmacion = "confirmacionModal";


    protected void Page_Load(object sender, EventArgs e)
    {
        ucPanelMensajes.PanelErrorVisible = false;
        ucPanelMensajes.PanelMensajeVisible = false;
        if (!IsPostBack)
        {
            if (Request.QueryString["id"] != null)
            {
                CajaACerrar = new Caja(Convert.ToInt32(Request.QueryString["id"]));
                if (CajaACerrar.FechaCierre != null)
                {
                    Response.Redirect("~/MensajeCajaCerrada.aspx");
                }
                lblFechaTitulo.Text = "Caja Del Dia De | " + DateTime.Today.ToShortDateString();
                txtFondoInicial.Text = CajaACerrar.FondoInicial.ToString();
                txtFondoFinal.Text = CajaACerrar.FondoFinal.ToString(); ;
            }
            else
            {
                lblFechaTitulo.Text = "Caja Del Dia De Hoy | " + DateTime.Today.ToShortDateString();
                CajaACerrar = null;
                validarCajaAbierta();
                txtFondoFinal.Text = ApplicationSesion.ActiveCaja.FondoFinal.ToString();
                

            }
            refrescarDatos();
        }

    }

    private void refrescarDatos()
    {
        Caja c;
        if (CajaACerrar != null)
        {
            c = CajaACerrar;
            
        }
        else
        {
            c = new Caja(ApplicationSesion.ActiveCaja.Idcaja);
        
        }

        txtFondoInicial.Text = c.FondoInicial.ToString();
        lblFechaTitulo.Text = c.Fecha.ToShortDateString();
        decimal totalMov=0;
        if (c.ListMovimientos!=null)
        {
            totalMov = c.ListMovimientos.Where(mm => mm.TipoMovimiento.EsSuma == true).Sum(mm => mm.Monto) - c.ListMovimientos.Where(mm => mm.TipoMovimiento.EsSuma ==false).Sum(mm => mm.Monto);
        }
        txtTotalFondos.Text = (c.FondoInicial + totalMov).ToString();
        decimal fondoFinal = Convert.ToDecimal(txtFondoFinal.Text);
        txtDiferencia.Text = (fondoFinal - totalMov - c.FondoInicial).ToString();
        if (c.ListMovimientos != null)
        {
            dgvMovimientosEfectivo.DataSource = c.ListMovimientos;
            dgvMovimientosEfectivo.DataBind();

            dgvMovimientosSinFondos.DataSource = c.ListMovimientosNoEfectivo;
            dgvMovimientosSinFondos.DataBind();
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

    public void validarCajaAbierta()
    {
        if (ApplicationSesion.ActiveCaja == null)
        {
            Response.Redirect("~/MensajeCajaCerrada.aspx");
        }
    }

    protected void btnRetiroCaja_Click(object sender, EventArgs e)
    {
        try
        {
            abrirModal(modaRetiroCaja);
        }
        catch (Exception ex)
        {
            mostrarExcepcion(ex.Message);

        }
    }
    protected void btnIngresoCaja_Click(object sender, EventArgs e)
    {
        try
        {
            abrirModal(modalIngresoCajaCaja);
        }
        catch (Exception ex)
        {
            mostrarExcepcion(ex.Message);

        }

    }
    protected void btnCerrarCaja_Click(object sender, EventArgs e)
    {
        try
        {
            abrirModal(modalConfirmacion);
        }
        catch (Exception ex)
        {
            mostrarExcepcion(ex.Message);

        }

    }
    protected void btnAceptarIngresoCajaModal_Click(object sender, EventArgs e)
    {
        try
        {
            MovimientoCaja m = new MovimientoCaja();
            m.Descripcion = txtDescripcionIngresoModal.Text;
            m.Fecha = DateTime.Now;
            if (CajaACerrar == null)
            {
                m.Idcaja = ApplicationSesion.ActiveCaja.Idcaja;
            }
            else
            {
                m.Idcaja = CajaACerrar.Idcaja;
            }
            m.IdtipoMovimiento = TipoMovimientoCaja.TipoMovimientoIngreso;
            m.Monto = Convert.ToDecimal(txtMontoIngresoModal.Text);
            m.Agregar();
            refrescarDatos();
            mostrarMensaje("Ingreso Agregado Con Exito");
            cerrarModal(modalIngresoCajaCaja);
        }
        catch (Exception ex)
        {
            mostrarExcepcion(ex.Message);
            cerrarModal(modalIngresoCajaCaja);

        }
    }
    protected void btnAceptarRetiroCajaModal_Click(object sender, EventArgs e)
    {
        try
        {
            MovimientoCaja m = new MovimientoCaja();
            m.Descripcion = txtDescripcionRetiroCajaModal.Text;
            m.Fecha = DateTime.Now;
            if (CajaACerrar == null)
            {
                m.Idcaja = ApplicationSesion.ActiveCaja.Idcaja;
            }
            else
            {
                m.Idcaja = CajaACerrar.Idcaja;
            }
            m.IdtipoMovimiento = TipoMovimientoCaja.TipoMovimientoRetiro;
            m.Monto = Convert.ToDecimal(txtMotoRetiroModal.Text);
            m.Agregar();
            refrescarDatos();
            mostrarMensaje("Retiro Agregado Con Exito");
            cerrarModal(modaRetiroCaja);

        }
        catch (Exception ex)
        {
            mostrarExcepcion(ex.Message);
            cerrarModal(modaRetiroCaja);

        }
    }
    protected void btnActualizarDiferencia_Click(object sender, EventArgs e)
    {
        try
        {
            refrescarDatos();
        }
        catch (Exception ex)
        {
            mostrarExcepcion(ex.Message);
            
        }
    }
    protected void btnAceptarModalFinal_Click(object sender, EventArgs e)
    {
        try
        {
            if (CajaACerrar != null)
            {
                CajaACerrar.FondoFinal = Convert.ToDecimal(txtFondoFinal.Text);
                CajaACerrar.FondoInicial = Convert.ToDecimal(txtFondoInicial.Text);
                CajaACerrar.Cerrar();
            }
            else
            {
                Caja c = ApplicationSesion.ActiveCaja;
                c.FondoFinal = Convert.ToDecimal(txtFondoFinal.Text);
                c.FondoInicial = Convert.ToDecimal(txtFondoInicial.Text);
                c.Cerrar();
                ApplicationSesion.ActiveCaja = null;
            }

            Response.Redirect("~/Default.aspx?m=Caja Cerrada Con Exito");
        }
        catch (Exception ex)
        {
            mostrarExcepcion(ex.Message);
            cerrarModal(modalConfirmacion);
        }
    }
}