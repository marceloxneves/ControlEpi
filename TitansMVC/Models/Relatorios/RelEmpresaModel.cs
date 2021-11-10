using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TitansMVC.Properties;

namespace TitansMVC.Models.Relatorios
{
    public class RelEmpresaModel
    {
        public int id { get; set; }
        [DisplayName("Razão Social")]
        public string razao_social { get; set; }
        [DisplayName("Fantasia")]
        public string fantasia { get; set; }
        [DisplayName("CNPJ")]
        public string cnpj { get; set; }
        [DisplayName("Inscr. Est.")]
        public string inscr_est { get; set; }
        [DisplayName("Inscr. Mun.")]
        public string inscr_mun { get; set; }
        [DisplayName("CNAE")]
        public string cnae { get; set; }
        [DisplayName("URL")]
        public string url { get; set; }
        [DisplayName("Endereço")]
        public string endereco { get; set; }
        public string telefone { get; set; }
        public string email { get; set; }
        [DisplayName("Observações")]
        public string obs { get; set; }
        [DisplayName("Logomarca")]
        public byte[] logomarca { get; set; }
    }
}