<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Intentor.Examples.Web.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>Exemplos de GridViews</h1>
    <div>        
        <asp:GridView ID="gvAllClientes" runat="server" Caption="Todos os clientes com paginação (vide code behind)" 
            AllowPaging="True" PageSize="5" AutoGenerateColumns="false" DataSourceID="odsClientes">
            <Columns>
                <asp:BoundField DataField="Nome" HeaderText="Cliente" />
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="odsClientes" runat="server" EnablePaging="True" 
            TypeName="Intentor.Examples.Web.ClientesDataSource" SelectMethod="Select" SelectCountMethod="SelectCount"></asp:ObjectDataSource>
    </div>
    <div>        
        <asp:GridView ID="gvClientesOrdered" runat="server" Caption="Todos os clientes ordenados por nome">
        </asp:GridView>
    </div>
    <div>        
        <asp:GridView ID="gvClientesNomesComJ" runat="server" Caption="Clientes de nomes que contenham J">
        </asp:GridView>
    </div>
    <div>        
        <asp:GridView ID="gvClientesComCertosNomes" runat="server" Caption="Clientes de nome João, José e Josisbaldo ordenados por nome">
        </asp:GridView>
    </div>
    <div>        
        <asp:GridView ID="gvProdutos" runat="server" Caption="Todos os produtos">
        </asp:GridView>
    </div>
</asp:Content>
