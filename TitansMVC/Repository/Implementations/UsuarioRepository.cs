using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TitansMVC.Repository.Interfaces;
using TitansMVC.Models;
using TitansMVC.Utils;

namespace TitansMVC.Repository.Implementations
{
    public class UsuarioRepository : RepositoryBase<UsuarioModel>, IUsuarioRepository
    {
        public override UsuarioModel GetById(string id)
        {
            return Db.Usuarios.FirstOrDefault(u => u.Id == id);
        }

        public UsuarioModel GetByIdEager(string idUsuario)
        {
            return Db.Usuarios/*.Where(c => c.Ativo)*/.Where(u => u.Id == idUsuario).Include(u => u.Permissoes).FirstOrDefault();
        }

        public IEnumerable<UsuarioModel> BuscarAtivos()
        {
            int idEmpresa = Util.GetEmpresaId();

            if (HttpContext.Current.User.IsInRole("role_master"))
            {
                return Db.Usuarios.Where(u => u.Ativo).OrderBy(u => u.Username).ToList();
            }

            return Db.Usuarios.Where(u => u.Ativo).Where(u => u.IdEmpresa == idEmpresa).OrderBy(u => u.Username).ToList();
        }

        public IEnumerable<UsuarioModel> BuscarPorUsername(string username)
        {
            int idEmpresa = Util.GetEmpresaId();

            return Db.Usuarios.Where(c => c.Ativo).Where(u => u.IdEmpresa == idEmpresa).Where(u => u.Username.StartsWith(username)).ToList();
        }

        public IEnumerable<UsuarioModel> BuscarPorUsername(string username, bool ativo)
        {
            
            var result = Db.Usuarios.Where(u=>u.Ativo == ativo );
            if (!HttpContext.Current.User.IsInRole("role_master"))
            {
                int idEmpresa = Util.GetEmpresaId();
                result = result.Where(u => u.IdEmpresa == idEmpresa);
            }
            return result.Where(u => u.Username.StartsWith(username)).OrderBy(u=>u.Username);
        }

        public IEnumerable<UsuarioModel> BuscarPorEmail(string email, bool ativo)
        {

            var result = Db.Usuarios.Where(u => u.Ativo == ativo);
            if (!HttpContext.Current.User.IsInRole("role_master"))
            {
                int idEmpresa = Util.GetEmpresaId();
                result = result.Where(u => u.IdEmpresa == idEmpresa);
            }
            return result.Where(u => u.Email.StartsWith(email)).OrderBy(u => u.Email);
        }

        public IEnumerable<UsuarioModel> GetAll(bool ativo)
        {

            var result = Db.Usuarios.Where(u => u.Ativo == ativo);
            if (!HttpContext.Current.User.IsInRole("role_master"))
            {
                int idEmpresa = Util.GetEmpresaId();
                result = result.Where(u => u.IdEmpresa == idEmpresa);
            }
            return result.OrderBy(u => u.Email);
        }


        public IEnumerable<UsuarioModel> BuscarPorEmail(string email)
        {
            int idEmpresa = Util.GetEmpresaId();

            return Db.Usuarios.Where(c => c.Ativo).Where(u => u.IdEmpresa == idEmpresa).Where(u => u.Email.StartsWith(email)).OrderBy(u => u.Username);
        }

        public bool ExisteUsuarioMesmoEmail(string email) 
        {
            return Db.Usuarios.Where(u => u.Ativo).Any(u => u.Email == email);
        }
    }
}