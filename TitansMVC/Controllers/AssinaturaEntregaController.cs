using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TitansMVC.Repository.Implementations;
using TitansMVC.Repository.Interfaces;

namespace TitansMVC.Controllers
{
    public class AssinaturaEntregaController : Controller
    {
        private readonly IEpiColaboradorRepository _epiColaboradorRepository;

        public AssinaturaEntregaController()
        {
            _epiColaboradorRepository = new EpiColaboradorRepository();
        }

        // GET: /Foto/
        public ActionResult Index(int id)
        {
            var fileToRetrieve = _epiColaboradorRepository.GetById(id);

            return File(fileToRetrieve.AssinaturaColaborador, "image/png");
        }
    }
}