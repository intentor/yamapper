using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Intentor.Examples.Model.Core.Entities;
using Intentor.Examples.Model.Data;
using Intentor.Examples.Model.Domain;

namespace Intentor.Examples.Facade
{
    public static class ProdutosFacade
    {
        public static List<Cliente> GetClientes()
        {
            ClienteBiz biz = new ClienteBiz(new ClienteDao());
            return biz.GetAll();
        }

        public static List<Cliente> GetClientesOrderedByName()
        {
            ClienteBiz biz = new ClienteBiz(new ClienteDao());
            return biz.GetClientesOrderedByName();
        }

        public static List<Cliente> GetClientesNomesComJ()
        {
            ClienteBiz biz = new ClienteBiz(new ClienteDao());
            return biz.GetClientesNomesComJ();
        }

        public static List<Cliente> GetClientesComCertosNomes()
        {
            ClienteBiz biz = new ClienteBiz(new ClienteDao());
            return biz.GetClientesComCertosNomes();
        }
        
        public static List<Produto> GetProdutos()
        {
            ProdutoBiz biz = new ProdutoBiz(new ProdutoDao());
            return biz.GetAll();
        }
    }
}
