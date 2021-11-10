using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TitansMVC.Properties;

namespace TitansMVC.Models
{
    public class MunicipioModel
    {
        private string _nome;

        [Key]
        public int Id { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "campo_obrig")]
        //[StringLength(200, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_200")]
        [MaxLength(200, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_200")]
        [DisplayName("Nome")]
        public string Nome {
            get { return _nome; }
            set { _nome = value != null ? value.ToUpper() : null; }
        }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "campo_obrig")]
        //[StringLength(7, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_7")]
        [MaxLength(7, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_7")]
        [DisplayName("Código IBGE")]
        public string CodIbge { get; set; }
        //public string UfSigla { get; set; }
        //public string UfCodIbge { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "campo_obrig")]
        [DisplayName("Uf")]
        public int UfId { get; set; }
        public UfModel Uf { get; set; }
        //[StringLength(500, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_500")]
        [MaxLength(500, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_500")]
        [DataType(DataType.MultilineText)]
        [DisplayName("Observações")]
        public string Obs { get; set; }
        [ScaffoldColumn(false)]
        public DateTime? DataCad { get; set; }
    }
}