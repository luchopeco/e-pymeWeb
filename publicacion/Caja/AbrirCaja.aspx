<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="AbrirCaja.aspx.cs" Inherits="Caja_AbrirCaja" %>

<%@ Register Src="../Controles/PanelMensajes.ascx" TagName="PanelMensajes" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="main-header">
        <h2>
            Abrir Caja
        </h2>
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <uc1:PanelMensajes ID="ucPanelMensajes" runat="server" />
            <div class="row">
                <div class="col-md-12">
                    <div class=" widget">
                        <div class=" widget-header">
                            <h3>
                                Abrir Caja
                            </h3>
                        </div>
                        <div class=" widget-content">
                            <div class="row">
                                <div class="col-md-12">
                                    Descripcion
                                    <asp:TextBox ID="txtDescripcion" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    Fondo Inicial
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ForeColor=""
                                        CssClass="validador" SetFocusOnError="True" ControlToValidate="txtFondoInicial"
                                        Display="Dynamic" ErrorMessage="Obligatorio " ValidationGroup="caja"></asp:RequiredFieldValidator>
                                    <asp:CompareValidator ID="CompareValidator1" Operator="DataTypeCheck" ForeColor=""
                                        CssClass="validador" SetFocusOnError="True" runat="server" ErrorMessage="Ingrese un importe"
                                        Type="Double" ControlToValidate="txtFondoInicial" Display="Dynamic" ValidationGroup="caja"></asp:CompareValidator>
                                    <div class="form-group input-group">
                                        <span class="input-group-addon">$ </span>
                                        <asp:TextBox ID="txtFondoInicial" runat="server" CssClass=" form-control"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <hr />
                                    <asp:Button ID="btnAceptar" runat="server" Text="Abrir" CssClass="btn btn-block btn-success"
                                        CausesValidation="true" ValidationGroup="caja" OnClick="btnAceptar_Click" />
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
