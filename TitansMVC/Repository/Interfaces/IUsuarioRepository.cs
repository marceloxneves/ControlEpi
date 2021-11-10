using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TitansMVC.Models;

namespace TitansMVC.Repository.Interfaces
{
    public interface IUsuarioRepository : IRepositoryBase<UsuarioModel>
    {
        UsuarioModel GetByIdEager(string idUsuario);
        IEnumerable<UsuarioModel> BuscarAtivos();
        IEnumerable<UsuarioModel> BuscarPorUsername(string username);
        IEnumerable<UsuarioModel> BuscarPorEmail(string email);
        bool ExisteUsuarioMesmoEmail(string email);
        IEnumerable<UsuarioModel> BuscarPorEmail(string email, bool ativo);
        IEnumerable<UsuarioModel> BuscarPorUsername(string username, bool ativo);
        IEnumerable<UsuarioModel> GetAll(bool ativo);


    }
}
