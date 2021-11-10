using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using TitansMVC.Models;
using TitansMVC.Repository.Implementations;
using TitansMVC.Repository.Interfaces;
using System.IO;
using System.Data.Entity.Infrastructure;

namespace TitansMVC.Controllers
{
    //ok
    [Authorize(Roles = "role_auxiliares, role_auxiliares_uniforme")]
    public class LbcController : BaseController
    {
        private readonly IUfRepository _ufRepository;
        private readonly IMunicipioRepository _municipioRepository;
        private readonly ILbcRepository _lbcRepository;

        public LbcController()
        {
            _lbcRepository = new LbcRepository();
            _ufRepository = new UfRepository();
            _municipioRepository = new MunicipioRepository();
        }

        // GET: Lbc
        public ActionResult Index(string searchBy, string search, int? page)
        {
            var lbcs = _lbcRepository.BuscarAtivos();

            if (searchBy == "Nome")
            {
                lbcs = _lbcRepository.BuscarPorNome(nome: search);
            }
            else if (searchBy == "Todos")
            {
                lbcs = _lbcRepository.BuscarAtivos();
            }

            return View(lbcs.ToPagedList(page ?? 1, 10));
        }

        // GET: Lbc/Create
        public ActionResult Create()
        {
            ViewBag.SiglaUf = new SelectList(_ufRepository.GetAll(), "Sigla", "Nome");
            return View();
        }

        // POST: Lbc/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(LbcModel lbc, HttpPostedFileBase uploadlogoUN)
        {
            ViewBag.SiglaUf = new SelectList(_ufRepository.GetAll(), "Sigla", "Nome", lbc.SiglaUf);
            ViewBag.MunicipioId = new SelectList(_municipioRepository.BuscarPorUf(lbc.SiglaUf), "Id", "Nome", lbc.MunicipioId);

            if (ModelState.IsValid)
            {
                lbc.Ativo = true;

                if (uploadlogoUN != null && uploadlogoUN.ContentLength > 0)
                {
                    try
                    {
                        using (var reader = new BinaryReader(uploadlogoUN.InputStream))
                        {
                            lbc.Logomarca = reader.ReadBytes(uploadlogoUN.ContentLength);
                        }
                    }
                    catch (RetryLimitExceededException /* dex */)
                    {
                        ModelState.AddModelError("", "Não foi possível salvar as alterações. Tente novamente, e se o problema persistir entre em contato com o administrador do sistema.");
                    }
                }
                else
                {
                    lbc.Logomarca = null;
                }

                _lbcRepository.Add(lbc);

                Success(String.Format("Registro gravado com sucesso!"), true);

                return RedirectToAction("Edit", lbc);
            }

            return View(lbc);
        }

        // GET: Lbc/Edit/5
        public ActionResult Edit(int id)
        {
            var lbc = _lbcRepository.GetById(id);
            ViewBag.SiglaUf = new SelectList(_ufRepository.GetAll(), "Sigla", "Nome", lbc.SiglaUf);
            ViewBag.MunicipioId = new SelectList(_municipioRepository.BuscarPorUf(lbc.SiglaUf), "Id", "Nome", lbc.MunicipioId);

            return View(lbc);
        }

        // POST: Lbc/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(LbcModel lbc, HttpPostedFileBase uploadlogoLbc)
        {
            ViewBag.SiglaUf = new SelectList(_ufRepository.GetAll(), "Sigla", "Nome", lbc.SiglaUf);
            ViewBag.MunicipioId = new SelectList(_municipioRepository.BuscarPorUf(lbc.SiglaUf), "Id", "Nome", lbc.MunicipioId);
            if (ModelState.IsValid)
            {

                if (uploadlogoLbc != null && uploadlogoLbc.ContentLength > 0)
                {
                    try
                    {
                        using (var reader = new BinaryReader(uploadlogoLbc.InputStream))
                        {
                            lbc.Logomarca = reader.ReadBytes(uploadlogoLbc.ContentLength);
                        }
                    }
                    catch (RetryLimitExceededException /* dex */)
                    {
                        ModelState.AddModelError("", "Não foi possível salvar as alterações. Tente novamente, e se o problema persistir entre em contato com o administrador do sistema.");
                    }
                }
                else
                {
                    lbc.Logomarca = null;
                }

                _lbcRepository.Update(lbc);

                Success(String.Format("Registro alterado com sucesso!"), true);
            }

            return View(lbc);
        }

        // GET: Lbc/Delete/5
        [Authorize(Roles = "role_master")]
        public ActionResult Delete(int id)
        {
            var lbc = _lbcRepository.GetById(id);
            return View(lbc);
        }

        // POST: CentroCusto/Delete/5
        [Authorize(Roles = "role_master")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var lbc = _lbcRepository.GetById(id);
            if (_lbcRepository.QtdeCentrosCustos(lbc.Id) > 0)
            {
                ViewBag.PossuiCentrosCustos = true;
                return View(lbc);
            }

            //_lbcRepository.Remove(lbc);
            _lbcRepository.RemoveLogical(id);
            return RedirectToAction("Index");
        }

        public JsonResult GetMunicipios(string siglaUf)
        {
            IEnumerable<MunicipioModel> municipios = _municipioRepository.BuscarPorUf(siglaUf);

            return Json(new SelectList(municipios, "Id", "Nome"), JsonRequestBehavior.AllowGet);
        }
    }
}