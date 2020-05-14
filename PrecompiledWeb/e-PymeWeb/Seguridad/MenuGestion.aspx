<%@ page title="" language="C#" masterpagefile="~/Site.master" autoeventwireup="true" inherits="GestionMenu, App_Web_b1y0b0l5" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
            <div class="main-header">
                <h2>
                    Gestion de Menus</h2>
                <em>Desde aqui se gestionan los menus</em>
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
            <div class="col-lg-6">
                <p>
                    <asp:Button ID="btnNuevoMenu" runat="server" Text="Nuevo Menu" 
                        CssClass="btn btn-primary btn-lg btn-block" onclick="btnNuevoMenu_Click" CausesValidation="False"
                         /></p>
            </div>
            <div class="col-lg-12">
                <div class="table-responsive">
                    <asp:GridView ID="dgvMenus" runat="server" AutoGenerateColumns="False" EnableModelValidation="True"
                        CssClass="table table-striped table-bordered table-hover dataTable no-footer"
                        OnRowCommand="dgvMenus_RowCommand" DataKeyNames="idmenu">
                        <Columns>
                            <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                            <asp:ButtonField CommandName="editMenu"  
                                ControlStyle-CssClass="btn btn-primary btn-xs" Text="Editar Menu">
                            <ControlStyle CssClass="btn btn-primary btn-xs" />
                            </asp:ButtonField>
                            <asp:ButtonField Text="Editar Items" ControlStyle-CssClass="btn btn-primary btn-xs" CommandName="editRecord">                                
                            <ControlStyle CssClass="btn btn-primary btn-xs" />
                            </asp:ButtonField>
                            <asp:ButtonField Text="Eliminar" ControlStyle-CssClass="btn btn-primary btn-xs" CommandName="deleteRecord">                                
                            <ControlStyle CssClass="btn btn-primary btn-xs" />
                            </asp:ButtonField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <!-- Add Record Modal Starts here-->
    <div id="addModal"  class="modal fade"  tabindex="-1" role="dialog" aria-labelledby="addModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                    ×</button>
                <h3 id="addModalLabel">
                    Agregar Menu</h3>
                <div>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Debe indicar un nombre"
                        ControlToValidate="txtNombre"></asp:RequiredFieldValidator></div>
            </div>
            <asp:UpdatePanel ID="upAdd" runat="server">
                <ContentTemplate>
                    <div class="modal-body">
                        <table class="table table-bordered table-hover">
                            <tr>
                                <td>
                                    Nombre
                                    <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnAgregarMenu" runat="server" Text="Agregar" 
                            CssClass="btn btn-success" onclick="btnAgregarMenu_Click"
                             />
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
    <!--Add Record Modal Ends here-->
     <!-- Delete Record Modal Starts here-->
    <div id="deleteModal" class="modal fade"   tabindex="-1" role="dialog" aria-labelledby="delModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                    ×</button>
                <h3 id="delModalLabel">
                    Eliminar Menu</h3>
            </div>
            <asp:UpdatePanel ID="upDel" runat="server">
                <ContentTemplate>
                    <div class="modal-body">
                        ¿Desea eliminar el Menu:  <asp:Label ID="lblMenuEliminar" runat="server" Text=""></asp:Label>?
                        <asp:HiddenField ID="hfCode" runat="server" />
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnEliminarMenu" runat="server" Text="Eliminar" 
                            CssClass="btn btn-success"  CausesValidation="False" onclick="btnEliminarMenu_Click"
                           />
                        <button class="btn btn-danger" data-dismiss="modal" aria-hidden="true">
                            Cancelar</button>
                    </div>
                </ContentTemplate>
                <%--<Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnEliminarPagina" EventName="Click" />
                </Triggers>--%>
            </asp:UpdatePanel>
        </div>
    </div>
    <!--Delete Record Modal Ends here -->
    <!-- Edit Modal Starts here -->
    <div id="editModal" class="modal fade"   tabindex="-1" role="dialog" aria-labelledby="editModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                    ×</button>
                <h3 id="editModalLabel">
                    Editar Menu</h3>
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
                        </table>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnModificarMenu" runat="server" Text="Modificar" 
                            CssClass="btn btn-success"  CausesValidation="False" onclick="btnModificarMenu_Click"
                             />
                        <button class="btn btn-danger" data-dismiss="modal" aria-hidden="true">
                            Cerrar</button>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <!-- Edit Modal Ends here -->
</asp:Content>
