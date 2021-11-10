using System.Collections.Generic;
using TitansMVC.Models;

namespace TitansMVC.Repository.Interfaces
{
    public interface IUniformeRepository: IRepositoryBase<UniformeModel>
    {
        UniformeModel GetByIdEager(int id);
        IEnumerable<UniformeModel> GetAll(bool ativos);
        IEnumerable<UniformeModel> BuscarInativos();
        IEnumerable<UniformeModel> BuscarPorId(int id);
        IEnumerable<UniformeModel> BuscarPorTipo(int idTipo); 
        IEnumerable<UniformeModel> BuscarPorNome(string nome,bool ativos);
        IEnumerable<UniformeModel> BuscarAtivos();
        UniformeModel GetByNomeAndMarca(string nome, string marca);
        bool EstaEmSetor(int idUniforme);
        UniformeModel BuscarPorCodigoDeBarras(string codigoDeBarras);
        UniformeModel ValidarCodigoDeBarras(string codigoDeBarras, int idUniforme);
    }
}
