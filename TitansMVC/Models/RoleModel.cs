using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TitansMVC.Properties;

namespace TitansMVC.Models
{
    public class RoleModel
    {
        private string _nome;
        private string _descricao;

        [Key]
        public string Id { get; set; }
        [ReadOnly(true)]
        [DisplayName("Nome")]
        public string Nome {
            get { return _nome; }
            set { _nome = value != null ? value.ToUpper() : null; }
        }

        //[StringLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_50")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_50")]
        [DisplayName("Descrição")]
        public string Descricao {
            get { return _descricao; }
            set { _descricao = value != null ? value.ToUpper() : null; }
        }

        [ScaffoldColumn(false)]
        public bool Ativo { get; set; }
    }
}