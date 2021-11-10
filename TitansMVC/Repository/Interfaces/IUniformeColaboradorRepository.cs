using System.Collections.Generic;
using TitansMVC.Models;

namespace TitansMVC.Repository.Interfaces
{
    public interface IUniformeColaboradorRepository: IRepositoryBase<UniformeColaboradorModel>
    {
        UniformeColaboradorModel GetByIdEager(int idUniforme);
        IEnumerable<UniformeColaboradorModel> BuscarUniformesPorColaborador(int idColaborador);
        IEnumerable<UniformeColaboradorModel> BuscarNaoBaixados(int idColaborador);
        IEnumerable<UniformeColaboradorModel> BuscarBaixados(int idColaborador);
        IEnumerable<UniformeColaboradorModel> BuscarUniformesVencidos(int idColaborador);
        IEnumerable<UniformeColaboradorModel> BuscarUniformesVencidos();
        IEnumerable<UniformeColaboradorModel> BuscarUniformesAVencer(int dias);
        bool EstaEmColaborador(int idUniformeSetor);
    }
}
