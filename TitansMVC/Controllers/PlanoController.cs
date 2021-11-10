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
    [Authorize(Roles = "role_master")]
    public class PlanoController : BaseController
    {
        private readonly IPlanoRepository _planoRepository;

        public PlanoController()
        {
            _planoRepository = new PlanoRepository();
        }

        // GET: Plano
        //public ActionResult Index()
        //{
        //    var planos = _planoRepository.BuscarTodosPlanos();
        //    foreach (var p in planos)
        //    {
        //        p.Cnpj = Encryptor.Decrypt(p.CnpjCriptografado);
        //        p.Validade = DateTime.Parse(Encryptor.Decrypt(p.ValidadeCriptografada)).Date;
        //        p.DataUltimaValidacao = Encryptor.Decrypt(p.DataUltimaValidacao);
        //    }
        //    return View(planos.ToPagedList(1, 10000));                
        //}

        public ActionResult Index(string searchBy,string search, int? page)
        {

            IEnumerable<PlanoModel> planos = null;
            switch (searchBy)
            {
                
                case "cnpj":
                    planos = _planoRepository.BuscarPorCnpj(search).ToPagedList(page.HasValue ? page.Value : 1,10);
                    foreach (var p in planos)
                    {
                        p.Cnpj = Encryptor.Decrypt(p.CnpjCriptografado);
                        p.Validade = DateTime.Parse(Encryptor.Decrypt(p.ValidadeCriptografada)).Date;
                        p.DataUltimaValidacao = Encryptor.Decrypt(p.DataUltimaValidacao);
                    }
                    break;
                default:
                    planos = _planoRepository.BuscarTodosPlanos().ToPagedList(page.HasValue ? page.Value : 1, 10);
                    foreach (var p in planos)
                    {
                        p.Cnpj = Encryptor.Decrypt(p.CnpjCriptografado);
                        p.Validade = DateTime.Parse(Encryptor.Decrypt(p.ValidadeCriptografada)).Date;
                        p.DataUltimaValidacao = Encryptor.Decrypt(p.DataUltimaValidacao);
                    }
                    break;
            }           
            return View(planos);
        }

        // GET: Plano/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: Plano/Create
        [HttpPost]
        public ActionResult Create(PlanoModel plano)
        {
            if (ModelState.IsValid)
            {
                _planoRepository.Add(plano);

                Success(String.Format("Registro gravado com sucesso!"), true);

                return RedirectToAction("Edit", plano);
            }

            return View();
        }

        // GET: Plano/Edit/5
        public ActionResult Edit(int id)
        {
            var teste = Encryptor.Encrypt("321321321321");
            var plano = _planoRepository.GetById(id);
            plano.Cnpj = Encryptor.Decrypt(plano.CnpjCriptografado);
            plano.Validade = DateTime.Parse(Encryptor.Decrypt(plano.ValidadeCriptografada)).Date;
            
            return View(plano);
        }

        // POST: Plano/Edit/5
        [HttpPost]
        public ActionResult Edit(PlanoModel plano)
        {
            if (ModelState.IsValid)
            {
                _planoRepository.Update(plano);
                _planoRepository.UpdatePlanoServidor(plano);
                Success(String.Format("Registro alterado com sucesso!"), true);
            }

            return View(plano);
        }
    }
}
