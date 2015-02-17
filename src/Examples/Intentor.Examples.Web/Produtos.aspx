<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Produtos.aspx.cs" Inherits="Intentor.Examples.Web.Produtos" %>
<%@ Register src="UserControls/UcProduto.ascx" tagname="UcProduto" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>Exemplo de WebForms gerado automaticamente - Produtos</h1>
    <uc1:UcProduto ID="UcProduto1" runat="server" />
</asp:Content>
