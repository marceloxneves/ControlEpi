using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TitansMVC.Models.Enums;

namespace TitansMVC.Models
{
    public class PlanoModel
    {
        private string _cnpj;
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Campo CNPJ é obrigatório")]
        [DisplayName(@"CNPJ")]
        public string Cnpj
        {
            get { return _cnpj; }
            set { _cnpj = value != null ? new string(value.Where(Char.IsDigit).ToArray()) : null; }
        }
        [Required(ErrorMessage = "Campo Plano é obrigatório")]
        [DisplayName(@"Plano")]
        public Plano? NivelPlano { get; set; }
        [Required(ErrorMessage = "Campo Validade é obrigatório")]
        [DisplayName(@"Validade")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        [DataType(DataType.Date, ErrorMessage = @"Data em formato inválido")]
        public DateTime Validade { get; set; }
        [DisplayName(@"Ultima Validação")]
        public string DataUltimaValidacao { get; set; }
        public string ValidadeCriptografada { get; set; }
        public string CnpjCriptografado { get; set; }
    }
}