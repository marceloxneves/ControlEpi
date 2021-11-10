using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using PagedList;
using TitansMVC.Models;
using TitansMVC.Repository.Implementations;
using TitansMVC.Repository.Interfaces;
using TitansMVC.Utils;
using WebGrease.Css.Extensions;
using TitansMVC.Models.Relatorios;

namespace TitansMVC.Controllers
{
    [Authorize]
    public class ColaboradorController : BaseController
    {
        private readonly IColaboradorRepository _colaboradorRepository;
        private readonly ISetorRepository _setorRepository;
        private readonly ICentroCustoRepository _centroCustoRepository;
        private readonly IEpiSetorRepository _epiSetorRepository;
        private readonly IEpiColaboradorRepository _epiColaboradorRepository;
        private readonly ITipoEpiRepository _tipoEpiRepository;
        private readonly IConfiguracaoRepository _configuracaoRepository;
        private readonly IEstoqueEpiRepository _estoqueEpiRepository;
        private readonly ILbcRepository _lbcRepository;
        private readonly IEpiRepository _epiRepository;
        private readonly IFuncaoRepository _funcaoRepository;
        private readonly IUniformeSetorRepository _uniformeSetorRepository;
        private readonly IUniformeColaboradorRepository _uniformeColaboradorRepository;
        private readonly ITipoUniformeRepository _tipoUniformeRepository;
        private readonly IEstoqueUniformeRepository _estoqueUniformeRepository;
        private readonly IUniformeRepository _uniformeRepository;

        public ColaboradorController()
        {
            _colaboradorRepository = new ColaboradorRepository();
            _setorRepository = new SetorRepository();
            _centroCustoRepository = new CentroCustoRepository();
            _epiSetorRepository = new EpiSetorRepository();
            _epiColaboradorRepository = new EpiColaboradorRepository();
            _tipoEpiRepository = new TipoEpiRepository();
            _configuracaoRepository = new ConfiguracaoRepository();
            _estoqueEpiRepository = new EstoqueEpiRepository();
            _lbcRepository = new LbcRepository();
            _epiRepository = new EpiRepository();
            _funcaoRepository = new FuncaoRepository();
            _uniformeSetorRepository = new UniformeSetorRepository();
            _uniformeColaboradorRepository = new UniformeColaboradorRepository();
            _tipoUniformeRepository = new TipoUniformeRepository();
            _estoqueUniformeRepository = new EstoqueUniformeRepository();
            _uniformeRepository = new UniformeRepository();

        }

        // GET: Colaborador
        [Authorize(Roles = "role_base, role_entregaepi, role_entrega_epi_outros, role_base_uniforme, role_entrega_uniforme, role_entrega_uniforme_outros")]
        public ActionResult Index(string searchBy, bool? ativos, string search, int? page, int? UnidadeNegocioId)
        {

            
            ViewBag.UnidadeNegocioId = new SelectList(_lbcRepository.BuscarAtivos(), "Id", "Fantasia");

            var empresa = Util.GetEmpresa();
            
            ViewBag.PossuiBiometria = empresa.PossuiBiometria;

            IEnumerable<ColaboradorModel> colaboradores;

            if (searchBy == "Nome")
            {
                colaboradores = _colaboradorRepository.BuscarPorNome(nome: search, UnidadeNegocioId: UnidadeNegocioId, ativo: ativos.HasValue ? ativos.Value : true);
            }
            else if (searchBy == "Cpf")
            {
                colaboradores = _colaboradorRepository.BuscarPorCpf(cpf: search, UnidadeNegocioId: UnidadeNegocioId, ativo: ativos.HasValue ? ativos.Value : true);
            }
            else /*if (searchBy == "Todos")*/
            {
                colaboradores = UnidadeNegocioId == null ?
                    _colaboradorRepository.GetAll(ativos: ativos.HasValue ? ativos.Value : true) :
                    _colaboradorRepository.BuscarPorUnidadeNegocio(UnidadeNegocioId.Value, ativo: ativos.HasValue ? ativos.Value : true);
            }

            return View(colaboradores.ToPagedList(page ?? 1, 10));
        }

        // GET: Colaborador/Create
        [Authorize(Roles = "role_base, role_base_uniforme")]
        public ActionResult Create()
        {
            ViewBag.SetorId = new SelectList(_setorRepository.BuscarAtivos(), "Id", "Nome");
            ViewBag.CentroCustoId = new SelectList(_centroCustoRepository.BuscarAtivos(), "Id", "Nome");
            ViewBag.LbcId = new SelectList(_lbcRepository.BuscarAtivos(), "Id", "Fantasia");
            ViewBag.FuncaoId = new SelectList(_funcaoRepository.BuscarAtivos(), "Id", "Nome");

            var colaborador = new ColaboradorModel() { RecebeuTreinamento = false, RecebeuAdvertencia = false };
            return View(colaborador);
        }

        // POST: Colaborador/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "role_base, role_base_uniforme")]
        public ActionResult Create(ColaboradorModel colaborador, HttpPostedFileBase uploadfoto)
        {
            if (!ExisteColaborador(colaborador.Cpf))
            {
                //var foto = Request.Files["foto"];
                try
                {
                    if (ModelState.IsValid)
                    {
                        colaborador.Ativo = true;

                        if (uploadfoto != null && uploadfoto.ContentLength > 0)
                        {
                            try
                            {
                                using (var reader = new BinaryReader(uploadfoto.InputStream))
                                {
                                    colaborador.Foto = reader.ReadBytes(uploadfoto.ContentLength);
                                }
                            }
                            catch (RetryLimitExceededException /* dex */)
                            {
                                ModelState.AddModelError("", "Não foi possível salvar as alterações. Tente novamente, e se o problema persistir entre em contato com o administrador do sistema.");
                            }
                        }

                        if (Util.LimiteAlcancado(_colaboradorRepository.ContarColaboradores(Util.GetEmpresaId())))
                        {
                            _colaboradorRepository.Add(colaborador);
                            Success(String.Format("Registro gravado com sucesso!"), true);
                            return RedirectToAction("Edit", colaborador);
                        }
                        else
                        {
                            Warning(String.Format("Limite de colaboradores atingido!"), true);
                            ViewBag.SetorId = new SelectList(_setorRepository.BuscarAtivos(), "Id", "Nome");
                            ViewBag.CentroCustoId = new SelectList(_centroCustoRepository.BuscarAtivos(), "Id", "Nome");
                            return View(colaborador);
                        }
                    }
                    else
                    {
                        Warning(String.Format("Existem alguns campos com preenchimento esperado!"), true);
                        ViewBag.SetorId = new SelectList(_setorRepository.BuscarAtivos(), "Id", "Nome");
                        ViewBag.CentroCustoId = new SelectList(_centroCustoRepository.BuscarAtivos(), "Id", "Nome");
                        ViewBag.LbcId = new SelectList(_lbcRepository.BuscarAtivos(), "Id", "Fantasia");
                        ViewBag.FuncaoId = new SelectList(_funcaoRepository.BuscarAtivos(), "Id", "Nome");
                        return View(colaborador);
                    }
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("",
                        "Não foi possível salvar as alterações. Tente novamente, e se o problema persistir entre em contato com o administrador do sistema.");
                }
            }

            Warning(String.Format("O sistema já possui um colaborador com este CPF cadastrado."), true);

            ViewBag.SetorId = new SelectList(_setorRepository.BuscarAtivos(), "Id", "Nome");
            ViewBag.CentroCustoId = new SelectList(_centroCustoRepository.BuscarAtivos(), "Id", "Nome");
            ViewBag.FuncaoId = new SelectList(_funcaoRepository.BuscarAtivos(), "Id", "Nome");
            ViewBag.LbcId = new SelectList(_lbcRepository.BuscarAtivos(), "Id", "Fantasia");
            return View(colaborador);
        }

