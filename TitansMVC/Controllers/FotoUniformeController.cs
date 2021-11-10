using System.Web.Mvc;
using TitansMVC.Repository.Implementations;
using TitansMVC.Repository.Interfaces;

namespace TitansMVC.Controllers
{
    [Authorize]
    public class FotoUniformeController : Controller
    {
        private readonly IUniformeRepository _uniformeRepository;

        public FotoUniformeController()
        {
            _uniformeRepository = new UniformeRepository();            
        }

        // GET: FotoUniforme
        public ActionResult Index(int id)
        {
            var uniforme = _uniformeRepository.GetById(id);
            var foto = uniforme.Foto;
            return File(foto, "image/png");
        }
    }
}