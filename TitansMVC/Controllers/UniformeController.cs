using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PagedList;
using TitansMVC.Consultas;
using TitansMVC.Models;
using TitansMVC.Models.Relatorios;
using TitansMVC.Repository.Implementations;
using TitansMVC.Repository.Interfaces;
using TitansMVC.Utils;
using static TitansMVC.Models.UniformeModel;

namespace TitansMVC.Controllers
{
    [Authorize(Roles = "role_base_uniforme")]
    public class UniformeController : BaseController
    {
        private readonly IUniformeRepository _uniformeRepository;
        private readonly ITipoUniformeRepository _tipoUniformeRepository;
        private readonly IFichaUniformeRepository _fichaUniformeRepository;
        private readonly IRelFichaUniformeRepository _relFichaUniformeRepository; 
        private readonly IEstoqueUniformeRepository _estoqueRepository;
        private readonly ILbcRepository _lbcRepository;
        private readonly IUniformeSetorRepository _setorUniformeRepository;

        public UniformeController()
        {
            _uniformeRepository = new UniformeRepository();
            _tipoUniformeRepository = new TipoUniformeRepository();
            _fichaUniformeRepository = new FichaUniformeRepository();
            _relFichaUniformeRepository = new RelFichaUniformeRepository(); 
            _estoqueRepository = new EstoqueUniformeRepository();
            _lbcRepository = new LbcRepository();
            _setorUniformeRepository = new UniformeSetorRepository();
        }

        // GET: Uniforme
        public ActionResult Index(string searchBy, bool? ativos, string search, int? page)
        {

            IEnumerable<UniformeModel> uniformes;
            if (searchBy == "Nome")
            {
                uniformes = _uniformeRepository.BuscarPorNome(nome: search, ativos: ativos.HasValue ? ativos.Value : true);
            }
            else if (searchBy == "Todos")
            {
                uniformes = _uniformeRepository.GetAll(ativos.HasValue ? ativos.Value : true);
            }
            else
            {
                uniformes = _uniformeRepository.BuscarAtivos();
            }
            return View(uniformes.ToPagedList(page ?? 1, 10));
        }

        // GET: Uniforme/Create
        public ActionResult Create()
        {
            ViewBag.TipoUniformeId = new SelectList(_tipoUniformeRepository.BuscarAtivos(), "Id", "Nome");
            ViewBag.UnidadeNegocioId = new SelectList(_lbcRepository.BuscarAtivos(), "Id", "Fantasia");

            return View();
        }

        // POST: Uniforme/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UniformeModel uniforme, HttpPostedFileBase uploadfoto)
        {
            if (ModelState.IsValid)
            {
                uniforme.Ativo = true;
                
                if (_uniformeRepository.ValidarCodigoDeBarras(uniforme.CodigoDeBarras, uniforme.Id) != null)
                {
                    ModelState.AddModelError("CodigoDeBarras", "Codigo de barras já cadastrado no sistema!");

                }
                else
                {
                    if (uploadfoto != null && uploadfoto.ContentLength > 0)
                    {
                        try
                        {
                            using (var reader = new BinaryReader(uploadfoto.InputStream))
                            {
                                uniforme.Foto = reader.ReadBytes(uploadfoto.ContentLength);
                            }
                        }
                        catch (RetryLimitExceededException /* dex */)
                        {
                            ModelState.AddModelError("", "Não foi possível salvar as alterações. Tente novamente, e se o problema persistir entre em contato com o administrador do sistema.");
                        }
                    }
                    else
                    {
                        uniforme.Foto = null;
                    }


                    _uniformeRepository.Add(uniforme);

                    Success(String.Format("Registro gravado com sucesso!"), true);

                    return RedirectToAction("Edit", uniforme);
                }
            }

            ViewBag.TipoUniformeId = new SelectList(_tipoUniformeRepository.BuscarAtivos(), "Id", "Nome");
            ViewBag.UnidadeNegocioId = new SelectList(_lbcRepository.BuscarAtivos(), "Id", "Fantasia");
            return View(uniforme);
        }

        // GET: Uniforme/Edit/5
        public ActionResult Edit(int id)
        {
            var uniforme = _uniformeRepository.GetByIdEager(id);

            ViewBag.TipoUniformeId = new SelectList(_tipoUniformeRepository.BuscarAtivos(), "Id", "Nome", uniforme.TipoUniformeId);
            ViewBag.UnidadeNegocioId = new SelectList(_lbcRepository.BuscarAtivos(), "Id", "Fantasia", uniforme.UnidadeNegocioId);
            var fichaUniforme = _fichaUniformeRepository.BuscarPorUniforme(uniforme.Id);

            ViewBag.PossuiFicha = false;

            if (fichaUniforme != null)
            {
                ViewBag.PossuiFicha = true;
            }

            return View(uniforme);
        }

