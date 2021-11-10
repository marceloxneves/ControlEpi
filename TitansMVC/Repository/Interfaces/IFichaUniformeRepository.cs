using TitansMVC.Models;

namespace TitansMVC.Repository.Interfaces
{
    interface IFichaUniformeRepository : IRepositoryBase<FichaUniformeModel>
    {
        FichaUniformeModel BuscarPorUniforme(int idUniforme);
    }
}
