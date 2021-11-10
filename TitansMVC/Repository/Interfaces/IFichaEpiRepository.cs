using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TitansMVC.Models;

namespace TitansMVC.Repository.Interfaces
{
    interface IFichaEpiRepository : IRepositoryBase<FichaEpiModel>
    {
        FichaEpiModel BuscarPorEpi(int idEpi);
    }
}