        // POST: Uniforme/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UniformeModel uniforme, HttpPostedFileBase uploadfoto)
        {
            var uniformeRepo = _uniformeRepository.GetByIdEager(uniforme.Id);
            var uniformeSetorRepo = (List<UniformeSetorModel>)_setorUniformeRepository.BuscarPorUniforme(uniforme.Id); 
            
            uniforme.Foto = uniformeRepo.Foto;

            _uniformeRepository.RetirarDoContexto(uniformeRepo);

            ViewBag.TipoUniformeId = new SelectList(_tipoUniformeRepository.BuscarAtivos(), "Id", "Nome", uniforme.TipoUniformeId);
            ViewBag.UnidadeNegocioId = new SelectList(_lbcRepository.BuscarAtivos(), "Id", "Fantasia");

            if (ModelState.IsValid)
            {                
                if (_uniformeRepository.ValidarCodigoDeBarras(uniforme.CodigoDeBarras, uniforme.Id) != null)
                {
                    ModelState.AddModelError("CodigoDeBarras", "Codigo de barras já cadastrado no sistema!");

                }
                else
                {
                    if (uploadfoto != null && uploadfoto.ContentLength > 0)
                    {
                        try
                        {
                            using (var reader = new BinaryReader(uploadfoto.InputStream))
                            {
                                uniforme.Foto = reader.ReadBytes(uploadfoto.ContentLength);
                            }
                        }
                        catch (RetryLimitExceededException /* dex */)
                        {
                            ModelState.AddModelError("", "Não foi possível salvar as alterações. Tente novamente, e se o problema persistir entre em contato com o administrador do sistema.");
                        }
                    }
                    else
                    {
                        uniforme.Foto = null;
                    }

                    _uniformeRepository.Update(uniforme);

                    foreach (var item in uniformeSetorRepo)
                    {
                        item.TipoUniformeId = uniforme.TipoUniformeId;
                        _setorUniformeRepository.Update(item);
                    }

                    Success(String.Format("Registro alterado com sucesso!"), true);
                }
            }

            var fichaUniforme = _fichaUniformeRepository.BuscarPorUniforme(uniforme.Id);

            ViewBag.PossuiFicha = false;

            if (fichaUniforme != null)
            {
                ViewBag.PossuiFicha = true;
            }

            return View(uniforme);
        }

        // GET: Uniforme/Delete/5
        public ActionResult Delete(int id)
        {
            var uniforme = _uniformeRepository.GetById(id);

            return View(uniforme);
        }

        // POST: Uniforme/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var uniforme = _uniformeRepository.GetById(id);

            if (_uniformeRepository.EstaEmSetor(uniforme.Id))
            {
                ViewBag.EstaEmSetor = true;
                return View(uniforme);
            }

            _uniformeRepository.RemoveLogical(id);

            return RedirectToAction("Index");
        }

        public ActionResult GravarFicha(string ficha, string operacao)
        {
            var fichaUniforme = JsonConvert.DeserializeObject<FichaUniformeModel>(ficha, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });

            if (operacao == "C")
            {
                _fichaUniformeRepository.Add(fichaUniforme);
            }
            else
            {
                _fichaUniformeRepository.Update(fichaUniforme);
            }

            ViewBag.PossuiFicha = true;

            return PartialView("_BtnFicha");
        }

        public JsonResult BuscarFicha(int idUniforme)
        {
            var fichaUniforme = _fichaUniformeRepository.BuscarPorUniforme(idUniforme);
            var fichaJson = JsonConvert.SerializeObject(fichaUniforme);
            return Json(fichaJson, JsonRequestBehavior.AllowGet);
        }

        //ficha uniforme
        public ActionResult ImprimirFicha(int idUniforme)
        {
            var consultaFicha = ConsultaFichaUniforme.GetConsulta(idUniforme);
            var ficha = _relFichaUniformeRepository.ConsultaEntidades(consultaFicha);

            var ds = new DataSet();

            var conversorDt = new Conversor();
            DataTable dtFichaUniforme = conversorDt.LINQToDataTable<RelFichaUniformeModel>(ficha);

            ds.Tables.Add(dtFichaUniforme);

            ReportDataSource rdsAct = new ReportDataSource("FichaUniforme", ds.Tables[0]);

            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;//`enter code here`
            string extension = string.Empty;

            ReportViewer viewer = new ReportViewer();
            viewer.LocalReport.Refresh();
            viewer.LocalReport.ReportPath = "Reports/UniformeFicha.rdlc"; //This is your rdlc             
            viewer.LocalReport.DataSources.Add(rdsAct); // Add  datasource here  

            byte[] bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);

            return File(bytes, mimeType, "FichaUniforme.pdf");
        }

        public void ExcluirFicha(int idUniforme)
        {
            var ficha = _fichaUniformeRepository.BuscarPorUniforme(idUniforme);
            _fichaUniformeRepository.Remove(ficha);
        }
        //ok

        public ActionResult AlterarEstoque(string est)
        {
            var estoqueTemp = JsonConvert.DeserializeObject<EstoqueAlterado>(est);

            var estoqueProduto = _estoqueRepository.GetById(estoqueTemp.IdEst);

            estoqueProduto.Qtde = estoqueTemp.Qtde;
            estoqueProduto.QtdeMin = estoqueTemp.QtdeMin;
            estoqueProduto.QtdeMax = estoqueTemp.QtdeMax;

            _estoqueRepository.Update(estoqueProduto);

            var estoquesVm = _estoqueRepository.BuscarPorProduto(estoqueProduto.IdUniforme);

            return PartialView("_Estoques", estoquesVm);
        }

        public ActionResult AddEstoque(int idEst, decimal qtde)
        {
            var estoqueProduto = _estoqueRepository.GetById(idEst);

            estoqueProduto.AtualizarEstoque(qtde);

            _estoqueRepository.Update(estoqueProduto);

            var estoquesVm = _estoqueRepository.BuscarPorProduto(estoqueProduto.IdUniforme);

            return PartialView("_Estoques", estoquesVm);
        }
    }
}
