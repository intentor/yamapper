/*********************************************
Generated by Intentor.Yamapper Class Generator
http://intentor.com.br/projects/yamapper/
*********************************************/
using System;
using System.Collections.Generic;
using Intentor.Examples.Model.Core.Entities;

namespace Intentor.Examples.Model.Core.DataInterfaces
{	
	/// <summary>Data interface IProdutoRepository.</summary>
	public partial interface IProdutoRepository :
        Intentor.Yamapper.ICommonDataBaseActions<Produto>
	{
		/// <summary>Obtém objeto <see cref="Produto"/> com base em sua(s) chave(s) primária(s).</summary>
		/// <returns>Objeto <see cref="Produto"/>.</returns>
		Produto GetById(int idProduto);
		
		/// <summary>Exclui um objeto <see cref="Produto"/> com base em sua(s) chave(s) primária(s).</summary>
        /// <returns>Quantidade de registros excluídos.</returns>
		int Delete(int idProduto);

	}
}
