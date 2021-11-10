using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TitansMVC.Models;
using TitansMVC.Repository.Interfaces;

namespace TitansMVC.Repository.Implementations
{
    public class MunicipioRepository : RepositoryBase<MunicipioModel>, IMunicipioRepository
    {
        public IEnumerable<MunicipioModel> GetAllEager()
        {
            return Db.Municipios.ToList();
        }

        public IEnumerable<MunicipioModel> BuscarPorNome(string nome)
        {
            return Db.Municipios.Where(m => m.Nome.StartsWith(nome));
        }

        public IEnumerable<MunicipioModel> BuscarPorCodIbge(string codIbge)
        {
            return Db.Municipios.Where(m => m.CodIbge.Equals(codIbge));
        }

        public IEnumerable<MunicipioModel> BuscarPorUf(string siglaUf)
        {
            if (!String.IsNullOrEmpty(siglaUf))
            {
                var uf = Db.Ufs.FirstOrDefault(u => u.Sigla == siglaUf);
                return Db.Municipios.Where(m => m.UfId == uf.Id).OrderBy(m => m.Nome);
            }

            return Enumerable.Empty<MunicipioModel>();
        }

        public MunicipioModel BuscaPorUfENome(string uf, string nome)
        {
            var ufObj = Db.Ufs.FirstOrDefault(u => u.Sigla.Equals(uf));
            return Db.Municipios.Where(m => m.UfId.Equals(ufObj.Id)).FirstOrDefault(m=>m.Nome.Equals(nome));

        }

    }
}