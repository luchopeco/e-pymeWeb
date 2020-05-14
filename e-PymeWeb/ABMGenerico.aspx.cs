using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Reflection;
using System.Data;
using Entidades;
using Negocio;

public partial class ABMGenerico : System.Web.UI.Page
{
    List<int> indiceColumnasAOcultar = new List<int>();
    protected void Page_Load(object sender, EventArgs e)
    {

        PanelMensaje.Visible = false;
        PanelError.Visible = false;
        if (Request.QueryString["entidad"] != null)
        {
            ViewState["entidad"] = Request.QueryString["entidad"];
            if (!IsPostBack)
            {
                Type tipoEntidad = HelperReflection.ObtenerTipoEntidad(ViewState["entidad"].ToString());
                lblTitulo.Text = "Gestion " + ((ABMClassAttribute)tipoEntidad.GetCustomAttributes(false)[0]).Titulo;
                lblPanel.Text = ((ABMClassAttribute)tipoEntidad.GetCustomAttributes(false)[0]).Titulo;
                ViewState["titulo"] = ((ABMClassAttribute)tipoEntidad.GetCustomAttributes(false)[0]).Titulo;
                bindGrid();

            }
            dgv.EditIndex = -1;
            generarFormDinamico();
            ///si estoy por modificar tengo q mantener la vista del formulario de modificacion al momento
            ///de hacer click en el boton aceptar. entonces solo mantengo la vista del formulario
            ///si hice click en un boton q no sea de la grilla, y q tenga el view state de l objeto a modificar
            ///lleno
            if (Page.Request.Params["__EVENTTARGET"] != null && Page.Request.Params["__EVENTTARGET"].ToString() == string.Empty && ViewState["objetoamodificar"] != null)
            {
                //string caca;
                //caca = Page.Request.Params["__EVENTTARGET"].ToString();
                generarFormularioModificacion();
            }

        }
    }

