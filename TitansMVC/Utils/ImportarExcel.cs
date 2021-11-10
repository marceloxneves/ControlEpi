using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using TitansMVC.Controllers;
using TitansMVC.Models;
using TitansMVC.Models.Enums;
using TitansMVC.Repository.Implementations;
using TitansMVC.Repository.Interfaces;

namespace TitansMVC.Utils
{
    public class ImportarExcel
    {
        private static IColaboradorRepository _colaboradorRepo;
        private static IColaboradorImportadoRepository _colaboradorImportadoRepo;
        private static ISetorRepository _setorRepo;
        private static IEpiRepository _epiRepo;
        private static readonly ITipoEpiRepository _tipoEpiRepo = new TipoEpiRepository();
        private static ICentroCustoRepository _centoCustoRepository;
        private static readonly ILbcRepository _lbcRepository = new LbcRepository();
        private static readonly IMunicipioRepository _municipioRepository = new MunicipioRepository();
        private static readonly IFuncaoRepository _funcaoRepo = new FuncaoRepository();
        private static IUniformeRepository _uniformeRepo;
        private static readonly ITipoUniformeRepository _tipoUniformeRepo = new TipoUniformeRepository();

        public static bool ImportarUnidadesNegocio(DataTable dt)
        {
            var unidades = new List<LbcModel>();
            try
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        var unidade = new LbcModel() { Ativo = true, IdEmpresa = Util.GetEmpresaId() };

                        foreach (DataColumn col in dt.Columns)
                        {
                            switch (col.ColumnName)
                            {
                                case "razao_social":
                                    unidade.Nome = string.IsNullOrWhiteSpace(row[col.ColumnName].ToString()) ? null : row[col.ColumnName].ToString();
                                    break;
                                case "fantasia":
                                    unidade.Fantasia = string.IsNullOrWhiteSpace(row[col.ColumnName].ToString()) ? null : row[col.ColumnName].ToString();
                                    break;
                                case "cnpj":
                                    unidade.Cnpj = string.IsNullOrWhiteSpace(row[col.ColumnName].ToString()) ? null : row[col.ColumnName].ToString();
                                    break;
                                case "insc_estadual":
                                    unidade.InscrEst = string.IsNullOrWhiteSpace(row[col.ColumnName].ToString()) ? null : row[col.ColumnName].ToString();
                                    break;
                                case "insc_municipal":
                                    unidade.InscrMun = string.IsNullOrWhiteSpace(row[col.ColumnName].ToString()) ? null : row[col.ColumnName].ToString();
                                    break;
                                case "endereco":
                                    unidade.EndEndereco = string.IsNullOrWhiteSpace(row[col.ColumnName].ToString()) ? null : row[col.ColumnName].ToString();
                                    break;
                                case "numero":
                                    unidade.EndNumero = string.IsNullOrWhiteSpace(row[col.ColumnName].ToString()) ? null : row[col.ColumnName].ToString();
                                    break;
                                case "complemento":
                                    unidade.EndComplemento = string.IsNullOrWhiteSpace(row[col.ColumnName].ToString()) ? null : row[col.ColumnName].ToString();
                                    break;
                                case "bairro":
                                    unidade.EndBairro = string.IsNullOrWhiteSpace(row[col.ColumnName].ToString()) ? null : row[col.ColumnName].ToString();
                                    break;
                                case "cep":
                                    unidade.EndCep = string.IsNullOrWhiteSpace(row[col.ColumnName].ToString()) ? null : row[col.ColumnName].ToString();
                                    break;
                                case "uf":
                                    unidade.SiglaUf = string.IsNullOrWhiteSpace(row[col.ColumnName].ToString()) ? null : row[col.ColumnName].ToString();
                                    break;
                                case "municipio":
                                    var nomeMunicipio = string.IsNullOrWhiteSpace(row[col.ColumnName].ToString()) ? null : row[col.ColumnName].ToString();
                                    if (nomeMunicipio != null)
                                    {
                                        var municipio = _municipioRepository.BuscaPorUfENome(unidade.SiglaUf, nomeMunicipio);
                                        unidade.Municipio = municipio;
                                        unidade.MunicipioId = municipio != null ? municipio.Id : 0;
                                    }

                                    break;
                            }
                        }

                        unidades.Add(unidade);
                    }
                }

