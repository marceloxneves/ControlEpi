using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TitansMVC.Repository.Interfaces
{
    public interface IRepositoryBase<TEntity> where TEntity : class
    {
        void Add(TEntity obj);
        TEntity AddWRet(TEntity obj);
        TEntity GetById(int id);
        TEntity GetById(string id);
        IEnumerable<TEntity> GetAll();
        void Update(TEntity obj);
        void Remove(TEntity obj);
        void RemoveLogical(int id);
        void RemoveLogical(string id);
        void RetirarDoContexto(TEntity obj);
        IEnumerable<TEntity> ConsultaEntidades(string consulta);
        void Dispose();
    }
}
