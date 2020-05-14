<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="SucursalesPaginas.aspx.cs" Inherits="Seguridad_SucursalesPaginas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
            <div class="main-header">
                <h2>
                    Restriccion de paginas</h2>
                <em>Desde aqui se Restringe el ingreso de paginas para cada sucursal</em>
            </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="PanelError" Visible="false" runat="server">
                <div class="alert alert-danger alert-dismissable">
                    <button type="button" class="close" data-dismiss="alert" aria-hidden="true">
                        ×</button>
                    <asp:Label ID="lblError" runat="server" Text=""></asp:Label>
                </div>
            </asp:Panel>
            <asp:Panel ID="PanelMensaje" Visible="false" runat="server">
                <div class="alert alert-success alert-dismissable">
                    <button type="button" class="close" data-dismiss="alert" aria-hidden="true">
                        ×</button>
                    <asp:Label ID="LabelMensaje" runat="server" Text=""></asp:Label>
                </div>
            </asp:Panel>
            <div class="row">
            <div class=" col-lg-12">
            <div>Sucursal</div>
                <p><asp:DropDownList ID="cbxSucursales" runat="server" CssClass=" form-control" 
                        AutoPostBack="True" onselectedindexchanged="cbxSucursales_SelectedIndexChanged">
                </asp:DropDownList></p>
            </div>
                <div class=" col-lg-6">
                    <div class=" panel panel-default">
                        <div class=" panel panel-heading">
                            Paginas sin Restriccion</div>
                        <div class=" pane panel-body">
                            <div class=" table-responsive">
                                <asp:GridView ID="dgvPaginas" runat="server" 
                                CssClass="table table-striped table-bordered table-hover" 
                                    AutoGenerateColumns="False" EnableModelValidation="True" 
                            DataKeyNames="Idpagina" PageSize="1000" onrowcommand="dgvPaginas_RowCommand">
                                    <Columns>
                                        <asp:BoundField DataField="NombrePagina" HeaderText="Pagina" />
                                        <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" />
                                        <asp:ButtonField Text="Agregar" CommandName="agregar">
                                        <ControlStyle CssClass="btn btn-primary btn-xs" />
                                        </asp:ButtonField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
                <div class=" col-lg-6">
                    <div class=" panel panel-info">
                        <div class=" panel panel-heading">
                            Paginas Restringidas</div>
                        <div class=" pane panel-body">
                            <div class=" table-responsive">
                                <asp:GridView ID="dgvPaginasRestringidas" runat="server" 
                                CssClass="table table-striped table-bordered table-hover" 
                                    EnableModelValidation="True" AutoGenerateColumns="False" 
                                    DataKeyNames="Idpagina" PageSize="1000" 
                                    onrowcommand="dgvPaginasRestringidas_RowCommand">
                                    <Columns>
                                        <asp:BoundField DataField="NombrePagina" HeaderText="Pagina" />
                                        <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" />
                                        <asp:ButtonField Text="Quitar" CommandName="quitar" >
                                        <ControlStyle CssClass="btn btn-primary btn-xs" />
                                        </asp:ButtonField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
