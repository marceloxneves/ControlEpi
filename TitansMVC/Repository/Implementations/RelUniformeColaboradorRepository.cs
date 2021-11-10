using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using TitansMVC.Context;
using TitansMVC.Models;
using TitansMVC.Models.Relatorios;
using TitansMVC.Repository.Interfaces;

namespace TitansMVC.Repository.Implementations
{
    public class RelUniformeColaboradorRepository : RepositoryBase<RelUniformeColModel>, IRelUniformeColaboradorRepository
    {
        //protected TitansContext Db = new TitansContext();

        //public IEnumerable<RelEpiColModel> ConsultaEpiColaborador(string consulta)
        //{
        //    ObjectResult<RelEpiColModel> episColaboradores =
        //        (Db as IObjectContextAdapter).ObjectContext.ExecuteStoreQuery<RelEpiColModel>(consulta);

        //    return episColaboradores.ToList();
        //}        
    }
}