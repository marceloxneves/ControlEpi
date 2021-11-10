using System.Web.Mvc;
using TitansMVC.Repository.Implementations;
using TitansMVC.Repository.Interfaces;

namespace TitansMVC.Controllers
{
    [Authorize(Roles = "role_base, role_base_uniforme")]
    public class AvisosController : BaseController
    {
        private readonly IConfiguracaoRepository _configuracaoRepository;
        private readonly IEpiRepository _epiRepository;
        private readonly IEpiColaboradorRepository _epiColaboradorRepository;
        private readonly IUniformeRepository _uniformeRepository;
        private readonly IUniformeColaboradorRepository _uniformeColaboradorRepository;

        public AvisosController()
        {
            _configuracaoRepository = new ConfiguracaoRepository();
            _epiRepository = new EpiRepository();
            _epiColaboradorRepository = new EpiColaboradorRepository();
            _uniformeRepository = new UniformeRepository();
            _uniformeColaboradorRepository = new UniformeColaboradorRepository();
        }

        // GET: Aviso
        public ActionResult Index()
        {
            var configuracao = _configuracaoRepository.GetUnique();
            
            if (configuracao.AvisarAposVencCa)
            {
                ViewBag.EpisCaVencidos = _epiRepository.BuscarCaVencidos();
            }

            if (configuracao.AvisarAposVencEpi)
            {
                ViewBag.EpisVencidos = _epiColaboradorRepository.BuscarEpisVencidos();
            }

            if (configuracao.AvisarAposVencUniforme)
            {
                ViewBag.UniformesVencidos = _uniformeColaboradorRepository.BuscarUniformesVencidos();
            }

            if (configuracao.AvisarVencCaComAntec && configuracao.QtdeDiasAvisoVencCa != null && configuracao.QtdeDiasAvisoVencCa > 0)
            {
                ViewBag.EpisCaAVencer = _epiRepository.BuscarCaAVencer(configuracao.QtdeDiasAvisoVencCa.Value);
            }

            if (configuracao.AvisarVencEpiComAntec && configuracao.QtdeDiasAvisoVencCa != null && configuracao.QtdeDiasAvisoVencEpi > 0)
            {
                ViewBag.EpisAVencer = _epiColaboradorRepository.BuscarEpisAVencer(configuracao.QtdeDiasAvisoVencEpi.Value);
            }

            if (configuracao.AvisarVencUniformeComAntec && configuracao.QtdeDiasAvisoVencUniforme > 0)
            {
                ViewBag.UniformesAVencer = _uniformeColaboradorRepository.BuscarUniformesAVencer(configuracao.QtdeDiasAvisoVencUniforme.Value);
            }

            return View();
        }
    }
}