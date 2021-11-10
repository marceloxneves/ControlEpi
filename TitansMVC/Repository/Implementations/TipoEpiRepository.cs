using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using TitansMVC.Models;
using TitansMVC.Repository.Interfaces;
using TitansMVC.Utils;

namespace TitansMVC.Repository.Implementations
{
    public class TipoEpiRepository : RepositoryBase<TipoEpiModel>, ITipoEpiRepository
    {
        public override void Add(TipoEpiModel tipoEpi)
        {
            tipoEpi.Ativo = true;
            tipoEpi.IdEmpresa = Util.GetEmpresaId();

            Db.TiposEpis.Add(tipoEpi);

            Db.SaveChanges();

            Db.Database.ExecuteSqlCommand(string.Format(
                    "insert into [controlepi_hard].[logs] (entidade, operacao, id_reg, id_usuario, datahora, id_empresa) values('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');",
                    "TipoEpi", "insert", tipoEpi.Id, HttpContext.Current.User.Identity.GetUserId(),
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), tipoEpi.IdEmpresa));
        }

        public override TipoEpiModel AddWRet(TipoEpiModel tipoEpi)
        {
            var entity = Db.Set<TipoEpiModel>().Add(tipoEpi);

            Db.SaveChanges();

            Db.Database.ExecuteSqlCommand(string.Format(
                    "insert into [controlepi_hard].[logs] (entidade, operacao, id_reg, id_usuario, datahora, id_empresa) values('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');",
                    "Setor", "insert", tipoEpi.Id, HttpContext.Current.User.Identity.GetUserId(),
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), tipoEpi.IdEmpresa));

            return entity;
        }

        public IEnumerable<TipoEpiModel> BuscarPorNome(string nome)
        {
            int idEmpresa = Util.GetEmpresaId();
            return Db.TiposEpis.Where(t => t.Ativo).Where(t => t.IdEmpresa == idEmpresa).Where(t => t.Nome.StartsWith(nome)).OrderBy(t => t.Nome);
        }

        public IEnumerable<TipoEpiModel> BuscarAtivos()
        {
            int idEmpresa = Util.GetEmpresaId();

            return Db.TiposEpis.Where(t => t.Ativo).Where(t => t.IdEmpresa == idEmpresa).OrderBy(t => t.Nome);
        }
    }
}