using System.ComponentModel;

namespace TitansMVC.Models.Relatorios
{
    public class RelUniformeFaltaFilter
    {
        [DisplayName("Uniforme")]
        public int? UniformeId { get; set; }
        [DisplayName("Nome Uniforme")]
        public string DescrUniforme { get; set; }
        [DisplayName("Marca Uniforme")]
        public string Marca { get; set; }
        [DisplayName("Faricante Uniforme")]
        public string FabricanteId { get; set; }
    }
}
