using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using TitansMVC.Models.Enums;
using TitansMVC.Properties;

namespace TitansMVC.Models
{
    public class ColaboradorModel
    {
        private string _nome;
        private string _numRegEmpresa;
        private string _nomeSetor;

        [Key]
        public int Id { get; set; }

        [DisplayName(@"Empresa")]
        public int? IdEmpresa { get; set; }
        public virtual EmpresaModel Empresa { get; set; }

        [Required(ErrorMessageResourceType = typeof (Resources), ErrorMessage = null, ErrorMessageResourceName = "nome_requirido")]
        [StringLength(200, ErrorMessageResourceType = typeof (Resources), ErrorMessage = null, ErrorMessageResourceName = "max_200")]
        [MaxLength(200, ErrorMessageResourceType = typeof (Resources), ErrorMessage = null, ErrorMessageResourceName = "max_200")]
        [DisplayName(@"Nome")]
        public string Nome
        {
            get { return _nome; } 
            set { _nome = value != null ? value.ToUpper() : null; }
        }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "cpf_requirido")]
        [StringLength(20, ErrorMessageResourceType = typeof (Resources), ErrorMessage = null, ErrorMessageResourceName = "max_20")]
        [MaxLength(20, ErrorMessageResourceType = typeof (Resources), ErrorMessage = null, ErrorMessageResourceName = "max_20")]
        [DisplayName(@"CPF")]
        public string Cpf { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "setor_requirido")]
        [DisplayName(@"Setor")]
        public int? SetorId { get; set; }
        public virtual SetorModel Setor { get; set; }

        [StringLength(50, ErrorMessageResourceType = typeof (Resources), ErrorMessage = null, ErrorMessageResourceName = "max_50")]
        [MaxLength(50, ErrorMessageResourceType = typeof (Resources), ErrorMessage = null, ErrorMessageResourceName = "max_50")]
        [DisplayName(@"Núm. Reg. na Empresa")]
        public string NumRegEmpresa
        {
            get { return _numRegEmpresa; }
            set { _numRegEmpresa = value != null ? value.ToUpper() : null; }
        }

        [DisplayName(@"Gênero")]
        public Genero? Genero { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "data_admissao_requirido")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [DataType(DataType.Date)]
        [DisplayName(@"Data Admissão")]
        public DateTime? DataAdmissao { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "data_nascimento_requirido")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [DataType(DataType.Date)]
        [DisplayName(@"Data Nascimento")]
        public DateTime? DataNascimento { get; set; }

        [DisplayName(@"Recebeu Treinamento?")]
        public bool RecebeuTreinamento { get; set; }

        [DisplayName(@"Recebeu Advertência?")]
        public bool RecebeuAdvertencia { get; set; }

        [StringLength(500, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_500")]
        [MaxLength(500, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_500")]
        [DataType(DataType.MultilineText)]
        [DisplayName(@"Motivo Advertência")]
        public string MotivoAdvertencia { get; set; }

        [DisplayName(@"Ativo?")]
        public bool Ativo { get; set; }

        [StringLength(500, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_500")]
        [MaxLength(500, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_500")]
        [DataType(DataType.MultilineText)]
        [DisplayName(@"Observações")]
        public string Obs { get; set; }

        [DisplayName(@"Foto")]
        public byte[] Foto { get; set; }

        [DisplayName(@"Assinatura")]
        public byte[] Assinatura { get; set; }

        [DisplayName("Biometria")]
        public string Biometria { get; set; }

        [ScaffoldColumn(false)]
        public bool? Importado { get; set; }

        [NotMapped]
        public string NomeSetor
        {
            get { return _nomeSetor; }
            set { _nomeSetor = value != null ? value.ToUpper() : null; }
        }

        [ScaffoldColumn(false)]
        public DateTime? DataCad { get; set; }
        public ICollection<EpiColaboradorModel> Epis { get; set; }
        //[DisplayName("Unidade de Negocio")]        
        [DisplayName("Nome Fantasia")] //Label Unidade de Negocio 20/03/2017
        public virtual LbcModel Lbc { get; set; }
        //[DisplayName("Unidade de Negócio")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "unidade_negocio_requirido")]
        [DisplayName("Nome Fantasia")] //Label Unidade de Negocio 20/03/2017
        public int? LbcId { get; set; }
        [DisplayName("Função")]
        public int? FuncaoId { get; set; }
        [DisplayName("Função")]
        public virtual FuncaoModel Funcao { get; set; }
        public ICollection<UniformeColaboradorModel> Uniformes { get; set; }

        public ColaboradorModel()
        {
            Epis = new List<EpiColaboradorModel>();
            Uniformes = new List<UniformeColaboradorModel>();
        }
    }

    public class ColaboradorImportacaoDuplicadoModel
    {
        [Key]
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string NumRegEmpresa { get; set; }
        public Genero? Genero { get; set; }
        public DateTime? DataAdmissao { get; set; }
        public DateTime? DataNascimento { get; set; }
        public bool? RecebeuTreinamento { get; set; }
        public bool? RecebeuAdvertencia { get; set; }
        public string MotivoAdvertencia { get; set; }
        public string Setor { get; set; }
        public string Obs { get; set; }
        public DateTime DataHora { get; set; }
    }
}