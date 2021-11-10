using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.EnterpriseServices;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using TitansMVC.Models;
using TitansMVC.Repository.Interfaces;
using TitansMVC.Utils;

namespace TitansMVC.Repository.Implementations
{
    public class EpiRepository : RepositoryBase<EpiModel>, IEpiRepository
    {
        public override void Add(EpiModel epi)
        {
            epi.Ativo = true;
            epi.IdEmpresa = Util.GetEmpresaId();
            epi.DataCad = DateTime.Now;
            EstoqueEpi estEpi = new EstoqueEpi();
            estEpi.DataCad = DateTime.Now;
            estEpi.Epi = epi;
            estEpi.IdEmpresa = Util.GetEmpresaId();
            epi.Estoques.Add(estEpi);
            Db.Epis.Add(epi);
            Db.SaveChanges();
            Db.Database.ExecuteSqlCommand(string.Format(
                    "insert into [controlepi_hard].[logs] (entidade, operacao, id_reg, id_usuario, datahora, id_empresa) values('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');",
                    "Epi", "insert", epi.Id, HttpContext.Current.User.Identity.GetUserId(),
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), epi.IdEmpresa));
        }

        public override EpiModel AddWRet(EpiModel epi)
        {
            var entity = Db.Set<EpiModel>().Add(epi);
            Db.SaveChanges();
            Db.Database.ExecuteSqlCommand(string.Format(
                    "insert into [controlepi_hard].[logs] (entidade, operacao, id_reg, id_usuario, datahora, id_empresa) values('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');",
                    "Epi", "insert", epi.Id, HttpContext.Current.User.Identity.GetUserId(),
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), epi.IdEmpresa));

            return entity;
        }

        public IEnumerable<EpiModel> BuscarPorId(int id)
        {
            return Db.Epis.Where(c => c.Ativo).Where(e => e.Id.Equals(id));
        }

        public IEnumerable<EpiModel> BuscarPorTipo(int idTipo)
        {
            int idEmpresa = Util.GetEmpresaId();

            return Db.Epis.Where(c => c.Ativo).Where(e => e.IdEmpresa == idEmpresa).Where(e => e.TipoEpiId == idTipo);
        }

        public IEnumerable<EpiModel> BuscarPorNome(string nome,bool ativos)
        {
            int idEmpresa = Util.GetEmpresaId();

            return Db.Epis.Where(c => c.Ativo.Equals(ativos)).Where(e => e.IdEmpresa == idEmpresa).Where(e => e.Nome.StartsWith(nome)).OrderBy(e => e.Nome);
        }

        public IEnumerable<EpiModel> GetAll(bool ativos)
        {
            return ativos ? this.BuscarAtivos() : this.BuscarInativos();
        }

        public override IEnumerable<EpiModel> GetAll()
        {
            int idEmpresa = Util.GetEmpresaId();
            return Db.Epis.Where(c => c.IdEmpresa == idEmpresa).OrderBy(c => c.Nome);

        }

        public IEnumerable<EpiModel> BuscarAtivos()
        {
            int idEmpresa = Util.GetEmpresaId();
            return Db.Epis.Where(e => e.Ativo).Where(e => e.IdEmpresa == idEmpresa).OrderBy(e => e.Nome);
        }

        public IEnumerable<EpiModel> BuscarInativos()
        {
            int idEmpresa = Util.GetEmpresaId();
            return Db.Epis.Where(e => !e.Ativo).Where(e => e.IdEmpresa == idEmpresa).OrderBy(e => e.Nome);
        }

        public IEnumerable<EpiModel> BuscarCaVencidos()
        {
            int idEmpresa = Util.GetEmpresaId();
            return Db.Epis.Where(c => c.Ativo).Where(e => e.IdEmpresa == idEmpresa).Where(e => e.ValidadeCa.Value.CompareTo(DateTime.Now) <= 0).OrderBy(e => e.Nome).ToList();
        }

        public IEnumerable<EpiModel> BuscarCaAVencer(int dias)
        {
            int idEmpresa = Util.GetEmpresaId();
            //resolvererroepi
            DateTime dataAAvisar = DateTime.Now.AddDays(dias).Date;
            return Db.Epis.Where(c => c.Ativo).Where(e => e.IdEmpresa == idEmpresa).Where(e => DbFunctions.TruncateTime(e.ValidadeCa) <= dataAAvisar && DbFunctions.TruncateTime(e.ValidadeCa) > DbFunctions.TruncateTime(DateTime.Now)).OrderBy(e => e.Nome).ToList();
        }

        public bool EstaEmSetor(int idEpi)
        {
            int qtde = Db.EpisSetores.Count(e => e.EpiId == idEpi);
            return qtde > 0;
        }

        public EpiModel GetByIdEager(int id)
        {
            return Db.Epis/*.Where(e => e.Ativo)*/.Where(e => e.Id == id).Include(e => e.Estoques).SingleOrDefault();
        }

        public EpiModel BuscarPorCodigoDeBarras(string codigoDeBarras) 
        {
            int idEmpresa = Util.GetEmpresaId();

            return Db.Epis.Include(e => e.TipoEpi).Where(c => c.Ativo).Where(e => e.IdEmpresa == idEmpresa).Where(e => e.CodigoDeBarras.Equals(codigoDeBarras)).FirstOrDefault();                        
        }

        public EpiModel ValidarCodigoDeBarras(string codigoDeBarras, int idEpi)
        {
            int idEmpresa = Util.GetEmpresaId();

            //aceitar campo nulo
            if (codigoDeBarras == null) return null;

            if (idEpi == 0)
            {   
                //validar na tela de create
                return Db.Epis.Include(e => e.TipoEpi).Where(c => c.Ativo).Where(e => e.IdEmpresa == idEmpresa).Where(e => e.CodigoDeBarras.Equals(codigoDeBarras)).FirstOrDefault();
            }
            else
            {
                //validar na tela de edição
                return Db.Epis.Include(e => e.TipoEpi).Where(c => c.Ativo).Where(e => e.IdEmpresa == idEmpresa).Where(e => e.CodigoDeBarras.Equals(codigoDeBarras)).Where(e => e.Id != idEpi).FirstOrDefault();
            }

        }

        public EpiModel GetByCa(string ca)
        {
            int idEmpresa = Util.GetEmpresaId();
            return Db.Epis.Where(e => e.IdEmpresa == idEmpresa).FirstOrDefault(e => e.Ca.Equals(ca));
        }

        //verificar se já existe por nome e por marca
        public EpiModel GetByNomeAndMarca(string nome, string marca)
        {
            int idEmpresa = Util.GetEmpresaId();
            
            return Db.Epis.Where(e => e.IdEmpresa == idEmpresa).Where(e => e.Nome.Equals(nome)).Where(e => e.Marca.Equals(marca)).FirstOrDefault();
        }

    }
}