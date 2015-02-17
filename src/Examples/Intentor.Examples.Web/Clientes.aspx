<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Clientes.aspx.cs" Inherits="Intentor.Examples.Web.Clientes" %>
<%@ Register src="UserControls/UcCliente.ascx" tagname="UcCliente" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>Clientes</h1>
    <p>Exemplo de WebForm gerado automaticamente</p>
    <uc1:UcCliente ID="UcCliente1" runat="server" />
</asp:Content>
