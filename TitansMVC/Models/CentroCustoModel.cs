using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Microsoft.Owin.Security;
using TitansMVC.Properties;

namespace TitansMVC.Models
{
    public class CentroCustoModel
    {
        private string _nome;

        [Key]
        public int Id { get; set; }
        [DisplayName(@"Empresa")]
        public int? IdEmpresa { get; set; }
        public virtual EmpresaModel Empresa { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "campo_obrig")]
        [StringLength(200,  ErrorMessageResourceType = typeof (Resources), ErrorMessage = null, ErrorMessageResourceName = "max_200")]
        [MaxLength(200, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_200")]
        [DisplayName(@"Nome")]
        public string Nome {
            get { return _nome; } 
            set { _nome = value != null ? value.ToUpper() : null; } 
        }

        [DisplayName(@"Ativo?")]
        public bool Ativo { get; set; }
        //[DisplayName(@"Unidade de Negócio")]
        [DisplayName(@"Nome Fantasia")] //Label Unidade de Negocio 20/03/2017
        public int? LbcId { get; set; }
        //[DisplayName("Unidade de Negocio")]
        [DisplayName("Nome Fantasia")] //Label Unidade de Negocio 20/03/2017
        public LbcModel Lbc { get; set; }
        [DataType(DataType.MultilineText)]
        [StringLength(500, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_500")]
        [MaxLength(500, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_500")]
        [DisplayName(@"Observações")]
        public string Obs { get; set; }        
        [ScaffoldColumn(false)]
        public DateTime? DataCad { get; set; }
    }
}