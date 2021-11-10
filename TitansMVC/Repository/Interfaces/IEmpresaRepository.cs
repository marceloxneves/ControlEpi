using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TitansMVC.Models;

namespace TitansMVC.Repository.Interfaces
{
    public interface IEmpresaRepository : IRepositoryBase<EmpresaModel>
    {
        EmpresaModel GetByIdEager(int idEmpresa);        
        IEnumerable<EmpresaModel> BuscarAtivos();
        IEnumerable<EmpresaModel> BuscarPorRazao(string razao);
        IEnumerable<EmpresaModel> BuscarPorCnpj(string cnpj);
        bool ExisteEmpresaMesmoCnpj(string cnpj);
        int? PegarNovoNumOs(int idEmpresa);
        EmpresaModel Atualizar(EmpresaModel empresa);
    }
}
