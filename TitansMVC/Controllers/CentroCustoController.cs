using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using TitansMVC.Models;
using TitansMVC.Repository.Implementations;
using TitansMVC.Repository.Interfaces;
using TitansMVC.Utils;

namespace TitansMVC.Controllers
{
    //ok
    [Authorize(Roles = "role_auxiliares, role_auxiliares_uniforme")]
    public class CentroCustoController : BaseController
    {
        private readonly ICentroCustoRepository _centroCustoRepository;
        private readonly ILbcRepository _lbcRepository;

        public CentroCustoController()
        {
            _centroCustoRepository = new CentroCustoRepository();
            _lbcRepository = new LbcRepository();
        }

        // GET: Setor
        public ActionResult Index(string searchBy, string search, int? page, int? UnidadeNegocioId)
        {
            ViewBag.UnidadeNegocioId = new SelectList(_lbcRepository.BuscarAtivos(), "Id", "Fantasia");
            var centrosCusto = _centroCustoRepository.BuscarAtivos();

            if (searchBy == "Nome")
            {
                centrosCusto = _centroCustoRepository.BuscarPorNome(nome: search, UnidadeNegocioId: UnidadeNegocioId);
            }
            else if (searchBy == "Todos")
            {
                centrosCusto = UnidadeNegocioId == null ? 
                    _centroCustoRepository.BuscarAtivos() : 
                    _centroCustoRepository.BuscarPorUnidadeNegocio(UnidadeNegocioId.Value);
            }

            return View(centrosCusto.ToPagedList(page ?? 1, 10));
        }

        // GET: CentroCusto/Create
        public ActionResult Create()
        {
            ViewBag.LbcId = new SelectList(_lbcRepository.BuscarAtivos(), "Id", "Fantasia");

            return View();
        }

        // POST: CentroCusto/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CentroCustoModel centroCusto)
        {
            ViewBag.LbcId = new SelectList(_lbcRepository.BuscarAtivos(), "Id", "Fantasia");
            if (ModelState.IsValid)
            {
                if (_centroCustoRepository.BuscarPorNome(centroCusto.Nome,centroCusto.LbcId).Any())
                {
                    Warning("Centro de custo já cadastrado no sistema!", true);
                    return View(centroCusto);
                }
                _centroCustoRepository.Add(centroCusto);
                Success(String.Format("Registro gravado com sucesso!"), true);

                return RedirectToAction("Edit", centroCusto);
            }


            return View(centroCusto);
        }

        // GET: CentroCusto/Edit/5
        public ActionResult Edit(int id)
        {            
            var centroCusto = _centroCustoRepository.GetById(id);
            ViewBag.LbcId = new SelectList(_lbcRepository.BuscarAtivos(), "Id", "Fantasia", centroCusto.LbcId);

            return View(centroCusto);
        }

        // POST: CentroCusto/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CentroCustoModel centroCusto)
        {
            ViewBag.LbcId = new SelectList(_lbcRepository.BuscarAtivos(), "Id", "Fantasia", centroCusto.LbcId);

            if (ModelState.IsValid)
            {
                _centroCustoRepository.Update(centroCusto);

                Success(String.Format("Registro alterado com sucesso!"), true);
            }

            return View(centroCusto);
        }

        // GET: CentroCusto/Delete/5
        public ActionResult Delete(int id)
        {
            var centroCusto = _centroCustoRepository.GetById(id);

            return View(centroCusto);
        }

        // POST: CentroCusto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //var centroCusto = _centroCustoRepository.GetById(id);
            //_centroCustoRepository.Remove(centroCusto);

            _centroCustoRepository.RemoveLogical(id);

            return RedirectToAction("Index");
        }
    }
}