                foreach (var unidade in unidades)
                {
                    var existe = _lbcRepository.BuscarAtivos().Any(s => s.Cnpj == unidade.Cnpj);
                    if (!existe && !String.IsNullOrWhiteSpace(unidade.Cnpj))
                    {
                        _lbcRepository.Add(unidade);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw new Exception(ex.Message);
            }
        }


        public static bool ImportarSetor(DataTable dt)
        {
            var setores = new List<SetorModel>();
            try
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        var setor = new SetorModel() { Ativo = true, Importado = true, IdEmpresa = Util.GetEmpresaId() };

                        foreach (DataColumn col in dt.Columns)
                        {
                            switch (col.ColumnName)
                            {
                                case "nome":
                                    setor.Nome = string.IsNullOrWhiteSpace(row[col.ColumnName].ToString()) ? null : row[col.ColumnName].ToString();
                                    break;
                                case "obs":
                                    setor.Obs = string.IsNullOrWhiteSpace(row[col.ColumnName].ToString()) ? null : row[col.ColumnName].ToString();
                                    break;
                            }
                        }

                        setores.Add(setor);
                    }
                }

                _setorRepo = new SetorRepository();

                foreach (var setor in setores)
                {
                    var existe = _setorRepo.BuscarAtivos().Any(s => s.Nome == setor.Nome);
                    if (!existe)
                    {
                        _setorRepo.Add(setor);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw new Exception(ex.Message);
            }
        }

        public static bool ImportarColaborador(DataTable dt)
        {
            _colaboradorRepo = new ColaboradorRepository();
            var colaboradores = new List<ColaboradorModel>();
            try
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        var colaborador = new ColaboradorModel() { Ativo = true, Importado = true, IdEmpresa = Util.GetEmpresaId() };

                        foreach (DataColumn col in dt.Columns)
                        {
                            switch (col.ColumnName)
                            {
                                case "nome":
                                    colaborador.Nome = string.IsNullOrWhiteSpace(row[col.ColumnName].ToString()) ? null : row[col.ColumnName].ToString();
                                    break;
                                case "cpf":
                                    colaborador.Cpf = string.IsNullOrWhiteSpace(row[col.ColumnName].ToString()) ? null : row[col.ColumnName].ToString();
                                    break;
                                case "cod_colaborador":
                                    colaborador.NumRegEmpresa = string.IsNullOrWhiteSpace(row[col.ColumnName].ToString()) ? null : row[col.ColumnName].ToString();
                                    break;
                                case "genero":
                                    if (!string.IsNullOrWhiteSpace(row[col.ColumnName].ToString()))
                                    {
                                        int genero;
                                        int.TryParse(row[col.ColumnName].ToString(), out genero);
                                        colaborador.Genero = (Genero)genero;
                                    }
                                    break;
                                case "data_admissao":
                                    if (!string.IsNullOrWhiteSpace(row[col.ColumnName].ToString()))
                                    {
                                        DateTime dataAdmissao;
                                        DateTime.TryParse(row[col.ColumnName].ToString(), out dataAdmissao);
                                        colaborador.DataAdmissao = dataAdmissao;
                                    }
                                    break;
                                case "data_nascimento":
                                    if (!string.IsNullOrWhiteSpace(row[col.ColumnName].ToString()))
                                    {
                                        DateTime dataNasc;
                                        DateTime.TryParse(row[col.ColumnName].ToString(), out dataNasc);
                                        colaborador.DataNascimento = dataNasc;
                                    }
                                    break;
                                case "recebeu_treinamento":
                                    if (!string.IsNullOrWhiteSpace(row[col.ColumnName].ToString()))
                                    {
                                        bool recebeuTrein = row[col.ColumnName].ToString() == "1";
                                        colaborador.RecebeuTreinamento = recebeuTrein;
                                    }
                                    break;
                                case "recebeu_advertencia":
                                    if (!string.IsNullOrWhiteSpace(row[col.ColumnName].ToString()))
                                    {
                                        bool recebeuAdv = row[col.ColumnName].ToString() == "1";
                                        colaborador.RecebeuAdvertencia = recebeuAdv;
                                    }
                                    break;
                                case "motivo_advertencia":
                                    colaborador.MotivoAdvertencia = string.IsNullOrWhiteSpace(row[col.ColumnName].ToString()) ? null : row[col.ColumnName].ToString();
                                    break;
                                case "setor":
                                    colaborador.NomeSetor = string.IsNullOrWhiteSpace(row[col.ColumnName].ToString())
                                        ? null
                                        : row[col.ColumnName].ToString();

                                    var setorNome = row[col.ColumnName].ToString();

                                    _setorRepo = new SetorRepository();

                                    var setorExiste = _setorRepo.BuscarPorNome(setorNome, null).Any();
                                    if (!setorExiste)
                                    {
                                        var setor = new SetorModel()
                                        {
                                            DataCad = DateTime.Now,
                                            Nome = row[col.ColumnName].ToString(),
                                            IdEmpresa = Util.GetEmpresaId(),
                                            Importado = true,
                                            Ativo = true
                                        };

                                        _setorRepo.Add(setor);
                                        //colaborador.Setor = setor;
                                        colaborador.SetorId = setor.Id;
                                    }
                                    else
                                        colaborador.SetorId = _setorRepo.BuscarPorNome(setorNome, null).FirstOrDefault().Id;
                                    break;
                                case "obs":
                                    colaborador.Obs = string.IsNullOrWhiteSpace(row[col.ColumnName].ToString()) ? null : row[col.ColumnName].ToString();
                                    break;
                                case "funcao":
                                    var nomeFuncao = string.IsNullOrWhiteSpace(row[col.ColumnName].ToString())
                                        ? "Padrão"
                                        : row[col.ColumnName].ToString();
                                    var funcao = _funcaoRepo.BuscarPorNome(nomeFuncao).FirstOrDefault();
                                    if (funcao == null)
                                    {
                                        funcao = new FuncaoModel()
                                        {
                                            Ativo = true,
                                            IdEmpresa = Util.GetEmpresaId(),
                                            Nome = nomeFuncao,
                                            Obs = "Importado"
                                        };
                                        _funcaoRepo.Add(funcao);
                                    }
                                    //colaborador.Funcao = funcao;
                                    colaborador.FuncaoId = funcao.Id;
                                    break;

                                case "cnpj_unidade_negocio":
                                    var cnpj_unidade = string.IsNullOrWhiteSpace(row[col.ColumnName].ToString())
                                        ? null
                                        : row[col.ColumnName].ToString();
                                    var unidade = _lbcRepository.GetByCnpj(cnpj_unidade);

                                    //colaborador.Lbc = unidade;
                                    colaborador.LbcId = unidade != null ? unidade.Id : 0;
                                    break;
                            }
                        }
                        colaboradores.Add(colaborador);
                    }
                }

                var colRestante = Util.GetNumeroColaboradoresRestante(Util.GetEmpresaId(), Util.GetEmpresaPlano(Util.GetEmpresaCnpj()));

                if (colRestante != -1 && colRestante < colaboradores.Count)
                {
                    return false;
                }

                _colaboradorRepo = new ColaboradorRepository();

                foreach (var colab in colaboradores)
                {
                    var existe = _colaboradorRepo.BuscarAtivos().Any(c => c.Cpf == colab.Cpf);
                    if (!existe && !String.IsNullOrWhiteSpace(colab.Cpf))
                    {
                        _colaboradorRepo.Add(colab);
                    }
                    else
                    {
                        continue;
                        var colaboradorImport = new ColaboradorImportacaoDuplicadoModel()
                        {
                            Nome = colab.Nome,
                            Cpf = colab.Cpf,
                            NumRegEmpresa = colab.NumRegEmpresa,
                            Genero = colab.Genero,
                            DataAdmissao = colab.DataAdmissao,
                            DataNascimento = colab.DataNascimento,
                            RecebeuTreinamento = colab.RecebeuTreinamento,
                            RecebeuAdvertencia = colab.RecebeuAdvertencia,
                            MotivoAdvertencia = colab.MotivoAdvertencia,
                            Setor = colab.NomeSetor,
                            Obs = colab.Obs,
                            DataHora = DateTime.Now
                        };

                        _colaboradorImportadoRepo = new ColaboradorImportadoRepository();
                        _colaboradorImportadoRepo.Add(colaboradorImport);
                    }

                }

                return true;
            }
            catch (Exception ex)
            {                
                return false;
                throw new Exception(ex.Message);
            }

        }

        public static bool ImportarEpi(DataTable dt)
        {
            var epis = new List<EpiModel>();
            try
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        var epi = new EpiModel() { Ativo = true, Importado = true, IdEmpresa = Util.GetEmpresaId() };

                        foreach (DataColumn col in dt.Columns)
                        {
                            switch (col.ColumnName)
                            {
                                case "nome":
                                    epi.Nome = string.IsNullOrWhiteSpace(row[col.ColumnName].ToString()) ? null : row[col.ColumnName].ToString();
                                    break;
                                case "marca":
                                    epi.Marca = string.IsNullOrWhiteSpace(row[col.ColumnName].ToString()) ? null : row[col.ColumnName].ToString();
                                    break;
                                case "ca":
                                    epi.Ca = string.IsNullOrWhiteSpace(row[col.ColumnName].ToString()) ? null : row[col.ColumnName].ToString();
                                    break;
                                case "validade_ca":
                                    if (!string.IsNullOrWhiteSpace(row[col.ColumnName].ToString()))
                                    {
                                        DateTime dataValidade;
                                        DateTime.TryParse(row[col.ColumnName].ToString(), out dataValidade);
                                        epi.ValidadeCa = dataValidade;
                                    }
                                    break;
                                case "cod_epi_fabricante":
                                    epi.CodEpiFabricante = string.IsNullOrWhiteSpace(row[col.ColumnName].ToString()) ? null : row[col.ColumnName].ToString();
                                    break;
                                case "cod_epi":
                                    epi.CodEpiCliente = string.IsNullOrWhiteSpace(row[col.ColumnName].ToString()) ? null : row[col.ColumnName].ToString();
                                    break;
                                case "custo":
                                    if (!string.IsNullOrWhiteSpace(row[col.ColumnName].ToString()))
                                    {
                                        decimal valor;
                                        decimal.TryParse(row[col.ColumnName].ToString(), out valor);
                                        epi.Custo = valor;
                                    }
                                    break;
                                case "obs":
                                    epi.Obs = string.IsNullOrWhiteSpace(row[col.ColumnName].ToString()) ? null : row[col.ColumnName].ToString();
                                    break;
                                case "codigo_barras":
                                    epi.CodigoDeBarras = string.IsNullOrWhiteSpace(row[col.ColumnName].ToString()) ? null : row[col.ColumnName].ToString();
                                    break;
                                case "tipo_epi":
                                    var nomeTipo = string.IsNullOrWhiteSpace(row[col.ColumnName].ToString()) ? null : row[col.ColumnName].ToString();
                                    var tipo = _tipoEpiRepo.BuscarPorNome(nomeTipo).FirstOrDefault();
                                    var existe = tipo != null;
                                    if (!existe && !string.IsNullOrWhiteSpace(nomeTipo))
                                    {
                                        tipo = _tipoEpiRepo.AddWRet(
                                            new TipoEpiModel()
                                            {
                                                Ativo = true,
                                                IdEmpresa = Util.GetEmpresaId(),
                                                Nome = nomeTipo,
                                                Obs = "Importado automaticamente"
                                            }
                                        );
                                    }
                                    epi.TipoEpiId = tipo.Id;
                                    break;
                            }
                        }

                        epis.Add(epi);
                    }
                }

                _epiRepo = new EpiRepository();

                foreach (var epi in epis)
                {
                    //alterado a pedido do Raphael Baptista em 02/03/2017 - pode importar o mesmo ca. se tiver o mesmo nome nao importa novamente

                    //var existe = _epiRepo.GetByCa(epi.Ca) != null;
                    var existe = _epiRepo.GetByNomeAndMarca(epi.Nome, epi.Marca) != null;
                    if (!existe && epi.Nome != null)
                        _epiRepo.Add(epi);
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw new Exception(ex.Message);
            }

        }

        public static bool ImportarCentroCusto(DataTable dt)
        {
            var centrosCusto = new List<CentroCustoModel>();
            try
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        var centroCusto = new CentroCustoModel { Ativo = true, IdEmpresa = Util.GetEmpresaId(), DataCad = DateTime.Now, };

                        foreach (DataColumn col in dt.Columns)
                        {
                            switch (col.ColumnName)
                            {
                                case "nome":
                                    centroCusto.Nome = string.IsNullOrWhiteSpace(row[col.ColumnName].ToString()) ? null : row[col.ColumnName].ToString();
                                    break;
                                case "unidade_de_negocio":
                                    var NomeUnidadeNegocio = string.IsNullOrWhiteSpace(row[col.ColumnName].ToString())
                                        ? null
                                        : row[col.ColumnName].ToString();

                                    if (NomeUnidadeNegocio != null && !_lbcRepository.BuscarPorNome(NomeUnidadeNegocio).Any())
                                    {
                                        var lbc = new LbcModel()
                                        {
                                            Nome = NomeUnidadeNegocio,
                                            Ativo = true,
                                            DataCad = DateTime.Now,
                                            IdEmpresa = Util.GetEmpresaId()
                                        };

                                        _lbcRepository.Add(lbc);
                                    }

                                    break;
                                case "obs":
                                    centroCusto.Obs = string.IsNullOrWhiteSpace(row[col.ColumnName].ToString()) ? null : row[col.ColumnName].ToString();
                                    break;
                            }
                        }

                        centrosCusto.Add(centroCusto);
                    }
                }

                _centoCustoRepository = new CentroCustoRepository();

                foreach (var centroCusto in centrosCusto)
                {
                    _centoCustoRepository.Add(centroCusto);
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw new Exception(ex.Message);
            }

        }

        public static void ImportarGenerico(DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    foreach (DataColumn col in dt.Columns)
                    {
                        object value = row[col.ColumnName];
                        //var colaborador = new ColaboradorModel()
                        //{
                        //    Nome = row[col.ColumnName].ToString(),
                        //    Ativo = true
                        //};
                    }
                }
            }
        }

        public static bool ImportarUniforme(DataTable dt)
        {
            var uniformes = new List<UniformeModel>();
            try
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        var uniforme = new UniformeModel() { Ativo = true, Importado = true, IdEmpresa = Util.GetEmpresaId() };

                        foreach (DataColumn col in dt.Columns)
                        {
                            switch (col.ColumnName)
                            {
                                case "nome":
                                    uniforme.Nome = string.IsNullOrWhiteSpace(row[col.ColumnName].ToString()) ? null : row[col.ColumnName].ToString();
                                    break;
                                case "marca":
                                    uniforme.Marca = string.IsNullOrWhiteSpace(row[col.ColumnName].ToString()) ? null : row[col.ColumnName].ToString();
                                    break;
                                case "cod_fabricante":
                                    uniforme.CodUniformeFabricante = string.IsNullOrWhiteSpace(row[col.ColumnName].ToString()) ? null : row[col.ColumnName].ToString();
                                    break;
                                case "cod_cliente":
                                    uniforme.CodUniformeCliente = string.IsNullOrWhiteSpace(row[col.ColumnName].ToString()) ? null : row[col.ColumnName].ToString();
                                    break;
                                case "custo":
                                    if (!string.IsNullOrWhiteSpace(row[col.ColumnName].ToString()))
                                    {
                                        decimal valor;
                                        decimal.TryParse(row[col.ColumnName].ToString(), out valor);
                                        uniforme.Custo = valor;
                                    }
                                    break;
                                case "obs":
                                    uniforme.Obs = string.IsNullOrWhiteSpace(row[col.ColumnName].ToString()) ? null : row[col.ColumnName].ToString();
                                    break;
                                case "codigo_barras":
                                    uniforme.CodigoDeBarras = string.IsNullOrWhiteSpace(row[col.ColumnName].ToString()) ? null : row[col.ColumnName].ToString();
                                    break;
                                case "tipo_uniforme":
                                    var nomeTipo = string.IsNullOrWhiteSpace(row[col.ColumnName].ToString()) ? null : row[col.ColumnName].ToString();
                                    var tipo = _tipoUniformeRepo.BuscarPorNome(nomeTipo).FirstOrDefault();
                                    var existe = tipo != null;
                                    if (!existe && !string.IsNullOrWhiteSpace(nomeTipo))
                                    {
                                        tipo = _tipoUniformeRepo.AddWRet(
                                            new TipoUniformeModel()
                                            {
                                                Ativo = true,
                                                IdEmpresa = Util.GetEmpresaId(),
                                                Nome = nomeTipo,
                                                Obs = "Importado automaticamente"
                                            }
                                        );
                                    }
                                    uniforme.TipoUniformeId = tipo.Id;
                                    break;
                            }
                        }

                        uniformes.Add(uniforme);
                    }
                }

                _uniformeRepo = new UniformeRepository();

                foreach (var uniforme in uniformes)
                {
                    var existe = _uniformeRepo.GetByNomeAndMarca(uniforme.Nome, uniforme.Marca) != null;
                    if (!existe && uniforme.Nome != null)
                        _uniformeRepo.Add(uniforme);
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw new Exception(ex.Message);
            }

        }
    }
}