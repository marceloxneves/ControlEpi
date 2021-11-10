using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TitansMVC.Models;
using TitansMVC.Repository.Interfaces;

namespace TitansMVC.Repository.Implementations
{
    public class UfRepository : RepositoryBase<UfModel>, IUfRepository
    {
        public override IEnumerable<UfModel> GetAll()
        {
            return Db.Ufs.ToList().OrderBy(u => u.Nome);
        }

        public IEnumerable<UfModel> BuscarPorNome(string nome)
        {
            return Db.Ufs.Where(u => u.Nome.StartsWith(nome));
        }
        public IEnumerable<UfModel> BuscarPorSigla(string sigla)
        {
            return Db.Ufs.Where(u => u.Sigla.Equals(sigla));
        }
        public IEnumerable<UfModel> BuscarPorCodIbge(string codIbge)
        {
            return Db.Ufs.Where(u => u.CodIbge.Equals(codIbge));
        }
    }
}