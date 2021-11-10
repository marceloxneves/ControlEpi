using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TitansMVC.Properties;

namespace TitansMVC.Models
{
    public class EpiColaboradorModel
    {
        private bool _baixado = false;
        private bool _assinaturaPendente = false;
        private string _nomeEpi;
        public string _nomeSetor;

        [Key]
        public int Id { get; set; }
        public int? IdEmpresa { get; set; }
        public virtual EmpresaModel Empresa { get; set; }
        [DisplayName(@"Tipo EPI")]
        public int? TipoEpiId { get; set; }
        public TipoEpiModel TipoEpi { get; set; }
        [DisplayName(@"Epi do Setor")]
        public int EpiSetorId { get; set; }
        //public EpiSetorModel EpiSetor { get; set; }
        [DisplayName(@"Colaborador")]
        public int ColaboradorId { get; set; }
        public ColaboradorModel Colaborador { get; set; }
        //[StringLength(200, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_200")]
        [MaxLength(200, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_200")]
        [DisplayName(@"EPI")]
        public string NomeEpi { 
            get { return _nomeEpi; } 
            set { _nomeEpi = value != null ? value.ToUpper() : null; } 
        }

        [DisplayName(@"Nome Setor")]
        public string NomeSetor
        {
            get { return _nomeSetor; }
            set { _nomeSetor = value != null ? value.ToUpper() : null; }
        }
        [DisplayName(@"Centro de Custo")]
        public int? CentroCustoId { get; set; }
        public CentroCustoModel CentroCusto { get; set; }
        [DisplayName(@"Durabilidade em Dias")]
        public int? ValidadeEmDias { get; set; }        
        //[DataType(DataType.Date)]
        [DisplayName(@"Data da Entrega")]
        public DateTime DataEntrega { get; set; }
        [DisplayName(@"Quantidade")]
        public int? Quantidade { get; set; }
        [DisplayName(@"Assinatura Colaborador")]
        public byte[] AssinaturaColaborador { get; set; }
        [DataType(DataType.Date)]
        [DisplayName(@"Fim da Durabilidade da Epi")]
        public DateTime? DataVencimento { get; set; }
        [DisplayName(@"Dt. Hr. Baixa")]
        public DateTime? DataHoraBaixa { get; set; }
        [DisplayName(@"Justificativa")]
        public string Justificativa { get; set; }
        public bool TipoValidacao { get; set; }
        public string JustificativaEpiOutroSetor { get; set; }

        [DisplayName(@"Baixado?")]
        public bool? Baixado
        {
            get { return _baixado; }
            set { _baixado = value ?? false; }
        }
        public bool? AssinaturaPendente
        {
            get { return _assinaturaPendente; }
            set { _assinaturaPendente = value ?? false; }
        }

       
        [DisplayName(@"Nome Fantasia")] //Label Unidade de Negocio 20/03/2017        
        public int? UnidadeNegocioId { get; set; }
        //[DisplayName("Nome Fantasia")] //Label Unidade de Negocio 20/03/2017
        public virtual LbcModel UnidadeNogocio { get; set; }
    }
}