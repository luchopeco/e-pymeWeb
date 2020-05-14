<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="MensajeCajaCerrada.aspx.cs" Inherits="MensajeCajaCerrada" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="main-header">
        <h2>
            CAJA CERRADA |
        </h2>
    </div>
    <div class="row">
        <div class="col-md-12">
            <div class=" widget">
                <div class=" widget-header">
                    No Puede Operar Fondos
                </div>
                <div class=" widget-content">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="alert alert-danger alert-dismissable">
                                El usuario actual no posee una caja abierta. Por lo tanto no puede operar con fondos
                                hasta abrir una caja, o reabrir la caja diaria.
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
