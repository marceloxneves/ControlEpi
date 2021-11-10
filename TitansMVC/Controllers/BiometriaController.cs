using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TitansMVC.Models;
using TitansMVC.Repository.Implementations;
using TitansMVC.Repository.Interfaces;
using TitansMVC.Utils;
using NITGEN.SDK.NBioBSP;


namespace TitansMVC.Controllers
{
    [Authorize]
    public class BiometriaController : BaseController
    {
        private NBioAPI m_NBioAPI;
        private IColaboradorRepository _colaboradorRepo;

        public BiometriaController()
        {
            _colaboradorRepo = new ColaboradorRepository();
        }
        [HttpGet]
        [Authorize(Roles = "role_base, role_base_uniforme")]
        public ActionResult Create(int? id)
        {
            var empresa = Util.GetEmpresa();
            ViewBag.PossuiBiometria = empresa.PossuiBiometria;

            if (!id.HasValue)
            {
                Warning("Necessário informar o identificador do colaborador",true);
                return RedirectToAction("Index","Home");
            }
            ViewBag.nomeColaborador = _colaboradorRepo.GetById(id.Value).Nome;
            ViewBag.IdColaborador = id;
            return View();
        }

        // GET: Biometria
        [Authorize(Roles = "role_base, role_base_uniforme")]        
        public ActionResult Index()
        {
            ViewBag.IdColaborador = new SelectList(_colaboradorRepo.BuscarAtivos(), "Id", "Nome");

            //return View();
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "role_base, role_base_uniforme")]
        [HttpPost]
        public ActionResult Create(FormCollection form)
        {
            var empresa = Util.GetEmpresa();
            ViewBag.PossuiBiometria = empresa.PossuiBiometria;
            ViewBag.IdColaborador = form["idColaborador"];
            var biometria = form["FIRTextData"];
            int idColaborador = 0;
            int.TryParse(form["IdColaborador"], out idColaborador);
            var colaborador = _colaboradorRepo.GetById(idColaborador);
            if (biometria != "")
            {
                colaborador.Biometria = biometria;
                _colaboradorRepo.Update(colaborador);
                Success(String.Format("Biometria Registrada com sucesso!"), true);
                //return RedirectToAction("Search");
                return RedirectToAction("Index","Colaborador");
            }
            else
            {
                Warning("Biometria não registrada!!");
                return RedirectToAction("Create", "Biometria", new { id = idColaborador });
            }
        }

        [HttpGet]
        [Authorize(Roles = "role_entregaepi, role_entrega_uniforme")]
        public ActionResult Search(string tipoEntrega)
        {
            var empresa = Util.GetEmpresa();
            ViewBag.PossuiBiometria = empresa.PossuiBiometria;

            if (tipoEntrega == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.TipoEntrega = tipoEntrega;
                return View();
            }
            
        }

        [HttpPost]
        [Authorize(Roles = "role_entregaepi, role_entrega_uniforme")]
        public ActionResult Search(FormCollection form)
        {
            m_NBioAPI = new NBioAPI();
            var hCapturedFIR = form["FIRTextData"];
            var empresa = Util.GetEmpresa();
            ViewBag.PossuiBiometria = empresa.PossuiBiometria;       
            ViewBag.TipoEntrega = form["SearchBiometria"];
            NBioAPI.Type.FIR_PAYLOAD myPayload = new NBioAPI.Type.FIR_PAYLOAD();
            uint ret;
            var colaboradores = _colaboradorRepo.BuscarAtivos();
            bool result;
            NBioAPI.Type.FIR_TEXTENCODE
                    textoCapt = new NBioAPI.Type.FIR_TEXTENCODE(),
                    textoStored = new NBioAPI.Type.FIR_TEXTENCODE();
            textoCapt.TextFIR = hCapturedFIR;
            foreach (var colaborador in colaboradores)
            {
                if (colaborador.Biometria == null || colaborador.Biometria == "")
                    continue;
                var hStoredFIR = colaborador.Biometria;
                textoStored.TextFIR = hStoredFIR;
                ret = m_NBioAPI.VerifyMatch(textoCapt, textoStored, out result, myPayload);
                if (ret == NBioAPI.Error.NONE)
                {
                    //verificação bem sucedida
                    if (result)
                    {
                        //encontrou
                        //return RedirectToAction("EntregaDeEpi", "Colaborador", new { idColaborador = colaborador.Id, hasBiometria = true });
                        if (form["SearchBiometria"].Equals("Epi"))
                        {
                            return RedirectToAction("EntregaDeEpi", "Colaborador", new { idColaborador = colaborador.Id, hasBiometria = true });
                        }
                        else if (form["SearchBiometria"].Equals("Uniforme"))
                        {
                            return RedirectToAction("EntregaDeUniforme", "Colaborador", new { idColaborador = colaborador.Id, hasBiometria = true });
                        }
                        else
                            return View();
                    }
                }
                else
                {
                    Warning("Ocorreu um erro!");
                    return View();
                    //erro
                }
            }
            Warning("Colaborador não encontrado!");
            return View();
        }
    }
}
