using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TitansMVC.Models.Enums
{
    public enum EstadoEpisConsulta
    {
        [Display(Name = "Todos")]
        Todos = 0,
        [Display(Name = "Em Uso")]
        Entregues = 1,
        [Display(Name = "Baixados")]
        Baixados = 2
    }

    public enum EstadoUniformesConsulta
    {
        [Display(Name = "Todos")]
        Todos = 0,
        [Display(Name = "Em Uso")]
        Entregues = 1,
        [Display(Name = "Baixados")]
        Baixados = 2
    }
}