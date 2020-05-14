<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="InformesVentas.aspx.cs" Inherits="Venta_Informes_InformesVentas" EnableEventValidation="false" %>

<%@ Register Src="../../Controles/PanelMensajes.ascx" TagName="PanelMensajes" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <script src="../../js/datepickerPropio.js" type="text/javascript"></script>
    <div class="main-header">
        <h2>
            Informes de Ventas
        </h2>
        <asp:Label ID="lblSucursalTitulo" runat="server"></asp:Label>
    </div>
    <div class="row">
        <div class="col-md-12">
            <div class=" widget">
                <div class=" widget-header">
                    <h3>
                        <i class="fa fa-edit"></i>Informes
                    </h3>
                </div>
                <div class=" widget-content">
                    <ul class="nav nav-tabs" role="tablist">
                        <li class="active"><a href="#marca" role="tab" data-toggle="tab">Marcas</a> </li>
                        <li class=""><a href="#articulo" role="tab" data-toggle="tab">Articulos</a> </li>
                        <li class=""><a href="#tipoarticulo" role="tab" data-toggle="tab">Tipo Articulo</a></li>
                        <li class=""><a href="#venta" role="tab" data-toggle="tab">Venta</a> </li>
                    </ul>
                    <div class="tab-content">
                        <div class="tab-pane fade active in" id="marca">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="row">
                                        <div class=" col-md-12">
                                            <uc1:PanelMensajes ID="ucPanelMensajesMarcas" runat="server" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class=" col-md-12">
                                            <div class=" widget">
                                                <div class=" widget-header">
                                                    <h3>
                                                        Totales por Marcas
                                                    </h3>
                                                    <div class="btn-group widget-header-toolbar">
                                                        <asp:LinkButton ID="btnExoprtarAExcelMarcas" runat="server" ToolTip="Exportar a Planilla de Calculo"
                                                            OnClick="btnExoprtarAExcelMarcas_Click">
                                                            <h3><i class="fa fa-file-excel-o"></i></h3></asp:LinkButton>
                                                    </div>
                                                </div>
                                                <div class=" widget-content">
                                                    <div class="row">
                                                        <div class=" col-md-4">
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ForeColor=""
                                                                CssClass="validador" SetFocusOnError="True" ControlToValidate="txtFechaDesdeMarcas"
                                                                Display="Dynamic" ErrorMessage="Obligatorio " ValidationGroup="m"></asp:RequiredFieldValidator>
                                                            <asp:CompareValidator ID="CompareValidator1" Operator="DataTypeCheck" ForeColor=""
                                                                CssClass="validador" SetFocusOnError="True" runat="server" ErrorMessage="Ingrese una fecha"
                                                                Type="Date" ControlToValidate="txtFechaDesdeMarcas" Display="Dynamic" ValidationGroup="m"></asp:CompareValidator>
                                                            Fecha Desde<div class="input-group">
                                                                <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                                <asp:TextBox ID="txtFechaDesdeMarcas" CssClass=" form-control datepicker" runat="server"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class=" col-md-4">
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ForeColor=""
                                                                CssClass="validador" SetFocusOnError="True" ControlToValidate="txtFechaHastaMarcas"
                                                                Display="Dynamic" ErrorMessage="Obligatorio " ValidationGroup="m"></asp:RequiredFieldValidator>
                                                            <asp:CompareValidator ID="CompareValidator4" Operator="DataTypeCheck" ForeColor=""
                                                                CssClass="validador" SetFocusOnError="True" runat="server" ErrorMessage="Ingrese una fecha"
                                                                Type="Date" ControlToValidate="txtFechaHastaMarcas" Display="Dynamic" ValidationGroup="m"></asp:CompareValidator>
                                                            Fecha Hasta<div class="input-group">
                                                                <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                                <asp:TextBox ID="txtFechaHastaMarcas" CssClass=" form-control datepicker" runat="server"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class=" col-md-4">
                                                            .<asp:Button ID="btnBuscarMarcas" CssClass=" btn btn-success btn-block" runat="server"
                                                                Text="Buscar" OnClick="btnBuscarMarcas_Click" ValidationGroup="m" CausesValidation="true" />
                                                        </div>
                                                    </div>
                                                    <hr />
                                                    <div class=" row">
                                                        <div class=" col-md-12">
                                                            <div class=" table-responsive">
                                                                <asp:GridView AutoGenerateColumns="true" ID="dgvTotalesPorMarca" runat="server" CssClass=" table table-bordered table-condensed table-hover table">
                                                                    <Columns>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="btnExoprtarAExcelMarcas" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                        <div class="tab-pane fade" id="articulo">
                            <asp:UpdatePanel ID="UpdatePanelArticulos" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="row">
                                        <div class=" col-md-12">
                                            <uc1:PanelMensajes ID="ucPanelMensajesArticulo" runat="server" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class=" col-md-12">
                                            <div class=" widget">
                                                <div class=" widget-header">
                                                    <h3>
                                                        Totales por Articulos
                                                    </h3>
                                                    <div class="btn-group widget-header-toolbar">
                                                        <asp:LinkButton ID="btnExoprtarAExcelArticulos" runat="server" ToolTip="Exportar a Planilla de Calculo"
                                                            OnClick="btnExoprtarAExcelArticulos_Click">
                                                            <h3><i class="fa fa-file-excel-o"></i></h3></asp:LinkButton>
                                                    </div>
                                                </div>
                                                <div class=" widget-content">
                                                    <div class="row">
                                                        <div class=" col-md-4">
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ForeColor=""
                                                                CssClass="validador" SetFocusOnError="True" ControlToValidate="txtFechaDesdeArticulos"
                                                                Display="Dynamic" ErrorMessage="Obligatorio " ValidationGroup="art"></asp:RequiredFieldValidator>
                                                            <asp:CompareValidator ID="CompareValidator2" Operator="DataTypeCheck" ForeColor=""
                                                                CssClass="validador" SetFocusOnError="True" runat="server" ErrorMessage="Ingrese una fecha"
                                                                Type="Date" ControlToValidate="txtFechaDesdeArticulos" Display="Dynamic" ValidationGroup="art"></asp:CompareValidator>
                                                            Fecha Desde<div class="input-group">
                                                                <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                                <asp:TextBox ID="txtFechaDesdeArticulos" CssClass=" form-control datepicker" runat="server"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class=" col-md-4">
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ForeColor=""
                                                                CssClass="validador" SetFocusOnError="True" ControlToValidate="txtFechaHastaArticulos"
                                                                Display="Dynamic" ErrorMessage="Obligatorio " ValidationGroup="art"></asp:RequiredFieldValidator>
                                                            <asp:CompareValidator ID="CompareValidator3" Operator="DataTypeCheck" ForeColor=""
                                                                CssClass="validador" SetFocusOnError="True" runat="server" ErrorMessage="Ingrese una fecha"
                                                                Type="Date" ControlToValidate="txtFechaHastaArticulos" Display="Dynamic" ValidationGroup="art"></asp:CompareValidator>
                                                            Fecha Hasta<div class="input-group">
                                                                <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                                <asp:TextBox ID="txtFechaHastaArticulos" CssClass=" form-control datepicker" runat="server"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class=" col-md-4">
                                                            .<asp:Button ID="btnBuscarArticulos" CssClass=" btn btn-success btn-block" runat="server"
                                                                Text="Buscar" OnClick="btnBuscarArticulos_Click" ValidationGroup="art" CausesValidation="true" />
                                                        </div>
                                                    </div>
                                                    <hr />
                                                    <div class=" row">
                                                        <div class=" col-md-12">
                                                            <div class=" table-responsive">
                                                                <asp:GridView AutoGenerateColumns="true" ID="dgvTotalesPorArticulo" runat="server"
                                                                    CssClass=" table table-bordered table-condensed table-hover table">
                                                                </asp:GridView>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="btnExoprtarAExcelArticulos" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                        <div class="tab-pane fade" id="tipoarticulo">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="row">
                                        <div class=" col-md-12">
                                            <uc1:PanelMensajes ID="ucPanelMensajesTipoArticulo" runat="server" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class=" col-md-12">
                                            <div class=" widget">
                                                <div class=" widget-header">
                                                    <h3>
                                                        Totales por Tipo de Articulos
                                                    </h3>
                                                    <div class="btn-group widget-header-toolbar">
                                                        <asp:LinkButton ID="btnExportarAExcelTipoArticulo" runat="server" ToolTip="Exportar a Planilla de Calculo"
                                                            OnClick="btnExportarAExcelTipoArticulo_Click">
                                                            <h3><i class="fa fa-file-excel-o"></i></h3></asp:LinkButton>
                                                    </div>
                                                </div>
                                                <div class=" widget-content">
                                                    <div class="row">
                                                        <div class=" col-md-4">
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ForeColor=""
                                                                CssClass="validador" SetFocusOnError="True" ControlToValidate="txtFechaDesdeTipoArt"
                                                                Display="Dynamic" ErrorMessage="Obligatorio " ValidationGroup="ta"></asp:RequiredFieldValidator>
                                                            <asp:CompareValidator ID="CompareValidator5" Operator="DataTypeCheck" ForeColor=""
                                                                CssClass="validador" SetFocusOnError="True" runat="server" ErrorMessage="Ingrese una fecha"
                                                                Type="Date" ControlToValidate="txtFechaDesdeTipoArt" Display="Dynamic" ValidationGroup="ta"></asp:CompareValidator>
                                                            Fecha Desde<div class="input-group">
                                                                <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                                <asp:TextBox ID="txtFechaDesdeTipoArt" CssClass=" form-control datepicker" runat="server"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class=" col-md-4">
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ForeColor=""
                                                                CssClass="validador" SetFocusOnError="True" ControlToValidate="txtFechaHastaTipoArt"
                                                                Display="Dynamic" ErrorMessage="Obligatorio " ValidationGroup="ta"></asp:RequiredFieldValidator>
                                                            <asp:CompareValidator ID="CompareValidator6" Operator="DataTypeCheck" ForeColor=""
                                                                CssClass="validador" SetFocusOnError="True" runat="server" ErrorMessage="Ingrese una fecha"
                                                                Type="Date" ControlToValidate="txtFechaHastaTipoArt" Display="Dynamic" ValidationGroup="ta"></asp:CompareValidator>
                                                            Fecha Hasta<div class="input-group">
                                                                <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                                <asp:TextBox ID="txtFechaHastaTipoArt" CssClass=" form-control datepicker" runat="server"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class=" col-md-4">
                                                            .<asp:Button ID="btnBuscarVentasTipoArt" CssClass=" btn btn-success btn-block" runat="server"
                                                                Text="Buscar" ValidationGroup="ta" CausesValidation="true" OnClick="btnBuscarVentasTipoArt_Click" />
                                                        </div>
                                                    </div>
                                                    <hr />
                                                    <div class=" row">
                                                        <div class=" col-md-12">
                                                            <div class=" table-responsive">
                                                                <asp:GridView AutoGenerateColumns="true" ID="dgvTotalesTipoArt" runat="server" CssClass=" table table-bordered table-condensed table-hover table">
                                                                </asp:GridView>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="btnExportarAExcelTipoArticulo" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                        <div class="tab-pane fade" id="venta">
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="row">
                                        <div class=" col-md-12">
                                            <uc1:PanelMensajes ID="ucPanelMensajesVentas" runat="server" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class=" col-md-12">
                                            <div class=" widget">
                                                <div class=" widget-header">
                                                    <h3>
                                                        Totales por Forma de pago
                                                    </h3>
                                                    <div class="btn-group widget-header-toolbar">
                                                        <asp:LinkButton ID="btnExportarAExcelFormaPago" runat="server" ToolTip="Exportar a Planilla de Calculo"
                                                            OnClick="btnExportarAExcelFormaPago_Click">
                                                            <h3><i class="fa fa-file-excel-o"></i></h3></asp:LinkButton>
                                                    </div>
                                                </div>
                                                <div class=" widget-content">
                                                    <div class="row">
                                                        <div class=" col-md-4">
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ForeColor=""
                                                                CssClass="validador" SetFocusOnError="True" ControlToValidate="txtFechaDesdeFormaPago"
                                                                Display="Dynamic" ErrorMessage="Obligatorio " ValidationGroup="fp"></asp:RequiredFieldValidator>
                                                            <asp:CompareValidator ID="CompareValidator7" Operator="DataTypeCheck" ForeColor=""
                                                                CssClass="validador" SetFocusOnError="True" runat="server" ErrorMessage="Ingrese una fecha"
                                                                Type="Date" ControlToValidate="txtFechaDesdeFormaPago" Display="Dynamic" ValidationGroup="fp"></asp:CompareValidator>
                                                            Fecha Desde<div class="input-group">
                                                                <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                                <asp:TextBox ID="txtFechaDesdeFormaPago" CssClass=" form-control datepicker" runat="server"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class=" col-md-4">
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ForeColor=""
                                                                CssClass="validador" SetFocusOnError="True" ControlToValidate="txtFechaHastaFormaPago"
                                                                Display="Dynamic" ErrorMessage="Obligatorio " ValidationGroup="fp"></asp:RequiredFieldValidator>
                                                            <asp:CompareValidator ID="CompareValidator8" Operator="DataTypeCheck" ForeColor=""
                                                                CssClass="validador" SetFocusOnError="True" runat="server" ErrorMessage="Ingrese una fecha"
                                                                Type="Date" ControlToValidate="txtFechaHastaFormaPago" Display="Dynamic" ValidationGroup="fp"></asp:CompareValidator>
                                                            Fecha Hasta<div class="input-group">
                                                                <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                                <asp:TextBox ID="txtFechaHastaFormaPago" CssClass=" form-control datepicker" runat="server"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class=" col-md-4">
                                                            .<asp:Button ID="btnBuscarFormasPago" CssClass=" btn btn-success btn-block" runat="server"
                                                                Text="Buscar" ValidationGroup="fp" CausesValidation="true" OnClick="btnBuscarFormasPago_Click" />
                                                        </div>
                                                    </div>
                                                    <hr />
                                                    <div class=" row">
                                                        <div class=" col-md-12">
                                                            <div class=" table-responsive">
                                                                <asp:GridView AutoGenerateColumns="true" ID="dgvFormaPago" runat="server" CssClass=" table table-bordered table-condensed table-hover table">
                                                                </asp:GridView>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="btnExportarAExcelFormaPago" />
                                </Triggers>
                            </asp:UpdatePanel>
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
</asp:Content>