        // GET: Colaborador/Edit/5
        [Authorize(Roles = "role_base, role_base_uniforme")]
        public ActionResult Edit(int id)
        {
            var colaborador = _colaboradorRepository.GetByIdEager(id);

            ViewBag.SetorId = new SelectList(_setorRepository.BuscarAtivos(), "Id", "Nome", colaborador.SetorId);
            ViewBag.TipoEpiId = new SelectList(_tipoEpiRepository.BuscarAtivos(), "Id", "Nome");
            ViewBag.EpiSetorId = new SelectList("", "");
            ViewBag.EpiOutrSetoresId = new SelectList("", "");
            ViewBag.CentroCustoId = new SelectList(_centroCustoRepository.BuscarAtivos(), "Id", "Nome");
            ViewBag.LbcId = new SelectList(_lbcRepository.BuscarAtivos(), "Id", "Fantasia", colaborador.LbcId);
            ViewBag.FuncaoId = new SelectList(_funcaoRepository.BuscarAtivos(), "Id", "Nome", colaborador.FuncaoId);
            ViewBag.TipoUniformeId = new SelectList(_tipoUniformeRepository.BuscarAtivos(), "Id", "Nome");
            ViewBag.UniformeSetorId = new SelectList("", "");
            ViewBag.UniformeOutrSetoresId = new SelectList("", "");

            foreach (var epi in colaborador.Epis)
            {
                if (epi.AssinaturaPendente.Value)
                {
                    epi.AssinaturaPendente = false;
                }
            }

            foreach (var uniforme in colaborador.Uniformes)
            {
                if (uniforme.AssinaturaPendente.Value)
                {
                    uniforme.AssinaturaPendente = false;
                }
            }
            var empresa = Util.GetEmpresa();

            ViewBag.PossuiAssinatura = empresa.PossuiAssinatura;
            ViewBag.PossuiBiometria = empresa.PossuiBiometria;

            return View(colaborador);
        }

        // POST: Colaborador/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "role_base, role_base_uniforme")]
        public ActionResult Edit(ColaboradorModel colaboradorAlterado, HttpPostedFileBase uploadfoto, string imageData, int assinOption = 0)
        {
            var colaboradorRepo = _colaboradorRepository.GetByIdEager(colaboradorAlterado.Id);

            colaboradorAlterado.Foto = colaboradorRepo.Foto;
            colaboradorAlterado.Assinatura = colaboradorRepo.Assinatura;
            if (!String.IsNullOrEmpty(imageData))
            {
                var converter = new Conversor();

                //Local onde vamos salvar a imagem (raiz do site + /canvas.png)
                //string path = Server.MapPath("~/canvas.png");
                switch (assinOption)
                {
                    case 1:
                        {
                            colaboradorAlterado.Assinatura = converter.StringToByte("", imageData);
                            break;
                        }
                    case 2:
                        {
                            foreach (var epi in colaboradorRepo.Epis)
                            {
                                if (epi.AssinaturaPendente.Value)
                                {
                                    epi.AssinaturaColaborador = converter.StringToByte("", imageData);
                                    epi.AssinaturaPendente = false;
                                }
                            }

                            foreach (var uniforme in colaboradorRepo.Uniformes)
                            {
                                if (uniforme.AssinaturaPendente.Value)
                                {
                                    uniforme.AssinaturaColaborador = converter.StringToByte("", imageData);
                                    uniforme.AssinaturaPendente = false;
                                }
                            }
                            break;
                        }
                }
            }

            //foreach (var epi in colaboradorRepo.Epis)
            //{
            //    colaboradorAlterado.Epis.Add(epi);
            //}
            colaboradorAlterado.Epis = colaboradorRepo.Epis;
            colaboradorAlterado.Uniformes = colaboradorRepo.Uniformes;
            var empresa = Util.GetEmpresa();
            ViewBag.PossuiBiometria = empresa.PossuiBiometria;

            _colaboradorRepository.RetirarDoContexto(colaboradorRepo);

            ViewBag.SetorId = new SelectList(_setorRepository.BuscarAtivos(), "Id", "Nome", colaboradorAlterado.SetorId);
            ViewBag.TipoEpiId = new SelectList(_tipoEpiRepository.BuscarAtivos(), "Id", "Nome");
            ViewBag.EpiSetorId = new SelectList("", "");
            ViewBag.EpiOutrSetoresId = new SelectList("", "");
            ViewBag.CentroCustoId = new SelectList(_centroCustoRepository.BuscarAtivos(), "Id", "Nome");
            ViewBag.PossuiAssinatura = empresa.PossuiAssinatura;
            ViewBag.PossuiBiometria = empresa.PossuiBiometria;
            ViewBag.LbcId = new SelectList(_lbcRepository.BuscarAtivos(), "Id", "Fantasia", colaboradorAlterado.LbcId);
            ViewBag.FuncaoId = new SelectList(_funcaoRepository.BuscarAtivos(), "Id", "Nome", colaboradorAlterado.FuncaoId);
            ViewBag.TipoUniformeId = new SelectList(_tipoUniformeRepository.BuscarAtivos(), "Id", "Nome");
            ViewBag.UniformeSetorId = new SelectList("", "");
            ViewBag.UniformeOutrSetoresId = new SelectList("", "");

            if (ModelState.IsValid)
            {
                // exemplo foreach lambda expressions
                //colaboradorAlterado.Epis.ToList().ForEach(e => e.DataVencimento = e.DataEntrega.AddDays(e.EpiSetor.ValidadeEmDias.Value));

                if (uploadfoto != null && uploadfoto.ContentLength > 0)
                {
                    try
                    {
                        using (var reader = new BinaryReader(uploadfoto.InputStream))
                        {
                            colaboradorAlterado.Foto = reader.ReadBytes(uploadfoto.ContentLength);
                        }
                    }
                    catch (RetryLimitExceededException /* dex */)
                    {
                        ModelState.AddModelError("", "Não foi possível salvar as alterações. Tente novamente, se o problema persistir entre em contato com o administrador do sistema.");
                    }
                }

                _colaboradorRepository.Update(colaboradorAlterado);

                Success(String.Format("Registro alterado com sucesso!"), true);

                return View(colaboradorAlterado);
            }

            ViewBag.bloqTipoUnico = false;

            return View(colaboradorAlterado);
        }

