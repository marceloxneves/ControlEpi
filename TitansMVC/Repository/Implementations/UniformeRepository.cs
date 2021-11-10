using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using TitansMVC.Models;
using TitansMVC.Repository.Interfaces;
using TitansMVC.Utils;

namespace TitansMVC.Repository.Implementations
{
    public class UniformeRepository : RepositoryBase<UniformeModel>, IUniformeRepository
    {
        public override void Add(UniformeModel uniforme)
        {
            uniforme.Ativo = true;
            uniforme.IdEmpresa = Util.GetEmpresaId();
            uniforme.DataCad = DateTime.Now;
            EstoqueUniforme estUniforme = new EstoqueUniforme();
            estUniforme.DataCad = DateTime.Now;
            estUniforme.Uniforme = uniforme;
            estUniforme.IdEmpresa = Util.GetEmpresaId();
            uniforme.Estoques.Add(estUniforme);
            Db.Uniformes.Add(uniforme);
            Db.SaveChanges();
            Db.Database.ExecuteSqlCommand(string.Format(
                    "insert into [controlepi_hard].[logs] (entidade, operacao, id_reg, id_usuario, datahora, id_empresa) values('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');",
                    "Uniforme", "insert", uniforme.Id, HttpContext.Current.User.Identity.GetUserId(),
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), uniforme.IdEmpresa));
        }

        public override UniformeModel AddWRet(UniformeModel uniforme)
        {
            var entity = Db.Set<UniformeModel>().Add(uniforme);
            Db.SaveChanges();
            Db.Database.ExecuteSqlCommand(string.Format(
                    "insert into [controlepi_hard].[logs] (entidade, operacao, id_reg, id_usuario, datahora, id_empresa) values('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');",
                    "Uniforme", "insert", uniforme.Id, HttpContext.Current.User.Identity.GetUserId(),
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), uniforme.IdEmpresa));

            return entity;
        }

        public IEnumerable<UniformeModel> BuscarPorId(int id)
        {
            return Db.Uniformes.Where(c => c.Ativo).Where(e => e.Id.Equals(id));
        }

        public IEnumerable<UniformeModel> BuscarPorTipo(int idTipo)
        {
            int idEmpresa = Util.GetEmpresaId();

            return Db.Uniformes.Where(c => c.Ativo).Where(e => e.IdEmpresa == idEmpresa).Where(e => e.TipoUniformeId == idTipo);
        }

        public IEnumerable<UniformeModel> BuscarPorNome(string nome,bool ativos)
        {
            int idEmpresa = Util.GetEmpresaId();

            return Db.Uniformes.Where(c => c.Ativo.Equals(ativos)).Where(e => e.IdEmpresa == idEmpresa).Where(e => e.Nome.StartsWith(nome)).OrderBy(e => e.Nome);
        }

        public IEnumerable<UniformeModel> GetAll(bool ativos)
        {
            return ativos ? this.BuscarAtivos() : this.BuscarInativos();
        }

        public override IEnumerable<UniformeModel> GetAll()
        {
            int idEmpresa = Util.GetEmpresaId();
            return Db.Uniformes.Where(c => c.IdEmpresa == idEmpresa).OrderBy(c => c.Nome);

        }

        public IEnumerable<UniformeModel> BuscarAtivos()
        {
            int idEmpresa = Util.GetEmpresaId();
            return Db.Uniformes.Where(e => e.Ativo).Where(e => e.IdEmpresa == idEmpresa).OrderBy(e => e.Nome);
        }

        public IEnumerable<UniformeModel> BuscarInativos()
        {
            int idEmpresa = Util.GetEmpresaId();
            return Db.Uniformes.Where(e => !e.Ativo).Where(e => e.IdEmpresa == idEmpresa).OrderBy(e => e.Nome);
        }

       
        public bool EstaEmSetor(int idUniforme)
        {
            int qtde = Db.UniformesSetores.Count(e => e.UniformeId == idUniforme);
            return qtde > 0;
        }

        public UniformeModel GetByIdEager(int id)
        {
            return Db.Uniformes/*.Where(e => e.Ativo)*/.Where(e => e.Id == id).Include(e => e.Estoques).SingleOrDefault();
        }

        public UniformeModel BuscarPorCodigoDeBarras(string codigoDeBarras) 
        {
            int idEmpresa = Util.GetEmpresaId();

            return Db.Uniformes.Include(e => e.TipoUniforme).Where(c => c.Ativo).Where(e => e.IdEmpresa == idEmpresa).Where(e => e.CodigoDeBarras.Equals(codigoDeBarras)).FirstOrDefault();                        
        }

        public UniformeModel ValidarCodigoDeBarras(string codigoDeBarras, int idUniforme)
        {
            int idEmpresa = Util.GetEmpresaId();

            //aceitar campo nulo
            if (codigoDeBarras == null) return null;

            if (idUniforme == 0)
            {   
                //validar na tela de create
                return Db.Uniformes.Include(e => e.TipoUniforme).Where(c => c.Ativo).Where(e => e.IdEmpresa == idEmpresa).Where(e => e.CodigoDeBarras.Equals(codigoDeBarras)).FirstOrDefault();
            }
            else
            {
                //validar na tela de edição
                return Db.Uniformes.Include(e => e.TipoUniforme).Where(c => c.Ativo).Where(e => e.IdEmpresa == idEmpresa).Where(e => e.CodigoDeBarras.Equals(codigoDeBarras)).Where(e => e.Id != idUniforme).FirstOrDefault();
            }

        }

        //verificar se já existe por nome e por marca
        public UniformeModel GetByNomeAndMarca(string nome, string marca)
        {
            int idEmpresa = Util.GetEmpresaId();
            
            return Db.Uniformes.Where(e => e.IdEmpresa == idEmpresa).Where(e => e.Nome.Equals(nome)).Where(e => e.Marca.Equals(marca)).FirstOrDefault();
        }

    }
}