using System.ComponentModel;

namespace TitansMVC.Models.Relatorios
{
    public class RelEpiFaltaFilter
    {
        [DisplayName("EPI")]
        public int? EpiId { get; set; }
        [DisplayName("Nome EPI")]
        public string DescrEpi { get; set; }
        [DisplayName("Marca EPI")]
        public string Marca { get; set; }
        [DisplayName("Faricante EPI")]
        public string FabricanteId { get; set; }
    }
}
