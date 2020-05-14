using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Negocio;
using Entidades;

public partial class Compra_Compras : System.Web.UI.Page
{
    private static string modalEliminar = "eliminarModal";

    protected void Page_Load(object sender, EventArgs e)
    {
        PanelError.Visible = false;
        PanelMensaje.Visible = false;
        if (!IsPostBack)
        {
            txtFechaDesde.Text = DateTime.Today.ToShortDateString();
            txtFechaHasta.Text = DateTime.Today.ToShortDateString();
            if (Request.QueryString["m"] != null)
            {
                PanelMensaje.Visible = true;
                LabelMensaje.Text = Request.QueryString["m"].ToString();
            }
            bindGrilla();
        }
    }

    private void bindGrilla()
    {
        try
        {
            lblTotalCompras.Text = "";
            List<Compra> listC;
            using (ControladorCompras c_compras = new ControladorCompras())
            {
                listC = c_compras.BuscarListCompras(Convert.ToDateTime(txtFechaDesde.Text), Convert.ToDateTime(txtFechaHasta.Text), ApplicationSesion.ActiveSucursal.IdSucursal).OrderBy(c => c.Fecha).ToList();
            }
            dgvCompras.DataSource = listC;
            dgvCompras.DataBind();
            // lblTotalCompras.Text = "Total: $" + listC.Sum(c => c.Total).ToString();

            if (listC != null)
            {

                List<FormaPago> listFP = new List<FormaPago>();
                foreach (Compra v in listC)
                {
                    if (v.ListFormaPago!=null)
                    {
                        listFP.AddRange(v.ListFormaPago);    
                    }
                    
                }

                string textoGastoAMostrar = "Totales [ ";
                lblTotalCompras.Text = "";
                var groupBy = listFP.GroupBy(lv => new { lv.Descripcion });
                foreach (var grupo in groupBy)
                {
                    string tipoGast = grupo.Key.Descripcion;
                    string total = listFP.Where(c => c.Descripcion == tipoGast).Sum(c => c.Monto).ToString();
                    textoGastoAMostrar = textoGastoAMostrar + tipoGast + ": $" + total + " | ";
                }
                textoGastoAMostrar = textoGastoAMostrar + "] Total: $" + listC.Sum(gg => gg.Total);
                lblTotalCompras.Text = textoGastoAMostrar;
                
            }

        }
        catch (ExcepcionPropia myex)
        {
            PanelError.Visible = true;
            lblError.Text = myex.Message;
            dgvCompras.DataSource = null;
            dgvCompras.DataBind();
        }
        catch (FormatException myex)
        {
            PanelError.Visible = true;
            lblError.Text = myex.Message;
            dgvCompras.DataSource = null;
            dgvCompras.DataBind();
        }

    }
    protected void btnNuevaCompra_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Compra/NuevaCompra.aspx");
    }
    protected void btnBuscarCompras_Click(object sender, EventArgs e)
    {
        bindGrilla();
    }
    protected void dgvCompras_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index = Convert.ToInt32(e.CommandArgument);
        if (e.CommandName.Equals("detCompra"))
        {
            string code = dgvCompras.DataKeys[index].Value.ToString();
            hfIdCompra.Value = code;
            try
            {
                Compra c;
                using (ControladorCompras c_compras = new ControladorCompras())
                {
                    c = c_compras.BuscarCompra(Convert.ToInt32(code));
                }
                List<Compra> listC = new List<Compra>();
                listC.Add(c);
                dvDetalleCompra.DataSource = listC;
                dvDetalleCompra.DataBind();
                dgvArticulos.DataSource = c.ListLineaCompra;
                dgvArticulos.DataBind();

                dgvFormaPago.DataSource = c.ListFormaPago;
                dgvFormaPago.DataBind();

                hfIdCompra.Value = c.Idcompra.ToString();
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append("$('#detModal').modal('show');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "DeleteModalScript", sb.ToString(), false);

            }
            catch (ExcepcionPropia myex)
            {
                PanelError.Visible = true;
                lblError.Text = myex.Message;
            }


        }
        else if (e.CommandName.Equals("editar"))
        {
            string code = dgvCompras.DataKeys[index].Value.ToString();
            Response.Redirect("~/Compra/NuevaCompra.aspx?id=" + code);
        }
        else if (e.CommandName.Equals("eliminar"))
        {
            hfIdCompraAeliminar.Value = dgvCompras.DataKeys[index].Value.ToString();
            abrirModal(modalEliminar);

        }
    }
    protected void dgvCompras_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {

            ((System.Web.UI.WebControls.LinkButton)(e.Row.Cells[5].Controls[0])).ToolTip = "Detalle / Editar Compra";
            ((System.Web.UI.WebControls.LinkButton)(e.Row.Cells[6].Controls[0])).ToolTip = "Editar Compra";

        }
        catch (Exception)
        {
        }
    }
    protected void dgvArticulos_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            ((System.Web.UI.WebControls.LinkButton)(e.Row.Cells[6].Controls[0])).ToolTip = "Anular Linea";
        }
        catch (Exception)
        {
        }

    }
    protected void dgvArticulos_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName.Equals("bajaLinea"))
            {
                if (((System.Web.UI.WebControls.CheckBox)dgvArticulos.Rows[index].Cells[4].Controls[0]).Checked)
                {
                    throw new ExcepcionPropia("La linea de compra ya se encuentra dada de baja");
                }
                lblModal.Text = "Desea Eliminar la linea de compra";
                hfIdarticulo.Value = dgvArticulos.DataKeys[index].Value.ToString();
                System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                sb1.Append(@"<script type='text/javascript'>");
                sb1.Append("$('#detModal').modal('hide');");
                sb1.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "DeleteModalScript", sb1.ToString(), false);
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append("$('#confirmacionModal').modal('show');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "confirmacionModalModalScript", sb.ToString(), false);
            }
        }
        catch (ExcepcionPropia myex)
        {
            PanelError.Visible = true;
            lblError.Text = myex.Message;
            dgvArticulos.DataSource = null;
            dgvArticulos.DataBind();
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#detModal').modal('hide');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "DeleteModalScript", sb.ToString(), false);
            System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
            sb1.Append(@"<script type='text/javascript'>");
            sb1.Append("var focalizar = $('#MainContent_btnNuevaCompra').position().top;");
            sb1.Append("$('html,body').animate({scrollTop: focalizar}, 500);");
            sb1.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", sb1.ToString(), false);
        }
        catch (FormatException myex)
        {
            PanelError.Visible = true;
            lblError.Text = myex.Message;
            dgvArticulos.DataSource = null;
            dgvArticulos.DataBind();
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#detModal').modal('hide');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "DeleteModalScript", sb.ToString(), false);
            System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
            sb1.Append(@"<script type='text/javascript'>");
            sb1.Append("var focalizar = $('#MainContent_btnNuevaCompra').position().top;");
            sb1.Append("$('html,body').animate({scrollTop: focalizar}, 500);");
            sb1.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", sb1.ToString(), false);
        }

    }
    protected void dgvArticulos_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            string sssss;
            sssss = e.Row.Cells[5].Controls[0].ToString();
            bool prueba = true;
            prueba = ((System.Web.UI.WebControls.CheckBox)(e.Row.Cells[5].Controls[0])).Checked;
            if (((System.Web.UI.WebControls.CheckBox)(e.Row.Cells[5].Controls[0])).Checked)
            {
                e.Row.CssClass = "danger";
            }
        }
        catch (Exception)
        {


        }
    }

    protected void btnAceptarModalConfirmacion_OnClick(object sender, EventArgs e)
    {
        try
        {
            int idCompra = Convert.ToInt32(hfIdCompra.Value);
            int idArt = Convert.ToInt32(hfIdarticulo.Value);
            int idUsuario = ApplicationSesion.ActiveUser.Idusuario;
            using (ControladorCompras c_compras = new ControladorCompras())
            {
                c_compras.BajaLineaCompra(idCompra, idArt, idUsuario);
            }
            PanelMensaje.Visible = true;
            LabelMensaje.Text = "Linea de compra anulada correctamente";
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#confirmacionModal').modal('hide');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "DeleteModalScript", sb.ToString(), false);
            System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
            sb1.Append(@"<script type='text/javascript'>");
            sb1.Append("var focalizar = $('#MainContent_btnNuevaCompra').position().top;");
            sb1.Append("$('html,body').animate({scrollTop: focalizar}, 500);");
            sb1.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", sb1.ToString(), false);

        }
        catch (ExcepcionPropia myex)
        {
            PanelError.Visible = true;
            lblError.Text = myex.Message;
            dgvArticulos.DataSource = null;
            dgvArticulos.DataBind();
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#confirmacionModal').modal('hide');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "DeleteModalScript", sb.ToString(), false);
            System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
            sb1.Append(@"<script type='text/javascript'>");
            sb1.Append("var focalizar = $('#MainContent_btnNuevaCompra').position().top;");
            sb1.Append("$('html,body').animate({scrollTop: focalizar}, 500);");
            sb1.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", sb1.ToString(), false);
        }
        catch (FormatException myex)
        {
            PanelError.Visible = true;
            lblError.Text = myex.Message;
            dgvArticulos.DataSource = null;
            dgvArticulos.DataBind();
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#confirmacionModal').modal('hide');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "DeleteModalScript", sb.ToString(), false);
            System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
            sb1.Append(@"<script type='text/javascript'>");
            sb1.Append("var focalizar = $('#MainContent_btnNuevaCompra').position().top;");
            sb1.Append("$('html,body').animate({scrollTop: focalizar}, 500);");
            sb1.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", sb1.ToString(), false);
        }

    }
    protected void btnExoprtarAExcel_Click(object sender, EventArgs e)
    {

        if (dgvCompras.Rows.Count == 0)
        {
            mostrarExcepcion("Debe Realizar una busqueda");
        }
        else
        {
            string datestyle = @"<style> .date{ mso-number-format:\#\.000; }</style>";
            //foreach (GridViewRow oItem in dgvArticulos.Rows)
            //    oItem.Cells[3].Attributes.Add("class", "date");

            Response.Clear();
            Response.AddHeader("content-disposition", "attachment; filename=Compras.xls");
            Response.ContentType = "application/vnd.xls";
            System.IO.StringWriter WriteItem = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter htmlText = new HtmlTextWriter(WriteItem);
            Response.Write(datestyle);
            dgvCompras.RenderControl(htmlText);
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
    protected void btnAceptarEliminar_Click(object sender, EventArgs e)
    {
        try
        {
            using (ControladorCompras c_compras = new ControladorCompras())
            {
                c_compras.EliminarCompra(Convert.ToInt32(hfIdCompraAeliminar.Value));
            }
            cerrarModal(modalEliminar);
            mostrarMensaje("Compra Eliminada Correctamente");
            bindGrilla();
        }
        catch (ExcepcionPropia ex)
        {
            mostrarExcepcion(ex.Message);
            cerrarModal(modalEliminar);
        }
    }
}