using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entidades;

public partial class Caja_AbrirCaja : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ucPanelMensajes.PanelErrorVisible = false;
        ucPanelMensajes.PanelMensajeVisible = false;
        if (!IsPostBack)
        {
            if (ApplicationSesion.ActiveCaja != null)
            {
                mostrarExcepcion("Actualmente posee una caja abierta no podra abrir una nueva");
                ocultarControles();
            }
        }
    }

    private void ocultarControles()
    { }
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
    protected void btnAceptar_Click(object sender, EventArgs e)
    {
        try
        {
            Caja c = new Caja();
            c.Descripcion = txtDescripcion.Text;
            c.Fecha = DateTime.Now;
            c.FondoInicial = Convert.ToDecimal(txtFondoInicial.Text);
            c.Idusuario = ApplicationSesion.ActiveUser.Idusuario;
            c.Abrir();
            ApplicationSesion.ActiveCaja = c;
            mostrarMensaje("Caja abierta con Exito");
        }
        catch (Exception ex)
        {
            mostrarExcepcion(ex.Message);
        }
    }
}