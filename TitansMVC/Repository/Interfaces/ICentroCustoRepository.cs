using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TitansMVC.Models;

namespace TitansMVC.Repository.Interfaces
{
    public interface ICentroCustoRepository : IRepositoryBase<CentroCustoModel>
    {
        IEnumerable<CentroCustoModel> BuscarPorNome(string nome, int? UnidadeNegocioId);
        IEnumerable<CentroCustoModel> BuscarAtivos();
        IEnumerable<CentroCustoModel> BuscarPorUnidadeNegocio(int UnidadeNegocioId);
        CentroCustoModel BuscarPorNomeRetId(string nome);
    }
}
