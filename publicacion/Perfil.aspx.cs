using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Negocio;
using Entidades;

public partial class Perfil : System.Web.UI.Page
{
    static string modalCambiarImagen = "cambiarImagenModal";

    protected void Page_Load(object sender, EventArgs e)
    {
        ucPanelMensajes.PanelErrorVisible = false;
        ucPanelMensajes.PanelMensajeVisible = false;
        if (!IsPostBack)
        {
            try
            {
                Usuario UsuarioActual;
                using (ControladorUsuarios c_usu = new ControladorUsuarios())
                {
                    UsuarioActual = c_usu.BuscarUsuario(ApplicationSesion.ActiveUser.Idusuario);

                }
                lblNombre.Text = UsuarioActual.NombreApellido;
                lblNombreUsuario.Text = UsuarioActual.NombreUsuario;
                if (UsuarioActual.Imagen != string.Empty)
                {
                    literalImagen.Text = "<img alt='User Pic' height='100' width='100' src='imagenes/usuarios/" + UsuarioActual.Imagen + "' class='img-circle'>";
                }
                else
                {
                    literalImagen.Text = "<img alt='User Pic' height='100' width='100' src='imagenes/photo.png' class='img-circle'>";
                }
            }
            catch (ExcepcionPropia myex)
            {
                mostrarExcepcion(myex.Message);
            }
        }
    }
    protected void btnModificarClave_Click(object sender, EventArgs e)
    {
        abrirModal("modificarClaveModal");
    }
    protected void btnModificarClaveModal_Click(object sender, EventArgs e)
    {
        try
        {
            using (ControladorUsuarios c_usu = new ControladorUsuarios())
            {
                Usuario u = c_usu.BuscarUsuario(ApplicationSesion.ActiveUser.Idusuario);
                if (u.Clave != txtClaveActual.Text)
                {
                    throw new ExcepcionPropia("La clave actual no coincide con la anterior");
                }
                u.Clave = txtClaveNueva.Text;
                c_usu.ModificarUsuario(u);
            }
            cerrarModal("modificarClaveModal");
            mostrarMensaje("Clave modificada con exito");
        }
        catch (ExcepcionPropia myEx)
        {
            cerrarModal("modificarClaveModal");
            mostrarExcepcion(myEx.Message);

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
    protected void btnModificarImagen_Click(object sender, EventArgs e)
    {
        abrirModal(modalCambiarImagen);
    }
    protected void btnResetearImagen_Click(object sender, EventArgs e)
    {
        try
        {
            int idUsuario = ApplicationSesion.ActiveUser.Idusuario;
            using (ControladorUsuarios c_usu = new ControladorUsuarios())
            {
                c_usu.ModificarUsuarioImagen(idUsuario, null);
            }
            Response.Redirect("~/Perfil.aspx");
        }
        catch (Exception)
        {
            mostrarExcepcion("La imagen no se pudo cargar");

        }
    }
    protected void btnAgregarImagen_Click(object sender, EventArgs e)
    {
        Boolean fileOK = false;
        String path = Server.MapPath("~/imagenes/usuarios/");
        if (FileUpload1.HasFile)
        {
            String fileExtension =
                System.IO.Path.GetExtension(FileUpload1.FileName).ToLower();
            String[] allowedExtensions = { ".gif", ".png", ".jpeg", ".jpg" };
            for (int i = 0; i < allowedExtensions.Length; i++)
            {
                if (fileExtension == allowedExtensions[i])
                {
                    fileOK = true;
                }
            }
        }

        if (fileOK)
        {
            try
            {
                string fileExtension = System.IO.Path.GetExtension(FileUpload1.FileName).ToLower();
                string fileName = HttpContext.Current.User.Identity.Name + fileExtension;
                FileUpload1.PostedFile.SaveAs(path
                    + fileName);
                int idUsuario = ApplicationSesion.ActiveUser.Idusuario;
                using (ControladorUsuarios c_per = new ControladorUsuarios())
                {
                    c_per.ModificarUsuarioImagen(idUsuario, fileName);
                }
                Response.Redirect("~/Perfil.aspx");
            }
            catch (Exception)
            {
                mostrarExcepcion("La imagen no se pudo cargar");

            }
        }
        else
        {
            mostrarExcepcion("Solo Puede subir imagenes con extension gif, png, jpeg, o jpg");
        }
    }
}