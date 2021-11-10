using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TitansMVC.Models.Relatorios
{
    public class RelUniformeColModel
    {
        public int id_unidade_negocio { get; set; }
        public int id_colaborador { get; set; }
        public int id_colaborador_uniforme { get; set; }
        public int id_centro_custo { get; set; }
        [DisplayName("Colaborador")]
        public string nome_colaborador { get; set; }
        [DisplayName("Uniforme")]
        public string nome_uniforme { get; set; }
        [DisplayName("Qtde")]
        public int qtde_uniformes { get; set; }
        [DisplayName("Dt. Entrega")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? data_entrega { get; set; }
        [DisplayName("Val. Dias")]
        public int validade_dias { get; set; }
        [DisplayName("Dt. Vencto")]
        public DateTime? data_vencimento { get; set; }
        [DisplayName("Assinatura")]
        public byte[] assinatura_colaborador { get; set; }
        [DisplayName("Dt. Baixa")]
        public DateTime? data_hora_baixa { get; set; }
        public bool baixado { get; set; }
        [DisplayName("Centro Custo")]
        public string nome_ccusto { get; set; }
        [DisplayName("Biometria")]
        public string possui_biometria { get; set; } //se tipo_validacao = 1 true
        [DisplayName("Biometria")]
        public bool tipo_validacao { get; set; } //se entrega de foi por biometria => true senao => false
        [DisplayName("Setor")]
        public string nome_setor { get; set; }
        [DisplayName("Função")]
        public string funcao { get; set; }
        [DisplayName("Nome Fantasia")]
        public string nome_fantasia { get; set; }
        [DisplayName("Razão Social")]
        public string razao_social { get; set; }
        [DisplayName("Logomarca")]
        public byte[] logomarca_lbc { get; set; }
    }
}