    private void bindGrid()
    {
        try
        {
            ///Obtengo la entidad en cuestion
            string entidad = ViewState["entidad"].ToString();
            ///Obtengo el nombre del metodo buscar todos
            string metodoBuscarTodos = HelperReflection.ObtenerNombreMetodoBuscarTodos(entidad);
            ///Ejecuto el metodo
            Object objetoAEnlazar = HelperReflection.EjecutarMetodo(entidad, metodoBuscarTodos, null);
            ViewState["lista"] = objetoAEnlazar;
            ///Establezco el datakey name
            dgv.DataKeyNames = HelperReflection.ObtenerClave(entidad);
            ///enlazo los datos a la grilla
            dgv.DataSource = objetoAEnlazar;
            dgv.DataBind();

        }
        catch (ExcepcionPropia ex)
        {
            PanelError.Visible = true;
            lblError.Text = ex.Message;
        }
        catch (TargetInvocationException exT)
        {
            PanelError.Visible = true;
            lblError.Text = exT.InnerException.Message;
        }
        catch (NullReferenceException nex)
        {
            PanelError.Visible = true;
            lblError.Text = nex.Message;
        }

    }
    protected void dgv_RowEditing(object sender, GridViewEditEventArgs e)
    {
        dgv.EditIndex = -1;
        ///Si el viewState esta lleno lo vacio
        ViewState["objetoamodificar"] = null;
        int index = Convert.ToInt32(e.NewEditIndex);
        ///Obtengo la entidad en cuestion
        string entidad = ViewState["entidad"].ToString();

        string code = dgv.DataKeys[index].Value.ToString();
        List<object> arg = new List<object>();
        arg.Add(Convert.ToInt32(code));
        string metodo = HelperReflection.ObtenerNombreMetodoBuscar(entidad);
        object objetoEntidad = HelperReflection.EjecutarMetodo(entidad, metodo, arg);
        ViewState["objetoamodificar"] = objetoEntidad;

        generarFormularioModificacion();
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append(@"<script type='text/javascript'>");
        sb.Append("$('#editModal').modal('show');");
        sb.Append(@"</script>");
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditShowModalScript", sb.ToString(), false);
    }
    private void generarFormularioModificacion()
    {
        string entidad = ViewState["entidad"].ToString();
        Object objetoEntidad = ViewState["objetoamodificar"];
        ///Obtengo sus propiedades
        PropertyInfo[] listPropiedades = HelperReflection.ObtenerListPropiedades(entidad);
        foreach (PropertyInfo p in listPropiedades)
        {
            object[] o = p.GetCustomAttributes(false);
            ///Si no tiene atributos muestro como viene
            if (o.Length == 0)
            {
                mostrarPropiedadTipoBasico(objetoEntidad, p);
            }
            ///Si la propiedad tiene atributo
            else
            {
                //si hay q mostrarolo
                if (((ABMPropertyAttribute)p.GetCustomAttributes(false)[0]).MostrarSiempre)
                {
                    //si no es comboBox
                    if (((ABMPropertyAttribute)p.GetCustomAttributes(false)[0]).NameSpace == null)
                    {
                        mostrarPropiedadTipoBasico(objetoEntidad, p);
                    }
                    ///Si es un comboBox
                    else
                    {
                        Label lbl = new Label();
                        lbl.Text = Helper.SepararPalabraCamello(p.Name);
                        DropDownList cbx = new DropDownList();
                        cbx.CssClass = "form-control";
                        cbx.DataTextField = ((ABMPropertyAttribute)p.GetCustomAttributes(false)[0]).DataTextField;
                        cbx.DataValueField = ((ABMPropertyAttribute)p.GetCustomAttributes(false)[0]).DataValueField;
                        cbx.DataSource = HelperReflection.EjecutarMetodo(p);
                        cbx.DataBind();
                        if (!((ABMPropertyAttribute)p.GetCustomAttributes(false)[0]).NoNulo)
                        {
                            ListItem li = new ListItem();
                            li.Value = "0";
                            li.Text = "<-- Sin Datos -->";
                            cbx.Items.Insert(0, li);
                        }

                        string selectdValue = ((ABMPropertyAttribute)p.GetCustomAttributes(false)[0]).SelectedValue;
                        PropertyInfo prop = listPropiedades.FirstOrDefault(pr => pr.Name == selectdValue);
                        object valorPropiedad = HelperReflection.ObtenerValorPropiedad(objetoEntidad, prop);
                        if (valorPropiedad != null)
                        {
                            cbx.SelectedValue = valorPropiedad.ToString();
                            //cbx.DataBind();
                        }

                        TableCell td_Celda0 = new TableCell();
                        td_Celda0.Controls.Add(lbl);

                        TableCell td_Celda = new TableCell();
                        td_Celda.Controls.Add(cbx);

                        TableRow tr_Celda = new TableRow();
                        tr_Celda.Controls.Add(td_Celda0);
                        tr_Celda.Controls.Add(td_Celda);

                        tablaModificar.Controls.Add(tr_Celda);
                    }

                }
                if (((ABMPropertyAttribute)p.GetCustomAttributes(false)[0]).EsClave)
                {
                    hfClave.Value = HelperReflection.ObtenerValorPropiedad(objetoEntidad, p).ToString();
                }
            }

        }
    }
    /// <summary>
    /// Se encaraga de mostrar la propiedad con el control correspondiente y
    /// el valor de lapropuiedad. Se utiliza cuando estoy modificando.
    /// Creo el control y muestro la informacion
    /// </summary>
    /// <param name="objetoEntidad"></param>
    /// <param name="p"></param>
    private void mostrarPropiedadTipoBasico(Object objetoEntidad, PropertyInfo p)
    {
        string tipoPropiedad = p.PropertyType.FullName;
        string tipoString = Type.GetType("System.String").Name;
        string tipoBool = Type.GetType("System.Boolean").Name;
        string tipoDate = Type.GetType("System.DateTime").Name;
        string tipoInt = Type.GetType("System.Int32").Name;
        string tipoBigInt = Type.GetType("System.Int64").Name;
        string tipoDecimal = Type.GetType("System.Decimal").Name;

        if (tipoPropiedad.Contains(tipoString))
        {
            Label lbl = new Label();
            lbl.Text = Helper.SepararPalabraCamello(p.Name);
            TextBox text = new TextBox();

            object objeto = HelperReflection.ObtenerValorPropiedad(objetoEntidad, p);
            if (objeto != null)
            {
                text.Text = objeto.ToString();
            }

            text.CssClass = "form-control";
            TableCell td_Celda0 = new TableCell();
            td_Celda0.Controls.Add(lbl);

            TableCell td_Celda = new TableCell();
            td_Celda.Controls.Add(text);

            TableRow tr_Celda = new TableRow();
            tr_Celda.Controls.Add(td_Celda0);
            tr_Celda.Controls.Add(td_Celda);

            tablaModificar.Controls.Add(tr_Celda);
        }
        else if (tipoPropiedad.Contains(tipoInt) || tipoPropiedad.Contains(tipoBigInt))
        {
            Label lbl = new Label();
            lbl.Text = Helper.SepararPalabraCamello(p.Name);
            TextBox text = new TextBox();

            object objeto = HelperReflection.ObtenerValorPropiedad(objetoEntidad, p);
            if (objeto != null)
            {
                text.Text = objeto.ToString();
            }

            text.CssClass = "form-control";
            text.Attributes.Add("placeholder", "Ingrese un numero entero");
            TableCell td_Celda0 = new TableCell();
            td_Celda0.Controls.Add(lbl);

            TableCell td_Celda = new TableCell();
            td_Celda.Controls.Add(text);

            TableRow tr_Celda = new TableRow();
            tr_Celda.Controls.Add(td_Celda0);
            tr_Celda.Controls.Add(td_Celda);

            tablaModificar.Controls.Add(tr_Celda);
        }
        else if (tipoPropiedad.Contains(tipoDecimal))
        {
            Label lbl = new Label();
            lbl.Text = Helper.SepararPalabraCamello(p.Name);
            TextBox text = new TextBox();

            object objeto = HelperReflection.ObtenerValorPropiedad(objetoEntidad, p);
            if (objeto != null)
            {
                text.Text = objeto.ToString();
            }

            text.CssClass = "form-control";
            text.Attributes.Add("placeholder", "Ingrese un numero decimal");
            TableCell td_Celda0 = new TableCell();
            td_Celda0.Controls.Add(lbl);

            TableCell td_Celda = new TableCell();
            td_Celda.Controls.Add(text);

            TableRow tr_Celda = new TableRow();
            tr_Celda.Controls.Add(td_Celda0);
            tr_Celda.Controls.Add(td_Celda);

            tablaModificar.Controls.Add(tr_Celda);
        }
        else if (tipoPropiedad.Contains(tipoDate))
        {
            Label lbl = new Label();
            lbl.Text = Helper.SepararPalabraCamello(p.Name);
            TextBox text = new TextBox();

            object objeto = HelperReflection.ObtenerValorPropiedad(objetoEntidad, p);
            if (objeto != null)
            {
                text.Text = objeto.ToString();
            }

            text.CssClass = "form-control datepicker";
            text.Attributes.Add("placeholder", "Ingrese una fecha");
            TableCell td_Celda0 = new TableCell();
            td_Celda0.Controls.Add(lbl);

            TableCell td_Celda = new TableCell();
            td_Celda.Controls.Add(text);

            TableRow tr_Celda = new TableRow();
            tr_Celda.Controls.Add(td_Celda0);
            tr_Celda.Controls.Add(td_Celda);

            tablaModificar.Controls.Add(tr_Celda);
        }
        if (tipoPropiedad.Contains(tipoBool))
        {
            Label lbl = new Label();
            lbl.Text = Helper.SepararPalabraCamello(p.Name);
            CheckBox chk = new CheckBox();
            chk.Checked = Convert.ToBoolean(HelperReflection.ObtenerValorPropiedad(objetoEntidad, p));
            chk.CssClass = "form-control";

            TableCell td_Celda0 = new TableCell();
            td_Celda0.Controls.Add(lbl);

            TableCell td_Celda = new TableCell();
            td_Celda.Controls.Add(chk);

            TableRow tr_Celda = new TableRow();
            tr_Celda.Controls.Add(td_Celda0);
            tr_Celda.Controls.Add(td_Celda);

            tablaModificar.Controls.Add(tr_Celda);
        }
    }

