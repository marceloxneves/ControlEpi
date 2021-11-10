using System.Web.Mvc;
using TitansMVC.Repository.Implementations;
using TitansMVC.Repository.Interfaces;

namespace TitansMVC.Controllers
{
    [Authorize]
    public class LogomarcaController : Controller
    {
        private readonly IEmpresaRepository _empresaRepository;

        public LogomarcaController()
        {
            _empresaRepository = new EmpresaRepository();
        }

        // GET: /Logomarca
        public ActionResult Index(int id)
        {
            var empresa = _empresaRepository.GetById(id);
            var fileToRetrieve = empresa.Logomarca;
            return File(fileToRetrieve, "image/png");
        }
    }
}