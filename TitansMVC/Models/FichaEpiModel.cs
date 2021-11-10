using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TitansMVC.Models
{
    public class FichaEpiModel
    {
        public int Id { get; set; }
        public int IdEpi { get; set; }
        public virtual EpiModel Epi { get; set; }
        public string EpiNome { get; set; }
        public string EpiCa { get; set; }
        public DateTime? EpiCaValidade { get; set; }
        public DateTime? RevisadoEm { get; set; }
        public string DescrDetEquip { get; set; }
        //public byte[] Foto { get; set; }
        public string FornFabr { get; set; }
        public string FormaHigienizacao { get; set; }
        public string FinalidadeAreaAplic { get; set; }
        public string Obs { get; set; }
        public string AprovadoPor { get; set; }
        public string Registro { get; set; }
        public string Area { get; set; }
        public string PecasReposicao { get; set; }
        public string NumIdentificacao { get; set; }
    }
}