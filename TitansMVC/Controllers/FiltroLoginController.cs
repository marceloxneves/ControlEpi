using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TitansMVC.Repository.Implementations;
using TitansMVC.Repository.Interfaces;
using TitansMVC.Utils;

namespace TitansMVC.Controllers
{
    //ok
    public class FiltroLoginController : BaseController
    {
        private readonly IPlanoRepository _planoRepository = new PlanoRepository();
        //private readonly IPlanoRepository _planoRepository = new PlanoRepository();
        // GET: FiltroLogin
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (!User.IsInRole("role_master"))
                {
                    var val = Util.GetEmpresaPlanoValidade(Util.GetEmpresaCnpj());

                    if (val < DateTime.Now)
                    {
                        _planoRepository.CopiarPlanoServidorParaCliente(Util.GetEmpresaCnpj());
                        val = Util.GetEmpresaPlanoValidade(Util.GetEmpresaCnpj());
                        if (val < DateTime.Now)
                        {
                            Warning(String.Format("Seu plano expirou, entre em contato com o administrador."), true);
                            return RedirectToAction("Desconecta", "Account");
                        }
                    }
                }
            }
            return RedirectToAction("Index", "Home");
        }
    }
}