using System;
using System.Collections.Generic;
using TitansMVC.Models;

namespace TitansMVC.Repository.Interfaces
{
    public interface ISetorRepository : IRepositoryBase<SetorModel>
    {
        SetorModel GetByIdEager(int id);
        IEnumerable<SetorModel> BuscarPorNome(string nome, int? UnidadeNegocioId);
        SetorModel BuscarPorNomeUnico(string nome);
        int BuscarIdSetor(string nome);
        IEnumerable<SetorModel> BuscarAtivos();
        IEnumerable<SetorModel> BuscarPorUnidadeNegocio(int id);
        int QtdeColaboradores(int idSetor);        
    }
}