        // GET: Colaborador/Delete/5
        [Authorize(Roles = "role_base, role_base_uniforme")]
        public ActionResult Delete(int id)
        {
            var colaborador = _colaboradorRepository.GetById(id);

            return View(colaborador);
        }

        // POST: Colaborador/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "role_base, role_base_uniforme")]
        public ActionResult DeleteConfirmed(int id)
        {
            //var colaborador = _colaboradorRepository.GetById(id);
            //_colaboradorRepository.Remove(colaborador);

            _colaboradorRepository.RemoveLogical(id);

            return RedirectToAction("Index");
        }

        // Desativado porque nao funciona no servidor da nuvem (param base64)
        //public ActionResult SalvarAssinatura(int idColaborador, string base64)
        //{
        //    var colaborador = _colaboradorRepository.GetByIdEager(idColaborador);

        //    var converter = new Conversor();

        //    //Local onde vamos salvar a imagem (raiz do site + /canvas.png)
        //    string path = Server.MapPath("~/canvas.png");

        //    colaborador.Assinatura = converter.StringToByte(path, base64);

        //    _colaboradorRepository.Update(colaborador);

        //    //Success(String.Format("Assinatura incluída com sucesso!"), true);
        //    return PartialView("_Assinatura", colaborador);
        //}        

        // Desativado porque nao funciona no servidor da nuvem (param base64)
        //public ActionResult EntregarEpi(string idModel, string epi, string dataEntrega, string base64)
        //{
        //    DateTime data;
        //    DateTime.TryParse(dataEntrega, out data);

        //    var epiObj = JsonConvert.DeserializeObject<EpiColaboradorModel>(epi);
        //    epiObj.DataEntrega = data;

        //    var converter = new Conversor();

        //    string path = Server.MapPath("~/canvas.png");

        //    epiObj.AssinaturaColaborador = converter.StringToByte(path, base64);

        //    var epiSetor = _epiSetorRepository.GetById(epiObj.EpiSetorId);
        //    var setorEpi = _setorRepository.GetById(epiSetor.SetorId);

        //    epiObj.ValidadeEmDias = epiSetor.ValidadeEmDias;
        //    epiObj.NomeSetor = setorEpi.Nome;
        //    epiObj.NomeEpi = epiSetor.NomeEpi;
        //    epiObj.TipoEpiId = epiSetor.TipoEpiId;

        //    var colaborador = _colaboradorRepository.GetByIdEager(Convert.ToInt32(idModel));

        //    var bloquearPorTipo = _configuracaoRepository.GetUnique();
        //    if (epiObj.NomeEpi != "Selecione")
        //    {
        //         if (bloquearPorTipo.BloquearPorTipoEpiUnico)
        //        {
        //            if (!PossuiEpiMesmoTipo(colaborador, epiObj.TipoEpiId))
        //            {

        //                if (epiObj.ValidadeEmDias != null && epiObj.ValidadeEmDias > 0)
        //                {
        //                    epiObj.DataVencimento = epiObj.DataEntrega.AddDays(epiObj.ValidadeEmDias.Value);
        //                }

        //                colaborador.Epis.Add(epiObj);
        //                _colaboradorRepository.Update(colaborador);

        //                return PartialView("_EpisEntregues", colaborador.Epis.Where(e => e.Baixado != null && !e.Baixado.Value).Where(e => e.DataVencimento > DateTime.Now));
        //            }

        //            Warning(String.Format("Este colaborador já possui um EPI deste tipo. Antes de pegar outro o mesmo deverá ser baixado."), true);
        //            ViewBag.bloqTipoUnico = true;
        //        }
        //        else                    
        //        {
        //            if (epiObj.ValidadeEmDias != null && epiObj.ValidadeEmDias > 0)
        //            {
        //                epiObj.DataVencimento = epiObj.DataEntrega.AddDays(epiObj.ValidadeEmDias.Value);
        //            }

        //            colaborador.Epis.Add(epiObj);
        //            _colaboradorRepository.Update(colaborador);
        //        }                
        //    }

        //    return PartialView("_EpisEntregues", colaborador.Epis.Where(e => e.Baixado != null && !e.Baixado.Value).Where(e => e.DataVencimento > DateTime.Now));
        //}
        [Authorize(Roles = "role_entregaepi, role_entrega_epi_outros")]
        [HttpGet]
        public ActionResult EntregaDeEpi(int? idColaborador, bool hasBiometria)
        {
            if (idColaborador.HasValue)
            {
                var colaborador = _colaboradorRepository.GetByIdEager(idColaborador.Value);
                if (!colaborador.Ativo)
                {
                    Warning("Colaboradores inativos não podem receber EPI's", true);
                    return RedirectToAction("Index");
                }
                //var lbc = _lbcRepository.BuscarPorNome("Matriz").FirstOrDefault();
                var centroCusto = _centroCustoRepository.BuscarPorNome("GERAL", null).FirstOrDefault();

                ViewBag.TipoEpiId = new SelectList(_tipoEpiRepository.BuscarAtivos(), "Id", "Nome");
                ViewBag.EpiSetorId = new SelectList("", "");
                ViewBag.EpiOutrSetoresId = new SelectList("", "");
                ViewBag.CentroCustoItems = new SelectList(_centroCustoRepository.BuscarAtivos(), "Id", "Nome", centroCusto.Id);
                ViewBag.ValidarBiom = hasBiometria;
                return View(colaborador);
            }
            return RedirectToAction("Search", "Biometria");
        }

