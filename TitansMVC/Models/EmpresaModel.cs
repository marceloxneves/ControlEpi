using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TitansMVC.Properties;

namespace TitansMVC.Models
{
    public class EmpresaModel
    {
        private string _razao;
        private string _fantasia;
        private string _cnpj;
        private string _url;
        private string _endEndereco;
        private string _complemento;
        private string _bairro;

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessageResourceType = typeof (Resources), ErrorMessage = null, ErrorMessageResourceName = "campo_obrig")]
        [StringLength(200, ErrorMessageResourceType = typeof (Resources), ErrorMessage = null, ErrorMessageResourceName = "max_200")]
        [MaxLength(200, ErrorMessageResourceType = typeof (Resources), ErrorMessage = null, ErrorMessageResourceName = "max_200")]
        [DisplayName(@"Razão Social")]
        public string Razao
        {
            get { return _razao;  } 
            set { _razao = value != null ? value.ToUpper() : null; }
        }

        [StringLength(50, ErrorMessageResourceType = typeof (Resources), ErrorMessage = null, ErrorMessageResourceName = "max_50")]
        [MaxLength(50, ErrorMessageResourceType = typeof (Resources), ErrorMessage = null, ErrorMessageResourceName = "max_50")]
        [DisplayName(@"Fantasia")]
        public string Fantasia
        {
            get { return _fantasia;  } 
            set { _fantasia = value != null ? value.ToUpper() : null; }
        }

        [StringLength(20, ErrorMessageResourceType = typeof (Resources), ErrorMessage = null, ErrorMessageResourceName = "max_20")]
        [MaxLength(20, ErrorMessageResourceType = typeof (Resources), ErrorMessage = null, ErrorMessageResourceName = "max_20")]
        [DisplayName(@"CNPJ")]
        public string Cnpj
        {
            get { return _cnpj; }
            set { _cnpj = value != null ? new string(value.Where(Char.IsDigit).ToArray()) : null; }
        }
        [MaxLength(20, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_20")]
        [DisplayName(@"Inscr. Est.")]
        public string InscrEst { get; set; }
        //[StringLength(20, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_20")]
        [MaxLength(20, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_20")]
        [DisplayName(@"Inscr. Mun.")]
        public string InscrMun { get; set; }
        //[StringLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_20")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_50")]
        [DisplayName(@"CNAE")]
        public string Cnae { get; set; }

        //[StringLength(200, ErrorMessageResourceType = typeof (Resources), ErrorMessage = null, ErrorMessageResourceName = "max_200")]
        [MaxLength(200, ErrorMessageResourceType = typeof (Resources), ErrorMessage = null, ErrorMessageResourceName = "max_200")]
        [DisplayName(@"URL")]
        public string Url
        {
            get { return _url; } 
            set { _url = value != null ? value.ToLower() : null; }
        }
        //[StringLength(20, ErrorMessage = "máximo 20 caracteres")]
        //[MaxLength(20, ErrorMessage = "Máximo 20 caracteres.")]
        //[DisplayName("Logradouro")]
        //[ScaffoldColumn(false)]
        //public string EndLogradouro { get; set; }
        //[StringLength(200, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_200")]
        [MaxLength(200, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_200")]
        [DisplayName(@"Endereço")]
        public string EndEndereco { get { return _endEndereco; } set
        {
            _endEndereco = value != null ? value.ToUpper() : null;
        } }
        //[StringLength(20, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_20")]
        [MaxLength(20, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_20")]
        [DisplayName(@"Número")]
        public string EndNumero { get; set; }
       // [StringLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_50")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_50")]
        [DisplayName(@"Complemento")]
        public string EndComplemento { 
            get { return _complemento; } 
            set { _complemento = value != null ? value.ToUpper() : null; } 
        }
        //[StringLength(200, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_200")]
        [MaxLength(200, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_200")]
        [DisplayName(@"Bairro")]
        public string EndBairro { get { return _bairro; } set { _bairro = value != null ? value.ToUpper() : null; } }
        //[StringLength(9, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_9")]
        [MaxLength(9, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_9")]
        [DisplayName(@"Cep")]
        public string EndCep { get; set; }
        [DisplayName(@"Município")]
        public int? MunicipioId { get; set; }
        public virtual MunicipioModel Municipio { get; set; }
        //[StringLength(2, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_2")]
        [MaxLength(2, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_2")]
        [DisplayName(@"UF")]
        public string SiglaUf { get; set; }        
        [ReadOnly(true)]
        [DisplayName(@"Próx. Num. OS")]
        public int? ProxNumOs { get; set; }
        [DisplayName(@"Matriz?")]
        public bool Matriz { get; set; }
        [DisplayName(@"Ativo?")]
        public bool Ativo { get; set; }
        public ICollection<TelefoneEmpresaModel> Telefones { get; set; }
        public ICollection<EmailEmpresaModel> Emails { get; set; }
        //[StringLength(500, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_500")]
        [MaxLength(500, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_500")]
        [DataType(DataType.MultilineText)]
        [DisplayName(@"Observações")]
        public string Obs { get; set; }
        [DisplayName(@"Logomarca")]
        public byte[] Logomarca { get; set; }
        [DisplayName("Possui Biometria?")]
        public bool PossuiBiometria { get; set; }
        [DisplayName("Possui Assinatura?")]
        public bool PossuiAssinatura { get; set; }
        //[ScaffoldColumn(false)]
        //public DateTime? DataCad { get; set; }

        public EmpresaModel()
        {
            Telefones = new List<TelefoneEmpresaModel>();
            Emails = new List<EmailEmpresaModel>();
        }
    }
}