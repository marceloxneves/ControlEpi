using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TitansMVC.Models;
using TitansMVC.Repository.Interfaces;

namespace TitansMVC.Repository.Implementations
{
    public class PermissaoUsuarioRepository : RepositoryBase<PermissaoUsuarioModel>, IPermissaoUsuarioRepository
    {
        public bool UsuarioIsMaster(UsuarioModel usuario)
        {
            return Db.PermissoesUsuarios.Where(u => u.IdUsuario.Equals(usuario.Id)).Any(u => u.IdPermissao.Equals("5"));
            //comentário
        }
    }
}