        [Authorize(Roles = "role_entregaepi, role_entrega_epi_outros")]
        [HttpPost]
        public ActionResult EntregaDeEpi(ColaboradorModel model)
        {
            if (!model.Ativo)
            {
                Warning("Colaboradores inativos não podem receber EPI's", true);
                return RedirectToAction("Index");
            }
            return RedirectToAction("GerarComprovanteEntregaEpi", "Report", new { dataInicial = DateTime.Now.Date, dataFinal = DateTime.Now.Date, idColaborador = model.Id });
        }

        [Authorize(Roles = "role_entregaepi, role_entrega_epi_outros")]
        public ActionResult EntregarEpi(string idModel, string epi, string dataEntrega, bool hasBiometria)
        {
            DateTime data;
            DateTime.TryParse(dataEntrega, out data);
            var colaborador = _colaboradorRepository.GetByIdEager(Convert.ToInt32(idModel));
            if (!colaborador.Ativo)
            {
                //enviar mensagem de erro colaborador nao pode receber epi.
                Warning(String.Format("Este colaborador foi inativado e não pode receber EPI's!"), true);
                ViewBag.bloqTipoUnico = true;
            }
            else
            {
                var epiObj = JsonConvert.DeserializeObject<EpiColaboradorModel>(epi);
                epiObj.DataEntrega = data;
                var epiSetor = _epiSetorRepository.GetById(epiObj.EpiSetorId);
                var setorEpi = _setorRepository.GetById(epiSetor.SetorId);
                
                epiObj.ValidadeEmDias = epiSetor.ValidadeEmDias;
                epiObj.NomeSetor = setorEpi.Nome;
                epiObj.NomeEpi = epiSetor.NomeEpi;
                epiObj.TipoEpiId = epiSetor.TipoEpiId;
                epiObj.AssinaturaPendente = true;

                epiObj.IdEmpresa = colaborador.IdEmpresa;
                epiObj.UnidadeNegocioId = colaborador.LbcId;

                epiObj.TipoValidacao = hasBiometria;

                var bloquearPorTipo = _configuracaoRepository.GetUnique();
                if (epiObj.NomeEpi != "Selecione")
                {
                    var estEpi = _estoqueEpiRepository.BuscarPorProduto(epiSetor.EpiId);
                    var estEpiVerificar = _estoqueEpiRepository.GetById(estEpi.First().Id);

                    if (estEpiVerificar.Qtde >= epiObj.Quantidade)
                    {

                        if (bloquearPorTipo.BloquearPorTipoEpiUnico)
                        {
                            if (!PossuiEpiMesmoTipo(colaborador, epiObj.TipoEpiId))
                            {
                                if (epiObj.ValidadeEmDias != null && epiObj.ValidadeEmDias > 0)
                                {
                                    epiObj.DataVencimento = epiObj.DataEntrega.AddDays(epiObj.ValidadeEmDias.Value);
                                }


                                colaborador.Epis.Add(epiObj);
                                _colaboradorRepository.Update(colaborador);

                                AtualizarEstoqueEntrega(epiSetor.EpiId, epiObj.Quantidade == null ? 0 : Convert.ToInt32(epiObj.Quantidade));

                                foreach (var item in colaborador.Epis)
                                {
                                    item.UnidadeNogocio = _lbcRepository.GetById(Convert.ToInt32(item.UnidadeNegocioId));
                                }

                                return PartialView("_EpisEntregues", colaborador.Epis.Where(e => e.Baixado != null && !e.Baixado.Value).Where(e => e.DataVencimento > DateTime.Now));
                            }

                            Warning(String.Format("Este colaborador já possui um EPI deste tipo. Antes de pegar outro o mesmo deverá ser baixado."), true);
                            ViewBag.bloqTipoUnico = true;
                        }
                        else
                        {
                            if (epiObj.ValidadeEmDias != null && epiObj.ValidadeEmDias > 0)
                            {
                                epiObj.DataVencimento = epiObj.DataEntrega.AddDays(epiObj.ValidadeEmDias.Value);
                            }

                            colaborador.Epis.Add(epiObj);
                            _colaboradorRepository.Update(colaborador);

                            AtualizarEstoqueEntrega(epiSetor.EpiId, epiObj.Quantidade == null ? 0 : Convert.ToInt32(epiObj.Quantidade));
                        }
                    }
                    else
                    {
                        Warning(String.Format("Não existe estoque suficiente para o EPI escolhido. Verifique o estoque!"), true);
                        ViewBag.bloqTipoUnico = true;
                    }
                }
            }

            return PartialView("_EpisEntregues", colaborador.Epis.Where(e => e.Baixado != null && !e.Baixado.Value).Where(e => e.DataVencimento > DateTime.Now));
        }

        [Authorize(Roles = "role_entregaepi, role_entrega_epi_outros")]
        public ActionResult RetirarEpi(int id)
        {
            var epi = _epiColaboradorRepository.GetById(id);
            var colaborador = _colaboradorRepository.GetByIdEager(epi.ColaboradorId);

            foreach (var item in colaborador.Epis)
            {
                if (item.Id.Equals(id))
                {
                    colaborador.Epis.Remove(item);

                    var epiSetor = _epiSetorRepository.GetById(item.EpiSetorId);
                    if (epiSetor != null)
                        AtualizarEstoqueEntrega(epiSetor.EpiId, -1 * (item.Quantidade == null ? 0 : Convert.ToInt32(item.Quantidade)));                 

                    break;
                }
            }

            _colaboradorRepository.Update(colaborador);
            return PartialView("_EpisEntregues", colaborador.Epis.Where(e => e.Baixado != null && !e.Baixado.Value).Where(e => e.DataVencimento > DateTime.Now));
        }

