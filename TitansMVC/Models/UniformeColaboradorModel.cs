using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TitansMVC.Properties;

namespace TitansMVC.Models
{
    public class UniformeColaboradorModel
    {
        private bool _baixado = false;
        private bool _assinaturaPendente = false;
        private string _nomeUniforme;
        public string _nomeSetor;

        [Key]
        public int Id { get; set; }
        public int? IdEmpresa { get; set; }
        public virtual EmpresaModel Empresa { get; set; }
        [DisplayName(@"Tipo Uniforme")]
        public int? TipoUniformeId { get; set; }
        public TipoUniformeModel TipoUniforme { get; set; }
        [DisplayName(@"Uniforme do Setor")]
        public int UniformeSetorId { get; set; }
        [DisplayName(@"Colaborador")]
        public int ColaboradorId { get; set; }
        public ColaboradorModel Colaborador { get; set; }

        [MaxLength(200, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_200")]
        [DisplayName(@"Uniforme")]
        public string NomeUniforme
        { 
            get { return _nomeUniforme; } 
            set { _nomeUniforme = value != null ? value.ToUpper() : null; } 
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
        
        [DisplayName(@"Data da Entrega")]
        public DateTime DataEntrega { get; set; }
        [DisplayName(@"Quantidade")]
        public int? Quantidade { get; set; }
        [DisplayName(@"Assinatura Colaborador")]
        public byte[] AssinaturaColaborador { get; set; }
        [DataType(DataType.Date)]
        [DisplayName(@"Fim da Durabilidade do Uniforme")]
        public DateTime? DataVencimento { get; set; }
        [DisplayName(@"Dt. Hr. Baixa")]
        public DateTime? DataHoraBaixa { get; set; }
        [DisplayName(@"Justificativa")]
        public string Justificativa { get; set; }
        public bool TipoValidacao { get; set; }
        public string JustificativaUniformeOutroSetor { get; set; }

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