using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TitansMVC.Models;

namespace TitansMVC.Repository.Interfaces
{
    public interface IPlanoRepository : IRepositoryBase<PlanoModel>
    {
        PlanoModel GetByCnpj(string cnpj);
        IEnumerable<PlanoModel> BuscarPorCnpj(string cnpj);
        IEnumerable<PlanoModel> BuscarTodosPlanos();
        PlanoModel CopiarPlanoServidorParaCliente(string cnpj);
        bool UpdatePlanoServidor(PlanoModel plano);

    }
}