using System;
using System.Web.Mvc;
using PagedList;
using TitansMVC.Models;
using TitansMVC.Repository.Implementations;
using TitansMVC.Repository.Interfaces;

namespace TitansMVC.Controllers
{
    [Authorize(Roles = "role_auxiliares_uniforme")]
    public class TipoUniformeController : BaseController
    {
        private readonly ITipoUniformeRepository _tipoUniformeRepository;

        public TipoUniformeController()
        {
            _tipoUniformeRepository = new TipoUniformeRepository();
        }

        // GET: TipoUniforme
        public ActionResult Index(string searchBy, string search, int? page)
        {
            var tiposUniforme = _tipoUniformeRepository.BuscarAtivos();

            if (searchBy == "Nome")
            {
                tiposUniforme = _tipoUniformeRepository.BuscarPorNome(nome: search);
            }
            else if (searchBy == "Todos")
            {
                tiposUniforme = _tipoUniformeRepository.BuscarAtivos();
            }

            return View(tiposUniforme.ToPagedList(page ?? 1, 10));
        }

        
        // GET: TipoUniforme/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TipoUniforme/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TipoUniformeModel tipoUniforme)
        {
            if (ModelState.IsValid)
            {
                tipoUniforme.Ativo = true;
                _tipoUniformeRepository.Add(tipoUniforme);

                Success(String.Format("Registro gravado com sucesso!"), true);

                return RedirectToAction("Edit", tipoUniforme);
            }

            return View(tipoUniforme);
        }

        // GET: TipoUniforme/Edit/5
        public ActionResult Edit(int id)
        {
            var tipoUniforme = _tipoUniformeRepository.GetById(id);

            return View(tipoUniforme);
        }

        // POST: TipoUniforme/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TipoUniformeModel tipoUniforme)
        {
            if (ModelState.IsValid)
            {
                _tipoUniformeRepository.Update(tipoUniforme);

                Success(String.Format("Registro alterado com sucesso!"), true);
            }

            return View(tipoUniforme);
        }

        // GET: TipoUniforme/Delete/5
        public ActionResult Delete(int id)
        {
            var tipoUniforme = _tipoUniformeRepository.GetById(id);

            return View(tipoUniforme);
        }

        // POST: TipoUniforme/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _tipoUniformeRepository.RemoveLogical(id);

            return RedirectToAction("Index");
        }

    }
}
