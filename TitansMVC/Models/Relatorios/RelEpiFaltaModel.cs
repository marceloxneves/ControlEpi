using System.ComponentModel;

namespace TitansMVC.Models.Relatorios
{
    public class RelEpiFaltaModel
    {
        [DisplayName("ID EPI")]
        public int id { get; set; }
        [DisplayName("EPI")]
        public string nome_epi { get; set; }
        [DisplayName("Marca EPI")]
        public string marca { get; set; }
        [DisplayName("CA EPI")]
        public string ca { get; set; }
        [DisplayName("Cod EPI")]
        public string cod_interno { get; set; }
        [DisplayName("Quant em estoque")]
        public decimal qtde_estoque { get; set; }
        [DisplayName("Quant Minima")]
        public decimal qtde_min { get; set; }
    }
}
