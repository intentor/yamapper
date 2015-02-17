<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcProduto.ascx.cs" Inherits="Intentor.Examples.Web.UserControls.UcProduto" %>
<asp:UpdatePanel ID="upDados" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
    <ContentTemplate>
        <div id="dvGrid" runat="server">
            <asp:GridView ID="grvDados" runat="server" AutoGenerateColumns="False" 
                Caption="Produto" AllowPaging="True" 
                DataKeyNames="IdProduto" PageSize="20"
                DataSourceID="odsProduto"
                OnRowDataBound="grvDados_RowDataBound"
                OnRowCommand="grvDados_RowCommand">
                <Columns>
                    <asp:TemplateField ShowHeader="False">
                        <ItemTemplate>
                            <asp:ImageButton ID="ibtnVer" runat="server" CausesValidation="false" 
                                CommandName="Ver" AlternateText="Ver"></asp:ImageButton>
                        </ItemTemplate>
                        <HeaderStyle Width="1.5em" />
                        <ItemStyle CssClass="center" />
                    </asp:TemplateField>
                    <asp:TemplateField ShowHeader="False">
                        <ItemTemplate>
                            <asp:ImageButton ID="ibtnExcluir" runat="server" CausesValidation="false" 
                                CommandName="Excluir" AlternateText="Excluir"
                                OnClientClick="if (!confirm('Tem certeza de que deseja excluir o registro selecionado?')) return false;"></asp:ImageButton>
                        </ItemTemplate>
                        <HeaderStyle Width="1.5em" />
                        <ItemStyle CssClass="center" />
                    </asp:TemplateField>
					<asp:BoundField DataField="Nome" HeaderText="Nome"></asp:BoundField>
				</Columns>
                <EmptyDataTemplate>
					Não existem registros cadastrados.
                </EmptyDataTemplate>
            </asp:GridView>
			<asp:ObjectDataSource ID="odsProduto" runat="server" EnablePaging="True" 
				TypeName="Intentor.Examples.Web.UserControls.ProdutoDataSource" SelectMethod="Select" SelectCountMethod="SelectCount"></asp:ObjectDataSource>
            <div class="controles">
                <asp:Button ID="btnNovo" runat="server" Text="Cadastrar" onclick="btnNovo_Click" />
            </div>
        </div>
        <div id="dvForm" runat="server">
            <asp:HiddenField ID="hidIdObj" runat="server" />
            <fieldset class="form noBottomBorder" >
                <legend>Dados do turno</legend>
				<div class="notes">
					<h4>Informações</h4>
					<p>
						Todos os campos marcados com * são de 
						preenchimeto obrigatório.
					</p>
				</div>
				<div>
					<label for="txtNome">Nome:</label>
					<asp:TextBox ID="txtNome" runat="server" MaxLength="100"></asp:TextBox>
					<span class="obrigatorio">*</span>
					<p>
						<asp:RequiredFieldValidator ID="rfvNome" runat="server" CssClass="validacao"
							ControlToValidate="txtNome" ErrorMessage="Campo Nome obrigatório." 
							ValidationGroup="Cadastro" Display="Dynamic"></asp:RequiredFieldValidator>
					</p>
				</div>
			</fieldset>
            <div id="dvBotoes">
                <asp:Button ID="btnCadastrar" runat="server" Text="Cadastrar" 
                    ValidationGroup="Cadastro" onclick="btnCadastrar_Click" />
                <asp:Button ID="btnSalvar" runat="server" Text="Salvar" 
                    ValidationGroup="Cadastro" onclick="btnSalvar_Click" />
                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" 
                    onclick="btnCancelar_Click" />
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>