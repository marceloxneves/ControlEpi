using System.Web.Mvc;
using TitansMVC.Repository.Implementations;
using TitansMVC.Repository.Interfaces;

namespace TitansMVC.Controllers
{
    [Authorize]
    public class LogomarcaLbcController : Controller
    {
        private readonly ILbcRepository _lbcRepository;

        public LogomarcaLbcController()
        {
            _lbcRepository = new LbcRepository();
        }

        // GET: /Logomarca
        public ActionResult Index(int id)
        {
            var lbc = _lbcRepository.GetById(id);
            var fileToRetrieve = lbc.Logomarca;
            return File(fileToRetrieve, "image/png");
        }
    }
}