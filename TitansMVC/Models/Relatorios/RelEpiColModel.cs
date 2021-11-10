using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TitansMVC.Models.Relatorios
{
    public class RelEpiColModel
    {
        public int id_unidade_negocio { get; set; }
        public int id_colaborador { get; set; }
        public int id_colaborador_epi { get; set; }
        public int id_centro_custo { get; set; }
        [DisplayName("Colaborador")]
        public string nome_colaborador { get; set; }
        [DisplayName("EPI")]
        public string nome_epi { get; set; }
        [DisplayName("Qtde")]
        public int qtde_epis { get; set; }
        [DisplayName("Dt. Entrega")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? data_entrega { get; set; }
        [DisplayName("Val. Dias")]
        public int validade_dias { get; set; }
        [DisplayName("Dt. Vencto")]
        public DateTime? data_vencimento { get; set; }
        [DisplayName("Assinatura")]
        public byte[] assinatura_colaborador { get; set; }
        [DisplayName("CA")]
        public string epi_ca { get; set; }
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