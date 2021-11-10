using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TitansMVC.Properties;

namespace TitansMVC.Models
{
    public class UfModel
    {
        private string _sigla;
        private string _nome;

        [Key]
        public int Id { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "campo_obrig")]
        //[StringLength(2, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_2")]
        [MaxLength(2, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_2")]
        public string Sigla {
            get { return _sigla; }
            set { _sigla = value != null ? value.ToUpper() : null; }
        }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "campo_obrig")]
        //[StringLength(200, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_200")]
        [MaxLength(200, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_200")]
        [DisplayName("Nome")]
        public string Nome {
            get { return _nome; }
            set { _nome = value != null ? value.ToUpper() : null; }
        }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "campo_obrig")]
        //[StringLength(2, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_2")]
        [MaxLength(2, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_2")]
        [DisplayName("Cód. IBGE")]
        public string CodIbge { get; set; }
        //[MaxLength(500, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_500")]
        [DataType(DataType.MultilineText)]
        [DisplayName("Observações")]
        public string Obs { get; set; }
        [ScaffoldColumn(false)]
        public DateTime? DataCad { get; set; }
    }
}