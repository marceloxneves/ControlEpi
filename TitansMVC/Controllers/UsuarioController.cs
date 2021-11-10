using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using PagedList;
using TitansMVC.Identity;
using TitansMVC.Repository.Implementations;
using TitansMVC.Repository.Interfaces;
using TitansMVC.Models;
using TitansMVC.Utils;
using System.Collections;
using System.Collections.Generic;

namespace TitansMVC.Controllers
{
    //ok
    [Authorize(Roles = "role_admin, role_master")]
    public class UsuarioController : BaseController
    {
        private ApplicationUserManager _userManager;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IPermissaoUsuarioRepository _permissaoUsuarioRepository;
        private readonly IRoleRepository _roleRepository;

        public UsuarioController()
        {
            _usuarioRepository = new UsuarioRepository();
            _permissaoUsuarioRepository = new PermissaoUsuarioRepository();
            _roleRepository = new RoleRepository();
        }

        // GET: Usuario
        public ActionResult Index(bool? ativo,string searchBy, string search, int? page)
        {
            //var usuariosView = _usuarioRepository.BuscarAtivos();
            IEnumerable<UsuarioModel> usuariosView;
            if (searchBy == "Username")
            {
                usuariosView = _usuarioRepository.BuscarPorUsername(username: search,ativo:ativo.HasValue ? ativo.Value : true);
            }else
            if (searchBy == "Email")
            {
                usuariosView = _usuarioRepository.BuscarPorEmail(email: search, ativo: ativo.HasValue ? ativo.Value : true);
            }
            else
            {
                usuariosView = _usuarioRepository.GetAll(ativo: ativo.HasValue ? ativo.Value : true);
            }
            return View(usuariosView.ToPagedList(page ?? 1, 10));
        }

        // GET: Setor/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Setor/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(UsuarioModel usuario)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser()
                {
                    UserName = usuario.Email,
                    Email = usuario.Email,
                    Active = true,
                    EmailConfirmed = true,
                    IdEmpresa = Util.GetEmpresaId()
                };

                var result = await UserManager.CreateAsync(user, usuario.Password);
                if (result.Succeeded)
                {
                    EmailService email = new EmailService();
                    await email.SendEmailUser(usuario);

                    Success(String.Format("Registro gravado com sucesso!"), true);

                    return RedirectToAction("Edit", user);
                }
                else
                {
                    foreach(var error in result.Errors)
                    {
                        Warning(error,true);
                    }
                }
            }

            // No caso de falha, reexibir a view. 
            return View(usuario);
        }

        // GET: Usuario/Edit/5
        public ActionResult Edit(string id)
        {
            var usuario = _usuarioRepository.GetByIdEager(id);
            ViewBag.RoleId = new SelectList(_roleRepository.BuscarPermissoesAdmin(), "Id", "Descricao");
            ViewBag.IsMaster = _permissaoUsuarioRepository.UsuarioIsMaster(usuario);
            return View(usuario);
        }

        // POST: Usuario/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UsuarioModel usuario)
        {
            ViewBag.RoleId = new SelectList(_roleRepository.BuscarPermissoesAdmin(), "Id", "Descricao");

            if (ModelState.IsValid)
            {
                _usuarioRepository.Update(usuario);

                usuario = _usuarioRepository.GetByIdEager(usuario.Id);

                Success(String.Format("Registro alterado com sucesso."), true);
            }

            ViewBag.partialPermission = false;
            return View(usuario);
        }

        // GET: Usuario/Delete/5
        public ActionResult Delete(string id)
        {
            var usuario = _usuarioRepository.GetById(id);

            return View(usuario);
        }

        // POST: Usuario/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            //var usuario = _usuarioRepository.GetById(id);
            //usuario.Ativo = false;
            //_usuarioRepository.Update(usuario);
            _usuarioRepository.RemoveLogical(id);

            return RedirectToAction("Index");
        }

        public ActionResult AdicionarPermissao(string idModel, string permissao)
        {
            var permissaoObject = JsonConvert.DeserializeObject<PermissaoUsuarioModel>(permissao);

            var usuario = _usuarioRepository.GetByIdEager(idModel);

            if (!permissaoObject.DescricaoPermissao.ToString().Equals("selecione", StringComparison.InvariantCultureIgnoreCase))
            {
                var existe = false;
                foreach (var item in usuario.Permissoes)
                {
                    existe = (item.IdPermissao == permissaoObject.IdPermissao);
                    if (existe) break;
                }
                if (!existe)
                {
                    usuario.Permissoes.Add(permissaoObject);

                    _usuarioRepository.Update(usuario);
                    return PartialView("_PermissoesUsuario", usuario);
                }

                Warning(String.Format("Este usuário já possui esta permissão!"), true);
                ViewBag.partialPermission = true;

            }
            else
            {
                Warning(String.Format("Você deve selecionar uma permissão!"), true);
                ViewBag.partialPermission = true;

            }

            return PartialView("_PermissoesUsuario", usuario);
        }

        public ActionResult RemoverPermissao(int id)
        {
            var permissao = _permissaoUsuarioRepository.GetById(id);
            var usuario = _usuarioRepository.GetByIdEager(permissao.IdUsuario);

            foreach (var item in usuario.Permissoes)
            {
                if (item.Id.Equals(id))
                {
                    usuario.Permissoes.Remove(item);
                    break;
                }
            }

            _usuarioRepository.Update(usuario);

            return PartialView("_PermissoesUsuario", usuario);
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
    }
}
