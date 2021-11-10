using System.Collections.Generic;
using TitansMVC.Models;

namespace TitansMVC.Repository.Interfaces
{
    public interface IEstoqueEpiRepository : IRepositoryBase<EstoqueEpi>
    {
        IEnumerable<EstoqueEpi> BuscarPorProduto(int idProduto);
        void RetirarEstoque(decimal qtde, int idEpi);
        void ReporEstoque(decimal qtde, int idEpi);
    }
}
