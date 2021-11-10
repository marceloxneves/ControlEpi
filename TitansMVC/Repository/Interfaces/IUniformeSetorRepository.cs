using System.Collections.Generic;
using TitansMVC.Models;

namespace TitansMVC.Repository.Interfaces
{
    public interface IUniformeSetorRepository : IRepositoryBase<UniformeSetorModel>
    {
        IEnumerable<UniformeSetorModel> BuscarPorTipo(int idTipo);
        IEnumerable<UniformeSetorModel> BuscarPorTipoESetor(int idTipo, int idSetor);
        IEnumerable<UniformeSetorModel> BuscarPorTipoEOutrosSetores(int idTipo, int idSetor);
        IEnumerable<UniformeSetorModel> BuscarPorSetor(int idSetor);
        IEnumerable<UniformeSetorModel> BuscarPorOutrosSetores(int idSetor);
        IEnumerable<UniformeSetorModel> BuscarPorUniforme(int idUniforme);
        bool UniformeSetorEntregue(int idUniformeSetor);
    }
}