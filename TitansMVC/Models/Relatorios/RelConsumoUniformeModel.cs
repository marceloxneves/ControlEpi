using System.ComponentModel;

namespace TitansMVC.Models.Relatorios
{
    public class RelConsumoUniformeModel
    {
        public int id_unidade_negocio { get; set; }
        public int id_colaborador { get; set; }
        public int id_colaborador_uniforme { get; set; }
        public int id_centro_custo { get; set; }        
        [DisplayName("Colaborador")]
        public string nome_colaborador { get; set; }     
        [DisplayName("Uniforme")]
        public string nome_uniforme { get; set; }
        [DisplayName("Qtde Entregue")]
        public int qtde_uniformes { get; set; }   
        [DisplayName("Custo")]
        public decimal uniforme_custo { get; set; }
        [DisplayName("Dt. Baixa")]
        public bool baixado { get; set; }
        [DisplayName("Centro Custo")]
        public string nome_ccusto { get; set; }
        [DisplayName("Custo Total")]
        public decimal custo_total
        {
            get{ return qtde_uniformes * uniforme_custo; }
        } 
        public string nome_setor { get; set; }
        public string nome_funcao { get; set; }
        [DisplayName("Nome Fantasia")]
        public string nome_fantasia { get; set; }
        [DisplayName("Razão Social")]
        public string razao_social { get; set; }
        [DisplayName("Justificativa")]
        public string justificativa { get; set; }
        [DisplayName("Justificativa Baixa")]
        public string justificativa_baixa { get; set; }
    }
}