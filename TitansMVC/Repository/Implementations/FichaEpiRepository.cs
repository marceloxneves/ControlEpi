using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TitansMVC.Models;
using TitansMVC.Repository.Interfaces;

namespace TitansMVC.Repository.Implementations
{
    public class FichaEpiRepository : RepositoryBase<FichaEpiModel>, IFichaEpiRepository
    {
        public FichaEpiModel BuscarPorEpi(int idEpi)
        {
            return Db.FichasEpi.SingleOrDefault(f => f.IdEpi == idEpi);
        }
    }
}