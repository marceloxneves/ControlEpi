using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TitansMVC.Models;
using TitansMVC.Repository.Interfaces;

namespace TitansMVC.Repository.Implementations
{
    public class RoleRepository : RepositoryBase<RoleModel>, IRoleRepository
    {
        public IEnumerable<RoleModel> BuscarAtivos()
        {
            return Db.Roles.Where(r => r.Ativo);
        }

        public IEnumerable<RoleModel> BuscarPermissoesAdmin()
        {
            return Db.Roles.Where(r => r.Ativo).Where(r => r.Nome.ToLower().Trim() != "role_master");
        }
    }
}