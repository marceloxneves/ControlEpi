using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using TitansMVC.Properties;

namespace TitansMVC.Models
{
    public class EpiModel
    {
        private string _nome;
        private string _marca;
        private string _ca;
        private string _codEpiFabricante;
        private string _codEpiCliente;
        private string _codEpiTitans;

        [Key]
        public int Id { get; set; }

        [DisplayName(@"Empresa")]
        public int? IdEmpresa { get; set; }
        public virtual EmpresaModel Empresa { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "tipo_epi_requirido")]
        [DisplayName(@"Tipo EPI")]
        public int TipoEpiId { get; set; }
        public virtual TipoEpiModel TipoEpi { get; set; }

        [Required(ErrorMessageResourceType = typeof (Resources), ErrorMessage = null, ErrorMessageResourceName = "nome_requirido")]
        //[StringLength(200, ErrorMessageResourceType = typeof (Resources), ErrorMessage = null, ErrorMessageResourceName = "max_200")]
        [MaxLength(200, ErrorMessageResourceType = typeof (Resources), ErrorMessage = null, ErrorMessageResourceName = "max_200")]
        [DisplayName(@"Nome")]
        public string Nome
        {
            get { return _nome; }
            set { _nome = value != null ? value.ToUpper() : null; }
        }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "marca_requirido")]
        //[StringLength(200, ErrorMessageResourceType = typeof (Resources), ErrorMessage = null, ErrorMessageResourceName = "max_200")]
        [MaxLength(200, ErrorMessageResourceType = typeof (Resources), ErrorMessage = null, ErrorMessageResourceName = "max_200")]
        [DisplayName(@"Marca")]
        public string Marca
        {
            get { return _marca; }
            set { _marca = value != null ? value.ToUpper() : null; }
        }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "ca_requirido")]
        //[StringLength(200, ErrorMessageResourceType = typeof (Resources), ErrorMessage = null, ErrorMessageResourceName = "max_200")]
        [MaxLength(200, ErrorMessageResourceType = typeof (Resources), ErrorMessage = null, ErrorMessageResourceName = "max_200")]
        [DisplayName("CA")]
        public string Ca
        {
            get { return _ca; }
            set { _ca = value != null ? value.ToUpper() : null; }
        }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "validade_ca_requirido")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        //123456*[DataType(DataType.Date, ErrorMessage = @"Data em formato inválido")]
        [DisplayName(@"Validade CA")]
        public DateTime? ValidadeCa { get; set; }

        //[StringLength(50, ErrorMessageResourceType = typeof (Resources), ErrorMessage = null, ErrorMessageResourceName = "max_50")]
        [MaxLength(50, ErrorMessageResourceType = typeof (Resources), ErrorMessage = null, ErrorMessageResourceName = "max_50")]
        [DisplayName(@"Cód. EPI Fabricante")]
        public string CodEpiFabricante
        {
            get { return _codEpiFabricante;  }
            set { _codEpiFabricante = value != null ? value.ToUpper() : null; }
        }

        //[StringLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_50")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_50")]
        [DisplayName(@"Cód. EPI Cliente")]
        public string CodEpiCliente {
            get { return _codEpiCliente;  }
            set { _codEpiCliente = value != null ? value.ToUpper() : null; }
        }

        //[StringLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_50")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_50")]
        [DisplayName(@"Cód. EPI Fornecedor")]
        public string CodEpiTitans {
            get { return _codEpiTitans; } 
            set { _codEpiTitans = value != null ? value.ToUpper() : null; } 
        }

        //[Required(ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "familia_epi_requirido")]
        //familia possivelmente tirar
        [DisplayName(@"Família do EPI")]
        public int? FamiliaId { get; set; }
        public virtual FamiliaEpiModel Familia { get; set; }

        [DataType(DataType.Currency)]
        [DisplayName(@"Custo")]
        public decimal? Custo { get; set; }

        [DisplayName(@"Ativo?")]
        public bool Ativo { get; set; }

        [DataType(DataType.MultilineText)]
        //[StringLength(500, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_500")]
        [MaxLength(500, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_500")]
        [DisplayName(@"Observações")]
        public string Obs { get; set; }

        [DisplayName(@"Foto")]
        public byte[] Foto { get; set; }

        [ScaffoldColumn(false)]
        public DateTime? DataCad { get; set; }

        [ScaffoldColumn(false)]
        public bool? Importado { get; set; }


        public virtual ICollection<EstoqueEpi> Estoques { get; set; }
        //public EstoqueEpi Estoques { get; set; }
        public virtual LbcModel UnidadeNegocio { get; set; }
        //[DisplayName(@"Unidade de Negócio")]
        [DisplayName(@"Nome Fantasia")] //Label Unidade de Negocio 20/03/2017
        public int? UnidadeNegocioId { get; set; }
        [DisplayName(@"Código de Barras")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "O Campo Código de Barras deve conter apenas números!")]
        public string CodigoDeBarras { get; set; }
        public EpiModel()
        {
            Estoques = new List <EstoqueEpi>();
        }


        public class EstoqueAlterado
        {
            public int IdEst { get; set; }
            public int IdEpi { get; set; }
            public decimal Qtde { get; set; }
            public decimal QtdeMin { get; set; }
            public decimal QtdeMax { get; set; }
        }
    }
}