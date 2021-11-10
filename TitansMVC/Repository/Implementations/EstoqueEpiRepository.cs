using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using TitansMVC.Models;
using TitansMVC.Repository.Interfaces;

namespace TitansMVC.Repository.Implementations
{
    public class EstoqueEpiRepository: RepositoryBase<EstoqueEpi>, IEstoqueEpiRepository
    {
        public IEnumerable<EstoqueEpi> BuscarPorProduto(int idEpi)
        {
            return Db.EstoquesEpi.Where(e => e.IdEpi == idEpi).Include(e => e.Epi).ToList();
        }

        public void RetirarEstoque(decimal qtde, int idEpi)
        {
            var estoque =
                Db.EstoquesEpi.Where(e => e.IdEpi == idEpi).SingleOrDefault();

            if (estoque == null) return;
            estoque.Qtde = estoque.Qtde - qtde;

            Db.Entry(estoque).State = EntityState.Modified;
        }

        public void ReporEstoque(decimal qtde, int idEpi)
        {
            var estoque =
                Db.EstoquesEpi.Where(e => e.IdEpi == idEpi).SingleOrDefault();

            if (estoque == null) return;
            estoque.Qtde = estoque.Qtde + qtde;

            Db.Entry(estoque).State = EntityState.Modified;
        }
    }
}
