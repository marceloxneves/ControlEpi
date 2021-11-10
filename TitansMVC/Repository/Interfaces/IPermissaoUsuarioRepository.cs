using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TitansMVC.Models;

namespace TitansMVC.Repository.Interfaces
{
    public interface IPermissaoUsuarioRepository : IRepositoryBase<PermissaoUsuarioModel>
    {
        bool UsuarioIsMaster(UsuarioModel usuario);
    }
}