using System.Web.Mvc;
using TitansMVC.Repository.Implementations;
using TitansMVC.Repository.Interfaces;

namespace TitansMVC.Controllers
{
    public class AssinaturaController : Controller
    {
        private readonly IColaboradorRepository _colaboradorRepository;

        public AssinaturaController()
        {
            _colaboradorRepository = new ColaboradorRepository();
        }

        // GET: /Assinatura/
        public ActionResult Index(int id)
        {
            var fileToRetrieve = _colaboradorRepository.GetById(id);
            return File(fileToRetrieve.Assinatura, "image/png");
        }
    }
}