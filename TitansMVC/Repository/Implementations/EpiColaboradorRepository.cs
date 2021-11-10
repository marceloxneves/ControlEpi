using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using TitansMVC.Models;
using TitansMVC.Repository.Interfaces;
using TitansMVC.Utils;

namespace TitansMVC.Repository.Implementations
{
    public class EpiColaboradorRepository: RepositoryBase<EpiColaboradorModel>, IEpiColaboradorRepository
    {
        public EpiColaboradorModel GetByIdEager(int idEpi)
        {
            //return Db.Set<EpiColaboradorModel>().Include(e => e.Colaborador).Include(e => e.EpiSetor).Single(e => e.Id == idEpi);
            return Db.Set<EpiColaboradorModel>().Include(e => e.Colaborador).Single(e => e.Id == idEpi);
        }

        public IEnumerable<EpiColaboradorModel> BuscarNaoBaixados(int idColaborador)
        {
            return Db.EpisColaboradores.Include(e => e.Colaborador).Where(e => e.ColaboradorId == idColaborador).Where(e => !e.Baixado.Value).OrderBy(e => e.Colaborador.Nome).ToList();
        }

        public IEnumerable<EpiColaboradorModel> BuscarBaixados(int idColaborador)
        {
            int idEmpresa = Util.GetEmpresaId();

            return Db.EpisColaboradores.Include(e => e.Colaborador).Where(e => e.IdEmpresa == idEmpresa).Where(e => e.Baixado.Value).OrderBy(e => e.Colaborador.Nome).ToList();
        }

        public IEnumerable<EpiColaboradorModel> BuscarEpisPorColaborador(int idColaborador)
        {
            return Db.EpisColaboradores.Include(e => e.Colaborador).Where(e => e.ColaboradorId == idColaborador).OrderBy(e => e.Colaborador.Nome).ToList();
        }

        public IEnumerable<EpiColaboradorModel> BuscarEpisVencidos(int idColaborador)
        {
            return Db.EpisColaboradores.Include(e => e.Colaborador).Where(e => e.ColaboradorId == idColaborador).Where(e => e.DataVencimento <= DateTime.Now).OrderBy(e => e.Colaborador.Nome).ToList();
        }

        public IEnumerable<EpiColaboradorModel> BuscarEpisVencidos()
        {
            int idEmpresa = Util.GetEmpresaId();

            return Db.EpisColaboradores.Include(e => e.Colaborador).Where(e => e.IdEmpresa == idEmpresa).Where(e => e.DataVencimento <= DateTime.Now).Where(e => !e.Baixado.Value).OrderBy(e => e.Colaborador.Nome).ToList();
        }

        public IEnumerable<EpiColaboradorModel> BuscarEpisAVencer(int dias)
        {
            int idEmpresa = Util.GetEmpresaId();
            DateTime dataAAvisar = DateTime.Now.AddDays(dias).Date;

            return Db.EpisColaboradores.Include(e => e.Colaborador).Where(e => e.IdEmpresa == idEmpresa).Where(e=> !e.Baixado.Value).Where(e => DbFunctions.TruncateTime(e.DataVencimento) <= dataAAvisar && DbFunctions.TruncateTime(e.DataVencimento) > DbFunctions.TruncateTime(DateTime.Now)).OrderBy(e => e.NomeEpi).ToList();
        }

        public bool EstaEmColaborador(int idEpiSetor)
        {
            return Db.EpisColaboradores.Any(ec => ec.EpiSetorId == idEpiSetor);
        }

    }
}