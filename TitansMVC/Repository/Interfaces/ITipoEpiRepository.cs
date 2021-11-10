using System.Collections.Generic;
using TitansMVC.Models;

namespace TitansMVC.Repository.Interfaces
{
    public interface ITipoEpiRepository : IRepositoryBase<TipoEpiModel>
    {
        IEnumerable<TipoEpiModel> BuscarPorNome(string nome);
        IEnumerable<TipoEpiModel> BuscarAtivos();
    }
}