        [Authorize(Roles = "role_entregaepi, role_entrega_epi_outros")]
        public ActionResult BuscarEpi(string codigoDeBarras, int colaboradorId)
        {
            var epi = _epiRepository.BuscarPorCodigoDeBarras(codigoDeBarras);
            var colaborador = _colaboradorRepository.GetByIdEager(colaboradorId);
            var setor = _setorRepository.GetById((colaborador.SetorId ?? 0));

            if (epi != null && setor != null)
            {
                IEnumerable<EpiSetorModel> episSetor = _epiSetorRepository.BUscarPorTipoESetor(epi.TipoEpiId, setor.Id);
                IEnumerable<EpiSetorModel> episOutroSetor = _epiSetorRepository.BuscarPorTipoEOutrosSetores(epi.TipoEpiId, setor.Id);

                var epiSetor = episSetor.Where(es => es.EpiId == epi.Id).Where(es => es.TipoEpiId == epi.TipoEpiId).Where(es => es.SetorId == setor.Id).FirstOrDefault();
                var epiOutroSetor = episOutroSetor.Where(es => es.EpiId == epi.Id).Where(es => es.TipoEpiId == epi.TipoEpiId).FirstOrDefault();
                var lbc = _lbcRepository.BuscarPorNome("Matriz").FirstOrDefault();
                var centroCusto = _centroCustoRepository.BuscarPorNome("GERAL", lbc.Id).FirstOrDefault();
                if (centroCusto != null)
                {
                    ViewBag.CentroCustoId = new SelectList(_centroCustoRepository.BuscarAtivos(), "Id", "Nome", centroCusto.Id);
                }
                else
                {
                    ViewBag.CentroCustoId = new SelectList(_centroCustoRepository.BuscarAtivos(), "Id", "Nome");
                }                    

                if (epiSetor != null || epiOutroSetor != null)
                {
                    ViewBag.TipoEpiId = new SelectList(_tipoEpiRepository.BuscarAtivos(), "Id", "Nome", epi.TipoEpiId);

                    if (episSetor.Where(e => e.EpiId == epi.Id).Any())
                    {
                        ViewBag.EpiSetorId = new SelectList(episSetor, "Id", "DescricaoSelectList", epiSetor.Id);
                    }
                    else
                    {
                        ViewBag.EpiSetorId = new SelectList("", "");
                    }
                    if (episOutroSetor.Where(e => e.EpiId == epi.Id).Any())
                    {
                        if (!episSetor.Where(e => e.EpiId == epi.Id).Any())
                            ViewBag.EpiOutrSetoresId = new SelectList(episOutroSetor, "Id", "DescricaoSelectList", epiOutroSetor.Id);
                        else
                            ViewBag.EpiOutrSetoresId = new SelectList(episOutroSetor, "Id", "DescricaoSelectList");
                    }
                    else
                    {
                        ViewBag.EpiOutrSetoresId = new SelectList("", "");
                    }
                }
                else
                {
                    ViewBag.TipoEpiId = new SelectList(_tipoEpiRepository.BuscarAtivos(), "Id", "Nome");
                    ViewBag.EpiSetorId = new SelectList("", "");
                    ViewBag.EpiOutrSetoresId = new SelectList("", "");
                }
            }
            else
            {
                ViewBag.SetorId = new SelectList(_setorRepository.BuscarAtivos(), "Id", "Nome", colaborador.SetorId);
                ViewBag.TipoEpiId = new SelectList(_tipoEpiRepository.BuscarAtivos(), "Id", "Nome");
                ViewBag.EpiSetorId = new SelectList("", "");
                ViewBag.EpiOutrSetoresId = new SelectList("", "");
                ViewBag.CentroCustoId = new SelectList(_centroCustoRepository.BuscarAtivos(), "Id", "Nome");
                ViewBag.LbcId = new SelectList(_lbcRepository.BuscarAtivos(), "Id", "Fantasia", colaborador.LbcId);
                Warning(String.Format("Não foram encontrados registros!"), true);
            }
            return PartialView("_EntregarEpi");
        }

        [Authorize(Roles = "role_entregaepi, role_entrega_epi_outros")]
        public ActionResult BaixarEpi(string epis, string dataHoraBaixa)
        {
            var episABaixar = JsonConvert.DeserializeObject<List<EpiABaixar>>(epis);

            var primeiroEpi = episABaixar.ToList()[0];

            foreach (var epi in episABaixar)
            {
                if (epi.baixar)
                {
                    var epiABaixar = _epiColaboradorRepository.GetByIdEager(epi.id);

                    epiABaixar.Baixado = true;

                    DateTime data;
                    DateTime.TryParse(dataHoraBaixa, out data);
                    epiABaixar.DataHoraBaixa = data;

                    _epiColaboradorRepository.Update(epiABaixar);
                }
            }

            var episVencidos = _epiColaboradorRepository.BuscarEpisVencidos(primeiroEpi.idCol);

            return PartialView("_BaixarEpi", episVencidos.Where(e => e.Baixado != null && !e.Baixado.Value));
        }

        [Authorize(Roles = "role_entregaepi, role_entrega_epi_outros")]
        public ActionResult BaixarEpiEntregue(string idColab, string idEpi, string just, string dataHoraBaixa)
        {
            var epiABaixar = _epiColaboradorRepository.GetByIdEager(int.Parse(idEpi));
            epiABaixar.Baixado = true;

            DateTime data;
            DateTime.TryParse(dataHoraBaixa, out data);
            epiABaixar.DataHoraBaixa = data;
            epiABaixar.Justificativa = just;

            _epiColaboradorRepository.Update(epiABaixar);

            var episEntregues = _epiColaboradorRepository.BuscarNaoBaixados(epiABaixar.ColaboradorId);

            return PartialView("_EpisEntregues", episEntregues.Where(e => e.DataVencimento > DateTime.Now));
        }

        [Authorize(Roles = "role_entregaepi, role_entrega_epi_outros")]
        public JsonResult GetEpisTipoSetor(int idTipo, int idColab)
        {
            var colaborador = _colaboradorRepository.GetById(idColab);
            var setor = _setorRepository.GetById((colaborador.SetorId ?? 0));
            IEnumerable<EpiSetorModel> epis = _epiSetorRepository.BUscarPorTipoESetor(idTipo, setor.Id);

            return Json(new SelectList(epis, "Id", "NomeEpi"), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "role_entregaepi, role_entrega_epi_outros")]
        public JsonResult GetEpisTipoOutrSetores(int idTipo, int idColab)
        {
            var colaborador = _colaboradorRepository.GetById(idColab);
            var setor = _setorRepository.GetById((colaborador.SetorId ?? 0));
            IEnumerable<EpiSetorModel> epis = _epiSetorRepository.BuscarPorTipoEOutrosSetores(idTipo, setor.Id);

            return Json(new SelectList(epis, "Id", "DescricaoSelectList"), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "role_entregaepi, role_entrega_epi_outros")]
        public bool PossuiEpiMesmoTipo(ColaboradorModel colaborador, int? tipoEpi)
        {
            return colaborador.Epis.Where(e => e.Baixado != null && !e.Baixado.Value).Any(epiColaborador => epiColaborador.TipoEpiId == tipoEpi);
        }

