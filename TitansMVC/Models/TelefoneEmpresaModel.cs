using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TitansMVC.Properties;

namespace TitansMVC.Models
{
    public class TelefoneEmpresaModel
    {
        private string _descricao;

        [Key]
        public int Id { get; set; }
        public int IdEmpresa { get; set; }
        public EmpresaModel Empresa { get; set; }
        //[StringLength(20, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_20")]
        [MaxLength(20, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_20")]
        [DisplayName("Número")]
        public string Numero { get; set; }
        //[StringLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_50")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_50")]
        [DisplayName("Descrição")]
        public string Descricao {
            get { return _descricao; } 
            set { _descricao = value != null ? value.ToUpper() : null; } 
        }

        //[StringLength(20, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_20")]
        [MaxLength(20, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_20")]
        [DisplayName("Ramal")]
        public string Ramal { get; set; }
        [ScaffoldColumn(false)]
        public DateTime? DataCad { get; set; }
    }
}