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
    public class TipoUniformeRepository : RepositoryBase<TipoUniformeModel>, ITipoUniformeRepository
    {
        public override void Add(TipoUniformeModel tipoUniforme)
        {
            tipoUniforme.Ativo = true;
            tipoUniforme.IdEmpresa = Util.GetEmpresaId();

            Db.TiposUniformes.Add(tipoUniforme);

            Db.SaveChanges();

            Db.Database.ExecuteSqlCommand(string.Format(
                    "insert into [controlepi_hard].[logs] (entidade, operacao, id_reg, id_usuario, datahora, id_empresa) values('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');",
                    "TipoEpi", "insert", tipoUniforme.Id, HttpContext.Current.User.Identity.GetUserId(),
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), tipoUniforme.IdEmpresa));
        }

        public override TipoUniformeModel AddWRet(TipoUniformeModel tipoUniforme)
        {
            var entity = Db.Set<TipoUniformeModel>().Add(tipoUniforme);

            Db.SaveChanges();

            Db.Database.ExecuteSqlCommand(string.Format(
                    "insert into [controlepi_hard].[logs] (entidade, operacao, id_reg, id_usuario, datahora, id_empresa) values('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');",
                    "Setor", "insert", tipoUniforme.Id, HttpContext.Current.User.Identity.GetUserId(),
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), tipoUniforme.IdEmpresa));

            return entity;
        }

        public IEnumerable<TipoUniformeModel> BuscarPorNome(string nome)
        {
            int idEmpresa = Util.GetEmpresaId();
            return Db.TiposUniformes.Where(t => t.Ativo).Where(t => t.IdEmpresa == idEmpresa).Where(t => t.Nome.StartsWith(nome)).OrderBy(t => t.Nome);
        }

        public IEnumerable<TipoUniformeModel> BuscarAtivos()
        {
            int idEmpresa = Util.GetEmpresaId();

            return Db.TiposUniformes.Where(t => t.Ativo).Where(t => t.IdEmpresa == idEmpresa).OrderBy(t => t.Nome);
        }
    }
}