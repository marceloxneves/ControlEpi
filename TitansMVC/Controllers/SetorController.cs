using System;
using System.Web.Mvc;
using Newtonsoft.Json;
using PagedList;
using TitansMVC.Models;
using TitansMVC.Repository.Implementations;
using TitansMVC.Repository.Interfaces;

namespace TitansMVC.Controllers
{

    [Authorize(Roles = "role_auxiliares, role_auxiliares_uniforme")]
    public class SetorController : BaseController
    {
        private readonly ISetorRepository _setorRepository;
        private readonly IEpiRepository _epiRepository;
        private readonly IEpiColaboradorRepository _epiColaboradorRepository;
        private readonly IEpiSetorRepository _epiSetorRepository;
        private readonly ILbcRepository _lbcRepository;
        private readonly IUniformeRepository _uniformeRepository;
        private readonly IUniformeColaboradorRepository _uniformeColaboradorRepository;
        private readonly IUniformeSetorRepository _uniformeSetorRepository;


        public SetorController()
        {
            _setorRepository = new SetorRepository();
            _epiRepository = new EpiRepository();
            _epiSetorRepository = new EpiSetorRepository();            
            _epiColaboradorRepository = new EpiColaboradorRepository();
            _lbcRepository = new LbcRepository();
            _uniformeRepository = new UniformeRepository();
            _uniformeSetorRepository = new UniformeSetorRepository();
            _uniformeColaboradorRepository = new UniformeColaboradorRepository();

        }

        // GET: Setor
        public ActionResult Index(string searchBy, string search, int? page, int? UnidadeNegocioId)
        {
            ViewBag.UnidadeNegocioId = new SelectList(_lbcRepository.BuscarAtivos(), "Id", "Fantasia");
            var setores = _setorRepository.BuscarAtivos();

            if (searchBy == "Nome")
            {
                setores = _setorRepository.BuscarPorNome(nome: search, UnidadeNegocioId: UnidadeNegocioId);
            }
            else if (searchBy == "Todos")
            {
                setores = UnidadeNegocioId == null ?
                    _setorRepository.BuscarAtivos() :
                    _setorRepository.BuscarPorUnidadeNegocio(UnidadeNegocioId.Value);
            }

            return View(setores.ToPagedList(page ?? 1, 10));
        }

        // GET: Setor/Create
        public ActionResult Create()
        {
            ViewBag.LbcId = new SelectList(_lbcRepository.BuscarAtivos(), "Id", "Fantasia");
            return View();
        }

        // POST: Setor/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SetorModel setor)
        {
            if (!ExisteSetor(setor.Nome))
            {
                if (ModelState.IsValid)
                {
                    setor.Ativo = true;

                    _setorRepository.Add(setor);

                    Success(String.Format("Registro gravado com sucesso!"), true);

                    return RedirectToAction("Edit", setor);
                }
            }

            Warning(String.Format("O sistema já possui um setor com este nome cadastrado."), true);

            return View(setor);
        }

        // GET: Setor/Edit/5
        public ActionResult Edit(int id)
        {
            var setor = _setorRepository.GetByIdEager(id);

            ViewBag.EpiId = new SelectList(_epiRepository.BuscarAtivos(), "Id", "Nome");
            ViewBag.UnidadeNegocioId = new SelectList(_lbcRepository.BuscarAtivos(), "Id", "Fantasia", setor.UnidadeNegocioId);
            ViewBag.UniformeId = new SelectList(_uniformeRepository.BuscarAtivos(), "Id", "Nome");

            return View(setor);
        }

        // POST: Setor/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SetorModel setor)
        {
            ViewBag.EpiId = new SelectList(_epiRepository.BuscarAtivos(), "Id", "Nome");
            ViewBag.UnidadeNegocioId = new SelectList(_lbcRepository.BuscarAtivos(), "Id", "Fantasia", setor.UnidadeNegocioId);
            ViewBag.UniformeId = new SelectList(_uniformeRepository.BuscarAtivos(), "Id", "Nome");

            var setorRepo = _setorRepository.GetByIdEager(setor.Id);
            
            foreach (var epi in setorRepo.Epis)
            {
                setor.Epis.Add(epi);
            }

            _setorRepository.RetirarDoContexto(setorRepo);

            var setorRepoUni = _setorRepository.GetByIdEager(setor.Id);

            foreach (var uniforme in setorRepo.Uniformes)
            {
                setor.Uniformes.Add(uniforme);
            }

            _setorRepository.RetirarDoContexto(setorRepoUni);

            if (ModelState.IsValid)
            {
                _setorRepository.Update(setor);

                Success(String.Format("Registro alterado com sucesso!"), true);

                return View(setor);
            }

            ViewBag.partialPermission = false;

            return View(setor);
        }

