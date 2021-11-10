using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TitansMVC.Properties;

namespace TitansMVC.Models
{
    public class UniformeSetorModel
    {
        private string _nomeUniforme;

        [Key]
        // opcional, mas poderia ser utilizado
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [DisplayName("Tipo Uniforme")]
        public int? TipoUniformeId { get; set; }
        public TipoUniformeModel TipoUniforme { get; set; }
        [DisplayName("Setor")]
        public int SetorId { get; set; }        
        public virtual SetorModel Setor { get; set; }
        [DisplayName("Uniforme")]
        public int UniformeId { get; set; }
        public UniformeModel Uniforme { get; set; }        
        [MaxLength(200, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_200")]
        [DisplayName("Nome do Uniforme")]
        public string NomeUniforme {
            get { return _nomeUniforme; }
            set { _nomeUniforme = value != null ? value.ToUpper() : null; }                 
        }

        [DisplayName("Tempo Estimado de Duração(em dias)")]
        public int? ValidadeEmDias { get; set; }
        public string DescricaoSelectList
        {
            get { return string.Format("{0} - {1} - {2}", NomeUniforme.ToUpper(), Setor.Nome.ToUpper() ?? "Nome não encontrado", ValidadeEmDias); }
        }
    }
}