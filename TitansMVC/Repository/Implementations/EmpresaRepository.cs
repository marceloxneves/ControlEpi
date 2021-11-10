using System;
using System.Collections.Generic;
using System.Linq;
using TitansMVC.Models;
using TitansMVC.Repository.Interfaces;
using System.Data.Entity;
using System.Web;
using Microsoft.AspNet.Identity;
using TitansMVC.Utils;

namespace TitansMVC.Repository.Implementations
{
    public class EmpresaRepository : RepositoryBase<EmpresaModel>, IEmpresaRepository
    {
        public override void Add(EmpresaModel empresa)
        {
            empresa.Ativo = true;
            empresa.PossuiBiometria = true;
            Db.Empresas.Add(empresa);            

            Db.Configuracoes.Add(new ConfiguracaoModel()
            {
                AvisarAposVencCa = false,
                AvisarAposVencEpi = false,
                AvisarVencCaComAntec = false,
                AvisarVencEpiComAntec = false,
                BloquearPorTipoEpiUnico = false,
                IdEmpresa = empresa.Id,
                QtdeDiasAvisoVencCa = 0,
                QtdeDiasAvisoVencEpi = 0,
                AvisarAposVencUniforme = false,
                AvisarVencUniformeComAntec = false,
                BloquearPorTipoUniformeUnico = false,
                QtdeDiasAvisoVencUniforme = 0
            });

            Db.SaveChanges();

            Db.Database.ExecuteSqlCommand(string.Format(
                    "insert into [controlepi_hard].[logs] (entidade, operacao, id_reg, id_usuario, datahora) values('{0}', '{1}', '{2}', '{3}', '{4}');",
                    "CentroCusto", "insert", empresa.Id, HttpContext.Current.User.Identity.GetUserId(),
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
        }

        public override EmpresaModel AddWRet(EmpresaModel empresa)
        {
            empresa.PossuiBiometria = true;
            var entity = Db.Set<EmpresaModel>().Add(empresa);
            
            Db.Configuracoes.Add(new ConfiguracaoModel()
            {
                AvisarAposVencCa = false,
                AvisarAposVencEpi = false,
                AvisarVencCaComAntec = false,
                AvisarVencEpiComAntec = false,
                BloquearPorTipoEpiUnico = false,
                IdEmpresa = empresa.Id,
                QtdeDiasAvisoVencCa = 0,
                QtdeDiasAvisoVencEpi = 0,
                AvisarAposVencUniforme = false,
                AvisarVencUniformeComAntec = false,
                BloquearPorTipoUniformeUnico = false,
                QtdeDiasAvisoVencUniforme = 0
            });

            Db.SaveChanges();

            Db.Database.ExecuteSqlCommand(string.Format(
                    "insert into [controlepi_hard].[logs] (entidade, operacao, id_reg, id_usuario, datahora) values('{0}', '{1}', '{2}', '{3}', '{4}');",
                    "Empresa", "insert", empresa.Id, HttpContext.Current.User.Identity.GetUserId(),
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));

            return entity;
        }

        public EmpresaModel GetByIdEager(int idEmpresa)
        {
            return Db.Set<EmpresaModel>().Include(e => e.Telefones).Include(e => e.Emails).Where(c => c.Ativo).FirstOrDefault(e => e.Id == idEmpresa);
        }

        public IEnumerable<EmpresaModel> BuscarAtivos()
        {
            int idEmpresa = Util.GetEmpresaId();

            if (HttpContext.Current.User.IsInRole("role_master"))
            {
                return Db.Empresas.Where(e => e.Ativo).OrderBy(e => e.Razao);
            }

            return Db.Empresas.Where(e => e.Ativo).Where(e => e.Id == idEmpresa).OrderBy(e => e.Razao);
            
        }

        public IEnumerable<EmpresaModel> BuscarPorCnpj(string cnpj)
        {
            int idEmpresa = Util.GetEmpresaId();

            return Db.Empresas.Where(e => e.Ativo).Where(e => e.Id == idEmpresa).Where(e => e.Cnpj == cnpj);
        }

        public IEnumerable<EmpresaModel> BuscarPorRazao(string razao)
        {
            int idEmpresa = Util.GetEmpresaId();

            return Db.Empresas.Where(e => e.Ativo).Where(e => e.Razao.StartsWith(razao)).OrderBy(e=>e.Razao);
        }

        public bool ExisteEmpresaMesmoCnpj(string cnpj)
        {
            return Db.Empresas.Where(e => e.Ativo).Any(e => e.Cnpj == cnpj);
        }

        public int? PegarNovoNumOs(int idEmpresa)
        {
            var empresa = Db.Set<EmpresaModel>().Find(idEmpresa);

            var proxNumOs = empresa.ProxNumOs;

            empresa.ProxNumOs = empresa.ProxNumOs + 1;
            this.Update(empresa);
            return proxNumOs;
        }

        public EmpresaModel Atualizar(EmpresaModel empresa)
        {
            Db.Entry(empresa).Reload();
            return empresa;
        }
    }
}