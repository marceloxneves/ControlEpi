using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TitansMVC.Models;

namespace TitansMVC.Repository.Interfaces
{
    public interface IMunicipioRepository : IRepositoryBase<MunicipioModel>
    {
        IEnumerable<MunicipioModel> GetAllEager();
        IEnumerable<MunicipioModel> BuscarPorNome(string nome);
        IEnumerable<MunicipioModel> BuscarPorCodIbge(string codIbge);
        IEnumerable<MunicipioModel> BuscarPorUf(string siglaUf);
        MunicipioModel BuscaPorUfENome(string uf, string nome);
    }
}
