using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using CrystalDecisions.CrystalReports.Engine;
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
using static TitansMVC.Models.EpiModel;

namespace TitansMVC.Controllers
{
    [Authorize(Roles = "role_base")]
    public class EpiController : BaseController
    {
        private readonly IEpiRepository _epiRepository;
        private readonly IFamiliaEpiRepository _familiaEpiRepository;
        private readonly ITipoEpiRepository _tipoEpiRepository;
        private readonly IFichaEpiRepository _fichaEpiRepository;
        private readonly IRelFichaEpiRepository _relFichaEpiRepository;
        private readonly IEstoqueEpiRepository _estoqueRepository;
        private readonly ILbcRepository _lbcRepository;
        private readonly IEpiSetorRepository _setorEpiRepository;

        public EpiController()
        {
            _epiRepository = new EpiRepository();
            _familiaEpiRepository = new FamiliaEpiRepository();
            _tipoEpiRepository = new TipoEpiRepository();
            _fichaEpiRepository = new FichaEpiRepository();
            _relFichaEpiRepository = new RelFichaEpiRepository();
            _estoqueRepository = new EstoqueEpiRepository();
            _lbcRepository = new LbcRepository();
            _setorEpiRepository = new EpiSetorRepository();
        }

        // GET: Epi
        public ActionResult Index(string searchBy, bool? ativos, string search, int? page)
        {

            IEnumerable<EpiModel> epis;                    

            if (searchBy == "Nome")
            {
                epis = _epiRepository.BuscarPorNome(nome: search, ativos: ativos.HasValue ? ativos.Value : true);
            }
            else if (searchBy == "Todos")
            {
                epis = _epiRepository.GetAll(ativos.HasValue ? ativos.Value : true);
            }
            else
            {
                epis = _epiRepository.BuscarAtivos();
            }
            return View(epis.ToPagedList(page ?? 1, 10));
        }

        // GET: Epi/Create
        public ActionResult Create()
        {
            //ViewBag.FamiliaId = new SelectList(_familiaEpiRepository.BuscarAtivos(), "Id", "Nome");
            ViewBag.TipoEpiId = new SelectList(_tipoEpiRepository.BuscarAtivos(), "Id", "Nome");
            ViewBag.UnidadeNegocioId = new SelectList(_lbcRepository.BuscarAtivos(), "Id", "Fantasia");

            return View();
        }

        // POST: Epi/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EpiModel epi, HttpPostedFileBase uploadfoto)
        {
            if (ModelState.IsValid)
            {
                epi.Ativo = true;
                //if (_epiRepository.BuscarPorCodigoDeBarras(epi.CodigoDeBarras) != null)
                if (_epiRepository.ValidarCodigoDeBarras(epi.CodigoDeBarras, epi.Id) != null)
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
                                epi.Foto = reader.ReadBytes(uploadfoto.ContentLength);
                            }
                        }
                        catch (RetryLimitExceededException /* dex */)
                        {
                            ModelState.AddModelError("", "Não foi possível salvar as alterações. Tente novamente, e se o problema persistir entre em contato com o administrador do sistema.");
                        }
                    }
                    else
                    {
                        epi.Foto = null;
                    }


                    _epiRepository.Add(epi);

                    Success(String.Format("Registro gravado com sucesso!"), true);

