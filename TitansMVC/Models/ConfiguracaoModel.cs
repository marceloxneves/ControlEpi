using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TitansMVC.Properties;

namespace TitansMVC.Models
{
    public class ConfiguracaoModel
    {
        [Key]
        public int Id { get; set; }
        [DisplayName(@"Empresa")]
        public int? IdEmpresa { get; set; }
        public virtual EmpresaModel Empresa { get; set; }
        [DisplayName("Avisar Venc. CA com antecedência?")]
        public bool AvisarVencCaComAntec { get; set; }
        [DisplayName("Qtde Dias Venc. CA")]
        public int? QtdeDiasAvisoVencCa { get; set; }
        [DisplayName("Avisar Venc. EPI com antecedência?")]
        public bool AvisarVencEpiComAntec { get; set; }
        [DisplayName("Qtde Dias Venc. EPI")]
        public int? QtdeDiasAvisoVencEpi { get; set; }
        [DisplayName("Avisar após Venc. CA?")]
        public bool AvisarAposVencCa { get; set; }
        [DisplayName("Avisar após Venc. EPI?")]
        public bool AvisarAposVencEpi { get; set; }
        [DisplayName("Ativar Bloqueio de Recebimento Único por Tipo Epi?")]
        public bool BloquearPorTipoEpiUnico { get; set; }
        [DataType(DataType.MultilineText)]
        [StringLength(500, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_500")]
        [MaxLength(500, ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, ErrorMessageResourceName = "max_500")]
        [DisplayName("Observações")]
        public string Obs { get; set; }
        [DisplayName("Avisar Venc. Uniforme com antecedência?")]
        public bool AvisarVencUniformeComAntec { get; set; }
        [DisplayName("Qtde Dias Venc. Uniforme")]
        public int? QtdeDiasAvisoVencUniforme { get; set; }
        [DisplayName("Avisar após Venc. Uniforme?")]
        public bool AvisarAposVencUniforme { get; set; }
        [DisplayName("Ativar Bloqueio de Recebimento Único por Tipo Uniforme?")]
        public bool BloquearPorTipoUniformeUnico { get; set; }

    }
}