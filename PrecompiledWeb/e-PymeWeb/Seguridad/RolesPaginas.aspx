<%@ page title="" language="C#" masterpagefile="~/Site.master" autoeventwireup="true" inherits="RolesPaginas, App_Web_b1y0b0l5" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">       
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
                <div class="main-header">
                <h2>
                   Editar Rol: <asp:Label ID="lblRol" runat="server" Text=""></asp:Label></h2>
                <em>Desde aqui se editan los roles</em>
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
                <div class=" col-lg-6">
                    <h4>
                        Menu</h4>
                    <div>
                        <asp:DropDownList ID="cbxMenu" runat="server" CssClass="form-control" 
                            OnSelectedIndexChanged="cbxMenu_SelectedIndexChanged" AutoPostBack="True">
                        </asp:DropDownList>
                    </div>
                    <p></p>
                </div>
            </div>
            <div class="row ">
                <div class="col-lg-6">
                    <div class=" panel panel-default">
                        <div class=" panel panel-heading">
                            Paginas del menu
                        </div>
                        <div class=" panel panel-body">
                            <div class=" table table-responsive">
                                <asp:GridView ID="dgvPaginasMenu" runat="server" CssClass="table table-striped table-bordered table-hover"
                                    AutoGenerateColumns="False" EnableModelValidation="True" 
                                    AllowPaging="True" HorizontalAlign="Center"
                                    OnRowCommand="dgvPaginasMenu_RowCommand" DataKeyNames="Idpagina" 
                                    PageSize="100">
                                    <Columns>
                                        <asp:BoundField DataField="NombrePagina" HeaderText="Pagina" />
                                        <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" />
                                        <asp:ButtonField Text="Agregar" ControlStyle-CssClass="btn btn-xs btn-info" CommandName="asignarPagina">
                                            <ControlStyle CssClass="btn btn-xs btn-info" />
                                        </asp:ButtonField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-lg-6">
                    <div class=" panel panel-primary">
                        <div class=" panel panel-heading">
                            Paginas del Rol
                        </div>
                        <div class=" panel panel-body">
                            <div class=" table table-responsive">
                                <asp:GridView ID="dgvPaginasRoles" runat="server" CssClass="table table-striped table-bordered table-hover"
                                    AutoGenerateColumns="False" EnableModelValidation="True" 
                                    AllowPaging="True" HorizontalAlign="Center"
                                    DataKeyNames="Idpagina" onrowcommand="dgvPaginasRoles_RowCommand" 
                                    PageSize="100">
                                    <Columns>
                                        <asp:BoundField DataField="NombrePagina" HeaderText="Pagina" />
                                        <asp:CheckBoxField DataField="SoloLectura" HeaderText="S.L" />
                                        <asp:CheckBoxField DataField="ReingresaClave" HeaderText="R.C" />
                                        <asp:CheckBoxField DataField="PideAutorizacion" HeaderText="P.A" />
                                        <asp:CheckBoxField DataField="Restringido" HeaderText="R" />
                                        <asp:ButtonField Text="Editar" ControlStyle-CssClass="btn btn-xs btn-info" CommandName="editPagina" />
                                        <asp:ButtonField Text="Eliminar" ControlStyle-CssClass="btn btn-xs btn-info" CommandName="deletePagina" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <!-- Add Record Modal Starts here-->
    <div id="addModal" class=" modal fade" tabindex="-1" role="dialog" aria-labelledby="addModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                    ×</button>
                <h3 id="addModalLabel">
                    Asignar Pagina a Rol</h3>
            </div>
            <asp:UpdatePanel ID="upAdd" runat="server">
                <ContentTemplate>
                    <div class="modal-body">
                        <table class="table table-bordered table-hover">
                            <tr>
                                <td>
                                    <asp:HiddenField ID="hfIdPagina" runat="server" />
                                    Pagina
                                    <asp:TextBox ID="txtPagina" Enabled="false" runat="server" CssClass="form-control"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chbxSoloLectura" runat="server" CssClass=" form-control" Text="Solo Lectura" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chbxReingresaClave" runat="server" CssClass=" form-control" Text="Reingresa Clave" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chbxPideAutorizacion" runat="server" CssClass=" form-control" Text="Pide Autorizacion" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chbxRestringido" runat="server" CssClass=" form-control" Text="Restringido" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnAsignarPagina" runat="server" Text="Agregar" CssClass="btn btn-success"
                            OnClick="btnAsignarPagina_Click" />
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
    <div id="deleteModal" class=" modal fade"  tabindex="-1" role="dialog" aria-labelledby="delModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                    ×</button>
                <h3 id="delModalLabel">
                    Quitar Pagina</h3>
            </div>
            <asp:UpdatePanel ID="upDel" runat="server">
                <ContentTemplate>
                    <div class="modal-body">
                        ¿Desea Quitar la pagina del Rol?
                        <asp:HiddenField ID="hfCode" runat="server" />
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnQuitarPagina" runat="server" Text="Eliminar" 
                            CssClass="btn btn-success" CausesValidation="False" onclick="btnQuitarPagina_Click"
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
    <!-- Add Record Modal Starts here-->
    <div id="editModal" class=" modal fade"  tabindex="-1" role="dialog" aria-labelledby="addModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                    ×</button>
                <h3 id="H1">
                    Modificar Pagina</h3>
            </div>
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <div class="modal-body">
                        <table class="table table-bordered table-hover">
                            <tr>
                                <td>
                                    <asp:HiddenField ID="hfidPaginaModif" runat="server" />
                                    Pagina
                                    <asp:TextBox ID="txtPaginaModif" Enabled="false" runat="server" CssClass="form-control"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chbxSoloLecturaModif" runat="server" CssClass=" form-control" Text="Solo Lectura" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chbxReingresaClaveModif" runat="server" CssClass=" form-control" Text="Reingresa Clave" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chbxPideAutModif" runat="server" CssClass=" form-control" Text="Pide Autorizacion" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chbxRestringidoModif" runat="server" CssClass=" form-control" Text="Restringido" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnModificar" runat="server" Text="Modificar" 
                            CssClass="btn btn-success" onclick="btnModificar_Click"
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
</asp:Content>
