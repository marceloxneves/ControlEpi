using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using TitansMVC.Models;
using TitansMVC.Repository.Interfaces;
using TitansMVC.Utils;

namespace TitansMVC.Repository.Implementations
{
    public class SetorRepository : RepositoryBase<SetorModel>, ISetorRepository
    {
        public override void Add(SetorModel setor)
        {
            setor.Ativo = true;
            setor.IdEmpresa = Util.GetEmpresaId();
            setor.DataCad = DateTime.Now;

            Db.Setores.Add(setor);

            Db.SaveChanges();

            Db.Database.ExecuteSqlCommand(string.Format(
                    "insert into [controlepi_hard].[logs] (entidade, operacao, id_reg, id_usuario, datahora, id_empresa) values('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');",
                    "Setor", "insert", setor.Id, HttpContext.Current.User.Identity.GetUserId(),
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), setor.IdEmpresa));
        }

        public override IEnumerable<SetorModel> GetAll()
        {
            int idEmpresa = Util.GetEmpresaId();
            return Db.Setores.Where(e => e.IdEmpresa == idEmpresa);
        }

        public override SetorModel AddWRet(SetorModel setor)
        {
            var entity = Db.Set<SetorModel>().Add(setor);

            Db.SaveChanges();

            Db.Database.ExecuteSqlCommand(string.Format(
                    "insert into [controlepi_hard].[logs] (entidade, operacao, id_reg, id_usuario, datahora, id_empresa) values('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');",
                    "Setor", "insert", setor.Id, HttpContext.Current.User.Identity.GetUserId(),
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), setor.IdEmpresa));

            return entity;
        }

        public SetorModel GetByIdEager(int id)
        {
            //var retorno = Db.Setores.Where(c => c.Ativo).Include(c => c.Epis).SingleOrDefault(c => c.Id == id);
            var retorno = Db.Setores.Where(c => c.Ativo).Include(c => c.Epis).Include(c => c.Uniformes).SingleOrDefault(c => c.Id == id);
            foreach (var item in retorno.Epis)
            {
                item.Epi = Db.Epis.SingleOrDefault(e => e.Id == item.EpiId);
            }
                        
            foreach (var item in retorno.Uniformes)
            {
                item.Uniforme = Db.Uniformes.SingleOrDefault(e => e.Id == item.UniformeId);
            }
            
            return retorno;
        }

        public IEnumerable<SetorModel> BuscarPorNome(string nome, int? UnidadeNegocioId)
        {
            int idEmpresa = Util.GetEmpresaId();
            if(UnidadeNegocioId == null)
                return Db.Setores.Where(c => c.Ativo).Where(s => s.IdEmpresa == idEmpresa).Where(s => s.Nome.StartsWith(nome)).OrderBy(s => s.Nome);

            return Db.Setores.Where(c => c.Ativo).Where(s => s.IdEmpresa == idEmpresa).Where(s => s.Nome.StartsWith(nome)).Where(c => c.UnidadeNegocioId == UnidadeNegocioId).OrderBy(s => s.Nome);
        }

        public SetorModel BuscarPorNomeUnico(string nome)
        {
            int idEmpresa = Util.GetEmpresaId();
            return Db.Setores.Where(s => s.Ativo).Where(s => s.IdEmpresa == idEmpresa).FirstOrDefault(s => s.Nome == nome);
        }

        public int BuscarIdSetor(string nome)
        {
            var id = Db.Database.SqlQuery<int>(
                string.Format("select id from [controlepi_hard].[setor] where lower(nome) = '{0}'", nome.ToLower()));

            return id.FirstOrDefault();
        }

        public IEnumerable<SetorModel> BuscarAtivos()
        {
            int idEmpresa = Util.GetEmpresaId();

            return Db.Setores.Where(s => s.Ativo).Where(s => s.IdEmpresa == idEmpresa).OrderBy(s => s.Nome);
        }

        public int QtdeColaboradores(int idSetor)
        {
            return Db.Setores.Count(s => s.Id == idSetor && s.Colaboradores.Any());
        }

        public IEnumerable<SetorModel> BuscarPorUnidadeNegocio(int id)
        {
            return Db.Setores.Where(s => s.Ativo).Where(s => s.UnidadeNegocioId == id).OrderBy(c => c.Nome);
        }

    }
}