using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TitansMVC.Models
{
    public class EmailConfirmacaoRegistroModel
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
    }
}