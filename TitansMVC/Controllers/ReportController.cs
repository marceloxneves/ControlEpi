using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using Newtonsoft.Json;
using TitansMVC.Consultas;
using TitansMVC.Models.Enums;
using TitansMVC.Models.Relatorios;
using TitansMVC.Repository.Implementations;
using TitansMVC.Repository.Interfaces;
using TitansMVC.Utils;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using TitansMVC.Models;
using Newtonsoft.Json.Converters;

namespace TitansMVC.Controllers
{
    [Authorize]
    public class ReportController : BaseController
    {
        private readonly IColaboradorRepository _colaboradorRepository;
        private readonly IEpiRepository _epiRepository;
        private readonly ITipoEpiRepository _tipoEpiRepository;
        private readonly ISetorRepository _setorRepository;
        private readonly ILbcRepository _lbcRepository;
        private readonly ICentroCustoRepository _centroCustoRepository;
        private readonly IRelEpiColaboradorRepository _relEpiColaboradorRepository;
        private readonly IRelConsumoEpiRepository _relConsumoEpiRepository;
        private readonly IRelEmpresaRepository _relEmpresaRepository;
        private readonly IRelEpiFaltaRepository _relEpiFaltaRepository;
        private readonly IUniformeRepository _uniformeRepository;
        private readonly ITipoUniformeRepository _tipoUniformeRepository;
        private readonly IRelUniformeColaboradorRepository _relUniformeColaboradorRepository;
        private readonly IRelConsumoUniformeRepository _relConsumoUniformeRepository;
        private readonly IRelUniformeFaltaRepository _relUniformeFaltaRepository;

        public string filterGlobal;
        private ColaboradorEpiFilter filtroGlobal;
        public string filterGlobalUniforme;
        private ColaboradorUniformeFilter filtroUniformeGlobal;

        public ReportController()
        {
            _colaboradorRepository = new ColaboradorRepository();
            _epiRepository = new EpiRepository();
            _setorRepository = new SetorRepository();
            _lbcRepository = new LbcRepository();
            _centroCustoRepository = new CentroCustoRepository();
            _tipoEpiRepository = new TipoEpiRepository();
            _relEpiColaboradorRepository = new RelEpiColaboradorRepository();
            _relConsumoEpiRepository = new RelConsumoEpiRepository();
            _relEmpresaRepository = new RelEmpresaRepository();
            _relEpiFaltaRepository = new RelEpiFaltaRepository();
            _uniformeRepository = new UniformeRepository();
            _tipoUniformeRepository = new TipoUniformeRepository();
            _relUniformeColaboradorRepository = new RelUniformeColaboradorRepository();
            _relConsumoUniformeRepository = new RelConsumoUniformeRepository();
            _relUniformeFaltaRepository = new RelUniformeFaltaRepository();

        }

