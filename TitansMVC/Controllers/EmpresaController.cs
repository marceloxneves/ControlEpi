using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using TitansMVC.Models;
using TitansMVC.Repository.Implementations;
using TitansMVC.Repository.Interfaces;
using TitansMVC.Utils;
using WebGrease.Css.Extensions;
using PagedList;
using TitansMVC.Models.Enums;

namespace TitansMVC.Controllers
{
    //ok
    [Authorize(Roles = "role_admin, role_master")]
    public class EmpresaController : BaseController
    {
        private readonly IEmpresaRepository _empresaRepository;
        private readonly IUfRepository _ufRepository;
        private readonly IMunicipioRepository _municipioRepository;
        private readonly ITelefoneEmpresaRepository _telefoneEmprRepository;
        private readonly IEmailEmpresaRepository _emailEmprRepository;
        private readonly IColaboradorRepository _colaboradorRepository;
        private readonly IPlanoRepository _planoRepository;
          
        public EmpresaController()
        {
            _empresaRepository = new EmpresaRepository();
            _ufRepository = new UfRepository();
            _municipioRepository = new MunicipioRepository();
            _telefoneEmprRepository = new TelefoneEmpresaRepository();
            _emailEmprRepository = new EmailEmpresaRepository();
            _colaboradorRepository = new ColaboradorRepository();
            _planoRepository = new PlanoRepository();
        }

        // GET: Empresa
        public ActionResult Index(string searchBy, string search, int? page)
        {
            var empresas = _empresaRepository.BuscarAtivos();

            if (searchBy == "Razao")
            {
                empresas = _empresaRepository.BuscarPorRazao(razao: search);
            }
            else if (searchBy == "Todos")
            {
                empresas = _empresaRepository.BuscarAtivos();
            }

            return View(empresas.OrderBy(e=>e.Razao).ToPagedList(page ?? 1, 10));
        }

        // GET: Empresa/Create
        public ActionResult Create()
        {
            ViewBag.SiglaUf = new SelectList(_ufRepository.GetAll(), "Sigla", "Nome");

            return View();
        }

        // POST: Empresa/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EmpresaModel empresa)
        {
            if (ModelState.IsValid)
            {
                empresa.Ativo = true;

                _empresaRepository.Add(empresa);

                //return RedirectToAction("Index");
                Success(string.Format("Registro gravado com sucesso."), true);
                return RedirectToAction("Edit", empresa);
            }

            ViewBag.SiglaUf = new SelectList(_ufRepository.GetAll(), "Sigla", "Nome", empresa.SiglaUf);
            ViewBag.MunicipioId = new SelectList(_municipioRepository.BuscarPorUf(empresa.SiglaUf), "Id", "Nome", empresa.MunicipioId);

            return View(empresa);
        }

        // GET: Empresa/Edit/5
        public ActionResult Edit(int id)
        {
            var empresa = _empresaRepository.GetByIdEager(id);

            ViewBag.SiglaUf = new SelectList(_ufRepository.GetAll(), "Sigla", "Nome", empresa.SiglaUf);
            ViewBag.MunicipioId = new SelectList(_municipioRepository.BuscarPorUf(empresa.SiglaUf), "Id", "Nome", empresa.MunicipioId);

            var planoCorrente = _planoRepository.GetByCnpj(empresa.Cnpj);

            if (planoCorrente != null)
            {
                ViewBag.PlanoNome = planoCorrente.NivelPlano.ToString();
                ViewBag.ValidadePlano = Encryptor.Decrypt(planoCorrente.ValidadeCriptografada);
                ViewBag.NumeroDeColaboradoresRestantes = Util.GetNumeroColaboradoresRestante(empresa.Id, planoCorrente.NivelPlano.ToString()).ToString() + " colaboradores";

                if (Util.GetNumeroColaboradoresRestante(empresa.Id, planoCorrente.NivelPlano.ToString()) == -1)
                {
                    ViewBag.NumeroDeColaboradoresRestantes = "Esta empresa não possui um limite de colaboradores.";
                }
            }
            
            //ViewBag.PlanoNome = Util.GetEmpresaPlano(empresa.Cnpj);
            //ViewBag.ValidadePlano = Util.GetEmpresaPlanoValidade(empresa.Cnpj).ToString("dd/MM/yyyy");
            //ViewBag.NumeroDeColaboradoresRestantes = Util.GetNumeroColaboradoresRestante(empresa.Cnpj, empresa.Id);

            return View(empresa);
        }

