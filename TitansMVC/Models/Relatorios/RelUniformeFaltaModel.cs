using System.ComponentModel;

namespace TitansMVC.Models.Relatorios
{
    public class RelUniformeFaltaModel
    {
        [DisplayName("ID Uniforme")]
        public int id { get; set; }
        [DisplayName("Uniforme")]
        public string nome_uniforme { get; set; }
        [DisplayName("Marca Uniforme")]
        public string marca { get; set; }
        [DisplayName("Cod Uniforme")]
        public string cod_interno { get; set; }
        [DisplayName("Quant em estoque")]
        public decimal qtde_estoque { get; set; }
        [DisplayName("Quant Minima")]
        public decimal qtde_min { get; set; }
    }
}
