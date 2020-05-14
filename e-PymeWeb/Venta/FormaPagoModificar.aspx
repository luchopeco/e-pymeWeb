<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="FormaPagoModificar.aspx.cs" Inherits="Venta_FormaPagoModificar" %>

<%@ Register Src="../Controles/PanelMensajes.ascx" TagName="PanelMensajes" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="main-header">
                <h2>
                    Editar Forma De Pago
                </h2>
            </div>
            <div class=" col-lg-12">
                <uc1:PanelMensajes ID="ucPanelMensajes" runat="server" />
            </div>
            <div class="row">
                <div class=" col-md-5">
                    <div class=" widget">
                        <div class=" widget-header">
                            <h3>
                                <i class="fa fa-edit"></i>Formas de pago
                            </h3>
                            <div class="btn-group widget-header-toolbar">
                                <asp:Button ID="btnNuevaFormaPago" runat="server" CssClass=" btn btn-xs btn-info"
                                    Text="Nueva Forma Pago" CausesValidation="false" OnClick="btnNuevaFormaPago_Click" />
                            </div>
                        </div>
                        <div class=" widget-content">
                            <div class=" table-responsive">
                                <asp:GridView ID="dgvFormaPago" CssClass="table table-striped table-bordered table-hover dataTable no-footer"
                                    runat="server" AutoGenerateColumns="False" DataKeyNames="IdtipoFormaPago" OnRowCommand="dgvFormaPago_RowCommand">
                                    <Columns>
                                        <asp:BoundField HeaderText="Forma Pago" DataField="Descripcion" />
                                        <asp:BoundField DataField="Monto" HeaderText="Monto" />
                                        <asp:ButtonField CommandName="deleteFP" Text="&lt;i class='fa fa-times'&gt;&lt;/i&gt;">
                                            <ControlStyle CssClass="btn btn-xs btn-danger" />
                                        </asp:ButtonField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                        <div class=" widget-footer">
                            <asp:Label ID="lblTotal" runat="server"></asp:Label>
                        </div>
                    </div>
                </div>
                <div class=" col-md-7">
                    <div class=" widget">
                        <div class=" widget-header">
                            <h3>
                                <i class="fa fa-edit"></i>Venta
                            </h3>
                        </div>
                        <div class=" widget-content">
                            <div class=" table-responsive">
                                <asp:DetailsView ID="dvDetalleVenta" runat="server" CssClass="table table-bordered table-condensed table-hover"
                                    AutoGenerateRows="false">
                                    <Fields>
                                        <asp:BoundField DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy}" />
                                        <asp:BoundField DataField="Total" HeaderText="Total" />
                                        <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" />
                                    </Fields>
                                </asp:DetailsView>
                            </div>
                        </div>
                    </div>
                    <hr />
                    <div class=" panel panel-default">
                        <div class=" panel-heading">
                            Linea De Venta</div>
                        <div class=" panel-body">
                            <div class=" table-responsive">
                                <asp:GridView ID="dgvArticulos" runat="server" CssClass="table  table-bordered table-striped table-hover no-footer"
                                    AutoGenerateColumns="False" DataKeyNames="Idarticulo">
                                    <Columns>
                                        <asp:BoundField DataField="DescArticulo" HeaderText="Articulo" />
                                        <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" />
                                        <asp:BoundField DataField="PrecioUnitario" HeaderText="Precio" />
                                        <asp:BoundField DataField="Subtotal" HeaderText="SubTotal" />
                                        <asp:CheckBoxField DataField="Devuelto" HeaderText="Devuelto">
                                            <ControlStyle CssClass="form-control" />
                                        </asp:CheckBoxField>
                                        <asp:CheckBoxField DataField="DadoBaja" HeaderText="Anulado">
                                            <ControlStyle CssClass="form-control" />
                                        </asp:CheckBoxField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <asp:Button ID="btnAceptarFinal" runat="server" Text="Aceptar" CssClass="btn btn-success btn-block"
                        CausesValidation="true" OnClick="btnAceptarFinal_Click" />
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
                        <uc1:PanelMensajes ID="ucPanelMensajesFormaPago" runat="server" />
                        <div class=" row">
                            <div class=" col-lg-12">
                                Tipo Forma Pago<asp:DropDownList ID="cbxTipoFormaPago" CssClass=" form-control" runat="server"
                                    AutoPostBack="True" OnSelectedIndexChanged="cbxTipoFormaPago_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-12">
                                <asp:Panel ID="panelNotaCredito" runat="server">
                                    Numero Nota Credito
                                    <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="Ingrese un monto valido"
                                        Operator="DataTypeCheck" ForeColor="Red" ValidationGroup="nc" ControlToValidate="txtNroNotaCredito"
                                        Type="Double"></asp:CompareValidator>
                                    <div class="form-group input-group">
                                        <span class="input-group-btn">
                                            <asp:LinkButton ID="btnResetarNotaCredito" CausesValidation="false" ToolTip="Limpiar nota credito"
                                                class="btn btn-warning" runat="server" OnClick="btnResetarNotaCredito_Click"><i class="fa fa-power-off"></i></asp:LinkButton>
                                        </span>
                                        <asp:TextBox ID="txtNroNotaCredito" CssClass=" form-control" runat="server"></asp:TextBox>
                                        <span class="input-group-btn">
                                            <asp:LinkButton ID="btnBuscarNroCredito" CausesValidation="true" ValidationGroup="nc"
                                                class="btn btn-success" ToolTip=" buscar " runat="server" OnClick="btnBuscarNroCredito_Click">
                                    <i class="fa fa-search">
</i></asp:LinkButton>
                                        </span>
                                    </div>
                                </asp:Panel>
                            </div>
                            <div class=" col-md-12">
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
    <!-- Nuevo Cliente Modal Starts here-->
    <div id="confirmarModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="addModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-content">
            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                <ContentTemplate>
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                            ×</button>
                    </div>
                    <div class=" modal-body ">
                        ¿Desea Modificar las formas de pago de la venta?
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnAceptarModalFinal" CausesValidation="true" ValidationGroup="ingreso"
                            runat="server" Text="Aceptar" CssClass="btn btn-success" 
                            onclick="btnAceptarModalFinal_Click"  />
                        <button class="btn btn-danger" data-dismiss="modal" aria-hidden="true">
                            Cancelar</button>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <!--Nuevo Cliente Modal Ends here-->
</asp:Content>