    protected void dgv_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        //Update the values.
        GridViewRow row = dgv.Rows[e.RowIndex];
        List<object> argumentos = new List<object>();
        int celdasAUsar = 0;
        foreach (TableCell celda in row.Cells)
        {
            if (celdasAUsar > 0)
            {
                argumentos.Add(celda.Text);
            }
            celdasAUsar = celdasAUsar + 1;
        }

        ViewState["argumentos"] = argumentos;
        lblEliminar.Text = ViewState["titulo"].ToString();
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append(@"<script type='text/javascript'>");
        sb.Append("$('#deleteModal').modal('show');");
        sb.Append(@"</script>");
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "DeleteModalScript", sb.ToString(), false);
    }
    protected void dgv_RowCreated(object sender, GridViewRowEventArgs e)
    {
        /////busco las propiedades q no se deben mostrar
        List<string> listPropNoMostrar = new List<string>();
        /////Obtengo la entidad en cuestion
        string entidad = ViewState["entidad"].ToString();
        ///Busco las propiedades de la entidad
        PropertyInfo[] listPropiedades = HelperReflection.ObtenerListPropiedades(entidad);
        foreach (PropertyInfo p in listPropiedades)
        {
            object[] o = p.GetCustomAttributes(false);
            if (o.Length > 0)
            {
                if (!((ABMPropertyAttribute)p.GetCustomAttributes(false)[0]).MostrarEnGrilla)
                {
                    string propiedadNoMostrar = p.Name;
                    listPropNoMostrar.Add(propiedadNoMostrar);
                }
            }
        }
        //Find indexes
        if (e.Row.RowType == DataControlRowType.Header)
        {
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                string titulo = e.Row.Cells[i].Text;
                foreach (string prop in listPropNoMostrar)
                {
                    if (e.Row.Cells[i].Text == prop)
                    {
                        indiceColumnasAOcultar.Add(i);
                    }
                }
                e.Row.Cells[i].Text = Helper.SepararPalabraCamello(titulo);
            }
        }
        //Hide cells
        foreach (var index in indiceColumnasAOcultar)
        {
            e.Row.Cells[index].Visible = false;
        }
        ///pongo los toolTip
        try
        {
            ((System.Web.UI.WebControls.LinkButton)(e.Row.Cells[0].Controls[0])).ToolTip = "Modificar";
            ((System.Web.UI.WebControls.LinkButton)(e.Row.Cells[0].Controls[2])).ToolTip = "Eliminar";

        }
        catch (Exception)
        {
        }
    }
    protected void btnEliminarMenu_Click(object sender, EventArgs e)
    {
        List<object> argumentos = (List<object>)ViewState["argumentos"];
        try
        {
            ///Obtengo la entidad en cuestion
            string entidad = ViewState["entidad"].ToString();
            List<object> listArgumentos = new List<object>();
            listArgumentos.Add(Convert.ToInt32(argumentos[0]));
            //listArgumentos.Add(DateTime.Today);
            string metodo = HelperReflection.ObtenerNombreMetodoBaja(entidad);
            HelperReflection.EjecutarMetodo(entidad, metodo, listArgumentos);
            bindGrid();

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#deleteModal').modal('hide');");
            sb.Append("var focalizar = $('#ctl00_ContentPlaceHolder1_btnNuevo').position().top;");
            sb.Append("$('html,body').animate({scrollTop: focalizar}, 500);");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "hideDeleteModalScript", sb.ToString(), false);
            PanelMensaje.Visible = true;
            LabelMensaje.Text = "Eliminacion realizada con exito";

        }
        catch (ExcepcionPropia ex)
        {
            PanelError.Visible = true;
            lblError.Text = ex.Message;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#deleteModal').modal('hide');");
            sb.Append("var focalizar = $('#ctl00_ContentPlaceHolder1_btnNuevo').position().top;");
            sb.Append("$('html,body').animate({scrollTop: focalizar}, 500);");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddHideModalScript", sb.ToString(), false);
        }
        catch (TargetInvocationException exT)
        {
            PanelError.Visible = true;
            lblError.Text = exT.InnerException.Message;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#deleteModal').modal('hide');");
            sb.Append("var focalizar = $('#ctl00_ContentPlaceHolder1_btnNuevo').position().top;");
            sb.Append("$('html,body').animate({scrollTop: focalizar}, 500);");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddHideModalScript", sb.ToString(), false);
        }
        catch (NullReferenceException nex)
        {
            PanelError.Visible = true;
            lblError.Text = nex.Message;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#deleteModal').modal('hide');");
            sb.Append("var focalizar = $('#ctl00_ContentPlaceHolder1_btnNuevo').position().top;");
            sb.Append("$('html,body').animate({scrollTop: focalizar}, 500);");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddHideModalScript", sb.ToString(), false);
        }
        catch (MissingMethodException nex)
        {
            PanelError.Visible = true;
            lblError.Text = nex.Message;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#deleteModal').modal('hide');");
            sb.Append("var focalizar = $('#ctl00_ContentPlaceHolder1_btnNuevo').position().top;");
            sb.Append("$('html,body').animate({scrollTop: focalizar}, 500);");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddHideModalScript", sb.ToString(), false);
        }
    }
    protected void btnNuevo_Click(object sender, EventArgs e)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append(@"<script type='text/javascript'>");
        sb.Append("$('#addModal').modal('show');");
        sb.Append(@"</script>");
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddShowModalScript", sb.ToString(), false);

    }
    private void generarFormDinamico()
    {
        ///Obtengo la entidad en cuestion
        string entidad = ViewState["entidad"].ToString();
        ///Obtengo sus propiedades
        PropertyInfo[] listPropiedades = HelperReflection.ObtenerListPropiedades(entidad);
        foreach (PropertyInfo p in listPropiedades)
        {
            object[] o = p.GetCustomAttributes(false);
            ///Si no tiene propiedades o el nameSpace es null
            ///Entonces tengo q generar un control q no es combobox
            if (o.Length == 0)
            {
                mostrarPropiedadTipoBasico(p);
            }
            ///si posee un 
            else
            {
                //si hay q mostrarolo ya 
                if (((ABMPropertyAttribute)p.GetCustomAttributes(false)[0]).MostrarSiempre)
                {
                    if (((ABMPropertyAttribute)p.GetCustomAttributes(false)[0]).NameSpace == null)
                    {
                        mostrarPropiedadTipoBasico(p);
                    }
                    else
                    {
                        Label lbl = new Label();
                        lbl.Text = Helper.SepararPalabraCamello(p.Name);
                        DropDownList cbx = new DropDownList();
                        cbx.CssClass = "form-control";
                        cbx.DataTextField = ((ABMPropertyAttribute)p.GetCustomAttributes(false)[0]).DataTextField;
                        cbx.DataValueField = ((ABMPropertyAttribute)p.GetCustomAttributes(false)[0]).DataValueField;
                        cbx.DataSource = HelperReflection.EjecutarMetodo(p);
                        cbx.DataBind();
                        if (!((ABMPropertyAttribute)p.GetCustomAttributes(false)[0]).NoNulo)
                        {
                            ListItem li = new ListItem();
                            li.Value = "0";
                            li.Text = "<-- Sin Datos -->";
                            cbx.Items.Insert(0, li);
                        }

                        TableCell td_Celda0 = new TableCell();
                        td_Celda0.Controls.Add(lbl);

                        TableCell td_Celda = new TableCell();
                        td_Celda.Controls.Add(cbx);

                        TableRow tr_Celda = new TableRow();
                        tr_Celda.Controls.Add(td_Celda0);
                        tr_Celda.Controls.Add(td_Celda);

                        tablaAgregar.Controls.Add(tr_Celda);
                    }
                }
            }

        }
    }
    /// <summary>
    /// Se encaraga de mostrar la propiedad con el control correspondiente. 
    /// Se utiliza cuando estoy Agregando.
    /// Creo el control.
    /// </summary>
    /// <param name="p"></param>
    private void mostrarPropiedadTipoBasico(PropertyInfo p)
    {
        string tipoPropiedad = p.PropertyType.FullName;
        string tipoString = Type.GetType("System.String").Name;
        string tipoBool = Type.GetType("System.Boolean").Name;
        string tipoDate = Type.GetType("System.DateTime").Name;
        string tipoInt = Type.GetType("System.Int32").Name;
        string tipoBigInt = Type.GetType("System.Int64").Name;
        string tipoDecimal = Type.GetType("System.Decimal").Name;
        if (tipoPropiedad.Contains(tipoString))
        {
            Label lbl = new Label();
            lbl.Text = Helper.SepararPalabraCamello(p.Name);
            TextBox text = new TextBox();
            text.CssClass = "form-control";
            TableCell td_Celda0 = new TableCell();
            td_Celda0.Controls.Add(lbl);

            TableCell td_Celda = new TableCell();
            td_Celda.Controls.Add(text);

            TableRow tr_Celda = new TableRow();
            tr_Celda.Controls.Add(td_Celda0);
            tr_Celda.Controls.Add(td_Celda);

            tablaAgregar.Controls.Add(tr_Celda);
        }
        else if (tipoPropiedad.Contains(tipoInt) || tipoPropiedad.Contains(tipoBigInt))
        {
            Label lbl = new Label();
            lbl.Text = Helper.SepararPalabraCamello(p.Name);
            TextBox text = new TextBox();
            text.CssClass = "form-control";
            text.Attributes.Add("placeholder", "Ingrese un numero entero");
            TableCell td_Celda0 = new TableCell();
            td_Celda0.Controls.Add(lbl);

            TableCell td_Celda = new TableCell();
            td_Celda.Controls.Add(text);

            TableRow tr_Celda = new TableRow();
            tr_Celda.Controls.Add(td_Celda0);
            tr_Celda.Controls.Add(td_Celda);

            tablaAgregar.Controls.Add(tr_Celda);
        }
        else if (tipoPropiedad.Contains(tipoDecimal))
        {
            Label lbl = new Label();
            lbl.Text = Helper.SepararPalabraCamello(p.Name);
            TextBox text = new TextBox();
            text.CssClass = "form-control";
            text.Attributes.Add("placeholder", "Ingrese un numero decimal");
            TableCell td_Celda0 = new TableCell();
            td_Celda0.Controls.Add(lbl);

            TableCell td_Celda = new TableCell();
            td_Celda.Controls.Add(text);

            TableRow tr_Celda = new TableRow();
            tr_Celda.Controls.Add(td_Celda0);
            tr_Celda.Controls.Add(td_Celda);

            tablaAgregar.Controls.Add(tr_Celda);
        }
        else if (tipoPropiedad.Contains(tipoDate))
        {
            Label lbl = new Label();
            lbl.Text = Helper.SepararPalabraCamello(p.Name);
            TextBox text = new TextBox();
            text.CssClass = "form-control datepicker";
            text.Attributes.Add("placeholder", "Ingrese una fecha");
            TableCell td_Celda0 = new TableCell();
            td_Celda0.Controls.Add(lbl);

            TableCell td_Celda = new TableCell();
            td_Celda.Controls.Add(text);

            TableRow tr_Celda = new TableRow();
            tr_Celda.Controls.Add(td_Celda0);
            tr_Celda.Controls.Add(td_Celda);

            tablaAgregar.Controls.Add(tr_Celda);
        }
        else if (tipoPropiedad.Contains(tipoBool))
        {
            Label lbl = new Label();
            lbl.Text = Helper.SepararPalabraCamello(p.Name);
            CheckBox chk = new CheckBox();
            chk.CssClass = "form-control";

            TableCell td_Celda0 = new TableCell();
            td_Celda0.Controls.Add(lbl);

            TableCell td_Celda = new TableCell();
            td_Celda.Controls.Add(chk);

            TableRow tr_Celda = new TableRow();
            tr_Celda.Controls.Add(td_Celda0);
            tr_Celda.Controls.Add(td_Celda);

            tablaAgregar.Controls.Add(tr_Celda);
        }
    }
    protected void btnAgrear_Click(object sender, EventArgs e)
    {
        try
        {
            /////Obtengo la entidad en cuestion
            string entidad = ViewState["entidad"].ToString();
            List<object> argumentos = new List<object>();
            foreach (TableRow tr in tablaAgregar.Rows)
            {
                foreach (TableCell tc in tr.Cells)
                {
                    foreach (Control c in tc.Controls)
                    {
                        try
                        {
                            //argumentos[indice] = celda.Controls[0].ToString();
                            TextBox text = (TextBox)c;
                            if (text.Text != string.Empty)
                            {
                                string placeholder = "";
                                try
                                {
                                    placeholder = text.Attributes["placeholder"];

                                    if (placeholder.Contains("entero"))
                                    {
                                        argumentos.Add(Convert.ToInt32(text.Text));
                                    }
                                    else if (placeholder.Contains("decimal"))
                                    {
                                        argumentos.Add(Convert.ToDecimal(text.Text));
                                    }
                                    else if (placeholder.Contains("fecha"))
                                    {
                                        argumentos.Add(Convert.ToDateTime(text.Text));
                                    }
                                }
                                catch (Exception)
                                {
                                    argumentos.Add(text.Text);
                                }

                            }
                            else
                            {
                                argumentos.Add(null);
                            }
                        }
                        catch (InvalidCastException)
                        {
                            try
                            {
                                CheckBox chk = (CheckBox)c;
                                argumentos.Add(chk.Checked);
                                // indice = indice + 1;
                            }
                            catch (InvalidCastException)
                            {
                                try
                                {
                                    DropDownList cbx = (DropDownList)c;
                                    argumentos.Add(Convert.ToInt32(cbx.SelectedValue));
                                }
                                catch (InvalidCastException)
                                {

                                }
                            }
                        }
                    }
                }
            }

            string metodo = HelperReflection.ObtenerNombreMetodoAlta(entidad);
            HelperReflection.EjecutarMetodo(entidad, metodo, argumentos);
            bindGrid();
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#addModal').modal('hide');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddHideModalScript", sb.ToString(), false);
            PanelMensaje.Visible = true;
            LabelMensaje.Text = ViewState["titulo"].ToString() + " Agregado con Exito";
        }
        catch (ExcepcionPropia ex)
        {
            PanelError.Visible = true;
            lblError.Text = ex.Message;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#addModal').modal('hide');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddHideModalScript", sb.ToString(), false);
        }
        catch (TargetInvocationException exT)
        {
            PanelError.Visible = true;
            lblError.Text = exT.InnerException.Message;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#addModal').modal('hide');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddHideModalScript", sb.ToString(), false);
        }
        catch (NullReferenceException nex)
        {
            PanelError.Visible = true;
            lblError.Text = nex.Message;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#addModal').modal('hide');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddHideModalScript", sb.ToString(), false);
        }
        catch (MissingMethodException nex)
        {
            PanelError.Visible = true;
            lblError.Text = nex.Message;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#addModal').modal('hide');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddHideModalScript", sb.ToString(), false);
        }
    }
    protected void btnModificar_Click(object sender, EventArgs e)
    {
        try
        {
            /////Obtengo la entidad en cuestion
            string entidad = ViewState["entidad"].ToString();
            List<object> argumentos = new List<object>();
            argumentos.Add(Convert.ToInt32(hfClave.Value));
            foreach (TableRow tr in tablaModificar.Rows)
            {
                foreach (TableCell tc in tr.Cells)
                {
                    foreach (Control c in tc.Controls)
                    {
                        try
                        {
                            //argumentos[indice] = celda.Controls[0].ToString();
                            TextBox text = (TextBox)c;
                            if (text.Text != string.Empty)
                            {
                                string placeholder = "";
                                try
                                {
                                    placeholder = text.Attributes["placeholder"];
                                    if (placeholder.Contains("entero"))
                                    {
                                        argumentos.Add(Convert.ToInt32(text.Text));
                                    }
                                    else if (placeholder.Contains("decimal"))
                                    {
                                        argumentos.Add(Convert.ToDecimal(text.Text));
                                    }
                                    else if (placeholder.Contains("fecha"))
                                    {
                                        argumentos.Add(Convert.ToDateTime(text.Text));
                                    }

                                }
                                catch (Exception)
                                {
                                    argumentos.Add(text.Text);
                                }

                            }
                            else
                            {
                                argumentos.Add(null);
                            }
                        }
                        catch (InvalidCastException)
                        {
                            try
                            {
                                CheckBox chk = (CheckBox)c;
                                argumentos.Add(chk.Checked);
                                // indice = indice + 1;
                            }
                            catch (InvalidCastException)
                            {
                                try
                                {
                                    DropDownList cbx = (DropDownList)c;
                                    argumentos.Add(Convert.ToInt32(cbx.SelectedValue));
                                }
                                catch (InvalidCastException)
                                {

                                }
                            }
                        }
                    }
                }
            }

            string metodo = HelperReflection.ObtenerNombreMetodoModificar(entidad);
            HelperReflection.EjecutarMetodo(entidad, metodo, argumentos);
            bindGrid();
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#editModal').modal('hide');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddHideModalScript", sb.ToString(), false);
            PanelMensaje.Visible = true;
            LabelMensaje.Text = ViewState["titulo"].ToString() + " Modificado con Exito";
            System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
            sb1.Append(@"<script type='text/javascript'>");
            sb1.Append("var focalizar = $('#ctl00_ContentPlaceHolder1_btnNuevo').position().top;");
            sb1.Append("$('html,body').animate({scrollTop: focalizar}, 500);");
            sb1.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", sb1.ToString(), false);
        }
        catch (ExcepcionPropia ex)
        {
            PanelError.Visible = true;
            lblError.Text = ex.Message;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#editModal').modal('hide');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddHideModalScript", sb.ToString(), false);
        }
        catch (TargetInvocationException exT)
        {
            PanelError.Visible = true;
            lblError.Text = exT.InnerException.Message;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#editModal').modal('hide');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddHideModalScript", sb.ToString(), false);
        }
        catch (NullReferenceException nex)
        {
            PanelError.Visible = true;
            lblError.Text = nex.Message;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#editModal').modal('hide');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddHideModalScript", sb.ToString(), false);
        }
        catch (MissingMethodException nex)
        {
            PanelError.Visible = true;
            lblError.Text = nex.Message;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#editModal').modal('hide');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddHideModalScript", sb.ToString(), false);
        }
    }
    protected void btnExoprtarAExcel_Click(object sender, EventArgs e)
    {
        if (dgv.Rows.Count == 0)
        {
            mostrarExcepcion("Debe Realizar una busqueda");
        }
        else
        {
            //string datestyle = @"<style> .date{ mso-number-format:\#\.000; }</style>";
            //foreach (GridViewRow oItem in dgvArticulos.Rows)
            //    oItem.Cells[3].Attributes.Add("class", "date");

            Response.Clear();
            Response.AddHeader("content-disposition", "attachment; filename=Exportar.xls");
            Response.ContentType = "application/vnd.xls";
            System.IO.StringWriter WriteItem = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter htmlText = new HtmlTextWriter(WriteItem);
            //esponse.Write(datestyle);
            dgv.RenderControl(htmlText);
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
}