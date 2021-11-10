using TitansMVC.Models.Relatorios;
using TitansMVC.Repository.Interfaces;

namespace TitansMVC.Repository.Implementations
{
    public class RelConsumoUniformeRepository : RepositoryBase<RelConsumoUniformeModel>, IRelConsumoUniformeRepository
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