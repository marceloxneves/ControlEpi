using System;
using System.Collections.Generic;
using TitansMVC.Models;

namespace TitansMVC.Repository.Interfaces
{
    public interface IFuncaoRepository : IRepositoryBase<FuncaoModel>
    {
        //SetorModel GetByIdEager(int id);
        IEnumerable<FuncaoModel> BuscarPorNome(string nome);
        //FuncaoModel BuscarPorNomeUnico(string nome);
        //int BuscarIdSetor(string nome);
        IEnumerable<FuncaoModel> BuscarAtivos();
        //IEnumerable<SetorModel> BuscarPorUnidadeNegocio(int id);
        //int QtdeColaboradores(int idSetor);
    }
}
