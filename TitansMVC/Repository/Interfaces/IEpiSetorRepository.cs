using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TitansMVC.Models;

namespace TitansMVC.Repository.Interfaces
{
    public interface IEpiSetorRepository : IRepositoryBase<EpiSetorModel>
    {
        IEnumerable<EpiSetorModel> BUscarPorTipo(int idTipo);
        IEnumerable<EpiSetorModel> BUscarPorTipoESetor(int idTipo, int idSetor);
        IEnumerable<EpiSetorModel> BuscarPorTipoEOutrosSetores(int idTipo, int idSetor);
        IEnumerable<EpiSetorModel> BuscarPorSetor(int idSetor);
        IEnumerable<EpiSetorModel> BuscarPorOutrosSetores(int idSetor);
        IEnumerable<EpiSetorModel> BuscarPorEpi(int idEpi);
        bool EpiSetorEntregue(int idEpiSetor);
    }
}