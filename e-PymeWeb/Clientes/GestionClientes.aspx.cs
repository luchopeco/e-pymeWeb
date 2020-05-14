using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Clientes_GestionClientes : System.Web.UI.Page
{
    static string modalCliente = "clienteModal";

    protected void Page_Load(object sender, EventArgs e)
    {
        lblTituloPagina.Text = "Gestion De Clientes";
    }
    protected void btnNuevoCliente_Click(object sender, EventArgs e)
    {
      
        lblTituloModalCliente.Text = "Agregando Cliente";
        lblPanelModalCliente.Text = "Cliente";        
        abrirModal(modalCliente);
        btnAceptarModalCliente.Text = "Agregar";        
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

    protected void btnAceptarModalCliente_Click(object sender, EventArgs e)
    {

    }
    protected void btnBuscarCliente_Click(object sender, EventArgs e)
    {

    }
}