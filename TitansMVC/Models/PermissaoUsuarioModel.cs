using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TitansMVC.Properties;

namespace TitansMVC.Models
{
    public class PermissaoUsuarioModel
    {
        private string _descricaoPermissao;

        [Key]
        public int Id { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "campo_obrig")]
        [DisplayName("Usuário")]
        public string IdUsuario { get; set; }

        public virtual UsuarioModel Usuario { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "campo_obrig")]
        [DisplayName("Permissão")]
        public string IdPermissao { get; set; }
        [StringLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_50")]
        //[MaxLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_50")]
        [DataType(DataType.MultilineText)]
        [DisplayName("Descrição")]
        public string DescricaoPermissao {
            get { return _descricaoPermissao; }
            set { _descricaoPermissao = value != null ? value.ToUpper() : null; }
        }
    }
}