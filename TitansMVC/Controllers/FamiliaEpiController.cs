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
    //ok
    [Authorize(Roles = "master")]
    public class FamiliaEpiController : BaseController
    {
        private readonly IFamiliaEpiRepository _familiaEpiRepository;

        public FamiliaEpiController()
        {
            _familiaEpiRepository = new FamiliaEpiRepository();
        }

        // GET: FamiliaEpi
        public ActionResult Index(string searchBy, string search, int? page)
        {
            var familiasEpi = _familiaEpiRepository.BuscarAtivos();

            if (searchBy == "Nome")
            {
                familiasEpi = _familiaEpiRepository.BuscarPorNome(nome: search);
            }
            else if (searchBy == "Todos")
            {
                familiasEpi = _familiaEpiRepository.BuscarAtivos();
            }

            return View(familiasEpi.ToPagedList(page ?? 1, 10));
        }

        // GET: FamiliaEpi/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FamiliaEpi/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FamiliaEpiModel familiaEpi)
        {
            if (ModelState.IsValid)
            {
                familiaEpi.Ativo = true;

                _familiaEpiRepository.Add(familiaEpi);

                Success(String.Format("Registro gravado com sucesso!"), true);

                return RedirectToAction("Edit", familiaEpi);
            }

            return View(familiaEpi);
        }

        // GET: FamiliaEpi/Edit/5
        public ActionResult Edit(int id)
        {
            var familiaEpi = _familiaEpiRepository.GetById(id);

            return View(familiaEpi);
        }

        // POST: FamiliaEpi/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FamiliaEpiModel familiaEpi)
        {
            if (ModelState.IsValid)
            {
                _familiaEpiRepository.Update(familiaEpi);

                Success(String.Format("Registro alterado com sucesso!"), true);
            }

            return View(familiaEpi);
        }

        // GET: FamiliaEpi/Delete/5
        public ActionResult Delete(int id)
        {
            var familiaEpi = _familiaEpiRepository.GetById(id);

            return View(familiaEpi);
        }

        // POST: FamiliaEpi/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //var familiaEpi = _familiaEpiRepository.GetById(id);
            //_familiaEpiRepository.Remove(familiaEpi);

            _familiaEpiRepository.RemoveLogical(id);

            return RedirectToAction("Index");
        }
    }
}
