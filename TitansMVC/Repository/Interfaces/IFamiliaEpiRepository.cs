using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TitansMVC.Models;

namespace TitansMVC.Repository.Interfaces
{
    public interface IFamiliaEpiRepository : IRepositoryBase<FamiliaEpiModel>
    {
        IEnumerable<FamiliaEpiModel> BuscarPorNome(string nome);
        IEnumerable<FamiliaEpiModel> BuscarAtivos();
    }
}