        [Authorize(Roles = "role_relatorios")]
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "role_relatorios")]
        public ActionResult EpiColaboradorViewer(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.ColaboradorId = new SelectList(_colaboradorRepository.GetAll(), "Id", "Nome");
            ViewBag.EpiId = new SelectList(_epiRepository.BuscarAtivos(), "Id", "Nome");
            ViewBag.TipoEpiId = new SelectList(_tipoEpiRepository.BuscarAtivos(), "Id", "Nome");
            ViewBag.SetorId = new SelectList(_setorRepository.GetAll(), "Id", "Nome");
            ViewBag.CentroCustoId = new SelectList(_centroCustoRepository.BuscarAtivos(), "Id", "Nome");
            ViewBag.UnidadeNegocioId = new SelectList(_lbcRepository.BuscarAtivos(), "Id", "Fantasia");

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "role_relatorios")]
        public ActionResult EpiColaboradorViewer(ColaboradorEpiFilter epiColFilter)
        {
            return RedirectToAction("VisualizarRelEpiColaborador", epiColFilter);
        }

        [Authorize(Roles = "role_relatorios")]
        public ActionResult VisualizarRelEpiColaborador(ColaboradorEpiFilter filtro)
        {
            var consulta = ConsultaEpiCol.getConsulta(filtro);
            var episColaboradores = _relEpiColaboradorRepository.ConsultaEntidades(consulta);

            ViewBag.FiltroEpi = filtro;

            if (filtro.EstadoEpis == EstadoEpisConsulta.Entregues)
            {
                return View(episColaboradores.Where(e => !e.baixado));
            }
            if (filtro.EstadoEpis == EstadoEpisConsulta.Baixados)
            {
                return View(episColaboradores.Where(e => e.baixado));
            }

            return View(episColaboradores);
        }

        [Authorize(Roles = "role_relatorios")]
        public ActionResult ConsumoEpiViewer(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.ColaboradorId = new SelectList(_colaboradorRepository.GetAll(), "Id", "Nome");
            ViewBag.EpiId = new SelectList(_epiRepository.BuscarAtivos(), "Id", "Nome");
            ViewBag.TipoEpiId = new SelectList(_tipoEpiRepository.BuscarAtivos(), "Id", "Nome");
            ViewBag.SetorId = new SelectList(_setorRepository.GetAll(), "Id", "Nome");
            ViewBag.CentroCustoId = new SelectList(_centroCustoRepository.BuscarAtivos(), "Id", "Nome");
            ViewBag.UnidadeNegocioId = new SelectList(_lbcRepository.BuscarAtivos(), "Id", "Fantasia");

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "role_relatorios")]
        public ActionResult ConsumoEpiViewer(ConsumoEpiFilter conEpiFilter)
        {

            ViewBag.ColaboradorId = new SelectList(_colaboradorRepository.BuscarAtivos(), "Id", "Nome");
            ViewBag.EpiId = new SelectList(_epiRepository.BuscarAtivos(), "Id", "Nome");
            ViewBag.TipoEpiId = new SelectList(_tipoEpiRepository.BuscarAtivos(), "Id", "Nome");
            ViewBag.SetorId = new SelectList(_setorRepository.BuscarAtivos(), "Id", "Nome");
            ViewBag.CentroCustoId = new SelectList(_centroCustoRepository.BuscarAtivos(), "Id", "Nome");
            ViewBag.UnidadeNegocioId = new SelectList(_lbcRepository.BuscarAtivos(), "Id", "Fantasia");

            //if (String.IsNullOrWhiteSpace(conEpiFilter.EpiId.ToString()) && String.IsNullOrWhiteSpace(conEpiFilter.ColaboradorId.ToString()))
            //{
            //    Warning("O campo EPI ou o campo Colaborador precisa ser preechido", true);
            //    return View(conEpiFilter);
            //}

            return RedirectToAction("VisualizarRelConsumoEpi", conEpiFilter);
        }

        [Authorize(Roles = "role_relatorios")]
        public ActionResult VisualizarRelConsumoEpi(ConsumoEpiFilter filtro)
        {
            var consulta = ConsultaConsumoEpi.GetConsulta(filtro);
            var consumoEpis = _relConsumoEpiRepository.ConsultaEntidades(consulta);

            ViewBag.FiltroEpi = filtro;

            return View(consumoEpis);
        }

        [Authorize(Roles = "role_relatorios")]
        public ActionResult DownloadReportConsumoEpi(string filtro)
        {
            filterGlobal = filtro;
            var filtroConsumoEpi = new ConsumoEpiFilter();

            if (filtro != null)
            {
                var format = "dd/MM/yyyy"; // your datetime format
                var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = format };
                filtroConsumoEpi = JsonConvert.DeserializeObject<ConsumoEpiFilter>(filtro, dateTimeConverter);
            }

            var consultaConsumoEpi = ConsultaConsumoEpi.GetConsulta(filtroConsumoEpi);
            var consumoEpis = _relConsumoEpiRepository.ConsultaEntidades(consultaConsumoEpi);
            var consultaEmpresa = ConsultaEmpresa.GetConsulta();
            var empresa = _relEmpresaRepository.ConsultaEntidades(consultaEmpresa);

            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;

            ////This is optional if you have parameter then you can add parameters as much as you want
            //ReportParameter[] param = new ReportParameter[5];
            //param[0] = new ReportParameter("Report_Parameter_0", "1st Para", true);
            //param[1] = new ReportParameter("Report_Parameter_1", "2nd Para", true);
            //param[2] = new ReportParameter("Report_Parameter_2", "3rd Para", true);
            //param[3] = new ReportParameter("Report_Parameter_3", "4th Para", true);
            //param[4] = new ReportParameter("Report_Parameter_4", "5th Para");

            //exemplo de uso:
            //parameter[18] = new ReportParameter("rp_logo", new string[]{dataImage});

            DataSet dsData = new DataSet();
            DataTable dtConsumoEpi;

            var conversorDt = new Conversor();

            dtConsumoEpi = conversorDt.LINQToDataTable<RelConsumoEpiModel>(consumoEpis);

            DataTable dtEmpresa = conversorDt.LINQToDataTable<RelEmpresaModel>(empresa);

            dsData.Tables.Add(dtConsumoEpi);
            dsData.Tables.Add(dtEmpresa);

            ReportDataSource rdsAct = new ReportDataSource("ConsumoEpi", dsData.Tables[0]);
            ReportDataSource rdsEmpr = new ReportDataSource("dsEmpresa", dsData.Tables[1]);

            ReportViewer viewer = new ReportViewer();
            viewer.LocalReport.Refresh();
            viewer.LocalReport.ReportPath = "Reports/ConsumoEpi.rdlc"; //This is your rdlc 
            //viewer.LocalReport.SetParameters(param);
            viewer.LocalReport.DataSources.Add(rdsAct); // Add  datasource here  
            viewer.LocalReport.DataSources.Add(rdsEmpr); // Add  datasource here

            byte[] bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension,
                out streamIds, out warnings);

            return File(bytes, mimeType, "ConsumoEpi.pdf");
        }


        [Authorize(Roles = "role_relatorios")]
        public ActionResult EpiFaltaViewer(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.EpiId = new SelectList(_epiRepository.BuscarAtivos(), "Id", "Nome");

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "role_relatorios")]
        public ActionResult EpiFaltaViewer(RelEpiFaltaFilter epiFaltaFilter)
        {
            return RedirectToAction("VisualizarRelEpiFalta", epiFaltaFilter);
        }

        [Authorize(Roles = "role_relatorios")]
        public ActionResult VisualizarRelEpiFalta(RelEpiFaltaFilter filtro)
        {
            var consulta = ConsultaFaltaEpi.GetConsulta(filtro);
            var episFalta = _relEpiFaltaRepository.ConsultaEntidades(consulta);

            ViewBag.FiltroEpi = filtro;

            return View(episFalta);
        }


        [Authorize(Roles = "role_relatorios")]
        public ActionResult DownloadReportEpiFalta(string filtro)
        {
            var filtroEpiFalta = new RelEpiFaltaFilter();

            if (filtro != null)
            {
                var format = "dd/MM/yyyy"; // your datetime format
                var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = format };
                filtroEpiFalta = JsonConvert.DeserializeObject<RelEpiFaltaFilter>(filtro, dateTimeConverter);
            }

            var consultaEpiFalta = ConsultaFaltaEpi.GetConsulta(filtroEpiFalta);
            var episFalta = _relEpiFaltaRepository.ConsultaEntidades(consultaEpiFalta);
            var consultaEmpresa = ConsultaEmpresa.GetConsulta();
            var empresa = _relEmpresaRepository.ConsultaEntidades(consultaEmpresa);

            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;

            ////This is optional if you have parameter then you can add parameters as much as you want
            //ReportParameter[] param = new ReportParameter[5];
            //param[0] = new ReportParameter("Report_Parameter_0", "1st Para", true);
            //param[1] = new ReportParameter("Report_Parameter_1", "2nd Para", true);
            //param[2] = new ReportParameter("Report_Parameter_2", "3rd Para", true);
            //param[3] = new ReportParameter("Report_Parameter_3", "4th Para", true);
            //param[4] = new ReportParameter("Report_Parameter_4", "5th Para");

            //exemplo de uso:
            //parameter[18] = new ReportParameter("rp_logo", new string[]{dataImage});

            DataSet dsData = new DataSet();
            DataTable dtEpiFalta;

            var conversorDt = new Conversor();

            dtEpiFalta = conversorDt.LINQToDataTable<RelEpiFaltaModel>(episFalta);

            DataTable dtEmpresa = conversorDt.LINQToDataTable<RelEmpresaModel>(empresa);

            dsData.Tables.Add(dtEpiFalta);
            dsData.Tables.Add(dtEmpresa);

            ReportDataSource rdsAct = new ReportDataSource("dsEpiFalta", dsData.Tables[0]);
            ReportDataSource rdsEmpr = new ReportDataSource("dsEmpresa", dsData.Tables[1]);

            ReportViewer viewer = new ReportViewer();
            viewer.LocalReport.Refresh();
            viewer.ProcessingMode = ProcessingMode.Local;
            //viewer.LocalReport.ReportPath = "Reports/EpiFalta.rdlc";
            viewer.LocalReport.ReportPath = Path.Combine(Server.MapPath("~/Reports"), "EpiFalta.rdlc");
            //viewer.LocalReport.SetParameters(param);
            viewer.LocalReport.DataSources.Add(rdsAct);
            viewer.LocalReport.DataSources.Add(rdsEmpr);

            byte[] bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension,
                out streamIds, out warnings);

            return File(bytes, mimeType, "EpiFalta.pdf");
        }


        [HttpGet]
        [Authorize(Roles = "role_relatorios")]
        public CrystalReportPdfResult Pdf()
        {
            List<RelConsumoEpiModel> model = new List<RelConsumoEpiModel>();
            model.Add(new RelConsumoEpiModel
            {
                nome_colaborador = "ColTeste" ?? "Teste",
                nome_ccusto = "CCTeste" ?? "Teste",
                nome_epi = "EPITeste" ?? "Teste",
                baixado = false,
                epi_ca = "123124" ?? "Teste",
                epi_custo = 10,
                id_centro_custo = 2,
                id_colaborador = 1,
                id_colaborador_epi = 1,
                qtde_epis = 1
            });
            string reportPath = Path.Combine(Server.MapPath("~/Reports"), "CrystalReportTest.rpt");
            return new CrystalReportPdfResult(reportPath, model);
        }

        //public ActionResult ExportToExcel(string filtro)
        //{
        //    var filtroConsumoEpi = new ConsumoEpiFilter();

        //    if (filtro != null)
        //    {
        //        filtroConsumoEpi = JsonConvert.DeserializeObject<ConsumoEpiFilter>(filtro);
        //    }

        //    var consultaConsumoEpi = ConsultaConsumoEpi.GetConsulta(filtroConsumoEpi);
        //    var consumoEpis = _relConsumoEpiRepository.ConsultaEntidades(consultaConsumoEpi);

        //    var consultaEmpresa = ConsultaEmpresa.GetConsulta();
        //    var empresa = _relEmpresaRepository.ConsultaEntidades(consultaEmpresa);

        //    DataSet dsData = new DataSet();
        //    DataTable dtConsumoEpi;

        //    var conversorDt = new Conversor();

        //    dtConsumoEpi = conversorDt.LINQToDataTable<RelConsumoEpiModel>(consumoEpis);

        //    DataTable dtEmpresa = conversorDt.LINQToDataTable<RelEmpresaModel>(empresa);

        //    dsData.Tables.Add(dtConsumoEpi);
        //    dsData.Tables.Add(dtEmpresa);


        //    var grid = new GridView();
        //    grid.DataSource = dsData;
        //    grid.DataBind();

        //    Response.ClearContent();
        //    Response.Buffer = true;
        //    Response.AddHeader("content-disposition", "attachment; filename=RelatorioConsumoEPI.xls");
        //    Response.ContentType = "application/ms-excel";

        //    Response.Charset = "";
        //    StringWriter sw = new StringWriter();
        //    HtmlTextWriter htw = new HtmlTextWriter(sw);

        //    grid.RenderControl(htw);

        //    Response.Output.Write(sw.ToString());
        //    Response.Flush();
        //    Response.End();

        //    return Content(sw.ToString(), "application/ms-excel");
        //}

        [Authorize(Roles = "role_relatorios")]
        public void ExportToExcelConsumo(string filtro)
        {
            var filtroConsumoEpi = new ConsumoEpiFilter();

            if (filtro != null)
            {
                var format = "dd/MM/yyyy"; // your datetime format
                var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = format };
                filtroConsumoEpi = JsonConvert.DeserializeObject<ConsumoEpiFilter>(filtro, dateTimeConverter);
            }

            var consultaConsumoEpi = ConsultaConsumoEpi.GetConsulta(filtroConsumoEpi);
            var consumoEpis = _relConsumoEpiRepository.ConsultaEntidades(consultaConsumoEpi);

            //var consultaEmpresa = ConsultaEmpresa.GetConsulta();
            //var empresa = _relEmpresaRepository.ConsultaEntidades(consultaEmpresa);
            //DataSet p_dsSrc = new DataSet();

            var conversorDt = new Conversor();

            DataTable dtConsumoEpi = conversorDt.LINQToDataTable<RelConsumoEpiModel>(consumoEpis);
            dtConsumoEpi.Columns.RemoveAt(0); //id_unidade_negocio
            dtConsumoEpi.Columns.RemoveAt(0); //id_colaborador
            dtConsumoEpi.Columns.RemoveAt(0); //id_colaborador_epi
            dtConsumoEpi.Columns.RemoveAt(0); //id_centro_custo
            dtConsumoEpi.Columns.RemoveAt(11); //razao_social
            //DataTable dtEmpresa = conversorDt.LINQToDataTable<RelEmpresaModel>(empresa);

            //p_dsSrc.Tables.Add(dtConsumoEpi);
            //p_dsSrc.Tables.Add(dtEmpresa);

            using (ExcelPackage pck = new ExcelPackage())
            {
                //Create the worksheet
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("ConsumoEpi");

                //Load the datatable into the sheet, starting from cell A1. Print the column names on row 1
                ws.Cells["A1"].LoadFromDataTable(dtConsumoEpi, true);

                //Format the header for column 1-3
                using (ExcelRange rng = ws.Cells["A1:M1"])
                {
                    rng.Style.Font.Bold = true;
                    rng.Style.Fill.PatternType = ExcelFillStyle.Solid;                      //Set Pattern for the background to Solid
                    rng.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(79, 129, 189));  //Set color to dark blue
                    rng.Style.Font.Color.SetColor(Color.White);
                }

                ws.Cells["A1"].Value = "Colaborador";
                ws.Cells["B1"].Value = "EPI";
                ws.Cells["C1"].Value = "Quantidade";
                ws.Cells["D1"].Value = "CA";
                ws.Cells["E1"].Value = "Custo";
                ws.Cells["F1"].Value = "Baixado";
                ws.Cells["G1"].Value = "Centro de Custo";
                ws.Cells["H1"].Value = "Custo Total";
                ws.Cells["I1"].Value = "Setor";
                ws.Cells["J1"].Value = "Função";
                ws.Cells["K1"].Value = "Nome Fantasia";
                ws.Cells["L1"].Value = "Justificativa";
                ws.Cells["M1"].Value = "Justificativa Baixa";

                ws.Cells[ws.Dimension.Address].AutoFitColumns();

                //Example how to Format Column 1 as numeric 
                //using (ExcelRange col = ws.Cells[2, 1, 2 + dtConsumoEpi.Rows.Count, 1])
                //{
                //    col.Style.Numberformat.Format = "#,##0.00";
                //    col.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //}

                //Write it back to the client
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=ConsumoEpi.xlsx");
                Response.BinaryWrite(pck.GetAsByteArray());
            }
        }

        [Authorize(Roles = "role_relatorios")]
        public ActionResult DownloadReport(string filtro)
        {
            filterGlobal = filtro;
            var filtroEpiColaborador = new ColaboradorEpiFilter();

            if (filtro != null)
            {
                var format = "dd/MM/yyyy"; // your datetime format
                var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = format };
                filtroEpiColaborador = JsonConvert.DeserializeObject<ColaboradorEpiFilter>(filtro, dateTimeConverter);
                //, new IsoDateTimeConverter() { DateTimeFormat = "dd/MM/yyyy" }
            }

            var consultaEpiCol = ConsultaEpiCol.getConsulta(filtroEpiColaborador);
            var episColaboradores = _relEpiColaboradorRepository.ConsultaEntidades(consultaEpiCol);
            episColaboradores = episColaboradores.GroupBy(ec => ec.id_colaborador).Select(ec => ec.First());
            var consultaEmpresa = ConsultaEmpresa.GetConsulta();
            var empresa = _relEmpresaRepository.ConsultaEntidades(consultaEmpresa);

            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;//`enter code here`
            string extension = string.Empty;

            ////This is optional if you have parameter then you can add parameters as much as you want
            //ReportParameter[] param = new ReportParameter[5];
            //param[0] = new ReportParameter("Report_Parameter_0", "1st Para", true);
            //param[1] = new ReportParameter("Report_Parameter_1", "2nd Para", true);
            //param[2] = new ReportParameter("Report_Parameter_2", "3rd Para", true);
            //param[3] = new ReportParameter("Report_Parameter_3", "4th Para", true);
            //param[4] = new ReportParameter("Report_Parameter_4", "5th Para");

            //exemplo de uso:
            //parameter[18] = new ReportParameter("rp_logo", new string[]{dataImage});

            DataSet dsData = new DataSet();
            DataTable dtEpiColaborador;

            var conversorDt = new Conversor();

            switch (filtroEpiColaborador.EstadoEpis)
            {
                case EstadoEpisConsulta.Entregues:
                    dtEpiColaborador = conversorDt.LINQToDataTable<RelEpiColModel>(episColaboradores.Where(e => !e.baixado));
                    break;

                case EstadoEpisConsulta.Baixados:
                    dtEpiColaborador = conversorDt.LINQToDataTable<RelEpiColModel>(episColaboradores.Where(e => e.baixado));
                    break;
                default:
                    dtEpiColaborador = conversorDt.LINQToDataTable<RelEpiColModel>(episColaboradores);
                    break;
            }

            DataTable dtEmpresa = conversorDt.LINQToDataTable<RelEmpresaModel>(empresa);

            dsData.Tables.Add(dtEpiColaborador);
            dsData.Tables.Add(dtEmpresa);

            ReportDataSource rdsAct = new ReportDataSource("EpixCol", dsData.Tables[0]);
            ReportDataSource rdsEmpr = new ReportDataSource("dsEmpresa", dsData.Tables[1]);

            ReportViewer viewer = new ReportViewer();
            viewer.LocalReport.Refresh();
            viewer.LocalReport.ReportPath = "Reports/EpiColaboradorSint.rdlc";
            //viewer.LocalReport.SetParameters(param);
            viewer.LocalReport.DataSources.Add(rdsAct);
            viewer.LocalReport.DataSources.Add(rdsEmpr);
            //viewer.LocalReport.SetParameters(new ReportParameter("nome_colaborador", "ADEMIR DA SILVA"));
            viewer.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(RenderizaSubRelatorio);

            byte[] bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);

            return File(bytes, mimeType, "EpiColaborador.pdf");
        }

        [Authorize(Roles = "role_entregaepi, role_entrega_epi_outros")]
        public ActionResult GerarComprovanteEntregaEpi(DateTime dataInicial, DateTime dataFinal, int idColaborador)
        {

            var filtroEpiColaborador = new ColaboradorEpiFilter();
            filtroEpiColaborador.ColaboradorId = idColaborador;
            filtroEpiColaborador.DataFinal = dataFinal;
            filtroEpiColaborador.DataInicial = dataInicial;
            filtroEpiColaborador.Ativos = true;
            filtroEpiColaborador.EstadoEpis = EstadoEpisConsulta.Entregues;
            filtroGlobal = filtroEpiColaborador;
            var consultaEpiCol = ConsultaEpiCol.getConsulta(filtroEpiColaborador);
            var episColaboradores = _relEpiColaboradorRepository.ConsultaEntidades(consultaEpiCol);
            episColaboradores = episColaboradores.GroupBy(ec => ec.id_colaborador).Select(ec => ec.First());
            var consultaEmpresa = ConsultaEmpresa.GetConsulta();
            var empresa = _relEmpresaRepository.ConsultaEntidades(consultaEmpresa);

            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;//`enter code here`
            string extension = string.Empty;

            ////This is optional if you have parameter then you can add parameters as much as you want
            //ReportParameter[] param = new ReportParameter[5];
            //param[0] = new ReportParameter("Report_Parameter_0", "1st Para", true);
            //param[1] = new ReportParameter("Report_Parameter_1", "2nd Para", true);
            //param[2] = new ReportParameter("Report_Parameter_2", "3rd Para", true);
            //param[3] = new ReportParameter("Report_Parameter_3", "4th Para", true);
            //param[4] = new ReportParameter("Report_Parameter_4", "5th Para");

            //exemplo de uso:
            //parameter[18] = new ReportParameter("rp_logo", new string[]{dataImage});

            DataSet dsData = new DataSet();
            DataTable dtEpiColaborador;

            var conversorDt = new Conversor();

            switch (filtroEpiColaborador.EstadoEpis)
            {
                case EstadoEpisConsulta.Entregues:
                    dtEpiColaborador = conversorDt.LINQToDataTable<RelEpiColModel>(episColaboradores.Where(e => !e.baixado));
                    break;

                case EstadoEpisConsulta.Baixados:
                    dtEpiColaborador = conversorDt.LINQToDataTable<RelEpiColModel>(episColaboradores.Where(e => e.baixado));
                    break;
                default:
                    dtEpiColaborador = conversorDt.LINQToDataTable<RelEpiColModel>(episColaboradores);
                    break;
            }

            DataTable dtEmpresa = conversorDt.LINQToDataTable<RelEmpresaModel>(empresa);

            dsData.Tables.Add(dtEpiColaborador);
            dsData.Tables.Add(dtEmpresa);

            ReportDataSource rdsAct = new ReportDataSource("EpixCol", dsData.Tables[0]);
            ReportDataSource rdsEmpr = new ReportDataSource("dsEmpresa", dsData.Tables[1]);

            ReportViewer viewer = new ReportViewer();
            viewer.LocalReport.Refresh();
            viewer.LocalReport.ReportPath = "Reports/EpiColaboradorSint.rdlc";
            //viewer.LocalReport.SetParameters(param);
            viewer.LocalReport.DataSources.Add(rdsAct);
            viewer.LocalReport.DataSources.Add(rdsEmpr);
            //viewer.LocalReport.SetParameters(new ReportParameter("nome_colaborador", "ADEMIR DA SILVA"));
            viewer.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(RenderizaSubRelatorio);
            byte[] bytes=null;
            try
            {
                bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
            }catch(Exception ex)
            {
                while (ex != null)
                {
                    Warning(ex.Message);
                    ex = ex.InnerException;
                }
                return RedirectToAction("Index","Home");

            }
            return File(bytes, mimeType, "Comprovante-" + filtroEpiColaborador.ColaboradorId.ToString() + "-" + filtroEpiColaborador.DataFinal.ToString() + ".pdf");
        }

        [Authorize(Roles = "role_relatorios, role_entregaepi, role_entrega_epi_outros")]
        private void RenderizaSubRelatorio(object sender, SubreportProcessingEventArgs e)
        {
            var filtro = filterGlobal;
            int idColaborador = int.Parse(e.Parameters["id_colaborador"].Values[0].ToString());
            var filtroEpiColaborador = new ColaboradorEpiFilter();

            if (filtro != null)
            {
                var format = "dd/MM/yyyy"; // your datetime format
                var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = format };
                filtroEpiColaborador = JsonConvert.DeserializeObject<ColaboradorEpiFilter>(filtro, dateTimeConverter);
            }
            else
            {
                filtroEpiColaborador = filtroGlobal;
            }

            var consultaEpiCol = ConsultaEpiCol.getConsulta(filtroEpiColaborador);
            var episColaboradores = _relEpiColaboradorRepository.ConsultaEntidades(consultaEpiCol);
            episColaboradores = episColaboradores.Distinct();
            var consultaEmpresa = ConsultaEmpresa.GetConsulta();
            var empresa = _relEmpresaRepository.ConsultaEntidades(consultaEmpresa);

            string mimeType = string.Empty;
            string encoding = string.Empty;//`enter code here`
            string extension = string.Empty;


            DataSet dsData = new DataSet();
            DataTable dtEpiColaborador;

            var conversorDt = new Conversor();

            switch (filtroEpiColaborador.EstadoEpis)
            {
                case EstadoEpisConsulta.Entregues:
                    dtEpiColaborador = conversorDt.LINQToDataTable<RelEpiColModel>(episColaboradores.Where(epi => !epi.baixado));
                    break;

                case EstadoEpisConsulta.Baixados:
                    dtEpiColaborador = conversorDt.LINQToDataTable<RelEpiColModel>(episColaboradores.Where(epi => epi.baixado));
                    break;
                default:
                    dtEpiColaborador = conversorDt.LINQToDataTable<RelEpiColModel>(episColaboradores);
                    break;
            }

            dsData.Tables.Add(dtEpiColaborador);

            ReportDataSource rdsAct = new ReportDataSource("EpixCol", dsData.Tables[0]);
            ReportDataSource ds = new ReportDataSource("DataSet1", dtEpiColaborador);
            e.DataSources.Add(ds);

        }

        [Authorize(Roles = "role_relatorios, role_relatorios_uniforme")]
        public JsonResult GetColaboradorPorUnidadeNegocio(int idUnidadeNegocio)
        {
            IEnumerable<ColaboradorModel> colaboradores;
            if (idUnidadeNegocio == 0) {
                colaboradores = _colaboradorRepository.GetAll();
            }
            else
            {
                colaboradores = _colaboradorRepository.BuscarPorUnidadeNegocio(idUnidadeNegocio);
                
            }
            var col = new SelectList(colaboradores, "Id", "Nome");
            return Json(col, JsonRequestBehavior.AllowGet);

        }

        [Authorize(Roles = "role_relatorios, role_relatorios_uniforme")]
        public JsonResult GetSetorPorUnidadeNegocio(int idUnidadeNegocio)
        {
         
            IEnumerable<SetorModel> setores;
            if (idUnidadeNegocio == 0)
            {
                setores = _setorRepository.GetAll();
            }
            else
            {
                setores = _setorRepository.BuscarPorUnidadeNegocio(idUnidadeNegocio);

            }
            var col = new SelectList(setores, "Id", "Nome");
            return Json(col, JsonRequestBehavior.AllowGet);
        }

        //Relarório Uniforme
        [Authorize(Roles = "role_relatorios_uniforme")]
        public ActionResult UniformeColaboradorViewer(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.ColaboradorId = new SelectList(_colaboradorRepository.GetAll(), "Id", "Nome");
            ViewBag.UniformeId = new SelectList(_uniformeRepository.BuscarAtivos(), "Id", "Nome");
            ViewBag.TipoUniformeId = new SelectList(_tipoUniformeRepository.BuscarAtivos(), "Id", "Nome");
            ViewBag.SetorId = new SelectList(_setorRepository.GetAll(), "Id", "Nome");
            ViewBag.CentroCustoId = new SelectList(_centroCustoRepository.BuscarAtivos(), "Id", "Nome");
            ViewBag.UnidadeNegocioId = new SelectList(_lbcRepository.BuscarAtivos(), "Id", "Fantasia");

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "role_relatorios_uniforme")]
        public ActionResult UniformeColaboradorViewer(ColaboradorUniformeFilter uniformeColFilter)
        {
            return RedirectToAction("VisualizarRelUniformeColaborador", uniformeColFilter);
        }

        [Authorize(Roles = "role_relatorios_uniforme")]
        public ActionResult VisualizarRelUniformeColaborador(ColaboradorUniformeFilter filtro)
        {
            var consulta = ConsultaUniformeCol.getConsulta(filtro);
            var uniformesColaboradores = _relUniformeColaboradorRepository.ConsultaEntidades(consulta);

            ViewBag.FiltroUniforme = filtro;

            if (filtro.EstadoUniformes == EstadoUniformesConsulta.Entregues)
            {
                return View(uniformesColaboradores.Where(e => !e.baixado));
            }
            if (filtro.EstadoUniformes == EstadoUniformesConsulta.Baixados)
            {
                return View(uniformesColaboradores.Where(e => e.baixado));
            }

            return View(uniformesColaboradores);
        }

        [Authorize(Roles = "role_relatorios_uniforme")]
        public ActionResult ConsumoUniformeViewer(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.ColaboradorId = new SelectList(_colaboradorRepository.GetAll(), "Id", "Nome");
            ViewBag.UniformeId = new SelectList(_uniformeRepository.BuscarAtivos(), "Id", "Nome");
            ViewBag.TipoUniformeId = new SelectList(_tipoUniformeRepository.BuscarAtivos(), "Id", "Nome");
            ViewBag.SetorId = new SelectList(_setorRepository.GetAll(), "Id", "Nome");
            ViewBag.CentroCustoId = new SelectList(_centroCustoRepository.BuscarAtivos(), "Id", "Nome");
            ViewBag.UnidadeNegocioId = new SelectList(_lbcRepository.BuscarAtivos(), "Id", "Fantasia");

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "role_relatorios_uniforme")]
        public ActionResult ConsumoUniformeViewer(ConsumoUniformeFilter conUniformeFilter)
        {

            ViewBag.ColaboradorId = new SelectList(_colaboradorRepository.BuscarAtivos(), "Id", "Nome");
            ViewBag.UniformeId = new SelectList(_uniformeRepository.BuscarAtivos(), "Id", "Nome");
            ViewBag.TipoUniformeId = new SelectList(_tipoUniformeRepository.BuscarAtivos(), "Id", "Nome");
            ViewBag.SetorId = new SelectList(_setorRepository.BuscarAtivos(), "Id", "Nome");
            ViewBag.CentroCustoId = new SelectList(_centroCustoRepository.BuscarAtivos(), "Id", "Nome");
            ViewBag.UnidadeNegocioId = new SelectList(_lbcRepository.BuscarAtivos(), "Id", "Fantasia");

            return RedirectToAction("VisualizarRelConsumoUniforme", conUniformeFilter);
        }

        [Authorize(Roles = "role_relatorios_uniforme")]
        public ActionResult VisualizarRelConsumoUniforme(ConsumoUniformeFilter filtro)
        {
            var consulta = ConsultaConsumoUniforme.GetConsulta(filtro);
            var consumoUniformes = _relConsumoUniformeRepository.ConsultaEntidades(consulta);

            ViewBag.FiltroUniforme = filtro;

            return View(consumoUniformes);
        }

        [Authorize(Roles = "role_relatorios_uniforme")]
        public ActionResult DownloadReportConsumoUniforme(string filtro)
        {
            filterGlobalUniforme = filtro;
            var filtroConsumoUniforme = new ConsumoUniformeFilter();

            if (filtro != null)
            {
                var format = "dd/MM/yyyy"; // your datetime format
                var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = format };
                filtroConsumoUniforme = JsonConvert.DeserializeObject<ConsumoUniformeFilter>(filtro, dateTimeConverter);
            }

            var consultaConsumoUniforme = ConsultaConsumoUniforme.GetConsulta(filtroConsumoUniforme);
            var consumoUniformes = _relConsumoUniformeRepository.ConsultaEntidades(consultaConsumoUniforme);
            var consultaEmpresa = ConsultaEmpresa.GetConsulta();
            var empresa = _relEmpresaRepository.ConsultaEntidades(consultaEmpresa);

            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;

            DataSet dsData = new DataSet();
            DataTable dtConsumoUniforme;

            var conversorDt = new Conversor();

            dtConsumoUniforme = conversorDt.LINQToDataTable<RelConsumoUniformeModel>(consumoUniformes);

            DataTable dtEmpresa = conversorDt.LINQToDataTable<RelEmpresaModel>(empresa);

            dsData.Tables.Add(dtConsumoUniforme);
            dsData.Tables.Add(dtEmpresa);
            
            ReportDataSource rdsAct = new ReportDataSource("ConsumoUniforme", dsData.Tables[0]);
            ReportDataSource rdsEmpr = new ReportDataSource("dsEmpresa", dsData.Tables[1]);

            ReportViewer viewer = new ReportViewer();
            viewer.LocalReport.Refresh();
            viewer.LocalReport.ReportPath = "Reports/ConsumoUniforme.rdlc"; //This is your rdlc 
            viewer.LocalReport.DataSources.Add(rdsAct); // Add  datasource here  
            viewer.LocalReport.DataSources.Add(rdsEmpr); // Add  datasource here

            byte[] bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension,
                out streamIds, out warnings);

            return File(bytes, mimeType, "ConsumoUniforme.pdf");
        }


        [Authorize(Roles = "role_relatorios_uniforme")]
        public ActionResult UniformeFaltaViewer(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.UniformeId = new SelectList(_uniformeRepository.BuscarAtivos(), "Id", "Nome");

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "role_relatorios_uniforme")]
        public ActionResult UniformeFaltaViewer(RelUniformeFaltaFilter uniformeFaltaFilter)
        {
            return RedirectToAction("VisualizarRelUniformeFalta", uniformeFaltaFilter);
        }

        [Authorize(Roles = "role_relatorios_uniforme")]
        public ActionResult VisualizarRelUniformeFalta(RelUniformeFaltaFilter filtro)
        {
            var consulta = ConsultaFaltaUniforme.GetConsulta(filtro);
            var uniformesFalta = _relUniformeFaltaRepository.ConsultaEntidades(consulta);

            ViewBag.FiltroUniforme = filtro;

            return View(uniformesFalta);
        }


        [Authorize(Roles = "role_relatorios_uniforme")]
        public ActionResult DownloadReportUniformeFalta(string filtro)
        {
            var filtroUniformeFalta = new RelUniformeFaltaFilter();

            if (filtro != null)
            {
                var format = "dd/MM/yyyy"; // your datetime format
                var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = format };
                filtroUniformeFalta = JsonConvert.DeserializeObject<RelUniformeFaltaFilter>(filtro, dateTimeConverter);
            }

            var consultaUniformeFalta = ConsultaFaltaUniforme.GetConsulta(filtroUniformeFalta);
            var uniformesFalta = _relUniformeFaltaRepository.ConsultaEntidades(consultaUniformeFalta);
            var consultaEmpresa = ConsultaEmpresa.GetConsulta();
            var empresa = _relEmpresaRepository.ConsultaEntidades(consultaEmpresa);

            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;

            DataSet dsData = new DataSet();
            DataTable dtUniformeFalta;

            var conversorDt = new Conversor();

            dtUniformeFalta = conversorDt.LINQToDataTable<RelUniformeFaltaModel>(uniformesFalta);

            DataTable dtEmpresa = conversorDt.LINQToDataTable<RelEmpresaModel>(empresa);

            dsData.Tables.Add(dtUniformeFalta);
            dsData.Tables.Add(dtEmpresa);

            ReportDataSource rdsAct = new ReportDataSource("dsUniformeFalta", dsData.Tables[0]);
            ReportDataSource rdsEmpr = new ReportDataSource("dsEmpresa", dsData.Tables[1]);

            ReportViewer viewer = new ReportViewer();
            viewer.LocalReport.Refresh();
            viewer.ProcessingMode = ProcessingMode.Local;
            viewer.LocalReport.ReportPath = Path.Combine(Server.MapPath("~/Reports"), "UniformeFalta.rdlc");
            viewer.LocalReport.DataSources.Add(rdsAct);
            viewer.LocalReport.DataSources.Add(rdsEmpr);

            byte[] bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension,
                out streamIds, out warnings);

            return File(bytes, mimeType, "UniformeFalta.pdf");
        }


        [HttpGet]
        [Authorize(Roles = "role_relatorios_uniforme")]
        public CrystalReportPdfResult PdfUniforme()
        {
            List<RelConsumoUniformeModel> model = new List<RelConsumoUniformeModel>();
            model.Add(new RelConsumoUniformeModel
            {
                nome_colaborador = "ColTeste" ?? "Teste",
                nome_ccusto = "CCTeste" ?? "Teste",
                nome_uniforme = "UniformeTeste" ?? "Teste",
                baixado = false,
                uniforme_custo = 10,
                id_centro_custo = 2,
                id_colaborador = 1,
                id_colaborador_uniforme = 1,
                qtde_uniformes = 1
            });
            string reportPath = Path.Combine(Server.MapPath("~/Reports"), "CrystalReportTest.rpt");
            return new CrystalReportPdfResult(reportPath, model);
        }

        [Authorize(Roles = "role_relatorios_uniforme")]
        public void ExportToExcelConsumoUniforme(string filtro)
        {
            var filtroConsumoUniforme = new ConsumoUniformeFilter();

            if (filtro != null)
            {
                var format = "dd/MM/yyyy"; // your datetime format
                var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = format };
                filtroConsumoUniforme = JsonConvert.DeserializeObject<ConsumoUniformeFilter>(filtro, dateTimeConverter);
            }

            var consultaConsumoUniforme = ConsultaConsumoUniforme.GetConsulta(filtroConsumoUniforme);
            var consumoUniformes = _relConsumoUniformeRepository.ConsultaEntidades(consultaConsumoUniforme);

            var conversorDt = new Conversor();

            DataTable dtConsumoUniforme = conversorDt.LINQToDataTable<RelConsumoUniformeModel>(consumoUniformes);
            dtConsumoUniforme.Columns.RemoveAt(0); //id_unidade_negocio
            dtConsumoUniforme.Columns.RemoveAt(0); //id_colaborador
            dtConsumoUniforme.Columns.RemoveAt(0); //id_colaborador_uniforme
            dtConsumoUniforme.Columns.RemoveAt(0); //id_centro_custo
            dtConsumoUniforme.Columns.RemoveAt(10); //razao_social
            
            using (ExcelPackage pck = new ExcelPackage())
            {
                //Create the worksheet
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("ConsumoUniforme");

                //Load the datatable into the sheet, starting from cell A1. Print the column names on row 1
                ws.Cells["A1"].LoadFromDataTable(dtConsumoUniforme, true);

                //Format the header for column 1-3
                using (ExcelRange rng = ws.Cells["A1:L1"])
                {
                    rng.Style.Font.Bold = true;
                    rng.Style.Fill.PatternType = ExcelFillStyle.Solid;                      //Set Pattern for the background to Solid
                    rng.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(79, 129, 189));  //Set color to dark blue
                    rng.Style.Font.Color.SetColor(Color.White);
                }

                ws.Cells["A1"].Value = "Colaborador";
                ws.Cells["B1"].Value = "Uniforme";
                ws.Cells["C1"].Value = "Quantidade";
                ws.Cells["D1"].Value = "Custo";
                ws.Cells["E1"].Value = "Baixado";
                ws.Cells["F1"].Value = "Centro de Custo";
                ws.Cells["G1"].Value = "Custo Total";
                ws.Cells["H1"].Value = "Setor";
                ws.Cells["I1"].Value = "Função";
                ws.Cells["J1"].Value = "Nome Fantasia";
                ws.Cells["K1"].Value = "Justificativa";
                ws.Cells["L1"].Value = "Justificativa Baixa";                

                ws.Cells[ws.Dimension.Address].AutoFitColumns();
                
                //Write it back to the client
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=ConsumoUniforme.xlsx");
                Response.BinaryWrite(pck.GetAsByteArray());
            }
        }

        [Authorize(Roles = "role_relatorios_uniforme")]
        public ActionResult DownloadReportUniforme(string filtro)
        {
            filterGlobalUniforme = filtro;
            var filtroUniformeColaborador = new ColaboradorUniformeFilter();

            if (filtro != null)
            {
                var format = "dd/MM/yyyy"; // your datetime format
                var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = format };
                filtroUniformeColaborador = JsonConvert.DeserializeObject<ColaboradorUniformeFilter>(filtro, dateTimeConverter);                
            }

            var consultaUniformeCol = ConsultaUniformeCol.getConsulta(filtroUniformeColaborador);
            var uniformesColaboradores = _relUniformeColaboradorRepository.ConsultaEntidades(consultaUniformeCol);
            uniformesColaboradores = uniformesColaboradores.GroupBy(ec => ec.id_colaborador).Select(ec => ec.First());
            var consultaEmpresa = ConsultaEmpresa.GetConsulta();
            var empresa = _relEmpresaRepository.ConsultaEntidades(consultaEmpresa);

            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;//`enter code here`
            string extension = string.Empty;

            DataSet dsData = new DataSet();
            DataTable dtUniformeColaborador;

            var conversorDt = new Conversor();

            switch (filtroUniformeColaborador.EstadoUniformes)
            {
                case EstadoUniformesConsulta.Entregues:
                    dtUniformeColaborador = conversorDt.LINQToDataTable<RelUniformeColModel>(uniformesColaboradores.Where(e => !e.baixado));
                    break;

                case EstadoUniformesConsulta.Baixados:
                    dtUniformeColaborador = conversorDt.LINQToDataTable<RelUniformeColModel>(uniformesColaboradores.Where(e => e.baixado));
                    break;
                default:
                    dtUniformeColaborador = conversorDt.LINQToDataTable<RelUniformeColModel>(uniformesColaboradores);
                    break;
            }

            DataTable dtEmpresa = conversorDt.LINQToDataTable<RelEmpresaModel>(empresa);

            dsData.Tables.Add(dtUniformeColaborador);
            dsData.Tables.Add(dtEmpresa);

            ReportDataSource rdsAct = new ReportDataSource("UniformexCol", dsData.Tables[0]);
            ReportDataSource rdsEmpr = new ReportDataSource("dsEmpresa", dsData.Tables[1]);

            ReportViewer viewer = new ReportViewer();
            viewer.LocalReport.Refresh();
            viewer.LocalReport.ReportPath = "Reports/UniformeColaboradorSint.rdlc";
            viewer.LocalReport.DataSources.Add(rdsAct);
            viewer.LocalReport.DataSources.Add(rdsEmpr);
            viewer.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(RenderizaSubRelatorioUniforme);

            byte[] bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);

            return File(bytes, mimeType, "UniformeColaborador.pdf");
        }

        [Authorize(Roles = "role_entrega_uniforme, role_entrega_uniforme_outros")]
        public ActionResult GerarComprovanteEntregaUniforme(DateTime dataInicial, DateTime dataFinal, int idColaborador)
        {

            var filtroUniformeColaborador = new ColaboradorUniformeFilter();
            filtroUniformeColaborador.ColaboradorId = idColaborador;
            filtroUniformeColaborador.DataFinal = dataFinal;
            filtroUniformeColaborador.DataInicial = dataInicial;
            filtroUniformeColaborador.Ativos = true;
            filtroUniformeColaborador.EstadoUniformes = EstadoUniformesConsulta.Entregues;
            filtroUniformeGlobal = filtroUniformeColaborador;
            var consultaUniformeCol = ConsultaUniformeCol.getConsulta(filtroUniformeColaborador);
            var uniformesColaboradores = _relUniformeColaboradorRepository.ConsultaEntidades(consultaUniformeCol);
            uniformesColaboradores = uniformesColaboradores.GroupBy(ec => ec.id_colaborador).Select(ec => ec.First());
            var consultaEmpresa = ConsultaEmpresa.GetConsulta();
            var empresa = _relEmpresaRepository.ConsultaEntidades(consultaEmpresa);

            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;//`enter code here`
            string extension = string.Empty;

            DataSet dsData = new DataSet();
            DataTable dtUniformeColaborador;

            var conversorDt = new Conversor();

            switch (filtroUniformeColaborador.EstadoUniformes)
            {
                case EstadoUniformesConsulta.Entregues:
                    dtUniformeColaborador = conversorDt.LINQToDataTable<RelUniformeColModel>(uniformesColaboradores.Where(e => !e.baixado));
                    break;

                case EstadoUniformesConsulta.Baixados:
                    dtUniformeColaborador = conversorDt.LINQToDataTable<RelUniformeColModel>(uniformesColaboradores.Where(e => e.baixado));
                    break;
                default:
                    dtUniformeColaborador = conversorDt.LINQToDataTable<RelUniformeColModel>(uniformesColaboradores);
                    break;
            }

            DataTable dtEmpresa = conversorDt.LINQToDataTable<RelEmpresaModel>(empresa);

            dsData.Tables.Add(dtUniformeColaborador);
            dsData.Tables.Add(dtEmpresa);

            ReportDataSource rdsAct = new ReportDataSource("UniformexCol", dsData.Tables[0]);
            ReportDataSource rdsEmpr = new ReportDataSource("dsEmpresa", dsData.Tables[1]);

            ReportViewer viewer = new ReportViewer();
            viewer.LocalReport.Refresh();
            viewer.LocalReport.ReportPath = "Reports/UniformeColaboradorSint.rdlc";
            viewer.LocalReport.DataSources.Add(rdsAct);
            viewer.LocalReport.DataSources.Add(rdsEmpr);
            viewer.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(RenderizaSubRelatorioUniforme);
            
            byte[] bytes = null;
            try
            {
                bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
            }
            catch (Exception ex)
            {
                while (ex != null)
                {
                    Warning(ex.Message);
                    ex = ex.InnerException;
                }
                return RedirectToAction("Index", "Home");

            }
            return File(bytes, mimeType, "Comprovante-" + filtroUniformeColaborador.ColaboradorId.ToString() + "-" + filtroUniformeColaborador.DataFinal.ToString() + ".pdf");
        }

        [Authorize(Roles = "role_entrega_uniforme, role_entrega_uniforme_outros")]
        private void RenderizaSubRelatorioUniforme(object sender, SubreportProcessingEventArgs e)
        {
            var filtro = filterGlobalUniforme;
            int idColaborador = int.Parse(e.Parameters["id_colaborador"].Values[0].ToString());
            var filtroUniformeColaborador = new ColaboradorUniformeFilter();

            if (filtro != null)
            {
                var format = "dd/MM/yyyy"; // your datetime format
                var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = format };
                filtroUniformeColaborador = JsonConvert.DeserializeObject<ColaboradorUniformeFilter>(filtro, dateTimeConverter);
            }
            else
            {
                filtroUniformeColaborador = filtroUniformeGlobal;
            }

            var consultaUniformeCol = ConsultaUniformeCol.getConsulta(filtroUniformeColaborador);
            var uniformesColaboradores = _relUniformeColaboradorRepository.ConsultaEntidades(consultaUniformeCol);
            uniformesColaboradores = uniformesColaboradores.Distinct();
            var consultaEmpresa = ConsultaEmpresa.GetConsulta();
            var empresa = _relEmpresaRepository.ConsultaEntidades(consultaEmpresa);

            string mimeType = string.Empty;
            string encoding = string.Empty;//`enter code here`
            string extension = string.Empty;


            DataSet dsData = new DataSet();
            DataTable dtUniformeColaborador;

            var conversorDt = new Conversor();

            switch (filtroUniformeColaborador.EstadoUniformes)
            {
                case EstadoUniformesConsulta.Entregues:
                    dtUniformeColaborador = conversorDt.LINQToDataTable<RelUniformeColModel>(uniformesColaboradores.Where(uniforme => !uniforme.baixado));
                    break;

                case EstadoUniformesConsulta.Baixados:
                    dtUniformeColaborador = conversorDt.LINQToDataTable<RelUniformeColModel>(uniformesColaboradores.Where(uniforme => uniforme.baixado));
                    break;
                default:
                    dtUniformeColaborador = conversorDt.LINQToDataTable<RelUniformeColModel>(uniformesColaboradores);
                    break;
            }

            dsData.Tables.Add(dtUniformeColaborador);

            ReportDataSource rdsAct = new ReportDataSource("UniformexCol", dsData.Tables[0]);
            ReportDataSource ds = new ReportDataSource("DataSet1", dtUniformeColaborador);
            e.DataSources.Add(ds);

        }
    }
}