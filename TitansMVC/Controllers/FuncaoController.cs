using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TitansMVC.Models;
using TitansMVC.Repository.Implementations;
using TitansMVC.Repository.Interfaces;
using TitansMVC.Utils;

namespace TitansMVC.Controllers
{
    [Authorize(Roles = "role_auxiliares, role_auxiliares_uniforme")]
    public class FuncaoController : BaseController
    {
        private readonly IFuncaoRepository _funcaoRepository;

        public FuncaoController()
        {
            _funcaoRepository = new FuncaoRepository();
        }

        // GET: Setor
        public ActionResult Index(string searchBy, string search, int? page)
        {
            var funcoes = _funcaoRepository.BuscarAtivos();

            if (searchBy == "Nome")
            {
                funcoes = _funcaoRepository.BuscarPorNome(nome: search);
            }
            else if (searchBy == "Todos")
            {
                funcoes = _funcaoRepository.BuscarAtivos();
            }

            return View(funcoes.ToPagedList(page ?? 1, 10));
        }

        // GET: Setor/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Setor/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FuncaoModel funcao)
        {
            if (ModelState.IsValid)
            {
                funcao.Ativo = true;
                funcao.IdEmpresa = Util.GetEmpresaId();
                _funcaoRepository.Add(funcao);
                Success(String.Format("Registro gravado com sucesso!"), true);
                return RedirectToAction("Edit", funcao);
            }

            //Warning(String.Format("O sistema já possui uma função com este nome cadastrado."), true);
            return View(funcao);
        }

        // GET: Setor/Edit/5
        public ActionResult Edit(int id)
        {
            var funcao = _funcaoRepository.GetById(id);
            return View(funcao);
        }

        // POST: Setor/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FuncaoModel funcao)
        {
            if (ModelState.IsValid)
            {
                _funcaoRepository.Update(funcao);
                Success(String.Format("Registro alterado com sucesso!"), true);              
            }

            return View(funcao);
        }

        // GET: Setor/Delete/5
        public ActionResult Delete(int id)
        {
            var funcao = _funcaoRepository.GetById(id);
            return View(funcao);
        }

        // POST: Setor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var funcao = _funcaoRepository.GetById(id);
            _funcaoRepository.RemoveLogical(id);
            return RedirectToAction("Index");
        }
    }
}