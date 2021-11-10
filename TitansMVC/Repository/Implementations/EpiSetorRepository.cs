using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TitansMVC.Models;
using TitansMVC.Repository.Interfaces;

namespace TitansMVC.Repository.Implementations
{
    public class EpiSetorRepository : RepositoryBase<EpiSetorModel>, IEpiSetorRepository
    {
        public IEnumerable<EpiSetorModel> BUscarPorTipo(int idTipo)
        {
            return Db.EpisSetores.Where(e => e.TipoEpiId == idTipo);
        }

        public IEnumerable<EpiSetorModel> BUscarPorTipoESetor(int idTipo, int idSetor)
        {
            return Db.EpisSetores.Where(e=>e.Epi.Ativo).Where(e => e.TipoEpiId == idTipo).Where(e => e.SetorId == idSetor).ToList();
        }

        public IEnumerable<EpiSetorModel> BuscarPorTipoEOutrosSetores(int idTipo, int idSetor)
        {
            return Db.EpisSetores.Where(e => e.Epi.Ativo).Where(e => e.TipoEpiId == idTipo).Where(e => e.SetorId != idSetor).OrderBy(e => e.NomeEpi).ToList();
        }

        public IEnumerable<EpiSetorModel> BuscarPorSetor(int idSetor)
        {
            return Db.EpisSetores.Where(e => e.SetorId == idSetor).OrderBy(e => e.NomeEpi);
        }

        public IEnumerable<EpiSetorModel> BuscarPorOutrosSetores(int idSetor)
        {
            return Db.EpisSetores.Where(e => e.SetorId != idSetor).OrderBy(e => e.NomeEpi);
        }

        public IEnumerable<EpiSetorModel> BuscarPorEpi(int idEpi)
        {
            return Db.EpisSetores.Where(e => e.EpiId == idEpi).ToList();
        }

        public bool EpiSetorEntregue(int idEpiSetor)
        {
            return Db.EpisColaboradores.Where(e => e.EpiSetorId == idEpiSetor).Any(e => !e.Baixado.Value);
        }
    }
}