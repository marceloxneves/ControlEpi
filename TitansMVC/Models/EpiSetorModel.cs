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
    public class EpiSetorModel
    {
        private string _nomeEpi;

        [Key]
        // opcional, mas poderia ser utilizado
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [DisplayName("Tipo EPI")]
        public int? TipoEpiId { get; set; }
        public TipoEpiModel TipoEpi { get; set; }
        [DisplayName("Setor")]
        public int SetorId { get; set; }
        //public string SetorNome { get; set; }
        public virtual SetorModel Setor { get; set; }
        [DisplayName("Epi")]
        public int EpiId { get; set; }
        public EpiModel Epi { get; set; }
        //[StringLength(200, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_200")]
        [MaxLength(200, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_200")]
        [DisplayName("Nome do EPI")]
        public string NomeEpi {
            get { return _nomeEpi; }
            set { _nomeEpi = value != null ? value.ToUpper() : null; }                 
        }

        [DisplayName("Tempo Estimado de Duração(em dias)")]
        public int? ValidadeEmDias { get; set; }
        public string DescricaoSelectList
        {
            get { return string.Format("{0} - {1} - {2}", NomeEpi.ToUpper(), Setor.Nome.ToUpper() ?? "Nome não encontrado", ValidadeEmDias); }
        }
    }
}