using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.Security;
using System.Web.UI.WebControls;
using Negocio;
using Entidades;


public partial class _Login : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        panelError.Visible = false;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        Usuario u = validaUsuario();
        if (u != null)
        {
            ApplicationSesion.ActiveUser = u;
            using (ControladorSucursal c_su = new ControladorSucursal())
            {
                ApplicationSesion.ActiveSucursal = c_su.BuscarSucursal(Convert.ToInt32(ucCbxSucursales.SelectedValue));
            }

            

            Caja c = new Caja(DateTime.Today, ApplicationSesion.ActiveUser.Idusuario, false);
            if (c.Idcaja != 0)
            {
                ApplicationSesion.ActiveCaja = c;
            }
            FormsAuthentication.RedirectFromLoginPage(u.Idusuario.ToString(), true);

            //// Success, create non-persistent authentication cookie.
            //FormsAuthentication.SetAuthCookie(txtUsuario.Text, false);

            //FormsAuthenticationTicket ticket1 =
            //   new FormsAuthenticationTicket(
            //        1,                                   // version
            //        this.txtUsuario.Text.Trim(),   // get username  from the form
            //        DateTime.Now,                        // issue time is now
            //        DateTime.Now.AddMinutes(10),         // expires in 10 minutes
            //        false,      // cookie is not persistent
            //        this.txtUsuario.Text.Trim()                         // usu assignment is stored
            //    // in userData
            //        );
            //HttpCookie cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket1));
            //Response.Cookies.Add(cookie1);
            //Session.Timeout = 30;
            ////Session.Add("usuario", txtUsuario.Text);
            //Session["usuario"] = u.NombreUsuario;
            //Session["idusuario"] = u.Idusuario;
            ////Session.Add("codusuario", codUsu);
            ////Session.Add("empresa", "CIBA");
            ////Session.Add("sucursal", "Casa Central, 8");
            //Response.Redirect("Default.aspx");
        }
    }




    private Usuario validaUsuario()
    {
        try
        {
            using (ControladorUsuarios c_usu = new ControladorUsuarios())
            {
                return c_usu.BuscarUsuario(txtUsuario.Text, txtPass.Text);

            }
        }
        catch (ExcepcionPropia myEx)
        {
            panelError.Visible = true;
            labelError.Text = myEx.Message;
            return null;
        }

    }




}
