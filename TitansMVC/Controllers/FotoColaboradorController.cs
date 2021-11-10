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
    public class FotoColaboradorController : Controller
    {
        private readonly IColaboradorRepository _colaboradorRepository;

        public FotoColaboradorController()
        {
            _colaboradorRepository = new ColaboradorRepository();
        }

        // GET: /Foto/
        public ActionResult Index(int id)
        {
            var colaborador = _colaboradorRepository.GetById(id);
            return File(colaborador.Foto, "image/png");
        }
    }
}