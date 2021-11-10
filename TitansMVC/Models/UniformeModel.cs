using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TitansMVC.Properties;

namespace TitansMVC.Models
{
    public class UniformeModel
    {
        private string _nome;
        private string _marca;
        private string _codUniformeFabricante;
        private string _codUniformeCliente;
        private string _codUniformeTitans;

        [Key]
        public int Id { get; set; }

        [DisplayName(@"Empresa")]
        public int? IdEmpresa { get; set; }
        public virtual EmpresaModel Empresa { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "tipo_uniforme_requirido")]
        [DisplayName(@"Tipo Uniforme")]
        public int TipoUniformeId { get; set; }
        public virtual TipoUniformeModel TipoUniforme { get; set; }

        [Required(ErrorMessageResourceType = typeof (Resources), ErrorMessage = null, ErrorMessageResourceName = "nome_requirido")]        
        [MaxLength(200, ErrorMessageResourceType = typeof (Resources), ErrorMessage = null, ErrorMessageResourceName = "max_200")]
        [DisplayName(@"Nome")]
        public string Nome
        {
            get { return _nome; }
            set { _nome = value != null ? value.ToUpper() : null; }
        }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "marca_requirido")]
        [MaxLength(200, ErrorMessageResourceType = typeof (Resources), ErrorMessage = null, ErrorMessageResourceName = "max_200")]
        [DisplayName(@"Marca")]
        public string Marca
        {
            get { return _marca; }
            set { _marca = value != null ? value.ToUpper() : null; }
        }

        [MaxLength(50, ErrorMessageResourceType = typeof (Resources), ErrorMessage = null, ErrorMessageResourceName = "max_50")]
        [DisplayName(@"Cód. Uniforme Fabricante")]
        public string CodUniformeFabricante
        {
            get { return _codUniformeFabricante;  }
            set { _codUniformeFabricante = value != null ? value.ToUpper() : null; }
        }

        [MaxLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_50")]
        [DisplayName(@"Cód. Uniforme Cliente")]
        public string CodUniformeCliente
        {
            get { return _codUniformeCliente;  }
            set { _codUniformeCliente = value != null ? value.ToUpper() : null; }
        }

        [MaxLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_50")]
        [DisplayName(@"Cód. Uniforme Fornecedor")]
        public string CodUniformeTitans
        {
            get { return _codUniformeTitans; } 
            set { _codUniformeTitans = value != null ? value.ToUpper() : null; } 
        }

        [DataType(DataType.Currency)]
        [DisplayName(@"Custo")]
        public decimal? Custo { get; set; }

        [DisplayName(@"Ativo?")]
        public bool Ativo { get; set; }

        [DataType(DataType.MultilineText)]
        [MaxLength(500, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_500")]
        [DisplayName(@"Observações")]
        public string Obs { get; set; }

        [DisplayName(@"Foto")]
        public byte[] Foto { get; set; }

        [ScaffoldColumn(false)]
        public DateTime? DataCad { get; set; }

        [ScaffoldColumn(false)]
        public bool? Importado { get; set; }

        public virtual ICollection<EstoqueUniforme> Estoques { get; set; }
        //public EstoqueUniforme Estoques { get; set; }
        public virtual LbcModel UnidadeNegocio { get; set; }
        //[DisplayName(@"Unidade de Negócio")]
        [DisplayName(@"Nome Fantasia")] //Label Unidade de Negocio 20/03/2017
        public int? UnidadeNegocioId { get; set; }
        [DisplayName(@"Código de Barras")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "O Campo Código de Barras deve conter apenas números!")]
        public string CodigoDeBarras { get; set; }
        public UniformeModel()
        {
            Estoques = new List <EstoqueUniforme>();
        }


        public class EstoqueAlterado
        {
            public int IdEst { get; set; }
            public int IdUniforme { get; set; }
            public decimal Qtde { get; set; }
            public decimal QtdeMin { get; set; }
            public decimal QtdeMax { get; set; }
        }
    }
}