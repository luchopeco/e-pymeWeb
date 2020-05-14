<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="VentaNotaCredito.aspx.cs" Inherits="Venta_VentaNotaCredito" %>

<%@ Register Src="../Controles/PanelMensajes.ascx" TagName="PanelMensajes" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="main-header">
                <h2>
                    Generar Nota Credito
                </h2>
            </div>
            <div class=" col-lg-12">
                <uc1:PanelMensajes ID="ucPanelMensajes" runat="server" />
            </div>
            <div class="row">
                <div class=" col-md-6">
                    <div class=" widget">
                        <div class=" widget-header">
                            <h3>
                                <i class="fa fa-edit"></i>Venta
                            </h3>
                        </div>
                        <div class=" widget-content">
                            <div class=" table-responsive">
                                <asp:DetailsView ID="dvVentas" runat="server" CssClass=" table table-bordered table-condensed table-hover"
                                    AutoGenerateRows="False">
                                    <Fields>
                                        <asp:BoundField DataField="Fecha" HeaderText="Fecha" />
                                        <asp:BoundField DataField="Total" HeaderText="Total" />
                                        <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" />
                                    </Fields>
                                </asp:DetailsView>
                            </div>
                        </div>
                    </div>
                </div>
                <div class=" col-md-6">
                    <div class=" widget">
                        <div class=" widget-header">
                            <h3>
                                <i class="fa fa-edit"></i>Formas Pago
                            </h3>
                        </div>
                        <div class=" widget-content">
                            <asp:GridView ID="dgvFormaPago" CssClass=" table table-bordered table-condensed table-hover"
                                AutoGenerateColumns="False" runat="server">
                                <Columns>
                                    <asp:BoundField DataField="Descripcion" HeaderText="Forma Pago" />
                                    <asp:BoundField DataField="Monto" HeaderText="Monto" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class=" widget">
                        <div class=" widget-header">
                            <h3>
                                <i class="fa fa-edit"></i>Lineas de Venta
                            </h3>
                        </div>
                        <div class=" widget-content">
                            <div class=" table-responsive">
                                <asp:GridView ID="dgvLinaVenta" CssClass=" table table-bordered table-condensed table-hover"
                                    runat="server" AutoGenerateColumns="False" DataKeyNames="Idarticulo" OnRowCommand="dgvLinaVenta_RowCommand">
                                    <Columns>
                                        <asp:BoundField DataField="DescArticulo" HeaderText="Articulo" />
                                        <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" />
                                        <asp:BoundField DataField="PrecioUnitario" HeaderText="Precio" />
                                        <asp:BoundField DataField="Subtotal" HeaderText="Subtotal" />
                                        <asp:CheckBoxField DataField="Devuelto" HeaderText="Devuelto" />
                                        <asp:ButtonField CommandName="seleccionar" Text="&gt;&gt;">
                                            <ControlStyle CssClass="btn btn-xs btn-success" />
                                        </asp:ButtonField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class=" widget">
                        <div class=" widget-header">
                            <h3>
                                <i class="fa fa-edit"></i>Nota de Credito
                            </h3>
                        </div>
                        <div class=" widget-content">
                            <div class="row">
                                <div class=" col-md-12">
                                    <div class=" table-responsive">
                                        <asp:GridView ID="dgvNotaCredito" CssClass=" table table-bordered table-condensed table-hover"
                                            runat="server" AutoGenerateColumns="false" OnRowCommand="dgvNotaCredito_RowCommand">
                                            <Columns>
                                                <asp:BoundField DataField="DescArticulo" HeaderText="Articulo" />
                                                <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" />
                                                <asp:BoundField DataField="PrecioUnitario" HeaderText="Precio" />
                                                <asp:BoundField DataField="Subtotal" HeaderText="Subtotal" />
                                                <asp:ButtonField CommandName="quitar" Text="x">
                                                    <ControlStyle CssClass="btn btn-xs btn-danger" />
                                                </asp:ButtonField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    Fecha Vto
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ForeColor=""
                                        CssClass="validador" SetFocusOnError="True" ControlToValidate="txtFechaVto" Display="Dynamic"
                                        ErrorMessage="Obligatorio " ValidationGroup="nota"></asp:RequiredFieldValidator>
                                    <asp:CompareValidator ID="CompareValidator1" Operator="DataTypeCheck" ForeColor=""
                                        CssClass="validador" SetFocusOnError="True" runat="server" ErrorMessage="Ingrese una fecha"
                                        Type="Date" ControlToValidate="txtFechaVto" Display="Dynamic" ValidationGroup="nota"></asp:CompareValidator>
                                    <asp:TextBox ID="txtFechaVto" CssClass=" form-control datepicker" runat="server"></asp:TextBox>
                                </div>
                                <div class="col-md-6">
                                    Total
                                    <asp:TextBox ID="txtTotal" CssClass=" form-control" runat="server" Text="0" Enabled="false"></asp:TextBox>
                                </div>
                                <div class="col-md-12">
                                    Descripcion
                                    <asp:TextBox ID="txtDescripcionNotaCredito" CssClass=" form-control" runat="server"
                                        TextMode="MultiLine"></asp:TextBox>
                                </div>
                                <div class="col-md-12">
                                    <asp:LinkButton ID="btnGenerarNotaCredito" CausesValidation="true" ValidationGroup="nota"
                                        CssClass="btn btn-block btn-success" runat="server" 
                                        onclick="btnGenerarNotaCredito_Click">Generar Nota Credito</asp:LinkButton></div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="100">
                <ProgressTemplate>
                    <div style="position: fixed; top: 2%; left: 50%; z-index: 1051;">
                        <button class="btn btn-default btn-lg">
                            <i class="fa fa-spinner fa-spin"></i>Cargando....
                        </button>
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