        [Authorize(Roles = "role_entregaepi, role_entrega_epi_outros, role_entrega_uniforme, role_entrega_uniforme_outros")]
        public bool ExisteColaborador(string cpf)
        {
            return _colaboradorRepository.BuscarPorCpf(cpf).Any();
        }

        [Authorize(Roles = "role_entregaepi, role_entrega_epi_outros, role_entrega_uniforme, role_entrega_uniforme_outros")]
        public void AtualizarAssinaturaPendente(int id)
        {
            var colaborador = _colaboradorRepository.GetByIdEager(id);
            colaborador.Epis.ForEach(e => e.AssinaturaPendente = false);
            colaborador.Uniformes.ForEach(e => e.AssinaturaPendente = false);
            _colaboradorRepository.Update(colaborador);
        }

        [Authorize(Roles = "role_entregaepi, role_entrega_epi_outros")]
        public void AtualizarEstoqueEntrega(int idEpi, int qtde)
        {
            var estEpi = _estoqueEpiRepository.BuscarPorProduto(idEpi);

            var estEpiAlterar = _estoqueEpiRepository.GetById(estEpi.First().Id);
            estEpiAlterar.AtualizarEstoque(Convert.ToDecimal(qtde * -1));

            _estoqueEpiRepository.Update(estEpiAlterar);
        }

        // Desativado porque nao funciona no servidor da nuvem (param base64)
        //[HttpPost]
        ////public ActionResult VerificarBiometria(FormCollection form)
        //public ActionResult VerificarBiometria(int idColab, string biometria)
        //{
        //    ViewBag.BiometriaValida = "n";
        //    //int idColaborador = 0;
        //    //int.TryParse(form["IdColaborador"], out idColaborador);
        //    //var biometriaBd = form["FIRTextDataBd"];
        //    //var biometria = form["FIRTextData"];

        //    int idColaborador = idColab;
        //    var colaborador = _colaboradorRepository.GetById(idColaborador);
        //    var biometriaBd = colaborador.Biometria;            

        //    var objNBioBSP = new NBioBSPCOMLib.NBioBSP();
        //    var objMatching = objNBioBSP.Matching;

        //    objMatching.VerifyMatch((string)biometria, (string)biometriaBd);

        //    if (objMatching.MatchingResult == 1)
        //    {
        //        ViewBag.BiometriaValida = "s";
        //        Success(String.Format("Registro Biométrico Válido!"), true);
        //    }
        //    else
        //    {
        //        ViewBag.BiometriaValida = "n";
        //        Warning(String.Format("Registro Biométrico Não Compatível."), true);
        //    }

        //    //Release NBioBSP object
        //    //Set objMatching = nothing
        //    //Set objNBioBSP = nothing  

        //    ViewBag.SetorId = new SelectList(_setorRepository.BuscarAtivos(), "Id", "Nome", colaborador.SetorId);
        //    ViewBag.TipoEpiId = new SelectList(_tipoEpiRepository.BuscarAtivos(), "Id", "Nome");
        //    ViewBag.EpiSetorId = new SelectList("", "");
        //    ViewBag.EpiOutrSetoresId = new SelectList("", "");
        //    ViewBag.CentroCustoId = new SelectList(_centroCustoRepository.BuscarAtivos(), "Id", "Nome");

        //    return PartialView("_EntregarEpi");
        //}

        //******* Entrega de Uniforme *******//
        [Authorize(Roles = "role_entrega_uniforme, role_entrega_uniforme_outros")]
        [HttpGet]
        public ActionResult EntregaDeUniforme(int? idColaborador, bool hasBiometria)
        {
            if (idColaborador.HasValue)
            {
                var colaborador = _colaboradorRepository.GetByIdEager(idColaborador.Value);
                if (!colaborador.Ativo)
                {
                    Warning("Colaboradores inativos não podem receber uniforme", true);
                    return RedirectToAction("Index");
                }
                var centroCusto = _centroCustoRepository.BuscarPorNome("GERAL", null).FirstOrDefault();

                ViewBag.TipoUniformeId = new SelectList(_tipoUniformeRepository.BuscarAtivos(), "Id", "Nome");
                ViewBag.UniformeSetorId = new SelectList("", "");
                ViewBag.UniformeOutrSetoresId = new SelectList("", "");
                ViewBag.CentroCustoItems = new SelectList(_centroCustoRepository.BuscarAtivos(), "Id", "Nome", centroCusto.Id);
                ViewBag.ValidarBiom = hasBiometria;
                return View(colaborador);
            }
            return RedirectToAction("Search", "Biometria");
        }

        [Authorize(Roles = "role_entrega_uniforme, role_entrega_uniforme_outros")]
        [HttpPost]
        public ActionResult EntregaDeUniforme(ColaboradorModel model)
        {
            if (!model.Ativo)
            {
                Warning("Colaboradores inativos não podem receber uniforme", true);
                return RedirectToAction("Index");
            }
            return RedirectToAction("GerarComprovanteEntregaUniforme", "Report", new { dataInicial = DateTime.Now.Date, dataFinal = DateTime.Now.Date, idColaborador = model.Id });
        }

