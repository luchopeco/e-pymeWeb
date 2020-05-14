using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Negocio;
using Entidades;

public partial class Controles_CbxSucursales : System.Web.UI.UserControl
{
    public int SelectedValue
    {
        get { return Convert.ToInt32(cbxSucursal.SelectedValue); }
        set { cbxSucursal.SelectedValue = value.ToString(); }
    }
    public string SelectedText
    {
        get { return cbxSucursal.SelectedItem.Text; }
        set { cbxSucursal.SelectedValue = value; }
    }
    public bool CbxArticuloAgrupacionEnable
    {
        get { return cbxSucursal.Enabled; }
        set { cbxSucursal.Enabled = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                List<Sucursal> listArtAg;
                using (ControladorSucursal c_suc = new ControladorSucursal())
                {
                    listArtAg = c_suc.BuscarListSucursales();
                }
                if (listArtAg != null && listArtAg.Count > 0)
                {
                    cbxSucursal.DataTextField = "Descripcion";
                    cbxSucursal.DataValueField = "IdSucursal";
                    cbxSucursal.DataSource = listArtAg.OrderBy(ss => ss.Descripcion);
                    cbxSucursal.DataBind();
                }
            }
            catch (ExcepcionPropia)
            {

            }
        }
    }
}