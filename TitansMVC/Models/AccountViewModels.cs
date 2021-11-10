using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TitansMVC.Properties;

namespace TitansMVC.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "email_requirido")]
        [Display(Name = "E-mail")]
        [EmailAddress(ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "email_valido")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required(ErrorMessage = " ")]
        public string Provider { get; set; }

        [Required(ErrorMessage= " ")]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "email_requirido")]
        [Display(Name = "E-mail")]
        [EmailAddress(ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "email_valido")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "email_requirido")]
        [Display(Name = "E-mail")]
        [EmailAddress(ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "email_valido")]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "senha_requirido")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        private string _razao;
        private string _email;

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "razao_social_requirido")]
        [Display(Name = "Razão Social")]
        public string RazaoSocialEmpr {
            get { return _razao; }
            set { _razao = value != null ? value.ToUpper() : null; }
        }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "cnpj_requirido")]
        [Display(Name = "CNPJ")]
        public string CnpjEmpr { get; set; }

        [Display(Name = "Telefone para Contato")]
        public string TelContato { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "email_requirido")]
        [EmailAddress(ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "email_valido")]
        [Display(Name = "E-mail")]
        public string Email {
            get { return _email; }
            set { _email = value != null ? value.ToLower() : null; }
        }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "senha_requirido")]
        //123456*[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Password { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "confirmar_senha_requirido")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar senha")]
        [Compare("Password", ErrorMessage = "A Senha confirmada não é igual à senha informada.")]
        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "email_requirido")]
        [EmailAddress(ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "email_valido")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "senha_requirido")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Password { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "confirmar_senha_requirido")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar senha")]
        [Compare("Password", ErrorMessage = "A Senha confirmada não é igual à senha informada.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "email_requirido")]
        [EmailAddress(ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "email_valido")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }
    }
}