        [Authorize(Roles = "role_entrega_uniforme, role_entrega_uniforme_outros")]
        public ActionResult EntregarUniforme(string idModel, string uniforme, string dataEntrega, bool hasBiometria)
        {
            DateTime data;
            DateTime.TryParse(dataEntrega, out data);
            var colaborador = _colaboradorRepository.GetByIdEager(Convert.ToInt32(idModel));
            if (!colaborador.Ativo)
            {
                //enviar mensagem de erro colaborador nao pode receber uniforme.
                Warning(String.Format("Este colaborador foi inativado e não pode receber uniforme!"), true);
                ViewBag.bloqTipoUnico = true;
            }
            else
            {
                var uniformeObj = JsonConvert.DeserializeObject<UniformeColaboradorModel>(uniforme);
                uniformeObj.DataEntrega = data;
                var uniformeSetor = _uniformeSetorRepository.GetById(uniformeObj.UniformeSetorId);
                var setorUniforme = _setorRepository.GetById(uniformeSetor.SetorId);

                uniformeObj.ValidadeEmDias = uniformeSetor.ValidadeEmDias;
                uniformeObj.NomeSetor = setorUniforme.Nome;
                uniformeObj.NomeUniforme = uniformeSetor.NomeUniforme;
                uniformeObj.TipoUniformeId = uniformeSetor.TipoUniformeId;
                uniformeObj.AssinaturaPendente = true;

                uniformeObj.IdEmpresa = colaborador.IdEmpresa;
                uniformeObj.UnidadeNegocioId = colaborador.LbcId;

                uniformeObj.TipoValidacao = hasBiometria;

                var bloquearPorTipo = _configuracaoRepository.GetUnique();
                if (uniformeObj.NomeUniforme != "Selecione")
                {
                    var estUniforme = _estoqueUniformeRepository.BuscarPorProduto(uniformeSetor.UniformeId);
                    var estUniformeVerificar = _estoqueUniformeRepository.GetById(estUniforme.First().Id);

                    if (estUniformeVerificar.Qtde >= uniformeObj.Quantidade)
                    {

                        if (bloquearPorTipo.BloquearPorTipoUniformeUnico)
                        {
                            if (!PossuiUniformeMesmoTipo(colaborador, uniformeObj.TipoUniformeId))
                            {
                                if (uniformeObj.ValidadeEmDias != null && uniformeObj.ValidadeEmDias > 0)
                                {
                                    uniformeObj.DataVencimento = uniformeObj.DataEntrega.AddDays(uniformeObj.ValidadeEmDias.Value);
                                }


                                colaborador.Uniformes.Add(uniformeObj);
                                _colaboradorRepository.Update(colaborador);

                                AtualizarEstoqueUniformeEntrega(uniformeSetor.UniformeId, uniformeObj.Quantidade == null ? 0 : Convert.ToInt32(uniformeObj.Quantidade));

                                foreach (var item in colaborador.Uniformes)
                                {
                                    item.UnidadeNogocio = _lbcRepository.GetById(Convert.ToInt32(item.UnidadeNegocioId));
                                }

                                return PartialView("_UniformesEntregues", colaborador.Uniformes.Where(e => e.Baixado != null && !e.Baixado.Value).Where(e => e.DataVencimento > DateTime.Now));
                            }

                            Warning(String.Format("Este colaborador já possui um uniforme deste tipo. Antes de pegar outro o mesmo deverá ser baixado."), true);
                            ViewBag.bloqTipoUnico = true;
                        }
                        else
                        {
                            if (uniformeObj.ValidadeEmDias != null && uniformeObj.ValidadeEmDias > 0)
                            {
                                uniformeObj.DataVencimento = uniformeObj.DataEntrega.AddDays(uniformeObj.ValidadeEmDias.Value);
                            }

                            colaborador.Uniformes.Add(uniformeObj);
                            _colaboradorRepository.Update(colaborador);

                            AtualizarEstoqueUniformeEntrega(uniformeSetor.UniformeId, uniformeObj.Quantidade == null ? 0 : Convert.ToInt32(uniformeObj.Quantidade));
                        }
                    }
                    else
                    {
                        Warning(String.Format("Não existe estoque suficiente para o uniforme escolhido. Verifique o estoque!"), true);
                        ViewBag.bloqTipoUnico = true;
                    }
                }
            }

            return PartialView("_UniformesEntregues", colaborador.Uniformes.Where(e => e.Baixado != null && !e.Baixado.Value).Where(e => e.DataVencimento > DateTime.Now));
        }

        [Authorize(Roles = "role_entrega_uniforme, role_entrega_uniforme_outros")]
        public ActionResult RetirarUniforme(int id)
        {
            var uniforme = _uniformeColaboradorRepository.GetById(id);
            var colaborador = _colaboradorRepository.GetByIdEager(uniforme.ColaboradorId);

            foreach (var item in colaborador.Uniformes)
            {
                if (item.Id.Equals(id))
                {
                    colaborador.Uniformes.Remove(item);

                    var uniformeSetor = _uniformeSetorRepository.GetById(item.UniformeSetorId);
                    if (uniformeSetor != null)
                        AtualizarEstoqueUniformeEntrega(uniformeSetor.UniformeId, -1 * (item.Quantidade == null ? 0 : Convert.ToInt32(item.Quantidade)));

                    break;
                }
            }

            _colaboradorRepository.Update(colaborador);
            return PartialView("_UniformesEntregues", colaborador.Uniformes.Where(e => e.Baixado != null && !e.Baixado.Value).Where(e => e.DataVencimento > DateTime.Now));
        }

        [Authorize(Roles = "role_entrega_uniforme, role_entrega_uniforme_outros")]
        public ActionResult BuscarUniforme(string codigoDeBarras, int colaboradorId)
        {
            var uniforme = _uniformeRepository.BuscarPorCodigoDeBarras(codigoDeBarras);
            var colaborador = _colaboradorRepository.GetByIdEager(colaboradorId);
            var setor = _setorRepository.GetById((colaborador.SetorId ?? 0));

            if (uniforme != null && setor != null)
            {
                IEnumerable<UniformeSetorModel> uniformesSetor = _uniformeSetorRepository.BuscarPorTipoESetor(uniforme.TipoUniformeId, setor.Id);
                IEnumerable<UniformeSetorModel> uniformesOutroSetor = _uniformeSetorRepository.BuscarPorTipoEOutrosSetores(uniforme.TipoUniformeId, setor.Id);

                var uniformeSetor = uniformesSetor.Where(es => es.UniformeId == uniforme.Id).Where(es => es.TipoUniformeId == uniforme.TipoUniformeId).Where(es => es.SetorId == setor.Id).FirstOrDefault();
                var uniformeOutroSetor = uniformesOutroSetor.Where(es => es.UniformeId == uniforme.Id).Where(es => es.TipoUniformeId == uniforme.TipoUniformeId).FirstOrDefault();
                var lbc = _lbcRepository.BuscarPorNome("Matriz").FirstOrDefault();
                var centroCusto = _centroCustoRepository.BuscarPorNome("GERAL", lbc.Id).FirstOrDefault();
                if (centroCusto != null)
                {
                    ViewBag.CentroCustoId = new SelectList(_centroCustoRepository.BuscarAtivos(), "Id", "Nome", centroCusto.Id);
                }
                else
                {
                    ViewBag.CentroCustoId = new SelectList(_centroCustoRepository.BuscarAtivos(), "Id", "Nome");
                }

                if (uniformeSetor != null || uniformeOutroSetor != null)
                {
                    ViewBag.TipoUniformeId = new SelectList(_tipoUniformeRepository.BuscarAtivos(), "Id", "Nome", uniforme.TipoUniformeId);

                    if (uniformesSetor.Where(e => e.UniformeId == uniforme.Id).Any())
                    {
                        ViewBag.UniformeSetorId = new SelectList(uniformesSetor, "Id", "DescricaoSelectList", uniformeSetor.Id);
                    }
                    else
                    {
                        ViewBag.UniformeSetorId = new SelectList("", "");
                    }
                    if (uniformesOutroSetor.Where(e => e.UniformeId == uniforme.Id).Any())
                    {
                        if (!uniformesSetor.Where(e => e.UniformeId == uniforme.Id).Any())
                            ViewBag.UniformeOutrSetoresId = new SelectList(uniformesOutroSetor, "Id", "DescricaoSelectList", uniformeOutroSetor.Id);
                        else
                            ViewBag.UniformeOutrSetoresId = new SelectList(uniformesOutroSetor, "Id", "DescricaoSelectList");
                    }
                    else
                    {
                        ViewBag.UniformeOutrSetoresId = new SelectList("", "");
                    }
                }
                else
                {
                    ViewBag.TipoUniformeId = new SelectList(_tipoUniformeRepository.BuscarAtivos(), "Id", "Nome");
                    ViewBag.UniformeSetorId = new SelectList("", "");
                    ViewBag.UniformeOutrSetoresId = new SelectList("", "");
                }
            }
            else
            {
                ViewBag.SetorId = new SelectList(_setorRepository.BuscarAtivos(), "Id", "Nome", colaborador.SetorId);
                ViewBag.TipoUniformeId = new SelectList(_tipoUniformeRepository.BuscarAtivos(), "Id", "Nome");
                ViewBag.UniformeSetorId = new SelectList("", "");
                ViewBag.UniformeOutrSetoresId = new SelectList("", "");
                ViewBag.CentroCustoId = new SelectList(_centroCustoRepository.BuscarAtivos(), "Id", "Nome");
                ViewBag.LbcId = new SelectList(_lbcRepository.BuscarAtivos(), "Id", "Fantasia", colaborador.LbcId);
                Warning(String.Format("Não foram encontrados registros!"), true);
            }
            return PartialView("_EntregarUniforme");
        }

