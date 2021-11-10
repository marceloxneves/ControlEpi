using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TitansMVC.Models;
using TitansMVC.Repository.Implementations;
using TitansMVC.Repository.Interfaces;

namespace TitansMVC.Controllers
{
    [Authorize(Roles ="role_admin, role_master")]
    public class ConfiguracaoController : BaseController
    {
        private readonly IConfiguracaoRepository _configuracaoRepository;

        public ConfiguracaoController()
        {
            _configuracaoRepository = new ConfiguracaoRepository();
        }

        // GET: Configuracao
        public ActionResult Index()
        {
            var configuracao = _configuracaoRepository.GetUnique();

            return View(configuracao);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(ConfiguracaoModel configuracao)
        {
            _configuracaoRepository.Update(configuracao);

            Success(string.Format("Registro alterado com sucesso."), true);

            return RedirectToAction("Index");
        }
    }
}