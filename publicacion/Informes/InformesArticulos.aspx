<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="InformesArticulos.aspx.cs" Inherits="Informes_InformesArticulos" %>

<%@ Register Src="../Controles/PanelMensajes.ascx" TagName="PanelMensajes" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <script src="../../js/datepickerPropio.js" type="text/javascript"></script>
    <div class="main-header">
        <h2>Informes de Articulos
        </h2>
        <asp:Label ID="lblSucursalEnTitulo" runat="server"></asp:Label>
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
                        <li class="active"><a href="#pocostock" role="tab" data-toggle="tab">Articulos Poco Stock</a></li>
                        <li class=""><a href="#mucoStock" role="tab" data-toggle="tab">Articulos Mucho stock</a> </li>
                        <li class=""><a href="#tipoArticuloPocoStock" role="tab" data-toggle="tab">Tipo Articulo Poco stock</a> </li>
                        <li class=""><a href="#tipoArticuloMuchoStock" role="tab" data-toggle="tab">Tipo Articulo Mucho stock</a> </li>
                    </ul>
                    <div class="tab-content">
                        <div class="tab-pane fade active in" id="pocostock">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="row">
                                        <div class=" col-md-12">
                                            <uc1:PanelMensajes ID="ucPanelMensajesPocoSotck" runat="server" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class=" col-md-12">
                                            <div class=" widget">
                                                <div class=" widget-header">
                                                    <h3>Articulos con poco stock
                                                    </h3>
                                                    <div class="btn-group widget-header-toolbar">
                                                        <asp:LinkButton ID="btnExoprtarAExcelPocoSotck" runat="server" ToolTip="Exportar a Planilla de Calculo"
                                                            OnClick="btnExoprtarAExcelPocoSotck_Click">
                                                            <h3><i class="fa fa-file-excel-o"></i></h3></asp:LinkButton>
                                                    </div>
                                                </div>
                                                <div class=" widget-content">
                                                    <div class="row">
                                                        <div class=" col-md-4">
                                                            Articulos con stock <= a
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ForeColor=""
                                                                CssClass="validador" SetFocusOnError="True" ControlToValidate="txtCantidadMenorA"
                                                                Display="Dynamic" ErrorMessage="Obligatorio " ValidationGroup="1"></asp:RequiredFieldValidator>
                                                            <asp:CompareValidator ID="CompareValidator1" Operator="DataTypeCheck" ForeColor=""
                                                                CssClass="validador" SetFocusOnError="True" runat="server" ErrorMessage="Ingrese un numero"
                                                                Type="Integer" ControlToValidate="txtCantidadMenorA" Display="Dynamic" ValidationGroup="1"></asp:CompareValidator>
                                                            <asp:TextBox ID="txtCantidadMenorA" runat="server" CssClass=" form-control" Text="5"></asp:TextBox>
                                                        </div>
                                                        <div class=" col-md-4">
                                                            Cantidad a mostrar
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ForeColor=""
                                                                CssClass="validador" SetFocusOnError="True" ControlToValidate="txtArticulosAmostrarPocoStock"
                                                                Display="Dynamic" ErrorMessage="Obligatorio " ValidationGroup="1"></asp:RequiredFieldValidator>
                                                            <asp:CompareValidator ID="CompareValidator2" Operator="DataTypeCheck" ForeColor=""
                                                                CssClass="validador" SetFocusOnError="True" runat="server" ErrorMessage="Ingrese un numero"
                                                                Type="Integer" ControlToValidate="txtArticulosAmostrarPocoStock" Display="Dynamic"
                                                                ValidationGroup="1"></asp:CompareValidator>
                                                            <asp:TextBox ID="txtArticulosAmostrarPocoStock" runat="server" CssClass=" form-control"
                                                                Text="10"></asp:TextBox>
                                                        </div>
                                                        <div class=" col-md-4">
                                                            .<asp:Button ID="btnBuscarArticulosPocoStock" CssClass=" btn btn-success btn-block"
                                                                runat="server" Text="Buscar" ValidationGroup="1" CausesValidation="true" OnClick="btnBuscarArticulosPocoStock_Click" />
                                                        </div>
                                                    </div>
                                                    <hr />
                                                    <div class=" row">
                                                        <div class=" col-md-12">
                                                            <div class=" table-responsive">
                                                                <asp:GridView AutoGenerateColumns="true" ID="dgvArticulosPocoStock" runat="server"
                                                                    CssClass=" table table-bordered table-condensed table-hover table">
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
                                    <asp:PostBackTrigger ControlID="btnExoprtarAExcelPocoSotck" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                        <div class="tab-pane fade" id="mucoStock">
                            <asp:UpdatePanel ID="UpdatePanelArticulos" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="row">
                                        <div class=" col-md-12">
                                            <uc1:PanelMensajes ID="ucPanelMensajesArticuloMuchoStock" runat="server" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class=" col-md-12">
                                            <div class=" widget">
                                                <div class=" widget-header">
                                                    <h3>Articulos con mucho Stock
                                                    </h3>
                                                    <div class="btn-group widget-header-toolbar">
                                                        <asp:LinkButton ID="btnExoprtarAExcelArticulosMuchoStock" runat="server" ToolTip="Exportar a Planilla de Calculo"
                                                            OnClick="btnExoprtarAExcelArticulosMuchoStock_Click">
                                                            <h3><i class="fa fa-file-excel-o"></i></h3></asp:LinkButton>
                                                    </div>
                                                </div>
                                                <div class=" widget-content">
                                                    <div class="row">
                                                        <div class=" col-md-4">
                                                            Articulos con stock mayor a
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ForeColor=""
                                                                CssClass="validador" SetFocusOnError="True" ControlToValidate="txtArticulosConStockMayorA"
                                                                Display="Dynamic" ErrorMessage="Obligatorio " ValidationGroup="2"></asp:RequiredFieldValidator>
                                                            <asp:CompareValidator ID="CompareValidator3" Operator="DataTypeCheck" ForeColor=""
                                                                CssClass="validador" SetFocusOnError="True" runat="server" ErrorMessage="Ingrese un numero"
                                                                Type="Integer" ControlToValidate="txtArticulosConStockMayorA" Display="Dynamic"
                                                                ValidationGroup="2"></asp:CompareValidator>
                                                            <asp:TextBox ID="txtArticulosConStockMayorA" runat="server" CssClass=" form-control"
                                                                Text="10"></asp:TextBox>
                                                        </div>
                                                        <div class=" col-md-4">
                                                            Cantidad articulos a mostrar
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ForeColor=""
                                                                CssClass="validador" SetFocusOnError="True" ControlToValidate="txtCantidadArticulosAMostrarMuchoStock"
                                                                Display="Dynamic" ErrorMessage="Obligatorio " ValidationGroup="2"></asp:RequiredFieldValidator>
                                                            <asp:CompareValidator ID="CompareValidator4" Operator="DataTypeCheck" ForeColor=""
                                                                CssClass="validador" SetFocusOnError="True" runat="server" ErrorMessage="Ingrese un numero"
                                                                Type="Integer" ControlToValidate="txtCantidadArticulosAMostrarMuchoStock" Display="Dynamic"
                                                                ValidationGroup="2"></asp:CompareValidator>
                                                            <asp:TextBox ID="txtCantidadArticulosAMostrarMuchoStock" runat="server" CssClass=" form-control"
                                                                Text="10"></asp:TextBox>
                                                        </div>
                                                        <div class=" col-md-4">
                                                            .<asp:Button ID="btnBuscarArticulosMuchoStock" CssClass=" btn btn-success btn-block"
                                                                runat="server" Text="Buscar" ValidationGroup="2" CausesValidation="true" OnClick="btnBuscarArticulosMuchoStock_Click" />
                                                        </div>
                                                    </div>
                                                    <hr />
                                                    <div class=" row">
                                                        <div class=" col-md-12">
                                                            <div class=" table-responsive">
                                                                <asp:GridView AutoGenerateColumns="true" ID="dgvArticulosMuchoStock" runat="server"
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
                                    <asp:PostBackTrigger ControlID="btnExoprtarAExcelArticulosMuchoStock" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                        <div class="tab-pane fade" id="tipoArticuloPocoStock">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="row">
                                        <div class=" col-md-12">
                                            <uc1:PanelMensajes ID="ucPanelMensajesTipoArticuloPocoSotck" runat="server" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class=" col-md-12">
                                            <div class=" widget">
                                                <div class=" widget-header">
                                                    <h3>Tipos Articulos con poco Stock
                                                    </h3>
                                                    <div class="btn-group widget-header-toolbar">
                                                        <asp:LinkButton ID="btnExoprtarAExcelTipoArticuloPocoSotck" runat="server" ToolTip="Exportar a Planilla de Calculo"
                                                            OnClick="btnExoprtarAExcelTipoArticuloPocoSotck_Click">
                                                            <h3><i class="fa fa-file-excel-o"></i></h3></asp:LinkButton>
                                                    </div>
                                                </div>
                                                <div class=" widget-content">
                                                    <div class="row">
                                                        <div class=" col-md-4">
                                                            Tipos Articulos con stock <= a
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ForeColor=""
                                                                CssClass="validador" SetFocusOnError="True" ControlToValidate="txtTipoArticuloCantidadMenorA"
                                                                Display="Dynamic" ErrorMessage="Obligatorio " ValidationGroup="1"></asp:RequiredFieldValidator>
                                                            <asp:CompareValidator ID="CompareValidator5" Operator="DataTypeCheck" ForeColor=""
                                                                CssClass="validador" SetFocusOnError="True" runat="server" ErrorMessage="Ingrese un numero"
                                                                Type="Integer" ControlToValidate="txtTipoArticuloCantidadMenorA" Display="Dynamic" ValidationGroup="1"></asp:CompareValidator>
                                                            <asp:TextBox ID="txtTipoArticuloCantidadMenorA" runat="server" CssClass=" form-control" Text="5"></asp:TextBox>
                                                        </div>
                                                        <div class=" col-md-4">
                                                            Cantidad a mostrar
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ForeColor=""
                                                                CssClass="validador" SetFocusOnError="True" ControlToValidate="txtTipoArticulosAmostrarPocoStock"
                                                                Display="Dynamic" ErrorMessage="Obligatorio " ValidationGroup="1"></asp:RequiredFieldValidator>
                                                            <asp:CompareValidator ID="CompareValidator6" Operator="DataTypeCheck" ForeColor=""
                                                                CssClass="validador" SetFocusOnError="True" runat="server" ErrorMessage="Ingrese un numero"
                                                                Type="Integer" ControlToValidate="txtTipoArticulosAmostrarPocoStock" Display="Dynamic"
                                                                ValidationGroup="1"></asp:CompareValidator>
                                                            <asp:TextBox ID="txtTipoArticulosAmostrarPocoStock" runat="server" CssClass=" form-control"
                                                                Text="10"></asp:TextBox>
                                                        </div>
                                                        <div class=" col-md-4">
                                                            .<asp:Button ID="btnBuscarTiposArticulosPocoStock" CssClass=" btn btn-success btn-block"
                                                                runat="server" Text="Buscar" ValidationGroup="1" CausesValidation="true" OnClick="btnBuscarTiposArticulosPocoStock_Click" />
                                                        </div>
                                                    </div>
                                                    <hr />
                                                    <div class=" row">
                                                        <div class=" col-md-12">
                                                            <div class=" table-responsive">
                                                                <asp:GridView AutoGenerateColumns="true" ID="dgvTipoArticulosPocoStock" runat="server"
                                                                    CssClass=" table table-bordered table-condensed table-hover table">
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
                                    <asp:PostBackTrigger ControlID="btnExoprtarAExcelTipoArticuloPocoSotck" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                        <div class="tab-pane fade" id="tipoArticuloMuchoStock">
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="row">
                                        <div class=" col-md-12">
                                            <uc1:PanelMensajes ID="ucPanelMensajesTipoArticuloMuchoStock" runat="server" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class=" col-md-12">
                                            <div class=" widget">
                                                <div class=" widget-header">
                                                    <h3>Tipos Articulos con mucho Stock
                                                    </h3>
                                                    <div class="btn-group widget-header-toolbar">
                                                        <asp:LinkButton ID="btnExoprtarAExcelTiposArticulosMuchoStock" runat="server" ToolTip="Exportar a Planilla de Calculo"
                                                            OnClick="btnExoprtarAExcelTiposArticulosMuchoStock_Click">
                                                            <h3><i class="fa fa-file-excel-o"></i></h3></asp:LinkButton>
                                                    </div>
                                                </div>
                                                <div class=" widget-content">
                                                    <div class="row">
                                                        <div class=" col-md-4">
                                                            Tipos Articulos con stock mayor a
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ForeColor=""
                                                                CssClass="validador" SetFocusOnError="True" ControlToValidate="txtTiposArticulosConStockMayorA"
                                                                Display="Dynamic" ErrorMessage="Obligatorio " ValidationGroup="2"></asp:RequiredFieldValidator>
                                                            <asp:CompareValidator ID="CompareValidator7" Operator="DataTypeCheck" ForeColor=""
                                                                CssClass="validador" SetFocusOnError="True" runat="server" ErrorMessage="Ingrese un numero"
                                                                Type="Integer" ControlToValidate="txtTiposArticulosConStockMayorA" Display="Dynamic"
                                                                ValidationGroup="2"></asp:CompareValidator>
                                                            <asp:TextBox ID="txtTiposArticulosConStockMayorA" runat="server" CssClass=" form-control"
                                                                Text="10"></asp:TextBox>
                                                        </div>
                                                        <div class=" col-md-4">
                                                            Cantidad articulos a mostrar
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ForeColor=""
                                                                CssClass="validador" SetFocusOnError="True" ControlToValidate="txtCantidadTiposArticulosAMostrarMuchoStock"
                                                                Display="Dynamic" ErrorMessage="Obligatorio " ValidationGroup="2"></asp:RequiredFieldValidator>
                                                            <asp:CompareValidator ID="CompareValidator8" Operator="DataTypeCheck" ForeColor=""
                                                                CssClass="validador" SetFocusOnError="True" runat="server" ErrorMessage="Ingrese un numero"
                                                                Type="Integer" ControlToValidate="txtCantidadTiposArticulosAMostrarMuchoStock" Display="Dynamic"
                                                                ValidationGroup="2"></asp:CompareValidator>
                                                            <asp:TextBox ID="txtCantidadTiposArticulosAMostrarMuchoStock" runat="server" CssClass=" form-control"
                                                                Text="10"></asp:TextBox>
                                                        </div>
                                                        <div class=" col-md-4">
                                                            .<asp:Button ID="btnBuscarTiposArticulosMuchoStock" CssClass=" btn btn-success btn-block"
                                                                runat="server" Text="Buscar" ValidationGroup="2" CausesValidation="true" OnClick="btnBuscarTiposArticulosMuchoStock_Click" />
                                                        </div>
                                                    </div>
                                                    <hr />
                                                    <div class=" row">
                                                        <div class=" col-md-12">
                                                            <div class=" table-responsive">
                                                                <asp:GridView AutoGenerateColumns="true" ID="dgvTiposArticulosMuchoStock" runat="server"
                                                                    CssClass=" table table-bordered table-condensed table-hover table">
                                                                </asp:GridView>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="btnExoprtarAExcelTiposArticulosMuchoStock" />
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
