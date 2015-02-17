using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Intentor.Examples.Facade;
using Intentor.Examples.Model.Core.Entities;
using Intentor.Examples.Model.Data;
using Intentor.Examples.Model.Domain;
using Intentor.Examples.Web.Core.Views;
using Intentor.Yamapper;
using Intentor.Utilities;

namespace Intentor.Examples.Web
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.gvClientesOrdered.DataSource = ProdutosFacade.GetClientesOrderedByName().BindTo<ClienteView, Cliente>();
            this.gvClientesOrdered.DataBind();

            this.gvClientesNomesComJ.DataSource = ProdutosFacade.GetClientesNomesComJ().BindTo<ClienteView, Cliente>();
            this.gvClientesNomesComJ.DataBind();

            this.gvClientesComCertosNomes.DataSource = ProdutosFacade.GetClientesComCertosNomes().BindTo<ClienteView, Cliente>();
            this.gvClientesComCertosNomes.DataBind();

            this.gvProdutos.DataSource = ProdutosFacade.GetProdutos().BindTo<ProdutoView, Produto>();
            this.gvProdutos.DataBind();
        }
    }

    #region ObjectDataSource

    /// <summary>
    /// ObjectDataSource da página.
    /// </summary>
    public class ClientesDataSource
    {
        private ClienteBiz _biz = new ClienteBiz(new ClienteDao());

        public int SelectCount()
        {
            return _biz.Count();
        }

        public List<ClienteView> Select(int maximumRows, int startRowIndex)
        {
            return _biz.GetByCriteria(Criteria.Create().Offset(startRowIndex).Limit(maximumRows)).BindTo<ClienteView, Cliente>();
        }
    }

    #endregion
}