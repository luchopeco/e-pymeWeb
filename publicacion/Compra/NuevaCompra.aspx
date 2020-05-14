<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="NuevaCompra.aspx.cs" Inherits="Compra_NuevaCompra" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../Controles/CbxArticulosAgrupacion.ascx" TagName="CbxArticulosAgrupacion"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <script src="../js/datepickerPropio.js" type="text/javascript"></script>
    <script type="text/javascript">
        function ClientProveedorSelected(source, eventArgs) {
            //alert(" Key : " + eventArgs.get_text() + "  Value :  " + eventArgs.get_value());
            document.getElementById('<%=hfIdProveedor.ClientID %>').value = eventArgs.get_value();
        }
        function ClientArticuloSelected(source, eventArgs) {
            //alert(" Key : " + eventArgs.get_text() + "  Value :  " + eventArgs.get_value());
            document.getElementById('<%=hfIdArticuloDescripcion.ClientID %>').value = eventArgs.get_value();
        }
    </script>
    <div class="main-header">
        <h2>
            <asp:Label ID="lblTituloPagina" runat="server"></asp:Label>
        </h2>
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class=" col-lg-12">
                <asp:Panel ID="PanelError" Visible="true" runat="server">
                    <div class="alert alert-danger alert-dismissable">
                        <button type="button" class="close" data-dismiss="alert" aria-hidden="true">
                            ×</button>
                        <asp:Label ID="lblError" runat="server" Text=""></asp:Label>
                    </div>
                </asp:Panel>
                <asp:Panel ID="PanelMensaje" Visible="true" runat="server">
                    <div class="alert alert-success alert-dismissable">
                        <button type="button" class="close" data-dismiss="alert" aria-hidden="true">
                            ×</button>
                        <asp:Label ID="LabelMensaje" runat="server" Text=""></asp:Label>
                    </div>
                </asp:Panel>
            </div>
            <div class=" col-md-6">
                <div class=" widget">
                    <div class=" widget-header">
                        <h3>
                            <i class="fa fa-edit"></i>Tipo de compra
                        </h3>
                    </div>
                    <div class=" widget-content">
                        <asp:DropDownList ID="cbxTipoCompra" CssClass=" form-control" runat="server">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class=" col-md-6">
                <div class=" widget">
                    <div class=" widget-header">
                        <h3>
                            <i class="fa fa-edit"></i>Proveedor
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtProveedor"
                                ValidationGroup="compra" ForeColor="Red" ErrorMessage="Ingrese un proveedor"></asp:RequiredFieldValidator>
                        </h3>
                    </div>
                    <div class=" widget-content">
                        <asp:TextBox ID="txtProveedor" PlaceHolder="Ingrese proveedor y seleccionelo" class="form-control"
                            runat="server" AutoPostBack="False"></asp:TextBox>
                        <asp:AutoCompleteExtender ID="txtProveedor_AutoCompleteExtender" runat="server" CompletionInterval="10"
                            DelimiterCharacters="" Enabled="True" MinimumPrefixLength="3" ServiceMethod="BuscarProveedores"
                            UseContextKey="true" ServicePath="~/Autocomplete.asmx" TargetControlID="txtProveedor"
                            OnClientItemSelected="ClientProveedorSelected">
                        </asp:AutoCompleteExtender>
                        <asp:HiddenField ID="hfIdProveedor" runat="server" />
                    </div>
                </div>
            </div>
            <div class=" col-md-8">
                <div class=" widget">
                    <div class=" widget-header">
                        <h3>
                            <i class="fa fa-edit"></i>Linea de Compra
                        </h3>
                        <div class="btn-group widget-header-toolbar">
                            <asp:Button ID="btnNuevoArt" runat="server" Text="Nuevo Articulo" CssClass="btn btn-info btn-xs"
                                OnClick="btnNuevoArt_Click" />
                        </div>
                    </div>
                    <div class=" widget-content">
                        <legend>Busqueda de articulos</legend>
                        <div class=" row">
                            <div class=" col-sm-6">
                                <div class="input-group ">
                                    <asp:TextBox ID="txtArticuloCodigo" PlaceHolder="Busqueda Codigo" class="form-control"
                                        runat="server"></asp:TextBox>
                                    <span class="input-group-btn">
                                        <asp:LinkButton ID="btnBuscarArtXCodigo" class="btn btn-default" runat="server" OnClick="btnBuscarArtXCodigo_Click"><i class="fa fa-search"></i></asp:LinkButton>
                                    </span>
                                </div>
                            </div>
                            <div class=" col-md-6">
                                <div class="input-group ">
                                    <asp:TextBox ID="txtDescArticulo" PlaceHolder="Busqueda por nombre" class="form-control"
                                        runat="server"></asp:TextBox>
                                    <span class="input-group-btn">
                                        <asp:LinkButton ID="btnBuscarArtXDesc" class="btn btn-default" runat="server" OnClick="btnBuscarArtXDesc_Click"><i class="fa fa-search"></i></asp:LinkButton>
                                    </span>
                                </div>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" CompletionInterval="10"
                                    DelimiterCharacters="" Enabled="True" MinimumPrefixLength="3" ServiceMethod="BuscarArticulosCompra"
                                    UseContextKey="true" ServicePath="~/Autocomplete.asmx" TargetControlID="txtDescArticulo"
                                    OnClientItemSelected="ClientArticuloSelected">
                                </asp:AutoCompleteExtender>
                                <asp:HiddenField ID="hfIdArticuloDescripcion" runat="server" />
                            </div>
                        </div>
                        <hr class="inner-separator">
                        <legend>Articulos de la compra</legend>
                        <div>
                            <asp:GridView ID="dgvArticulos" runat="server" CssClass="table table-striped table-bordered table-hover dataTable no-footer"
                                AutoGenerateColumns="False" OnRowCommand="dgvArticulos_RowCommand">
                                <Columns>
                                    <asp:BoundField DataField="DescArticulo" HeaderText="Articulo" />
                                    <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" />
                                    <asp:BoundField DataField="CostoUnitario" HeaderText="Costo Unitario" />
                                    <asp:BoundField DataField="PrecioVenta" HeaderText="Precio Venta" />
                                    <asp:BoundField DataField="Subtotal" HeaderText="SubTotal" />                                
                                    <asp:ButtonField CommandName="deleteArticulo" Text="&lt;i class='fa fa-times'&gt;&lt;/i&gt;">
                                        <ControlStyle CssClass="btn btn-xs btn-danger" />
                                    </asp:ButtonField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
            <div class=" col-md-4">
                <div class=" widget">
                    <div class=" widget-header">
                        <h3>
                            <i class="fa fa-edit"></i>Total
                        </h3>
                    </div>
                    <div class=" widget-content">
                        <div class="input-group">
                            <span class="input-group-addon">$</span>
                            <asp:TextBox ID="txtTotal" CssClass=" form-control " runat="server"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class=" widget">
                    <div class=" widget-header">
                        <h3>
                            <i class="fa fa-edit"></i>Forma de pago
                        </h3>
                        <div class="btn-group widget-header-toolbar">
                            <asp:Button ID="btnNuevaFormaPago" runat="server" CssClass=" btn btn-xs btn-info"
                                Text="Nueva Forma Pago" CausesValidation="false" OnClick="btnNuevaFormaPago_Click" />
                        </div>
                    </div>
                    <div class=" widget-content">
                        <asp:GridView ID="dgvFormaPago" CssClass="table table-striped table-bordered table-hover dataTable no-footer"
                            runat="server" AutoGenerateColumns="False" OnRowCommand="dgvFormaPago_RowCommand">
                            <Columns>
                                <asp:BoundField HeaderText="Forma Pago" DataField="Descripcion" />
                                <asp:BoundField DataField="Monto" HeaderText="Monto" />
                                <asp:ButtonField CommandName="deleteFP" Text="&lt;i class='fa fa-times'&gt;&lt;/i&gt;">
                                    <ControlStyle CssClass="btn btn-xs btn-danger" />
                                </asp:ButtonField>
                            </Columns>
                        </asp:GridView>
                        <div>
                            Total<div class="input-group">
                                <span class="input-group-addon">$</span>
                                <asp:TextBox ID="txtTotalFP" Enabled="false" CssClass=" form-control " runat="server"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
                <div class=" widget">
                    <div class=" widget-header">
                        <h3>
                            <i class="fa fa-edit"></i>Comprobante
                        </h3>
                        <div class="btn-group widget-header-toolbar">
                            <asp:Button ID="btnAgregarComprobante" runat="server" CssClass=" btn btn-xs btn-info"
                                Text="Agregar Comprobante" CausesValidation="false" OnClick="btnAgregarComprobante_Click" />
                        </div>
                    </div>
                    <div class=" widget-content">
                        <asp:GridView ID="dgvComprobante" CssClass="table table-striped table-bordered table-hover dataTable no-footer"
                            runat="server" AutoGenerateColumns="False" OnRowCommand="dgvComprobante_RowCommand">
                            <Columns>
                                <asp:BoundField HeaderText="Tipo" DataField="TipoComprobante" />
                                <asp:BoundField DataField="Numero" HeaderText="Numero" />
                                <asp:BoundField DataField="Monto" HeaderText="Monto" />
                                <asp:ButtonField CommandName="deleteComp" Text="&lt;i class='fa fa-times'&gt;&lt;/i&gt;">
                                    <ControlStyle CssClass="btn btn-xs btn-danger" />
                                </asp:ButtonField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
            <div class=" col-lg-12">
                <asp:TextBox ID="txtDescripcion" CssClass="form-control" PlaceHolder="Ingrese si desea una descripcion para la compra"
                    runat="server" TextMode="MultiLine"></asp:TextBox></div>
            <div class=" col-lg-12">
                <asp:LinkButton ID="btnAgregarCompra" CssClass=" btn btn-success btn-block" runat="server"
                    OnClick="btnAgregarCompra_Click" CausesValidation="true" ValidationGroup="compra">Agregar Compra</asp:LinkButton>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <!-- Add Record Modal Starts here-->
    <div id="addModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="addModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                    ×</button>
                <h3 id="addModalLabel">
                    Nuevo Articulo</h3>
            </div>
            <asp:UpdatePanel ID="upAdd" runat="server">
                <ContentTemplate>
                    <div class="row">
                        <div class=" col-md-12">
                            Descripcion
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="NuevoArticulo"
                                ErrorMessage="Ingrese una descripcion" ForeColor="Red" ControlToValidate="txtDescripcionArtAgregar"></asp:RequiredFieldValidator>
                            <asp:TextBox ID="txtDescripcionArtAgregar" CssClass=" form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class=" col-md-12">
                            Codigo
                            <asp:TextBox ID="txtCodigo" CssClass=" form-control" runat="server"></asp:TextBox></div>
                        <div class=" col-md-12">
                            Marca
                            <asp:DropDownList ID="cbxMarca" runat="server" CssClass=" form-control">
                            </asp:DropDownList>
                        </div>
                        <div class=" col-md-12">
                            Tipo Articulo
                            <asp:DropDownList ID="cbxTipoArt" runat="server" CssClass=" form-control">
                            </asp:DropDownList>
                        </div>
                        <div class=" col-lg-12">
                            <uc1:CbxArticulosAgrupacion ID="ucCbxArticulosAgrupacion" runat="server" />
                        </div>
                        <div class=" col-md-12">
                            Costo Unitario
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ValidationGroup="NuevoArticulo"
                                ErrorMessage="Ingrese un costo" ForeColor="Red" ControlToValidate="txtCosto"></asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="Ingrese un costo valido"
                                Operator="DataTypeCheck" ForeColor="Red" ValidationGroup="NuevoArticulo" ControlToValidate="txtCosto"
                                Type="Double"></asp:CompareValidator>
                            <div class="input-group">
                                <span class="input-group-addon">$</span>
                                <asp:TextBox ID="txtCosto" CssClass=" form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class=" col-md-12">
                            Precio Venta
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ValidationGroup="NuevoArticulo"
                                ErrorMessage="Ingrese un precio" ForeColor="Red" ControlToValidate="txtPrecioVenta"></asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="CompareValidator2" ForeColor="Red" runat="server" ErrorMessage="Ingrese un Precio valido"
                                Operator="DataTypeCheck" ValidationGroup="NuevoArticulo" ControlToValidate="txtPrecioVenta"
                                Type="Double"></asp:CompareValidator>
                            <div class="input-group">
                                <span class="input-group-addon">$</span>
                                <asp:TextBox ID="txtPrecioVenta" CssClass=" form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class=" col-md-12">
                            Controlar Stock<asp:CheckBox ID="chbxControlarStock" CssClass=" form-control" runat="server"
                                AutoPostBack="True" OnCheckedChanged="chbxControlarStock_CheckedChanged" /></div>
                        <div class=" col-md-12">
                            Cantidad
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ValidationGroup="NuevoArticulo"
                                ErrorMessage="Ingrese una cantidad" ForeColor="Red" ControlToValidate="txtCantidad"></asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="CompareValidator3" ForeColor="Red" runat="server" ErrorMessage="Ingrese una cantidad valida"
                                Operator="DataTypeCheck" ValidationGroup="NuevoArticulo" ControlToValidate="txtCantidad"
                                Type="Integer"></asp:CompareValidator>
                            <asp:TextBox ID="txtCantidad" CssClass=" form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnAgregar" runat="server" Text="Agregar" CssClass="btn btn-success"
                            CausesValidation="True" ValidationGroup="NuevoArticulo" OnClick="btnAgregar_Click" />
                        <button class="btn btn-danger" data-dismiss="modal" aria-hidden="true">
                            Cancelar</button>
                    </div>
                </ContentTemplate>
                <%--<Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnAddRecord" EventName="Click" />
                </Triggers>--%>
            </asp:UpdatePanel>
        </div>
    </div>
    <!--Add Record Modal Ends here-->
    <!-- Busqueda Articulo Modal Starts here-->
    <div id="searchModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="addModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                    ×</button>
                <h3 id="H1">
                    Articulo</h3>
            </div>
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <div class=" col-lg-12">
                        Descripcion
                        <asp:TextBox ID="txtDescripcionArtBusqueda" Enabled="false" CssClass=" form-control"
                            runat="server"></asp:TextBox>
                        Codigo
                        <asp:TextBox ID="txtCodigoArtBusqueda" Enabled="false" CssClass=" form-control" runat="server"></asp:TextBox>
                        Marca
                        <asp:TextBox ID="txtMarcaArtBusqueda" Enabled="false" CssClass=" form-control" runat="server"></asp:TextBox>
                        Tipo Articulo
                        <asp:TextBox ID="txtTipoArtBusqueda" Enabled="false" CssClass=" form-control" runat="server"></asp:TextBox>
                        
                        Costo Unitario
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ValidationGroup="BusquedaArticulo"
                            ErrorMessage="Ingrese un costo" ForeColor="Red" ControlToValidate="txtCostoBusqueda"></asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="CompareValidator4" runat="server" ErrorMessage="Ingrese un costo valido"
                            Operator="DataTypeCheck" ForeColor="Red" ValidationGroup="BusquedaArticulo" ControlToValidate="txtCostoBusqueda"
                            Type="Double"></asp:CompareValidator>
                        <asp:TextBox ID="txtCostoBusqueda" CssClass=" form-control" runat="server"></asp:TextBox>
                        Precio Venta
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ValidationGroup="BusquedaArticulo"
                            ErrorMessage="Ingrese un precio" ForeColor="Red" ControlToValidate="txtPrecioBusqueda"></asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="CompareValidator5" ForeColor="Red" runat="server" ErrorMessage="Ingrese un Precio valido"
                            Operator="DataTypeCheck" ValidationGroup="BusquedaArticulo" ControlToValidate="txtPrecioBusqueda"
                            Type="Double"></asp:CompareValidator>
                        <asp:TextBox ID="txtPrecioBusqueda" CssClass=" form-control" runat="server"></asp:TextBox>
                        Controlar Stock<asp:CheckBox ID="chbControlarStockBusqueda" CssClass=" form-control"
                            runat="server" Checked="True" />
                        Cantidad
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ValidationGroup="BusquedaArticulo"
                            ErrorMessage="Ingrese una cantidad" ForeColor="Red" ControlToValidate="txtCantidadBusqueda"></asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="CompareValidator6" ForeColor="Red" runat="server" ErrorMessage="Ingrese una cantidad valida"
                            Operator="DataTypeCheck" ValidationGroup="NuevoArtBusquedaArticuloiculo" ControlToValidate="txtCantidadBusqueda"
                            Type="Integer"></asp:CompareValidator>
                        <asp:TextBox ID="txtCantidadBusqueda" CssClass=" form-control" runat="server"></asp:TextBox>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnAgregarArticuloExistente" runat="server" Text="Agregar" CssClass="btn btn-success"
                            CausesValidation="True" ValidationGroup="BusquedaArticulo" OnClick="btnAgregarArticuloExistente_Click" />
                        <button class="btn btn-danger" data-dismiss="modal" aria-hidden="true">
                            Cancelar</button>
                    </div>
                </ContentTemplate>
                <%--<Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnAddRecord" EventName="Click" />
                </Triggers>--%>
            </asp:UpdatePanel>
        </div>
    </div>
    <!-- Busqueda ArticuloModal Ends here-->
    <!-- confirmacion Modal Starts here-->
    <div id="confirmacionModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="addModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                    ×</button>
                <h3 id="H2">
                    <asp:Label ID="lblModalConfirmacion" runat="server"></asp:Label></h3>
            </div>
            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                <ContentTemplate>
                    <div class=" modal-body">
                        <div class=" row">
                            <div class=" col-lg-12">
                                <h4>
                                    Tipo Compra:
                                    <asp:Label ID="lblTipoCompra" runat="server" Text=""></asp:Label></h4>
                                <h4>
                                    Proveedor:
                                    <asp:Label ID="lblProveedor" runat="server" Text=""></asp:Label></h4>
                                <h4>
                                    Por un Total:$
                                    <asp:Label ID="lblTotal" runat="server" Text=""></asp:Label></h4>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnAgregarCompraFinal" runat="server" Text="Agregar" CssClass="btn btn-success"
                            CausesValidation="false" OnClick="btnAgregarCompraFinal_Click" />
                        <button class="btn btn-danger" data-dismiss="modal" aria-hidden="true">
                            Cancelar</button>
                    </div>
                </ContentTemplate>
                <%--<Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnAddRecord" EventName="Click" />
                </Triggers>--%>
            </asp:UpdatePanel>
        </div>
    </div>
    <!-- confirmacion modal Modal Ends here-->
    <!-- formaPago Modal Starts here-->
    <div id="formaPagoModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="addModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                    ×</button>
                <h3 id="H3">
                    Agregar Forma de Pago</h3>
            </div>
            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                <ContentTemplate>
                    <div class=" modal-body">
                        <div class=" row">
                            <div class=" col-lg-12">
                                Tipo Forma Pago<asp:DropDownList ID="cbxTipoFormaPago" CssClass=" form-control" runat="server">
                                </asp:DropDownList>
                                Monto
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ValidationGroup="fp"
                                    ErrorMessage="Ingrese un monto" ForeColor="Red" ControlToValidate="txtMontoFP"></asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="CompareValidator7" runat="server" ErrorMessage="Ingrese un monto valido"
                                    Operator="DataTypeCheck" ForeColor="Red" ValidationGroup="fp" ControlToValidate="txtMontoFP"
                                    Type="Double"></asp:CompareValidator>
                                <div class="input-group">
                                    <span class="input-group-addon">$</span>
                                    <asp:TextBox ID="txtMontoFP" CssClass=" form-control" runat="server"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnAgregarFormaPAgo" runat="server" Text="Agregar" CssClass="btn btn-success"
                            CausesValidation="true" ValidationGroup="fp" OnClick="btnAgregarFormaPAgo_Click" />
                        <button class="btn btn-danger" data-dismiss="modal" aria-hidden="true">
                            Cancelar</button>
                    </div>
                </ContentTemplate>
                <%--<Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnAddRecord" EventName="Click" />
                </Triggers>--%>
            </asp:UpdatePanel>
        </div>
    </div>
    <!-- forma pago modal Modal Ends here-->
    <!-- comprobante Modal Starts here-->
    <div id="comprobanteModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="addModalLabel"
        aria-hidden="true">
        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
            <ContentTemplate>
                <div class="modal-dialog modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                            ×</button>
                        <h3 id="H4">
                            Comprobante</h3>
                    </div>
                    <div class=" modal-body ">
                        <div class=" row">
                            <div class="col-md-12">
                                Fecha
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ForeColor=""
                                    CssClass="validador" SetFocusOnError="True" ControlToValidate="txtFechaComprobanteModal"
                                    Display="Dynamic" ErrorMessage="Obligatorio " ValidationGroup="comprobante"></asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="CompareValidator10" Operator="DataTypeCheck" ForeColor=""
                                    CssClass="validador" SetFocusOnError="True" runat="server" ErrorMessage="Ingrese una fecha"
                                    Type="Date" ControlToValidate="txtFechaComprobanteModal" Display="Dynamic" ValidationGroup="comprobante"></asp:CompareValidator>
                                <asp:TextBox ID="txtFechaComprobanteModal" runat="server" CssClass=" form-control datepicker"></asp:TextBox>
                            </div>
                            <div class="col-md-12">
                                Tipo Comprobante
                                <asp:CompareValidator ID="CompareValidator9" Operator="NotEqual" ForeColor="" CssClass="validador"
                                    SetFocusOnError="True" runat="server" ErrorMessage="Seleccione tipo comprobante"
                                    ValueToCompare="0" ControlToValidate="cbxTipoComprobanteModal" Display="Dynamic"
                                    ValidationGroup="comprobante"></asp:CompareValidator>
                                <asp:DropDownList ID="cbxTipoComprobanteModal" runat="server" CssClass=" form-control">
                                    <asp:ListItem Value="0">Seleccionar</asp:ListItem>
                                    <asp:ListItem>A</asp:ListItem>
                                    <asp:ListItem>B</asp:ListItem>
                                    <asp:ListItem>C</asp:ListItem>
                                    <asp:ListItem>X</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-12">
                                Numero Comprobante
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ForeColor=""
                                    CssClass="validador" SetFocusOnError="True" ControlToValidate="txtNumeroComprobanteModal"
                                    Display="Dynamic" ErrorMessage="Obligatorio " ValidationGroup="comprobante"></asp:RequiredFieldValidator>
                                <asp:TextBox ID="txtNumeroComprobanteModal" runat="server" CssClass=" form-control"></asp:TextBox>
                            </div>
                            <div class="col-md-12">
                                Monto
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ForeColor=""
                                    CssClass="validador" SetFocusOnError="True" ControlToValidate="txtMontoComprobanteModal"
                                    Display="Dynamic" ErrorMessage="Obligatorio " ValidationGroup="comprobante"></asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="CompareValidator8" Operator="DataTypeCheck" ForeColor=""
                                    CssClass="validador" SetFocusOnError="True" runat="server" ErrorMessage="Ingrese un monto"
                                    Type="Integer" ControlToValidate="txtMontoComprobanteModal" Display="Dynamic"
                                    ValidationGroup="comprobante"></asp:CompareValidator>
                                <asp:TextBox ID="txtMontoComprobanteModal" runat="server" CssClass=" form-control"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnAceptarComprobanteModal" runat="server" Text="Aceptar" CssClass="btn btn-success"
                            ValidationGroup="comprobante" CausesValidation="true" OnClick="btnAceptarComprobanteModal_Click" />
                        <button class="btn btn-danger" data-dismiss="modal" aria-hidden="true">
                            Cancelar</button>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <!--comprobante Modal Ends here-->
</asp:Content>
