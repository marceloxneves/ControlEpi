using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TitansMVC.Properties;

namespace TitansMVC.Models
{
    public class LbcModel
    {
        private string _nome;
        private string _fantasia;
        private string _cnpj;
        private string _url;
        private string _endEndereco;
        private string _complemento;
        private string _bairro;
        

        [Key]
        public int Id { get; set; }
        [DisplayName(@"Empresa")]
        public int? IdEmpresa { get; set; }
        public virtual EmpresaModel Empresa { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "campo_obrig")]
        //[StringLength(200, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_200")]
        [MaxLength(200, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_200")]
        [DisplayName(@"Razão Social")]
        public string Nome {
            get { return _nome; }
            set { _nome = value != null ? value.ToUpper() : null; }
        }

        [DisplayName("Ativo?")]
        public bool Ativo { get; set; }
        [DataType(DataType.MultilineText)]
        [StringLength(500, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_500")]
        [MaxLength(500, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_500")]
        [DisplayName("Observações")]
        public string Obs { get; set; }
        public virtual ICollection<CentroCustoModel> CentrosDeCustos { get; set; }
        [ScaffoldColumn(false)]
        public DateTime? DataCad { get; set; }

        //[StringLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_50")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_50")]
        [DisplayName(@"Nome Fantasia")]
        public string Fantasia
        {
            get { return _fantasia; }
            set { _fantasia = value != null ? value.ToUpper() : null; }
        }

        //[StringLength(20, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_20")]
        [MaxLength(20, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_20")]
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

        //[StringLength(200, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_200")]
        [MaxLength(200, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_200")]
        [DisplayName(@"URL")]
        public string Url
        {
            get { return _url; }
            set { _url = value != null ? value.ToLower() : null; }
        }

        //[StringLength(200, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_200")]
        [MaxLength(200, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_200")]
        [DisplayName(@"Endereço")]
        public string EndEndereco
        {
            get { return _endEndereco; }
            set
            {
                _endEndereco = value != null ? value.ToUpper() : null;
            }
        }
        //[StringLength(20, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_20")]
        [MaxLength(20, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_20")]
        [DisplayName(@"Número")]
        public string EndNumero { get; set; }
        //[StringLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_50")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_50")]
        [DisplayName(@"Complemento")]
        public string EndComplemento
        {
            get { return _complemento; }
            set { _complemento = value != null ? value.ToUpper() : null; }
        }
        //[StringLength(200, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_200")]
        [MaxLength(200, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_200")]
        [DisplayName(@"Bairro")]
        public string EndBairro { get { return _bairro; } set { _bairro = value != null ? value.ToUpper() : null; } }
        [StringLength(9, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_9")]
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
        [DisplayName(@"Logomarca")]
        public byte[] Logomarca { get; set; }
    }
}




