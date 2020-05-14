<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="NotasCreditosGestion.aspx.cs" Inherits="Venta_NotasCreditosGestion" %>

<%@ Register Src="../Controles/PanelMensajes.ascx" TagName="PanelMensajes" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <script src="../js/datepickerPropio.js" type="text/javascript"></script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="main-header">
                <h2>
                    Gestion Notas Credito
                </h2>
            </div>
            <div class=" col-lg-12">
                <uc1:PanelMensajes ID="ucPanelMensajes" runat="server" />
            </div>
            <div class="row">
                <div class=" col-md-4">
                    Fecha Desde
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ForeColor=""
                        CssClass="validador" SetFocusOnError="True" ControlToValidate="txtFechaDesde"
                        Display="Dynamic" ErrorMessage="Obligatorio " ValidationGroup="fecha"></asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="CompareValidator1" Operator="DataTypeCheck" ForeColor=""
                        CssClass="validador" SetFocusOnError="True" runat="server" ErrorMessage="Ingrese una fecha"
                        Type="Date" ControlToValidate="txtFechaDesde" Display="Dynamic" ValidationGroup="fecha"></asp:CompareValidator>
                    <div class="input-group">
                        <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                        <asp:TextBox ID="txtFechaDesde" CssClass=" form-control datepicker" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class=" col-md-4">
                    Fecha Hasta
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ForeColor=""
                        CssClass="validador" SetFocusOnError="True" ControlToValidate="txtFechaHasta"
                        Display="Dynamic" ErrorMessage="Obligatorio " ValidationGroup="fecha"></asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="CompareValidator2" Operator="DataTypeCheck" ForeColor=""
                        CssClass="validador" SetFocusOnError="True" runat="server" ErrorMessage="Ingrese una fecha"
                        Type="Date" ControlToValidate="txtFechaHasta" Display="Dynamic" ValidationGroup="fecha"></asp:CompareValidator>
                    <div class="input-group">
                        <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                        <asp:TextBox ID="txtFechaHasta" CssClass=" form-control datepicker" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class=" col-md-4">
                    .<asp:Button ID="btnBuscarVentas" ValidationGroup="fecha" CausesValidation="true"
                        CssClass=" btn btn-success btn-block" runat="server" Text="Buscar" OnClick="btnBuscarVentas_Click" />
                </div>
            </div>
            <hr />
            <div class="row">
                <div class="col-md-12">
                    <div class=" widget">
                        <div class=" widget-header">
                            <h3>
                                <i class="fa fa-edit"></i>Notas de credito
                            </h3>
                        </div>
                        <div class=" widget-content">
                            <div class=" row">
                                <div class=" col-md-12">
                                    <div class=" table-responsive">
                                        <asp:GridView ID="dgvNotaCredito" AutoGenerateColumns="False" CssClass=" table table-bordered table-condensed table-hover"
                                            runat="server">
                                            <Columns>
                                                <asp:BoundField DataField="Numero" HeaderText="Numero" />
                                                <asp:BoundField DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy}" />
                                                <asp:BoundField DataField="Monto" HeaderText="Monto" />
                                                <asp:BoundField DataField="FechaVto" HeaderText="Vencimiento" DataFormatString="{0:dd/MM/yyyy}" />
                                                <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" />
                                                <asp:CheckBoxField DataField="UtilizadaEnVenta" HeaderText="Utilizada" />
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
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
