using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using TitansMVC.Models;
using TitansMVC.Repository.Implementations;
using TitansMVC.Repository.Interfaces;

namespace TitansMVC.Controllers
{

    [Authorize(Roles = "role_auxiliares")]
    public class TipoEpiController : BaseController
    {
        private readonly ITipoEpiRepository _tipoEpiRepository;

        public TipoEpiController()
        {
            _tipoEpiRepository = new TipoEpiRepository();
        }

        // GET: TipoEpi
        public ActionResult Index(string searchBy, string search, int? page)
        {
            var tiposEpi = _tipoEpiRepository.BuscarAtivos();

            if (searchBy == "Nome")
            {
                tiposEpi = _tipoEpiRepository.BuscarPorNome(nome: search);
            }
            else if (searchBy == "Todos")
            {
                tiposEpi = _tipoEpiRepository.BuscarAtivos();
            }

            return View(tiposEpi.ToPagedList(page ?? 1, 10));
        }

        // GET: TipoEpi/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TipoEpi/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TipoEpiModel tipoEpi)
        {
            if (ModelState.IsValid)
            {
                tipoEpi.Ativo = true;
                _tipoEpiRepository.Add(tipoEpi);

                Success(String.Format("Registro gravado com sucesso!"), true);

                return RedirectToAction("Edit", tipoEpi);
            }

            return View(tipoEpi);
        }

        // GET: TipoEpi/Edit/5
        public ActionResult Edit(int id)
        {
            var tipoEpi = _tipoEpiRepository.GetById(id);

            return View(tipoEpi);
        }

        // POST: TipoEpi/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TipoEpiModel tipoEpi)
        {
            if (ModelState.IsValid)
            {
                _tipoEpiRepository.Update(tipoEpi);

                Success(String.Format("Registro alterado com sucesso!"), true);
            }

            return View(tipoEpi);
        }

        // GET: TipoEpi/Delete/5
        public ActionResult Delete(int id)
        {
            var tipoEpi = _tipoEpiRepository.GetById(id);

            return View(tipoEpi);
        }

        // POST: FamiliaEpi/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //var tipoEpi = _tipoEpiRepository.GetById(id);
            //_tipoEpiRepository.Remove(tipoEpi);
            _tipoEpiRepository.RemoveLogical(id);

            return RedirectToAction("Index");
        }
    }
}
