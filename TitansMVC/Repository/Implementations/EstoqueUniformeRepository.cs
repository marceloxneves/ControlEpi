using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using TitansMVC.Models;
using TitansMVC.Repository.Interfaces;

namespace TitansMVC.Repository.Implementations
{
    public class EstoqueUniformeRepository: RepositoryBase<EstoqueUniforme>, IEstoqueUniformeRepository
    {
        public IEnumerable<EstoqueUniforme> BuscarPorProduto(int idUniforme)
        {
            return Db.EstoquesUniformes.Where(e => e.IdUniforme == idUniforme).Include(e => e.Uniforme).ToList();
        }

        public void RetirarEstoque(decimal qtde, int idUniforme)
        {
            var estoque =
                Db.EstoquesUniformes.Where(e => e.IdUniforme == idUniforme).SingleOrDefault();

            if (estoque == null) return;
            estoque.Qtde = estoque.Qtde - qtde;

            Db.Entry(estoque).State = EntityState.Modified;
        }

        public void ReporEstoque(decimal qtde, int idUniforme)
        {
            var estoque =
                Db.EstoquesUniformes.Where(e => e.IdUniforme == idUniforme).SingleOrDefault();

            if (estoque == null) return;
            estoque.Qtde = estoque.Qtde + qtde;

            Db.Entry(estoque).State = EntityState.Modified;
        }
    }
}
