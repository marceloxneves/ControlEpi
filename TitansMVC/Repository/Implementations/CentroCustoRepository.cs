using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using TitansMVC.Models;
using TitansMVC.Repository.Interfaces;
using TitansMVC.Utils;

namespace TitansMVC.Repository.Implementations
{
    public class CentroCustoRepository : RepositoryBase<CentroCustoModel>, ICentroCustoRepository
    {
        public override void Add(CentroCustoModel centroCusto)
        {
            centroCusto.Ativo = true;

            centroCusto.IdEmpresa = centroCusto.IdEmpresa == null ? Util.GetEmpresaId() : centroCusto.IdEmpresa ;
            centroCusto.DataCad = DateTime.Now;

            Db.CentrosCusto.Add(centroCusto);

            Db.SaveChanges();

            Db.Database.ExecuteSqlCommand(string.Format(
                    "insert into [controlepi_hard].[logs] (entidade, operacao, id_reg, id_usuario, datahora, id_empresa) values('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');",
                    "CentroCusto", "insert", centroCusto.Id, HttpContext.Current.User.Identity.GetUserId(),
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), centroCusto.IdEmpresa));
        }

        public override CentroCustoModel AddWRet(CentroCustoModel centroCusto)
        {
            var entity = Db.Set<CentroCustoModel>().Add(centroCusto);

            //Db.Entry(obj).State = EntityState.Added;
            // implantar padrao Unit of Work posteriormente
            Db.SaveChanges();

            Db.Database.ExecuteSqlCommand(string.Format(
                    "insert into [controlepi_hard].[logs] (entidade, operacao, id_reg, id_usuario, datahora, id_empresa) values('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');",
                    "CentroCusto", "insert", centroCusto.Id, HttpContext.Current.User.Identity.GetUserId(),
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), centroCusto.IdEmpresa));

            return entity;
        }

        public IEnumerable<CentroCustoModel> BuscarPorNome(string nome, int? UnidadeNegocioId)
        {
            int idEmpresa = Util.GetEmpresaId();

            if (UnidadeNegocioId == null)
                return Db.CentrosCusto.Where(c => c.Ativo).Where(c => c.IdEmpresa == idEmpresa).Where(c => c.Nome.StartsWith(nome)).Include(c => c.Lbc).OrderBy(c => c.Nome);

            return Db.CentrosCusto.Where(c => c.Ativo).Where(c => c.IdEmpresa == idEmpresa).Where(c => c.Nome.StartsWith(nome)).Where(c => c.LbcId == UnidadeNegocioId).Include(c => c.Lbc).OrderBy(c => c.Nome);
        }

        public IEnumerable<CentroCustoModel> BuscarAtivos()
        {
            int idEmpresa = Util.GetEmpresaId();

            return Db.CentrosCusto.Where(c => c.Ativo).Where(c => c.IdEmpresa == idEmpresa).Include(c => c.Lbc).OrderBy(c => c.Nome);
        }

        public CentroCustoModel BuscarPorNomeRetId(string nome)
        {
            int idEmpresa = Util.GetEmpresaId();
            
            return Db.CentrosCusto.Where(c => c.Ativo).Where(c => c.IdEmpresa == idEmpresa).Where(c => c.Nome.StartsWith(nome)).Include(c => c.Lbc).OrderBy(c => c.Nome).Single();
        }

        public IEnumerable<CentroCustoModel> BuscarPorUnidadeNegocio(int UnidadeNegocioId)
        {
            return Db.CentrosCusto.Where(c => c.Ativo).Where(c => c.LbcId == UnidadeNegocioId).Include(c => c.Lbc).OrderBy(c => c.Nome);
        }
    }
}