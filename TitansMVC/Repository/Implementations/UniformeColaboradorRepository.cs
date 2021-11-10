using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using TitansMVC.Models;
using TitansMVC.Repository.Interfaces;
using TitansMVC.Utils;

namespace TitansMVC.Repository.Implementations
{
    public class UniformeColaboradorRepository: RepositoryBase<UniformeColaboradorModel>, IUniformeColaboradorRepository
    {
        public UniformeColaboradorModel GetByIdEager(int idUniforme)
        {
            return Db.Set<UniformeColaboradorModel>().Include(e => e.Colaborador).Single(e => e.Id == idUniforme);
        }

        public IEnumerable<UniformeColaboradorModel> BuscarNaoBaixados(int idColaborador)
        {
            return Db.UniformesColaboradores.Include(e => e.Colaborador).Where(e => e.ColaboradorId == idColaborador).Where(e => !e.Baixado.Value).OrderBy(e => e.Colaborador.Nome).ToList();
        }

        public IEnumerable<UniformeColaboradorModel> BuscarBaixados(int idColaborador)
        {
            int idEmpresa = Util.GetEmpresaId();

            return Db.UniformesColaboradores.Include(e => e.Colaborador).Where(e => e.IdEmpresa == idEmpresa).Where(e => e.Baixado.Value).OrderBy(e => e.Colaborador.Nome).ToList();
        }

        public IEnumerable<UniformeColaboradorModel> BuscarUniformesPorColaborador(int idColaborador)
        {
            return Db.UniformesColaboradores.Include(e => e.Colaborador).Where(e => e.ColaboradorId == idColaborador).OrderBy(e => e.Colaborador.Nome).ToList();
        }

        public IEnumerable<UniformeColaboradorModel> BuscarUniformesVencidos(int idColaborador)
        {
            return Db.UniformesColaboradores.Include(e => e.Colaborador).Where(e => e.ColaboradorId == idColaborador).Where(e => e.DataVencimento <= DateTime.Now).OrderBy(e => e.Colaborador.Nome).ToList();
        }

        public IEnumerable<UniformeColaboradorModel> BuscarUniformesVencidos()
        {
            int idEmpresa = Util.GetEmpresaId();

            return Db.UniformesColaboradores.Include(e => e.Colaborador).Where(e => e.IdEmpresa == idEmpresa).Where(e => e.DataVencimento <= DateTime.Now).Where(e => !e.Baixado.Value).OrderBy(e => e.Colaborador.Nome).ToList();
        }

        public IEnumerable<UniformeColaboradorModel> BuscarUniformesAVencer(int dias)
        {
            int idEmpresa = Util.GetEmpresaId();
            DateTime dataAAvisar = DateTime.Now.AddDays(dias).Date;

            return Db.UniformesColaboradores.Include(e => e.Colaborador).Where(e => e.IdEmpresa == idEmpresa).Where(e=> !e.Baixado.Value).Where(e => DbFunctions.TruncateTime(e.DataVencimento) <= dataAAvisar && DbFunctions.TruncateTime(e.DataVencimento) > DbFunctions.TruncateTime(DateTime.Now)).OrderBy(e => e.NomeUniforme).ToList();
        }

        public bool EstaEmColaborador(int idUniformeSetor)
        {
            return Db.UniformesColaboradores.Any(ec => ec.UniformeSetorId == idUniformeSetor);
        }

    }
}