<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="PaginasGestion.aspx.cs" Inherits="PaginasGestion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="main-header">
                <h2>
                    Gestion de Paginas</h2>
                <em>Desde aqui se agregan las paginas</em>
            </div>
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
            <div class="col-lg-6">
                <p>
                    <asp:Button ID="btnNuevaPagina" runat="server" Text="Nueva Pagina" CssClass="btn btn-primary btn-lg btn-block"
                        OnClick="btnNuevaPagina_Click" CausesValidation="False" /></p>
            </div>
            <div class="col-lg-12">
                <div class="table-responsive">
                    <asp:GridView ID="dgvPagina" runat="server" HorizontalAlign="Center" AutoGenerateColumns="False"
                        AllowPaging="True" DataKeyNames="Idpagina" CssClass="table table-bordered table-hover tablesorter"
                        EnableModelValidation="True" OnRowCommand="dgvPagina_RowCommand" PageSize="1000">
                        <Columns>
                            <asp:BoundField DataField="NombrePagina" HeaderText="Nombre" />
                            <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" />
                            <asp:BoundField DataField="Codigo" HeaderText="Codigo" />
                            <asp:ButtonField CommandName="editRecord" ControlStyle-CssClass="btn btn-xs btn-info"
                                ButtonType="Button" Text="Editar"></asp:ButtonField>
                            <asp:ButtonField CommandName="deleteRecord" ControlStyle-CssClass="btn btn-xs btn-info"
                                ButtonType="Button" Text="Eliminar"></asp:ButtonField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <!-- Delete Record Modal Starts here-->
    <div id="deleteModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="delModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                    ×</button>
                <h3 id="delModalLabel">
                    Eliminar Pagina</h3>
            </div>
            <asp:UpdatePanel ID="upDel" runat="server">
                <ContentTemplate>
                    <div class="modal-body">
                        ¿Desea eliminar la pagina:
                        <asp:Label ID="lblPaginaEliminar" runat="server" Text=""></asp:Label>?
                        <asp:HiddenField ID="hfCode" runat="server" />
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnEliminarPagina" runat="server" Text="Eliminar" CssClass="btn btn-success"
                            OnClick="btnEliminarPagina_Click" CausesValidation="False" />
                        <button class="btn btn-danger" data-dismiss="modal" aria-hidden="true">
                            Cancelar</button>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <!--Delete Record Modal Ends here -->
    <!-- Add Record Modal Starts here-->
    <div id="addModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="addModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-content">
            <asp:UpdatePanel ID="upAdd" runat="server">
                <ContentTemplate>
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                            ×</button>
                        <h3 id="addModalLabel">
                            Agregar Pagina</h3>
                    </div>
                    <div class="modal-body">
                        <table class="table table-bordered table-hover">
                            <tr>
                                <td>
                                    Nombre
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Debe indicar un nombre"
                                        ControlToValidate="txtNombre"></asp:RequiredFieldValidator>
                                    <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Descripcion
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Debe indicar una descripcion"
                                        ControlToValidate="txtDescripcion"></asp:RequiredFieldValidator>
                                    <asp:TextBox ID="txtDescripcion" runat="server" CssClass="form-control"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Codigo
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Debe indicar un Codigo"
                                        ControlToValidate="txtCodigo"></asp:RequiredFieldValidator>
                                    <asp:TextBox ID="txtCodigo" runat="server" CssClass="form-control"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnAgregarPagina" runat="server" Text="Agregar" CssClass="btn btn-success"
                            OnClick="btnAgregarPagina_Click" />
                        <button class="btn btn-danger" data-dismiss="modal" aria-hidden="true">
                            Cancelar</button>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <!--Add Record Modal Ends here-->
    <!-- Edit Modal Starts here -->
    <div id="editModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="editModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                    ×</button>
                <h3 id="editModalLabel">
                    Editar Pagina</h3>
            </div>
            <asp:UpdatePanel ID="upEdit" runat="server">
                <ContentTemplate>
                    <asp:HiddenField ID="HiddenFieldModificar" runat="server" />
                    <div class="modal-body">
                        <table class="table">
                            <tr>
                                <td>
                                    Nombre
                                    <asp:TextBox ID="txtNombreModif" runat="server" CssClass="form-control"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Descripcion
                                    <asp:TextBox ID="txtDescripcionModificar" runat="server" CssClass="form-control"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Codigo
                                    <asp:TextBox ID="txtCodigoModificar" runat="server" CssClass="form-control"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnModificarPagina" runat="server" Text="Modificar" CssClass="btn btn-success"
                            OnClick="btnModificarPagina_Click" CausesValidation="False" />
                        <button class="btn btn-danger" data-dismiss="modal" aria-hidden="true">
                            Cerrar</button>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <!-- Edit Modal Ends here -->
</asp:Content>
