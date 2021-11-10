using System;

namespace TitansMVC.Models
{
    public class FichaUniformeModel
    {
        public int Id { get; set; }
        public int IdUniforme { get; set; }
        public virtual UniformeModel Uniforme { get; set; }
        public string UniformeNome { get; set; }
        public DateTime? RevisadoEm { get; set; }
        public string DescrDetEquip { get; set; }        
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