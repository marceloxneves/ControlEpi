using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TitansMVC.Repository.Implementations;
using TitansMVC.Repository.Interfaces;

namespace TitansMVC.Controllers
{
    [Authorize]
    public class FotoEpiController : Controller
    {
        private readonly IEpiRepository _epiRepository;

        public FotoEpiController()
        {
            _epiRepository = new EpiRepository();            
        }

        // GET: FotoEpi
        public ActionResult Index(int id)
        {
            var epi = _epiRepository.GetById(id);
            var foto = epi.Foto;
            return File(foto, "image/png");
        }
    }
}