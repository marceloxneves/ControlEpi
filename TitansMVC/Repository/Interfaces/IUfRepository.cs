using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TitansMVC.Models;

namespace TitansMVC.Repository.Interfaces
{
    public interface IUfRepository : IRepositoryBase<UfModel>
    {
        IEnumerable<UfModel> BuscarPorNome(string nome);
        IEnumerable<UfModel> BuscarPorSigla(string sigla);
        IEnumerable<UfModel> BuscarPorCodIbge(string codIbge);
    }
}