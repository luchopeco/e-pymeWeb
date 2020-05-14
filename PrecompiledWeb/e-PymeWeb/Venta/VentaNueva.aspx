<%@ page title="" language="C#" masterpagefile="~/Site.master" autoeventwireup="true" inherits="Venta_VentaNueva, App_Web_4vyxtacj" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
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
                        <legend>Busqueda de articulos</legend>
                        <div class="input-group">
                            <asp:TextBox ID="txtArticuloCodigo" PlaceHolder="Ingrese el codigo completo del articulo"
                                class="form-control" runat="server"></asp:TextBox>
                            <span class="input-group-btn">
                                <asp:LinkButton ID="btnBuscarArtXCodigo" class="btn btn-default" runat="server"><i class="fa fa-search"></i></asp:LinkButton>
                            </span>
                        </div>
                        <div class="input-group">
                            <asp:TextBox ID="txtDescArticulo" PlaceHolder="Ingrese el nombre del articulo" class="form-control"
                                runat="server"></asp:TextBox>
                            <span class="input-group-btn">
                                <asp:LinkButton ID="btnBuscarArtXDesc" class="btn btn-default" runat="server"><i class="fa fa-search"></i></asp:LinkButton>
                            </span>
                        </div>
                        <div class="input-group bootstrap-touchspin">
                            <span class="input-group-btn">
                                <button class="btn btn-default bootstrap-touchspin-down" type="button">
                                    -</button>
                            </span><span class="input-group-addon bootstrap-touchspin-prefix btn btn-default">
                            </span>
                            <input id="" type="text" class="form-control">
                            <span class="input-group-addon bootstrap-touchspin-postfix"></span><span class="input-group-btn">
                                <button class="btn btn-default bootstrap-touchspin-up" type="button">
                                    +</button>
                            </span>
                        </div>
                        <hr class="inner-separator">
                        <legend>Articulos de la Venta</legend>
                        <div>
                            <asp:GridView ID="dgvArticulos" runat="server">
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
            <div class=" col-md-4">
                <div class=" widget">
                    <div class=" widget-header">
                        <h3>
                            <i class="fa fa-edit"></i>Forma de pago
                        </h3>
                    </div>
                    <div class=" widget-content">
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
