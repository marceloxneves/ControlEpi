using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TitansMVC.Models;

namespace TitansMVC.Repository.Interfaces
{
    public interface ILbcRepository : IRepositoryBase<LbcModel>
    {
        IEnumerable<LbcModel> BuscarPorNome(string nome);
        IEnumerable<LbcModel> BuscarAtivos();
        LbcModel GetByCnpj(string cnpj);
        int QtdeCentrosCustos(int idLbc);
    }
}
