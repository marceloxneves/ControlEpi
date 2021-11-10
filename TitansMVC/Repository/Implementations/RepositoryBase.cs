using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using Microsoft.AspNet.Identity;
using TitansMVC.Context;
using TitansMVC.Models.Relatorios;
using TitansMVC.Repository.Interfaces;

namespace TitansMVC.Repository.Implementations
{
    public class RepositoryBase<TEntity> : IDisposable, IRepositoryBase<TEntity> where TEntity : class
    {
        protected TitansContext Db = new TitansContext();
        public virtual void Add(TEntity obj)
        {
            Db.Set<TEntity>().Add(obj);

            //Db.Entry(obj).State = EntityState.Added;
            // implantar padrao Unit of Work posteriormente
            Db.SaveChanges();

            var idObjReflection = obj.GetType().GetProperty("Id");
            int id = (int)idObjReflection.GetValue(obj, null);

            Db.Database.ExecuteSqlCommand(string.Format(
                    "insert into [controlepi_hard].[logs] (entidade, operacao, id_reg, id_usuario, datahora) values('{0}', '{1}', '{2}', '{3}', '{4}');",
                    obj.GetType().Name, "insert", id, HttpContext.Current.User.Identity.GetUserId(),
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
        }

        public virtual TEntity AddWRet(TEntity obj)
        {
            var entity = Db.Set<TEntity>().Add(obj);

            //Db.Entry(obj).State = EntityState.Added;
            // implantar padrao Unit of Work posteriormente
            Db.SaveChanges();

            if (obj.GetType().Name != "Usuario")
            {
                var idObjReflection = obj.GetType().GetProperty("Id");
                int id = (int)idObjReflection.GetValue(obj, null);

                Db.Database.ExecuteSqlCommand(string.Format(
                    "insert into [controlepi_hard].[logs] (entidade, operacao, id_reg, id_usuario, datahora) values('{0}', '{1}', '{2}', '{3}', '{4}');",
                    obj.GetType().Name, "insert", id, HttpContext.Current.User.Identity.GetUserId(),
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
            else
            {
                var idObjReflection = obj.GetType().GetProperty("Id");
                string id = (string)idObjReflection.GetValue(obj, null);

                Db.Database.ExecuteSqlCommand(string.Format(
                    "insert into [controlepi_hard].[logs] (entidade, operacao, id_reg, id_usuario, datahora) values('{0}', '{1}', '{2}', '{3}', '{4}');",
                    obj.GetType().Name, "insert", id, HttpContext.Current.User.Identity.GetUserId(),
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }

            return entity;
        }

        public TEntity GetById(int id)
        {
            return Db.Set<TEntity>().Find(id);
        }

        public virtual TEntity GetById(string id)
        {
            var entity = Db.Set<TEntity>().Find(id);

            Db.Entry(entity).Reload();

            return entity;
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            // procurar desabilitar tracking (no tracking) de entidades no EF
            // conforme um exemplo em meus favoritos
            return Db.Set<TEntity>().ToList();
        }

        public virtual void Update(TEntity obj)
        {
            Db.Entry(obj).State = EntityState.Modified;
            Db.SaveChanges();

            var idObjReflection = obj.GetType().GetProperty("Id");

            object id;

            if (obj.GetType().GetProperty("Id").PropertyType.Name == "Int32")
            {
                id = (int)idObjReflection.GetValue(obj, null);
            }
            else
            {
                id = (string)idObjReflection.GetValue(obj, null);
            }

            Db.Database.ExecuteSqlCommand(string.Format(
                    "insert into [controlepi_hard].[logs] (entidade, operacao, id_reg, id_usuario, datahora) values('{0}', '{1}', '{2}', '{3}', '{4}');",
                    obj.GetType().Name, "update", id, HttpContext.Current.User.Identity.GetUserId(),
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            
        }

        public void Remove(TEntity obj)
        {
            Db.Set<TEntity>().Remove(obj);
            Db.SaveChanges();

            var idObjReflection = obj.GetType().GetProperty("Id");
            int id = (int)idObjReflection.GetValue(obj, null);

            Db.Database.ExecuteSqlCommand(string.Format(
                    "insert into [controlepi_hard].[logs] (entidade, operacao, id_reg, id_usuario, datahora) values('{0}', '{1}', '{2}', '{3}', '{4}');",
                    obj.GetType().Name, "remove", id, HttpContext.Current.User.Identity.GetUserId(),
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
        }

        public void RemoveLogical(int id)
        {
            var obj = this.GetById(id);
            obj.GetType().GetProperty("Ativo").SetValue(obj, false);
            Update(obj);
            Db.SaveChanges();

            var idObjReflection = obj.GetType().GetProperty("Id");
            int idObj = (int)idObjReflection.GetValue(obj, null);

            Db.Database.ExecuteSqlCommand(string.Format(
                    "insert into [controlepi_hard].[logs] (entidade, operacao, id_reg, id_usuario, datahora) values('{0}', '{1}', '{2}', '{3}', '{4}');",
                    obj.GetType().Name, "remove-logical", idObj, HttpContext.Current.User.Identity.GetUserId(),
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
        }

        public void RemoveLogical(string id)
        {
            var obj = this.GetById(id);
            obj.GetType().GetProperty("Ativo").SetValue(obj, false);
            Update(obj);
            Db.SaveChanges();

            var idObjReflection = obj.GetType().GetProperty("Id");
            string idObj = (string)idObjReflection.GetValue(obj, null);

            Db.Database.ExecuteSqlCommand(string.Format(
                    "insert into [controlepi_hard].[logs] (entidade, operacao, id_reg, id_usuario, datahora) values('{0}', '{1}', '{2}', '{3}', '{4}');",
                    obj.GetType().Name, "remove-logical", idObj, HttpContext.Current.User.Identity.GetUserId(),
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
        }

        public void RetirarDoContexto(TEntity obj)
        {
            Db.Entry(obj).State = EntityState.Detached;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> ConsultaEntidades(string consulta)
        {
            ObjectResult<TEntity> objs =
            (Db as IObjectContextAdapter).ObjectContext.ExecuteStoreQuery<TEntity>(consulta);
            return objs.ToList();         
        }
    }
}