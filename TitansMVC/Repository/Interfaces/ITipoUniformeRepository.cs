using System.Collections.Generic;
using TitansMVC.Models;

namespace TitansMVC.Repository.Interfaces
{
    public interface ITipoUniformeRepository : IRepositoryBase<TipoUniformeModel>
    {
        IEnumerable<TipoUniformeModel> BuscarPorNome(string nome);
        IEnumerable<TipoUniformeModel> BuscarAtivos();
    }
}
