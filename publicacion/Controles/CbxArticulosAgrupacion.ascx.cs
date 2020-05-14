using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Negocio;
using Entidades;

public partial class Controles_CbxArticulosAgrupacion : System.Web.UI.UserControl
{
    public int SelectedValue
    {
        get { return Convert.ToInt32(cbxArticulosAgrupacion.SelectedValue); }
        set { cbxArticulosAgrupacion.SelectedValue = value.ToString(); }
    }
    public string SelectedText
    {
        get { return cbxArticulosAgrupacion.SelectedItem.Text; }
        set { cbxArticulosAgrupacion.SelectedValue = value; }
    }
    public bool CbxArticuloAgrupacionEnable
    {
        get { return cbxArticulosAgrupacion.Enabled; }
        set { cbxArticulosAgrupacion.Enabled = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                List<ArticuloAgrupacion> listArtAg;
                using (ControladorArticulos c_art = new ControladorArticulos())
                {
                    listArtAg = c_art.BuscarListArticuloAgrupacion();
                }
                ArticuloAgrupacion aa = new ArticuloAgrupacion();
                aa.IdagrupacionArticulo = 0;
                aa.TipoArticulo = new TipoArticulo();
                aa.Marca = new Marca();
                aa.Descripcion = "Sin Datos";
                cbxArticulosAgrupacion.DataTextField = "DescripcionCompleta";
                cbxArticulosAgrupacion.DataValueField = "IdagrupacionArticulo";
                if (listArtAg==null)
                {
                    listArtAg = new List<ArticuloAgrupacion>();
                }
                listArtAg.Add(aa);
                cbxArticulosAgrupacion.DataSource = listArtAg;
                cbxArticulosAgrupacion.DataBind();
                cbxArticulosAgrupacion.SelectedValue = "0";

            }
            catch (Exception ex)
            {
                ControladorExcepcion.tiraExcepcion(ex.Message);
            }
        }
    }
}