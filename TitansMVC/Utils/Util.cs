using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using TitansMVC.Models;
using TitansMVC.Models.Enums;
using TitansMVC.Models.Relatorios;
using TitansMVC.Repository.Implementations;
using TitansMVC.Repository.Interfaces;

namespace TitansMVC.Utils
{
    public class Util
    {
        private static readonly IUsuarioRepository _usuarioRepository = new UsuarioRepository();
        private static readonly IEmpresaRepository _empresaRepository  = new EmpresaRepository();
        private static readonly IPlanoRepository _planoRepository = new PlanoRepository();
        private static readonly IColaboradorRepository _colaboradorRepository = new ColaboradorRepository();

        public static string GetDescricaoEstadoEpi(EstadoEpisConsulta estado)
        {
            switch (estado)
            {
                case EstadoEpisConsulta.Entregues: return "EmUso";

                case EstadoEpisConsulta.Baixados:
                    return "Baixados";

                default: return "Todos";
            }
        }

        public static int GetEmpresaId()
        {
            //var empresa = _empresaRepository.GetById(GetUsuario().IdEmpresa ?? 0);

            //return empresa != null ? empresa.Id : 0;
            return GetUsuario().IdEmpresa ?? 0;
        }

        public static string GetEmpresaCnpj()
        {

            var empresa = _empresaRepository.GetById(GetUsuario().IdEmpresa ?? 0);

            return empresa != null ? empresa.Cnpj : String.Empty;
        }

        public static EmpresaModel GetEmpresa()
        {
            
            var empresa = _empresaRepository.GetById(GetUsuario().IdEmpresa ?? 0);

            empresa = _empresaRepository.Atualizar(empresa);

            return empresa;
        }

        public static UsuarioModel GetUsuario()
        {
            var usuario = _usuarioRepository.GetById(HttpContext.Current.User.Identity.GetUserId());
            return usuario;
        }

        public static string GetEmpresaPlano(string cnpj)
        {
            var plano = _planoRepository.GetByCnpj(cnpj);
            return plano.NivelPlano.ToString();
        }

        public static DateTime GetEmpresaPlanoValidade(string cnpj)
        {
            if (HttpContext.Current.User.IsInRole("role_master"))
            {
                return DateTime.MaxValue;
            }
            var plano = _planoRepository.GetByCnpj(GetEmpresaCnpj());
            plano.Validade = DateTime.Parse(Encryptor.Decrypt(plano.ValidadeCriptografada));
            return plano.Validade;
        }

        public static Boolean LimiteAlcancado(int numeroColaboradores)
        {
            var plano = GetEmpresaPlano(GetEmpresaCnpj());
            bool limiteAlcanado;
            switch (plano)
            {
                case "Starter":
                    limiteAlcanado = numeroColaboradores < 100;
                break;
                case "Basic":
                    limiteAlcanado = numeroColaboradores < 300;
                break;
                case"Standard":
                    limiteAlcanado = numeroColaboradores < 500;
                break;
                case "Master":
                    limiteAlcanado = numeroColaboradores < 1000;
                break;
                case "Ultimate":
                    limiteAlcanado = true;
                break;
                default:
                    limiteAlcanado = false;
                break;
            }

            return limiteAlcanado;
        }

        public static int GetNumeroColaboradoresRestante(int id, string plano)
        {
            int numeroDeColaboradoresRestante = 0;
            int numeroColaboradores = _colaboradorRepository.ContarColaboradores(id);

            switch (plano)
            {
                case "Starter":
                    numeroDeColaboradoresRestante = 100 - numeroColaboradores;
                    break;
                case "Basic":
                    numeroDeColaboradoresRestante = 300 - numeroColaboradores;
                    break;
                case "Standard":
                    numeroDeColaboradoresRestante = 500 - numeroColaboradores;
                    break;
                case "Master":
                    numeroDeColaboradoresRestante = 1000 - numeroColaboradores;
                    break;
                case "Ultimate":
                    numeroDeColaboradoresRestante = -1;
                    break;
            }
            return numeroDeColaboradoresRestante;
        }
    }
}