        // GET: Setor/Delete/5
        public ActionResult Delete(int id)
        {
            var setor = _setorRepository.GetById(id);

            return View(setor);
        }

        // POST: Setor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var setor = _setorRepository.GetById(id);

            if (_setorRepository.QtdeColaboradores(setor.Id) > 0)
            {
                ViewBag.PossuiColaboradores = true;
                return View(setor);
            }

            //_setorRepository.Remove(setor);
            _setorRepository.RemoveLogical(id);

            return RedirectToAction("Index");
        }

        public ActionResult AdicionarEpi(string idModel, string epi)
        {
            var epiObj = JsonConvert.DeserializeObject<EpiSetorModel>(epi);
            var setorDomain = _setorRepository.GetByIdEager(Convert.ToInt32(idModel));
            
            //epiObj.SetorNome = setorDomain.Nome;

            if (epiObj.NomeEpi != "Selecione")
            {
                var existe = false;
                foreach (var item in setorDomain.Epis)
                {
                    existe = (item.EpiId == epiObj.EpiId);
                    if (existe) break;
                }
                if (!existe)
                {
                    var epiDomain = _epiRepository.GetById(epiObj.EpiId);
                    epiObj.TipoEpiId = epiDomain.TipoEpiId;
                    setorDomain.Epis.Add(epiObj);
                    _setorRepository.Update(setorDomain);
                    setorDomain = _setorRepository.GetByIdEager(setorDomain.Id);                    
                    return PartialView("_Epis", setorDomain);
                }
                Warning(String.Format("Este epi já foi adicionado"), true);
                ViewBag.partialPermission = true;
            }

            return PartialView("_Epis", setorDomain);
        }

        public ActionResult RemoverEpi(int id)
        {
            var epi = _epiSetorRepository.GetById(id);
            var setor = _setorRepository.GetByIdEager(epi.SetorId);            

            if (_epiColaboradorRepository.EstaEmColaborador(epi.Id))
            {
                Warning("Epi não pode ser removido pois está entregue a um colaborador.",true);
                ViewBag.partialPermission = true;
            }
            else
            {

                if (_epiSetorRepository.EpiSetorEntregue(epi.Id))
                {
                    ViewBag.EpiSetorEntregue = true;
                    return PartialView("_Epis", setor);
                }

                foreach (var item in setor.Epis)
                {
                    if (item.Id.Equals(id))
                    {
                        setor.Epis.Remove(item);
                        break;
                    }
                }
                _setorRepository.Update(setor);
            }
            return PartialView("_Epis", setor);
        }

        public bool ExisteSetor(string nome)
        {
            return (_setorRepository.BuscarPorNomeUnico(nome) != null);
        }

        public ActionResult AdicionarUniforme(string idModel, string uniforme)
        {
            var uniformeObj = JsonConvert.DeserializeObject<UniformeSetorModel>(uniforme);
            var setorDomain = _setorRepository.GetByIdEager(Convert.ToInt32(idModel));

            if (uniformeObj.NomeUniforme != "Selecione")
            {
                var existe = false;
                foreach (var item in setorDomain.Uniformes)
                {
                    existe = (item.UniformeId == uniformeObj.UniformeId);
                    if (existe) break;
                }
                if (!existe)
                {
                    var uniformeDomain = _uniformeRepository.GetById(uniformeObj.UniformeId);
                    uniformeObj.TipoUniformeId = uniformeDomain.TipoUniformeId;
                    setorDomain.Uniformes.Add(uniformeObj);
                    _setorRepository.Update(setorDomain);
                    setorDomain = _setorRepository.GetByIdEager(setorDomain.Id);
                    return PartialView("_Uniformes", setorDomain);
                }
                Warning(String.Format("Este uniforme já foi adicionado"), true);
                ViewBag.partialPermission = true;
            }

            return PartialView("_Uniformes", setorDomain);
        }

        public ActionResult RemoverUniforme(int id)
        {
            var uniforme = _uniformeSetorRepository.GetById(id);
            var setor = _setorRepository.GetByIdEager(uniforme.SetorId);

            if (_uniformeColaboradorRepository.EstaEmColaborador(uniforme.Id))
            {
                Warning("Uniforme não pode ser removido pois está entregue a um colaborador.", true);
                ViewBag.partialPermission = true;
            }
            else
            {

                if (_uniformeSetorRepository.UniformeSetorEntregue(uniforme.Id))
                {
                    ViewBag.UniformeSetorEntregue = true;
                    return PartialView("_Uniformes", setor);
                }

                foreach (var item in setor.Uniformes)
                {
                    if (item.Id.Equals(id))
                    {
                        setor.Uniformes.Remove(item);
                        break;
                    }
                }
                _setorRepository.Update(setor);
            }
            return PartialView("_Uniformes", setor);
        }
    }
}
