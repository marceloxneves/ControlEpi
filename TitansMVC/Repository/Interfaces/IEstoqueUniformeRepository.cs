using System.Collections.Generic;
using TitansMVC.Models;

namespace TitansMVC.Repository.Interfaces
{
    public interface IEstoqueUniformeRepository : IRepositoryBase<EstoqueUniforme>
    {
        IEnumerable<EstoqueUniforme> BuscarPorProduto(int idProduto);
        void RetirarEstoque(decimal qtde, int idUniforme);
        void ReporEstoque(decimal qtde, int idUniforme);
    }
}