                    return RedirectToAction("Edit", epi);
                }
            }

            //ViewBag.FamiliaId = new SelectList(_familiaEpiRepository.BuscarAtivos(), "Id", "Nome");
            ViewBag.TipoEpiId = new SelectList(_tipoEpiRepository.BuscarAtivos(), "Id", "Nome");
            ViewBag.UnidadeNegocioId = new SelectList(_lbcRepository.BuscarAtivos(), "Id", "Fantasia");
            return View(epi);
        }

        // GET: Epi/Edit/5
        public ActionResult Edit(int id)
        {
            //var epi = _epiRepository.GetById(id);
            var epi = _epiRepository.GetByIdEager(id);

            //ViewBag.FamiliaId = new SelectList(_familiaEpiRepository.BuscarAtivos(), "Id", "Nome", epi.FamiliaId);
            ViewBag.TipoEpiId = new SelectList(_tipoEpiRepository.BuscarAtivos(), "Id", "Nome", epi.TipoEpiId);
            ViewBag.UnidadeNegocioId = new SelectList(_lbcRepository.BuscarAtivos(), "Id", "Fantasia", epi.UnidadeNegocioId);
            var fichaEpi = _fichaEpiRepository.BuscarPorEpi(epi.Id);

            ViewBag.PossuiFicha = false;

            if (fichaEpi != null)
            {
                ViewBag.PossuiFicha = true;
            }

            return View(epi);
        }

        // POST: Epi/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EpiModel epi, HttpPostedFileBase uploadfoto)
        {
            //var epiRepo = _epiRepository.GetById(epi.Id);
            var epiRepo = _epiRepository.GetByIdEager(epi.Id);
            var epiSetorRepo = (List<EpiSetorModel>)_setorEpiRepository.BuscarPorEpi(epi.Id);

            epi.Foto = epiRepo.Foto;

            _epiRepository.RetirarDoContexto(epiRepo);

            //ViewBag.FamiliaId = new SelectList(_familiaEpiRepository.BuscarAtivos(), "Id", "Nome", epi.FamiliaId);
            ViewBag.TipoEpiId = new SelectList(_tipoEpiRepository.BuscarAtivos(), "Id", "Nome", epi.TipoEpiId);
            ViewBag.UnidadeNegocioId = new SelectList(_lbcRepository.BuscarAtivos(), "Id", "Fantasia");

            if (ModelState.IsValid)
            {                
                if (_epiRepository.ValidarCodigoDeBarras(epi.CodigoDeBarras, epi.Id) != null)
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
                                epi.Foto = reader.ReadBytes(uploadfoto.ContentLength);
                            }
                        }
                        catch (RetryLimitExceededException /* dex */)
                        {
                            ModelState.AddModelError("", "Não foi possível salvar as alterações. Tente novamente, e se o problema persistir entre em contato com o administrador do sistema.");
                        }
                    }
                    else
                    {
                        epi.Foto = null;
                    }

                    _epiRepository.Update(epi);

                    foreach (var item in epiSetorRepo)
                    {
                        item.TipoEpiId = epi.TipoEpiId;
                        _setorEpiRepository.Update(item);
                    }

                    Success(String.Format("Registro alterado com sucesso!"), true);
                }
            }

            var fichaEpi = _fichaEpiRepository.BuscarPorEpi(epi.Id);

            ViewBag.PossuiFicha = false;

            if (fichaEpi != null)
            {
                ViewBag.PossuiFicha = true;
            }

            return View(epi);
        }

        // GET: Epi/Delete/5
        public ActionResult Delete(int id)
        {
            var epi = _epiRepository.GetById(id);

            return View(epi);
        }

        // POST: Epi/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var epi = _epiRepository.GetById(id);

            if (_epiRepository.EstaEmSetor(epi.Id))
            {
                ViewBag.EstaEmSetor = true;
                return View(epi);
            }

            //_epiRepository.Remove(epi);

            _epiRepository.RemoveLogical(id);

            return RedirectToAction("Index");
        }

        //public ActionResult GravarFicha(string ficha, string foto)
        public ActionResult GravarFicha(string ficha, string operacao)
        {
            var fichaEpi = JsonConvert.DeserializeObject<FichaEpiModel>(ficha, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });

            if (operacao == "C")
            {
                _fichaEpiRepository.Add(fichaEpi);
            }
            else
            {
                _fichaEpiRepository.Update(fichaEpi);
            }

            ViewBag.PossuiFicha = true;

            return PartialView("_BtnFicha");
        }

        public JsonResult BuscarFicha(int idEpi)
        {
            var fichaEpi = _fichaEpiRepository.BuscarPorEpi(idEpi);
            var fichaJson = JsonConvert.SerializeObject(fichaEpi);
            return Json(fichaJson, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult ImprimirFicha(int idEpi)
        //{
        //    var consultaFicha = ConsultaFichaEpi.GetConsulta(idEpi);
        //    var ficha = _relFichaEpiRepository.ConsultaEntidades(consultaFicha);

        //    var ds = new DataSet();

        //    var conversorDt = new Conversor();
        //    DataTable dtFichaEpi = conversorDt.LINQToDataTable<RelFichaEpiModel>(ficha);

        //    ds.Tables.Add(dtFichaEpi);

        //    ReportDocument rd = new ReportDocument();

        //    rd.Load(Path.Combine(Server.MapPath("~/Reports"), "FichaEpi.rpt"));
        //    //rd.SetDataSource(ds);
        //    rd.Database.Tables[0].SetDataSource(dtFichaEpi);
        //    rd.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Portrait;
        //    Response.Buffer = false;
        //    Response.ClearContent();
        //    Response.ClearHeaders();
        //    try
        //    {
        //        Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
        //        stream.Seek(0, SeekOrigin.Begin);
        //        return File(stream, "application/pdf", "FichaEpi.pdf");
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        public ActionResult ImprimirFicha(int idEpi)
        {
            var consultaFicha = ConsultaFichaEpi.GetConsulta(idEpi);
            var ficha = _relFichaEpiRepository.ConsultaEntidades(consultaFicha);

            var ds = new DataSet();

            var conversorDt = new Conversor();
            DataTable dtFichaEpi = conversorDt.LINQToDataTable<RelFichaEpiModel>(ficha);

            ds.Tables.Add(dtFichaEpi);

            ReportDataSource rdsAct = new ReportDataSource("FichaEpi", ds.Tables[0]);

            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;//`enter code here`
            string extension = string.Empty;

            ReportViewer viewer = new ReportViewer();
            viewer.LocalReport.Refresh();
            viewer.LocalReport.ReportPath = "Reports/EpiFicha.rdlc"; //This is your rdlc 
            //viewer.LocalReport.SetParameters(param);
            viewer.LocalReport.DataSources.Add(rdsAct); // Add  datasource here  

            byte[] bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);

            return File(bytes, mimeType, "FichaEpi.pdf");
        }

        public void ExcluirFicha(int idEpi)
        {
            var ficha = _fichaEpiRepository.BuscarPorEpi(idEpi);
            _fichaEpiRepository.Remove(ficha);
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

            var estoquesVm = _estoqueRepository.BuscarPorProduto(estoqueProduto.IdEpi);

            return PartialView("_Estoques", estoquesVm);
        }

        public ActionResult AddEstoque(int idEst, decimal qtde)
        {
            var estoqueProduto = _estoqueRepository.GetById(idEst);

            estoqueProduto.AtualizarEstoque(qtde);

            _estoqueRepository.Update(estoqueProduto);

            var estoquesVm = _estoqueRepository.BuscarPorProduto(estoqueProduto.IdEpi);

            return PartialView("_Estoques", estoquesVm);
        }
    }
}
