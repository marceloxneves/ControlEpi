using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using TitansMVC.Models;
using TitansMVC.Repository.Interfaces;
using TitansMVC.Utils;
using WebGrease.Css.Extensions;

namespace TitansMVC.Repository.Implementations
{
    public class ColaboradorRepository : RepositoryBase<ColaboradorModel>, IColaboradorRepository
    {
        public override void Add(ColaboradorModel colaborador)
        {           
            colaborador.Ativo = true;
            colaborador.IdEmpresa = Util.GetEmpresaId();
            colaborador.DataCad = DateTime.Now;

            Db.Colaboradores.Add(colaborador);

            Db.SaveChanges();

            Db.Database.ExecuteSqlCommand(string.Format(
                "insert into [controlepi_hard].[logs] (entidade, operacao, id_reg, id_usuario, datahora, id_empresa) values('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');",
                "Colaborador", "insert", colaborador.Id, HttpContext.Current.User.Identity.GetUserId(),
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), colaborador.IdEmpresa));           
        }

        public override ColaboradorModel AddWRet(ColaboradorModel colaborador)
        {
            var entity = Db.Set<ColaboradorModel>().Add(colaborador);

            Db.SaveChanges();
            Db.Database.ExecuteSqlCommand(string.Format(
                "insert into [controlepi_hard].[logs] (entidade, operacao, id_reg, id_usuario, datahora, id_empresa) values('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');",
                "Colaborador", "insert", colaborador.Id, HttpContext.Current.User.Identity.GetUserId(),
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), colaborador.IdEmpresa));

            return entity;
        }

        public override IEnumerable<ColaboradorModel> GetAll()
        {
            int idEmp = Util.GetEmpresaId();
            return Db.Colaboradores.Include(c => c.Setor).Where(c => c.IdEmpresa == idEmp).OrderBy(c => c.Nome);
        }

        public ColaboradorModel GetByIdEager(int id)
        {
            //return Db.Colaboradores./*Where(c => c.Ativo).*/Include(c => c.Funcao).Include(c => c.Setor).Include(c => c.Epis).SingleOrDefault(c => c.Id == id);
            return Db.Colaboradores.Include(c => c.Funcao).Include(c => c.Setor).Include(c => c.Epis).Include(c => c.Uniformes).SingleOrDefault(c => c.Id == id);
        }

        public IEnumerable<ColaboradorModel> BuscarPorId(int id)
        {
            return Db.Colaboradores.Where(c => c.Ativo).Where(c => c.Id.Equals(id));
        }

        public IEnumerable<ColaboradorModel> BuscarPorNome(string nome, int? UnidadeNegocioId,bool ativo)
        {
            int idEmpresa = Util.GetEmpresaId();

            if(UnidadeNegocioId == null)
                return Db.Colaboradores.Where(c => c.Ativo.Equals(ativo)).Where(c => c.IdEmpresa == idEmpresa).Where(s => s.Nome.StartsWith(nome)).OrderBy(s => s.Nome);

            return Db.Colaboradores.Where(c => c.Ativo.Equals(ativo)).Where(c => c.IdEmpresa == idEmpresa).Where(s => s.Nome.StartsWith(nome)).Where(c => c.LbcId == UnidadeNegocioId).OrderBy(s => s.Nome);
        }

        public IEnumerable<ColaboradorModel> BuscarPorCpf(string cpf, int? UnidadeNegocioId, bool ativo)
        {
            int idEmpresa = Util.GetEmpresaId();

            if(UnidadeNegocioId == null)
                return Db.Colaboradores.Where(c => c.Ativo.Equals(ativo)).Where(c => c.IdEmpresa == idEmpresa).Where(s => s.Cpf == cpf).OrderBy(s => s.Nome);

            return Db.Colaboradores.Where(c => c.Ativo.Equals(ativo)).Where(c => c.IdEmpresa == idEmpresa).Where(s => s.Cpf == cpf).Where(c => c.LbcId == UnidadeNegocioId).OrderBy(s => s.Nome);
        }

        public IEnumerable<ColaboradorModel> BuscarAtivos()
        {
            int idEmpresa = Util.GetEmpresaId();

            return Db.Colaboradores.Where(c => c.IdEmpresa == idEmpresa).Where(c => c.Ativo).OrderBy(c => c.Nome);
        }

        public int ContarColaboradores(int idEmpresa)
        {
            return Db.Colaboradores.Where(c => c.IdEmpresa == idEmpresa).Count(c => c.Ativo);
        }

        public override void Update(ColaboradorModel colaborador)
        {
            Db.Entry(colaborador).State = EntityState.Modified;

            Db.SaveChanges();

            Db.Database.ExecuteSqlCommand(string.Format(
                    "insert into [controlepi_hard].[logs] (entidade, operacao, id_reg, id_usuario, datahora, id_empresa) values('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');",
                    "Colaborador", "update", colaborador.Id, HttpContext.Current.User.Identity.GetUserId(),
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), colaborador.IdEmpresa));
        }
        
        public IEnumerable<ColaboradorModel> BuscarPorUnidadeNegocio(int id, bool ativo)
        {
            int idEmp = Util.GetEmpresaId();

            return Db.Colaboradores.Where(c => c.IdEmpresa == idEmp).Where(c => c.Ativo.Equals(ativo)).Where(c => c.LbcId == id).OrderBy(c => c.Nome);
        }
        public IEnumerable<ColaboradorModel> GetAll(bool ativos)
        {
            int idEmp = Util.GetEmpresaId();
            return Db.Colaboradores.Where(c => c.IdEmpresa == idEmp).Where(c=>c.Ativo.Equals(ativos)).OrderBy(s => s.Nome);
        }

        public IEnumerable<ColaboradorModel> BuscarPorCpf(string cpf)
        {
            int idEmp = Util.GetEmpresaId();
            return Db.Colaboradores.Where(c => c.IdEmpresa == idEmp).Where(c => c.Cpf.Equals(cpf));
        }

        public IEnumerable<ColaboradorModel> BuscarPorUnidadeNegocio(int id)
        {
            int idEmp = Util.GetEmpresaId();

            return Db.Colaboradores.Where(c => c.IdEmpresa == idEmp).Where(c => c.LbcId == id);
        }
    }
}