using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.Ajax.Utilities;
using TitansMVC.Models;
using TitansMVC.Repository.Implementations;
using TitansMVC.Repository.Interfaces;
using TitansMVC.Utils;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace TitansMVC.Controllers
{
    //ok
    [Authorize]
    public class HomeController : BaseController
    {
        private readonly IConfiguracaoRepository _configuracaoRepository;
        private readonly IEpiRepository _epiRepository;
        private readonly IEpiSetorRepository _epiSetorRepository;
        private readonly IEpiColaboradorRepository _epiColaboradorRepository;
        private readonly IColaboradorRepository _colaboradorRepository;
        private readonly ISetorRepository _setorRepository;
        private readonly ICentroCustoRepository _centroCustoRepository;
        private readonly ITipoEpiRepository _tipoEpiRepository;
        private readonly IEstoqueEpiRepository _estoqueRepository;
        //private readonly ILbcRepository _lbcRepository;
        //private readonly IFuncaoRepository _funcaoRepository;

        private readonly IUniformeRepository _uniformeRepository;
        private readonly IUniformeSetorRepository _uniformeSetorRepository;
        private readonly IUniformeColaboradorRepository _uniformeColaboradorRepository;
        private readonly ITipoUniformeRepository _tipoUniformeRepository;
        private readonly IEstoqueUniformeRepository _estoqueUniformeRepository;

        public HomeController()
        {
            _configuracaoRepository = new ConfiguracaoRepository();
            _epiRepository = new EpiRepository();
            _epiSetorRepository = new EpiSetorRepository();
            _epiColaboradorRepository = new EpiColaboradorRepository();
            _colaboradorRepository = new ColaboradorRepository();
            _setorRepository = new SetorRepository();
            _centroCustoRepository = new CentroCustoRepository();
            _epiSetorRepository = new EpiSetorRepository();
            _tipoEpiRepository = new TipoEpiRepository();
            _estoqueRepository = new EstoqueEpiRepository();

            _uniformeRepository = new UniformeRepository();
            _uniformeSetorRepository = new UniformeSetorRepository();
            _uniformeColaboradorRepository = new UniformeColaboradorRepository();
            _tipoUniformeRepository = new TipoUniformeRepository();
            _estoqueUniformeRepository = new EstoqueUniformeRepository();

        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.SetorId = new SelectList(_setorRepository.BuscarAtivos(), "Id", "Nome");
                ViewBag.TipoEpiId = new SelectList(_tipoEpiRepository.BuscarAtivos(), "Id", "Nome");
                ViewBag.EpiSetorId = new SelectList("", "");
                ViewBag.EpiOutrSetoresId = new SelectList("", "");
                ViewBag.CentroCustoId = new SelectList(_centroCustoRepository.BuscarAtivos(), "Id", "Nome");
                ViewBag.ColaboradorId = new SelectList(_colaboradorRepository.BuscarAtivos(), "Id", "Nome");

                var episCaVencidos = new List<EpiModel>();
                var episVencidos = new List<EpiColaboradorModel>();
                var episCaAVencer = new List<EpiModel>();
                var episAVencer = new List<EpiColaboradorModel>();

                var configuracao = _configuracaoRepository.GetUnique();

                if ((Util.GetEmpresaPlanoValidade(Util.GetEmpresaCnpj()) - DateTime.Now).TotalDays < 10 && (Util.GetEmpresaPlanoValidade(Util.GetEmpresaCnpj()) - DateTime.Now).TotalDays > 0)
                {
                    int days = (int) (Util.GetEmpresaPlanoValidade(Util.GetEmpresaCnpj()) - DateTime.Now).TotalDays;
                    Danger(String.Format("Seu plano irá expirar em " + days + " dias, entre em contato com o administrador."), true);
                }
                if (configuracao != null)
                {
                    if (configuracao.AvisarAposVencCa)
                    {
                        episCaVencidos = (List<EpiModel>)_epiRepository.BuscarCaVencidos();
                    }

                    if (configuracao.AvisarAposVencEpi)
                    {
                        episVencidos = (List<EpiColaboradorModel>)_epiColaboradorRepository.BuscarEpisVencidos();
                    }

                    if (configuracao.AvisarVencCaComAntec && configuracao.QtdeDiasAvisoVencCa != null && configuracao.QtdeDiasAvisoVencCa > 0)
                    {
                        episCaAVencer = (List<EpiModel>)_epiRepository.BuscarCaAVencer(configuracao.QtdeDiasAvisoVencCa.Value);
                    }

                    if (configuracao.AvisarVencEpiComAntec && configuracao.QtdeDiasAvisoVencCa != null && configuracao.QtdeDiasAvisoVencEpi > 0)
                    {
                        episAVencer = (List<EpiColaboradorModel>)_epiColaboradorRepository.BuscarEpisAVencer(configuracao.QtdeDiasAvisoVencCa.Value);
                    }
                }

                ViewBag.PossuiAvisos = (episCaVencidos.Count > 0) || (episVencidos.Count > 0)
                                       || (episCaAVencer.Count > 0) || (episAVencer.Count > 0);
                return View();

            }
            return RedirectToAction("Login", "Account");
        }

        //public ActionResult About()
        //{
        //    ViewBag.Message = "Your application description page.";

        //    return View();
        //}

        //public ActionResult Contact()
        //{
        //    ViewBag.Message = "Your contact page.";

        //    return View();
        //}
    }
}