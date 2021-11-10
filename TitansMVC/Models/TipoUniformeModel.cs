using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TitansMVC.Properties;

namespace TitansMVC.Models
{
    public class TipoUniformeModel
    {
        private string _nome;

        [Key]
        public int Id { get; set; }
        [DisplayName(@"Empresa")]
        public int? IdEmpresa { get; set; }
        public virtual EmpresaModel Empresa { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "campo_obrig")]       
        [MaxLength(200, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_200")]
        [DisplayName("Nome")]
        public string Nome { 
            get { return _nome; }
            set { _nome = value != null ? value.ToUpper() : null; }
        }

        [DataType(DataType.MultilineText)]       
        [MaxLength(500, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_500")]
        [DisplayName("Observações")]
        public string Obs { get; set; }
        [DisplayName("Ativo?")]
        public bool Ativo { get; set; }
    }
}