using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TitansMVC.Models.Enums
{
    public enum Plano
    {
        [Display(Name = "Starter")]
        Starter = 131,
        [Display(Name = "Basic")]
        Basic = 190,
        [Display(Name = "Standard")]
        Standard = 467,
        [Display(Name = "Master")]
        Master = 792,
        [Display(Name = "Ultimate")]
        Ultimate = 944
    }
}