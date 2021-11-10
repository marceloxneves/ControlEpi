using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TitansMVC.Models;

namespace TitansMVC.Repository.Interfaces
{
    public interface IRoleRepository : IRepositoryBase<RoleModel>
    {
        IEnumerable<RoleModel> BuscarAtivos();
        IEnumerable<RoleModel> BuscarPermissoesAdmin();
    }
}
