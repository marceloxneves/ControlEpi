using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using TitansMVC.Properties;

namespace TitansMVC.Models
{
    public class IndexViewModel
    {
        public bool HasPassword { get; set; }
        public IList<UserLoginInfo> Logins { get; set; }
        public string PhoneNumber { get; set; }
        public bool TwoFactor { get; set; }
        public bool BrowserRemembered { get; set; }
    }

    public class ManageLoginsViewModel
    {
        public IList<UserLoginInfo> CurrentLogins { get; set; }
        public IList<AuthenticationDescription> OtherLogins { get; set; }
    }

    public class FactorViewModel
    {
        public string Purpose { get; set; }
    }

    public class SetPasswordViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "nova_senha_requirido")]
        //123456*[StringLength(100, ErrorMessage = "mínimo 6 caracteres", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nova senha")]
        public string NewPassword { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "confirmar_senha_requirido")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar nova senha")]
        [Compare("NewPassword", ErrorMessage = "A Senha confirmada não é igual à senha informada.")]
        public string ConfirmPassword { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "senha_atual_requirido")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha atual")]
        public string OldPassword { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "nova_senha_requirido")]
        //1232456*[StringLength(100, ErrorMessage = "mínimo 6 caracteres", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nova senha")]
        public string NewPassword { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "confirmar_senha_requirido")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar nova senha")]
        [Compare("NewPassword", ErrorMessage = "A Senha confirmada não é igual à senha informada.")]
        public string ConfirmPassword { get; set; }
    }

    public class AddPhoneNumberViewModel
    {
        [Required(ErrorMessage = " ")]
        [Phone]
        [Display(Name = "Phone Number")]
        public string Number { get; set; }
    }

    public class VerifyPhoneNumberViewModel
    {
        [Required(ErrorMessage = " ")]
        [Display(Name = "Code")]
        public string Code { get; set; }

        [Required(ErrorMessage = " ")]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
    }

    public class ConfigureTwoFactorViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
    }
}