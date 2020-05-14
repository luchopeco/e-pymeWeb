using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Controles_PanelMensajes : System.Web.UI.UserControl
{
    public bool PanelMensajeVisible
    {
        get
        {
            return PanelMensaje.Visible = true;
        }
        set
        {
            PanelMensaje.Visible = value;
        }
    }
    public bool PanelErrorVisible
    {
        get
        {
            return PanelError.Visible = true;
        }
        set
        {
            PanelError.Visible = value;
        }
    }
    public string LblMensaje
    {
        get { return LabelMensaje.Text; }
        set { LabelMensaje.Text = value; }
    }
    public string LblError
    {
        get { return lblError.Text; }
        set { lblError.Text = value; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {

    }
}