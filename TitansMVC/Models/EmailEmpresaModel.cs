using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TitansMVC.Properties;

namespace TitansMVC.Models
{
    public class EmailEmpresaModel
    {
        private string _descricao;
        private string _email;

        [Key]
        public int Id { get; set; }
        [DisplayName(@"Empresa")]
        public int IdEmpresa { get; set; }
        public EmpresaModel Empresa { get; set; }

        [Required(ErrorMessageResourceType = typeof (Resources), ErrorMessage = null, ErrorMessageResourceName = "campo_obrig")]
        [StringLength(50, ErrorMessageResourceType = typeof (Resources), ErrorMessage = null, ErrorMessageResourceName = "max_50")]
        [MaxLength(50, ErrorMessageResourceType = typeof (Resources), ErrorMessage = null, ErrorMessageResourceName = "max_50")]
        [DisplayName(@"Descrição")]
        public string Descricao
        {
            get { return _descricao; } 
            set { _descricao = value != null ? value.ToUpper() : null; }
        }

        [Required(ErrorMessageResourceType = typeof (Resources), ErrorMessage = null, ErrorMessageResourceName = "campo_obrig")]
        [StringLength(200, ErrorMessageResourceType = typeof (Resources), ErrorMessage = null, ErrorMessageResourceName = "max_200")]
        [MaxLength(200, ErrorMessageResourceType = typeof (Resources), ErrorMessage = null, ErrorMessageResourceName = "max_200")]
        [DisplayName(@"E-mail")]
        public string Email
        {
            get { return _email; }
            set { _email = value != null ? value.ToLower() : null; }
        }

        [ScaffoldColumn(false)]
        public DateTime? DataCad { get; set; }
    }
}