        // POST: Empresa/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EmpresaModel empresa, HttpPostedFileBase uploadlogo, int planoOption = 0)
        {
            ViewBag.SiglaUf = new SelectList(_ufRepository.GetAll(), "Sigla", "Nome", empresa.SiglaUf);
            ViewBag.MunicipioId = new SelectList(_municipioRepository.BuscarPorUf(empresa.SiglaUf), "Id", "Nome", empresa.MunicipioId);

            var planoCorrente = _planoRepository.GetByCnpj(empresa.Cnpj);

            ViewBag.PlanoNome = planoCorrente.NivelPlano.ToString();
            ViewBag.ValidadePlano = Encryptor.Decrypt(planoCorrente.ValidadeCriptografada);
            ViewBag.NumeroDeColaboradoresRestantes = Util.GetNumeroColaboradoresRestante(empresa.Id, planoCorrente.NivelPlano.ToString());

            if (Convert.ToInt32(ViewBag.NumeroDeColaboradoresRestantes) == -1)
            {
                ViewBag.NumeroDeColaboradoresRestantes = "Esta empresa não possui um limite de colaboradores.";
            }

            ViewBag.NumeroDeColaboradoresRestantes = ViewBag.NumeroDeColaboradoresRestantes.ToString() + "colaboradores";

            if (planoOption == 1)
            {
                PlanoModel plano = _planoRepository.CopiarPlanoServidorParaCliente(empresa.Cnpj);
                var numeroColaboradores = _colaboradorRepository.ContarColaboradores(Util.GetEmpresaId());
                string result;

                ViewBag.PlanoNome = plano.NivelPlano.ToString();
                ViewBag.ValidadePlano = plano.Validade.ToString();
                switch (plano.NivelPlano.ToString())
                {
                    case "Starter":
                        result = (100 - numeroColaboradores).ToString();
                        break;
                    case "Basic":
                        result = (300 - numeroColaboradores).ToString();
                        break;
                    case "Standard":
                        result = (500 - numeroColaboradores).ToString();
                        break;
                    case "Master":
                        result = (1000 - numeroColaboradores).ToString();
                        break;
                    case "Ultimate":
                        result = "Esta empresa não possui um limite de";
                        break;
                    default:
                        result = "Plano não existe!";
                        break;
                }

                ViewBag.NumeroDeColaboradoresRestantes = result + " colaboradores";

                //ViewBag.PlanoNome = Util.GetEmpresaPlano(empresa.Cnpj);
                //ViewBag.ValidadePlano = Util.GetEmpresaPlanoValidade(empresa.Cnpj).ToString("dd/MM/yyyy");
                //ViewBag.NumeroDeColaboradoresRestantes = Util.GetNumeroColaboradoresRestante(empresa.Cnpj, empresa.Id);
            }
            var empresaRepo = _empresaRepository.GetByIdEager(empresa.Id);

            empresa.Logomarca = empresaRepo.Logomarca;
            foreach (var telefone in empresaRepo.Telefones)
            {
                empresa.Telefones.Add(telefone);
            }

            foreach (var email in empresaRepo.Emails)
            {
                empresa.Emails.Add(email);
            }

            _empresaRepository.RetirarDoContexto(empresaRepo);

            if (ModelState.IsValid)
            {
                if (uploadlogo != null && uploadlogo.ContentLength > 0)
                {
                    try
                    {
                        using (var reader = new BinaryReader(uploadlogo.InputStream))
                        {
                            empresa.Logomarca = reader.ReadBytes(uploadlogo.ContentLength);
                        }
                    }
                    catch (RetryLimitExceededException /* dex */)
                    {
                        ModelState.AddModelError("", "Não foi possível salvar as alterações. Tente novamente, e se o problema persistir entre em contato com o administrador do sistema.");
                    }
                }
                else
                {
                    empresa.Logomarca = null;
                }

                _empresaRepository.Update(empresa);

                Success(string.Format("Registro alterado com sucesso."), true);
                return View(empresa);
            }            

            return View(empresa);
        }

        // GET: Empresa/Delete/5
        public ActionResult Delete(int id)
        {
            var empresa = _empresaRepository.GetById(id);

            return View(empresa);
        }

        // POST: Empresa/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //var empresa = _empresaRepository.GetById(id);
            //_empresaRepository.Remove(empresa);

            _empresaRepository.RemoveLogical(id);

            return RedirectToAction("Index");
        }


        public ActionResult AdicionarTelefone(string idModel, string telefone)
        {
            var telefoneModel = JsonConvert.DeserializeObject<TelefoneEmpresaModel>(telefone);
            var empresa = _empresaRepository.GetByIdEager(Convert.ToInt32(idModel));
            empresa.Telefones.Add(telefoneModel);

            _empresaRepository.Update(empresa);

            return PartialView("_Telefones", empresa.Telefones);
        }

        public ActionResult RemoverTelefone(int id)
        {
            var telefone = _telefoneEmprRepository.GetById(id);
            var empresa = _empresaRepository.GetByIdEager(telefone.IdEmpresa);

            foreach (var item in empresa.Telefones)
            {
                if (item.Id.Equals(id))
                {
                    empresa.Telefones.Remove(item);
                    break;
                }
            }

            _empresaRepository.Update(empresa);

            return PartialView("_Telefones", empresa.Telefones);
        }

        public ActionResult AdicionarEmail(string idModel, string email)
        {
            var emailModel = JsonConvert.DeserializeObject<EmailEmpresaModel>(email);
            var empresa = _empresaRepository.GetByIdEager(Convert.ToInt32(idModel));
            empresa.Emails.Add(emailModel);

            _empresaRepository.Update(empresa);

            return PartialView("_Emails", empresa.Emails);
        }

        public ActionResult RemoverEmail(int id)
        {
            var email = _emailEmprRepository.GetById(id);
            var empresa = _empresaRepository.GetByIdEager(email.IdEmpresa);

            foreach (var item in empresa.Emails)
            {
                if (item.Id.Equals(id))
                {
                    empresa.Emails.Remove(item);
                    break;
                }
            }

            _empresaRepository.Update(empresa);

            return PartialView("_Emails", empresa.Emails);
        }

        public JsonResult GetMunicipios(string siglaUf)
        {
            IEnumerable<MunicipioModel> municipios = _municipioRepository.BuscarPorUf(siglaUf);

            return Json(new SelectList(municipios, "Id", "Nome"), JsonRequestBehavior.AllowGet);
        }

      
    }
}
