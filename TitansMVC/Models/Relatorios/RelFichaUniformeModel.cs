using System;

namespace TitansMVC.Models.Relatorios
{
    public class RelFichaUniformeModel
    {
        public int id_ficha { get; set; }
        public int id_uniforme { get; set; }
        public string uniforme_nome { get; set; }
        public DateTime? revisado_em { get; set; }
        public string descricao_det_equip { get; set; }
        public string fornecedor_fabricante { get; set; }
        public string forma_higienizacao { get; set; }
        public string finalidade_area_aplic { get; set; }
        public string obs { get; set; }
        public string aprovado_por { get; set; }
        public string registro { get; set; }
        public string area { get; set; }
        public string pecas_reposicao { get; set; }
        public string num_identificacao { get; set; }
        public byte[] foto { get; set; }
    }
}