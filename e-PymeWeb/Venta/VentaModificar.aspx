<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="VentaModificar.aspx.cs" Inherits="Venta_VentaModificar" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <script type="text/javascript">
        function ClientArticuloSelected(source, eventArgs) {
            //alert(" Key : " + eventArgs.get_text() + "  Value :  " + eventArgs.get_value());
            document.getElementById('<%=hfIdArticuloDescripcion.ClientID %>').value = eventArgs.get_value();
        }
    </script>
    <div class="main-header">
        <h2>
            Nueva Venta
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
            <div class=" col-md-8">
                <div class=" widget">
                    <div class=" widget-header">
                        <h3>
                            <i class="fa fa-edit"></i>Linea de Venta
                        </h3>
                    </div>
                    <div class=" widget-content">
                        <legend>Articulos Actuales de la Venta</legend>
                        <div class=" table-responsive">
                            <asp:GridView ID="dgvArticulos" runat="server" CssClass="table table-striped table-bordered table-hover dataTable no-footer"
                                AutoGenerateColumns="False" OnRowCommand="dgvArticulos_RowCommand">
                                <Columns>
                                    <asp:BoundField DataField="DescArticulo" HeaderText="Articulo" />
                                    <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" />
                                    <asp:BoundField DataField="PrecioUnitario" HeaderText="Precio Unitario" />
                                    <asp:BoundField DataField="Subtotal" HeaderText="SubTotal" />
                                    <asp:ButtonField CommandName="editArticulo" Text="&lt;i class='fa fa-pencil-square-o'&gt;&lt;/i&gt;">
                                        <ControlStyle CssClass="btn btn-xs btn-info" />
                                    </asp:ButtonField>
                                    <asp:ButtonField CommandName="deleteArticulo" Text="&lt;i class='fa fa-times'&gt;&lt;/i&gt;">
                                        <ControlStyle CssClass="btn btn-xs btn-info" />
                                    </asp:ButtonField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <hr class="inner-separator">
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
                                    DelimiterCharacters="" Enabled="True" MinimumPrefixLength="3" ServiceMethod="BuscarArticulosVenta"
                                    UseContextKey="true" ServicePath="~/Autocomplete.asmx" TargetControlID="txtDescArticulo"
                                    OnClientItemSelected="ClientArticuloSelected">
                                </asp:AutoCompleteExtender>
                                <asp:HiddenField ID="hfIdArticuloDescripcion" runat="server" />
                            </div>
                        </div>
                        <hr class="inner-separator">
                        <div class=" table-responsive">
                            <asp:GridView ID="dgvArticulosNuevos" runat="server" CssClass="table table-striped table-bordered table-hover dataTable no-footer"
                                AutoGenerateColumns="False" OnRowCommand="dgvArticulos_RowCommand">
                                <Columns>
                                    <asp:BoundField DataField="DescArticulo" HeaderText="Articulo" />
                                    <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" />
                                    <asp:BoundField DataField="PrecioUnitario" HeaderText="Precio Unitario" />
                                    <asp:BoundField DataField="Subtotal" HeaderText="SubTotal" />
                                    <asp:ButtonField CommandName="editArticulo" Text="&lt;i class='fa fa-pencil-square-o'&gt;&lt;/i&gt;">
                                        <ControlStyle CssClass="btn btn-xs btn-info" />
                                    </asp:ButtonField>
                                    <asp:ButtonField CommandName="deleteArticulo" Text="&lt;i class='fa fa-times'&gt;&lt;/i&gt;">
                                        <ControlStyle CssClass="btn btn-xs btn-info" />
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
                            <i class="fa fa-edit"></i>Forma de pago Actuales
                        </h3>
                    </div>
                    <div class=" widget-content">
                        <div class=" table-responsive">
                            <asp:GridView ID="dgvFormaPagoActuales" CssClass="table table-striped table-bordered table-hover dataTable no-footer"
                                runat="server" AutoGenerateColumns="False" OnRowCommand="dgvFormaPago_RowCommand">
                                <Columns>
                                    <asp:BoundField HeaderText="Forma Pago" DataField="Descripcion" />
                                    <asp:BoundField DataField="Monto" HeaderText="Monto" />
                                    <asp:ButtonField CommandName="deleteFP" Text="&lt;i class='fa fa-times'&gt;&lt;/i&gt;">
                                        <ControlStyle CssClass="btn btn-xs btn-info" />
                                    </asp:ButtonField>
                                </Columns>
                            </asp:GridView>
                            <div>
                                Total<div class="input-group">
                                    <span class="input-group-addon">$</span>
                                    <asp:TextBox ID="txtTotalFPActual" Enabled="false" CssClass=" form-control " runat="server"></asp:TextBox>
                                </div>
                            </div>
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
                                    <ControlStyle CssClass="btn btn-xs btn-info" />
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
            </div>
            <div class=" col-lg-12">
                <asp:TextBox ID="txtDescripcionVenta" runat="server" PlaceHolder="Ingrese una observacion si lo desea"
                    CssClass=" form-control" TextMode="MultiLine"></asp:TextBox>
            </div>
            <p>
            </p>
            <div class=" col-lg-12">
                <asp:Button ID="btnGuardarVenta" Text="Agregar Venta" runat="server" CssClass="btn btn-block btn-success"
                    OnClick="btnGuardarVenta_Click" /></div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <!-- articulo Record Modal Starts here-->
    <div id="articuloModal" class=" modal fade" tabindex="-1" role="dialog" aria-labelledby="addModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-content">
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                            ×</button>
                        <h3 id="H1">
                            Agregando Articulo</h3>
                    </div>
                    <div class=" modal-body">
                        <asp:HiddenField ID="hfClave" runat="server"></asp:HiddenField>
                        <div class=" col-lg-12">
                            Articulo
                            <asp:TextBox ID="txtArticuloModal" CssClass=" form-control" Enabled="false" runat="server"></asp:TextBox>
                            Precio
                            <asp:TextBox ID="txtPrecioModal" CssClass=" form-control" Enabled="false" runat="server"></asp:TextBox>
                            Cantidad
                            <asp:RequiredFieldValidator ControlToValidate="txtCantidadModal" ID="RequiredFieldValidator1"
                                runat="server" ErrorMessage="Ingrese una Cantidad" ForeColor="Red" Display="Dynamic"
                                ValidationGroup="cantidad"></asp:RequiredFieldValidator>
                            <asp:CompareValidator ControlToValidate="txtCantidadModal" runat="server" ID="compareValidator"
                                Display="Dynamic" ForeColor="Red" ErrorMessage="Ingrese una cantidad Valida"
                                ValidationGroup="cantidad" Type="Integer" Operator="DataTypeCheck"></asp:CompareValidator>
                            <asp:TextBox ID="txtCantidadModal" CssClass=" form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnAgregar" runat="server" Text="Agregar" CssClass="btn btn-success"
                            CausesValidation="true" ValidationGroup="cantidad" OnClick="btnAgregar_Click" />
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
    <!--articulo Record Modal Ends here-->
    <!-- formaPago Modal Starts here-->
    <div id="formaPagoModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="addModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-content">
            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                <ContentTemplate>
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                            ×</button>
                        <h3 id="H3">
                            Agregar Forma de Pago</h3>
                    </div>
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
            </asp:UpdatePanel>
        </div>
    </div>
    <!-- forma pago modal Modal Ends here-->
    <!-- confirmacion Modal Starts here-->
    <div id="confirmModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="addModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                    ×</button>
                <h3 id="H2">
                    ¿Desea Agregar la venta?</h3>
            </div>
            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                <ContentTemplate>
                    <div class=" modal-body">
                        <div class=" row">
                            <div class=" col-lg-12">
                                <h4>
                                    Por un Total de:$
                                    <asp:Label ID="lblTotal" runat="server" Text=""></asp:Label></h4>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnAgregarVentaFinal" runat="server" Text="Agregar" CssClass="btn btn-success"
                            CausesValidation="false" OnClick="btnAgregarVentaFinal_Click" />
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
</asp:Content>
