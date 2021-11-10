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
    public class FamiliaEpiRepository : RepositoryBase<FamiliaEpiModel>, IFamiliaEpiRepository
    {
        public override void Add(FamiliaEpiModel familiaEpi)
        {
            familiaEpi.Ativo = true;
            familiaEpi.IdEmpresa = Util.GetEmpresaId();
            familiaEpi.DataCad = DateTime.Now;

            Db.FamiliasEpi.Add(familiaEpi);

            Db.SaveChanges();

            Db.Database.ExecuteSqlCommand(string.Format(
                    "insert into [controlepi_hard].[logs] (entidade, operacao, id_reg, id_usuario, datahora, id_empresa) values('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');",
                    "FamiliaEpi", "insert", familiaEpi.Id, HttpContext.Current.User.Identity.GetUserId(),
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), familiaEpi.IdEmpresa));
        }

        public override FamiliaEpiModel AddWRet(FamiliaEpiModel familiaEpi)
        {
            var entity = Db.Set<FamiliaEpiModel>().Add(familiaEpi);

            Db.SaveChanges();

            Db.Database.ExecuteSqlCommand(string.Format(
                    "insert into [controlepi_hard].[logs] (entidade, operacao, id_reg, id_usuario, datahora, id_empresa) values('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');",
                    "FamiliaEpi", "insert", familiaEpi.Id, HttpContext.Current.User.Identity.GetUserId(),
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), familiaEpi.IdEmpresa));

            return entity;
        }

        public IEnumerable<FamiliaEpiModel> BuscarPorNome(string nome)
        {
            int idEmpresa = Util.GetEmpresaId();

            return Db.FamiliasEpi.Where(f => f.Ativo).Where(f => f.IdEmpresa == idEmpresa).Where(f => f.Nome.StartsWith(nome)).OrderBy(f => f.Nome);
        }

        public IEnumerable<FamiliaEpiModel> BuscarAtivos()
        {
            int idEmpresa = Util.GetEmpresaId();

            return Db.FamiliasEpi.Where(f => f.Ativo).Where(f => f.IdEmpresa == idEmpresa).OrderBy(f => f.Nome);
        }
    }
}