using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TitansMVC.Models;

namespace TitansMVC.Repository.Interfaces
{
    public interface IColaboradorRepository : IRepositoryBase<ColaboradorModel>
    {
        ColaboradorModel GetByIdEager(int id);
        IEnumerable<ColaboradorModel> BuscarPorId(int id);
        IEnumerable<ColaboradorModel> BuscarPorNome(string nome, int? UnidadeNegocioId,bool ativo);
        IEnumerable<ColaboradorModel> BuscarPorCpf(string cpf, int? UnidadeNegocioId,bool ativo);
        IEnumerable<ColaboradorModel> BuscarPorCpf(string cpf);
        IEnumerable<ColaboradorModel> BuscarPorUnidadeNegocio(int id,bool ativo);
        IEnumerable<ColaboradorModel> BuscarPorUnidadeNegocio(int id);
        IEnumerable<ColaboradorModel> BuscarAtivos();
        int ContarColaboradores(int idEmpresa);
        IEnumerable<ColaboradorModel> GetAll(bool ativos);
    }
}
