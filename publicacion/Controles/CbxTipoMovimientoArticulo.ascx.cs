using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entidades;
using Negocio;

public partial class Controles_CbxTipoMovimientoArticulo : System.Web.UI.UserControl
{
    public int SelectedValue
    {
        get { return Convert.ToInt32(cbxTipoMovimientos.SelectedValue); }
        set { cbxTipoMovimientos.SelectedValue = value.ToString(); }
    }
    public string SelectedText
    {
        get { return cbxTipoMovimientos.SelectedItem.Text; }
        set { cbxTipoMovimientos.SelectedValue = value; }
    }
    public bool CbxArticuloAgrupacionEnable
    {
        get { return cbxTipoMovimientos.Enabled; }
        set { cbxTipoMovimientos.Enabled = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                List<TipoMovimientoArticulo> listTipoMovimiento;
                using (ControladorMovimientos c_suc = new ControladorMovimientos())
                {
                    listTipoMovimiento = c_suc.BuscarListTiposMovimientos();
                }
                if (listTipoMovimiento != null && listTipoMovimiento.Count > 0)
                {
                    cbxTipoMovimientos.DataTextField = "Descripcion";
                    cbxTipoMovimientos.DataValueField = "IdTipoMovimiento";
                    cbxTipoMovimientos.DataSource = listTipoMovimiento;
                    cbxTipoMovimientos.DataBind();
                }
            }
            catch (ExcepcionPropia)
            {

            }
        }
    }

}