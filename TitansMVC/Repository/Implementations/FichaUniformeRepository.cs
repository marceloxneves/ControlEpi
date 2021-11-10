using System.Linq;
using TitansMVC.Models;
using TitansMVC.Repository.Interfaces;

namespace TitansMVC.Repository.Implementations
{
    public class FichaUniformeRepository : RepositoryBase<FichaUniformeModel>, IFichaUniformeRepository
    {
        public FichaUniformeModel BuscarPorUniforme(int idUniforme)
        {
            return Db.FichasUniformes.SingleOrDefault(f => f.IdUniforme == idUniforme);
        }
    }
}