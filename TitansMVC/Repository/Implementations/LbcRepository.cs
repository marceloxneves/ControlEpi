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
    public class LbcRepository : RepositoryBase<LbcModel>, ILbcRepository
    {
        public override void Add(LbcModel lbc)
        {
            lbc.Ativo = true;
            lbc.IdEmpresa = Util.GetEmpresaId();
            lbc.DataCad = DateTime.Now;

            Db.Lbcs.Add(lbc);

            Db.SaveChanges();

            Db.Database.ExecuteSqlCommand(string.Format(
                    "insert into [controlepi_hard].[logs] (entidade, operacao, id_reg, id_usuario, datahora, id_empresa) values('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');",
                    "Unidade de Negócio", "insert", lbc.Id, HttpContext.Current.User.Identity.GetUserId(),
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), lbc.IdEmpresa));
        }

        public override LbcModel AddWRet(LbcModel lbc)
        {
            var entity = Db.Set<LbcModel>().Add(lbc);

            Db.SaveChanges();

            Db.Database.ExecuteSqlCommand(string.Format(
                    "insert into [controlepi_hard].[logs] (entidade, operacao, id_reg, id_usuario, datahora, id_empresa) values('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');",
                    "Unidade de Negócio", "insert", lbc.Id, HttpContext.Current.User.Identity.GetUserId(),
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), lbc.IdEmpresa));

            return entity;
        }

        public IEnumerable<LbcModel> BuscarPorNome(string nome)
        {
            int idEmpresa = Util.GetEmpresaId();

            return Db.Lbcs.Where(c => c.Ativo).Where(l => l.IdEmpresa == idEmpresa).Where(l => l.Nome.StartsWith(nome)).OrderBy(l => l.Nome);
        }

        public IEnumerable<LbcModel> BuscarAtivos()
        {
            int idEmpresa = Util.GetEmpresaId();

            return Db.Lbcs.Where(l => l.Ativo).Where(l => l.IdEmpresa == idEmpresa).OrderBy(l => l.Nome);
        }

        public LbcModel GetByCnpj(string cnpj)
        {
            return Db.Lbcs.FirstOrDefault(l => l.Cnpj.Equals(cnpj));
        }

        public int QtdeCentrosCustos(int idLbc)
        {
            return Db.Lbcs.Count(l => l.Id == idLbc && l.CentrosDeCustos.Any());
        }
    }
}