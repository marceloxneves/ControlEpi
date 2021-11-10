using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using TitansMVC.Properties;

namespace TitansMVC.Models
{
    public class UsuarioModel
    {
        [Key]
        public string Id { get; set; }
        [DisplayName(@"Empresa")]
        public int? IdEmpresa { get; set; }
        public virtual EmpresaModel Empresa { get; set; }
        [DisplayName(@"Usuário")]
        public string Username { get; set; }
        [DisplayName(@"E-mail")]
        public string Email { get; set; }
        [DisplayName(@"E-mail Confirmado")]
        public bool EmailConfirmed { get; set; }
        [DisplayName(@"Ativo?")]
        public bool Ativo { get; set; }
        [DataType(DataType.MultilineText)]
        //[StringLength(500, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_500")]
        [MaxLength(500, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_500")]
        [DisplayName(@"Observações")]
        public string Obs { get; set; }
        public ICollection<PermissaoUsuarioModel> Permissoes { get; set; }
        //public string PasswordHash { get; set; }
        //public string SecurityStamp { get; set; }
        //public string PhoneNumber { get; set; }
        //public bool PhoneNumberConfirmed { get; set; }
        //public bool TwoFactorEnabled { get; set; }
        //public DateTime LockoutEndDateUtc { get; set; }
        //public bool LockoutEnabled { get; set; }
        //public int AccessFailedCount { get; set; }
        [NotMapped]
        public string Password { get; set; }

        public UsuarioModel()
        {
            Permissoes = new List<PermissaoUsuarioModel>();
        }
    }
}