        [Authorize(Roles = "role_entrega_uniforme, role_entrega_uniforme_outros")]
        public ActionResult BaixarUniforme(string uniformes, string dataHoraBaixa)
        {
            var uniformesABaixar = JsonConvert.DeserializeObject<List<UniformeABaixar>>(uniformes);

            var primeiroUniforme = uniformesABaixar.ToList()[0];

            foreach (var uniforme in uniformesABaixar)
            {
                if (uniforme.baixar)
                {
                    var uniformeABaixar = _uniformeColaboradorRepository.GetByIdEager(uniforme.id);

                    uniformeABaixar.Baixado = true;

                    DateTime data;
                    DateTime.TryParse(dataHoraBaixa, out data);
                    uniformeABaixar.DataHoraBaixa = data;

                    _uniformeColaboradorRepository.Update(uniformeABaixar);
                }
            }

            var uniformesVencidos = _uniformeColaboradorRepository.BuscarUniformesVencidos(primeiroUniforme.idCol);

            return PartialView("_BaixarUniforme", uniformesVencidos.Where(e => e.Baixado != null && !e.Baixado.Value));
        }

        [Authorize(Roles = "role_entrega_uniforme, role_entrega_uniforme_outros")]
        public ActionResult BaixarUniformeEntregue(string idColab, string idUniforme, string just, string dataHoraBaixa)
        {
            var uniformeABaixar = _uniformeColaboradorRepository.GetByIdEager(int.Parse(idUniforme));
            uniformeABaixar.Baixado = true;

            DateTime data;
            DateTime.TryParse(dataHoraBaixa, out data);
            uniformeABaixar.DataHoraBaixa = data;
            uniformeABaixar.Justificativa = just;

            _uniformeColaboradorRepository.Update(uniformeABaixar);

            var uniformesEntregues = _uniformeColaboradorRepository.BuscarNaoBaixados(uniformeABaixar.ColaboradorId);

            return PartialView("_UniformesEntregues", uniformesEntregues.Where(e => e.DataVencimento > DateTime.Now));
        }

        [Authorize(Roles = "role_entrega_uniforme, role_entrega_uniforme_outros")]
        public JsonResult GetUniformesTipoSetor(int idTipo, int idColab)
        {
            var colaborador = _colaboradorRepository.GetById(idColab);
            var setor = _setorRepository.GetById((colaborador.SetorId ?? 0));
            IEnumerable<UniformeSetorModel> uniformes = _uniformeSetorRepository.BuscarPorTipoESetor(idTipo, setor.Id);

            return Json(new SelectList(uniformes, "Id", "NomeUniforme"), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "role_entrega_uniforme, role_entrega_uniforme_outros")]
        public JsonResult GetUniformesTipoOutrSetores(int idTipo, int idColab)
        {
            var colaborador = _colaboradorRepository.GetById(idColab);
            var setor = _setorRepository.GetById((colaborador.SetorId ?? 0));
            IEnumerable<UniformeSetorModel> uniformes = _uniformeSetorRepository.BuscarPorTipoEOutrosSetores(idTipo, setor.Id);

            return Json(new SelectList(uniformes, "Id", "DescricaoSelectList"), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "role_entrega_uniforme, role_entrega_uniforme_outros")]
        public bool PossuiUniformeMesmoTipo(ColaboradorModel colaborador, int? tipoUniforme)
        {
            return colaborador.Uniformes.Where(e => e.Baixado != null && !e.Baixado.Value).Any(uniformeColaborador => uniformeColaborador.TipoUniformeId == tipoUniforme);
        }

        [Authorize(Roles = "role_entrega_uniforme, role_entrega_uniforme_outros")]
        public void AtualizarEstoqueUniformeEntrega(int idUniforme, int qtde)
        {
            var estUniforme = _estoqueUniformeRepository.BuscarPorProduto(idUniforme);

            var estUniformeAlterar = _estoqueUniformeRepository.GetById(estUniforme.First().Id);
            estUniformeAlterar.AtualizarEstoque(Convert.ToDecimal(qtde * -1));

            _estoqueUniformeRepository.Update(estUniformeAlterar);
        }
    }

    public class EpiABaixar
    {
        public int idCol { get; set; }
        public int id { get; set; }
        public bool baixar { get; set; }
    }
        
    public class UniformeABaixar
    {
        public int idCol { get; set; }
        public int id { get; set; }
        public bool baixar { get; set; }
    }
}
